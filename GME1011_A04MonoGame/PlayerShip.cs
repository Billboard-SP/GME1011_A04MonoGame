using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GME1011_A04MonoGame
{
    internal class PlayerShip : Ship
    {
        public float Speed = 200f;
        private float shootCooldown = 0f;
        public float SpeedBoostTime = 0f;

        public PlayerShip(Texture2D texture, Vector2 position) : base(texture, position, 3)
        {
        }

        public override void Update(GameTime gameTime, List<Projectile> projectiles, Texture2D projectileTexture)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var keyboardState = Keyboard.GetState();

            // the cha cha slide code
            if (keyboardState.IsKeyDown(Keys.Left))
                Position.X -= Speed * elapsed;
            if (keyboardState.IsKeyDown(Keys.Right))
                Position.X += Speed * elapsed;

            Position.X = MathHelper.Clamp(Position.X, 0, 800 - Texture.Width);

            // Gave it a gun
            shootCooldown -= elapsed;
            if (keyboardState.IsKeyDown(Keys.Space) && shootCooldown <= 0f)
            {
                projectiles.Add(new Projectile(projectileTexture, new Vector2(Position.X + Texture.Width / 2 - projectileTexture.Width / 2, Position.Y), new Vector2(0, -1), true));
                shootCooldown = 0.5f;
            }
        }
    }
}