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

        public EnemyShip(Texture2D texture, Vector2 position) : base(texture, position, 1)
        {
            shootCooldown = (float)(new Random().NextDouble() * 3 + 2); // 2-5 seconds random cooldown
        }

        public override void Update(GameTime gameTime, List<Projectile> projectiles, Texture2D projectileTexture)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            shootCooldown -= elapsed;
            if (shootCooldown <= 0f)
            {
                projectiles.Add(new Projectile(projectileTexture, new Vector2(Position.X + Texture.Width / 2 - projectileTexture.Width / 2, Position.Y + Texture.Height), new Vector2(0, 1), false));
                shootCooldown = (float)(new Random().NextDouble() * 3 + 2);
            }
        }
    }
}