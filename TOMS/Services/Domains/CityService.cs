using TOMS.DAO;
using TOMS.Models.DataModel;

namespace TOMS.Services.Domains
{
    public class CityService : ICityService
    {
        public readonly ApplicationDbContext _dbcontext;
        public CityService(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public void Create(CityEntity city)
        {
            _dbcontext.Add(city);
            _dbcontext.SaveChanges();
        }

        public void Delete(string id)
        {
            var city = _dbcontext.Cities.Find(id);
            if (city != null)
            {
                _dbcontext.Cities.Remove(city);
                _dbcontext.SaveChanges();
            }
        }

        public CityEntity GetByID(string id)
        {
            return _dbcontext.Cities.Where(p => p.Id == id).SingleOrDefault();
        }

        public void Update(CityEntity city)
        {
            _dbcontext.Cities.Update(city);
            _dbcontext.SaveChanges();
        }

        public IList<CityEntity> RetrieveAll()
        {
            return _dbcontext.Cities.ToList();
        }
    }
}
