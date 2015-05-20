using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sugar_Run
{
    class AnimatedSprite: Sprite
    {
        private int cols,rows;
        public Point currentFrame;
        private float animationInterval = 1f / 10f;
        private float animationTimer = 0f;

        public bool Loop { get; set; }

        public AnimatedSprite(ContentManager content, string textureName, int rows, int cols) : base(content,textureName)
        {
            this.cols = cols;
            this.rows = rows;
            this.pixelsize.X = this.pixelsize.X / cols;
            this.pixelsize.Y = this.pixelsize.Y / rows;
            this.size = new Vector2(1f, (float)pixelsize.Y / (float)pixelsize.X);
            this.currentFrame = Point.Zero;
            Loop = true;
        }

        private void NextFrame()
        {
            if(currentFrame.X < cols - 1)
            {
                currentFrame.X++;
            }
            else if(currentFrame.Y < rows - 1)
            {
                currentFrame.X = 0;
                currentFrame.Y++;
            }
            else if (Loop)
            {
                currentFrame = Point.Zero;
            }
            else
            {
                Destroy();
            }
        }

        public override void Update(GameTime gameTime)
        {
            animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(animationTimer> animationInterval)
            {
                animationTimer = 0f;
                NextFrame();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
           
            source = new Rectangle((int)(currentFrame.X * pixelsize.X),
                (int)(currentFrame.Y * pixelsize.Y), (int)pixelsize.X,
                (int)pixelsize.Y);

            base.Draw(gameTime);
        }

        public override void EnableCollisions()
        {
            this.HasCollisions = true;

            this.radius = (float)Math.Sqrt(Math.Pow(size.X / 2, 2) +
                                           Math.Pow(size.Y / 2, 2));

            pixels = new Color[(int)(pixelsize.X * pixelsize.Y)];
            image.GetData<Color>(0, new Rectangle(
                    (int)(currentFrame.X * pixelsize.X),
                    (int)(currentFrame.Y * pixelsize.Y), 
                    (int)pixelsize.X,
                    (int)pixelsize.Y),
                 pixels, 0, 
                (int)(pixelsize.X * pixelsize.Y));
        }
    }
}
