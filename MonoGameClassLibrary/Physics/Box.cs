using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameClassLibrary.Physics
{
	public partial class Box : EntityManager.Drawable
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

		public class CollisionEventArgs : EventArgs
		{
			public Box Box { get; protected set; }
			public Side CollisionSide { get; protected set; }

			public CollisionEventArgs(Box box, Side collisionSide)
				: base()
			{
				this.Box = box;
				this.CollisionSide = collisionSide;
			}
		}

		public delegate void CollisionHandler(Box sender, CollisionEventArgs e);
		public event CollisionHandler OnCollision;

		public Dictionary<Box, Side> Collisions { get; protected set; }

		public Vector2 Acceleration;
		public Vector2 Speed;
		protected Vector2 Vector2Location;//ASDFGHGGSDVDFGD

		public bool Solid { get; set; }
		public bool InteractWithSolid { get; set; }
		public bool Collisionable { get; set; }

		public Box(Rectangle rectangle, bool solid = false, bool interactWithSolid = false, bool collisionable = false)
		{
			this.Rectangle = rectangle;
			this.Solid = solid;
			this.InteractWithSolid = interactWithSolid;
			this.Collisionable = collisionable;

			Collisions = new Dictionary<Box, Side>();

			Acceleration = new Vector2(0, 0);
			Speed = new Vector2(0, 0);
			Vector2Location = Location.ToVector2();
		}

		public Box(Box box)
			: this(box.Rectangle, box.Solid, box.InteractWithSolid)
		{
		}

		public virtual void PhysicsUpdate(GameTime gameTime) { }

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

		public void ClearCollisions()
		{
			Collisions.Clear();
		}

		public void AddCollision(Box box, Side side = Side.Unknown)
		{
			if (Collisions.ContainsKey(box))
			{
				Collisions[box] |= side;
			}
			else
			{
				Collisions.Add(box, side);
				OnCollision?.Invoke(this, new CollisionEventArgs(box, side));
			}
		}

		public bool SolidLeftCollision()
		{
			foreach (var collisions in LeftCollision())
			{
				if (collisions.Solid)
				{
					return true;
				}
			}
			return false;
		}

		public bool SolidRightCollision()
		{
			foreach (var collisions in RightCollision())
			{
				if (collisions.Solid)
				{
					return true;
				}
			}
			return false;
		}

		public bool SolidTopCollision()
		{
			foreach (var collisions in TopCollision())
			{
				if (collisions.Solid)
				{
					return true;
				}
			}
			return false;
		}

		public bool SolidBottomCollision()
		{
			foreach (var collisions in BottomCollision())
			{
				if (collisions.Solid)
				{
					return true;
				}
			}
			return false;
		}

		public IEnumerable<Box> LeftCollision()
		{
			foreach (KeyValuePair<Box, Box.Side> collisions in Collisions)
			{
				if (collisions.Value.HasFlag(Box.Side.Left))
				{
					yield return collisions.Key;
				}
			}
		}

		public IEnumerable<Box> RightCollision()
		{
			foreach (KeyValuePair<Box, Box.Side> collisions in Collisions)
			{
				if (collisions.Value.HasFlag(Box.Side.Right))
				{
					yield return collisions.Key;
				}
			}
		}

		public IEnumerable<Box> TopCollision()
		{
			foreach (KeyValuePair<Box, Box.Side> collisions in Collisions)
			{
				if (collisions.Value.HasFlag(Box.Side.Top))
				{
					yield return collisions.Key;
				}
			}
		}

		public IEnumerable<Box> BottomCollision()
		{
			foreach (KeyValuePair<Box, Box.Side> collisions in Collisions)
			{
				if (collisions.Value.HasFlag(Box.Side.Bottom))
				{
					yield return collisions.Key;
				}
			}
		}
	}
}