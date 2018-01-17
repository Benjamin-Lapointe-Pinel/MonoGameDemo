using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameClassLibrary.Physics
{
	public class SpatialGrid : DrawableGameComponent
	{
		public static readonly int TILE_SIZE = 128;
		
		public List<AABB>[,] Tiles { get; protected set; }
		public int TilesWidth { get { return Tiles.GetLength(0); } }
		public int TilesHeight { get { return Tiles.GetLength(1); } }
		public int Width { get { return Tiles.GetLength(0) * TILE_SIZE; } }
		public int Height { get { return Tiles.GetLength(1) * TILE_SIZE; } }

		public SpatialGrid(Game game, int width, int height)
			: base(game)
		{
			Tiles = new List<AABB>[width, height];
			for (int i = 0; i < Tiles.GetLength(0); i++)
			{
				for (int j = 0; j < Tiles.GetLength(1); j++)
				{
					Tiles[i, j] = new List<AABB>();
				}
			}
		}

		public List<AABB> Add(AABB aabb)
		{
			List<AABB> collisions = new List<AABB>();

			foreach (List<AABB> tile in GetCollisionTiles(aabb))
			{
				foreach (AABB box in tile)
				{
					collisions.Add(box);
				}
				tile.Add(aabb);
			}

			return collisions;
		}

		public List<AABB> Remove(AABB aabb)
		{
			List<AABB> collisions = new List<AABB>();

			foreach (List<AABB> tile in GetCollisionTiles(aabb))
			{
				foreach (AABB box in tile)
				{
					collisions.Add(box);
				}
				tile.Remove(aabb);
			}

			return collisions;
		}		

		public IEnumerable<AABB> GetProbableSolidCollisions(AABB aabb)
		{
			foreach (AABB box in GetProbableCollisions(aabb))
			{
				if (box.Solid)
				{
					yield return box;
				}
			}
		}

		public IEnumerable<AABB> GetProbableCollisions(AABB aabb)
		{
			List<AABB> boxes = new List<AABB>();

			foreach (List<AABB> tiles in GetCollisionTiles(aabb))
			{
				foreach (AABB box in tiles)
				{
					if ((aabb != box) && !boxes.Contains(box))
					{
						boxes.Add(box);
					}
				}
			}

			return boxes;
		}

		protected IEnumerable<List<AABB>> GetCollisionTiles(AABB aabb)
		{
			int startingI = Math.Max((aabb.Rectangle.Left - 1) / TILE_SIZE, 0);
			int endingI = Math.Min(aabb.Rectangle.Right / TILE_SIZE, Tiles.GetLength(0) - 1);
			int startingJ = Math.Max((aabb.Rectangle.Top - 1) / TILE_SIZE, 0);
			int endingJ = Math.Min(aabb.Rectangle.Bottom / TILE_SIZE, Tiles.GetLength(1) - 1);
			for (int i = startingI; i <= endingI; i++)
			{
				for (int j = startingJ; j <= endingJ; j++)
				{
					yield return Tiles[i, j];
				}
			}
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();
			DrawHelper.DrawOutline(spriteBatch, new Rectangle(0, 0, Width, Height), Color.Blue);

			Rectangle rectangle = new Rectangle(0, 0, TILE_SIZE, TILE_SIZE);
			Color red = new Color(Color.Red, 0.25f);
			Color blue = new Color(Color.Blue, 0.5f);

			for (int i = 0; i < Tiles.GetLength(0); i++)
			{
				for (int j = 0; j < Tiles.GetLength(1); j++)
				{
					rectangle.X = i * TILE_SIZE;
					rectangle.Y = j * TILE_SIZE;
					if (Tiles[i, j].Count == 1)
					{
						DrawHelper.DrawOutline(spriteBatch, rectangle, blue);
					}
					else if (Tiles[i, j].Count > 1)
					{
						DrawHelper.DrawOutline(spriteBatch, rectangle, red);
					}
				}
			}
		}
	}
}
