namespace FPSProject
{
    public interface IPoolableObject
    {
        IPoolableObject Prefab { get; }
        bool IsInUse { get; set; }
        void ResetPoolableObject();
    }
}