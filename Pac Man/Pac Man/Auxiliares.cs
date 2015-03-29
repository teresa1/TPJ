using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pac_Man
{
    public static class Auxiliares
    {
        static public Vector2 Matrix2Screen(Vector2 matrixPosition)
        {
            return (matrixPosition * 30);
        }

        static public Vector2 Screen2Matrix(Vector2 screenPosition)
        {
            return (screenPosition / 30);
        }

        public static bool CanGo(int x, int y, byte[,] board)
        {
            if (x > 21 || y > 20)
                return false;
            if (board[y, x] != 0)
                return true;
        
                
            else return false;
        }
    }
}