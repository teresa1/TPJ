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

		// Construtor
        public Player(ContentManager content, String textureName)
            : base(content, textureName, 1, 4)
		{
			this.isJumping = false;
            this.isFalling = true;
            this.position = new Vector2(0, 0);
			this.maxDistance = 4f;
			this.velocity = .5f;
			this.direction = Vector2.Zero;
            this.EnableCollisions();
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
           
			KeyboardState keyState = Keyboard.GetState();
			if (keyState.IsKeyDown(Keys.Up) && isJumping == false)
				Jump();

			// Movimento de salto
			if (isJumping)
			{
				if ((position - sourcePosition).Length() <= maxDistance)
				{
					position = position + direction * velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * 5;
				}
				else
				{
					position = position - direction * velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * 5;
					if (position.Y <= sourcePosition.Y)
					{
						position.Y = 0f;
						isJumping = false;
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
	}
}
