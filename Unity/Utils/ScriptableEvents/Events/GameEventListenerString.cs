using System;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class GameEventListenerString : MonoBehaviour
    {
        [Serializable]
        public class TypedUnityEvent : UnityEvent<string> { }

        [SerializeField]
        public GameEventString gameEvent;

        [SerializeField]
        public TypedUnityEvent unityEvent;

        public void Invoke(string t)
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
