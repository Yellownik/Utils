using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent gameEvent;
        public UnityEvent unityEvent;

        public void Invoke()
        {
            unityEvent.Invoke();
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
