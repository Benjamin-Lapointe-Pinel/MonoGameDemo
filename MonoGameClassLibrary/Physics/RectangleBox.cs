using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameClassLibrary.Physics
{
	partial class Box
	{
		protected Rectangle rectangle;
		public Rectangle Rectangle { get { return rectangle; } protected set { rectangle = value; } }
		public int X { get { return rectangle.X; } set { rectangle.X = value; } }
		public int Y { get { return rectangle.Y; } set { rectangle.Y = value; } }
		public int Width { get { return rectangle.Width; } set { rectangle.Width = value; } }
		public int Height { get { return rectangle.Height; } set { rectangle.Height = value; } }
		public Point Location { get { return rectangle.Location; } set { rectangle.Location = value; } }
		public int Bottom { get { return rectangle.Bottom; } }
		public int Top { get { return rectangle.Top; } }
		public int Right { get { return rectangle.Right; } }
		public int Left { get { return rectangle.Left; } }
		public Point Size { get { return rectangle.Size; } set { rectangle.Size = value; } }
		public Point Center { get { return rectangle.Center; } }

		public static Box Intersect(Box value1, Box value2)
		{
			return new Box(Rectangle.Intersect(value1.rectangle, value2.rectangle));
		}

		public static void Intersect(ref Box value1, ref Box value2, out Box result)
		{
			result = new Box(Rectangle.Intersect(value1.rectangle, value2.rectangle));
		}

		public void Intersects(ref Box value, out bool result)
		{
			result = rectangle.Intersects(value.rectangle);
		}

		public bool Intersects(Box value)
		{
			return rectangle.Intersects(value.rectangle);
		}

		public static void Union(ref Box value1, ref Box value2, out Box result)
		{			
			result = new Box(Rectangle.Union(value1.rectangle, value2.rectangle));
		}

		public static Box Union(Box value1, Box value2)
		{
			return new Box( Rectangle.Union(value1.rectangle, value2.rectangle));
		}

		public void Contains(ref Box value, out bool result)
		{
			rectangle.Contains(ref value.rectangle, out result);
		}

		public bool Contains(int x, int y)
		{
			return rectangle.Contains(x, y);
		}
		public void Contains(ref Vector2 value, out bool result)
		{
			result = rectangle.Contains(value);
		}

		public bool Contains(float x, float y)
		{
			return rectangle.Contains(x, y);
		}

		public bool Contains(Point value)
		{
			return rectangle.Contains(value);
		}

		public void Contains(ref Point value, out bool result)
		{
			result = rectangle.Contains(value);
		}

		public bool Contains(Vector2 value)
		{
			return rectangle.Contains(value);
		}

		public bool Contains(Box value)
		{
			return rectangle.Contains(value.rectangle);
		}

		public override int GetHashCode()
		{
			return rectangle.GetHashCode();
		}

		public void Inflate(float horizontalAmount, float verticalAmount)
		{
			rectangle.Inflate(horizontalAmount, verticalAmount);
		}

		public void Inflate(int horizontalAmount, int verticalAmount)
		{
			rectangle.Inflate(horizontalAmount, verticalAmount);
		}

		public void Offset(Vector2 amount)
		{
			rectangle.Offset(amount);
		}

		public void Offset(float offsetX, float offsetY)
		{
			rectangle.Offset(offsetX, offsetY);
		}

		public void Offset(int offsetX, int offsetY)
		{
			rectangle.Offset(offsetX, offsetY);
		}

		public void Offset(Point amount)
		{
			rectangle.Offset(amount);
		}

		public override string ToString()
		{
			return rectangle.ToString();
		}
	}
}
