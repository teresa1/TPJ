using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    class Player : AnimatedSprite
    {
        // Construtor
        public Player(ContentManager content, String textureName) : base(content, textureName, 1, 4)
        {

        }

        // Draw
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        // Update
        public override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.D))
            { 
                this.position.X += 1f; 
            }
            base.Update(gameTime);
        }
    }
}
