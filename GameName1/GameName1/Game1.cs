#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
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
        Enemy enemy;
        // Plataformas
        Vector2 platformPosition;
        float platformTime;
        int platformCounter;
        int randomPlatformHeight;
        // Inimigos
        float enemyTime;
        // Power-Ups
        int randomLollipop;

        SpriteFont font;
       
        

        // Construtor
        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // Definição do tamanho da janela
            graphics.PreferredBackBufferHeight = 450;
            graphics.PreferredBackBufferWidth = 800;
            graphics.IsFullScreen = true;
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
            player = new Player(Content, "CandyGirl");
            scene.AddSprite(player);


            font = Content.Load<SpriteFont>("SpriteFont1");

            // Backgrounds
            //background = new ScrollingBackground(Content, "Backgrounds/Sky", 0f);
            //scene.AddBackground(background);
            background = new ScrollingBackground(Content, "Backgrounds/Small Clouds", 1 / 10f);
            background = new ScrollingBackground(Content, "Backgrounds/Sky", 0f);
            scene.AddBackground(background);
            background = new ScrollingBackground(Content, "Backgrounds/Small Clouds", 1/5f);
            scene.AddBackground(background);
            background = new ScrollingBackground(Content, "Backgrounds/Forest", 0f);
            scene.AddBackground(background);
            background = new ScrollingBackground(Content, "Backgrounds/Sky Clouds", 1 / 5f);
            scene.AddBackground(background);
        }

        // Unload Content
        protected override void UnloadContent()
        {
            scene.Dispose();
            spriteBatch.Dispose();
        }
        
        // Update
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            platformTime += gameTime.ElapsedGameTime.Milliseconds;
            if (platformTime >= 50)
            {
                platformTime = 0;
                CreatePlatform();
               
            }

            enemyTime += gameTime.ElapsedGameTime.Milliseconds;
            if(enemyTime >= 5000)
            {
                enemyTime = 0;
                CreateEnemy();
            }

            if (platformPosition.X* Camera.Ratio < graphics.PreferredBackBufferWidth)
                CreatePlatform();

            scene.Update(gameTime);
          
            base.Update(gameTime); 
        }
        
        // Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            scene.Draw(gameTime);
            
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Score: " + player.timer.ToString("0"), new Vector2(600, 10), Color.Black);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        
        // Geração aleatória de plataformas
        public void CreatePlatform()
        {
            // Controla o número mínimo de plataformas por altura
            if (platformCounter < 3)
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

            // Não deixa que as plataformas fiquem muito altas
            if (plataforma.position.Y < -1.5f)
                plataforma.position.Y = -1.5f;

            scene.AddSprite(plataforma);
            platformPosition = plataforma.position;

            randomLollipop = random.Next(100);

            if(randomLollipop > 95)
            {
                PowerUps lollipop = new PowerUps(Content, plataforma.position);
                scene.AddSprite(lollipop);
            }
        }

        public void CreateEnemy()
        {
            enemy = new Enemy(Content, "inimigo1");
            scene.AddSprite(enemy);
            enemy.position.X = player.position.X + 10;
        }
    }
}
