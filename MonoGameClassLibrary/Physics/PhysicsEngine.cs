using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameClassLibrary.Physics
{
	public class PhysicsEngine : DrawableGameComponent
	{
		public int Width { get { return SpatialGrid.Width; } }
		public int Height { get { return SpatialGrid.Width; } }
		protected SpatialGrid SpatialGrid;
		protected SortedList<int, AABB> aabbs;

		public PhysicsEngine(Game game, int width, int height)
			: base(game)
		{
			aabbs = new SortedList<int, AABB>(new DuplicateKeyComparer<int>());
			SpatialGrid = new SpatialGrid(Game, width / SpatialGrid.TILE_SIZE, height / SpatialGrid.TILE_SIZE);
			DrawOrder = int.MaxValue;
		}

		public void Add(AABB aabb)
		{
			aabbs.Add(aabb.UpdateOrder, aabb);

			SpatialGrid.Add(aabb);

			aabb.PropertyChanging += aabb_PropertyChanging;
			aabb.PropertyChanged += aabb_PropertyChanged;
		}

		public void Remove(AABB aabb)
		{
			aabbs.RemoveAt(aabbs.IndexOfValue(aabb));

			SpatialGrid.Remove(aabb);

			aabb.PropertyChanging -= aabb_PropertyChanging;
			aabb.PropertyChanged -= aabb_PropertyChanged;
		}

		public override void Update(GameTime gameTime)
		{
			foreach (AABB aabb in aabbs.Values)
			{
				if (aabb is Box)
				{
					Box box = aabb as Box;
					box.UpdateSpeed(gameTime);
					box.Update(gameTime);
					box.UpdateLocation(gameTime);
					CollisionHelper.StopSpeed(box, SpatialGrid);
				}
				else
				{
					aabb.Update(gameTime);
				}
			}
		}

		private void aabb_PropertyChanging(AABB sender, PropertyChangingEventArgs e)
		{
			SpatialGrid.Remove(sender);
		}

		private void aabb_PropertyChanged(AABB sender, PropertyChangedEventArgs e)
		{
			SpatialGrid.Add(sender);

			if (sender is Box)
			{
				Box box = sender as Box;
				if (box.SpeedUpdate)
				{
					CollisionHelper.MovementCollision(box, SpatialGrid);
				}
				else
				{
					CollisionHelper.ClassicCollisions(box, SpatialGrid);
				}
			}
			else
			{
				CollisionHelper.ClassicCollisions(sender, SpatialGrid);
			}

			CollisionHelper.SetCollisionNotification(sender, SpatialGrid);
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();
			foreach (AABB aabb in SpatialGrid.GetProbableCollisions(new AABB(null, 0, 0, Width, Height)))
			{
				if (aabb.CollisionSide.HasFlag(AABB.CollisionDirection.Inside))
				{
					DrawHelper.DrawRectangle(spriteBatch, aabb.Rectangle, new Color(Color.Green, 0.1f));
				}
				DrawHelper.DrawOutline(spriteBatch, aabb.Rectangle, Color.Black);
				if (aabb.CollisionSide.HasFlag(AABB.CollisionDirection.Top))
				{
					DrawHelper.DrawRectangle(spriteBatch, new Rectangle(aabb.Rectangle.Left, aabb.Rectangle.Top, aabb.Rectangle.Width, 1), Color.Green);
				}
				if (aabb.CollisionSide.HasFlag(AABB.CollisionDirection.Left))
				{
					DrawHelper.DrawRectangle(spriteBatch, new Rectangle(aabb.Rectangle.Left, aabb.Rectangle.Top, 1, aabb.Rectangle.Height), Color.Green);
				}
				if (aabb.CollisionSide.HasFlag(AABB.CollisionDirection.Right))
				{
					DrawHelper.DrawRectangle(spriteBatch, new Rectangle(aabb.Rectangle.Right - 1, aabb.Rectangle.Top, 1, aabb.Rectangle.Height), Color.Green);
				}
				if (aabb.CollisionSide.HasFlag(AABB.CollisionDirection.Bottom))
				{
					DrawHelper.DrawRectangle(spriteBatch, new Rectangle(aabb.Rectangle.Left, aabb.Rectangle.Bottom - 1, aabb.Rectangle.Width, 1), Color.Green);
				}

				if (aabb is Box)
				{
					Box box = aabb as Box;

					Vector2 end = box.Acceleration;
					end *= (float)gameTime.ElapsedGameTime.TotalSeconds;
					DrawHelper.DrawLine(spriteBatch, box.Rectangle.Center, box.Rectangle.Center + end.ToPoint(), Color.Red);

					end = box.Speed;
					end *= (float)gameTime.ElapsedGameTime.TotalSeconds;
					DrawHelper.DrawLine(spriteBatch, box.Rectangle.Center, box.Rectangle.Center + end.ToPoint(), Color.Blue);
				}
			}
			SpatialGrid.Draw(gameTime);
		}
	}
}
