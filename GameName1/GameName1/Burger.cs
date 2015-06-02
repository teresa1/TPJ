using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        // Variáveis
        public float maxDistance = 15f;
        public float velocity = 5f;
        private Vector2 sourcePosition;
        private Vector2 direction;

        
        // Construtor
        public Burger(ContentManager cManager, Vector2 sourcePosition) : base(cManager, "burger")
        {
            this.position = sourcePosition;
            this.sourcePosition = sourcePosition;
            this.direction = new Vector2(1,0);
            this.name = "burger";
            this.Scale(.5f);
            this.EnableCollisions();
        }

        // Load Content
        public override void LoadContent()
        {
            base.LoadContent();
        }

        // Unload Cotent
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        // Update
        public override void Update(GameTime gameTime)
        {
            position = position + direction * velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * 2;

         

            base.Update(gameTime);
        }

        public override void Destroy()
        {
            
            AnimatedSprite explosion;
            explosion = new AnimatedSprite(content, "explosion", 1, 12);
            scene.AddSprite(explosion);
            explosion.SetPosition(this.position);
            explosion.Scale(.3f);
            explosion.loop = false;
            base.Destroy();
        }

    }
}
