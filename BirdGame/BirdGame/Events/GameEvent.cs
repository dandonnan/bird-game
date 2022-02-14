namespace BirdGame.Events
{
    using System;

    internal class GameEvent
    {
        public string Event { get; set; }

        public DateTime TimeFired { get; set; }
    }
}
