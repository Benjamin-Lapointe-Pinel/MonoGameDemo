﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClassLibrary;
using MonoGameClassLibrary.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDemo
{
	public class PassthroughPlatform : Box
	{
		public Color Color { get; protected set; }

		public PassthroughPlatform(Game game, float x, float y, float width)
			: base(game, x, y, width, 20, true)
		{
		}

		public override bool Intersects(Box value)
		{
			if (base.Intersects(value))
			{
				if (value is KineticBox)
				{
					KineticBox box = value as KineticBox;
					if (box.Speed.Y < 0)
					{
						return false;
					}
					else if (box.Bottom > Bottom)
					{
						return false;
					}
				}

				return true;
			}

			return false;
		}
	}
}
