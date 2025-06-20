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

        bool isGameOver = false;

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
                            isGameOver = true;
                        }
                    }
                }
            }

            // and then they started blasting
            foreach (var enemy in enemies)
            {
                enemy.Update(gameTime, projectiles, projectileTexture);
            }

            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Update(gameTime);

                if (projectiles[i].isFromPlayer)
                {
                    for (int j = enemies.Count - 1; j >= 0; j--)
                    {
                        if (projectiles[i].Bounds.Intersects(enemies[j].Bounds))
                        {
                            enemies[j].Health -= projectiles[i].Damage;
                            projectiles.RemoveAt(j);
                            if (enemies[j].health <= 0)
                            {
                                enemies.RemoveAt(j);
                                score += 10;

                                // power up upon commiting a crime
                                if (RandomFloat() < 0.1f)
                                    powerUps.Add(new PowerUp(powerUpTexture, enemies[j].Position));
                            }
                            break;
                        }
                    }
                }
                else
                {
                    if (projectiles[i].Bounds.Intersects(player.Bounds))
                    {
                        lives--;
                        projectiles.RemoveAt(i);
                        if (lives <= 0)
                        {
                            isGameOver = true;
                        }
                    }

                    if (i < projectiles.Count && (projectiles[i].Position.Y < -projectiles[i].Texture.Height || projectiles[i].Position.Y > _graphics.PreferredBackBufferHeight))
                        projectiles.RemoveAt(i);
                }

                // power up logic
                for (int k = powerUps.Count - 1; k >= 0; k--)
                {
                    powerUps[k].Update(gameTime);
                    if (powerUps[k].Bounds.Intersects(player.Bounds))
                    {
                        if (powerUps[k].Type == PowerUpType.ExtraLife)
                            lives++;
                        else if (powerUps[k].Type == PowerUpType.SpeedBoost)
                        {
                            player.Speed *= 2f;
                            player.SpeedBoostTime = 5f;
                        }
                        powerUps.RemoveAt(k);
                    }
                    else if (powerUps[k].Position.Y > _graphics.PreferredBackBufferHeight)
                    {
                        powerUps.RemoveAt(k);
                    }
                }

                if (player.SpeedBoostTime > 0)
                {
                    player.SpeedBoostTime -= elapsed;
                    if (player.SpeedBoostTime <= 0)
                        player.Speed /= 2f;
                }

                if (enemies.Count == 0)
                {
                    wave++;
                    SpawnWave(wave);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            player.Draw(_spriteBatch);
            foreach (var enemy in enemies)
                enemy.Draw(_spriteBatch);
            foreach (var proj in projectiles)
                proj.Draw(_spriteBatch);
            foreach (var pu in powerUps)
                pu.Draw(_spriteBatch);

            _spriteBatch.DrawString(font, $"Score: {score}", new Vector2(10, 10), Color.White);
            _spriteBatch.DrawString(font, $"Lives: {lives}", new Vector2(10, 30), Color.White);
            _spriteBatch.DrawString(font, $"Wave: {wave}", new Vector2(10, 50), Color.White);

            if (isGameOver)
            {
                string message = "GAME OVER\nPress Enter to Restart";
                Vector2 size = font.MeasureString(message);
                Vector2 pos = new Vector2(400 -  size.X / 2, 300 - size.Y / 2);
                _spriteBatch.DrawString(font, message, pos, Color.Red);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
