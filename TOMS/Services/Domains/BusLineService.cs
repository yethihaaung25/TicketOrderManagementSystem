using TOMS.DAO;
using TOMS.Models.DataModel;

namespace TOMS.Services.Domains
{
    public class BusLineService : IBusLineService
    {
        private readonly ApplicationDbContext _dbcontext;
        public BusLineService(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public void Create(BusLineEntity bus)
        {
            _dbcontext.BusLines.Add(bus);
            _dbcontext.SaveChanges();
        }

        public void Delete(string id)
        {
            var bus = _dbcontext.BusLines.Find(id);
            if (bus != null)
            {
                _dbcontext.BusLines.Remove(bus);
                _dbcontext.SaveChanges();
            }
        }

        public BusLineEntity GetByID(string id)
        {
            return _dbcontext.BusLines.Find(id);
        }

        public void Update(BusLineEntity bus)
        {
            _dbcontext.BusLines.Update(bus);
            _dbcontext.SaveChanges();
        }

        public IList<BusLineEntity> RetrieveAll()
        {
            return _dbcontext.BusLines.ToList();
        }
    }
}
