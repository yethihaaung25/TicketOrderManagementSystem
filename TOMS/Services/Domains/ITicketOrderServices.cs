using TOMS.Models.DataModel;
using TOMS.Models.ViewModel;

namespace TOMS.Services.Domains
{
    public interface ITicketOrderServices
    {
        // CRUD process for passenger domain
        void Create(TicketEntity ticket);
        void Create(List<TicketEntity> ticket);
        IList<TicketEntity> RetrieveAll();
        // for available and non-available tickets checking by
        IList<SeatPlan> ReteriveByTicketOrderedDateAndRouteId(DateTime ticketOrderedDate, string routeId);//reterice process
        void Update(TicketEntity ticket);//update process 
        TicketEntity GetById(string id);//get the recrod to called the update function
        void Delete(string Id);//for delete process according to Id
    }
}
