using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClassLibrary.Physics;

namespace MonoGameClassLibrary
{
	public class MainGame : Game
	{
		public GraphicsDeviceManager Graphics { get; protected set; }
		public SpriteBatch SpriteBatch { get; protected set; }
		public EntityManager EntityManager { get; protected set; }
		public PhysicsEngine PhysicsEngine { get; protected set; }
		public Camera Camera { get; protected set; }

		public MainGame()
		{
			Content.RootDirectory = "Content";
			Graphics = new GraphicsDeviceManager(this);
		}

		protected override void LoadContent()
		{
			new DrawHelper(GraphicsDevice);
			SpriteBatch = new SpriteBatch(GraphicsDevice);
			EntityManager = new EntityManager(this);
			PhysicsEngine = new PhysicsEngine();//La taille ne devrait pas être fixe
			Camera = new Camera(GraphicsDevice.Viewport);
		}

		protected override void Update(GameTime gameTime)
		{
			EntityManagerWillUpdate();
			EntityManager.Update(gameTime);
			EntityManagerWillUpdate();

			PhysicsEngineWillUpdate();
			PhysicsEngine.Update(gameTime);
			PhysicsEngineHasUpdate();

			CameraWillUpdate();
			Camera.Update(gameTime);
			CameraHasUpdate();
		}

		protected virtual void EntityManagerWillUpdate() { }
		protected virtual void EntityManagerHasUpdate() { }
		protected virtual void PhysicsEngineWillUpdate() { }
		protected virtual void PhysicsEngineHasUpdate() { }
		protected virtual void CameraWillUpdate() { }
		protected virtual void CameraHasUpdate() { }

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Transparent);
			SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.Transform);
			EntityManager.Draw(gameTime);
#if DEBUG
			PhysicsEngine.Draw(SpriteBatch, gameTime);
#endif
			SpriteBatch.End();
		}
	}
}
