using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Pac_Man
{
    class PacMan
    {
        // Variáveis
        public Vector2 position;
        private Texture2D sprite;
        private int score;

        // Construtor
        public PacMan(Point position, string textureName, ContentManager content)
        {
            this.position = Auxiliares.Matrix2Screen(position);
            sprite = content.Load<Texture2D>(textureName);
            score = 0;
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, Color.White);
        }

        public void Move(float lastHumanMove, byte[,] board)
        {
            int movementSize = 3;

            // Simplifica a escrita da função dos botões do teclado e comando
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);

            if (lastHumanMove >= 1f / 100f)
            {
                #region Pac Man
                // Baixo
                if ((keyboardState.IsKeyDown(Keys.Down) || gamepadState.IsButtonDown(Buttons.DPadDown)))
                {
                    if (Auxiliares.CanGo((int)position.X, (int)position.Y + movementSize, board))
                    {
                        position.Y += movementSize;
                        Comer(board);
                    }
                }
                // Cima
                else if (keyboardState.IsKeyDown(Keys.Up) || gamepadState.IsButtonDown(Buttons.DPadUp))
                {
                    if (Auxiliares.CanGo((int)position.X, (int)position.Y - movementSize, board))
                    {
                        position.Y -= movementSize;
                        Comer(board);
                    }
                }
                // Esquerda
                else if (keyboardState.IsKeyDown(Keys.Left) || gamepadState.IsButtonDown(Buttons.DPadLeft))
                {
                    Vector2 vPosition = Auxiliares.Screen2Matrix(position);

                    // Warp da esquerda para a direita
                    if (vPosition.X == 0 && vPosition.Y == 9)
                        position.X = 21 * 30;

                    if (Auxiliares.CanGo((int)position.X - movementSize, (int)position.Y, board))
                    {
                        position.X -= movementSize;
                        Comer(board);
                    }
                }
                // Direita
                else if (keyboardState.IsKeyDown(Keys.Right) || gamepadState.IsButtonDown(Buttons.DPadRight))
                {
                    Vector2 vPosition = Auxiliares.Screen2Matrix(position);

                    // Warp da direita para a esquerda
                    if (vPosition.X == 20 && vPosition.Y == 9)
                        position.X = -30;

                    if (Auxiliares.CanGo((int)position.X + movementSize, (int)position.Y, board))
                    {
                        position.X += movementSize;
                        Comer(board);
                    }
                }
                #endregion
                /*
                #region Pac Woman
                // Baixo
                if ((keyState.IsKeyDown(Keys.S) || gamepadState.IsButtonDown(Buttons.DPadDown)))
                {
                    if (Auxiliares.CanGo((int)pacWoman.vPosition.X, (int)pacWoman.vPosition.Y + 1, board))
                    {
                        pacWoman.vPosition.Y++;
                        pacWoman.Comer(board);
                    }
                }
                // Cima
                else if (keyState.IsKeyDown(Keys.W) || gamepadState.IsButtonDown(Buttons.DPadUp))
                {
                    if (Auxiliares.CanGo((int)pacWoman.vPosition.X, (int)pacWoman.vPosition.Y - 1, board))
                    {
                        pacWoman.vPosition.Y--;
                        pacWoman.Comer(board);
                    }
                }
                // Esquerda
                else if (keyState.IsKeyDown(Keys.A) || gamepadState.IsButtonDown(Buttons.DPadLeft))
                {
                    // Warp da esquerda para a direita
                    if (pacWoman.vPosition.X == 0 && pacWoman.vPosition.Y == 9)
                        pacWoman.vPosition.X = 21;

                    if (Auxiliares.CanGo((int)pacWoman.vPosition.X - 1, (int)pacWoman.vPosition.Y, board))
                    {
                        pacWoman.vPosition.X--;
                        pacWoman.Comer(board);
                    }
                }
                // Direita
                else if (keyState.IsKeyDown(Keys.D) || gamepadState.IsButtonDown(Buttons.DPadRight))
                {
                    // Warp da direita para a esquerda
                    if (pacWoman.vPosition.X == 20 && pacWoman.vPosition.Y == 9)
                        pacWoman.vPosition.X = -1;

                    if (Auxiliares.CanGo((int)pacWoman.vPosition.X + 1, (int)pacWoman.vPosition.Y, board))
                    {
                        pacWoman.vPosition.X++;
                        pacWoman.Comer(board);
                    }
                }
                #endregion
 * */
            }
        }

         // Faz o Pac Man comer as pellets
        public void Comer(byte[,] board)
        {
            if (board[Auxiliares.Screen2Matrix((int)position.Y), Auxiliares.Screen2Matrix((int)position.X)] == 1)
            {
                board[Auxiliares.Screen2Matrix((int)position.Y), Auxiliares.Screen2Matrix((int)position.X)] = 2;
                score++;
            }
        }

        // Dispose
        public void Dispose()
        {
            sprite.Dispose();
        }


        /*
        private void Collide(List<Fantasma> fantasmas)
        {
          
            int x;
            int y;
            int distancia;
            foreach (Fantasma fantasma in fantasmas)
            {
                Vector2 vPosition = Auxiliares.Screen2Matrix(position);
                    x = (int)vPosition.X - (int)fantasma.Position.X;
                    y = (int)vPosition.Y - (int)fantasma.Position.Y;
                    
                    distancia = (int)Math.Sqrt(x * x + y * y);

                    
                    if(distancia < 0.5)
                    {
                        
                    }
                
            }
        }
         */
    }
}
