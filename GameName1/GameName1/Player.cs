﻿using Microsoft.Xna.Framework;
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
        Sprite Collided;
        Vector2 CollisionPoint;
		// Construtor
        public Player(ContentManager content, String textureName)
            : base(content, textureName, 1, 4)
		{
			this.isJumping = false;
            this.position = new Vector2(8, 8);
			this.maxDistance = 3f;
			this.velocity = .5f;
			this.direction = Vector2.Zero;
            this.EnableCollisions();
		}

		// Update
		public override void Update(GameTime gameTime)
		{
			// Movimento para a direita automático
			this.position.X += 0.05f;

            if (!isJumping)
            {
                // Gravidade puxa para baixo
                this.position.Y -= 0.05f;

                if (this.scene.Collides(this, out this.Collided, out this.CollisionPoint))
                {
                    // Oops, colidimos. Vamos regressar para cima    
                    this.position.Y += 0.05f;

                    // se colidimos estamos no chao, logo, so nesta altura podemos ver se podemos saltar
                    KeyboardState keyState = Keyboard.GetState();
                    if (keyState.IsKeyDown(Keys.Up))
                        Jump();
                }
            }
            else
            {
                if ((position - sourcePosition).Length() <= maxDistance)
                {
                    position = position + direction * velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * 5;
                }
                else
                {
                    isJumping = false;
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
