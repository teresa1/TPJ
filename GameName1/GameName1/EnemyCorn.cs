using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Sugar_Run
{
    class EnemyCorn : AnimatedSprite
    {
        // Variáveis
        ContentManager Content;
        Random random = new Random();
        private Vector2 sourcePosition;
        private Vector2 direction;
        private float velocity;
        // Colisões
        Sprite Collided;
        Vector2 CollisionPoint;

        List<Platform> plataformas;
        public Platform p;
        public int direção = -1;

        // Construtor
        public EnemyCorn(ContentManager content) : base(content, "Enemies/Corn", 1, 2)
        {
            this.Content = content;
            this.position = new Vector2(15, 4);
            this.velocity = 1f;
            this.direction = Vector2.Zero;
            this.EnableCollisions();
            this.name = "enemy";
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
            this.position.X += 0.05f * direção;

            if (this.scene.Collides(this, out this.Collided, out this.CollisionPoint))
            {
                if (Collided.name == "Plataforma" || Collided.name == "lollipop")
                {
                    direção = -direção;
                }

                if (Collided.name == "burger")
                {
                    AnimatedSprite explosion = new AnimatedSprite(Content, "explosion", 1, 12);
                    explosion.loop = false;
                    explosion.SetPosition(this.position);
                    explosion.Scale(1.5f);
                    scene.AddSprite(explosion);
                    this.Destroy();
                    scene.RemoveSprite(explosion);
                }
            }
            
            base.Update(gameTime);
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

            int rand = (random.Next(4) - 2); // como chegar ao Lenght da plataformaaaaaaaaaaaaaaaaa? .-.

            this.position.X = p.position.X + rand;


        }
    }
}
