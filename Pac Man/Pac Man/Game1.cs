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
        Texture2D parede;
        Texture2D pacMan;
        Texture2D blinky;
        float ticker;
        KeyboardState keyStatus;
        GamePadState gamepadState;
       // Pacman pacMano;
        int yPac = 13, xPac = 9;
        int xBlink = 9, yBlink = 10;
        float lastHumanMove;
        SpriteFont font;
        int score = 0;

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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            dot = Content.Load<Texture2D>("dot");
            parede = Content.Load<Texture2D>("parede");
            pacMan = Content.Load<Texture2D>("pac man1");
            font = Content.Load<SpriteFont>("SpriteFont1");
            blinky = Content.Load<Texture2D>("blinky");
        }

        protected override void UnloadContent()
        {
            dot.Dispose();
            parede.Dispose();
            pacMan.Dispose();
            blinky.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            lastHumanMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
            ticker += gameTime.ElapsedGameTime.Milliseconds;

            MovePac();

                if (ticker >= 1000)
                {
                    ticker -= 1000;
                    LerTeclas();

                }
            base.Update(gameTime);
        }

        private void MovePac()
        {
                if (lastHumanMove >= 1f / 5f )
                {
                    lastHumanMove = 0f;

                    // Baixo
                    if ((keyStatus.IsKeyDown(Keys.Down) || gamepadState.IsButtonDown(Buttons.DPadDown)))
                    {
                        // Warp em baixo
                        if(xPac == 9 && yPac == 20)
                            yPac = 0;
                        else if (CanGo(xPac, yPac + 1))
                            yPac++;
                        Comer(xPac, yPac);
                    }
                    // Cima
                    else if (keyStatus.IsKeyDown(Keys.Up) || gamepadState.IsButtonDown(Buttons.DPadUp))
                    {
                        // Warp em cima
                        if(xPac==9 && yPac == 0)
                            yPac = 21;
                        if (CanGo(xPac, yPac - 1))
                            yPac--;
                        Comer(xPac, yPac);
                    }
                    // Esquerda
                    else if (keyStatus.IsKeyDown(Keys.Left) || gamepadState.IsButtonDown(Buttons.DPadLeft))
                    {
                        if (CanGo(xPac -1, yPac))
                            xPac--;
                        Comer(xPac, yPac);
                    }
                    // Direita
                    else if (keyStatus.IsKeyDown(Keys.Right) || gamepadState.IsButtonDown(Buttons.DPadRight))
                    {
                        if (CanGo(xPac+1, yPac))
                            xPac++;
                        Comer(xPac, yPac);
                    }
                } 
        }

        private void LerTeclas()
        {
            keyStatus = Keyboard.GetState(); //Simplifica a escrita da função dos botões
            gamepadState = GamePad.GetState(PlayerIndex.One); //Simplifica a escrita da função dos botões do comando
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
                for (int x = 0; x < 21; x++)
                {
                    for (int y = 0; y < 21; y++)
                    {
                        if (board[x, y] == 1) // ver comida
                            spriteBatch.Draw(dot, new Vector2(y * parede.Height, x * parede.Width), Color.White);

                       if (board[x, y] == 0) // ver parede
                           spriteBatch.Draw(parede, new Vector2(y * parede.Height, x * parede.Width), Color.White);

                     
                    }
                }
            spriteBatch.Draw(pacMan,Matrix2Screen( new Vector2(xPac, yPac)), Color.White);
            spriteBatch.Draw(blinky, Matrix2Screen( new Vector2(xBlink, yBlink)), Color.White);
            //spriteBatch.Draw(blinky, new Rectangle(xBlink * 30, yBlink*30, 30, 30), Color.Red);
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(670, 100), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);

            
        }
       
        private bool CanGo(int xPac,int yPac)
        {
            if (board[xPac, yPac] != 0)
                return true;
            else return false;
        }

        private void Comer(int xPac, int yPac)
        {
            if (board[xPac, yPac] == 1)
            {
                board[xPac, yPac] = 2;
                score++;
            }
        }

     
        private void Blinky(int xBlink, int yBlink)
        {
            xBlink--;
        }
        
        private Vector2 Matrix2Screen(Vector2 matrixPosition)
        {
            return (matrixPosition * 30);
        }
        private Vector2 Screen2Matrix(Vector2 screenPosition)
        {
            return (screenPosition / 30);
        }
    }
}
