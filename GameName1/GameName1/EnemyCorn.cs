using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace Sugar_Run
{
    class EnemyCorn : AnimatedSprite
    {
        // Variáveis
        ContentManager Content;
        Random random = new Random();
        private Vector2 sourcePosition;
        private float velocity;
        // Colisões
        Sprite Collided;
        Vector2 CollisionPoint;
        public bool isJumping;

        List<Platform> plataformas;
        public Platform platform;

        public int direction;

        SoundEffect som;
        // Construtor
        public EnemyCorn(ContentManager content) : base(content, "Enemies/Corn", 1, 2)
        {
            this.Content = content;
            this.name = "enemy";
            this.position = new Vector2(15f, 1f);
            this.velocity = 0.05f;
            this.direction = -1;
            this.Scale(1.2f);
            this.EnableCollisions();
            som = Content.Load<SoundEffect>("Explosion1");
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
            // Movimento para a esquerda automático
            this.position.X += velocity * direction;

            if (this.scene.Collides(this, out this.Collided, out this.CollisionPoint))
            {
                if (Collided.name == "Plataforma")
                {
                    direction = -direction;
                }

                if (Collided.name == "lollipop")
                {
                    this.position.Y -= 0.05f;
                    this.position.X += velocity * direction;
                }

                if (Collided.name == "burger")
                {
                    som.Play();
                    AnimatedSprite explosion = new AnimatedSprite(Content, "explosion", 1, 12);
                    explosion.loop = false;
                    explosion.SetPosition(this.position);
                    explosion.Scale(1.5f);
                    scene.AddSprite(explosion);
                    this.Destroy();
                }
            }
            if (!isJumping)
            {
                // Gravidade puxa para baixo
                this.position.Y -= 0.05f;

                if (this.scene.Collides(this, out this.Collided, out this.CollisionPoint))
                {
                    // Oops, colidimos. Vamos regressar para cima    
                    this.position.Y += 0.05f;
                }
            }
            base.Update(gameTime);
        }

        public void GerarAleatoriamente()
        {
            foreach (Sprite sprite in this.scene.spriteList)
            {
                if (sprite is Platform)
                {
                    this.plataformas.Add(this.platform);
                }
            }

            this.position.Y = platform.position.Y + 2.5f;
            int rand = (random.Next(4) - 2); 
            this.position.X = platform.position.X + rand;
        }
    }
}
