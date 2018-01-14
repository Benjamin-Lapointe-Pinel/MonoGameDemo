using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameClassLibrary.Physics
{
	public class Box : AxisAlignedBoundingBox
	{		
		public bool IsAffectedByGravity;
		public Vector2 Speed;
		protected Vector2 Vector2Location;

		public Box(Rectangle rectangle, bool solid = true, bool isAffectedByGravity = true)
			: base(rectangle, solid)
		{
			this.IsAffectedByGravity = isAffectedByGravity;

			Vector2Location = Location.ToVector2();
			Speed = new Vector2(0, 0);

			ResetCollisionFlags();
		}

		public override void Update(GameTime gameTime)
		{
			Vector2Location += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			rectangle.X = (int)Math.Round(Vector2Location.X, 0);
			rectangle.Y = (int)Math.Round(Vector2Location.Y, 0);
		}
	}
}
