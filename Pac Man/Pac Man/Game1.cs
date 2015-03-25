﻿#region Using Statements
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
        byte[,] board = {{2,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0,2}, //linha 0
                         {2,0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,2}, //linha 1
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
                         {2,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0,2}};//linha 20

        // Variaveis
        Texture2D dot, wall, wallTop;
        PacMan pacMan, pacWoman;
        Fantasma blinky, pinky, inky, clyde;
        List<Fantasma> fantasmas;
        List<PacMan> jogadores;

        KeyboardState keyState;
        GamePadState gamepadState;

        float lastHumanMove;
        float ticker;
        int timer = 0;
        int score = 0;

        SpriteFont font;

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
            jogadores = new List<PacMan>();
            fantasmas = new List<Fantasma>();
            random = new Random();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            dot = Content.Load<Texture2D>("dot");
            wall = Content.Load<Texture2D>("wall");
            wallTop = Content.Load<Texture2D>("wallTop");
            font = Content.Load<SpriteFont>("SpriteFont1");

            // Criação de Pac Mans
            pacMan = new PacMan(new Vector2(9, 7), "PacMan", Content);
            pacWoman = new PacMan(new Vector2(11, 9), "PacWoman", Content);
            jogadores.Add(pacMan);
            jogadores.Add(pacWoman);

            // Criação de Fantasmas
            blinky = new Fantasma(new Vector2(18, 2), "blinky", Content);
            pinky = new Fantasma(new Vector2(3, 20), "blinky", Content);
            inky = new Fantasma(new Vector2(3, 2), "blinky", Content);
            clyde = new Fantasma(new Vector2(3, 2), "blinky", Content);
            fantasmas.Add(blinky);
            fantasmas.Add(pinky);
            fantasmas.Add(inky);
            fantasmas.Add(clyde);
        }

        protected override void UnloadContent()
        {
            dot.Dispose();
            wall.Dispose();
            foreach (var pacMan in jogadores)
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

            timer += (int)(gameTime.ElapsedGameTime.TotalSeconds + 0.5f);

            LerTeclas();
            Move();

            // Movimento dos fantasmas e do jogador
            if (ticker >= 100)
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
                for (int y = 0; y < 21; y++)
                {
                    // Comida
                    if (board[y, x] == 1)
                        spriteBatch.Draw(dot, new Vector2(x * wall.Width, y * wall.Height), Color.White);
                    // Paredes
                    if (board[y, x] == 0)
                        spriteBatch.Draw(wall, new Vector2(x * wall.Width, y * wall.Height), Color.White);
                    if (board[y, x] == 3)
                        spriteBatch.Draw(wallTop, new Vector2(x * wall.Width, y * wall.Height), Color.White);
                }

            // Pac Man's e Fantamas 
            foreach (var pacMan in jogadores)
                pacMan.Draw(spriteBatch);
            foreach (var fantasma in fantasmas)
                fantasma.Draw(spriteBatch);

            // Score e Time
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(670, 100), Color.White);
            spriteBatch.DrawString(font, "Time: " + timer, new Vector2(650, 500), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        // Simplifica a escrita da função dos botões do teclado e comando
        private void LerTeclas()
        {
            keyState = Keyboard.GetState();
            gamepadState = GamePad.GetState(PlayerIndex.One);
        }

        // Movimento do jogador
        private void Move()
        {
            if (lastHumanMove >= 1f / 50f)
            {
                lastHumanMove = 0f;

                #region Pac Man
                // Baixo
                if ((keyState.IsKeyDown(Keys.Down) || gamepadState.IsButtonDown(Buttons.DPadDown)))
                {
                    if (Auxiliares.CanGo(pacMan.rPosition, board))
                    {
                        pacMan.changeDirection(PacMan.Movimentos.baixo);
                    }
                }
                // Cima
                else if (keyState.IsKeyDown(Keys.Up) || gamepadState.IsButtonDown(Buttons.DPadUp))
                {
                    if (Auxiliares.CanGo(pacMan.rPosition.X, pacMan.rPosition.Y - Auxiliares.Matrix2Screen(1), board))
                    {
                        pacMan.changeDirection(PacMan.Movimentos.cima);
                    }
                }
                // Esquerda
                else if (keyState.IsKeyDown(Keys.Left) || gamepadState.IsButtonDown(Buttons.DPadLeft))
                {
                    // Warp da esquerda para a direita sim n faz mal.. isso é o menos..
                    /*if (pacMan.vPosition.X == 0 && pacMan.vPosition.Y == 9)
                        pacMan.vPosition.X = 21;*/

                    if (Auxiliares.CanGo(pacMan.rPosition.X - Auxiliares.Matrix2Screen(1), pacMan.rPosition.Y, board))
                    {
                        pacMan.changeDirection(PacMan.Movimentos.esquerda);
                    }
                }
                // Direita
                else if (keyState.IsKeyDown(Keys.Right) || gamepadState.IsButtonDown(Buttons.DPadRight))
                {
                    // Warp da direita para a esquerda
                    /*if (pacMan.vPosition.X == 20 && pacMan.vPosition.Y == 9)
                        pacMan.vPosition.X = -1;*/

                    if (Auxiliares.CanGo(pacMan.rPosition.X + Auxiliares.Matrix2Screen(1), pacMan.rPosition.Y, board))
                    {
                        pacMan.changeDirection(PacMan.Movimentos.direita);
                    }
                }

                pacMan.Update(board);
                #endregion

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
            }
        }
    }
}
// ja nem sei o que ele mudou pq eu tava a fazer outras coisas >.<