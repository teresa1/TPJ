using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Net;

namespace Pac_Man
{
    class PacMan
    {
        KeyboardState keyboardState;
        GamePadState gamepadState;
        Texture2D[] pacmanos = new Texture2D[4];

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right,
            Null
        }

        // Variáveis
        public Vector2 position;
        // private Texture2D sprite;
        private Direction direction;
        public int score;
        public int playerIndex;
        public int PacManAtual = 0;

        // Construtor
        public PacMan(Vector2 position, string textureName, int playerIndex, ContentManager content)
        {
            this.position = position;
            this.playerIndex = playerIndex;
            pacmanos[0] = content.Load<Texture2D>(textureName + "1");
            pacmanos[1] = content.Load<Texture2D>(textureName + "2");
            pacmanos[2] = content.Load<Texture2D>(textureName + "3");
            pacmanos[3] = content.Load<Texture2D>(textureName + "4");
            score = 0;
            this.direction = Direction.Null;
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pacmanos[PacManAtual], Auxiliares.Matrix2Screen(position), Color.White);
        }

        // Update
        public void Update(GameTime gameTime)
        {

        }

        // Dispose
        public void Dispose()
        {
            pacmanos[PacManAtual].Dispose();
        }

        // Simplifica a escrita da função dos botões do teclado e comando
        public void LerTeclas()
        {
            keyboardState = Keyboard.GetState();
            if (playerIndex == 1)
                gamepadState = GamePad.GetState(PlayerIndex.One);
            else if (playerIndex == 2)
                gamepadState = GamePad.GetState(PlayerIndex.Two);
        }

        private Direction GetDirectionByKeyState()
        {
            LerTeclas();

            // Baixo
            if ((keyboardState.IsKeyDown(Keys.Down) || gamepadState.IsButtonDown(Buttons.DPadDown)))
                return Direction.Down;
            // Cima
            else if (keyboardState.IsKeyDown(Keys.Up) || gamepadState.IsButtonDown(Buttons.DPadUp))
                return Direction.Up;
            // Esquerda
            else if (keyboardState.IsKeyDown(Keys.Left) || gamepadState.IsButtonDown(Buttons.DPadLeft))
                return Direction.Left;
            // Direita
            else if (keyboardState.IsKeyDown(Keys.Right) || gamepadState.IsButtonDown(Buttons.DPadRight))
                return Direction.Right;
            // Caso nenhuma tecla esteja a ser pressionada
            else return Direction.Null;
        }

        private void AutoMove(Direction direction, byte[,] board)
        {
            if (direction == Direction.Down)
            {
                if (Auxiliares.CanGo((int)position.X, (int)position.Y + 1, board))
                {

                    position.Y++;
                    Comer(board);
                }
            }
            else if (direction == Direction.Up)
            {
                // Cima
                if (Auxiliares.CanGo((int)position.X, (int)position.Y - 1, board))
                {

                    position.Y--;
                    Comer(board);
                }
            }
            else if (direction == Direction.Left)
            {

                // Warp da esquerda para a direita
                if (position.X == 0 && position.Y == 9)
                    position.X = 21;

                if (Auxiliares.CanGo((int)position.X - 1, (int)position.Y, board))
                {
                    position.X--;
                    Comer(board);

                }
            }
            else if (direction == Direction.Right)
            {
                // Warp da direita para a esquerda
                if (position.X == 20 && position.Y == 9)
                    position.X = -1;

                if (Auxiliares.CanGo((int)position.X + 1, (int)position.Y, board))
                {
                    position.X++;
                    Comer(board);

                }
            }
            else if (direction == Direction.Null) return;
        }

        public void HumanMove(float lastHumanMove, byte[,] board)
        {
            LerTeclas();
            int movementSize = 1;

            if (lastHumanMove >= 1f / 100f)
            {
                if (GetDirectionByKeyState() == Direction.Null)
                {
                    AutoMove(direction, board);
                    return;
                }

                #region Pac Man
                if (playerIndex == 1)
                {
                    // Baixo
                    if (keyboardState.IsKeyDown(Keys.Down) || gamepadState.IsButtonDown(Buttons.DPadDown))
                    {
                        if (Auxiliares.CanGo((int)position.X, (int)position.Y + movementSize, board))
                        {
                            PacManAtual = 1;
                            direction = Direction.Down;
                            position.Y += movementSize;
                            Comer(board);
                        }
                    }
                    // Cima
                    else if (keyboardState.IsKeyDown(Keys.Up) || gamepadState.IsButtonDown(Buttons.DPadUp))
                    {
                        if (Auxiliares.CanGo((int)position.X, (int)position.Y - movementSize, board))
                        {
                            PacManAtual = 2;
                            direction = Direction.Up;
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
                        else if (Auxiliares.CanGo((int)position.X - movementSize, (int)position.Y, board))
                        {
                            PacManAtual = 3;
                            direction = Direction.Left;
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
                            position.X = -3;
                        else if (Auxiliares.CanGo((int)position.X + movementSize, (int)position.Y, board))
                        {
                            PacManAtual = 0;
                            direction = Direction.Right;
                            position.X += movementSize;
                            Comer(board);
                        }
                    }
                }
                #endregion

                #region Pac Woman
                else if (playerIndex == 2)
                {
                    // Baixo
                    if ((keyboardState.IsKeyDown(Keys.S) || gamepadState.IsButtonDown(Buttons.DPadDown)))
                    {
                        if (Auxiliares.CanGo((int)position.X, (int)position.Y + 1, board))
                        {
                            PacManAtual = 1;
                            position.Y++;
                            Comer(board);
                        }
                    }
                    // Cima
                    else if (keyboardState.IsKeyDown(Keys.W) || gamepadState.IsButtonDown(Buttons.DPadUp))
                    {
                        if (Auxiliares.CanGo((int)position.X, (int)position.Y - 1, board))
                        {
                            PacManAtual = 2;
                            position.Y--;
                            Comer(board);
                        }
                    }
                    // Esquerda
                    else if (keyboardState.IsKeyDown(Keys.A) || gamepadState.IsButtonDown(Buttons.DPadLeft))
                    {
                        // Warp da esquerda para a direita
                        if (position.X == 0 && position.Y == 9)
                            position.X = 21;
                        else if (Auxiliares.CanGo((int)position.X - 1, (int)position.Y, board))
                        {
                            PacManAtual = 3;
                            position.X--;
                            Comer(board);
                        }
                    }
                    // Direita
                    else if (keyboardState.IsKeyDown(Keys.D) || gamepadState.IsButtonDown(Buttons.DPadRight))
                    {
                        // Warp da direita para a esquerda
                        if (position.X == 20 && position.Y == 9)
                            position.X = -1;
                        else if (Auxiliares.CanGo((int)position.X + 1, (int)position.Y, board))
                        {
                            PacManAtual = 0;
                            position.X++;
                            Comer(board);
                        }
                    }
                }
                #endregion
            }
        }

        // Faz o Pac Man comer as pellets
        public void Comer(byte[,] board)
        {
            if (board[(int)(position.Y + 0.5f), (int)(position.X + 0.5f)] == 1)
            {
                board[(int)(position.Y + 0.5f), (int)(position.X + 0.5f)] = 2;
                score++;
            }
            if (board[(int)(position.Y + 0.5f), (int)(position.X + 0.5f)] == 3)
            {
                board[(int)(position.Y + 0.5f), (int)(position.X + 0.5f)] = 2;
                score += 10; // idk
            }

        }

        public bool Collide(List<Fantasma> fantasmas)
        {
            foreach (Fantasma fantasma in fantasmas)
            {
                if (position.X == fantasma.Position.X && position.Y == fantasma.Position.Y)
                {
                    Console.WriteLine("mnham mnahm");
                    return true;
                }

            } return false;
        }

        public bool CanKill(List<Fantasma> fantasmas, string texturename)
        {
                foreach (Fantasma fantasmaa in fantasmas)
                {
                    texturename = "CanDie"; // >.<
                    return true;
                }

               return false;
           
        }

    }
}
