using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameClassLibrary
{
	public class Camera : GameComponent
	{
		protected Viewport viewport;
		public Matrix TransformMatrix { get; protected set; }
		public Point center;
		public Point Center
		{
			get
			{
				return center;

			}
			set
			{
				center = value;
				Update();
			}
		}
		public float zoom;
		public float Zoom
		{
			get
			{
				return zoom;

			}
			set
			{
				zoom = value;
				Update();
			}
		}

		public Camera(Game game)
			: base(game)
		{
			this.viewport = Game.GraphicsDevice.Viewport;

			Point Center = new Point(0, 0);
			Zoom = 1;
		}

		protected void Update()
		{
			TransformMatrix = Matrix.CreateTranslation(-Center.X, -Center.Y, 0);
			TransformMatrix *= Matrix.CreateScale(Zoom);
			TransformMatrix *= Matrix.CreateTranslation(viewport.Width / 2, viewport.Height / 2, 0);
		}

		public Vector2 ToWorld(Vector2 vector2)
		{
			return Vector2.Transform(vector2, Matrix.Invert(TransformMatrix));
		}
	}
}
