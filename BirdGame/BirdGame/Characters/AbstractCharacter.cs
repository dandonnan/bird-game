namespace BirdGame.Characters
{
    using Microsoft.Xna.Framework;

    internal abstract class AbstractCharacter
    {
        public abstract void Update(GameTime gameTime);

        public abstract void Draw();
    }
}
