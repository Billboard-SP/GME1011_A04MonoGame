using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GME1011_A04MonoGame
{
    public class Projectile
    {
        public Texture2D Texture;
        public Vector2 Position;
        public Vector2 Direction;
        public float Speed = 400f;
        public bool IsFromPlayer;
        public int Damage = 1;
        public Rectangle Bounds => new((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

        public Projectile(Texture2D texture, Vector2 position, Vector2 direction, bool isFromPlayer)
        {
            Texture = texture;
            Position = position;
            Direction = direction;
            IsFromPlayer = isFromPlayer;
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += Direction * Speed * elapsed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}