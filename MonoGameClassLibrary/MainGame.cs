using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClassLibrary.Physics;

namespace MonoGameClassLibrary
{
	public class MainGame : Game
	{
		//Retirer les évenements et les transformer en appel de Scene
		public delegate void UpdateHandler(MainGame sender, GameTime gameTime);
		public event UpdateHandler WillUpdate;
		public event UpdateHandler HasUpdated;
		public event UpdateHandler EntityManagerWillUpdate;
		public event UpdateHandler EntityManagerHasUpdated;
		public event UpdateHandler PhysicsEngineWillUpdate;
		public event UpdateHandler PhysicsEngineHasUpdated;
		public event UpdateHandler CameraWillUpdate;
		public event UpdateHandler CameraHasUpdated;

		public delegate void DrawHandler(MainGame sender, GameTime gameTime);
		public event DrawHandler WillDraw;
		public event DrawHandler HasDrawn;

		public GraphicsDeviceManager Graphics { get; protected set; }
		public SpriteBatch SpriteBatch { get; protected set; }
		public EntityManager EntityManager { get; protected set; }
		public PhysicsEngine PhysicsEngine { get; protected set; }
		public Camera Camera { get; protected set; }

		public MainGame()
		{
			IsFixedTimeStep = false;
			Content.RootDirectory = "Content";
			Graphics = new GraphicsDeviceManager(this);
		}

		protected override void LoadContent()
		{
			DrawHelper.Init(GraphicsDevice, Content);

			SpriteBatch = new SpriteBatch(GraphicsDevice);
			EntityManager = new EntityManager(this);
			PhysicsEngine = new PhysicsEngine(2000, 2000);//La taille ne devrait pas être fixe
			Camera = new Camera(GraphicsDevice.Viewport);
		}

		protected override void Update(GameTime gameTime)
		{
			WillUpdate?.Invoke(this, gameTime);

			EntityManagerWillUpdate?.Invoke(this, gameTime);
			EntityManager.Update(gameTime);
			EntityManagerHasUpdated?.Invoke(this, gameTime);

			PhysicsEngineWillUpdate?.Invoke(this, gameTime);
			PhysicsEngine.EntityUpdate(gameTime);
			PhysicsEngineHasUpdated?.Invoke(this, gameTime);

			CameraWillUpdate?.Invoke(this, gameTime);
			Camera.EntityUpdate(gameTime);
			CameraHasUpdated?.Invoke(this, gameTime);

			HasUpdated?.Invoke(this, gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			WillDraw?.Invoke(this, gameTime);

			GraphicsDevice.Clear(Color.Transparent);
			SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.Transform);
			EntityManager.Draw(gameTime);
#if DEBUG
			PhysicsEngine.Draw(SpriteBatch, gameTime);
#endif
			SpriteBatch.End();

			HasDrawn?.Invoke(this, gameTime);
		}
	}
}
