using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class GameEvent<T> : ScriptableObject
    {
        private readonly List<Action<T>> listeners = new List<Action<T>>();

        public void Invoke(T t)
        {
            var listenersCopy = listeners.ToArray();
            for (int i = 0; i < listenersCopy.Length; i++)
            {
                try
                {
                    listenersCopy[i].Invoke(t);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        public void RegisterListener(Action<T> listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(Action<T> listener)
        {
            listeners.Remove(listener);
        }
    }

    public class GameEvent<T, U> : ScriptableObject
    {
        private readonly List<Action<T, U>> listeners = new List<Action<T, U>>();

        public void Invoke(T t, U u)
        {
            var listenersCopy = listeners.ToArray();
            for (int i = 0; i < listenersCopy.Length; i++)
            {
                try
                {
                    listenersCopy[i].Invoke(t, u);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        public void RegisterListener(Action<T, U> listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(Action<T, U> listener)
        {
            listeners.Remove(listener);
        }
    }

    public class GameEvent<T, U, V> : ScriptableObject
    {
        private readonly List<Action<T, U, V>> listeners = new List<Action<T, U, V>>();

        public void Invoke(T t, U u, V v)
        {
            var listenersCopy = listeners.ToArray();
            for (int i = 0; i < listenersCopy.Length; i++)
            {
                try
                {
                    listenersCopy[i].Invoke(t, u, v);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        public void RegisterListener(Action<T, U, V> listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(Action<T, U, V> listener)
        {
            listeners.Remove(listener);
        }
    }

    public class GameEvent<T, U, V, W> : ScriptableObject
    {
        private readonly List<Action<T, U, V, W>> listeners = new List<Action<T, U, V, W>>();

        public void Invoke(T t, U u, V v, W w)
        {
            var listenersCopy = listeners.ToArray();
            for (int i = 0; i < listenersCopy.Length; i++)
            {
                try
                {
                    listenersCopy[i].Invoke(t, u, v, w);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        public void RegisterListener(Action<T, U, V, W> listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(Action<T, U, V, W> listener)
        {
            listeners.Remove(listener);
        }
    }
}
