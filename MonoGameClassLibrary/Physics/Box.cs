using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameClassLibrary.Physics
{
	public partial class Box : PhysicsEngine.Updatable
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

		public object Owner { get; protected set; }
		public Dictionary<Box, Side> Collisions { get; protected set; }

		public bool Solid { get; set; }
		public bool InteractWithSolid { get; set; }
		public bool AffectedByGravity { get; set; }

		public Vector2 Speed;
		protected Vector2 Vector2Location;

		public Box(object owner, Rectangle rectangle, bool solid = false, bool interactWithSolid = false, bool affectedByGravity = false)
		{
			this.Owner = owner;
			this.Rectangle = rectangle;
			this.Solid = solid;
			this.InteractWithSolid = interactWithSolid;
			this.AffectedByGravity = affectedByGravity;

			Collisions = new Dictionary<Box, Side>();
			Vector2Location = Location.ToVector2();
			Speed = new Vector2(0, 0);
		}

		public Box(Box box)
			: this(box.Owner, box.Rectangle, box.Solid, box.InteractWithSolid, box.AffectedByGravity)
		{
		}

		public override void Update(GameTime gameTime)
		{
			Vector2Location += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			rectangle.X = (int)Math.Round(Vector2Location.X, 0);
			rectangle.Y = (int)Math.Round(Vector2Location.Y, 0);
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
			}
			OnCollision?.Invoke(this, new CollisionEventArgs(box, side));
		}
	}
}
