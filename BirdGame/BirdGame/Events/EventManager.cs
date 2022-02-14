namespace BirdGame.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class EventManager
    {
        private static EventManager eventManager;

        private readonly List<GameEvent> events;

        private EventManager()
        {
            events = new List<GameEvent>();

            eventManager = this;
        }

        public static EventManager Initialise()
        {
            EventManager manager = null;

            if (eventManager == null)
            {
                manager = new EventManager();
            }

            return manager;
        }

        public static void FireEvent(string eventName)
        {
            eventManager.events.Add(new GameEvent
            {
                Event = eventName,
                TimeFired = DateTime.Now
            });
        }

        public static bool EventFired(string eventName)
        {
            return eventManager.events.Any(e => e.Event == eventName);
        }

        public void Update()
        {
            List<GameEvent> expired = new List<GameEvent>();

            DateTime currentTime = DateTime.Now;

            foreach (GameEvent gameEvent in events)
            {
                if (gameEvent.TimeFired < currentTime.AddSeconds(-1))
                {
                    expired.Add(gameEvent);
                }
            }

            foreach (GameEvent gameEvent in expired)
            {
                events.Remove(gameEvent);
            }

            expired.Clear();
        }
    }
}
