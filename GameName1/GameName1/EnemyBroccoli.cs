using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Sugar_Run
{
    class EnemyBroccoli : AnimatedSprite
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

        List<Platform> plataformas;
        public Platform p;

        // Construtor
        public EnemyBroccoli(ContentManager content) : base(content, "Enemies/Broccoli", 1, 1)
        {
            this.Content = content;
            this.name = "enemy";
            this.isJumping = false;
            this.position = new Vector2(15, 4);
            this.velocity = 1f;
            this.direction = Vector2.Zero;
            this.EnableCollisions();
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

        int direção = -1;
        
        // Update    
        public override void Update(GameTime gameTime)
        {
            // Movimento para a esquerda automático
            this.position.X += 0.005f * direção;

            if (this.scene.Collides(this, out this.Collided, out this.CollisionPoint))
            {


                if (Collided.name == "Plataforma")
                {
                    direção = -direção;
                }

                if (Collided.name == "burger")
                {
                    AnimatedSprite e = new AnimatedSprite(Content, "explosion", 1, 12);
                    e.loop = false;
                    e.SetPosition(this.position);
                    e.Scale(1.5f);
                    scene.AddSprite(e);
                    this.Destroy();
                }

                if(Collided.name == "lollipop")
                {
                    this.position.Y -= 0.05f;
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
            foreach (Sprite s in this.scene.spriteList)
            {
                if (s is Platform)
                {
                    this.plataformas.Add(this.p);
                }
            }

            this.position.Y = p.position.Y;

            int rand = (random.Next(4) - 2);

            this.position.X = p.position.X + rand;


        }
    }
}
