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
        //Variáveis
        public SpriteBatch SpriteBatch {get; private set;}
        // Lists
        public List<Sprite> spriteList;
        private List<ScrollingBackground> backgroundList;

        public Scene(SpriteBatch sb)
        {
            this.SpriteBatch = sb;
            this.spriteList = new List<Sprite>();
            this.backgroundList = new List<ScrollingBackground>();
        }

        public void AddSprite(Sprite s)
        {
            this.spriteList.Add(s);
            s.SetScene(this);
        }

        public void AddBackground(ScrollingBackground background)
        {
            this.backgroundList.Add(background);
            background.SetScene(this);
        }

        public void RemoveSprite(Sprite s)
        {
            this.spriteList.Remove(s);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var sprite in spriteList.ToList())
            {
                sprite.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (spriteList.Count > 0 || backgroundList.Count > 0)
            {
                this.SpriteBatch.Begin();
                // Desenha os fundos
                foreach (var background in backgroundList)
                    background.Draw(gameTime);
                
                // Desenha as sprites
                foreach (var sprite in spriteList)
                    sprite.Draw(gameTime);

                this.SpriteBatch.End();
            }
        }

        public bool Collides(Sprite s, out Sprite collided,
                                       out Vector2 collisionPoint)
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

   

        public void Dispose()
        {
            foreach (var sprite in spriteList)
                sprite.Dispose();
        }
    }
}
