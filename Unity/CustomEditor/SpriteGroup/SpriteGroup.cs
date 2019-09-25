using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SelfieGame.Sticker.Storage
{
    [CreateAssetMenu(fileName = "SpriteGroup", menuName = "Sprites/Group", order = 0)]
    public class SpriteGroup : ScriptableObject
    {
        public Sprite preview;
        public List<Sprite> Sprites = new List<Sprite>();
    }
}