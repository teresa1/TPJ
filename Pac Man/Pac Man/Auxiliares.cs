using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pac_Man
{
    public class Auxiliares
    {
        // Converte as coordenadas virtuais para reais
        static public Vector2 Matrix2Screen(Point matrixPosition)
        {
            return new Vector2(matrixPosition.X * 30, 30 * matrixPosition.Y);
        }

        static public int Matrix2Screen(int screenPosition)
        {
            return (screenPosition * 30);
        }

        //Converte as coordenadas reais para virtuais
        static public Vector2 Screen2Matrix(Vector2 screenPosition)
        {
            return (screenPosition / 30);
        }

        static public int Screen2Matrix(int screenPosition)
        {
            return (int)((screenPosition) / 30 );
        }

        static public bool Span(int x)
        {
            if (x % 30 != 0)
                return true;
            else
                return false;
        }

        // Verifica se o objeto pode prosseguir
  /*      public static bool CanGo(Vector2 rPosition, byte[,] board)
        {
            if (Screen2Matrix(rPosition).X >= 21 || Screen2Matrix(rPosition).Y >= 20)
                return false;
            if (board[(int)Screen2Matrix(rPosition).Y , (int)Screen2Matrix(rPosition).X] != 0)
                return true;
            else return false;
        }
*/
        public static bool CanGo(int x, int y, byte[,] board)
        {
            int mX = Screen2Matrix(x);
            int mY = Screen2Matrix(y);
            if (mX > 21 || mY > 20)
                return false;

            bool canMove = true;
            if (board[mY,mX] == 0 || board[mY, mX] == 3) canMove = false;
            if (Span(x) && (board[mY,mX+1] == 0 || board[mY,mX+1] == 3)) canMove = false;
            if (Span(y) && (board[mY+1,mX] == 0 || board[mY+1,mX] == 3)) canMove = false;
            if (Span(x) && Span(y) && (board[mY+1,mX+1] == 0 || board[mY+1,mX+1] == 3)) canMove = false;
            return canMove;
        }
    }
}
