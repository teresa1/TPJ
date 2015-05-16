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
            this.Scale(.7f);
            this.name = "lollipop";
        }


    }
}
