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
		ContentManager Content;
		private bool isJumping;
        private bool isShooting;
		private float maxDistance, velocity;
		private Vector2 sourcePosition;
		private Vector2 direction;
		Sprite Collided;
		Vector2 CollisionPoint;
        private float fireCounter = 0f;
        private float fireInterval = 0.2f;
        PowerUps lollipop;
        public float timer = 0;
	   
		// Construtor
		public Player(ContentManager content, String textureName) : base(content, textureName, 1, 4)
		{
			this.Content = content;
			this.isJumping = false;
            this.isShooting = false;
			this.position = new Vector2(4, 3);
			this.maxDistance = 2f;
			this.velocity = 1f;
			this.direction = Vector2.Zero;
			this.EnableCollisions();
            this.name = "Girl";
		}
      
		// Update
		public override void Update(GameTime gameTime )
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds * 10;
			// Movimento para a direita automático
			this.position.X += 0.05f;
            KeyboardState keyState = Keyboard.GetState();

            

            if (isJumping || !isJumping)
            {
                if (this.scene.Collides(this, out this.Collided, out this.CollisionPoint))
                {
                    if (Collided.name == "Plataforma" || Collided.name == "enemy")
                    {
                        AnimatedSprite e = new AnimatedSprite(Content, "explosion", 1, 12);
                        e.Loop = false;
                        e.SetPosition(this.position);
                        e.Scale(1.5f);
                        scene.AddSprite(e);
                        this.Destroy();
                    }
                }
                if (!isJumping && Collided.name == "Plataforma")
                {
                     this.position.Y += 0.05f;

                    // se colidimos estamos no chao, logo, so nesta altura podemos ver se podemos saltar
                    if (keyState.IsKeyDown(Keys.Up))
                        Jump();
                }

                if(Collided.name == "lollipop")
                {
                    AnimatedSprite sparkle;
                    sparkle = new AnimatedSprite(cManager, "sparkle", 4, 8);
                    scene.AddSprite(sparkle);
                    sparkle.SetPosition(this.position);
                    sparkle.Scale(.7f);
                    sparkle.Loop = false;
                    Collided.Destroy();
                    timer += 50;
                }
                
            }

            //Disparar Burgers!! :D
            fireCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (fireCounter >= fireInterval && keyState.IsKeyDown(Keys.Space))
            {
                Vector2 pos;
                pos.X = this.position.X + 1f;
                pos.Y = this.position.Y;
                Burger bullet = new Burger(cManager, pos);
                scene.AddSprite(bullet);
                fireCounter = 0f;
            }

			if (!isJumping)
			{
				// Gravidade puxa para baixo
				this.position.Y -= 0.05f;

                if (this.scene.Collides(this, out this.Collided, out this.CollisionPoint) || Collided.name == "lollipop")
                {
                    // Oops, colidimos. Vamos regressar para cima    
                    this.position.Y += 0.05f;
                    
                    // se colidimos estamos no chao, logo, so nesta altura podemos ver se podemos saltar
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

        public void Shoot()
        {
            this.isShooting = true;
        }

		public override void SetScene(Scene s)
		{
			this.scene = s;
		}
	}
}
