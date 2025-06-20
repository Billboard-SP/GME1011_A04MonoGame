using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;



namespace GME1011_A04MonoGame
{
    public abstract class Ship
    {
        public Texture2D Texture;
        public Vector2 Position;
        public int Health;
        public Rectangle Bounds => new((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

        public Ship(Texture2D texture, Vector2 position, int health)
        {
            Texture = texture;
            Position = position;
            Health = health;
        }

        public abstract void Update(GameTime gameTime, List<Projectile> projectiles, Texture2D projectileTexture);
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
