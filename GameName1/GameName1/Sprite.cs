using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sugar_Run
{
    public class Sprite
    {
        // Variáveis
        protected ContentManager content;
        protected Scene scene;
        //Sprite
        public string name;
        protected Texture2D texture;
        public Vector2 position;
        public Vector2 size;
        protected Vector2 pixelSize;
        protected float rotation;
        protected Rectangle? source = null;
        // Colisões
        protected bool hasCollisions;
        // Raio da "bounding box"
        protected float radius;
        protected Color[] pixels;

        // Construtor
        public Sprite(ContentManager content, string textureName)
        {
            this.name = textureName;
            this.content = content;
            this.position = Vector2.Zero;
            this.texture = content.Load<Texture2D>(textureName);
            this.size = new Vector2(1f, (float)texture.Height / (float)texture.Width);
            this.pixelSize = new Vector2(texture.Width, texture.Height);
            this.rotation = 0f;
            this.hasCollisions = false;
        }

        // Load Content
        public virtual void LoadContent()
        {

        }

        // Unload Content
        public virtual void UnloadContent()
        {
            this.texture.Dispose();
        }

        // Update
        public virtual void Update(GameTime gameTime) 
        {

        }

        // Draw 
        public virtual void Draw(GameTime gameTime)
        {
            Rectangle rectPosition = Camera.WorldSize2PixelRectangle(this.position, this.size);
            scene.spriteBatch.Draw(this.texture, rectPosition, source, Color.White, this.rotation, new Vector2(pixelSize.X / 2, pixelSize.Y / 2), SpriteEffects.None, 0);
        }


        // Ativa as colisões e inicializa a bounding box
        public virtual void EnableCollisions()
        {
            this.hasCollisions = true;
            this.radius = (float)Math.Sqrt(Math.Pow(size.X / 2, 2) + Math.Pow(size.Y / 2, 2));

            pixels = new Color[(int)(pixelSize.X * pixelSize.Y)];
            texture.GetData<Color>(pixels);
        }

        // Se houver colisao, collisionPoint é o ponto de colisão
        // se não houver, collisionPoint deve ser ignorado!
        public bool CollidesWith(Sprite other, out Vector2 collisionPoint)
        {
            collisionPoint = position; // Calar o compilador

            if (!this.hasCollisions)  return false;
            if (!other.hasCollisions) return false;

            float distance = (this.position - other.position).Length();

            if (distance > this.radius + other.radius) return false;
            
            return this.PixelTouches(other, out collisionPoint);
        }

        public Color GetColorAt(int x, int y)
        {
            // Se nao houver collider, da erro!!!
            return pixels[x + y * (int)pixelSize.X];
        }

        public bool PixelTouches(Sprite other, out Vector2 collisionPoint)
        {
            // Se nao houver colisao, o ponto de colisao retornado é a posicao da Sprite (podia ser outro valor qualquer)
            collisionPoint = position;

            bool touches = false;

            int i = 0;
            while (touches == false && i < pixelSize.X)
            {
                int j = 0;
                while (touches == false && j < pixelSize.Y)
                {
                    if (GetColorAt(i, j).A > 0)
                    {
                        Vector2 CollidePoint = ImagePixelToVirtualWorld(i, j);
                        Vector2 otherPixel = other.VirtualWorldPointToImagePixel(CollidePoint);

                        if (otherPixel.X >= 0 && otherPixel.Y >= 0 &&
                            otherPixel.X < other.pixelSize.X &&
                            otherPixel.Y < other.pixelSize.Y)
                        {
                            if (other.GetColorAt((int)otherPixel.X, (int)otherPixel.Y).A > 0)
                            {
                                touches = true;
                                collisionPoint = CollidePoint;
                            }
                        }

                    }
                    j++;
                }
                i++;
            }
            return touches;
        }


        // Converte coordenadas reais (pixels) para coordenadas virtuais (metros)
        private Vector2 ImagePixelToVirtualWorld(int i, int j)
        {
            float x = i * size.X / (float)pixelSize.X;
            float y = j * size.Y / (float)pixelSize.Y;
            return new Vector2(position.X + x - (size.X * 0.5f),
                               position.Y - y + (size.Y * 0.5f));
        }

        // Converte coordenadas virtuais (metros) para coordenadas virtuais (pixels)
        private Vector2 VirtualWorldPointToImagePixel(Vector2 p)
        {
            Vector2 delta = p - position;
            float i = delta.X * pixelSize.X / size.X;
            float j = delta.Y * pixelSize.Y / size.Y;

            i += pixelSize.X * 0.5f;
            j = pixelSize.Y * 0.5f - j;

            return new Vector2(i, j);
        }


        // Seleciona uma cena
        public virtual void SetScene(Scene s)
        {
            this.scene = s;
        }

        // Escala o tamanho da sprite
        public virtual void Scale(float scale)
        {
            this.size *= scale;
        }

        // Escala o tamanho da sprite, devolvendo essa sprite
        public Sprite SpriteScale(float scale)
        {
            this.Scale(scale);
            return this;
        }

        // Altera a posição da sprite
        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        // Altera a posição da sprite, devolvendo a sprite
        public Sprite SpritePosition(Vector2 position)
        {
            this.SetPosition(position);
            return this;
        }

        // Altera a rotação da sprite
        public virtual void SetRotation(float rotation)
        {
            this.rotation = rotation;
        }


        // Remove a sprite da cena
        public virtual void Destroy()
        {
            this.scene.RemoveSprite(this);
        }


        // Métodos get/set
        public bool HasCollisions
        {
            get { return hasCollisions; }
            protected set { hasCollisions = value; }
        }
    }
}
