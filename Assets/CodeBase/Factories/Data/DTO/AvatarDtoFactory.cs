using Model;
using UnityEngine;

namespace Factories.Data.DTO
{
    public class AvatarDtoFactory
    {
        public AvatarDto[] CreateAvatarDtos(Sprite[] sprites)
        {
            var avatars = new AvatarDto[sprites.Length];

            for (var i = 0; i < avatars.Length; i++) 
                avatars[i] = CreateNewSpriteInfo(sprites[i], i);
        
            return avatars;
        }

        private AvatarDto CreateNewSpriteInfo(Sprite sprite, int id)
        {
            return new AvatarDto
            {
                Id = id,
                Height = sprite.texture.height,
                Width = sprite.texture.width,
                Data = sprite.texture.GetRawTextureData(),
                TextureFormat = sprite.texture.format,
            };;
        }
    }
}