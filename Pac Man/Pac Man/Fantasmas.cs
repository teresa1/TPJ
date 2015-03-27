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
        public Fantasma(Point position, string textureName, ContentManager content)
        {
            this.position = Auxiliares.Matrix2Screen(position);
            sprite = content.Load<Texture2D>(textureName);
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, Color.White);
        }

        // Update
        public void Update(GameTime gameTime, Random random, byte[,] board)
        {
            int movementSize = 1;
            float lastMachineMove = 0f;
            float lastHumanMove = 0f;

            lastHumanMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
            lastMachineMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
            direction = random.Next(1, 5);

            // Movimento de fantasmas
            // Tentativa de fazer o movimento fluído, mas tornou-se quase impossível
            while (lastMachineMove <= 2f)
            {
                lastHumanMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
                lastMachineMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (lastHumanMove >= 1f / 10f)
                {
                    lastHumanMove = 0f;
                    switch (direction)
                    {
                        // Baixo
                        case 1:
                        if (Auxiliares.CanGo((int)position.X, (int)position.Y + movementSize, board))
                        {
                            position.Y += movementSize;
                        }
                        break;
                        // Cima
                        case 2:
                        if (Auxiliares.CanGo((int)position.X, (int)position.Y - movementSize, board))
                        {
                            position.Y -= movementSize;
                        }
                        break;
                        // Direita
                        case 3:
                        if (Auxiliares.CanGo((int)position.X + movementSize, (int)position.Y, board))
                        {
                            position.X += movementSize;
                        }
                        break;
                        // Esquerda
                        case 4:
                        if (Auxiliares.CanGo((int)position.X - movementSize, (int)position.Y, board))
                        {
                            position.X -= movementSize;
                        }
                        break;

                        /* if (Auxiliares.CanGo(position.X + 1, position.Y, board))
                            {
                                if (position.X == 20 && position.Y == 9)
                                    position.X = -1;
                                else position.X++;
                         * }
                            if (position.X == 0 && position.Y == 9)
                                position.X = 20;
                            else position.X--;
                        }
                        break;
                        */
                    }
                }
            }
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
