using System;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class GameEventListenerInt : MonoBehaviour
    {
        [Serializable]
        public class TypedUnityEvent : UnityEvent<int> { }

        [SerializeField]
        public GameEventInt gameEvent;

        [SerializeField]
        public TypedUnityEvent unityEvent;

        public void Invoke(int t)
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
}
