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
        static public Vector2 Matrix2Screen(Vector2 matrixPosition)
        {
            return (matrixPosition * 30);
        }

        static public float Matrix2Screen(float screenPosition)
        {
            return (screenPosition * 30);
        }

        //Converte as coordenadas reais para virtuais
        static public Vector2 Screen2Matrix(Vector2 screenPosition)
        {
            return (screenPosition / 30);
        }

        static public int Screen2Matrix(float screenPosition)
        {
            return ((int)(screenPosition + 0.5f) / 30);
        }

        // Verifica se o objeto pode prosseguir
        public static bool CanGo(Vector2 rPosition, byte[,] board)
        {
            if (Screen2Matrix(rPosition).X > 21 || Screen2Matrix(rPosition).Y > 20)
                return false;
            if (board[(int)Screen2Matrix(rPosition).Y + 1, (int)Screen2Matrix(rPosition).X] != 0)
                return true;
            else return false;
        }

        public static bool CanGo(float x, float y, byte[,] board)
        {
            if (Screen2Matrix(x) > 21 || Screen2Matrix(y) > 20)
                return false;
            if (board[Screen2Matrix(y), Screen2Matrix(x)] != 0 && board[Screen2Matrix(y), Screen2Matrix(x)] != 3)
                return true;
            else return false;
        }
    }
}
