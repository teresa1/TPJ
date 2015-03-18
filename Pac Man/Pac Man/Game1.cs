#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Pac_Man
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random random;
        
        /*            0-parede    1- comida    2-vazio         */
        byte[,] board = {{2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2}, //linha 0
                         {2,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2}, //linha 1
                         {2,0,1,0,0,1,0,0,0,1,0,1,0,0,0,1,0,0,1,0,2}, //linha 2
                         {2,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2}, //linha 3
                         {2,0,1,0,0,1,0,1,0,0,0,0,0,1,0,1,0,0,1,0,2}, //linha 4
                         {2,0,1,1,1,1,0,1,1,1,0,1,1,1,0,1,1,1,1,0,2}, //linha 5
                         {2,0,0,0,0,1,0,0,0,2,0,2,0,0,0,1,0,0,0,0,2}, //linha 6
                         {2,2,2,2,0,1,0,2,2,2,2,2,2,2,0,1,0,2,2,2,2}, //linha 7
                         {0,0,0,0,0,1,0,2,0,0,2,0,0,2,0,1,0,0,0,0,0}, //linha 8
                         {2,2,2,2,2,1,2,2,0,2,2,2,0,2,2,1,2,2,2,2,2}, //linha 9
                         {0,0,0,0,0,1,0,2,0,0,0,0,0,2,0,1,0,0,0,0,0}, //linha 10
                         {2,2,2,2,0,1,0,2,2,2,2,2,2,2,0,1,0,2,2,2,2}, //linha 11
                         {2,0,0,0,0,1,0,2,0,0,0,0,0,2,0,1,0,0,0,0,2}, //linha 12
                         {2,0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,2}, //linha 13
                         {2,0,1,0,0,1,0,0,0,1,0,1,0,0,0,1,0,0,1,0,2}, //linha 14
                         {2,0,1,1,0,1,1,1,1,1,1,1,1,1,1,1,0,1,1,0,2}, //linha 15
                         {2,0,0,1,0,1,0,1,0,0,0,0,0,1,0,1,0,1,0,0,2}, //linha 16
                         {2,0,1,1,1,1,0,1,1,1,0,1,1,1,0,1,1,1,1,0,2}, //linha 17
                         {2,0,1,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,1,0,2}, //linha 18
                         {2,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2}, //linha 19
                         {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2}};//linha 20

        // Variaveis
        Texture2D dot;
        Texture2D wall;
        Texture2D pacMan;

        KeyboardState keyState;
        GamePadState gamepadState;

        int yPac = 13, xPac = 9;

        float lastHumanMove;
        float ticker;
        int score = 0;

        SpriteFont font;
        List<Fantasma> fantasmas;
        
        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 630;
            graphics.PreferredBackBufferWidth = 850;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            fantasmas = new List<Fantasma>();
            random = new Random();

            // Simplifica a escrita da função dos botões do teclado e comando
            keyState = Keyboard.GetState();
            gamepadState = GamePad.GetState(PlayerIndex.One);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            dot = Content.Load<Texture2D>("dot");
            wall = Content.Load<Texture2D>("parede");
            pacMan = Content.Load<Texture2D>("pac man1");
            font = Content.Load<SpriteFont>("SpriteFont1");
            Fantasma fantasma1 = new Fantasma(new Vector2(14, 15), "blinky", Content);
            Fantasma fantasma2 = new Fantasma(new Vector2(5, 10), "blinky", Content);
            fantasmas.Add(fantasma1);
            fantasmas.Add(fantasma2);
        }

        protected override void UnloadContent()
        {
            dot.Dispose();
            wall.Dispose();
            pacMan.Dispose();
            foreach (var fantasma in fantasmas)
                fantasma.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            lastHumanMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
            ticker += gameTime.ElapsedGameTime.Milliseconds;


            // Movimento dos fantasmas e do jogador
            if (ticker >= 200)
            {
                ticker -= 200;
                Move();

                foreach (var fantasma in fantasmas)
                    fantasma.Update(board, random);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
                for (int x = 0; x < 21; x++)
                {
                    for (int y = 0; y < 21; y++)
                    {
                        // Comida
                        if (board[y, x] == 1)
                            spriteBatch.Draw(dot, new Vector2(x * wall.Width , y * wall.Height), Color.White);
                        // Paredes
                        if (board[y, x] == 0)
                           spriteBatch.Draw(wall, new Vector2(x * wall.Width , y * wall.Height), Color.White);
                    }
                }
            // Pacman e Fantamas 
            spriteBatch.Draw(pacMan, Auxiliares.Matrix2Screen(new Vector2(xPac, yPac)), Color.White);
            foreach (var fantasma in fantasmas)
                fantasma.Draw(spriteBatch);
            // Score
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(670, 100), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        // Movimento do jogador
        private void Move()
        {
            if (lastHumanMove >= 1f / 5f)
            {
                lastHumanMove = 0f;

                // Baixo
                if ((keyState.IsKeyDown(Keys.Down) || gamepadState.IsButtonDown(Buttons.DPadDown)))
                {
                    if (Auxiliares.canGo(xPac, yPac + 1, board))
                    {
                        yPac++;
                        Comer(xPac, yPac);
                    }

                }
                // Cima
                else if (keyState.IsKeyDown(Keys.Up) || gamepadState.IsButtonDown(Buttons.DPadUp))
                {
                    if (Auxiliares.canGo(xPac, yPac - 1, board))
                    {
                        yPac--;
                        Comer(xPac, yPac);
                    }
                }
                // Esquerda
                else if (keyState.IsKeyDown(Keys.Left) || gamepadState.IsButtonDown(Buttons.DPadLeft))
                {
                    // Warp da esquerda para a direita
                    if (xPac == 0 && yPac == 9)
                        xPac = 21;

                    if (Auxiliares.canGo(xPac - 1, yPac, board))
                    {
                        xPac--;
                        Comer(xPac, yPac);
                    }
                }
                // Direita
                else if (keyState.IsKeyDown(Keys.Right) || gamepadState.IsButtonDown(Buttons.DPadRight))
                {
                    // Warp da direita para a esquerda
                    if (xPac == 20 && yPac == 9)
                        xPac = -1;

                    if (Auxiliares.canGo(xPac + 1, yPac, board))
                    {
                        xPac++;
                        Comer(xPac, yPac);
                    }
                }
            }
        }

        // Faz pacman comer dots
        private void Comer(int xPac, int yPac)
        {
            if (board[yPac, xPac] == 1)
            {
                board[yPac, xPac] = 2;
                score++;
            }
        }
    }
}
