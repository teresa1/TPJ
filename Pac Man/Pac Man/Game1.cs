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
        byte[,] board = {{2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2}, //linha 0
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
                         {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2}};//linha 20

        // Variaveis
        Texture2D dot, wall;
        PacMan pacMan, pacWoman;
        Fantasma blinky, pinky, inky, clyde;
        List<Fantasma> fantasmas;
        List<PacMan> jogadores;

        KeyboardState keyState;
        GamePadState gamepadState;

        float lastHumanMove;
        float ticker;
        float timer = 0;
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
            wall = Content.Load<Texture2D>("parede");
            font = Content.Load<SpriteFont>("SpriteFont1");

            // Criação de Pac Mans
            pacMan = new PacMan(new Vector2(9, 9), "PacMan", Content);
            pacWoman = new PacMan(new Vector2(11, 9), "PacMan", Content);
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

            //Collide(xPac, yPac);
            lastHumanMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
            ticker += gameTime.ElapsedGameTime.Milliseconds;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            LerTeclas();
           
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
                    for (int y = 0; y < 21; y++)
                    {
                        // Comida
                        if (board[y, x] == 1)
                            spriteBatch.Draw(dot, new Vector2(x * wall.Width , y * wall.Height), Color.White);
                        // Paredes
                        if (board[y, x] == 0)
                           spriteBatch.Draw(wall, new Vector2(x * wall.Width , y * wall.Height), Color.White);
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
                    if (Auxiliares.CanGo((int)pacMan.position.X, (int)pacMan.position.Y + 1, board))
                    {
                        pacMan.position.Y++;
                        pacMan.Comer(board);
                    }
                }
                // Cima
                else if (keyState.IsKeyDown(Keys.Up) || gamepadState.IsButtonDown(Buttons.DPadUp))
                {
                    if (Auxiliares.CanGo((int)pacMan.position.X, (int)pacMan.position.Y - 1, board))
                    {
                        pacMan.position.Y--;
                        pacMan.Comer(board);
                    }
                }
                // Esquerda
                else if (keyState.IsKeyDown(Keys.Left) || gamepadState.IsButtonDown(Buttons.DPadLeft))
                {
                    // Warp da esquerda para a direita
                    if (pacMan.position.X == 0 && pacMan.position.Y == 9)
                        pacMan.position.X = 21;

                    if (Auxiliares.CanGo((int)pacMan.position.X - 1, (int)pacMan.position.Y, board))
                    {
                        pacMan.position.X--;
                        pacMan.Comer(board);
                    }
                }
                // Direita
                else if (keyState.IsKeyDown(Keys.Right) || gamepadState.IsButtonDown(Buttons.DPadRight))
                {
                    // Warp da direita para a esquerda
                    if (pacMan.position.X == 20 && pacMan.position.Y == 9)
                        pacMan.position.X = -1;

                    if (Auxiliares.CanGo((int)pacMan.position.X + 1, (int)pacMan.position.Y, board))
                    {
                        pacMan.position.X++;
                        pacMan.Comer(board);
                    }
                }
                #endregion

                #region Pac Woman
                // Baixo
                if ((keyState.IsKeyDown(Keys.S) || gamepadState.IsButtonDown(Buttons.DPadDown)))
                {
                    if (Auxiliares.CanGo((int)pacWoman.position.X, (int)pacWoman.position.Y + 1, board))
                    {
                        pacWoman.position.Y++;
                        pacWoman.Comer(board);
                    }
                }
                // Cima
                else if (keyState.IsKeyDown(Keys.W) || gamepadState.IsButtonDown(Buttons.DPadUp))
                {
                    if (Auxiliares.CanGo((int)pacWoman.position.X, (int)pacWoman.position.Y - 1, board))
                    {
                        pacWoman.position.Y--;
                        pacWoman.Comer(board);
                    }
                }
                // Esquerda
                else if (keyState.IsKeyDown(Keys.A) || gamepadState.IsButtonDown(Buttons.DPadLeft))
                {
                    // Warp da esquerda para a direita
                    if (pacWoman.position.X == 0 && pacWoman.position.Y == 9)
                        pacWoman.position.X = 21;

                    if (Auxiliares.CanGo((int)pacWoman.position.X - 1, (int)pacWoman.position.Y, board))
                    {
                        pacWoman.position.X--;
                        pacWoman.Comer(board);
                    }
                }
                // Direita
                else if (keyState.IsKeyDown(Keys.D) || gamepadState.IsButtonDown(Buttons.DPadRight))
                {
                    // Warp da direita para a esquerda
                    if (pacWoman.position.X == 20 && pacWoman.position.Y == 9)
                        pacWoman.position.X = -1;

                    if (Auxiliares.CanGo((int)pacWoman.position.X + 1, (int)pacWoman.position.Y, board))
                    {
                        pacWoman.position.X++;
                        pacWoman.Comer(board);
                    }
                }
                #endregion
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
