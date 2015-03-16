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

        public static bool canGo(int xPac, int yPac, byte[,] board)
        {
            if (board[yPac, xPac] != 0)
                return true;
            else return false;
        }
    }
}
