using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "GameEventBool", menuName = "GameEvent/GameEvent Bool", order = 1)]
    public class GameEventBool : GameEvent<bool>
    {
        public bool Value;
    }
}
