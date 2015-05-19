using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sugar_Run
{
    public class ScrollingBackground
    {
        // Variáveis
        private Scene scene;
        private Texture2D texture;
        private Vector2 position;
        // World size
        private Vector2 size;
        // Centro da imagem em pixels
        private Vector2 origin;
        private Vector2 lastCameraPosition;
        // Velocidade de deslizamento
        private float speedRatio;

        // Construtor
        public ScrollingBackground(ContentManager cManager, string textureName, float speedRatio)
        {
            this.texture = cManager.Load<Texture2D>(textureName);
            this.origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            this.lastCameraPosition = Camera.GetTarget();
            this.position = Camera.GetTarget();
            this.size = new Vector2(2 * Camera.WorldWidth, 2 * Camera.WorldWidth * texture.Height / texture.Width);
            this.speedRatio = speedRatio;
        }

        // Load Content
        public void LoadContent() { }

        // Unload Content
        public void UnloadContent() 
        {
            texture.Dispose();
        }

        // Update
        public void Update() { }

        // Draw
        public void Draw(GameTime gameTime)
        {
            Vector2 movement = lastCameraPosition - Camera.GetTarget();
            position = position + speedRatio * movement;
            lastCameraPosition = Camera.GetTarget();

            int xMin, xMax, yMin, yMax;
            Rectangle destination = Camera.WorldSize2PixelRectangle(position, size);

            xMin = -(int)Math.Ceiling((destination.X - 0.5f * destination.Width) / destination.Width);
            yMin = -(int)Math.Ceiling((destination.Y - 0.5f * destination.Height) / destination.Height);

            xMax = (int)Math.Ceiling((Camera.GraphicsDevManager.PreferredBackBufferWidth - destination.X - destination.Width * .5f) / destination.Width); 
            yMax = (int)Math.Ceiling((Camera.GraphicsDevManager.PreferredBackBufferHeight - destination.Y - destination.Height * .5f) / destination.Height);


            for (int i = xMin; i <= xMax; i++)
                for (int j = yMin; j <= yMax; j++)
                {
                    Rectangle drawDestination;
                    drawDestination = new Rectangle(destination.X + i * destination.Width, destination.Y + j * destination.Height, destination.Width, destination.Height);
                    scene.SpriteBatch.Draw(texture, drawDestination, null, Color.White, 0f, origin, SpriteEffects.None, 0);
                }
        }

        public void SetScene(Scene scene)
        {
            this.scene = scene;
        }

        //public Texture2D texture1, texture2;
        //public Vector2 posText1, posTex2;

        //public ScrollingBackground(ContentManager content)
        //{
           
        //    texture1 = content.Load<Texture2D>("Background");
        //    texture2 = texture1; //poupar processamento ^_^

        //    posText1 = new Vector2(0,0);
        //    posTex2 = new Vector2(texture1.Width, 0); 
        //}

        //public void Update(GameTime gameTime)
        //{

        //    this.posText1.X -= 2; //isto faz andar os backgrounds
        //    this.posTex2.X -= 2;
              
        //    // isto faz saltar imagens
        //    if(posTex2.X == -texture1.Width ) 
        //    {
        //        posTex2.X = texture1.Width;
        //    }
        //    if (posText1.X == -texture2.Width) 
        //    {
        //        posText1.X = texture2.Width; 
        //    }

        //}

        //public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Begin();

        //    spriteBatch.Draw(texture1, posText1, Color.White);
        //    spriteBatch.Draw(texture2, posTex2, Color.White);

        //    spriteBatch.End();
        //}
    }
}
