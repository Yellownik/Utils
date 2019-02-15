using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SelfieGame.Sticker.Storage
{
    [CreateAssetMenu(fileName = "StickerGroup", menuName = "Stickers/Group", order = 0)]
    public class StickerGroup : ScriptableObject
    {
        public Sprite preview;

        public int StickerCount = 0;
        public List<Sprite> Stickers = new List<Sprite>();
    }
}