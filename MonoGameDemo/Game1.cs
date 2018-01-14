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
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			Sprite background = new Sprite(DrawHelper.Pixel);
			background.Color = Color.CornflowerBlue;
			background.DestinationRectangle = new Rectangle(0, 0, PhysicsEngine.spatialGrid.Width, PhysicsEngine.spatialGrid.Height);
			EntityManager.AddDrawable(background);

			constructLevel();

			Texture2D PlayerTexture = Content.Load<Texture2D>("playerSheet");
			AnimationSheet animationSheet = AnimationSheetFactory(PlayerTexture);
			player1 = new Character(animationSheet, new Box(new Rectangle(128, 128, 64, 64)));
			animationSheet = AnimationSheetFactory(PlayerTexture);
			player2 = new Character(animationSheet, new Box(new Rectangle(256, 128, 64, 64)));

			EntityManager.AddDrawable(player1);
			EntityManager.AddDrawable(player2);
			PhysicsEngine.Add(player1.BoundingBox);
			PhysicsEngine.Add(player2.BoundingBox);
		}

		private void constructLevel()
		{
			DebugPlatform plateform = new DebugPlatform(new Rectangle(0, 1000, 1000, 20));
			PhysicsEngine.Add(plateform.box);
			EntityManager.AddDrawable(plateform);
		}

		protected override void Update(GameTime gameTime)
		{
			KeyboardState keyboardState = Keyboard.GetState();

			if (keyboardState.IsKeyDown(Keys.A))
			{
				player1.WalkLeft(gameTime);
			}
			if (keyboardState.IsKeyDown(Keys.D))
			{
				player1.WalkRight(gameTime);
			}
			if (keyboardState.IsKeyDown(Keys.W))
			{
				player1.Jump(gameTime);
			}

			if (keyboardState.IsKeyDown(Keys.Left))
			{
				player2.WalkLeft(gameTime);
			}
			if (keyboardState.IsKeyDown(Keys.Right))
			{
				player2.WalkRight(gameTime);
			}
			if (keyboardState.IsKeyDown(Keys.Up))
			{
				player2.Jump(gameTime);
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

		protected override void CameraWillUpdate()
		{
			Camera.Center = player1.BoundingBox.Rectangle.Center;
		}
	}
}
