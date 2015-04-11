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
		// Colisões
		protected bool hasCollisions;
		// Raio da "bounding box"
		protected float radius;
		protected Color[] pixels;

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

		// Update (virtual)
		public virtual void Update(GameTime gameTime)
		{

		}

		// Draw
		public virtual void Draw(GameTime gameTime)
		{
			Rectangle pos = new Rectangle(0, 0, 0, 0);//Camera.WorldSize2PixelRectangle(this.position, this.size);
			// scene.SpriteBatch.Draw(this.image, pos, Color.White);

			//spriteBatch.Begin();
			spriteBatch.Draw(this.texture, pos, source, Color.White,
				this.rotation, new Vector2(pixelSize.X / 2, pixelSize.Y / 2),
				SpriteEffects.None, 0);
			//spriteBatch.End();
		}

		// Ativa as colisões da sprite e define o raio da "bounding box"
		public virtual void EnableCollisions()
		{
			this.hasCollisions = true;
			this.radius = (float)Math.Sqrt(Math.Pow(size.X / 2, 2) + Math.Pow(size.Y / 2, 2));

			pixels = new Color[(int)(pixelSize.X * pixelSize.Y)];
			texture.GetData<Color>(pixels);
		}

		/* Se houver colisao, collisionPoint é o ponto de colisão
		 * Se não houver, collisionPoint deve ser ignorado! */
		public bool CollidesWith(Sprite otherSprite, out Vector2 collisionPoint)
		{
			// Para calar o compilador
			collisionPoint = position;

			if (!this.hasCollisions) return false;
			if (!otherSprite.hasCollisions) return false;

			float distance = (this.position - otherSprite.position).Length();

			if (distance > this.radius + otherSprite.radius) return false;

			return this.PixelTouches(otherSprite, out collisionPoint);
		}

		/* Deteta se os pixels de ambas as sprite colidem
		 * 
		 * Se nao houver colisao, o ponto de colisão retornado é
		 * a posicao da sprite (podia ser outro valor qualquer) */
		public bool PixelTouches(Sprite otherSprite, out Vector2 collisionPoint)
		{
			bool isTouching = false;
			// Para calar o compilador
			collisionPoint = position;

			for (int i = 0; !isTouching && i < pixelSize.X; i++)
			{
				for (int j = 0; !isTouching && j < pixelSize.Y; j++)
				{
					if (GetColorAt(i, j).A > 0)
					{
						Vector2 collidePoint = ImagePixelToVirtualWorld(i, j);
						Vector2 otherPixel = otherSprite.VirtualWorldPointToImagePixel(collidePoint);

						if (otherPixel.X >= 0 && otherPixel.Y >= 0 && otherPixel.X < otherSprite.pixelSize.X && otherPixel.Y < otherSprite.pixelSize.Y)
						{
							if (otherSprite.GetColorAt((int)otherPixel.X, (int)otherPixel.Y).A > 0)
							{
								isTouching = true;
								collisionPoint = collidePoint;
							}
						}
					}
				}
			}
			return isTouching;
		}

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
	}
}
