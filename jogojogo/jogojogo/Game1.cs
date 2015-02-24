#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace jogojogo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D box;
        byte[,] board = new byte[22, 10];
        Piece piece;
        Color[] colors = { Color.Orange, Color.Red, Color.MintCream, Color.Lime, Color.LightBlue, Color.Yellow, Color.DimGray };

        int pX = 4, pY = -2;
        float lastAutomaticMove = 0f;
        float lastHumanMove = 0f;
        bool spacePressed = false;


        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 300;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            piece = new Piece();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            box = Content.Load <Texture2D>("box");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            box.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //sair do jogo
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //conta tempo
            lastAutomaticMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
            lastHumanMove += (float)gameTime.ElapsedGameTime.TotalSeconds; 
            //movimento para baixo
            if (lastAutomaticMove > 1f)
            {
                if (canGoDown())
                {
                    pY++;
                    lastAutomaticMove = 0;
                }
                else newPiece();
                
            }
            if (lastHumanMove >= 1f / 20f)
            {//verificar se a seta para baixo foi pressionada
                lastHumanMove = 0f;

                if (Keyboard.GetState().IsKeyDown(Keys.Down) && (canGoDown()))
                {
                     pY++;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Right) && (canGoRight()))
                {
                     pX++;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Left) && (canGoLeft()))
                {
                     pX--;
                }
                if (Keyboard.GetState().IsKeyUp(Keys.Space)) spacePressed = false;
                else if (spacePressed == false && Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    while (canGoDown())
                    { pY++; }
                    newPiece();
                    spacePressed = true;
                }
               
                base.Update(gameTime);
            }
        }

        private void newPiece()
        {
            {
                freeze();
                pY = -2;
                piece = new Piece();
                pX = ((10 - piece.width) / 2);
            }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
           
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    if(board[y+2, x] != 0) // ver peças mortas
                                spriteBatch.Draw(box, new Vector2(x*30, y*30), colors[board[y+2,x]-1]);
                    if(y >= pY && x>=pX && y<pY +piece.height && x<pX + piece.width) // desenhar a peça que está a cair
                         {
                             if (piece.GetBlock(y - pY, x - pX) != 0)
                                 spriteBatch.Draw(box, new Vector2(x * 30, y * 30), colors[piece.GetBlock(y-pY,x-pX)-1]);
                         }
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);

        }

        private bool canGoDown()
        {
            if(pY + piece.height >= 20)
                return false;
            else 
                return canGo(pX, pY+1);
        }

        private bool canGo(int dx, int dy)
        {
            //supondo que é possivel...
            for (int x = 0; x < piece.width; x++)
            {
                for (int y = 0; y < piece.height; y++)
                {
                    if (piece.GetBlock(y, x) != 0 && board[dy + y +2 , dx + x] != 0)
                        return false;
                }
            }
            return true;
        }

        private bool canGoLeft()
        {
            if (pX == 0) return false;
            else return canGo(pX - 1, pY);
        }
        private bool canGoRight()
        {
            if (pX + piece.width == 10) return false;
            else return canGo(pX + 1, pY);
        }
        private  void freeze()
        {
            for (int x = 0; x < piece.width; x++)
            {
                for (int y = 0; y < piece.height; y++)
                {
                    if (piece.GetBlock(y, x) != 0)
                    {
                        board[pY + y + 2, pX + x] = piece.GetBlock(y, x);
                    }
                }
            }
        }
    }
}
