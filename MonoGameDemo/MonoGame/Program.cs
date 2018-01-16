#define MONOGAME_DEBUG
using MonoGameClassLibrary;
using System;

namespace MonoGameDemo
{
    public static class Program
    {
		private class Game1 : MainGame
		{
			protected override void MainGameBegin()
			{
				Scenes.Push(new Scene1(this));
			}
		}

		[STAThread]
        static void Main()
        {
			using (var game = new Game1())
			{
				game.Run();
			}
        }
    }
}
