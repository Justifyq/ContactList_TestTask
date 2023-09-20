using UnityEngine;

namespace Model
{
    public class Avatar
    {
        public int Id { get; }
        public Sprite Sprite { get; }

        public Avatar(AvatarDto avatarDto)
        {
            Id = avatarDto.Id;
            var tex = new Texture2D(avatarDto.Width, avatarDto.Height, avatarDto.TextureFormat, false);
            tex.LoadRawTextureData(avatarDto.Data);
            tex.Apply();
            Sprite = Sprite.Create(tex, new Rect(0,0, avatarDto.Width, avatarDto.Height), new Vector2(.5f, .5f));
        }
    
    }
}