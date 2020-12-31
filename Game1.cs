using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using MemoryMatch.Scenes;

namespace MemoryMatch
{
    public class Game1 : Core
    {
        public Game1()
        {
            Screen.IsFullscreen = true;
        }

       

        //public Core(int width = 1280, int height = 720, bool isFullScreen = false, string windowTitle = "Nez", string contentDirectory = "Content")
        protected override void Initialize()
        {
            
            base.Initialize();
            
            Scene  = new MainScene();
        }

 

    }
}
