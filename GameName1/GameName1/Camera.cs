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

        // Define o alvo da camera
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
