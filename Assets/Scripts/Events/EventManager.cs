//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="EventManager.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Events
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.Assertions;
    using UnityEngine.Events;

    /// <summary>
    ///     An EventManager allowing communication between decoupled components
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        /// <summary>
        ///     Singleton <see cref="EventManager" />
        /// </summary>
        private static EventManager eventManager;

        /// <summary>
        ///     Event storage collection
        /// </summary>
        private Dictionary<Type, UnityEvent<GameEvent>> eventDictionary;

        /// <summary>
        ///     Gets the current <see cref="EventManager" />
        /// </summary>
        public static EventManager Instance
        {
            get
            {
                if (!eventManager)
                {
                    eventManager = FindObjectOfType<EventManager>();

                    Assert.IsTrue(eventManager, "Scene does not have a valid EventManager");

                    if (eventManager)
                    {
                        eventManager.Initialize();
                    }
                }

                return eventManager;
            }
        }

        /// <summary>
        ///     Adds a listener to an event of type <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T">Type of event</typeparam>
        /// <param name="del">UnityAction to execute </param>
        public static void AddListener<T>(UnityAction<GameEvent> del) where T : GameEvent
        {
            UnityEvent<GameEvent> unityEvent;
            if (Instance.eventDictionary.TryGetValue(typeof(T), out unityEvent))
            {
                unityEvent.AddListener(del);
            }
            else
            {
                unityEvent = new EventGameEvent();
                Instance.eventDictionary.Add(typeof(T), unityEvent);
                unityEvent.AddListener(del);
            }
        }

        /// <summary>
        ///     Raises an event of the given GameEvent subtype
        /// </summary>
        /// <param name="gameEvent">Event data to send</param>
        /// <typeparam name="T">The Type of event to raise</typeparam>
        public static void Raise<T>(T gameEvent) where T : GameEvent
        {
            UnityEvent<GameEvent> unityEvent;
            if (Instance.eventDictionary.TryGetValue(typeof(T), out unityEvent))
            {
                unityEvent.Invoke(gameEvent);
            }
        }

        /// <summary>
        ///     Remove a listener of from the event of type <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T">Type of the event</typeparam>
        /// <param name="del">UnityEvent to remove</param>
        public static void RemoveListener<T>(UnityAction<GameEvent> del) where T : GameEvent
        {
            // Early out if the manager is exiting or not created yet
            if (!eventManager)
            {
                return;
            }

            UnityEvent<GameEvent> unityEvent;
            if (Instance.eventDictionary.TryGetValue(typeof(T), out unityEvent))
            {
                unityEvent.RemoveListener(del);
            }
        }

        /// <summary>
        ///     Initialize the class
        /// </summary>
        private void Initialize()
        {
            if (eventDictionary == null)
            {
                Instance.eventDictionary = new Dictionary<Type, UnityEvent<GameEvent>>();
            }
        }

        /// <summary>
        ///     Concrete implementation of UnityEvent&lt;T0&gt;
        /// </summary>
        public class EventGameEvent : UnityEvent<GameEvent>
        {
        }
    }
}