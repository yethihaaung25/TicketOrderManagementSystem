using TOMS.DAO;
using TOMS.Models.DataModel;

namespace TOMS.Services.Domains
{
    public class RouteService : IRouteService
    {
        private readonly ApplicationDbContext _dbContext;
        public RouteService(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public void Create(RouteEntity route)
        {
            _dbContext.Routes.Add(route);
            _dbContext.SaveChanges();
        }

        public void Delete(string id)
        {
            var route = _dbContext.Routes.Find(id);
            if (route != null)
            {
                _dbContext.Routes.Remove(route);
                _dbContext.SaveChanges();
            }
        }

        public RouteEntity GetByID(string id)
        {
            return _dbContext.Routes.Where(p => p.Id == id).SingleOrDefault();
        }

        public IList<RouteEntity> RetrieveAll()
        {
            return _dbContext.Routes.ToList();
        }

        public void Update(RouteEntity route)
        {
            _dbContext.Routes.Update(route);
            _dbContext.SaveChanges();
        }
    }
}
