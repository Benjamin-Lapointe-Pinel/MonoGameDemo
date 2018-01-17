using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameClassLibrary.Physics
{
	public class AABB : DrawableGameComponent
	{
		[Flags]
		public enum CollisionDirection
		{
			None = 0,
			Inside = 1,
			Left = 2,
			Right = 4,
			Top = 8,
			Bottom = 16,
		}

		public class CollisionEventArgs : EventArgs
		{
			public AABB CollidedWith { get; protected set; }
			public CollisionDirection CollisionSide { get; protected set; }

			public CollisionEventArgs(AABB collidedWith, CollisionDirection collisionSide)
				: base()
			{
				this.CollidedWith = collidedWith;
				this.CollisionSide = collisionSide;
			}
		}

		public delegate void CollisionHandler(AABB sender, CollisionEventArgs e);
		public event CollisionHandler OnCollision;

		public delegate void PropertyChangedHandler(AABB sender, PropertyChangedEventArgs e);
		public event PropertyChangedHandler PropertyWillChange;
		public event PropertyChangedHandler PropertyHasChange;

		protected static PropertyChangedEventArgs PropertyChangedEventArgsX = new PropertyChangedEventArgs("X");
		protected static PropertyChangedEventArgs PropertyChangedEventArgsY = new PropertyChangedEventArgs("Y");
		protected static PropertyChangedEventArgs PropertyChangedEventArgsWidth = new PropertyChangedEventArgs("Width");
		protected static PropertyChangedEventArgs PropertyChangedEventArgsHeight = new PropertyChangedEventArgs("Height");

		public float x;
		public float X
		{
			get
			{
				return x;
			}
			set
			{
				PropertyWillChange?.Invoke(this, PropertyChangedEventArgsX);
				x = value;
				PropertyHasChange?.Invoke(this, PropertyChangedEventArgsX);
			}
		}
		public float y;
		public float Y
		{
			get
			{
				return y;
			}
			set
			{
				PropertyWillChange?.Invoke(this, PropertyChangedEventArgsY);
				y = value;
				PropertyHasChange?.Invoke(this, PropertyChangedEventArgsY);
			}
		}
		public float width;
		public float Width
		{
			get
			{
				return width;
			}
			set
			{
				PropertyWillChange?.Invoke(this, PropertyChangedEventArgsWidth);
				width = value;
				PropertyHasChange?.Invoke(this, PropertyChangedEventArgsWidth);
			}
		}
		public float height;
		public float Height
		{
			get
			{
				return height;
			}
			set
			{
				PropertyWillChange?.Invoke(this, PropertyChangedEventArgsHeight);
				height = value;
				PropertyHasChange?.Invoke(this, PropertyChangedEventArgsHeight);
			}
		}
		public float Left { get { return X; } }
		public float Right { get { return Left + Width; } }
		public float Top { get { return Y; } }
		public float Bottom { get { return Top + Height; } }
		public bool Solid { get; set; }

		public Rectangle Rectangle
		{
			get
			{
				return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
				//return new Rectangle((int)Math.Round(X, 0), (int)Math.Round(Y, 0), (int)Math.Round(Width, 0), (int)Math.Round(Height, 0));
			}
			set
			{
				X = value.X;
				Y = value.Y;
				Width = value.Width;
				Height = value.Height;
			}
		}

		public Vector2 Location
		{
			get
			{
				return new Vector2(X, Y);
			}
			set
			{
				X = value.X;
				Y = value.Y;
			}
		}

		public Vector2 Size
		{
			get
			{
				return new Vector2(Width, Height);
			}
			set
			{
				Width = value.X;
				Height = value.Y;
			}
		}

		public Vector2 Center
		{
			get
			{
				return new Vector2((Right - Left) / 2, (Bottom - Top) / 2);
			}
		}

		public AABB(Game game, Rectangle rectangle, bool solid = false)
			: base(game)
		{
			this.Rectangle = rectangle;
			this.Solid = solid;

			UpdateOrder = int.MaxValue - 1;
		}

		public AABB(AABB aabb)
			: this(aabb.Game, aabb.Rectangle, aabb.Solid)
		{
		}

		public void CollisionNotification(AABB aabb)
		{
			CollisionDirection side = CollisionDirection.None;
			if (this.Intersects(aabb))
			{
				side |= CollisionDirection.Inside;
			}
			if (LeftCollision(aabb))
			{
				side |= CollisionDirection.Left;
			}
			if (RightCollision(aabb))
			{
				side |= CollisionDirection.Right;
			}
			if (TopCollision(aabb))
			{
				side |= CollisionDirection.Top;
			}
			if (BottomCollision(aabb))
			{
				side |= CollisionDirection.Bottom;
			}

			OnCollision?.Invoke(this, new CollisionEventArgs(aabb, side));
		}

		public virtual bool LeftCollision(AABB aabb)
		{
			x -= 1;
			bool result = Intersects(aabb);
			x += 1;

			return result;
		}

		public virtual bool RightCollision(AABB aabb)
		{
			x += 1;
			bool result = Intersects(aabb);
			x -= 1;

			return result;
		}

		public virtual bool TopCollision(AABB aabb)
		{
			y -= 1;
			bool result = Intersects(aabb);
			y += 1;

			return result;
		}

		public virtual bool BottomCollision(AABB aabb)
		{
			y += 1;
			bool result = Intersects(aabb);
			y -= 1;

			return result;
		}

		public bool LeftCollision(IEnumerable<AABB> aabbs)
		{
			foreach (AABB aabb in aabbs)
			{
				if (LeftCollision(aabb))
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool RightCollision(IEnumerable<AABB> aabbs)
		{
			foreach (AABB aabb in aabbs)
			{
				if (RightCollision(aabb))
				{
					return true;
				}
			}
			return false;
		}

		public bool TopCollision(IEnumerable<AABB> aabbs)
		{
			foreach (AABB aabb in aabbs)
			{
				if (TopCollision(aabb))
				{
					return true;
				}
			}
			return false;
		}

		public bool BottomCollision(IEnumerable<AABB> aabbs)
		{
			foreach (AABB aabb in aabbs)
			{
				if (BottomCollision(aabb))
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool Intersects(AABB value)
		{
			return Rectangle.Intersects(value.Rectangle);
		}

		public bool Intersect(IEnumerable<AABB> aabbs)
		{
			foreach (AABB aabb in aabbs)
			{
				if (Rectangle.Intersects(aabb.Rectangle))
				{
					return true;
				}
			}
			return false;
		}

		#region MonoGame Methods

		public bool Contains(int x, int y)
		{
			return ((((this.X <= x) && (x < (this.X + this.Width))) && (this.Y <= y)) && (y < (this.Y + this.Height)));
		}

		public bool Contains(float x, float y)
		{
			return ((((this.X <= x) && (x < (this.X + this.Width))) && (this.Y <= y)) && (y < (this.Y + this.Height)));
		}

		public bool Contains(Point value)
		{
			return ((((this.X <= value.X) && (value.X < (this.X + this.Width))) && (this.Y <= value.Y)) && (value.Y < (this.Y + this.Height)));
		}

		public void Contains(ref Point value, out bool result)
		{
			result = ((((this.X <= value.X) && (value.X < (this.X + this.Width))) && (this.Y <= value.Y)) && (value.Y < (this.Y + this.Height)));
		}

		public bool Contains(Vector2 value)
		{
			return ((((this.X <= value.X) && (value.X < (this.X + this.Width))) && (this.Y <= value.Y)) && (value.Y < (this.Y + this.Height)));
		}

		public void Contains(ref Vector2 value, out bool result)
		{
			result = ((((this.X <= value.X) && (value.X < (this.X + this.Width))) && (this.Y <= value.Y)) && (value.Y < (this.Y + this.Height)));
		}

		public bool Contains(AABB value)
		{
			return ((((this.X <= value.X) && ((value.X + value.Width) <= (this.X + this.Width))) && (this.Y <= value.Y)) && ((value.Y + value.Height) <= (this.Y + this.Height)));
		}

		public void Contains(ref AABB value, out bool result)
		{
			result = ((((this.X <= value.X) && ((value.X + value.Width) <= (this.X + this.Width))) && (this.Y <= value.Y)) && ((value.Y + value.Height) <= (this.Y + this.Height)));
		}

		public void Inflate(float horizontalAmount, float verticalAmount)
		{
			X -= horizontalAmount;
			Y -= verticalAmount;
			Width += horizontalAmount * 2;
			Height += verticalAmount * 2;
		}

		public void Offset(float offsetX, float offsetY)
		{
			X += offsetX;
			Y += offsetY;
		}

		public void Offset(Point amount)
		{
			X += amount.X;
			Y += amount.Y;
		}

		public void Offset(Vector2 amount)
		{
			X += amount.X;
			Y += amount.Y;
		}

		public override string ToString()
		{
			return "{X:" + X + " Y:" + Y + " Width:" + Width + " Height:" + Height + "}";
		}

		#endregion
	}
}
