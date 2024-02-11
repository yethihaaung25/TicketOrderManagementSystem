using TOMS.DAO;
using TOMS.Models.DataModel;

namespace TOMS.Services.Domains
{
	public class PassengerService : IPassengerService
	{
		private readonly ApplicationDbContext _dbcontext;
		public PassengerService(ApplicationDbContext dbcontext)
		{
			_dbcontext = dbcontext;
		}

		public void Create(PassengerEntity passenger)
		{
			_dbcontext.Passengers.Add(passenger);
			_dbcontext.SaveChanges();
		}

		public void Delete(string id)
		{
			var passenger = _dbcontext.Passengers.Find(id);
			if(passenger != null)
			{
				_dbcontext.Passengers.Remove(passenger);
				_dbcontext.SaveChanges();
			}
		}

		public PassengerEntity GetByID(string id)
		{
			return _dbcontext.Passengers.Where(p => p.Id == id).SingleOrDefault();
		}

		public void Update(PassengerEntity passenger) 
		{
			_dbcontext.Passengers.Update(passenger);
			_dbcontext.SaveChanges();
		}

		public IList<PassengerEntity> RetrieveAll()
		{
			return _dbcontext.Passengers.ToList();
		}
	}
}
