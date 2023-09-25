using System;
using System.Linq;
using Model;
using Model.DTO.Storages;
using Services.Network;
using UnityEngine;
using Avatar = Model.Avatar;

namespace Factories.Data
{
    public class AvatarFactory
    {
        private readonly Sprite _defaultSprite;
        private readonly ISpriteLoaderService _spriteLoaderService;
        
        public AvatarFactory(Sprite defaultSprite, ISpriteLoaderService spriteLoaderService)
        {
            _defaultSprite = defaultSprite;
            _spriteLoaderService = spriteLoaderService;
        }

        public Avatar[] CreateAvatars(int avatarCount, Action<AvatarDto[]> dtoCreated)
        {
            var avatars = new Avatar[avatarCount];

            for (var i = 0; i < avatars.Length; i++)
                avatars[i] = new Avatar(i, _defaultSprite);

            _spriteLoaderService.LoadSprites(avatarCount, s => SpriteLoader_Loaded(avatars, s, dtoCreated));
            return avatars;
        }
        
        public Avatar[] CreateAvatars(AvatarDtoStorage storage)
        {
            var avatars = new Avatar[storage.Data.Length];
            
            for (var i = 0; i < avatars.Length; i++)
            {
                var dto = storage.Data.FirstOrDefault(d => d.Id == i);
                avatars[i] = new Avatar(i, _defaultSprite);
                avatars[i].UpdateSprite(CreateSprite(dto));
            }

            return avatars;
        }

        private void SpriteLoader_Loaded(Avatar[] avatars, Sprite[] sprites, Action<AvatarDto[]> dtoCreated)
        {
            var dtos = new AvatarDto[sprites.Length];
            for (var i = 0; i < avatars.Length; i++)
            {
                var dto = new AvatarDto
                {
                    Id = i,
                    Width = sprites[i].texture.width,
                    Height = sprites[i].texture.height,
                    TextureFormat = sprites[i].texture.format,
                    Data = sprites[i].texture.GetRawTextureData(),
                };

                dtos[i] = dto;
                avatars[i].UpdateSprite(sprites[i]);
            }
            
            dtoCreated?.Invoke(dtos);
        }

        private Sprite CreateSprite(AvatarDto avatarDto)
        {
            var tex = new Texture2D(avatarDto.Width, avatarDto.Height, avatarDto.TextureFormat, false);
            tex.LoadRawTextureData(avatarDto.Data);
            tex.Apply();
            return Sprite.Create(tex, new Rect(0,0, avatarDto.Width, avatarDto.Height), new Vector2(.5f, .5f));
        }
    }
}