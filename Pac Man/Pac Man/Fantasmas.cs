using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pac_Man
{
    class Fantasma
    {
        Vector2 ghostCoords;
        Texture2D Ghost;

        public Fantasma(Vector2 ghostCoords, string textureName, ContentManager Content)
        {
            this.ghostCoords = ghostCoords;
            Ghost = Content.Load<Texture2D>(textureName);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Ghost, Auxiliares.Matrix2Screen(ghostCoords), Color.White);
        }

        public void Dispose()
        {
            Ghost.Dispose();
        }


         int direcao;
        public void Update(byte[,] board, Random random)
        {

            
            direcao = random.Next(1, 5);


            switch (direcao)
            {

                case 1:
                    if(Auxiliares.canGo((int)ghostCoords.X + 1, (int)ghostCoords.Y, board))
                    {
                        ghostCoords.X++;
                    }
                    break;
                      
                case 2:
                    if(Auxiliares.canGo((int)ghostCoords.X, (int)ghostCoords.Y + 1, board))
                    {
                        ghostCoords.Y++;
                    }
                    break;

                case 3:
                    if (Auxiliares.canGo((int)ghostCoords.X - 1, (int)ghostCoords.Y, board))
                    {
                        ghostCoords.X--;
                    }
                    break;

                case 4:
                    if (Auxiliares.canGo((int)ghostCoords.X, (int)ghostCoords.Y - 1, board))
                    {
                        ghostCoords.Y--;
                    }
                    break;
            }

        }

    }
}
