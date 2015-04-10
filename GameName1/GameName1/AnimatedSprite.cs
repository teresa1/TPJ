using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameName1
{
    public class AnimatedSprite : Sprite
    {
        public int rows;
        public int columns;
        private int currentFrame;
        private int totalFrames;

        //Declarar primeiro a animated sprite com esta função (imagem,linhas,colunas)
        public AnimatedSprite(ContentManager content, String textureName, int rows, int columns) : base(content, textureName)
        {
            this.rows = rows;
            this.columns = columns;
            currentFrame = 0;
            totalFrames = rows * columns;
        }

        public override void Update(GameTime gameTime)
        {
            currentFrame++;
            if (currentFrame == totalFrames)
                currentFrame = 0;

            base.Update(gameTime);
        }

        //Usar esta função para desenhar (spriteBatch, coordenadas)
        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = texture.Width / columns;
            int height = texture.Height / rows;
            int row = (int)((float)currentFrame / (float)columns);
            int column = currentFrame % columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            //spriteBatch.Begin();
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
            //spriteBatch.End();
        }
    }
}
