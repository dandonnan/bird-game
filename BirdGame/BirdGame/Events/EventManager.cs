namespace BirdGame.Events
{
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An event manager.
    /// </summary>
    internal class EventManager
    {
        /// <summary>
        /// The singleton for the event manager, so there can only be one.
        /// </summary>
        private static EventManager eventManager;

        /// <summary>
        /// A list of events that have been triggered.
        /// </summary>
        private readonly List<GameEvent> events;

        /// <summary>
        /// The private constructor so it can only be created through
        /// the Initialise method.
        /// </summary>
        private EventManager()
        {
            events = new List<GameEvent>();

            eventManager = this;
        }

        /// <summary>
        /// Create an event manager.
        /// </summary>
        /// <returns>The event manager.</returns>
        public static EventManager Initialise()
        {
            EventManager manager = eventManager;

            if (eventManager == null)
            {
                manager = new EventManager();
            }

            return manager;
        }

        /// <summary>
        /// Fire a new game event.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        public static void FireEvent(string eventName)
        {
            // Add the event to the list
            eventManager.events.Add(new GameEvent
            {
                Event = eventName,
                TimeFired = DateTime.Now
            });
        }

        /// <summary>
        /// Fire a new game event with game data.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="eventData">The data to fire with the event.</param>
        public static void FireEvent(string eventName, object eventData)
        {
            // Add the event to the list
            eventManager.events.Add(new GameEvent
            {
                Event = eventName,
                TimeFired = DateTime.Now,
                EventData = eventData
            });
        }

        /// <summary>
        /// Get whether an event has been fired.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <returns>true if the event has been fired, false if not.</returns>
        public static bool EventFired(string eventName)
        {
            // Go through the list of events and check to see if an event with
            // the given name exists
            return eventManager.events.Any(e => e.Event == eventName);
        }

        /// <summary>
        /// Get whether an event has been fired, and kill it so
        /// nothing else can react to it.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <returns>true if the event has been fired, false if not.</returns>
        public static bool EventFiredThenKill(string eventName)
        {
            bool eventFired = false;

            // Get a list of events that match the given event id / name
            List<GameEvent> events = eventManager.events.Where(e => e.Event == eventName)
                                                        .ToList();

            // If there was a matching event
            if (events.Any())
            {
                // The event was fired
                eventFired = true;

                // Remove the matching events from the main event list
                // so nothing else can react to them
                foreach (GameEvent gameEvent in events)
                {
                    eventManager.events.Remove(gameEvent);
                }
            }

            return eventFired;
        }

        /// <summary>
        /// Get whether an event was fired within the given bounds.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="bounds">The bounds to check.</param>
        /// <param name="poop">A Poop object to return, if the poop dropped event was triggered.</param>
        /// <returns>true if the event was fired in the bounds, false if not.</returns>
        public static bool IsEventFiredInBounds(string eventName, Rectangle bounds, out Poop poop)
        {
            bool inBounds = false;

            poop = null;

            // Check the list of events to find the event with the given name
            GameEvent gameEvent = eventManager.events.FirstOrDefault(e => e.Event == eventName);

            // If there was an event and it had event data
            if (gameEvent != null && gameEvent.EventData != null)
            {
                try
                {
                    Vector2 position;

                    // If the data was a poop
                    if (gameEvent.EventData.GetType() == typeof(Poop))
                    {
                        // Convert the object into poop
                        poop = (Poop)gameEvent.EventData;

                        // Set the position based on the poop
                        position = poop.Position;
                    }
                    else
                    {
                        // Set the position from the data
                        position = (Vector2)gameEvent.EventData;
                    }

                    // If the position was inside the bounds
                    if (position.X >= bounds.X && position.X <= bounds.X + bounds.Width
                        && position.Y >= bounds.Y && position.Y <= bounds.Y + bounds.Height)
                    {
                        inBounds = true;

                        // Remove the event
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

        /// <summary>
        /// Get an object from an event that has been fired.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <returns>The object from the event, or null if there was no event.</returns>
        public static object GetEventObject(string eventName)
        {
            object eventObject = null;

            // Get an event from the event list that has the event id / name
            GameEvent gameEvent = eventManager.events.FirstOrDefault(e => e.Event == eventName);

            // If there was an event
            if (gameEvent != null)
            {
                // Get the object from the event
                eventObject = gameEvent.EventData;

                // Remove the event from the main list
                eventManager.events.Remove(gameEvent);
            }

            return eventObject;
        }

        /// <summary>
        /// Update the event manager.
        /// </summary>
        public void Update()
        {
            // A list of expired events
            List<GameEvent> expired = new List<GameEvent>();

            // The current time
            DateTime currentTime = DateTime.Now;

            // Go through each event
            foreach (GameEvent gameEvent in events)
            {
                // If the event was fired more than 1 second ago
                if (gameEvent.TimeFired < currentTime.AddSeconds(-1))
                {
                    // Add it to the list of expired events
                    expired.Add(gameEvent);
                }
            }

            // Go through the expired list and remove the event from the main list
            foreach (GameEvent gameEvent in expired)
            {
                events.Remove(gameEvent);
            }

            expired.Clear();
        }
    }
}
