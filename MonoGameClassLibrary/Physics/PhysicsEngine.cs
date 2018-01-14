using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static MonoGameClassLibrary.EntityManager;

namespace MonoGameClassLibrary.Physics
{
	//TODO: Spatial Grid
	public class PhysicsEngine : EntityManager.Drawable
	{
		public SpatialGrid spatialGrid { get; protected set; }
		protected List<Box> boxes;
		public Vector2 Gravity;

		public PhysicsEngine()
		{
			spatialGrid = new SpatialGrid(100, 100);
			boxes = new List<Box>();
			Gravity = new Vector2(0, 5000);

			DrawOrder = int.MaxValue;
		}

		public void Add(Box box)
		{
			boxes.Add(box);
			spatialGrid.AddAxisAlignedBoundingBox(box);
		}

		public void Remove(Box box)
		{
			boxes.Remove(box);
			spatialGrid.RemoveAxisAlignedBoundingBox(box);
		}
		
		public override void Update(GameTime gameTime)
		{
			ApplyPhysics(gameTime);
			ApplyCollisions();
		}

		public void ApplyCollisions()
		{
			foreach (Box box in boxes)
			{
				spatialGrid.RemoveAxisAlignedBoundingBox(box);

				CollisionHelper.SetCollisionFlag(box, spatialGrid);

				spatialGrid.AddAxisAlignedBoundingBox(box);
			}
		}

		//BUG : résolution de collision de deux objets qui bougent, pas précis
		private void ApplyPhysics(GameTime gameTime)
		{
			foreach (Box box in boxes)
			{
				spatialGrid.RemoveAxisAlignedBoundingBox(box);

				if (box.AffectedByGravity)
				{
					float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
					box.Speed += Gravity * totalSeconds;
				}

				if (box.Speed != Vector2.Zero)
				{
					int steps = 1;
					GameTime relativeGameTime = new GameTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime);
					if (box.Speed.Length() > SpatialGrid.TILE_SIZE)
					{
						//Tunneling resolution
						steps = (int)Math.Ceiling(box.Speed.Length() / SpatialGrid.TILE_SIZE);
						relativeGameTime.ElapsedGameTime = new TimeSpan(gameTime.ElapsedGameTime.Ticks / steps);
					}
					for (int i = 0; i < steps; i++)
					{
						box.Update(relativeGameTime);

						CollisionHelper.PhysicalCollisions(relativeGameTime, box, spatialGrid);
					}
				}

				spatialGrid.AddAxisAlignedBoundingBox(box);
			}
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			foreach (Box box in boxes)
			{
				spriteBatch.Draw(DrawHelper.Pixel, box.Rectangle, new Color(Color.Magenta, 0.5f));
			}
			spatialGrid.Draw(spriteBatch, gameTime);
		}

		public class Updatable
		{
			public virtual void Update(GameTime gameTime) { }
		}
	}
}
