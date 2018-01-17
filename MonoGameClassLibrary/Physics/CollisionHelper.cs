using Microsoft.Xna.Framework;
using MonoGameClassLibrary.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameClassLibrary.Physics
{

	//TODO RELIRE
	public static class CollisionHelper
	{
		//TODO: gestion manuelle de retirer box de spatial grid avant de jouer ici...
		public static bool PhysicalCollisions(GameTime gameTime, Box box, SpatialGrid spatialGrid)
		{
			//S'il y a vraiment collision
			if (box.Intersect(spatialGrid.GetProbableSolidCollisions(box)))
			{
				Vector2 oldSpeed = box.Speed;
				Rectangle OldRectangle = new Rectangle();
				//Tant que la collision n'est pas résolue
				while (box.Rectangle != OldRectangle)
				{
					OldRectangle = box.Rectangle;
					if (box.Intersect(spatialGrid.GetProbableSolidCollisions(box)))
					{
						//Défait le mouvement qui créer la collion
						box.Speed = Vector2.Negate(box.Speed);
						box.UpdateLocation(gameTime);
						box.Speed = Vector2.Negate(box.Speed);
					}
					else
					{
						//Avance plus lentement vers la collion
						box.Speed /= 2;
						box.UpdateLocation(gameTime);
					}
				}

				box.Speed = oldSpeed;
				StopSpeed(box, spatialGrid);
				box.UpdateLocation(gameTime);
				box.Speed = oldSpeed;

				//New collision possible
				if (oldSpeed != Vector2.Zero)
				{
					//Resolve new movement
					PhysicalCollisions(gameTime, box, spatialGrid);
				}

				return true;
			}

			return false;
		}

		public static void StopSpeed(Box box, SpatialGrid spatialGrid)
		{
			//Restore l'ancienne vitesse en vérifiant quels côtés ont fait la collision
			IEnumerable<AABB> solids = spatialGrid.GetProbableSolidCollisions(box);
			if (((box.Speed.X < 0) && (box.LeftCollision(solids))) ||
				((box.Speed.X > 0) && ((box.RightCollision(solids)))))
			{
				box.Speed.X = 0;
			}
			if (((box.Speed.Y < 0) && (box.TopCollision(solids))) ||
				((box.Speed.Y > 0) && (box.BottomCollision(solids))))
			{
				box.Speed.Y = 0;
			}
		}
	}
}
