using System;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class GameEventListenerGeneric<T> : MonoBehaviour
    {
        [Serializable]
        public class TypedUnityEvent : UnityEvent<T> { }

        [Serializable]
        public class TypedGameEvent : GameEvent<T> { }

        [SerializeField]
        public TypedGameEvent gameEvent;

        [SerializeField]
        public TypedUnityEvent unityEvent;

        public void Invoke(T t)
        {
            unityEvent.Invoke(t);
        }

        void OnEnable()
        {
            gameEvent.RegisterListener(Invoke);
        }

        void OnDisable()
        {
            gameEvent.UnregisterListener(Invoke);
        }
    }

    public class GameEventListenerGeneric<T, U> : MonoBehaviour
    {
        [Serializable]
        public class TypedUnityEvent : UnityEvent<T, U> { }

        public GameEvent<T, U> gameEvent;
        public TypedUnityEvent unityEvent;

        public void Invoke(T t, U u)
        {
            unityEvent.Invoke(t, u);
        }

        void OnEnable()
        {
            gameEvent.RegisterListener(Invoke);
        }

        void OnDisable()
        {
            gameEvent.UnregisterListener(Invoke);
        }
    }

    public class GameEventListenerGeneric<T, U, V> : MonoBehaviour
    {
        [Serializable]
        public class TypedUnityEvent : UnityEvent<T, U, V> { }

        public GameEvent<T, U, V> gameEvent;
        public TypedUnityEvent unityEvent;

        public void Invoke(T t, U u, V v)
        {
            unityEvent.Invoke(t, u, v);
        }

        void OnEnable()
        {
            gameEvent.RegisterListener(Invoke);
        }

        void OnDisable()
        {
            gameEvent.UnregisterListener(Invoke);
        }
    }

    public class GameEventListenerGeneric<T, U, V, W> : MonoBehaviour
    {
        [Serializable]
        public class TypedUnityEvent : UnityEvent<T, U, V, W> { }

        public GameEvent<T, U, V, W> gameEvent;
        public TypedUnityEvent unityEvent;

        public void Invoke(T t, U u, V v, W w)
        {
            unityEvent.Invoke(t, u, v, w);
        }

        void OnEnable()
        {
            gameEvent.RegisterListener(Invoke);
        }

        void OnDisable()
        {
            gameEvent.UnregisterListener(Invoke);
        }
    }
}
