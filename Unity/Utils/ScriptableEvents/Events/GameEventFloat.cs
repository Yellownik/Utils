using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "GameEventFloat", menuName = "GameEvent/GameEvent Float", order = 2)]
    public class GameEventFloat : GameEvent<float>
    {
        public float Value;
    }
}
