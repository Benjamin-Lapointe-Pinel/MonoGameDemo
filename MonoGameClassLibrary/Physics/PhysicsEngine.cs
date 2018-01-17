using System;
using System.Collections.Generic;
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
			SpatialGrid.Add(aabb);
			aabbs.Add(aabb.UpdateOrder, aabb);
		}

		public void Remove(AABB aabb)
		{
			SpatialGrid.Remove(aabb);
			aabbs.RemoveAt(aabbs.IndexOfValue(aabb));
		}

		public override void Update(GameTime gameTime)
		{
			UpdatePositions(gameTime);
			ResolveCollisions();
		}

		private void ResolveCollisions()
		{
			foreach (AABB aabb in aabbs.Values)
			{
				if (aabb is Box)
				{
					//TODO: classic collision resolution
					//Pourrait aller dans La spatial grid?
					//Ou ne pas le faire dans la boucle ici, mais bien s'abbonner aux évenements des boites?
				}

				SetCollisionFlags(aabb, SpatialGrid.GetProbableCollisions(aabb));
			}
		}

		private void UpdatePositions(GameTime gameTime)
		{
			foreach (AABB aabb in aabbs.Values)
			{
				if (aabb is Box)
				{
					Box box = aabb as Box;
					box.UpdateSpeed(gameTime);
					box.Update(gameTime);
					if (box.PreciseMovement)
					{
						SpatialGrid.Remove(box);
						PreciseMovement(gameTime, box);
						SpatialGrid.Add(box);
					}
					else
					{
						box.UpdateLocation(gameTime);
					}
				}
				else
				{
					aabb.Update(gameTime);
				}
			}
		}

		protected void SetCollisionFlags(AABB toBeFlagged, IEnumerable<AABB> aabbs)
		{
			foreach (AABB aabb in aabbs)
			{
				toBeFlagged.CollisionNotification(aabb);
			}
		}

		//BUG : résolution de collision de deux objets qui bougent, pas précis
		//TODO collision le long du mouvement
		protected void PreciseMovement(GameTime gameTime, Box box)
		{
			int steps = 1;

			float maxSpeed = SpatialGrid.TILE_SIZE;
			if (box.PreciseMovement)
			{
				maxSpeed = Math.Min(box.Width, box.Height);
			}
			if (box.Speed.Length() > maxSpeed)
			{
				steps = (int)Math.Ceiling(box.Speed.Length() / maxSpeed);
			}

			GameTime relativeGameTime = new GameTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime);
			relativeGameTime.ElapsedGameTime = new TimeSpan(gameTime.ElapsedGameTime.Ticks / steps);

			for (int i = 0; i < steps; i++)
			{
				box.UpdateLocation(relativeGameTime);

				if (box.Speed == Vector2.Zero)
				{
					break;
				}
				else if (box.Intersect(SpatialGrid.GetProbableCollisions(box)))
				{
					if (box.InteractWithSolid)
					{
						CollisionHelper.PhysicalCollisions(relativeGameTime, box, SpatialGrid);
					}
					SetCollisionFlags(box, SpatialGrid.GetProbableCollisions(box));
				}
			}
			CollisionHelper.StopSpeed(box, SpatialGrid);
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();
			foreach (AABB aabb in SpatialGrid.GetProbableCollisions(new AABB(null, new Rectangle(0, 0, Width, Height))))
			{
				DrawHelper.DrawOutline(spriteBatch, aabb.Rectangle, Color.Black);

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
