using System;
using System.Collections;
using Model.DTO.Storages;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace Services.Network
{
    public class HttpContactLoader : IContactsLoader
    {
        private const string Uri = "https://drive.google.com/uc?export=download&id=1YvE6Y5-vxVWXrrYyb83ssqMfVUUkndLw";
        
        private readonly ICoroutineRunner _coroutineRunner;

        public HttpContactLoader(ICoroutineRunner coroutineRunner) => _coroutineRunner = coroutineRunner;

        public void Load(Action<EmployeesInfoStorage> loaded) => _coroutineRunner.StartCoroutine(LoadContactsCoroutine(loaded));

        private IEnumerator LoadContactsCoroutine(Action<EmployeesInfoStorage> callback)
        {
            var request = UnityWebRequest.Get(Uri);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                callback?.Invoke(null);
                yield break;
            }
            
            callback.Invoke(JsonUtility.FromJson<EmployeesInfoStorage>(request.downloadHandler.text));
        }
    }
}