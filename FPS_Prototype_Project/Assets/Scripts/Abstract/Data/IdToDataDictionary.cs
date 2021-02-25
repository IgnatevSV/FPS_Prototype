namespace FPSProject
{
    public abstract class IdToDataDictionary<T> : SerializableDictionary<int, T> where T : IConfigData {}
}