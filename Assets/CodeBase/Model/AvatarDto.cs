using System;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class AvatarDto
    {
        public int Id;
        public byte[] Data;
        public int Width;
        public int Height;
        public TextureFormat TextureFormat;
    }
}