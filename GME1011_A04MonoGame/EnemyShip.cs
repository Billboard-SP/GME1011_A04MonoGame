using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GME1011_A04MonoGame
{
    public class EnemyShip : Ship
    {
        private float shootCooldown;
        private static readonly Random rng = new();

        public EnemyShip(Texture2D texture, Vector2 position) : base(texture, position, 1)
        {
            ResetCooldown();
        }

        private void ResetCooldown()
        {
            shootCooldown = (float)(rng.NextDouble() * 3 + 2); // 2–5 seconds
        }

        public override void Update(GameTime gameTime, List<Projectile> projectiles, Texture2D projectileTexture)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            shootCooldown -= elapsed;

            if (shootCooldown <= 0f)
            {
                if (rng.NextDouble() < 0.3) // 30% chance to shoot when cooldown expires
                {
                    Vector2 projPos = new(Position.X + Texture.Width / 2 - projectileTexture.Width / 2, Position.Y + Texture.Height);
                    projectiles.Add(new Projectile(projectileTexture, projPos, new Vector2(0, 3f), false));
                }

                ResetCooldown();
            }
        }
    }
}