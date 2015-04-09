using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    class Sprite
    {
        // Variáveis
        protected ContentManager cManager;
        protected Scene scene;
        //Sprite
        protected Texture2D image;
        protected Vector2 position;
        protected Vector2 size;
        protected Vector2 pixelSize;
        protected float rotation;
        protected Rectangle? source = null;
        // Colisões
        protected bool hasCollisions;
        // Raio da "bounding box"
        protected float radius;
        protected Color[] pixels;

        // Construtor
        public Sprite(ContentManager cManager, String textureName)
        {
            this.cManager = cManager;
            this.image = cManager.Load<Texture2D>(textureName);
            this.position = Vector2.Zero;
            this.size = new Vector2(1f, (float)image.Height / (float)image.Width);
            this.pixelSize = new Vector2(image.Width, image.Height);
            this.rotation = 0f;
            this.hasCollisions = false;
        }

        // Ativa as colisões da sprite
        public virtual void EnableCollisions()
        {
            this.hasCollisions = true;
            this.radius = (float)Math.Sqrt(Math.Pow(size.X / 2, 2) + Math.Pow(size.Y / 2, 2));

            pixels = new Color[(int)(pixelSize.X * pixelSize.Y)];
            image.GetData<Color>(pixels);
        }

        // Se houver colisao, collisionPoint é o ponto de colisão
        // se não houver, collisionPoint deve ser ignorado!
        public bool CollidesWith(Sprite other, out Vector2 collisionPoint)
        {
            collisionPoint = position; // Calar o compilador

            if (!this.HasCollisions) return false;
            if (!other.HasCollisions) return false;

            float distance = (this.position - other.position).Length();

            if (distance > this.radius + other.radius) return false;

            return this.PixelTouches(other, out collisionPoint);
        }

        public Color GetColorAt(int x, int y)
        {
            // Se nao houver collider, da erro!!!
            return pixels[x + y * (int)pixelSize.X];
        }

        private Vector2 ImagePixelToVirtualWorld(int i, int j)
        {
            float x = i * size.X / (float)pixelSize.X;
            float y = j * size.Y / (float)pixelSize.Y;
            return new Vector2(position.X + x - (size.X * 0.5f),
                               position.Y - y + (size.Y * 0.5f));
        }

        private Vector2 VirtualWorldPointToImagePixel(Vector2 p)
        {
            Vector2 delta = p - position;
            float i = delta.X * pixelSize.X / size.X;
            float j = delta.Y * pixelSize.Y / size.Y;

            i += pixelSize.X * 0.5f;
            j = pixelSize.Y * 0.5f - j;

            return new Vector2(i, j);
        }

        public bool PixelTouches(Sprite other, out Vector2 collisionPoint)
        {
            // Se nao houver colisao, o ponto de colisao retornado e'
            // a posicao da Sprite (podia ser outro valor qualquer)
            bool touches = false;
            collisionPoint = position;

            for (int i = 0; !touches && i < pixelSize.Y; i++)
			{
			    for (int j = 0; !touches && j < pixelSize.Y; j++)
                {
                    if (GetColorAt(i, j).A > 0)
                    {
                        Vector2 CollidePoint = ImagePixelToVirtualWorld(i, j);
                        Vector2 otherPixel = other.VirtualWorldPointToImagePixel(CollidePoint);

                        if (otherPixel.X >= 0 && otherPixel.Y >= 0 && otherPixel.X < other.pixelSize.X && otherPixel.Y < other.pixelSize.Y)
                        {
                            if (other.GetColorAt((int)otherPixel.X, (int)otherPixel.Y).A > 0)
                            {
                                touches = true;
                                collisionPoint = CollidePoint;
                            }
                        }
                    }
                }
			}
            return touches;
        }

        public virtual void Scale(float scale)
        {
            this.size *= scale;
        }

        public virtual void SetScene(Scene s)
        {
            this.scene = s;
        }
        public Sprite Scl(float scale)
        {
            this.Scale(scale);
            return this;
        }


        public virtual void Draw(GameTime gameTime)
        {
            Rectangle pos = Camera.WorldSize2PixelRectangle(this.position, this.size);
            // scene.SpriteBatch.Draw(this.image, pos, Color.White);
            scene.SpriteBatch.Draw(this.image, pos, source, Color.White,
                this.rotation, new Vector2(pixelSize.X / 2, pixelSize.Y / 2),
                SpriteEffects.None, 0);
        }

        public virtual void SetRotation(float r)
        {
            this.rotation = r;
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Dispose()
        {
            this.image.Dispose();
        }

        public void Destroy()
        {
            this.scene.RemoveSprite(this);
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }
        public Sprite At(Vector2 p)
        {
            this.SetPosition(p);
            return this;
        }

        // Métodos get/set
        public bool HasCollisions
        { 
            get { return hasCollisions; }
            protected set { hasCollisions = value; }
        }
    }
}
