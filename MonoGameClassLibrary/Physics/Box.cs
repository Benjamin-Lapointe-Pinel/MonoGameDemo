﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameClassLibrary.Physics
{
	public class Box : AABB
	{
		public bool InteractWithSolid { get; set; }
		public float MovementIncrement { get; set; }

		public Vector2 Acceleration;
		public Vector2 Speed;

		public Box(Game game, float x, float y, float width, float height, bool solid = false, bool interactWithSolid = false, float movementIncrement = 0)
			: base(game, x, y, width, height, solid)
		{
			this.InteractWithSolid = interactWithSolid;
			this.MovementIncrement = movementIncrement;

			Acceleration = new Vector2(0, 0);
			Speed = new Vector2(0, 0);
			UpdateOrder = Int32.MaxValue;
		}

		public Box(Box box)
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
					Location += Speed * (float)relativeGameTime.ElapsedGameTime.TotalSeconds;
					//Speed is 0 anyway after collision
					//if (Speed == Vector2.Zero)
					//{
					//	break;
					//}
				}
			}
		}
	}
}