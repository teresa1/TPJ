using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    class Camera
    {
        // Variáveis
        private static GraphicsDeviceManager gDevManager;
        private static float worldWidth;
        private static float ratio;
        private static Vector2 target; 
        private static int lastSeenPixelWidth = 0;
 
        // Define o GraphicsDeviceManager a usar
        public static void SetGraphicsDeviceManager(GraphicsDeviceManager gDevManager) 
        { 
            Camera.gDevManager = gDevManager; 
        } 

        // Define a largura do mundo
        public static void SetWorldWidth(float worldWidth) 
        { 
            Camera.worldWidth = worldWidth; 
        } 

        // Define o alvo da camera (centro)
        public static void SetTarget(Vector2 target) 
        { 
            Camera.target = target; 
        } 

        /* Atualiza o ratio a ser utilizado pela câmara
         * Depende do tamanho da janela de visualização do Windows */
        private static void UpdateRatio() 
        { 
            if (Camera.lastSeenPixelWidth != Camera.gDevManager.PreferredBackBufferWidth) 
            { 
                Camera.ratio = Camera.gDevManager.PreferredBackBufferWidth / Camera.worldWidth; 
                Camera.lastSeenPixelWidth = Camera.gDevManager.PreferredBackBufferWidth; 
             } 
        }

        // Converte um ponto virtual (metros) para um ponto real (pixels)
        public static Vector2 WorldPoint2Pixels(Vector2 point) 
        { 
            Camera.UpdateRatio(); 
            Vector2 pixelPoint = new Vector2(); 

            // Calcula os pixels em relação ao alvo da camara (centro) 
            pixelPoint.X = (int)((point.X - target.X) * Camera.ratio + 0.5f); 
            pixelPoint.Y = (int)((point.Y - target.Y) * Camera.ratio + 0.5f); 

            // Proteta os pixels calculados para o canto inferior esquerdo do ecra 
            pixelPoint.X += Camera.lastSeenPixelWidth / 2; 
            pixelPoint.Y += Camera.gDevManager.PreferredBackBufferHeight / 2; 

            // Inverter as coordenadas Y 
            pixelPoint.Y = Camera.gDevManager.PreferredBackBufferHeight - pixelPoint.Y; 

            return pixelPoint; 
        } 

        public static Rectangle WorldSize2PixelRectangle(Vector2 position, Vector2 size) 
        { 
            Camera.UpdateRatio(); 
            Vector2 pixelPosition = WorldPoint2Pixels(position); 

            int pixelWidth = (int)( size.X * Camera.ratio + .5f); 
            int pixelHeight = (int)(size.Y * Camera.ratio + .5f); 

            return new Rectangle((int)pixelPosition.X, (int)pixelPosition.Y, pixelWidth, pixelHeight); 
        } 


        // Métodos get/set
        public static float WorldWidth
        {
            get { return worldWidth; }
            private set { worldWidth = value; }
        }

        public static float Ratio
        {
            get { return ratio; }
            private set { ratio = value; }
        }
    }
}
