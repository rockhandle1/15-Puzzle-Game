using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel.Design;
using System.Collections.Generic;

namespace _15Puzzle
{



    class Sprite : Transform
    {
        


        Thread test = new Thread(Worker);
        Texture2D texture;
        int i;
        float selection;
        int Grid = 4;
        public Sprite(Texture2D newTexture)
        {
            texture = newTexture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int calc = 1026 / Grid;
            int[] num = { calc };
            //Images are 1026x1026
            for (float e = 0f; e <= (float)Grid - 1f; e++)
            {
                for (int i = 0; i <= 1026; i = i + (int)calc)
                {
                    selection = e / Grid * 1026f;
                    spriteBatch.Draw(
                    texture,
                    new Vector2(selection, i),
                    new Rectangle((int)selection, i, calc, calc),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    scale,  //Scale
                    SpriteEffects.None,
                    0f
                    );
                }
            }
/*            for (int i = 0; i <= 1020; i = i + 68)
            {
                Selector(i / 1020 * 15);
                spriteBatch.Draw(
                texture,
                new Vector2(i, i),
                new Rectangle(i, i, 68, 68),
                Color.White,
                0f,
                Vector2.Zero,
                scale,  //Scale
                SpriteEffects.None,
                0f
                );
                //Debug.WriteLine(i);
            }*/
        }
        static void Worker(Object spriteBatch)
        {
/*            spriteBatch.Draw(
            texture,
            new Vector2(i, i),
            new Rectangle(i, i, 68, 68),
            Color.White,
            0f,
            Vector2.Zero,
            scale,  //Scale
            SpriteEffects.None,
            0f
            );
            Debug.WriteLine(i);*/
        }
    }
}
