using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameClassLibrary
{
	public class EntityManager : DrawableGameComponent
	{
		protected SortedList<int, Updatable> Updatables;
		protected SortedList<int, Drawable> Drawables;
		protected SpriteBatch spriteBatch;

		public EntityManager(MainGame mainGame)
			: base(mainGame)
		{
			Updatables = new SortedList<int, Updatable>(new DuplicateKeyComparer<int>());
			Drawables = new SortedList<int, Drawable>(new DuplicateKeyComparer<int>());
			spriteBatch = mainGame.SpriteBatch;
		}

		public void AddUpdatable(Updatable updatable)
		{
			Updatables.Add(updatable.UpdateOrder, updatable);
		}

		public void RemoveUpdatable(Updatable updatable)
		{
			Updatables.RemoveAt(Updatables.IndexOfValue(updatable));
		}

		public void AddDrawable(Drawable drawable)
		{
			Updatables.Add(drawable.UpdateOrder, drawable);
			Drawables.Add(drawable.DrawOrder, drawable);
		}

		public void RemoveDrawable(Drawable drawable)
		{
			Updatables.RemoveAt(Updatables.IndexOfValue(drawable));
			Drawables.RemoveAt(Drawables.IndexOfValue(drawable));
		}

		public override void Update(GameTime gameTime)
		{
			foreach (Updatable updatable in Updatables.Values)
			{
				updatable.Update(gameTime);
			}
		}

		public override void Draw(GameTime gameTime)
		{
			foreach (Drawable drawable in Drawables.Values)
			{
				drawable.Draw(spriteBatch, gameTime);
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

			public virtual void Update(GameTime gameTime) { }
		}
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