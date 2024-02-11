using TOMS.Models.DataModel;

namespace TOMS.Services.Domains
{
    public interface ICityService
    {
        void Create(CityEntity city);
        IList<CityEntity> RetrieveAll();

        CityEntity GetByID(string id);
        void Update(CityEntity city);
        void Delete(string id);
    }
}
