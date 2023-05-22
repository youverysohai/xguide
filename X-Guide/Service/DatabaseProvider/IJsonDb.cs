namespace X_Guide.Service.DatabaseProvider
{
    public interface IJsonDb
    {
        T Get<T>() where T : new();

        void Update<T>(T data);
    }
}