using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameClassLibrary.Physics
{
	public class CollisionBox : Box
	{
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

		public delegate void CollisionHandler(CollisionBox sender, CollisionEventArgs e);
		public event CollisionHandler OnCollision;

		public Dictionary<Box, Side> Collisions { get; protected set; }

		public CollisionBox(Game game, Rectangle rectangle, bool solid = false, bool interactWithSolid = false, bool preciseCollision = false)
			: base(game, rectangle, solid, interactWithSolid, preciseCollision)
		{
			Collisions = new Dictionary<Box, Side>();
		}

		public CollisionBox(CollisionBox box)
			: this(box.Game, box.Rectangle, box.Solid, box.InteractWithSolid, box.PreciseCollision)
		{
		}

		public void ClearCollisions()
		{
			Collisions.Clear();
		}

		public override void Collided(Box box, Side side = Side.Unknown)
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
			foreach (KeyValuePair<Box, Side> collisions in Collisions)
			{
				if (collisions.Value.HasFlag(Side.Left))
				{
					yield return collisions.Key;
				}
			}
		}

		public IEnumerable<Box> RightCollision()
		{
			foreach (KeyValuePair<Box, Side> collisions in Collisions)
			{
				if (collisions.Value.HasFlag(Side.Right))
				{
					yield return collisions.Key;
				}
			}
		}

		public IEnumerable<Box> TopCollision()
		{
			foreach (KeyValuePair<Box, Side> collisions in Collisions)
			{
				if (collisions.Value.HasFlag(Side.Top))
				{
					yield return collisions.Key;
				}
			}
		}

		public IEnumerable<Box> BottomCollision()
		{
			foreach (KeyValuePair<Box, Side> collisions in Collisions)
			{
				if (collisions.Value.HasFlag(Side.Bottom))
				{
					yield return collisions.Key;
				}
			}
		}
	}
}