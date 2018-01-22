using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameClassLibrary.Physics
{
	public class KineticBox : Box
	{
		public bool InteractWithSolid { get; set; }
		public float MovementIncrement { get; set; }
		public bool SpeedUpdate { get; protected set; }

		public Vector2 Acceleration;
		public Vector2 Speed;

		public KineticBox(Game game, float x, float y, float width, float height, bool solid = false, bool interactWithSolid = false, float movementIncrement = 0)
			: base(game, x, y, width, height, solid)
		{
			this.InteractWithSolid = interactWithSolid;

			this.MovementIncrement = movementIncrement;
			if (MovementIncrement == 0)
			{
				MovementIncrement = Math.Min(width, height);
			}


			Acceleration = new Vector2(0, 0);
			Speed = new Vector2(0, 0);
			UpdateOrder = Int32.MaxValue;

			SpeedUpdate = false;
		}

		public KineticBox(KineticBox box)
			: this(box.Game, box.X, box.Y, box.Width, box.Height, box.Solid, box.InteractWithSolid, box.MovementIncrement)
		{
		}

		public void UpdateSpeed(GameTime gameTime)
		{
			Speed += Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
		}

		public void UpdateLocation(GameTime gameTime)
		{
			if (Speed != Vector2.Zero) //Don't trigger PropertyChanged event needlessly
			{
				int steps = 1;
				GameTime relativeGameTime = new GameTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime);

				if (MovementIncrement > 0)
				{
					float SpeedLength = (Speed * (float)relativeGameTime.ElapsedGameTime.TotalSeconds).Length();
					if (SpeedLength > MovementIncrement)
					{
						steps = (int)Math.Ceiling(SpeedLength / MovementIncrement);
						relativeGameTime.ElapsedGameTime = new TimeSpan(gameTime.ElapsedGameTime.Ticks / steps);
					}
				}

				for (int i = 0; i < steps; i++)
				{
					SpeedUpdate = true;
					Location += Speed * (float)relativeGameTime.ElapsedGameTime.TotalSeconds;
					SpeedUpdate = false;
				}
			}
		}
	}
}