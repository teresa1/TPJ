﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sugar_Run
{
    class Camera
    {
        // Variáveis
        private static GraphicsDeviceManager graphicsDevManager;
        private static float worldWidth;
        private static float ratio;
        private static Vector2 target = new Vector2(0, 2.3f); 
        private static int lastSeenPixelWidth = 0;
 
        // Define o GraphicsDeviceManager a usar
        public static void SetGraphicsDeviceManager(GraphicsDeviceManager graphicsDevManager) 
        { 
            Camera.graphicsDevManager = graphicsDevManager; 
        } 

        // Define a largura do mundo
        public static void SetWorldWidth(float worldWidth) 
        { 
            Camera.worldWidth = worldWidth; 
        } 

        // Define o alvo da câmera (centro)
        public static void SetTarget(Vector2 target) 
        { 
            Camera.target.X = target.X;
        }

        // Devolve o alvo da câmara (centro)
        public static Vector2 GetTarget()
        {
            return Camera.target;
        }

        /* Atualiza o ratio a ser utilizado pela câmara
         * Depende do tamanho da janela de visualização do Windows */
        private static void UpdateRatio() 
        { 
            if (Camera.lastSeenPixelWidth != Camera.graphicsDevManager.PreferredBackBufferWidth) 
            { 
                Camera.ratio = Camera.graphicsDevManager.PreferredBackBufferWidth / Camera.worldWidth; 
                Camera.lastSeenPixelWidth = Camera.graphicsDevManager.PreferredBackBufferWidth; 
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

            // Projeta os pixels calculados para o canto inferior esquerdo do ecra 
            pixelPoint.X += Camera.lastSeenPixelWidth / 2; 
            pixelPoint.Y += Camera.graphicsDevManager.PreferredBackBufferHeight / 2; 

            // Inverter as coordenadas Y 
            pixelPoint.Y = Camera.graphicsDevManager.PreferredBackBufferHeight - pixelPoint.Y; 

            return pixelPoint; 
        } 

        public static Rectangle WorldSize2PixelRectangle(Vector2 position, Vector2 size) 
        { 
            Camera.UpdateRatio(); 
            Vector2 pixelPosition = WorldPoint2Pixels(position); 

            int pixelWidth = (int)(size.X * Camera.ratio + .5f); 
            int pixelHeight = (int)(size.Y * Camera.ratio + .5f); 

            return new Rectangle((int)pixelPosition.X, (int)pixelPosition.Y, pixelWidth, pixelHeight); 
        } 

        // Métodos get/set
        public static  GraphicsDeviceManager GraphicsDevManager
        {
            get { return graphicsDevManager; }
            private set { graphicsDevManager = value; } 
        }
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
