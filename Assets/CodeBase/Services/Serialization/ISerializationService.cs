namespace Services.Serialization
{
    public interface ISerializationService
    {
        string Serialize<T>(T obj);
        T Deserialize<T>(string json);
    }
}