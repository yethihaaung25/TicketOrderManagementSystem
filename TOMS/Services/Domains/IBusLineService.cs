using TOMS.Models.DataModel;

namespace TOMS.Services.Domains
{
    public interface IBusLineService
    {
        void Create(BusLineEntity bus);
        IList<BusLineEntity> RetrieveAll();

        BusLineEntity GetByID(string id);
        void Update(BusLineEntity bus);
        void Delete(string id);
    }
}
