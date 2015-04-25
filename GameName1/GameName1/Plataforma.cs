using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    class Plataforma : Sprite
    {
        // Construtor
        public Plataforma(ContentManager content) : base (content, "Plataforma")
        {
            this.position = new Vector2(7f, -1.5f);
            this.SetScale(10f);
            this.EnableCollisions();
        }

        public void repete()
        {

        }
    }
}
