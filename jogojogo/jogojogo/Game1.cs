#region Using Statements
using System;
using Microsoft.Xna.Framework.Media;
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
        enum GameStatus
        {
            Gameplay,
            Freeze,
            Highlight,
            GameOver,
        };
        GameStatus status;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D box;
        Texture2D pixel;
        byte[,] board = new byte[22, 10];
        Piece piece;
        Piece nextPiece;
        Color[] colors = { Color.Orange, Color.Red, Color.MintCream, Color.Lime, Color.LightBlue, Color.Yellow, Color.DeepPink, Color.Black };
        SpriteFont font;

        Song cocaine;

        float hightlightTime;
        int pX = 4, pY = -2;
        float lastAutomaticMove = 0f;
        float lastHumanMove = 0f;
        bool spacePressed = false;
        bool upPressed = false;
        int completeLine;

        int score = 0;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 650;
            graphics.PreferredBackBufferWidth = 500;
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
            nextPiece = new Piece();
            status = GameStatus.Gameplay;
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
            font = Content.Load<SpriteFont>("SpriteFont1");
            pixel = Content.Load<Texture2D>("pixel");
            //cocaine = Content.Load<Song>("Nomy- Cocain (with lyrics)");
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
            Console.WriteLine("update: " + status);
            //sair do jogo
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //conta tempo
            lastAutomaticMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
            lastHumanMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //movimento para baixo
            if (status == GameStatus.Gameplay)
            {
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
                    if (Keyboard.GetState().IsKeyUp(Keys.Up)) upPressed = false;
                    else if (Keyboard.GetState().IsKeyDown(Keys.Up) && upPressed == false)
                    {
                        piece.Rotate();
                        if (!canGo(pX, pY))
                            piece.Unrotate();

                        upPressed = true;
                    }
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
                }
            }
                if (status == GameStatus.Highlight)
                {
                    if (hightlightTime >= .2f)
                    {
                        RemoveLine(completeLine);
                        DetectCompleteLine();
                        if (completeLine != 0)
                        {
                            highlightLine(completeLine);
                            hightlightTime = 0f;
                        }
                        else
                        {
                            piece = nextPiece;
                            nextPiece = new Piece();
                            pY = -2;
                            pX = (10 - piece.width) / 2;
                            if (canGo(pX, pY))
                            {
                                status = GameStatus.Gameplay;
                            }
                            else status = GameStatus.GameOver;
                        }
                    }
                    hightlightTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            
            base.Update(gameTime);

        }

        private void newPiece()
        {
            {
                freeze();
                if (completeLine == 0)
                {
                    piece = nextPiece;
                    status = GameStatus.Gameplay;
                    pY = -2;
                    nextPiece = new Piece();
                    pX = ((10 - piece.width) / 2);

                    if(canGo(pX,pY))
                    {
                        status = GameStatus.Gameplay;
                    }
                    else
                    {
                        status = GameStatus.GameOver;
                    }
                }
                else
                {
                    status = GameStatus.Highlight;
                    highlightLine(completeLine);
                    hightlightTime = 0f;
                }
            }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
           
            GraphicsDevice.Clear(Color.Black);
            
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            DrawRectangle(new Rectangle(25, 25, 302, 602), Color.White);

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    if(board[y+2, x] != 0) // ver peças mortas
                                spriteBatch.Draw(box, new Vector2(x*30+25, y*30+25), colors[board[y+2,x]-1]);
                    if(status == GameStatus.Gameplay && y >= pY && x>=pX && y<pY +piece.height && x<pX + piece.width) // desenhar a peça que está a cair
                         {
                             if (piece.GetBlock(y - pY, x - pX) != 0)
                                 spriteBatch.Draw(box, new Vector2(x * 30+25, y * 30+25), colors[piece.GetBlock(y-pY,x-pX)-1]);
                         }
                }
            }


            if(status == GameStatus.GameOver)
            {
                spriteBatch.DrawString(font, "Game Over!!!", new Vector2(10, 250), Color.White);
            }

            spriteBatch.DrawString(font, "Score: " + score, new Vector2(330, 100), Color.White);
            DrawNextPiece();
            spriteBatch.End();
          
            base.Draw(gameTime);

        }
        private void DrawRectangle(Rectangle r, Color c)
        {
            spriteBatch.Draw(pixel, new Rectangle(r.X, r.Y,  r.Width, 1), Color.White);
            spriteBatch.Draw(pixel, new Rectangle(r.X, r.Y, 1, r.Height), Color.White);
            spriteBatch.Draw(pixel, new Rectangle(r.X, r.Y +r.Height-1, r.Width, 1), Color.White);
            spriteBatch.Draw(pixel, new Rectangle(r.X+r.Width-1, r.Y, 1 , r.Height), Color.White);
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
            if (dx < 0) return (false);
            if (dx + piece.width > 10) return (false);
            if (dy + piece.height > 20) return (false);
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
            if (pX == 10) return false;
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
            status = GameStatus.Freeze;
            DetectCompleteLine();
            hightlightTime = 0f;
            score++;
        }
        
        private void highlightLine(int l)
        {
            for (int x = 0; x < 10; x++)
            {
                board[l, x] = 8; // cor para highlight
            }
            score = score + 5;
        }

        private void DetectCompleteLine()
        {
            completeLine = 0;

            for (int y = 21; y > 1 && completeLine == 0; y--)
            {
                bool complete = true;
                for (int x = 0; x < 10 && complete; x++)
                {
                    if (board[y, x] == 0) complete = false;
                }
                if (complete) completeLine = y;
            }
        }

        private void RemoveLine(int line)
        {
            for (int y = line; y >= 1; y--)
            {
                for (int x = 0; x < 10; x++)
                {
                    board[y, x] = board[y - 1, x];
                }
            }
        }

        private void DrawNextPiece()
        {
            int posX = 350;
            int posY = 150;

            for (int x = 0; x < nextPiece.width; x++)
            {
                for (int y = 0; y < nextPiece.height; y++)
                {
                    byte c = nextPiece.GetBlock(y, x);

                    if(c!= 0)
                    {
                        spriteBatch.Draw(box, new Vector2(posX + x*30, posY + y*30),  colors[c-1]);
                    }
                }
            }
        }
    }
}
