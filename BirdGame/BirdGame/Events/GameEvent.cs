namespace BirdGame.Events
{
    using System;

    /// <summary>
    /// A game event.
    /// </summary>
    internal class GameEvent
    {
        /// <summary>
        /// The id / name of the event.
        /// </summary>
        public string Event { get; set; }

        /// <summary>
        /// The time the event was fired.
        /// </summary>
        public DateTime TimeFired { get; set; }

        /// <summary>
        /// Data that can be fired with the event to pass to other objects.
        /// </summary>
        public object EventData { get; set; }
    }
}
