using X_Guide.MVVM.Model;

namespace X_Guide.Service.DatabaseProvider
{
    public interface IGeneralDb
    {
        GeneralModel Get();

        void Update(GeneralModel generalVm);
    }
}