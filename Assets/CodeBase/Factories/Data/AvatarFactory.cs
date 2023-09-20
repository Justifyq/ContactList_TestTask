using Model;

namespace Factories.Data
{
    public class AvatarFactory
    {
        public Avatar[] CreateAvatars(AvatarDto[] avatarDtos)
        {
            var avatars = new Avatar[avatarDtos.Length];

            var dtos = avatarDtos;

            for (var i = 0; i < avatars.Length; i++)
                avatars[i] = new Avatar(dtos[i]);

            return avatars;
        }
    }
}