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
        List<Fantasma> Fantasmas;
        
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

            Fantasmas = new List<Fantasma>();
            random = new Random();

           base.Initialize();
           
           
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            dot = Content.Load<Texture2D>("dot");
            parede = Content.Load<Texture2D>("parede");
            pacMan = Content.Load<Texture2D>("pac man1");
            font = Content.Load<SpriteFont>("SpriteFont1");
            Fantasma fantasma1 = new Fantasma(new Vector2(14, 15), "blinky", Content);
            Fantasma fantasma2 = new Fantasma(new Vector2(5, 10), "blinky", Content);
            Fantasmas.Add(fantasma1);
            Fantasmas.Add(fantasma2);
            
        }

        protected override void UnloadContent()
        {
            dot.Dispose();
            parede.Dispose();
            pacMan.Dispose();
            foreach (var fantasma in Fantasmas)
            {
                fantasma.Dispose();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            lastHumanMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
            ticker += gameTime.ElapsedGameTime.Milliseconds;

                    LerTeclas();

                if (ticker >= 200)
                {
                    ticker -= 200;
            MoveIt();

                    foreach (var fantasma in Fantasmas)
                    {
                        fantasma.Update(board, random);
                    }

                }

                

            base.Update(gameTime);
        }

        private void MoveIt()
        {
                if (lastHumanMove >= 1f / 5f )
                {
                    lastHumanMove = 0f;
                    if ((keyStatus.IsKeyDown(Keys.Down) || gamepadState.IsButtonDown(Buttons.DPadDown)))
                    {
                        if(xPac == 9 && yPac == 20)
                        {
                            yPac = 0;
                        }
                        else if (Auxiliares.canGo(xPac, yPac + 1, board))
                        {
                            yPac++;
                            Comer(xPac, yPac);
                        }
                        
                    }
                    else if (keyStatus.IsKeyDown(Keys.Up) || gamepadState.IsButtonDown(Buttons.DPadUp))
                    {
                        if(xPac==9 && yPac == 0)
                        {
                            yPac = 21;
                        }
                        if (Auxiliares.canGo(xPac, yPac - 1, board))
                        yPac--;
                        Comer(xPac, yPac);
                    }
                    else if (keyStatus.IsKeyDown(Keys.Left) || gamepadState.IsButtonDown(Buttons.DPadLeft))
                    {
                        if (Auxiliares.canGo(xPac -1, yPac, board))
                        xPac--;
                        Comer(xPac, yPac);
                    }
                    else if (keyStatus.IsKeyDown(Keys.Right) || gamepadState.IsButtonDown(Buttons.DPadRight))
                    {
                        if (Auxiliares.canGo(xPac+1, yPac, board))
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
                        if (board[y, x] == 1) // ver comida
                            spriteBatch.Draw(dot, new Vector2(x * parede.Width , y * parede.Height ), Color.White);

                       if (board[y, x] == 0) // ver parede
                           spriteBatch.Draw(parede, new Vector2(x * parede.Width , y * parede.Height), Color.White);

                     
                    }
                }
            spriteBatch.Draw(pacMan,Auxiliares.Matrix2Screen( new Vector2(xPac, yPac)), Color.White);
            
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(670, 100), Color.White);
            foreach (var fantasma in Fantasmas)
            {
                fantasma.Draw(spriteBatch);
            }


            spriteBatch.End();


            base.Draw(gameTime);

            
        }
       
        

        private void Comer(int xPac, int yPac)
        {
            if (board[yPac, xPac] == 1)
            {
                board[yPac, xPac] = 2;
                score++;
            }
        }

     
        private void Blinky(int xBlink, int yBlink)
        {
            xBlink--;
        }
        
       
       
    }
}
