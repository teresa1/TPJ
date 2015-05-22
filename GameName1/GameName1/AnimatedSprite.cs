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
        // Variáveis
        private int cols,rows;
        public Point currentFrame;
        private float animationInterval = 1f / 10f;
        private float animationTimer = 0f;
        public bool loop;

        // Construtor
        public AnimatedSprite(ContentManager content, string textureName, int rows, int cols) : base(content, textureName)
        {
            this.cols = cols;
            this.rows = rows;
            this.pixelSize.X = this.pixelSize.X / cols;
            this.pixelSize.Y = this.pixelSize.Y / rows;
            this.size = new Vector2(1f, (float)pixelSize.Y / (float)pixelSize.X);
            this.currentFrame = Point.Zero;
            loop = true;
        }

        // Load Content
        public override void LoadContent()
        {
            base.LoadContent();
        }

        // Unload Content
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        // Update
        public override void Update(GameTime gameTime)
        {
            animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Controla o tempo de cada frame
            if(animationTimer> animationInterval)
            {
                animationTimer = 0f;
                NextFrame();
            }

            base.Update(gameTime);
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
            if (currentFrame.X < cols - 1)
            {
                currentFrame.X++;
            }
            else if (currentFrame.Y < rows - 1)
            {
                currentFrame.X = 0;
                currentFrame.Y++;
            }
            else if (loop)
            {
                currentFrame = Point.Zero;
            }
            else
            {
                Destroy();
            }
        }

        // Ativa as colisões e cria a "bounding circle"
        public override void EnableCollisions()
        {
            this.hasCollisions = true;

            this.radius = (float)Math.Sqrt(Math.Pow(size.X / 2, 2) + Math.Pow(size.Y / 2, 2));

            pixels = new Color[(int)(pixelSize.X * pixelSize.Y)];
            texture.GetData<Color>(0, new Rectangle((int)(currentFrame.X * pixelSize.X), (int)(currentFrame.Y * pixelSize.Y), (int)pixelSize.X, (int)pixelSize.Y),
                                   pixels, 0, (int)(pixelSize.X * pixelSize.Y));
        }

        // Métodos get/set
        public bool Loop
        {
            get { return loop; }
            private set { loop = value; }
        }
    }
}
