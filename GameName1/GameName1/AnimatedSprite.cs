using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameName1
{
    public class AnimatedSprite : Sprite
    {
        // Variáveis
        private int rows, columns;
        private Point currentFrame;
        private float animationInterval = 1f / 10f;
        private float animationTimer = 0f;

        // Construtor
        public AnimatedSprite(ContentManager content, String textureName, int rows, int columns) : base(content, textureName)
        {
            this.rows = rows;
            this.columns = columns;
            this.pixelSize.X = this.pixelSize.X / columns;
            this.pixelSize.Y = this.pixelSize.Y / rows;
            this.size = new Vector2(1f, (float)pixelSize.Y / (float)pixelSize.X);
            this.currentFrame = Point.Zero;
        }

        // Update
        public override void Update(GameTime gameTime)
        {
            animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Controla o tempo de cada frame
            if (animationTimer > animationInterval)
            {
                animationTimer = 0f;
                NextFrame();
            }
            base.Update(gameTime);

            // Faz a bounding box seguir a sprite
            if (this.HasCollisions)
                this.boundingBox = new Rectangle((int)(position.X + 0.5f), (int)(position.Y + 0.5f), texture.Width / columns, texture.Height / rows);
        }

        // Draw
        public override void Draw(GameTime gameTime)
        {
            source = new Rectangle((int)(currentFrame.X * pixelSize.X), (int)(currentFrame.Y * pixelSize.Y), (int)pixelSize.X, (int)pixelSize.Y);
            base.Draw(gameTime);
        }

        // Passa para a próxima frame da spritesheet
        private void NextFrame()
        {
            if (currentFrame.X < columns - 1)
                currentFrame.X++;
            else if (currentFrame.Y < rows - 1)
            {
                currentFrame.X = 0;
                currentFrame.Y++;
            }
            else currentFrame = Point.Zero;
        }

        // Ativa as colisões e cria a bounding box
        public override void EnableCollisions()
        {
            this.HasCollisions = true;
            this.boundingBox = new Rectangle((int)(position.X + 0.5f), (int)(position.Y + 0.5f), texture.Width / columns, texture.Height / rows);
        }
    }
}
