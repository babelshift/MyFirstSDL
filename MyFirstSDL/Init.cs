using System;
using System.Runtime.InteropServices;

namespace MyFirstSDL
{
    public class Init
    {
        [STAThread]
        static void Main(string[] args)
        {
            MyGame game = new MyGame();
			game.Run();
        }
    }
}
