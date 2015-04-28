using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sugar_Run
{
    class ScrollingBackground
    {
        public Texture2D texture1, texture2;
        public Vector2 posText1, posTex2;

        public ScrollingBackground(ContentManager content)
        {
           
            texture1 = content.Load<Texture2D>("Background");
            texture2 = texture1; //poupar processamento ^_^

            posText1 = new Vector2(0,0);
            posTex2 = new Vector2(texture1.Width, 0); 
        }

        public void Update(GameTime gameTime)
        {

            this.posText1.X -= 2; //isto faz andar os backgrounds
            this.posTex2.X -= 2;
              
            // isto faz saltar imagens
            if(posTex2.X == -texture1.Width ) 
            {
                posTex2.X = texture1.Width;
            }
            if (posText1.X == -texture2.Width) 
            {
                posText1.X = texture2.Width; 
            }

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(texture1, posText1, Color.White);
            spriteBatch.Draw(texture2, posTex2, Color.White);

            spriteBatch.End();
        }
    }
}
