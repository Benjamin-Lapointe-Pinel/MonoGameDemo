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

		public Camera Camera { get; protected set; }
		public PhysicsEngine PhysicsEngine { get; protected set; }

		public Scene(MainGame mainGame, int width, int height)
		{
			MainGame = mainGame;

			Updatables = new SortedList<int, IUpdateable>(new DuplicateKeyComparer<int>());
			Drawables = new SortedList<int, IDrawable>(new DuplicateKeyComparer<int>());

			Camera = new Camera(MainGame);
			PhysicsEngine = new PhysicsEngine(MainGame, width, height);
		}

		public void AddToScene(IUpdateable component)
		{
			if (component is AABB)
			{
				PhysicsEngine.Add(component as AABB);
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
			if (component is AABB)
			{
				PhysicsEngine.Remove(component as AABB);
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
			MainGame.GraphicsDevice.Clear(Color.Transparent);
			MainGame.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.Transform);

			foreach (IDrawable drawable in Drawables.Values)
			{
				drawable.Draw(gameTime);
			}

			MainGame.SpriteBatch.End();
		}
	}
}
