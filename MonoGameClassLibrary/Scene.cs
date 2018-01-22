using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClassLibrary.Physics;

namespace MonoGameClassLibrary
{
	public class Scene
	{
		protected MainGame MainGame;
		protected SortedList<int, IUpdateable> Updatables;
		protected SortedList<int, IDrawable> Drawables;

		public Color BackgroundColor { get; set; }
		public Camera Camera { get; protected set; }
		public PhysicsEngine PhysicsEngine { get; protected set; }

		public Scene(MainGame mainGame, int width, int height)
		{
			MainGame = mainGame;

			Updatables = new SortedList<int, IUpdateable>(new DuplicateKeyComparer<int>());
			Drawables = new SortedList<int, IDrawable>(new DuplicateKeyComparer<int>());

			BackgroundColor = Color.Transparent;
			Camera = new Camera(MainGame);
			PhysicsEngine = new PhysicsEngine(MainGame, width, height);
		}

		public Scene(MainGame mainGame)
			: this(mainGame, mainGame.GraphicsDevice.Viewport.Width, mainGame.GraphicsDevice.Viewport.Height)
		{
		}

		public void AddToScene(IUpdateable component)
		{
			if (component is Box)
			{
				PhysicsEngine.Add(component as Box);
			}
			else
			{
				Updatables.Add(component.UpdateOrder, component);
			}

			if (component is IDrawable)
			{
				IDrawable drawable = component as IDrawable;
				Drawables.Add(drawable.DrawOrder, drawable);
			}
		}

		public void RemoveFromScene(IUpdateable component)
		{
			if (component is Box)
			{
				PhysicsEngine.Remove(component as Box);
			}
			else
			{
				Updatables.Remove(Updatables.IndexOfValue(component));
			}

			if (component is IDrawable)
			{
				IDrawable drawable = component as IDrawable;
				Drawables.Remove(Drawables.IndexOfValue(drawable));
			}
		}

		protected void Exit()
		{
			MainGame.Scenes.Pop();
		}

		public virtual void Update(GameTime gameTime)
		{
			foreach (IUpdateable updatable in Updatables.Values)
			{
				updatable.Update(gameTime);
			}

			PhysicsEngine.Update(gameTime);
		}

		public virtual void Draw(GameTime gameTime)
		{
			MainGame.GraphicsDevice.Clear(BackgroundColor);

			//Background
			MainGame.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
			int i = 0;
			for (; i < Drawables.Count; i++)
			{
				if (Drawables.Values[i].DrawOrder == Sprite.BACKGROUND)
				{
					Drawables.Values[i].Draw(gameTime);
				}
				else
				{
					break;
				}
			}
			MainGame.SpriteBatch.End();

			//Middleground
			MainGame.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.TransformMatrix);
			for (; i < Drawables.Count; i++)
			{
				if (Drawables.Values[i].DrawOrder != Sprite.FOREGROUND)
				{
					Drawables.Values[i].Draw(gameTime);
				}
				else
				{
					break;
				}
			}
			MainGame.SpriteBatch.End();

			//Foreground
			MainGame.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
			for (; i < Drawables.Count; i++)
			{
				Drawables.Values[i].Draw(gameTime);
			}
			MainGame.SpriteBatch.End();
		}
	}
}
