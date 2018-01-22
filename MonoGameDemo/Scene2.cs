using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameClassLibrary;
using MonoGameClassLibrary.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDemo
{
	public class Scene2 : Scene
	{
		private DebugPlatform[] debugPlatforms;
		private int debugPlatformIndex;
		private DebugPlatform MinkowskiDifference;

		public Scene2(MainGame mainGame)
			: base(mainGame)
		{
			mainGame.IsMouseVisible = true;

			debugPlatforms = new DebugPlatform[2];
			debugPlatforms[0] = new DebugPlatform(mainGame, 50, 50, 100, 100, Color.DarkGreen);
			debugPlatforms[1] = new DebugPlatform(mainGame, 200, 200, 100, 100, Color.DarkRed);

			AddToScene(debugPlatforms[0]);
			AddToScene(debugPlatforms[1]);

			debugPlatformIndex = 0;

			MinkowskiDifference = new DebugPlatform(mainGame, 0, 0, 0, 0, Color.Red);
			AddToScene(MinkowskiDifference);

			AddToScene(new DebugPlatform(mainGame, 0, 0, 1, 1, Color.Black));

			Camera.Center = mainGame.GraphicsDevice.Viewport.Bounds.Center;
		}

		MouseState lastMouseState;
		public override void Update(GameTime gameTime)
		{
			MouseState mouseState = Mouse.GetState();

			if (mouseState.LeftButton == ButtonState.Pressed)
			{
				DebugPlatform debugPlatform = debugPlatforms[debugPlatformIndex];
				if (lastMouseState.LeftButton == ButtonState.Pressed)
				{
					debugPlatform.Size = Camera.ToWorld(mouseState.Position.ToVector2()) - debugPlatform.Location;
				}
				else
				{
					debugPlatform.Location = Camera.ToWorld(mouseState.Position.ToVector2());
				}
			}
			else if (lastMouseState.LeftButton == ButtonState.Pressed)
			{
				debugPlatformIndex = (debugPlatformIndex + 1) % debugPlatforms.Length;
			}

			lastMouseState = mouseState;
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			Vector2 exitVector = debugPlatforms[0].ExitVector(debugPlatforms[1]);

			MainGame.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.TransformMatrix);
			if (exitVector != Vector2.Zero)
			{
				DrawHelper.DrawLine(MainGame.SpriteBatch, debugPlatforms[0].Center.ToPoint(), (debugPlatforms[0].Center - exitVector).ToPoint(), Color.Black);

				BackgroundColor = Color.Orange;
			}
			else
			{
				BackgroundColor = Color.CornflowerBlue;
			}
			MainGame.SpriteBatch.End();
		}
	}
}