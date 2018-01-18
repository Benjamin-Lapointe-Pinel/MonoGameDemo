using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameClassLibrary.Physics
{
	//Je n'aime pas cette classe
	public class SmartBox : Box
	{
		public Dictionary<AABB, CollisionDirection> Collisions { get; protected set; }

		public SmartBox(Game game, float x, float y, float width, float height, bool solid = false, bool interactWithSolid = false, float movementIncrement = 0)
			: base(game, x, y, width, height, solid, interactWithSolid, movementIncrement)
		{
			Collisions = new Dictionary<AABB, CollisionDirection>();

			OnCollision += SmartBox_OnCollision;
		}

		public SmartBox(SmartBox box)
			: this(box.Game, box.X, box.Y, box.Width, box.Height, box.Solid, box.InteractWithSolid, box.MovementIncrement)
		{
		}

		public override void Update(GameTime gameTime)
		{
			Collisions.Clear();
		}

		private void SmartBox_OnCollision(AABB sender, CollisionEventArgs e)
		{
			if (Collisions.ContainsKey(e.CollidedWith))
			{
				Collisions[e.CollidedWith] |= e.CollisionSide;
			}
			else
			{
				Collisions.Add(e.CollidedWith, e.CollisionSide);
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

		public IEnumerable<AABB> LeftCollision()
		{
			foreach (KeyValuePair<AABB, CollisionDirection> collisions in Collisions)
			{
				if (collisions.Value.HasFlag(CollisionDirection.Left))
				{
					yield return collisions.Key;
				}
			}
		}

		public IEnumerable<AABB> RightCollision()
		{
			foreach (KeyValuePair<AABB, CollisionDirection> collisions in Collisions)
			{
				if (collisions.Value.HasFlag(CollisionDirection.Right))
				{
					yield return collisions.Key;
				}
			}
		}

		public IEnumerable<AABB> TopCollision()
		{
			foreach (KeyValuePair<AABB, CollisionDirection> collisions in Collisions)
			{
				if (collisions.Value.HasFlag(CollisionDirection.Top))
				{
					yield return collisions.Key;
				}
			}
		}

		public IEnumerable<AABB> BottomCollision()
		{
			foreach (KeyValuePair<AABB, CollisionDirection> collisions in Collisions)
			{
				if (collisions.Value.HasFlag(CollisionDirection.Bottom))
				{
					yield return collisions.Key;
				}
			}
		}
	}
}