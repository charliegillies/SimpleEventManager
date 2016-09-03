using System;

namespace SimpleEvents
{
    /// <summary>
    /// Defines a listener that listens for a certain event.
    /// </summary>
    public sealed class EventListener<T> where T : BaseEvent
    {
        private Action<T> _listener;

        public EventListener(Action<T> listener)
        {
            _listener = listener;
        }

        /// <summary>
        /// Invokes the listener action.
        /// </summary>
        public void Invoke(T param)
        {
            _listener(param);
        }
    }
}