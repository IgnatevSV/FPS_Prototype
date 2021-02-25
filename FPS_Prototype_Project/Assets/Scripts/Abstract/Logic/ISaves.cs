namespace FPSProject
{
    public interface ISaves
    {
        T GetSavesData<T>() where T : ISavesPart, new();

        void Save();
    }
}