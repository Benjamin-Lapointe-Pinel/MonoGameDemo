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
		Character player2;

		public Scene1(MainGame mainGame)
			: base(mainGame, 5000, 5000)
		{
			Sprite background = new Sprite(MainGame, DrawHelper.Pixel);
			background.Color = Color.CornflowerBlue;
			background.DestinationRectangle = new Rectangle(0, 0, PhysicsEngine.Width, PhysicsEngine.Height);
			AddComponent(background);

			constructLevel();

			Texture2D PlayerTexture = MainGame.Content.Load<Texture2D>("playerSheet");
			AnimationSheet animationSheet = AnimationSheetFactory(PlayerTexture);
			player1 = new Character(MainGame, animationSheet, new Rectangle(128, 128, 64, 64));
			animationSheet = AnimationSheetFactory(PlayerTexture);
			player2 = new Character(MainGame, animationSheet, new Rectangle(256, 128, 64, 64));

			AddComponent(player1);
			AddComponent(player2);

			//Test de performance
			for (int i = 0; i < 50; i++)
			{
				//animationSheet = AnimationSheetFactory(PlayerTexture);
				//Character character = new Character(MainGame, animationSheet, new Rectangle(256 + (i * 16), 128, 64, 64));
				//AddComponent(character);
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

			if (keyboardState.IsKeyDown(Keys.Left))
			{
				player2.WalkLeft();
			}
			if (keyboardState.IsKeyDown(Keys.Right))
			{
				player2.WalkRight();
			}
			if (keyboardState.IsKeyDown(Keys.Up))
			{
				player2.Jump();
			}
			
			base.Update(gameTime);

			Camera.Center = player1.Center;

			if (player1.Location.Y > PhysicsEngine.SpatialGrid.Height)
			{
				Exit();
			}
		}

		protected void constructLevel()
		{
			DebugPlatform plateform = new DebugPlatform(MainGame, new Rectangle(0, 1000, 10000, 20), Color.SandyBrown);
			AddComponent(plateform);

			plateform = new DebugPlatform(MainGame, new Rectangle(0, 0, 20, 1000), Color.SandyBrown);
			AddComponent(plateform);

			plateform = new DebugPlatform(MainGame, new Rectangle(500, 800, 20, 136), Color.SandyBrown);
			AddComponent(plateform);

			plateform = new DebugPlatform(MainGame, new Rectangle(500, 800, 100, 20), Color.SandyBrown);
			AddComponent(plateform);

			int i = 0;
			for (; i < 5; i++)
			{
				plateform = new DebugPlatform(MainGame, new Rectangle(600 + (i * (63 + 20)), 800, 20, 20), Color.Magenta);
				AddComponent(plateform);
			}
			for (; i < 10; i++)
			{
				plateform = new DebugPlatform(MainGame, new Rectangle(600 + (i * (64 + 20)), 800, 20, 20), Color.Green);
				AddComponent(plateform);
			}

			plateform = new DebugPlatform(MainGame, new Rectangle(1500, 0, 20, 936), Color.SandyBrown);
			AddComponent(plateform);

			Lever lever = new Lever(MainGame, new Point(1350, 770));
			lever.OnAction += Lever_OnAction;
			AddComponent(lever);

			door = new Door(MainGame, new Point(1500, 936));
			AddComponent(door);
		}

		Door door;
		private void Lever_OnAction()
		{
			door.Open();
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
