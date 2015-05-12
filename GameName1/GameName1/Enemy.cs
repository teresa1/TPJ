using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sugar_Run
{
    class Enemy : AnimatedSprite
    {
        // Variáveis
        ContentManager Content;
        public bool isJumping;
        private float velocity;
        private Vector2 sourcePosition;
        private Vector2 direction;
        Sprite Collided;
        Vector2 CollisionPoint;
        Random random = new Random();

        List<Plataform> plataformas;
        public Plataform p;

        // Construtor
        public Enemy(ContentManager content, String textureName)
            : base(content, textureName, 1, 1)
        {
            this.Content = content;
            this.isJumping = false;
            this.position = new Vector2(56, 1);
            this.velocity = 1f;
            this.direction = Vector2.Zero;
            this.EnableCollisions();
        }

        // Update    
        public override void Update(GameTime gameTime)
        {
            // Movimento para a esquerda automático
            this.position.X -= 0.05f;

            if (this.scene.Collides(this, out this.Collided, out this.CollisionPoint))
            {
                AnimatedSprite e = new AnimatedSprite(Content, "explosion", 1, 12);
                e.Loop = false;
                e.SetPosition(this.position);
                e.Scale(1.5f);
                scene.AddSprite(e);
                this.Destroy();
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
            else
            {

            }
            
            base.Update(gameTime);
        }

        // Draw
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public void GerarAleatoriamente()
        {
            foreach (Sprite s in this.scene.sprites)
            {
                if (s is Plataform)
                {
                    this.plataformas.Add(this.p);
                }
            }

            this.position.Y = p.position.Y;

            int rand = (random.Next(4) - 2); // como chegar ao Lenght da plataformaaaaaaaaaaaaaaaaa? .-.

            this.position.X = p.position.X + rand;


        }
    }
}
