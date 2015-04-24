using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    public class Scene
    {
        // Variáveis
        public SpriteBatch spriteBatch;
        private List<Sprite> spriteList;

        // Construtor
        public Scene(SpriteBatch spriteBatch)
        {
            this.SpriteBatch = spriteBatch;
            this.spriteList = new List<Sprite>();
        }

        // Update
        public void Update(GameTime gameTime)
        {
            foreach (var sprite in spriteList.ToList())
                sprite.Update(gameTime);
        }

        // Draw
        public void Draw(GameTime gameTime)
        {
            if (spriteList.Count > 0)
            {
                this.spriteBatch.Begin();
                foreach (var sprite in spriteList)
                    sprite.Draw(gameTime);
                this.spriteBatch.End();
            }
        }

        // Adiciona uma nova sprite à cena
        public void AddSprite(Sprite sprite)
        {
            this.spriteList.Add(sprite);
            sprite.SetScene(this);
        }




        // Remove uma sprite da cena
        public void RemoveSprite(Sprite sprite)
        {
            this.spriteList.Remove(sprite);
        }

        // Dispose
        public void Dispose()
        {
            foreach (var sprite in spriteList)
                sprite.Dispose();
        }

        // Métodos get/set
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
            private set { spriteBatch = value; }
        }

       
    }
}
