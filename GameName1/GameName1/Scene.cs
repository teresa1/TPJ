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
        private List<ScrollingBackground> backgroundList;
        public List<Sprite> spriteList;

        // Construtor
        public Scene(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            this.spriteList = new List<Sprite>();
            this.backgroundList = new List<ScrollingBackground>();
        }

        // Load Content

        public void LoadContent()
        {
            foreach (var background in backgroundList)
                background.LoadContent();
            foreach (var sprite in spriteList)
                sprite.LoadContent();
        }

        // Unload Content
        public void UnloadContent()
        {
            foreach (var background in backgroundList)
                background.UnloadContent();
            foreach (var sprite in spriteList)
                sprite.UnloadContent();
        }

        // Update
        public void Update(GameTime gameTime)
        {
            foreach (var background in backgroundList)
                background.Update();
            foreach (var sprite in spriteList.ToList())
                sprite.Update(gameTime);
        }

        // Draw
        public void Draw(GameTime gameTime)
        {
            if (spriteList.Count > 0 || backgroundList.Count > 0)
            {
                this.spriteBatch.Begin();
                // Desenha os fundos
                foreach (var background in backgroundList)
                    background.Draw(gameTime);

                // Desenha as sprites
                foreach (var sprite in spriteList)
                    sprite.Draw(gameTime);

                this.spriteBatch.End();
            }
        }
        // Adiciona um novo fundo à cena
        public void AddBackground(ScrollingBackground background)
        {
            this.backgroundList.Add(background);
            background.SetScene(this);
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
        // Remove um  background da cena
        public void RemoveBackgrond(ScrollingBackground background)
        {
            this.backgroundList.Remove(background);
        }

        // Calcula as colisões de todas as sprites da cena (com colisões ativas)
        public bool Collides(Sprite s, out Sprite collided, out Vector2 collisionPoint)
        {
            bool collisionExists = false;
            collided = s;  // para calar o compilador
            collisionPoint = Vector2.Zero; // para calar o compilador

            foreach (var sprite in spriteList)
            {
                if (s == sprite) continue;
                if (s.CollidesWith(sprite, out collisionPoint))
                {
                    collisionExists = true;
                    collided = sprite;
                    break;
                }
            }
            return collisionExists;
        }

        // Métodos get/set
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
            private set { spriteBatch = value; }
        }
    }
}
