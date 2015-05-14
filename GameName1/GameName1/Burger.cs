using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sugar_Run
{
    class Burger : Sprite
    {
        
        public float maxDistance = 15;
        public float velocity = 4;
        private Vector2 sourcePosition;
        private Vector2 direction;
      

        public Burger(ContentManager cManager,
                      Vector2 sourcePosition)
            : base(cManager, "burger")
        {
            this.position = sourcePosition;
            this.EnableCollisions();
            this.sourcePosition = sourcePosition;
            this.Scale(.5f);
            this.direction = new Vector2(1,0);
        }

        public override void Update(GameTime gameTime)
        {
            position = position + direction * velocity *
                  (float)gameTime.ElapsedGameTime.TotalSeconds * 2;

            if ((position - sourcePosition).Length() > maxDistance) 
            {
                this.Destroy();
            }


            base.Update(gameTime);
        }

        public override void Destroy()
        {
            AnimatedSprite explosion;
            explosion = new AnimatedSprite(cManager, "explosion", 1, 12);
            scene.AddSprite(explosion);
            explosion.SetPosition(this.position);
            explosion.Scale(.3f);
            explosion.Loop = false;
            base.Destroy();
        }

    }
}
