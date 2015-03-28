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
        public Texture2D sprite;
        private int direction;

        // Construtor
        public Fantasma(Vector2 position, string textureName, ContentManager content)
        {
            this.position = position;
            sprite = content.Load<Texture2D>(textureName);
        }

        // Update
        public void Update(byte[,] board, Random random)
        {
            // Movimento dos fantasmas
            direction = random.Next(1, 5);
            switch (direction)
            {
                // Baixo
                case 2:
                    if (Auxiliares.CanGo((int)position.X, (int)position.Y + 1, board))
                        position.Y++;
                    break;
                // Cima
                case 4:
                    if (Auxiliares.CanGo((int)position.X, (int)position.Y - 1, board))
                        position.Y--;
                    break;
                // Direita
                case 1:
                    if (Auxiliares.CanGo((int)position.X + 1, (int)position.Y, board))
                    {
                        // Warp da direita para a esquerda
                        if (position.X == 20 && position.Y == 9)
                            position.X = -1;
                        else position.X++;
                    }
                    break;
                // Esquerda
                case 3:
                    if (Auxiliares.CanGo((int)position.X - 1, (int)position.Y, board))
                    {
                        // Warp da esquerda para a direita
                        if (position.X == 0 && position.Y == 9)
                            position.X = 21;
                        else position.X--;
                    }
                    break;
            }
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, Auxiliares.Matrix2Screen(position), Color.White);
        }

        // Dispose
        public void Dispose()
        {
            sprite.Dispose();
        }

        // Métodos get/set
        public Vector2 Position
        {
            get { return position; }
        }

    
    }
}
