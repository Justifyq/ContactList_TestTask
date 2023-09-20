using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace Services.Network
{
    public class HttpSpriteLoader : ISpriteLoaderService
    {
        private const string Uri = "https://loremflickr.com/320/240";
        private readonly ICoroutineRunner _coroutineRunner;
        private Coroutine _coroutine;

        public HttpSpriteLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void LoadSprites(int spritesCount, Action<Sprite[]> loaded) => _coroutine = _coroutineRunner.StartCoroutine(LoadSpritesCoroutine(spritesCount, loaded));

        public void Dispose() => _coroutineRunner.StopCoroutine(_coroutine);
        
        private IEnumerator LoadSpritesCoroutine(int spritesCount, Action<Sprite[]> loaded)
        {
            var sprites = new Sprite[spritesCount];

            for (int i = 0; i < sprites.Length; i++)
            {
                var request = UnityWebRequestTexture.GetTexture(Uri);
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    loaded?.Invoke(Array.Empty<Sprite>());
                    yield break;
                }
                
                var texture = DownloadHandlerTexture.GetContent(request);
                sprites[i] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));
            }
        
            loaded?.Invoke(sprites);
        }
    }
}