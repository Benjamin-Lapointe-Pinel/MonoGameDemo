using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClassLibrary.Physics;
using System.Collections.Generic;

namespace MonoGameClassLibrary
{
	public class MainGame : Game
	{
		public GraphicsDeviceManager Graphics { get; protected set; }
		public SpriteBatch SpriteBatch { get; protected set; }
		public Stack<Scene> Scenes;

		public MainGame()
		{
			IsFixedTimeStep = false;
			Content.RootDirectory = "Content";
			Graphics = new GraphicsDeviceManager(this);
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
			if (Scenes.Count > 0)
			{
				Scenes.Peek().Update(gameTime);
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
			}
			else
			{
				Exit();
			}
#if DEBUG
			SpriteBatch.Begin();
			int fps = (int)(1 / gameTime.ElapsedGameTime.TotalSeconds);
			DrawHelper.DrawRectangle(SpriteBatch, new Rectangle(0, 0, 400, 100), new Color(Color.Gray, 0.25f));
			DrawHelper.DrawText(SpriteBatch, fps.ToString(), new Vector2(0, 0), Color.Black);
			SpriteBatch.End();
#endif
		}
	}
}
