using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameClassLibrary
{
	public class Camera : EntityManager.Updatable
	{
		public Matrix Transform { get; protected set; }
		public Point Center;
		public float Zoom;
		protected Viewport viewport;

		public Camera(Viewport viewport)
		{
			this.viewport = viewport;
			
			Point Center = new Point(0, 0);
			Zoom = 1;
		}

		public override void EntityUpdate(GameTime gameTime)
		{
			Transform = Matrix.CreateTranslation(-Center.X, -Center.Y, 0);
			Transform *= Matrix.CreateScale(Zoom);
			Transform *= Matrix.CreateTranslation(viewport.Width / 2, viewport.Height / 2, 0);
		}
	}
}
