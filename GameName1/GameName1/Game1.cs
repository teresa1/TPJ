#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Sugar_Run
{
    public class Game1 : Game
    {
        // Variáveis
        Random random = new Random();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Scene scene;
        ScrollingBackground background;
        Player player;
        EnemyBroccoli enemyBroccoli;
        EnemyCorn enemyCorn;
        // Plataformas
        Vector2 platformPosition;
        float platformTime;
        int platformCounter;
        int randomPlatformHeight;
        // Inimigos
        float enemyTime1;
        float enemyTime2;
        // Power-Ups
        int randomLollipop;
        Texture2D start;
        SpriteFont font;

        bool escPressed = false;
        
        KeyboardState keyboardState = Keyboard.GetState();
        
        public enum GameStatus 
        {
            start, game
        }
       public GameStatus status;

        
        // Construtor
        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "Sugar Run - Kill Those Vegies ";
        }

        protected override void Initialize()
        {
            // Definição do tamanho da janela
            graphics.PreferredBackBufferHeight = 450;
            graphics.PreferredBackBufferWidth = 800;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            // Inicialização da câmara
            Camera.SetGraphicsDeviceManager(graphics);
            Camera.SetWorldWidth(15);

            platformPosition = new Vector2(6, -1.5f);
            platformCounter = 0;

            base.Initialize();
        }
      
        // Load Content
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            scene = new Scene(spriteBatch);

            //scene.AddPlatform(new Plataform(Content));
            player = new Player(Content, this);
            scene.AddSprite(player);

            // Fontes
            font = Content.Load<SpriteFont>("SpriteFont1");

            start = Content.Load<Texture2D>("start");

            // Fundos
            background = new ScrollingBackground(Content, "Backgrounds/Sky", -1f);
            scene.AddBackground(background);
            background = new ScrollingBackground(Content, "Backgrounds/Small Clouds", -1 / 10f);
            scene.AddBackground(background);
            background = new ScrollingBackground(Content, "Backgrounds/Mountains", -1 / 2f);
            scene.AddBackground(background);
            background = new ScrollingBackground(Content, "Backgrounds/Small Clouds", 1 / 5f);
            scene.AddBackground(background);
            background = new ScrollingBackground(Content, "Backgrounds/Forest", 1 / 4f);
            scene.AddBackground(background);
            background = new ScrollingBackground(Content, "Backgrounds/Sky Clouds", 1 / 3f);
            scene.AddBackground(background);

            
            scene.LoadContent();
        }

        // Unload Content
        protected override void UnloadContent()
        {
            scene.UnloadContent();
            spriteBatch.Dispose();
        }
        
        // Update
        protected override void Update(GameTime gameTime)
        {
            if (escPressed == true && Keyboard.GetState().IsKeyUp(Keys.Escape))
            { escPressed = false; }
            if (status == GameStatus.start)
            {

                // Saída do jogo
                if (escPressed == false && Keyboard.GetState().IsKeyDown(Keys.Escape) && status == GameStatus.start)
                    Exit();
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                { status = GameStatus.game; }
            }
          


            if (status == GameStatus.game)
            {


                if (player.timer >= 250) player.velocity = 1.6f;
                if (player.timer >= 500) player.velocity = 1.8f;
                if (player.timer >= 1000) player.velocity = 2f;

               
                if (escPressed == false  && Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                   
                    escPressed = true;
                    Restart();

                }
                // Geração de plataformas
                platformTime += gameTime.ElapsedGameTime.Milliseconds;
                if (platformTime >= 50)
                {
                    platformTime = 0;
                    CreatePlatform();
                }

                // Geração de inimigos
                enemyTime1 += gameTime.ElapsedGameTime.Milliseconds;
                enemyTime2 += gameTime.ElapsedGameTime.Milliseconds;
                if (enemyTime1 >= 5000)
                {
                    enemyTime1 = 0;
                    CreateEnemyBroccoli();
                }

                if (enemyTime2 >= 7000)
                {
                    enemyTime2 = 0;
                    CreateEnemyCorn();
                }

                if (platformPosition.X * Camera.Ratio < graphics.PreferredBackBufferWidth)
                    CreatePlatform();

                scene.Update(gameTime);
            }
            base.Update(gameTime); 
        }
        
        // Draw
        protected override void Draw(GameTime gameTime)
        {

            if (status == GameStatus.start)
            {
                spriteBatch.Begin();

                spriteBatch.Draw(start, new Vector2(0, 0), Color.White); 
                spriteBatch.End(); }

            if (status == GameStatus.game)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);

                scene.Draw(gameTime);

                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Score: " + player.timer.ToString("0"), new Vector2(600, 10), Color.White);
                spriteBatch.End();


            }

            base.Draw(gameTime);
        }

        
        // Geração aleatória de uma plataforma
        public void CreatePlatform()
        {
            // Controla o número mínimo de plataformas por altura
            if (platformCounter < 6)
            {
                randomPlatformHeight = 0;
                platformCounter++;
            }
            else
            {
                randomPlatformHeight = (random.Next(4) - 2);
                platformCounter = 0;
            }

            Platform plataforma = new Platform(Content);
            plataforma.position.X = platformPosition.X + (plataforma.size.X);
            plataforma.position.Y = platformPosition.Y + randomPlatformHeight;

            // Proíbe que as plataformas fiquem demasiado altas
            if (plataforma.position.Y < -1.7f)
                plataforma.position.Y = -1.7f;

            if (plataforma.position.Y > 200)
                plataforma.position.Y = 200;

            scene.AddSprite(plataforma);
            platformPosition = plataforma.position;

            randomLollipop = random.Next(100);

            if(randomLollipop > 95)
            {
                PowerUps lollipop = new PowerUps(Content, plataforma.position);
                scene.AddSprite(lollipop);
            }
        }

        // Geração aleatória de um inimigo
        public void CreateEnemyBroccoli()
        {
            enemyBroccoli = new EnemyBroccoli(Content);
            scene.AddSprite(enemyBroccoli);
            enemyBroccoli.position.X = player.position.X + 10;

        }
        public void CreateEnemyCorn()
        {
            enemyCorn = new EnemyCorn(Content);
            scene.AddSprite(enemyCorn);
            enemyCorn.position.X = player.position.X + 10;
        }

        public void Restart()
        {
            Initialize();
            LoadContent();
            status = GameStatus.start;
        }
    }
}
