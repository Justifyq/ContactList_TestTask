using System;
using UnityEngine;

namespace Model
{
    public class Avatar
    {
        public event Action SpriteUpdated;
        public int Id { get; }
        public Sprite Sprite { get; private set; }

        public Avatar(int id, Sprite sprite)
        {
            Id = id;
            Sprite = sprite;
        }

        public void UpdateSprite(Sprite sprite)
        {
            Sprite = sprite;
            SpriteUpdated?.Invoke();
        }

    }
}