using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sugar_Run
{
    public class Platform : Sprite
    {
        // Construtor
        public Platform(ContentManager content) : base (content, "Plataforma")
        {
            this.position = new Vector2(0f, -1.7f);
            this.Scale(1f);
            this.EnableCollisions();
            this.name = "Plataforma";
        }
    }
}
