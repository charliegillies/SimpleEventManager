using System.Collections.Generic;

namespace SimpleEvents
{
    /// <summary>
    /// Non generic implementation of ListenerGroup.
    /// </summary>
    public interface IEventGroup
    {
        void Invoke(BaseEvent @event);
    }

    /// <summary>
    /// Defines a group of events that are invoked by a certain type.
    /// </summary>
    public sealed class ListenerGroup<T> : IEventGroup where T : BaseEvent
    {
        // A list of all the event listeners that this group uses.
        private List<EventListener<T>> _listeners;

        public ListenerGroup()
        {
            _listeners = new List<EventListener<T>>();
        }

        /// <summary>
        /// Invoke the event using generics.
        /// </summary>
        public void Invoke(T eventParam)
        {
            for(int i = 0; i < _listeners.Count; i++)
                _listeners[i].Invoke(eventParam);
        }

        /// <summary>
        /// Invoke the event without generics.
        /// </summary>
        public void Invoke(BaseEvent @event)
        {
            // Attempt to convert the event to our own.
            var e = @event as T;
            if (e == null) return;

            // Call the generic invoke method.
            Invoke(e);
        }

        /// <summary>
        /// Adds an event listener to the internal group.
        /// </summary>
        public void Subscribe(EventListener<T> listener)
        {
            _listeners.Add(listener);
        }

        /// <summary>
        /// Removes an event listener from the internal group.
        /// </summary>
        public void Unsubscribe(EventListener<T> listener)
        {
            _listeners.Remove(listener);
        }

    }
}