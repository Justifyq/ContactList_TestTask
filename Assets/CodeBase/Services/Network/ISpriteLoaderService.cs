using System;
using UnityEngine;

namespace Services.Network
{
    public interface ISpriteLoaderService : IDisposable
    {
        void LoadSprites(int spritesCount, Action<Sprite[]> loaded);
    }
}