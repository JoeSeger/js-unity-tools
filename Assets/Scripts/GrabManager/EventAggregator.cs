using System;
using System.Collections.Generic;
using UnityEngine;

namespace GrabManager
{
    [CreateAssetMenu(fileName = "EventAggregator",menuName = "EventAggregator")]
    public class EventAggregator : ScriptableObject
    {
        private readonly Dictionary<Type, List<Delegate>> _eventSubscribers = new();

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