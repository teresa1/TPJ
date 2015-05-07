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

        // Construtor
        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // Definição do tamanho da janela
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();

            // Inicialização da câmara
            Camera.SetGraphicsDeviceManager(graphics);
            //Camera.SetTarget(new Vector2(0, 2));
            Camera.SetWorldWidth(15);

            base.Initialize();
        }
        
       public Random random = new Random();
      
        // Load Content
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            scene = new Scene(spriteBatch);

            //scene.AddPlatform(new Plataform(Content));
            player = new Player(Content, "CandyGirl");
            scene.AddSprite(player);

            
            
            
            Vector2 posiçãoPlat = new Vector2(0f, -1.5f);
           
      
            for (int i = 0; i < 100; i++)
            { 
                int rand = (random.Next(4)-2);
                Plataform p = new Plataform(Content);

                scene.AddSprite(p);
                p.position.X = posiçãoPlat.X + (p.size.X);
           
               p.position.Y = posiçãoPlat.Y + rand;

               if (p.position.Y < -1.5f)
                   p.position.Y = -1.5f;

              
               posiçãoPlat = p.position;

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
            for (int i = 0; i < 10; i++)
            {
                if(i == 9)
            scene.Update(gameTime);  
            }
            
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
