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

		public Vector2 Acceleration;
		public Vector2 Speed;

		public Box(Game game, Rectangle rectangle, bool solid = false, bool interactWithSolid = false)
			: base(game, rectangle, solid)
		{
			this.InteractWithSolid = interactWithSolid;

			Acceleration = new Vector2(0, 0);
			Speed = new Vector2(0, 0);
			UpdateOrder = Int32.MaxValue;
		}

		public Box(Box box)
			: this(box.Game, box.Rectangle, box.Solid, box.InteractWithSolid)
		{
		}

		public void UpdateLocation(GameTime gameTime)
		{
			if (Speed != Vector2.Zero) //Don't trigger PropertyChanged event needlessly
			{
				Location += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
		}

		public void UpdateSpeed(GameTime gameTime)
		{
			Speed += Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
		}
	}
}