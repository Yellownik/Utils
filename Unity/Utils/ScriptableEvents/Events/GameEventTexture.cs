using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "GameEventTexture", menuName = "GameEvent/GameEvent Texture", order = 4)]
    public class GameEventTexture : GameEvent<Texture>
    {
    }
}