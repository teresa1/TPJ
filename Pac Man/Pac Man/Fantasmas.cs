using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pac_Man
{
    public class Fantasma
    {
        // Variáveis
        private Vector2 position;
        private Texture2D sprite;
        private int direction;

        // Construtor
        public Fantasma(Vector2 position, string textureName, ContentManager content)
        {
            this.position = position;
            sprite = content.Load<Texture2D>(textureName);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, Auxiliares.Matrix2Screen(position), Color.White);
        }

        public void Dispose()
        {
            sprite.Dispose();
        }

        public void Update(byte[,] board, Random random)
        {
            direction = random.Next(1, 5);

            switch (direction)
            {
                case 1:
                    if (Auxiliares.CanGo(position.X + 1, position.Y, board))
                    {
                        if (position.X == 20 && position.Y == 9)
                            position.X = -1;
                        else position.X++;
                    }
                    break;

                case 2:
                    if (Auxiliares.CanGo(position.X, position.Y + 1, board))
                    {
                        position.Y++;
                    }
                    break;

                case 3:
                    if (Auxiliares.CanGo(position.X - 1, position.Y, board))
                    {
                        if (position.X == 0 && position.Y == 9)
                            position.X = 20;
                        else position.X--;
                    }
                    break;

                case 4:
                    if (Auxiliares.CanGo(position.X, position.Y - 1, board))
                    {
                        position.Y--;
                    }
                    break;
            }

        }

        // Métodos get/set
        public Vector2 Position
        {
            get { return position; }
        }

    }
}
