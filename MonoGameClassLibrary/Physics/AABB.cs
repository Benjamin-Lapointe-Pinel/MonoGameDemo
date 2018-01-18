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

		public delegate void PropertyChangingEventHandler(AABB aabb, PropertyChangingEventArgs e);
		public delegate void PropertyChangedEventHandler(AABB aabb, PropertyChangedEventArgs e);
		public event PropertyChangingEventHandler PropertyChanging;
		public event PropertyChangedEventHandler PropertyChanged;
		protected static PropertyChangingEventArgs PropertyChangingEventArgsLocation = new PropertyChangingEventArgs("Location");
		protected static PropertyChangedEventArgs PropertyChangedEventArgsLocation = new PropertyChangedEventArgs("Location");
		protected static PropertyChangingEventArgs PropertyChangingEventArgsSize = new PropertyChangingEventArgs("Size");
		protected static PropertyChangedEventArgs PropertyChangedEventArgsSize = new PropertyChangedEventArgs("Size");

		protected float x;
		protected float y;
		protected float width;
		protected float height;
		public float X { get { return x; } }
		public float Y { get { return y; } }
		public float Width { get { return width; } }
		public float Height { get { return height; } }
		public float Left { get { return X; } }
		public float Right { get { return Left + Width; } }
		public float Top { get { return Y; } }
		public float Bottom { get { return Top + Height; } }
		public bool Solid { get; set; }

		public Vector2 OldLocation { get; protected set; }
		public Vector2 OldSize { get; protected set; }
		public Rectangle Rectangle
		{
			get
			{
				return new Rectangle((int)Math.Round(X, 0), (int)Math.Round(Y, 0), (int)Math.Round(Width, 0), (int)Math.Round(Height, 0));
			}
			set
			{
				PropertyChanging?.Invoke(this, PropertyChangingEventArgsSize);
				OldSize = Size;
				x = value.X;
				y = value.Y;
				width = value.Width;
				height = value.Height;
				PropertyChanged?.Invoke(this, PropertyChangedEventArgsSize);
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
				PropertyChanging?.Invoke(this, PropertyChangingEventArgsLocation);
				OldLocation = Location;
				x = value.X;
				y = value.Y;
				PropertyChanged?.Invoke(this, PropertyChangedEventArgsLocation);
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
				PropertyChanging?.Invoke(this, PropertyChangingEventArgsSize);
				OldSize = Size;
				width = value.X;
				height = value.Y;
				PropertyChanged?.Invoke(this, PropertyChangedEventArgsSize);
			}
		}

		public Vector2 Center
		{
			get
			{
				return new Vector2((Right - Left) / 2, (Bottom - Top) / 2);
			}
		}

		public AABB(Game game, float x, float y, float width, float height, bool solid = false)
			: base(game)
		{
			this.Location = new Vector2(x, y);
			this.Size = new Vector2(width, height);
			this.Solid = solid;

			UpdateOrder = Int16.MaxValue;
		}

		public AABB(AABB aabb)
			: this(aabb.Game, aabb.X, aabb.Y, aabb.Width, aabb.Height, aabb.Solid)
		{
		}

		//TODO : retirer CollisionDirection.None. On devrait que passer des vrais collisions ici
		public void CollisionNotification(AABB aabb)
		{
			CollisionDirection senderSide = CollisionDirection.None;
			CollisionDirection receiverSide = CollisionDirection.None;

			if (Intersects(aabb))
			{
				senderSide |= CollisionDirection.Inside;
				receiverSide |= CollisionDirection.Inside;
			}
			else
			{
				if (this is Box)
				{
					Box box = this as Box;
					if ((box.Speed.X <= 0) && LeftCollision(aabb))
					{
						senderSide |= CollisionDirection.Left;
						receiverSide |= CollisionDirection.Right;
					}
					if ((box.Speed.X >= 0) && RightCollision(aabb))
					{
						senderSide |= CollisionDirection.Right;
						receiverSide |= CollisionDirection.Left;
					}
					if ((box.Speed.Y <= 0) && TopCollision(aabb))
					{
						senderSide |= CollisionDirection.Top;
						receiverSide |= CollisionDirection.Bottom;
					}
					if ((box.Speed.Y >= 0) && BottomCollision(aabb))
					{
						senderSide |= CollisionDirection.Bottom;
						receiverSide |= CollisionDirection.Top;
					}
				}
				else
				{
					if (LeftCollision(aabb))
					{
						senderSide |= CollisionDirection.Left;
						receiverSide |= CollisionDirection.Right;
					}
					if (RightCollision(aabb))
					{
						senderSide |= CollisionDirection.Right;
						receiverSide |= CollisionDirection.Left;
					}
					if (TopCollision(aabb))
					{
						senderSide |= CollisionDirection.Top;
						receiverSide |= CollisionDirection.Bottom;
					}
					if (BottomCollision(aabb))
					{
						senderSide |= CollisionDirection.Bottom;
						receiverSide |= CollisionDirection.Top;
					}
				}
			}

			if (senderSide != CollisionDirection.None)
			{
				OnCollision?.Invoke(this, new CollisionEventArgs(aabb, senderSide));
			}
			if (receiverSide != CollisionDirection.None)
			{
				aabb.OnCollision?.Invoke(aabb, new CollisionEventArgs(this, receiverSide));
			}
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

		public virtual bool Intersects(AABB value)
		{
			return Rectangle.Intersects(value.Rectangle);
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

		public void Inflate(float horizontalAmount, float verticalAmount)
		{
			PropertyChanging?.Invoke(this, PropertyChangingEventArgsSize);
			OldSize = Size;
			x -= horizontalAmount;
			y -= verticalAmount;
			width += horizontalAmount * 2;
			height += verticalAmount * 2;
			PropertyChanged?.Invoke(this, PropertyChangedEventArgsSize);
		}

		public void Offset(float offsetX, float offsetY)
		{
			Location = new Vector2(X + offsetX, Y + offsetY);
		}

		public void Offset(Point amount)
		{
			Location = new Vector2(X + amount.X, Y + amount.Y);
		}

		public void Offset(Vector2 amount)
		{
			Location = new Vector2(X + amount.X, Y + amount.Y);
		}

		public override string ToString()
		{
			return GetType().Name + " {X:" + X + " Y:" + Y + " Width:" + Width + " Height:" + Height + "}";
		}

		#endregion
	}
}
