using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClassLibrary.Physics;
using System;
using System.Collections.Generic;

namespace MonoGameClassLibrary
{
	public class MainGame : Game
	{
		public float SlowDownFactor { get; protected set; }
		public GraphicsDeviceManager Graphics { get; protected set; }
		public SpriteBatch SpriteBatch { get; protected set; }
		public Stack<Scene> Scenes { get; protected set; }

		public MainGame()
		{
			IsFixedTimeStep = true;
			SlowDownFactor = 0;
			Content.RootDirectory = "Content";
			Graphics = new GraphicsDeviceManager(this)
			{
				SynchronizeWithVerticalRetrace = false
			};

			Scenes = new Stack<Scene>();
		}

		protected override void LoadContent()
		{
			SpriteBatch = new SpriteBatch(GraphicsDevice);
			Services.AddService(SpriteBatch);

			DrawHelper.Init(GraphicsDevice, Content);

			MainGameBegin();
		}

		protected virtual void MainGameBegin() { }
		
		protected override void Update(GameTime gameTime)
		{
			//gameTime.ElapsedGameTime = TimeSpan.FromTicks(gameTime.ElapsedGameTime.Ticks / 8);
			if (Scenes.Count > 0)
			{
				if (gameTime.IsRunningSlowly)
				{
					if (SlowDownFactor > 1)
					{
						gameTime.ElapsedGameTime = TimeSpan.FromTicks((long)(gameTime.ElapsedGameTime.Ticks / SlowDownFactor));
						Scenes.Peek().Update(gameTime);
					}
				}
				else
				{
					Scenes.Peek().Update(gameTime);
				}
			}
			else
			{
				Exit();
			}
		}

		protected override void Draw(GameTime gameTime)
		{
			if (Scenes.Count > 0)
			{
				Scenes.Peek().Draw(gameTime);
#if DEBUG
				SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Scenes.Peek().Camera.TransformMatrix);
				Scenes.Peek().PhysicsEngine.Draw(new GameTime(gameTime.TotalGameTime, TargetElapsedTime));
				SpriteBatch.End();
#endif
			}
			else
			{
				Exit();
			}
		}
	}
}
