using System;
using System.Collections.Generic;

namespace SimpleEvents
{
    /// <summary>
    /// This is responsible for handling the listeners and
    /// broadcasting the events as appropiate.
    /// </summary>
    public sealed class EventManager
    {
        private Dictionary<Type, IEventGroup> _listeners;
        private Queue<BaseEvent> _queuedEvents; 

        public EventManager()
        {
            _listeners = new Dictionary<Type, IEventGroup>();
            _queuedEvents = new Queue<BaseEvent>();
        }

        /// <summary>
        /// Invokes all queued events.
        /// This should be called every frame.
        /// </summary>
        public void InvokeQueuedEvents()
        {
            // Don't update if there are no queued events.
            if (_queuedEvents.Count == 0) return;

            while (_queuedEvents.Count > 0)
            {
                // Dequeue the event
                var e = _queuedEvents.Dequeue();
                // Invoke the event without the generic value
                Invoke(e);
            }
        }

        /// <summary>
        /// Invokes an event without using a generic value.
        /// </summary>
        /// <param name="e">The event to invoke.</param>
        private void Invoke(BaseEvent e)
        {
            var group = GetListenerGroup(e.GetType());
    
            if (group != null)
                group.Invoke(e);
        }

        /// <summary>
        /// Queues an event to be invoked on the next Update call.
        /// </summary>
        /// <typeparam name="T">Generic type of the event that should be invoked.</typeparam>
        /// <param name="param">The event to invoke.</param>
        public void QueueInvoke<T>(T param) where T : BaseEvent
        {
            _queuedEvents.Enqueue(param);
        }

        /// <summary>
        /// Immediately invokes the event.
        /// </summary>
        /// <typeparam name="T">Generic type of the event that shoudld be invoked.</typeparam>
        /// <param name="param">The event to invoke.</param>
        public void Invoke<T>(T param) where T : BaseEvent
        {
            var group = GetListenerGroup<T>();
            if (group != null)
                group.Invoke(param);
        }

        /// <summary>
        /// Creates an event listener that listens to a specific event.
        /// </summary>
        /// <typeparam name="T">The type of event to listen too.</typeparam>
        /// <param name="callback">The action that is carried out on the event being invoked.</param>
        /// <returns>The event listener that is created. Use this to Unsubscribe.</returns>
        public EventListener<T> Subscribe<T>(Action<T> callback) where T : BaseEvent
        {
            var listener = new EventListener<T>(callback);

            // Get the listener group, subscribe to it.
            var group = GetOrCreateListenerGroup<T>();
            group.Subscribe(listener);

            return listener;
        }

        /// <summary>
        /// Unsubscribes an event listener from the event manager.
        /// </summary>
        /// <typeparam name="T">The type of event that the event listener listens too.</typeparam>
        /// <param name="listener">The listener object that is returned by the subscribe method.</param>
        public void Unsubscribe<T>(EventListener<T> listener) where T : BaseEvent
        {
            // Get an existing listener group, if there is one..
            var group = GetListenerGroup<T>();
            if (group == null) return;

            // Unsubscribe from the group
            group.Unsubscribe(listener);
        }

        /// <summary>
        /// Gets a listener group using generics.
        /// </summary>
        private ListenerGroup<T> GetListenerGroup<T>() where T : BaseEvent
        {
            // Attempt to get the listener from the key->value pair
            IEventGroup group;
            if (_listeners.TryGetValue(typeof(T), out group))
                return group as ListenerGroup<T>;

            return null;
        }

        /// <summary>
        /// Gets a listener group through a type parameter.
        /// </summary>
        private IEventGroup GetListenerGroup(Type type)
        {
            // Attempt to get the listener from the key->value pair
            IEventGroup group;
            if (_listeners.TryGetValue(type, out group))
                return group;

            return null;
        }

        /// <summary>
        /// Gets or creates a specific listener group.
        /// </summary>
        private ListenerGroup<T> GetOrCreateListenerGroup<T>() where T : BaseEvent
        {
            // Try and get an existing group
            var group = GetListenerGroup<T>();
            if (group != null)
                return group;

            // If we couldn't find an existing listener, 
            // create one & add it to the listeners
            var newGroup = new ListenerGroup<T>();
            _listeners.Add(typeof(T), newGroup);
            return newGroup;
        }
    }
}