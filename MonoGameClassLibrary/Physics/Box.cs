using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameClassLibrary.Physics
{
	public class Box : DrawableGameComponent
	{
		[Flags]
		public enum Side
		{
			None = 0,
			Unknown = 1,
			Left = 2,
			Right = 4,
			Top = 8,
			Bottom = 16,
		}

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
		public bool InteractWithSolid { get; set; }
		public bool PreciseCollision { get; internal set; }

		public Vector2 Acceleration;
		public Vector2 Speed;
		protected Vector2 Vector2Location;//ASDFGHGGSDVDFGD

		public Box(Game game, Rectangle rectangle, bool solid = false, bool interactWithSolid = false, bool preciseCollision = false)
			: base(game)
		{
			this.Rectangle = rectangle;
			this.Solid = solid;
			this.InteractWithSolid = interactWithSolid;
			this.PreciseCollision = preciseCollision;

			Acceleration = new Vector2(0, 0);
			Speed = new Vector2(0, 0);
			Vector2Location = Location.ToVector2();
		}

		public Box(Box box)
			: this(box.Game, box.Rectangle, box.Solid, box.InteractWithSolid, box.PreciseCollision)
		{
		}

		public void UpdateLocation(GameTime gameTime)
		{
			Vector2Location += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			rectangle.X = (int)Math.Round(Vector2Location.X, 0);
			rectangle.Y = (int)Math.Round(Vector2Location.Y, 0);
		}

		public void UpdateSpeed(GameTime gameTime)
		{
			Speed += Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
		}

		public virtual void Collided(Box box, Side side = Side.Unknown) { }

		public void Intersects(ref Box value, out bool result)
		{
			result = rectangle.Intersects(value.rectangle);
		}

		public bool Intersects(Box value)
		{
			return rectangle.Intersects(value.rectangle);
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