using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameClassLibrary;
using MonoGameClassLibrary.Animation;
using MonoGameClassLibrary.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDemo
{
	public class Scene1 : Scene
	{
		Character player1;
		//Character player2;

		public Scene1(MainGame mainGame)
			: base(mainGame, 10000, 2000)
		{
			Sprite background = new Sprite(MainGame, MainGame.Content.Load<Texture2D>("background"))
			{
				DestinationRectangle = new Rectangle(0, 0, MainGame.GraphicsDevice.Viewport.Width, MainGame.GraphicsDevice.Viewport.Height),
				DrawOrder = Sprite.BACKGROUND
			};
			AddToScene(background);

			Sprite hud = new Sprite(MainGame, DrawHelper.Pixel)
			{
				DestinationRectangle = new Rectangle(8, 8, 256, 64),
				Color = new Color(Color.Gray, 0.5f),
				DrawOrder = Sprite.FOREGROUND
			};
			AddToScene(hud);

			constructLevel();

			Texture2D PlayerTexture = MainGame.Content.Load<Texture2D>("playerSheet");
			AnimationSheet animationSheet = AnimationSheetFactory(PlayerTexture);
			player1 = new Character(MainGame, animationSheet, 128, 128, 64, 64);
			animationSheet = AnimationSheetFactory(PlayerTexture);
			//player2 = new Character(MainGame, animationSheet, new Rectangle(256, 128, 64, 64));

			AddToScene(player1);
			//AddToScene(player2);

			//Test de performance
			for (int i = 0; i < 100; i++)
			{
				//animationSheet = AnimationSheetFactory(PlayerTexture);
				//Character character = new Character(MainGame, animationSheet, 256 + (i * 32), 128, 64, 64);
				//AddToScene(character);
			}
		}

		public override void Update(GameTime gameTime)
		{
			KeyboardState keyboardState = Keyboard.GetState();

			if (keyboardState.IsKeyDown(Keys.A))
			{
				player1.WalkLeft();
			}
			if (keyboardState.IsKeyDown(Keys.D))
			{
				player1.WalkRight();
			}
			if (keyboardState.IsKeyDown(Keys.W))
			{
				player1.Jump();
			}
			if (keyboardState.IsKeyDown(Keys.Space))
			{
				player1.Location = new Vector2(1503 - (player1.Width / 2), 936);
			}

			//if (keyboardState.IsKeyDown(Keys.Left))
			//{
			//	player2.WalkLeft();
			//}
			//if (keyboardState.IsKeyDown(Keys.Right))
			//{
			//	player2.WalkRight();
			//}
			//if (keyboardState.IsKeyDown(Keys.Up))
			//{
			//	player2.Jump();
			//}

			if (player1.Location.Y > PhysicsEngine.Height)
			{
				player1.Location = new Vector2(-21, 500 - player1.Width / 2);
			}

			base.Update(gameTime);

			if (Mouse.GetState().ScrollWheelValue < lastScrollWheelValue)
			{
				Camera.Zoom /= 1.1f;
			}
			else if(Mouse.GetState().ScrollWheelValue > lastScrollWheelValue)
			{
				Camera.Zoom *= 1.1f;
			}
			lastScrollWheelValue = Mouse.GetState().ScrollWheelValue;

			Camera.Center = player1.Rectangle.Center;
		}
		int lastScrollWheelValue = 0;

		protected void constructLevel()
		{
			DebugPlatform plateform = new DebugPlatform(MainGame, 0, 1000, 10000, 20, Color.SandyBrown);
			AddToScene(plateform);

			plateform = new DebugPlatform(MainGame, 0, 0, 20, 1000, Color.SandyBrown);
			AddToScene(plateform);

			AddToScene(new DebugPlatform(MainGame, 100, 900, 2, 2, Color.White));

			//plateform = new DebugPlatform(MainGame, 500, 800, 20, 136, Color.SandyBrown);
			//AddToScene(plateform);

			AddToScene(new Lever(MainGame, new Point(250, 904), TimeSpan.FromMilliseconds(100)));
			AddToScene(new Lever(MainGame, new Point(300, 968), TimeSpan.FromMilliseconds(100)));

			AddToScene(new PassthroughPlatform(MainGame, 400, 750, 200));

			int i = 0;
			for (; i < 5; i++)
			{
				plateform = new DebugPlatform(MainGame, (600 + (i * (63 + 20))), 800, 20, 20, Color.Magenta);
				AddToScene(plateform);
			}
			for (; i < 10; i++)
			{
				plateform = new DebugPlatform(MainGame, 600 + (i * (64 + 20)), 800, 20, 20, Color.Green);
				AddToScene(plateform);
			}

			plateform = new DebugPlatform(MainGame, 1500, 0, 20, 936, Color.SandyBrown);
			AddToScene(plateform);
			

			Door door = new Door(MainGame, new Point(1500, 873));
			AddToScene(door);

			Lever lever = new Lever(MainGame, new Point(1350, 768), TimeSpan.FromSeconds(2));
			lever.SwitchedOn += door.Open;
			lever.SwitchedOff += door.Close;
			AddToScene(lever);
		}

		private AnimationSheet AnimationSheetFactory(Texture2D texture2D)
		{
			Cycle[] cycles = new Cycle[4];
			Frame[] frames = new Frame[1];
			frames[0] = new Frame(TimeSpan.FromMilliseconds(100));
			cycles[0] = new Cycle(frames);
			frames = new Frame[4];
			frames[0] = new Frame(TimeSpan.FromMilliseconds(100));
			frames[1] = new Frame(TimeSpan.FromMilliseconds(100));
			frames[2] = new Frame(TimeSpan.FromMilliseconds(100));
			frames[3] = new Frame(TimeSpan.FromMilliseconds(100));
			cycles[1] = new Cycle(frames);
			frames = new Frame[1];
			frames[0] = new Frame(TimeSpan.FromMilliseconds(100));
			cycles[2] = new Cycle(frames);
			frames = new Frame[1];
			frames[0] = new Frame(TimeSpan.FromMilliseconds(100));
			cycles[3] = new Cycle(frames);

			return new AnimationSheet(MainGame, texture2D, new Rectangle(0, 0, 64, 64), cycles);
		}
	}
}
