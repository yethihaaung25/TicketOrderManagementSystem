using TOMS.Models.DataModel;

namespace TOMS.Services.Domains
{
	public interface IPassengerService
	{
		// CRUD Process
		void Create(PassengerEntity passenger);
		IList<PassengerEntity> RetrieveAll();

		PassengerEntity GetByID(string id);
		void Update(PassengerEntity passenger);
		void Delete(string id);

	}
}
