using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sugar_Run
{
	class Player : AnimatedSprite
	{
		// Variáveis
		private bool isJumping;
		private float maxDistance, velocity;
		private Vector2 sourcePosition;
		private Vector2 direction;
        bool isFalling;
        Sprite Collided;
        Vector2 CollisionPoint;
		// Construtor
        public Player(ContentManager content, String textureName)
            : base(content, textureName, 1, 4)
		{
			this.isJumping = false;
            this.isFalling = true;
            this.position = new Vector2(8, 8);
			this.maxDistance = 1.5f;
			this.velocity = .5f;
			this.direction = Vector2.Zero;
            this.EnableCollisions();
		}

        public bool IsFalling
        {
            get { return isFalling; }
            set { isFalling = value; }
        }
		// Draw
		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
		}

		// Update
		public override void Update(GameTime gameTime)
		{
			// Movimento para a direita automático
			this.position.X += 0.05f;

            if (!this.scene.Collides(this, out this.Collided, out this.CollisionPoint))
            {
                
                    this.position.Y -= 0.05f;
                   // this.isFalling = false;
                
            }

			KeyboardState keyState = Keyboard.GetState();
			if (keyState.IsKeyDown(Keys.Up) && isJumping == false)
				Jump();

            Console.WriteLine(this.position.X + "  "+this.position.Y);
			// Movimento de salto
			if (isJumping)
			{
               // this.isFalling = false;
               
				if ((position.Y - sourcePosition.Y) <= maxDistance)
				{ position.Y += position.Y * 0.1f;
					position = position + direction * velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * 5;
                    this.isFalling = true;
                    position.Y -= position.Y * 5f;
				}
				else
				{
					position = position - direction * velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * 5;
					if (position.Y <= sourcePosition.Y)
					{
						position.Y = 0f;
						isJumping = false;
                        this.isFalling = true;
					}
				}
             
			}
			
			// Acompanhamento da câmara com o jogador
			Camera.SetTarget(this.position);

			base.Update(gameTime);
		}

		// Inicializa as variáveis a serem usadas para saltar
		public void Jump()
		{
			this.isJumping = true;
			this.sourcePosition = position;
			this.direction = new Vector2((float)Math.Sin(rotation), (float)Math.Cos(rotation));
		}

        public override void SetScene(Scene s)
        {
            this.scene = s;
        }
	}
}
