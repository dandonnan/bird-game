namespace BirdGame.Events
{
    using BirdGame.World;
    using Microsoft.Xna.Framework;
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
            EventManager manager = eventManager;

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

        public static void FireEvent(string eventName, object eventData)
        {
            eventManager.events.Add(new GameEvent
            {
                Event = eventName,
                TimeFired = DateTime.Now,
                EventData = eventData
            });
        }

        public static bool EventFired(string eventName)
        {
            return eventManager.events.Any(e => e.Event == eventName);
        }

        public static bool EventFiredThenKill(string eventName)
        {
            bool eventFired = false;

            List<GameEvent> events = eventManager.events.Where(e => e.Event == eventName)
                                                        .ToList();

            if (events.Any())
            {
                eventFired = true;

                foreach (GameEvent gameEvent in events)
                {
                    eventManager.events.Remove(gameEvent);
                }
            }

            return eventFired;
        }

        public static bool IsEventFiredInBounds(string eventName, Rectangle bounds, out Poop poop)
        {
            bool inBounds = false;

            poop = null;

            GameEvent gameEvent = eventManager.events.FirstOrDefault(e => e.Event == eventName);

            if (gameEvent != null && gameEvent.EventData != null)
            {
                try
                {
                    Vector2 position;

                    if (gameEvent.EventData.GetType() == typeof(Poop))
                    {
                        poop = (Poop)gameEvent.EventData;
                        position = poop.Position;
                    }
                    else
                    {
                        position = (Vector2)gameEvent.EventData;
                    }

                    if (position.X >= bounds.X && position.X <= bounds.X + bounds.Width
                        && position.Y >= bounds.Y && position.Y <= bounds.Y + bounds.Height)
                    {
                        inBounds = true;
                        eventManager.events.Remove(gameEvent);
                    }
                }
                catch
                {
                    // Don't do anything
                }
            }

            return inBounds;
        }

        public static object GetEventObject(string eventName)
        {
            object eventObject = null;

            GameEvent gameEvent = eventManager.events.FirstOrDefault(e => e.Event == eventName);

            if (gameEvent != null)
            {
                eventObject = gameEvent.EventData;
                eventManager.events.Remove(gameEvent);
            }

            return eventObject;
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
