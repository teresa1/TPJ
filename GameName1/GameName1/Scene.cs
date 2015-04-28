using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sugar_Run
{
    public class Scene
    {
        // Variáveis
        public SpriteBatch spriteBatch;
        private List<Sprite> spriteList;
        private List<Plataform> platformList;

        // Construtor
        public Scene(SpriteBatch spriteBatch)
        {
            this.SpriteBatch = spriteBatch;
            this.spriteList = new List<Sprite>();
            this.platformList = new List<Plataform>();
        }

        // Load Content
        public void LoadContent()
        { }

        // Unload Content
        public void UnloadContent()
        {
            foreach (var sprite in spriteList)
                sprite.Dispose();
            foreach (var platform in platformList)
                platform.Dispose();
        }

        // Update
        public void Update(GameTime gameTime)
        {
            foreach (var sprite in spriteList.ToList())
                sprite.Update(gameTime);
            foreach (var plat in platformList.ToList())
                plat.Update(gameTime);
        }

        // Draw
        public void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            if (spriteList.Count > 0)
            { 
                foreach (var sprite in spriteList)
                    sprite.Draw(gameTime);
            }
            if (platformList.Count > 0)
            {
                foreach (var platform in platformList)
                    platform.Draw(gameTime);
            }
            this.spriteBatch.End();
        }

        // Adiciona uma nova sprite à cena
        public void AddSprite(Sprite sprite)
        {
            this.spriteList.Add(sprite);
            sprite.SetScene(this);
        }

        // Adiciona uma nova plataforma à cena
        public void AddPlatform(Plataform platform)
        {
            this.platformList.Add(platform);
            platform.SetScene(this);
        }

        // Remove uma sprite da cena
        public void RemoveSprite(Sprite sprite)
        {
            this.spriteList.Remove(sprite);
        }

        // Remove uma plataforma da cena
        public void RemoveSprite(Plataform platform)
        {
            this.platformList.Remove(platform);
        }

        // Calcula as colisões de todas as sprites da cena (com colisões)
        public void Collides()
        {
         
            foreach (var sprite in spriteList)
            {
                sprite.IsFalling = true;
                foreach (var platform in platformList)
                {
                   
                    if (sprite.BoundingBox.Intersects(platform.BoundingBox))
                        sprite.IsFalling = false;
                }
            }
        }

        // Métodos get/set
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
            private set { spriteBatch = value; }
        }
    }
}
