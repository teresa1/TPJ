using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
	public class Sprite
	{
		// Variáveis
		protected ContentManager content;
		protected Scene scene;
		//Sprite
		protected Texture2D texture;
		protected Vector2 position;
		protected Vector2 size;
		protected Vector2 pixelSize;
		protected float rotation;
		protected Rectangle? source = null;
        Rectangle rect = new Rectangle(10, 0, 500, 80);
		// Colisões
		protected bool hasCollisions;
        protected Rectangle boundingBox;
        protected bool isFalling;
		// Raio da "bounding box"
		protected float radius;
		protected Color[] pixels;

        public Texture2D Textura;
        public Vector2 Posicao;

		// Construtor
		public Sprite(ContentManager content, String textureName)
		{
			this.content = content;
			this.texture = content.Load<Texture2D>(textureName);
			this.position = Vector2.Zero;
			this.size = new Vector2(1f, (float)texture.Height / (float)texture.Width);
			this.pixelSize = new Vector2(texture.Width, texture.Height);
			this.rotation = 0f;
			this.hasCollisions = false;
		}

		// Update
		public virtual void Update(GameTime gameTime)
		{
            Gravidade();

            if (this.HasCollisions)
                this.boundingBox = new Rectangle((int)(position.X + 0.5f), (int)(position.Y + 0.5f), texture.Width, texture.Height);
		}

		// Draw
		public virtual void Draw(GameTime gameTime)
		{
			Rectangle pos = Camera.WorldSize2PixelRectangle(this.position, this.size);
			// scene.SpriteBatch.Draw(this.image, pos, Color.White);

			scene.SpriteBatch.Draw(this.texture, pos, source, Color.White,
				this.rotation, new Vector2(pixelSize.X / 2, pixelSize.Y / 2),
				SpriteEffects.None, 0);
		}

        // Ativa as colisões e cria a bounding box
        public virtual void EnableCollisions()
        {
            this.HasCollisions = true;
            this.boundingBox = new Rectangle((int)(position.X + 0.5f), (int)(position.Y + 0.5f), texture.Width, texture.Height);
        }

        //public bool Collides(Sprite sprite1, Sprite sprite2)
        //{
        //    bool isColiding = false;

        //    if ((sprite1.Posicao.X - sprite1.Textura.Width / 2 < sprite2.Posicao.X + sprite2.Textura.Width / 2) &&
        //        (sprite1.Posicao.X + sprite1.Textura.Width / 2 > sprite2.Posicao.X - sprite2.Textura.Width / 2) &&
        //        (sprite1.Posicao.Y - sprite1.Textura.Height / 2 < sprite2.Posicao.X + sprite2.Textura.Height / 2) &&
        //        (sprite1.Posicao.Y + sprite1.Textura.Height / 2 > sprite2.Posicao.X - sprite2.Textura.Height / 2))
        //        isColiding = true;
        //    return isColiding;
        //}

		/* Deteta se os pixels de ambas as sprite colidem
		 * 
		 * Se nao houver colisao, o ponto de colisão retornado é
		 * a posicao da sprite (podia ser outro valor qualquer) */
		
		// Devolve o alpha de um pixel da sprite
		public Color GetColorAt(int x, int y)
		{
			// Se nao houver collider, da erro!!!
			return pixels[x + y * (int)pixelSize.X];
		}

		// Converte coordenadas reais (pixels) para coordenadas virtuais (metros)
		private Vector2 ImagePixelToVirtualWorld(int i, int j)
		{
			float x = i * size.X / (float)pixelSize.X;
			float y = j * size.Y / (float)pixelSize.Y;
			return new Vector2(position.X + x - (size.X * 0.5f), position.Y - y + (size.Y * 0.5f));
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
		public virtual void SetScene(Scene scene)
		{
			this.scene = scene;
		}

		// Altera a rotação da sprite
		public virtual void SetRotation(float rotation)
		{
			this.rotation = rotation;
		}

		// Escala o tamanho da sprite
		public virtual void SetScale(float scale)
		{
			this.size *= scale;
		}

		// Escala o tamanho da sprite, devolvendo a sprite
		public Sprite SpriteScale(float scale)
		{
			this.SetScale(scale);
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

		// Remove a sprite da cena
		public void Destroy()
		{
			this.scene.RemoveSprite(this);
		}

		// Dispose
		public virtual void Dispose()
		{
			this.texture.Dispose();
		}

		// Métodos get/set
		public bool HasCollisions
		{ 
			get { return hasCollisions; }
			protected set { hasCollisions = value; }
		}

        public void LoadContent(string assetName)
        {
            Textura = content.Load<Texture2D>(assetName);
            Posicao = Vector2.Zero;
        }

        public virtual void Collides(Plataforma plataforma)
        {
            if (this.boundingBox.Intersects(plataforma.boundingBox))
                this.isFalling = false;
            else isFalling = true;
        }

        public void Gravidade()
        {
            if (isFalling)
                this.position.Y -= 0.1f;
        }

        //public void draw(int fade)
        //{
        //    SpriteBatch.Draw(Textura, position, new Color(fade, fade, fade));
        //}
	}
}
