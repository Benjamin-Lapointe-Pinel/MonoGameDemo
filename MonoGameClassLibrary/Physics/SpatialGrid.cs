using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameClassLibrary.Physics
{
	public class SpatialGrid : EntityManager.Drawable
	{
		public static readonly int TILE_SIZE = 32;
		public List<AxisAlignedBoundingBox>[,] Tiles { get; protected set; }
		public int TilesWidth { get { return Tiles.GetLength(0); } }
		public int TilesHeight { get { return Tiles.GetLength(1); } }
		public int Width { get { return Tiles.GetLength(0) * TILE_SIZE; } }
		public int Height { get { return Tiles.GetLength(1) * TILE_SIZE; } }

		public SpatialGrid(int width, int height)
		{
			DrawOrder = int.MaxValue;

			Tiles = new List<AxisAlignedBoundingBox>[width, height];
			for (int i = 0; i < Tiles.GetLength(0); i++)
			{
				for (int j = 0; j < Tiles.GetLength(1); j++)
				{
					Tiles[i, j] = new List<AxisAlignedBoundingBox>();
				}
			}
		}

		public void AddAxisAlignedBoundingBox(AxisAlignedBoundingBox axisAlignedBoundingBox)
		{
			foreach (List<AxisAlignedBoundingBox> boxes in GetProbableCollisionTiles(axisAlignedBoundingBox))
			{
				boxes.Add(axisAlignedBoundingBox);
			}
		}

		public void RemoveAxisAlignedBoundingBox(AxisAlignedBoundingBox axisAlignedBoundingBox)
		{
			foreach (List<AxisAlignedBoundingBox> boxes in GetProbableCollisionTiles(axisAlignedBoundingBox))
			{
				boxes.Remove(axisAlignedBoundingBox);
			}
		}

		protected IEnumerable<List<AxisAlignedBoundingBox>> GetProbableCollisionTiles(AxisAlignedBoundingBox box)
		{
			int startingI = Math.Max(box.Left / TILE_SIZE, 0);
			int endingI = Math.Min((box.Right - 1) / TILE_SIZE, Tiles.GetLength(0) - 1);
			int startingJ = Math.Max(box.Top / TILE_SIZE, 0);
			int endingJ = Math.Min((box.Bottom - 1) / TILE_SIZE, Tiles.GetLength(1) - 1);
			for (int i = startingI; i <= endingI; i++)
			{
				for (int j = startingJ; j <= endingJ; j++)
				{
					yield return Tiles[i, j];
				}
			}
		}

		public IEnumerable<AxisAlignedBoundingBox> GetProbableCollisions(AxisAlignedBoundingBox axisAlignedBoundingBox)
		{
			HashSet<AxisAlignedBoundingBox> boxes = new HashSet<AxisAlignedBoundingBox>();

			foreach (List<AxisAlignedBoundingBox> tiles in GetProbableCollisionTiles(axisAlignedBoundingBox))
			{
				foreach (AxisAlignedBoundingBox box in tiles)
				{
					if (!boxes.Contains(box))
					{
						boxes.Add(box);
					}
				}
			}

			return boxes;
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			Rectangle rectangle = new Rectangle(0, 0, TILE_SIZE, TILE_SIZE);
			const float alpha = 0.5f;
			Color red = new Color(Color.Red, alpha);
			Color green = new Color(Color.Green, alpha);

			spriteBatch.Draw(DrawHelper.Pixel, new Rectangle(0, 0, Width, 1), Color.Blue);
			spriteBatch.Draw(DrawHelper.Pixel, new Rectangle(0, 0, 1, Height), Color.Blue);
			spriteBatch.Draw(DrawHelper.Pixel, new Rectangle(0, Height, Width, 1), Color.Blue);
			spriteBatch.Draw(DrawHelper.Pixel, new Rectangle(Width, 0, 1, Height), Color.Blue);

			for (int i = 0; i < Tiles.GetLength(0); i++)
			{
				for (int j = 0; j < Tiles.GetLength(1); j++)
				{
					rectangle.X = i * TILE_SIZE;
					rectangle.Y = j * TILE_SIZE;
					if (Tiles[i, j].Count == 1)
					{
						spriteBatch.Draw(DrawHelper.Pixel, rectangle, green);
					}
					else if (Tiles[i, j].Count > 1)
					{
						spriteBatch.Draw(DrawHelper.Pixel, rectangle, red);
					}
				}
			}
		}
	}
}
