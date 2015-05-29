using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Sugar_Run
{
	class Player : AnimatedSprite
	{
		// Variáveis
		ContentManager Content;
		private bool isJumping;
		private float maxDistance, velocity;
		private Vector2 sourcePosition;
		private Vector2 direction;
		// Colisões
		Sprite Collided;
		Vector2 CollisionPoint;
		// Burgers
		private float fireCounter = 0f;
		private float fireInterval = 0.5f;
		// Score
		public float timer = 0;
		// Sons
        SoundEffect JumpiT;
        SoundEffect ExplodeIT;
        SoundEffect Pickup;
        Game1 game1;
	   
		// Construtor
		public Player(ContentManager content, Game1 game1) : base(content, "CandyGirl", 1, 4)
		{
			this.Content = content;
			this.isJumping = false;
			this.position = new Vector2(4, 3);
			this.maxDistance = 2f;
			this.velocity = 0.6f;
			this.position = new Vector2(4, 3);
			this.maxDistance = 2f;
			this.velocity = 3/2f;
			this.direction = Vector2.Zero;
			this.EnableCollisions();
			this.name = "Girl";
            this.game1 = game1;
            JumpiT = Content.Load<SoundEffect>("Jump");
            ExplodeIT = Content.Load<SoundEffect>("Explosion1");
            Pickup = Content.Load<SoundEffect>("Pickup"); 

		}

		

		// Unload Content
		public override void UnloadContent()
		{
			//this.jumpSound.Dispose();

			base.UnloadContent();
		}
	  
		// Update
		public override void Update(GameTime gameTime)
		{
			timer += (float)gameTime.ElapsedGameTime.TotalSeconds * 10;
			KeyboardState keyboardState = Keyboard.GetState();

			// Movimento para a direita automático
			this.position.X += 0.05f;
			// Dispara burgers
			this.Shoot(gameTime, keyboardState);

			if (isJumping || !isJumping)
			{
				// Colisões
				if (this.scene.Collides(this, out this.Collided, out this.CollisionPoint))
				{
					// Se colidir com uma plataforma ou inimigo, explode e game over
					if (Collided.name == "Plataforma" || Collided.name == "enemy")
					{
                        ExplodeIT.Play();
						AnimatedSprite explosion = new AnimatedSprite(Content, "explosion", 1, 12);
						explosion.loop = false;
						explosion.SetPosition(this.position);
						explosion.Scale(1.5f);
						scene.AddSprite(explosion);
                        this.Destroy();

                        this.game1.status = Game1.GameStatus.start;
                        this.game1.Restart();
						// GAME OVER
					}

					// Se colidir com a plataforma enquanto salta, deixa de cair
					if (!isJumping && Collided.name == "Plataforma")
					{
						this.position.Y += 0.05f;

						// Se colidimos estamos no chao, logo, ness altura podemos saltar
						if (keyboardState.IsKeyDown(Keys.Up))
							Jump();
					}

					// Se colidir com um power up, ganha pontos
					if (Collided.name == "lollipop")
					{
                        Pickup.Play();
						AnimatedSprite sparkle;
						sparkle = new AnimatedSprite(content, "sparkle", 4, 8);
						scene.AddSprite(sparkle);
						sparkle.SetPosition(this.position);
						sparkle.Scale(.7f);
						sparkle.loop = false;
						Collided.Destroy();
						timer += 50;
					}
				
				}
			}

			if (!isJumping)
			{
				// Gravidade puxa para baixo
				this.position.Y -= 0.05f;

				if (this.scene.Collides(this, out this.Collided, out this.CollisionPoint))
				{
					// Oops, colidimos. Vamos regressar para cima 
                    if(this.Collided.name == "Plataforma")
					this.position.Y += 0.05f;
                    else
                    {
                        ExplodeIT.Play();
                        Collided.Destroy();
                        AnimatedSprite explosion = new AnimatedSprite(Content, "explosion", 1, 12);
						explosion.loop = false;
						explosion.SetPosition(this.position);
						explosion.Scale(1.5f);
                        scene.AddSprite(explosion);
                        
                    }
					// se colidimos estamos no chao, logo, so nesta altura podemos ver se podemos saltar
					if (keyboardState.IsKeyDown(Keys.Up))
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

		// Draw
		public override void Draw(GameTime gameTime)
		{
			if (isJumping)
				currentFrame = new Point(0, 0);
 
			base.Draw(gameTime);
		}

		// Faz o jogador saltar
		public void Jump()
		{
			this.isJumping = true;
			this.sourcePosition = position;
			this.direction = new Vector2((float)Math.Sin(rotation), (float)Math.Cos(rotation));
            JumpiT.Play();
		}

		// Dispara burgers
		public void Shoot(GameTime gameTime, KeyboardState keyboardState)
		{			
			// Dispara burgers 
			fireCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (fireCounter >= fireInterval && keyboardState.IsKeyDown(Keys.Space))
			{
				Vector2 burgerSourcePosition = new Vector2(this.position.X + 0.8f, this.position.Y);
				Burger burger = new Burger(content, burgerSourcePosition);
				scene.AddSprite(burger);
				fireCounter = 0f;
			}
		}

		// Seleciona a cena
		public override void SetScene(Scene scene)
		{
			this.scene = scene;
		}
	}
}
