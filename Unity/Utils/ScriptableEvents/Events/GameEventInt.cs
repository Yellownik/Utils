using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "GameEventInt", menuName = "GameEvent/GameEvent Int", order = 2)]
    public class GameEventInt : GameEvent<int>
    {
        public int Value = 0;
    }
}
