﻿using Microsoft.Xna.Framework;
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
		public static readonly int TILE_SIZE = 128;

		public List<Box>[,] Tiles { get; protected set; }
		public int TilesWidth { get { return Tiles.GetLength(0); } }
		public int TilesHeight { get { return Tiles.GetLength(1); } }
		public int Width { get { return Tiles.GetLength(0) * TILE_SIZE; } }
		public int Height { get { return Tiles.GetLength(1) * TILE_SIZE; } }

		public SpatialGrid(int width, int height)
		{
			DrawOrder = int.MaxValue;

			Tiles = new List<Box>[width, height];
			for (int i = 0; i < Tiles.GetLength(0); i++)
			{
				for (int j = 0; j < Tiles.GetLength(1); j++)
				{
					Tiles[i, j] = new List<Box>();
				}
			}
		}

		public void AddBox(Box axisAlignedBoundingBox)
		{
			foreach (List<Box> boxes in GetCollisionTiles(axisAlignedBoundingBox))
			{
				boxes.Add(axisAlignedBoundingBox);
			}
		}

		public void RemoveBox(Box axisAlignedBoundingBox)
		{
			foreach (List<Box> boxes in GetCollisionTiles(axisAlignedBoundingBox))
			{
				boxes.Remove(axisAlignedBoundingBox);
			}
		}

		public IEnumerable<Box> GetProbableSolidCollisions(Box mainBox)
		{
			foreach (Box box in GetProbableCollisions(mainBox))
			{
				if (box.Solid)
				{
					yield return box;
				}
			}
		}

		public IEnumerable<Box> GetProbableCollisions(Box mainBox)
		{
			List<Box> boxes = new List<Box>();

			foreach (List<Box> tiles in GetCollisionTiles(mainBox))
			{
				foreach (Box box in tiles)
				{
					if (!boxes.Contains(box))
					{
						boxes.Add(box);
					}
				}
			}

			return boxes;
		}

		protected IEnumerable<List<Box>> GetCollisionTiles(Box box)
		{
			int startingI = Math.Max((box.Left - 1) / TILE_SIZE, 0);
			int endingI = Math.Min(box.Right / TILE_SIZE, Tiles.GetLength(0) - 1);
			int startingJ = Math.Max((box.Top - 1) / TILE_SIZE, 0);
			int endingJ = Math.Min(box.Bottom / TILE_SIZE, Tiles.GetLength(1) - 1);
			for (int i = startingI; i <= endingI; i++)
			{
				for (int j = startingJ; j <= endingJ; j++)
				{
					yield return Tiles[i, j];
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
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
