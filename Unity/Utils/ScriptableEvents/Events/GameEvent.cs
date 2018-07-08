using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvent/GameEvent", order = 0)]
    public class GameEvent : ScriptableObject
    {
        private readonly List<Action> listeners = new List<Action>();

        public void Invoke()
        {
            var listenersCopy = listeners.ToArray();
            for(int i = 0; i < listenersCopy.Length; i++)
            {
                try
                {
                    listenersCopy[i].Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        public void RegisterListener(Action listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(Action listener)
        {
            listeners.Remove(listener);
        }
    }
}
