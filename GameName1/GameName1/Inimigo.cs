using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sugar_Run
{
    class Inimigo : Sprite
    {


        ContentManager Content;
        Sprite Collided;
        Vector2 CollisionPoint;
        Random random = new Random();
         public Inimigo(ContentManager content) : base (content, "inimigo1")
        {
            this.position = new Vector2(0f, -1.5f);
            this.Scale(10f);
            this.EnableCollisions();
            this.Content = content;
            
        }


         public override void Update(GameTime gameTime)
         {
             // Movimento para a direita automático
             this.position.X += 0.05f;
            
             if (this.scene.Collides(this, out this.Collided, out this.CollisionPoint))
             {

                 AnimatedSprite e = new AnimatedSprite(Content, "explosion", 1, 12);
                 e.Loop = false;
                 e.SetPosition(this.position);
                 e.Scale(1.5f);
                 scene.AddSprite(e);
                 this.Destroy();
             }
         }


        
        List<Plataform> plataformas;
        public Plataform p;

        public void GerarAleatoriamente()
         {
             foreach (Sprite s in this.scene.sprites)
             {
                 if(s is Plataform)
                 {
                     this.plataformas.Add(this.p); 
                 }
             }
             
                this.position.Y = p.position.Y;
        
                int rand = (random.Next(4)-2); // como chegar ao Lenght da plataformaaaaaaaaaaaaaaaaa? .-.

                this.position.X = p.position.X + rand;


         }
    }
}
