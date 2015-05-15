using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sugar_Run
{
    class PowerUps : Sprite
    {
       
        Sprite Collided;

        Vector2 CollisionPoint;


        public PowerUps(ContentManager cManager, Vector2 sourcePosition) : base(cManager, "lollipop")
        {
            this.position = sourcePosition;
            this.position.Y += 1;
            this.EnableCollisions();
            this.Scale(.5f);
            this.name = "lollipop";
        }

        public override void Update(GameTime gameTime)
        {
            if (this.scene.Collides(this, out this.Collided, out this.CollisionPoint))
            {
                this.Destroy();
            }
        }

        public override void Destroy()
        {
            AnimatedSprite explosion;
            explosion = new AnimatedSprite(cManager, "sparkle", 4, 8);
            scene.AddSprite(explosion);
            explosion.SetPosition(this.position);
            explosion.Scale(.3f);
            explosion.Loop = false;
            base.Destroy();
        }





    }
}
