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

        /*            0-parede    1- comida    2-vazio     3- big dots idk    */
        byte[,] board = {{2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2}, // Linha 0
                         {2,0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,2}, // Linha 1
                         {2,0,3,0,0,1,0,0,0,1,0,1,0,0,0,1,0,0,3,0,2}, // Linha 2
                         {2,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2}, // Linha 3
                         {2,0,1,0,0,1,0,1,0,0,0,0,0,1,0,1,0,0,1,0,2}, // Linha 4
                         {2,0,1,1,1,1,0,1,1,1,0,1,1,1,0,1,1,1,1,0,2}, // Linha 5
                         {2,0,0,0,0,1,0,0,0,2,0,2,0,0,0,1,0,0,0,0,2}, // Linha 6
                         {2,2,2,2,0,1,0,2,2,2,2,2,2,2,0,1,0,2,2,2,2}, // Linha 7
                         {0,0,0,0,0,1,0,2,0,2,2,2,0,2,0,1,0,0,0,0,0}, //linha 8
                         {2,2,2,2,2,1,2,2,0,2,2,2,0,2,2,1,2,2,2,2,2}, //linha 9
                         {0,0,0,0,0,1,0,2,0,0,0,0,0,2,0,1,0,0,0,0,0}, //linha 10
                         {2,2,2,2,0,1,0,2,2,2,2,2,2,2,0,1,0,2,2,2,2}, //linha 11
                         {2,0,0,0,0,1,0,2,0,0,0,0,0,2,0,1,0,0,0,0,2}, //linha 12
                         {2,0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,2}, //linha 13
                         {2,0,1,0,0,1,0,0,0,1,0,1,0,0,0,1,0,0,1,0,2}, //linha 14
                         {2,0,3,1,0,1,1,1,1,1,1,1,1,1,1,1,0,1,3,0,2}, //linha 15
                         {2,0,0,1,0,1,0,1,0,0,0,0,0,1,0,1,0,1,0,0,2}, //linha 16
                         {2,0,1,1,1,1,0,1,1,1,0,1,1,1,0,1,1,1,1,0,2}, //linha 17
                         {2,0,1,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,1,0,2}, //linha 18
                         {2,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2}, //linha 19
                         {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2}};//linha 20

        // Variaveis
        Texture2D dot, largeDot, wall;
        PacMan pacMan, pacWoman;
        Fantasma blinky, pinky, inky, clyde;
        List<Fantasma> fantasmas;
        List<PacMan> jogadores;

        float lastHumanMove;
        float ticker;
        float timer = 0;
        bool isGameOver = false;

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
            largeDot = Content.Load<Texture2D>("largeDot");
            wall = Content.Load<Texture2D>("parede");
            font = Content.Load<SpriteFont>("SpriteFont1");

            // Ciação de Pac Mans
            pacMan = new PacMan(new Vector2(9, 9), "PacMan", 1, Content);
            pacWoman = new PacMan(new Vector2(11, 9), "PacWoman", 2, Content);
            jogadores.Add(pacMan);
            jogadores.Add(pacWoman);

            // Criação de Fantasmas
            blinky = new Fantasma(new Vector2(5, 3), "blinky", Content);
            pinky = new Fantasma(new Vector2(15, 3), "pinky", Content);
            inky = new Fantasma(new Vector2(5, 15), "pinky", Content);
            clyde = new Fantasma(new Vector2(15, 15), "blinky", Content);
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

            // Caso não seja Game Over, continua o jogo
            if (!isGameOver)
            {
                lastHumanMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
                ticker += gameTime.ElapsedGameTime.Milliseconds;
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                GameOver();

                // Movimento dos fantasmas e do jogador
                if (ticker >= 250)
                {
                    ticker -= 250;
                    pacMan.HumanMove(lastHumanMove, board);
                    pacWoman.HumanMove(lastHumanMove, board);

                    foreach (var fantasma in fantasmas)
                        fantasma.Update(board, random);

                }
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
                    // Comida bónus
                    if (board[y, x] == 3)
                        spriteBatch.Draw(largeDot, new Vector2(x * wall.Width, y * wall.Height), Color.White);
                    // Paredes
                    if (board[y, x] == 0)
                        spriteBatch.Draw(wall, new Vector2(x * wall.Width, y * wall.Height), Color.White);
                }

            // Pac Man's e Fantamas 
            foreach (var pac in jogadores)
                pac.Draw(spriteBatch);
            foreach (var fanta in fantasmas)
                fanta.Draw(spriteBatch);
               
            // Score e Time
            spriteBatch.DrawString(font, "Score: " + (this.pacMan.score + pacWoman.score), new Vector2(670, 100), Color.White);
            spriteBatch.DrawString(font, "Time: " + timer.ToString("0"), new Vector2(650, 500), Color.White);

            // Game Over
            if (isGameOver)
                spriteBatch.DrawString(font, " Hahah\n Morreste :D\n Tenta de novo!!", new Vector2(630, 250), Color.Red);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        // Altera a variável para true caso seja Game Over
        private void GameOver()
        {
            if (pacMan.Collide(fantasmas) || pacWoman.Collide(fantasmas))
            {
                isGameOver = true;
            }
        }
    }
}
