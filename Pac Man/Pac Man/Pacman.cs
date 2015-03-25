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
        public enum Movimentos
        {
            direita,
            esquerda,
            cima,
            baixo,
            parado
        }

        // Variáveis
        public Vector2 vPosition;
        public Vector2 rPosition;
        private Texture2D sprite;
        private int score;
        Movimentos movimentoAtual = Movimentos.parado;

        // Construtor
        public PacMan(Vector2 position, string textureName, ContentManager content)
        {
            this.vPosition = position;
            this.rPosition = Auxiliares.Matrix2Screen(position);
            sprite = content.Load<Texture2D>(textureName);
            score = 0;
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, rPosition, Color.White);
        }

        // Dispose
        public void Dispose()
        {
            sprite.Dispose();
        }

        // Faz o Pac Man comer as pellets
        public void Comer(byte[,] board)
        {
            if (board[Auxiliares.Screen2Matrix(rPosition.Y), Auxiliares.Screen2Matrix(rPosition.X)] == 1)
            {
                board[Auxiliares.Screen2Matrix(rPosition.Y), Auxiliares.Screen2Matrix(rPosition.X)] = 2;
                score++;
            }
        }

        public void Update(byte[,] board)
        {
            Console.WriteLine("yeh X:" + vPosition.X + " Y:" + vPosition.Y);

            //Aqui, ele está a mover-se, e a detetar se já chegou ao centro do próximo quadrado
            float speed = 0.01f;
            if (movimentoAtual == Movimentos.direita)
            {
                rPosition.X = -speed;
                if (vPosition.X == (int)vPosition.X) //Centro de um quadrado
                    movimentoAtual = Movimentos.parado;
            }
            else if (movimentoAtual == Movimentos.esquerda)
            {
                rPosition.X = speed;
                if (vPosition.X == (int)vPosition.X)
                    movimentoAtual = Movimentos.parado;
            }
            else if (movimentoAtual == Movimentos.cima)
            {
                rPosition.Y = speed;
                if (vPosition.Y == (int)vPosition.Y) //Centro de um quadrado
                    movimentoAtual = Movimentos.parado;
            }
            else if (movimentoAtual == Movimentos.baixo)
            {
                rPosition.Y = -speed;
                if (vPosition.Y == (int)vPosition.Y)
                    movimentoAtual = Movimentos.parado;
            }

            Comer(board);
        }

        public bool changeDirection(Movimentos newDirection)
        {
            if (movimentoAtual == Movimentos.parado)
            {
                movimentoAtual = newDirection;
                return true;
            }
            else
                return false;

        }

        private void Collide(List<Fantasma> fantasmas)
        {
            int x;
            int y;
            int distancia;
            foreach (Fantasma fantasma in fantasmas)
            {
               
                    x = (int)vPosition.X - (int)fantasma.Position.X;
                    y = (int)vPosition.Y - (int)fantasma.Position.Y;
                    
                    distancia = (int)Math.Sqrt(x * x + y * y);

                    
                    if(distancia < 0.5)
                    {
                        
                    }
                
            }
        }
    }
}
