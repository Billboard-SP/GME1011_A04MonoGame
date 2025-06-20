using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GME1011_A04MonoGame
{
    public enum PowerUpType
    {
        ExtraLife,
        SpeedBoost
    }

    public class PowerUp
    {
        public Texture2D Texture;
        public Vector2 Position;
        public PowerUpType Type;
        public float Speed = 15f;
        public Rectangle Bounds => new((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

        public PowerUp(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
            // Randomly assign powerup type
            Type = (PowerUpType)(new System.Random().Next(0, 2));
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += new Vector2(0, Speed * elapsed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}