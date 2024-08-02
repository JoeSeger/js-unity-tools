using System;
using System.Collections.Generic;
using UnityEngine;

namespace GrabManager
{
    /// <summary>
    /// EventAggregator is a ScriptableObject used for managing event subscriptions and publishing events.
    /// It allows different parts of the application to communicate through events in a decoupled manner.
    /// </summary>
    [CreateAssetMenu(fileName = "EventAggregator", menuName = "EventAggregator")]
    public class EventAggregator : ScriptableObject
    {
        private readonly Dictionary<Type, List<Delegate>> _eventSubscribers = new();

        /// <summary>
        /// Subscribes one or more actions to an event of type TEvent.
        /// </summary>
        /// <typeparam name="TEvent">The type of event to subscribe to.</typeparam>
        /// <param name="subscribing">The actions to subscribe to the event.</param>
        public void Subscribe<TEvent>(params Action<TEvent>[] subscribing)
        {
            foreach (var action in subscribing)
            {
                if (!_eventSubscribers.TryGetValue(typeof(TEvent), out var subscribers))
                {
                    subscribers = new List<Delegate>();
                    _eventSubscribers[typeof(TEvent)] = subscribers;
                }

                subscribers.Add(action);
            }
        }

        /// <summary>
        /// Unsubscribes one or more actions from an event of type TEvent.
        /// </summary>
        /// <typeparam name="TEvent">The type of event to unsubscribe from.</typeparam>
        /// <param name="subscriber">The actions to unsubscribe from the event.</param>
        public void Unsubscribe<TEvent>(params Action<TEvent>[] subscriber)
        {
            foreach (var action in subscriber)
            {
                if (_eventSubscribers.TryGetValue(typeof(TEvent), out var subscribers))
                {
                    subscribers.Remove(action);
                }
            }
        }

        /// <summary>
        /// Publishes an event of type TEvent, invoking all subscribed actions.
        /// </summary>
        /// <typeparam name="TEvent">The type of event to publish.</typeparam>
        /// <param name="eventToPublish">The event instance to publish.</param>
        public void Publish<TEvent>(TEvent eventToPublish)
        {
            if (!_eventSubscribers.TryGetValue(typeof(TEvent), out var subscribers)) return;
            foreach (var subscriber in subscribers)
            {
                ((Action<TEvent>)subscriber)(eventToPublish);
            }
        }
    }
}
