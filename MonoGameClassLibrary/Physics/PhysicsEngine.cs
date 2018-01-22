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
		protected SortedList<int, Box> boxes;

		public PhysicsEngine(Game game, int width, int height)
			: base(game)
		{
			boxes = new SortedList<int, Box>(new DuplicateKeyComparer<int>());
			SpatialGrid = new SpatialGrid(Game, (int)Math.Ceiling((double)width / SpatialGrid.TILE_SIZE), (int)Math.Ceiling((double)height / SpatialGrid.TILE_SIZE));
			DrawOrder = int.MaxValue;
		}

		public void Add(Box aabb)
		{
			boxes.Add(aabb.UpdateOrder, aabb);

			SpatialGrid.Add(aabb);

			aabb.PropertyChanging += aabb_PropertyChanging;
			aabb.PropertyChanged += aabb_PropertyChanged;
		}

		public void Remove(Box aabb)
		{
			boxes.RemoveAt(boxes.IndexOfValue(aabb));

			SpatialGrid.Remove(aabb);

			aabb.PropertyChanging -= aabb_PropertyChanging;
			aabb.PropertyChanged -= aabb_PropertyChanged;
		}

		public override void Update(GameTime gameTime)
		{
			foreach (Box aabb in boxes.Values)
			{
				if (aabb is KineticBox)
				{
					KineticBox box = aabb as KineticBox;
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

		private void aabb_PropertyChanging(Box sender, PropertyChangingEventArgs e)
		{
			SpatialGrid.Remove(sender);
		}

		private void aabb_PropertyChanged(Box sender, PropertyChangedEventArgs e)
		{
			SpatialGrid.Add(sender);

			if (sender is KineticBox)
			{
				KineticBox box = sender as KineticBox;
				if (box.SpeedUpdate)
				{
					CollisionHelper.MovementCollision(box, SpatialGrid);
				}
				else
				{
					CollisionHelper.ClassicCollision(box, SpatialGrid);
				}
			}
			else
			{
				CollisionHelper.ExpulseCollision(sender, SpatialGrid);
			}

			CollisionHelper.SetCollisionNotification(sender, SpatialGrid);
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();
			foreach (Box aabb in SpatialGrid.GetProbableCollisions(new Box(null, 0, 0, Width, Height)))
			{
				if (aabb.CollisionSide.HasFlag(Box.CollisionDirection.Inside))
				{
					DrawHelper.DrawRectangle(spriteBatch, aabb.Rectangle, new Color(Color.Green, 0.1f));
				}
				DrawHelper.DrawOutline(spriteBatch, aabb.Rectangle, Color.Black);
				if (aabb.CollisionSide.HasFlag(Box.CollisionDirection.Top))
				{
					DrawHelper.DrawRectangle(spriteBatch, new Rectangle(aabb.Rectangle.Left, aabb.Rectangle.Top, aabb.Rectangle.Width, 1), Color.Green);
				}
				if (aabb.CollisionSide.HasFlag(Box.CollisionDirection.Left))
				{
					DrawHelper.DrawRectangle(spriteBatch, new Rectangle(aabb.Rectangle.Left, aabb.Rectangle.Top, 1, aabb.Rectangle.Height), Color.Green);
				}
				if (aabb.CollisionSide.HasFlag(Box.CollisionDirection.Right))
				{
					DrawHelper.DrawRectangle(spriteBatch, new Rectangle(aabb.Rectangle.Right - 1, aabb.Rectangle.Top, 1, aabb.Rectangle.Height), Color.Green);
				}
				if (aabb.CollisionSide.HasFlag(Box.CollisionDirection.Bottom))
				{
					DrawHelper.DrawRectangle(spriteBatch, new Rectangle(aabb.Rectangle.Left, aabb.Rectangle.Bottom - 1, aabb.Rectangle.Width, 1), Color.Green);
				}

				if (aabb is KineticBox)
				{
					KineticBox box = aabb as KineticBox;

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
