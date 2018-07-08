using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    [RequireComponent(typeof(Button))]
    public class ButtonEventCaller : MonoBehaviour
    {
        [SerializeField]
        private GameEvent onButtonClickEvent;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            if (onButtonClickEvent != null) {
                onButtonClickEvent.Invoke();
            }
        }
    }
}
