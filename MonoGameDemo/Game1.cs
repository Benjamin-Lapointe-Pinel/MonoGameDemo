using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameClassLibrary;
using MonoGameClassLibrary.Animation;
using MonoGameClassLibrary.Physics;
using System;

namespace MonoGameDemo
{
	public class Game1 : MainGame
	{
		Character player1;
		Character player2;

		public Game1()
		{
			CameraWillUpdate += Game_CameraWillUpdate;
			HasDrawn += Game_HasDrawn;
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			Sprite background = new Sprite(DrawHelper.Pixel);
			background.Color = Color.CornflowerBlue;
			background.DestinationRectangle = new Rectangle(0, 0, PhysicsEngine.Width, PhysicsEngine.Height);
			EntityManager.AddDrawable(background);

			constructLevel();

			Texture2D PlayerTexture = Content.Load<Texture2D>("playerSheet");
			AnimationSheet animationSheet = AnimationSheetFactory(PlayerTexture);
			player1 = new Character(animationSheet, new Rectangle(128, 128, 64, 64));
			animationSheet = AnimationSheetFactory(PlayerTexture);
			player2 = new Character(animationSheet, new Rectangle(256, 128, 64, 64));

			PhysicsEngine.Add(player1);
			PhysicsEngine.Add(player2);
			EntityManager.AddDrawable(player1);
			EntityManager.AddDrawable(player2);

			//Test de performance
			for (int i = 0; i < 50; i++)
			{
				//Character character = new Character(animationSheet, new Rectangle(256 + (i * 16), 128, 64, 64));
				//PhysicsEngine.Add(character);
				//EntityManager.AddDrawable(character);
			}
		}

		private void constructLevel()
		{
			DebugPlatform plateform = new DebugPlatform(new Rectangle(0, 1000, 10000, 20), Color.SandyBrown);
			PhysicsEngine.Add(plateform);
			EntityManager.AddDrawable(plateform);

			plateform.OnCollision += Plateform_OnCollision;

			plateform = new DebugPlatform(new Rectangle(0, 0, 20, 1000), Color.SandyBrown);
			PhysicsEngine.Add(plateform);
			EntityManager.AddDrawable(plateform);

			plateform = new DebugPlatform(new Rectangle(500, 800, 20, 136), Color.SandyBrown);
			PhysicsEngine.Add(plateform);
			EntityManager.AddDrawable(plateform);

			plateform = new DebugPlatform(new Rectangle(500, 800, 100, 20), Color.SandyBrown);
			PhysicsEngine.Add(plateform);
			EntityManager.AddDrawable(plateform);

			int i = 0;
			for (; i < 5; i++)
			{
				plateform = new DebugPlatform(new Rectangle(600 + (i * (63 + 20)), 800, 20, 20), Color.Magenta);
				PhysicsEngine.Add(plateform);
				EntityManager.AddDrawable(plateform);
			}
			for (; i < 10; i++)
			{
				plateform = new DebugPlatform(new Rectangle(600 + (i * (64 + 20)), 800, 20, 20), Color.Green);
				PhysicsEngine.Add(plateform);
				EntityManager.AddDrawable(plateform);
			}
		}

		private void Plateform_OnCollision(Box sender, Box.CollisionEventArgs e)
		{
			Console.WriteLine("floor");
		}

		protected override void Update(GameTime gameTime)
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

			return new AnimationSheet(texture2D, new Rectangle(0, 0, 64, 64), cycles);
		}

		private void Game_CameraWillUpdate(MainGame sender, GameTime gameTime)
		{
			Camera.Center = player1.Center;
		}

		private void Game_HasDrawn(MainGame sender, GameTime gameTime)
		{
#if DEBUG
			int fps = (int)(1 / gameTime.ElapsedGameTime.TotalSeconds);
			DrawHelper.DrawDebugText(fps.ToString(), new Vector2(0, 0), Color.Black);
#endif
		}
	}
}
