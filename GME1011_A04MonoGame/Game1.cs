using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GME1011_A04MonoGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        PlayerShip player;
        List<EnemyShip> enemies = new();
        List<Projectile> projectiles = new();
        List<PowerUp> powerUps = new();

        Texture2D playerTexture, enemyTexture, projectileTexture, powerUpTexture;
        SpriteFont font;

        int score = 0;
        int lives = 3;

        float enemyMoveTimer = 0f;
        float enemyMoveInterval = 0.5f;
        int enemyMoveDirection = 1;

        int wave = 1;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            playerTexture = Content.Load<Texture2D>("player");
            enemyTexture = Content.Load<Texture2D>("enemy");
            projectileTexture = Content.Load<Texture2D>("projectile");
            powerUpTexture = Content.Load<Texture2D>("powerup");
            font = Content.Load<SpriteFont>("Arial");

            player = new PlayerShip(playerTexture, new Vector2(400, 500));

            SpawnWave(wave);
        }

        void SpawnWave(int waveNumber)
        {
            enemies.Clear();

            int rows = 3 + waveNumber;
            int cols = 7;
            float startX = 100;
            float startY = 50;
            float spacingX = 60;
            float spacingY = 50;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Vector2 pos = new Vector2(startX + col * spacingX, startY + row * spacingY);
                    EnemyShip enemy = new EnemyShip(enemyTexture, pos);
                    enemies.Add(enemy);
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            player.Update(gameTime, projectiles, projectileTexture);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // enemy move timer
            enemyMoveTimer += elapsed;
            if (enemyMoveTimer > enemyMoveInterval)
            {
                enemyMoveTimer = 0;
                bool changeDirection = false;

                foreach (var enemy in enemies)
                {
                    enemy.Position += new Vector2(10 * enemyMoveDirection, 0);

                    if (enemy.Position.X > _graphics.PreferredBackBufferWidth - enemy.Texture.Width || enemy.Position.X < 0)
                    {
                        changeDirection = true;
                    }
                }   
                
                if (changeDirection)
                {
                    enemyMoveDirection *= -1;
                    foreach (var enemy in enemies)
                    {
                        enemy.Position += new Vector2(0, 20);

                        if (enemy.Position.Y + enemy.Texture.Height >= player.Position)
                        {
                            // lose sceneario
                            lives = 0;
                        }
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
