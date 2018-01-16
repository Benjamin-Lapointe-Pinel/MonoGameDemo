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
		protected SortedList<int, GameComponent> Components;

		public Camera Camera { get; protected set; }
		public PhysicsEngine PhysicsEngine { get; protected set; }

		public Scene(MainGame mainGame, Rectangle rectangle)
		{
			MainGame = mainGame;

			Components = new SortedList<int, GameComponent>(new DuplicateKeyComparer<int>());

			Camera = new Camera(MainGame);
			PhysicsEngine = new PhysicsEngine(MainGame, rectangle);
		}

		public void AddComponent(GameComponent component)
		{
			Components.Add(component.UpdateOrder, component);
		}

		public void RemoveComponent(GameComponent component)
		{
			Components.Remove(Components.IndexOfValue(component));
		}

		protected void Exit()
		{
			MainGame.Scenes.Pop();
		}

		public virtual void Update(GameTime gameTime)
		{
			PhysicsEngine.Update(gameTime, Components.Values);
		}

		public virtual void Draw(GameTime gameTime)
		{
			MainGame.GraphicsDevice.Clear(Color.Transparent);
			MainGame.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.Transform);

			foreach (GameComponent component in Components.Values)
			{
				if (component is DrawableGameComponent)
				{
					(component as DrawableGameComponent).Draw(gameTime);
				}
			}

#if DEBUG
			PhysicsEngine.Draw(gameTime);
#endif
			MainGame.SpriteBatch.End();
		}
	}

	public class Drawable : Updatable
	{
		public int DrawOrder;

		public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) { }
	}

	public class Updatable
	{
		public int UpdateOrder;

		public virtual void EntityUpdate(GameTime gameTime) { }
	}

	//https://stackoverflow.com/a/21886340
	public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
	{
		public int Compare(TKey x, TKey y)
		{
			int result = x.CompareTo(y);

			if (result == 0)
			{
				return -1;
			}
			else
			{
				return result;
			}
		}
	}
}
