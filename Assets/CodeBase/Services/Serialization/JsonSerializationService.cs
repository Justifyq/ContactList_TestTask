using UnityEngine;

namespace Services.Serialization
{
    public class JsonSerializationService : ISerializationService
    {
        public string Serialize<T>(T obj) => JsonUtility.ToJson(obj);

        public T Deserialize<T>(string json) => JsonUtility.FromJson<T>(json);
    }
}