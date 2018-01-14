using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameClassLibrary.Physics
{
	public class AxisAlignedBoundingBox : PhysicsEngine.Updatable, IEquatable<AxisAlignedBoundingBox>
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

		public bool Solid { get; set; }
		public bool LeftCollision { get; protected set; }
		public bool RightCollision { get; protected set; }
		public bool TopCollision { get; protected set; }
		public bool BottomCollision { get; protected set; }

		public AxisAlignedBoundingBox(Rectangle rectangle, bool solid = false)
		{
			this.rectangle = rectangle;
			this.Solid = solid;

			ResetCollisionFlags();
		}

		public AxisAlignedBoundingBox(AxisAlignedBoundingBox axisAlignedBoundingBox)
			: this(axisAlignedBoundingBox.rectangle, axisAlignedBoundingBox.Solid)
		{
		}

		public void ResetCollisionFlags()
		{
			LeftCollision = false;
			RightCollision = false;
			TopCollision = false;
			BottomCollision = false;
		}

		public void FlagLeftCollision()
		{
			LeftCollision = true;
		}

		public void FlagRightCollision()
		{
			RightCollision = true;
		}

		public void FlagTopCollision()
		{
			TopCollision = true;
		}

		public void FlagBottomCollision()
		{
			BottomCollision = true;
		}

		public static AxisAlignedBoundingBox Intersect(AxisAlignedBoundingBox value1, AxisAlignedBoundingBox value2)
		{
			return new AxisAlignedBoundingBox(Rectangle.Intersect(value1.rectangle, value2.rectangle));
		}

		public static void Intersect(ref AxisAlignedBoundingBox value1, ref AxisAlignedBoundingBox value2, out AxisAlignedBoundingBox result)
		{
			Rectangle rectangle;
			Rectangle.Intersect(ref value1.rectangle, ref value2.rectangle, out rectangle);
			result = new AxisAlignedBoundingBox(rectangle);
		}

		public static void Union(ref AxisAlignedBoundingBox value1, ref AxisAlignedBoundingBox value2, out AxisAlignedBoundingBox result)
		{
			Rectangle rectangle;
			Rectangle.Union(ref value1.rectangle, ref value2.rectangle, out rectangle);
			result = new AxisAlignedBoundingBox(rectangle);
		}

		public static AxisAlignedBoundingBox Union(AxisAlignedBoundingBox value1, AxisAlignedBoundingBox value2)
		{
			return new AxisAlignedBoundingBox(Rectangle.Union(value1.rectangle, value2.rectangle));
		}

		public void Contains(ref AxisAlignedBoundingBox value, out bool result)
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

		public bool Contains(AxisAlignedBoundingBox value)
		{
			return rectangle.Contains(value.rectangle);
		}

		public override bool Equals(object obj)
		{
			return rectangle.Equals(obj);
		}

		public bool Equals(AxisAlignedBoundingBox other)
		{
			return rectangle.Equals(other.rectangle);
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

		public void Intersects(ref AxisAlignedBoundingBox value, out bool result)
		{
			result = rectangle.Intersects(value.rectangle);
		}

		public bool Intersects(AxisAlignedBoundingBox value)
		{
			return rectangle.Intersects(value.rectangle);
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

		public static bool operator ==(AxisAlignedBoundingBox a, AxisAlignedBoundingBox b)
		{
			return a.Rectangle == b.Rectangle;
		}

		public static bool operator !=(AxisAlignedBoundingBox a, AxisAlignedBoundingBox b)
		{
			return a.Rectangle != b.Rectangle;
		}
	}
}
