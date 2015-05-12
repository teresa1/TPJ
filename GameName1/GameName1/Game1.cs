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

            base.Initialize();
        }
      
        // Load Content
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            scene = new Scene(spriteBatch);

            //scene.AddPlatform(new Plataform(Content));
            player = new Player(Content, "CandyGirl");
            enemy = new Enemy(Content, "inimigo1");
            scene.AddSprite(player);
            scene.AddSprite(enemy);

            // Geração aleatória de Plataformas
            posiçãoPlataforma = new Vector2(0f, -1.5f);
            for (int i = 0; i < 100; i++)
            { 
                int rand = (random.Next(4)-2);
                Plataform p = new Plataform(Content);

                scene.AddSprite(p);
                p.position.X = posiçãoPlataforma.X + (p.size.X);
           
               p.position.Y = posiçãoPlataforma.Y + 0*rand;

               if (p.position.Y < -1.5f)
                   p.position.Y = -1.5f;

              
               posiçãoPlataforma = p.position;
            }
        
            background = new ScrollingBackground(Content);
        }

        // Unload Content
        protected override void UnloadContent()
        {
            spriteBatch.Dispose();
        }
        
        // Update
        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
           
              if (scene.sprites.Contains(player))
            {
                background.Update(gameTime);
            } 
            scene.Update(gameTime);  
            
            
            base.Update(gameTime); 
        }
        
        // Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            background.Draw(gameTime, spriteBatch);
            scene.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
