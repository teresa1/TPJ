using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    class Cenário : Sprite
    {
        Vector2 Posicao2;
        Texture2D textura2;
        Vector2 posição;
        public float speed;
        public bool spawn;

        public Cenário(ContentManager content, SpriteBatch SpriteBatch, GraphicsDeviceManager Graphics, float speed)
            : base(content, "textura")
        {
            this.speed = speed;
        }

        public void update(GameTime gameTime)
        {
            if (position.X <= -1200)
            {
                position.X = Posicao2.X + 1200;
            }
            else if (Posicao2.X <= -1200)
            {
                Posicao2.X = position.X + 1200;
                spawn = true;
            }
            else
                spawn = false;

            position.X -= speed;
            Posicao2.X -= speed;
        }

        public void loadContent(string assetName, string assetName2)
        {
            base.loadContent(assetName);
            textura2 = content.Load<Texture2D>(assetName2);
            Posicao2 = new Vector2(1200, 0);
        }

        //public new void draw(int fade)
        //{
        //    SpriteBatch.Draw(texture, position, new Color(fade, fade, fade));
        //    SpriteBatch.Draw(textura2, Posicao2, new Color(fade, fade, fade));
        //}
    }
}
