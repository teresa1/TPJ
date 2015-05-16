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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Scene scene;
        ScrollingBackground background;
        Player player;
        Enemy enemy;
        Vector2 posiçãoPlataforma;
        Random random = new Random();
        float timePlat;
        float timeEnemy;
        int platformCounter;
        int randomPlatformHeight;
        int randomLollipop;


        
        // Construtor
        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // Definição do tamanho da janela
            graphics.PreferredBackBufferHeight = 520;
            graphics.PreferredBackBufferWidth = 800;
            graphics.ApplyChanges();

            // Inicialização da câmara
            Camera.SetGraphicsDeviceManager(graphics);
            //Camera.SetTarget(new Vector2(0, 2));
            Camera.SetWorldWidth(15);

            posiçãoPlataforma = new Vector2(6, -1.5f);
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
          

            background = new ScrollingBackground(Content, "background", 0);
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

            timePlat += gameTime.ElapsedGameTime.Milliseconds;
            if (timePlat >= 50)
            {
                timePlat = 0;
                CreatePlatform();
               
            }

            timeEnemy += gameTime.ElapsedGameTime.Milliseconds;
            if(timeEnemy >= 5000)
            {
                timeEnemy = 0;
                CreateEnemy();
            }

            if (posiçãoPlataforma.X* Camera.Ratio < graphics.PreferredBackBufferWidth)
                CreatePlatform();

            scene.Update(gameTime);
          
            base.Update(gameTime); 
        }
        
        // Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            scene.Draw(gameTime);

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
           
            plataforma.position.X = posiçãoPlataforma.X + (plataforma.size.X);
            plataforma.position.Y = posiçãoPlataforma.Y + randomPlatformHeight;

            // Não deixa que as plataformas fiquem muito altas
            if (plataforma.position.Y < -1.5f)
                plataforma.position.Y = -1.5f;

            scene.AddSprite(plataforma);
            posiçãoPlataforma = plataforma.position;

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
