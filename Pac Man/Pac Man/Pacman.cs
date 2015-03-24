using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Net;

namespace Pac_Man
{
    class PacMan
    {
        // Variáveis
        public Vector2 position;
        private Texture2D sprite;
        public int score;
        
        // Construtor
        public PacMan(Vector2 position, string textureName, ContentManager content)
        {
            this.position = position;
            sprite = content.Load<Texture2D>(textureName);
            score = 0;

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, Auxiliares.Matrix2Screen(position), Color.White);
        }

        public void Dispose()
        {
            sprite.Dispose();
        }

        public void Comer(byte[,] board)
        {
            if (board[(int)(position.Y + 0.5f), (int)(position.X + 0.5f)] == 1)
            {
                board[(int)(position.Y + 0.5f), (int)(position.X + 0.5f)] = 2;
                score++;
            }
        }


        public bool Collide(List<Fantasma> fantasmas)
        {
          
            int x;
            int y;
            int distancia;
            foreach (Fantasma fantasma in fantasmas)
            {
               
                    
                    distancia = (int)(position-fantasma.Position).Length();


                    if (distancia < 1)
                    {
                        Console.WriteLine("mnham mnahm");
                        return true;
                    }
                    else return false;
                
            }   return false;
        }
    }
}
