using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.Metadata.Ecma335;

namespace _15Puzzle
{
    class Tile
    {
        public Tile(int row, int column, bool empty, Texture2D texture2D, Rectangle source)
        {
            Row = row;
            Column = column;
            Texture = texture2D;
            isEmpty = empty;
            Source = source;
        }
        public void Draw(SpriteBatch spriteBatch, Rectangle destination) {
            if (isEmpty) return;    
            spriteBatch.Draw(Texture, destination, Source, Color.White);
        }
        public int Row, Column;
        public Texture2D Texture;
        public bool isEmpty;

        public Rectangle Source;
    }

    public class Game1 : Game
    {
        int tileDirection;
        private GraphicsDeviceManager _graphics;
        int Grid = 4;
        //int randrow, randcol; 
        private SpriteBatch _spriteBatch;
        private double delta;
        Sprite sprite;
        int puzzleSelection = 0;
        int emptyTileRow = 3;
        int emptyTileCol = 3;
        Texture2D texture;

        List<List<Tile>> tiles = new List<List<Tile>>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
        void shuffle(bool _shuffle = false, bool solve = false) {
            int width_interval = texture.Width / Grid;
            int height_interval = texture.Height / Grid;
            // HashSet<(int, int)> generatedValues = new HashSet<(int, int)>();
            Random random = new Random();
            tiles.Clear();
            for (int row = 0; row < Grid; row++)
            {
                List<Tile> row_of_tiles = new List<Tile>();
                for (int col = 0; col < Grid; col++)
                {
                    bool isEmpty = row == emptyTileRow && col == emptyTileCol;
                    // do {
                    //     randrow = random.Next(0, Grid);
                    //     randcol = random.Next(0, Grid);
                    //     //Console.WriteLine(randrow);
                    // } while (generatedValues.Contains((randrow, randcol)));
                    // generatedValues.Add((randrow, randcol));
                    Rectangle source = new Rectangle(col * height_interval, row * width_interval, width_interval, height_interval);
                    // if (!solve) 
                    // {
                    //     source = new Rectangle(col * height_interval, row * width_interval, width_interval, height_interval);
                    // } else 
                    // {
                    //     source = new Rectangle(col * height_interval, row * width_interval, width_interval, height_interval);
                    // }

                    Tile newTile = new Tile(row, col, isEmpty,  texture, source);
                    row_of_tiles.Add(newTile);
                }
                tiles.Add(row_of_tiles);

            }
            KeyboardState kbstate = Keyboard.GetState();
            for (int shuffles = 0; shuffles < 200; shuffles++)
            {
                int prevValue = 0;
                do {
                    tileDirection = random.Next(-1, 2);
                } while (tileDirection == 0 && !solve || tileDirection == prevValue * -1 && !solve);
                prevValue = tileDirection;
                if (solve) {
                    tileDirection = 0;
                    shuffles = 200;
                }
                updateTiles(kbstate, random.Next(0, 2), true);
            }
            if(solve) return;
            if(isPuzzleSolved()) shuffle();
        }

        bool isPuzzleSolved()
        {
            int index = -1;
            for (int i = 0; i < Grid; i++) {
                for (int j = 0; j < Grid; j++) {
                    Tile tile = tiles[i][j];
                    index++;
                    if (tile.isEmpty) {
                        continue;
                    }
                    int sourceRow = tile.Source.Y / (tile.Texture.Height / Grid);
                    int sourceCol = tile.Source.X / (tile.Texture.Width / Grid);
                    if (sourceRow != index / Grid || sourceCol != index % Grid)
                    {
                        return false;
                    }
                }
            }
            Console.WriteLine("You Win");
            return true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1080;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Content.Load<Texture2D>("Puzzles/maxresdefault");
            shuffle();

            // TODO: use this.Content to load your game content here
        }

        bool KeyPressed;
        void inputManager(Keys key, KeyboardState kbState, int direction)
        {
            if (kbState.IsKeyDown(key) && !KeyPressed)
            {
                tileDirection = direction;
                // value = Clamp(value + direction, 0, 3);
                KeyPressed = true;
                updateTiles(kbState);
            }
            //return value;
        }

        void updateTiles(KeyboardState kbState, int shuffle = 0, bool shuffled = false)
        {
            Tile emptyTile = tiles[emptyTileRow][emptyTileCol];
            if (kbState.IsKeyDown(Keys.Up) || kbState.IsKeyDown(Keys.Down) || shuffle == 1)
            {
                int newRow = Clamp(emptyTileRow + tileDirection, 0, 3);
                Tile newEmptyTile = tiles[newRow][emptyTileCol];
                tiles[newRow][emptyTileCol] = emptyTile;
                tiles[emptyTileRow][emptyTileCol] = newEmptyTile;
                emptyTileRow = newRow;
            } else
            {
                int newCol = Clamp(emptyTileCol + tileDirection, 0, 3);
                Tile newEmptyTile = tiles[emptyTileRow][newCol];
                tiles[emptyTileRow][newCol] = emptyTile;
                tiles[emptyTileRow][emptyTileCol] = newEmptyTile;
                emptyTileCol = newCol;
            }
            if (shuffled) return;
            isPuzzleSolved();
        }

        void victory() 
        {
            
        }
        protected override void Update(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kbState.IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (kbState.GetPressedKeyCount() == 0)
            {
                KeyPressed = false;
            }
            if(kbState.IsKeyDown(Keys.Q)) shuffle(true);
            if(kbState.IsKeyDown(Keys.W)) shuffle(false, true);
            inputManager(Keys.Up, kbState, -1);
            inputManager(Keys.Down, kbState, 1);
            inputManager(Keys.Right, kbState, 1);
            inputManager(Keys.Left, kbState, -1);
            delta = gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            switch (puzzleSelection)
            {
                case 0:
                    sprite = new Sprite(Content.Load<Texture2D>("Puzzles/Badlands"));
                    break;
                case 1:
                    sprite = new Sprite(Content.Load<Texture2D>("Puzzles/maxresdefault"));
                    break;
            }
            // TODO: Add your drawing code here
            //sprite.Draw(_spriteBatch);

            for (int row = 0; row < Grid; row++)
            {
                for (int col = 0; col < Grid; col++)
                {
                    Tile t = tiles[row][col];
                    int calc = texture.Width / Grid;

                    Rectangle destination = new Rectangle(col * calc, row * calc, calc, calc);

                    t.Draw(_spriteBatch, destination);
                }

            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
