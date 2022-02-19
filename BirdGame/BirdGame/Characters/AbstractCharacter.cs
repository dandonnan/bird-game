namespace BirdGame.Characters
{
    using BirdGame.World;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// An abstract character that contains common properties used
    /// by all characters.
    /// </summary>
    internal abstract class AbstractCharacter
    {
        /// <summary>
        /// A publically accessible position of the character.
        /// </summary>
        public Vector2 Position => position;

        /// <summary>
        /// The publically accessible rotation of the character.
        /// </summary>
        public float Rotation => rotation;

        /// <summary>
        /// The publically accessible origin (the point they rotate around) of the character.
        /// </summary>
        public Vector2 Origin => origin;

        /// <summary>
        /// How long the character has been active.
        /// </summary>
        public float Lifetime { get;  protected set; }

        /// <summary>
        /// How long the character must be active before they can despawn.
        /// </summary>
        public float MinLifetime { get; protected set; }

        /// <summary>
        /// The position of the character.
        /// </summary>
        protected Vector2 position;

        /// <summary>
        /// The rotation of the character.
        /// </summary>
        protected float rotation;

        /// <summary>
        /// The origin (the point they rotate around) of the character.
        /// </summary>
        protected Vector2 origin;

        /// <summary>
        /// The spawn point the character spawns from.
        /// </summary>
        protected SpawnPoint spawnPoint;

        /// <summary>
        /// The width of the character. This method must be implemented
        /// in the classes that inherit from this.
        /// </summary>
        /// <returns>The width of the character.</returns>
        public abstract int GetWidth();

        /// <summary>
        /// The height of the character. This method must be implemented
        /// in the classes that inherit from this.
        /// </summary>
        /// <returns>The height of the character.</returns>
        public abstract int GetHeight();

        /// <summary>
        /// Kill / despawn the character. This method must be implemented
        /// in the classes that inherit from this.
        /// </summary>
        public abstract void Kill();

        /// <summary>
        /// Update the character. This method must be implemented in the
        /// classes that inherit from this.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draw the character. This method must be implemented in the
        /// classes that inherit from this.
        /// </summary>
        public abstract void Draw();
    }
}
