using System;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class GameEventListenerBool : MonoBehaviour
    {
        [Serializable]
        public class TypedUnityEvent : UnityEvent<bool> { }

        [SerializeField]
        public GameEventBool gameEvent;

        [SerializeField]
        public TypedUnityEvent unityEvent;

        public void Invoke(bool t)
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
