using System;

namespace Game
{
    public class Publisher<T>
    {
        public event Action<T> OnUpdate;

        public T currentValue;
        public DateTime lastUpdated = DateTime.Now;

        public Publisher(T initialValue)
        {
            currentValue = initialValue;
            lastUpdated = DateTime.Now;
        }

        public void Update(T newValue)
        {
            currentValue = newValue;
            // Invoke the event, notifying all subscribers
            OnUpdate?.Invoke(newValue);
        }

        public Subscriber<T> CreateSubscriber()
        {
            Subscriber<T> sub = new(this);
            return sub;
        }
    }

    public class Subscriber<T>
    {
        private readonly Publisher<T> _publisher;

        public Subscriber(Publisher<T> publisher)
        {
            _publisher = publisher;
        }


        public void Subscribe(Action<T> action, bool fireForCurrentValue = false)
        {
            _publisher.OnUpdate += action;

            if (fireForCurrentValue)
            {
                action.Invoke(_publisher.currentValue);
            }
        }

        public void Unsubscribe(Action<T> action)
        {
            _publisher.OnUpdate -= action;
        }
    }
}
