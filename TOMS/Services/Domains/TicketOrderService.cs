using TOMS.DAO;
using TOMS.Models.DataModel;
using TOMS.Models.ViewModel;

namespace TOMS.Services.Domains
{
    public class TicketOrderService : ITicketOrderServices
    {
        private readonly ApplicationDbContext _context;
        public TicketOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Create(TicketEntity ticket)
        {
            _context.Tickets.Add(ticket);
            _context.SaveChanges();
        }

        public void Create(List<TicketEntity> tickets)
        {
            foreach(TicketEntity ticket in tickets)
            {
                _context.Tickets.Add(ticket);
                _context.SaveChanges();
            }
        }

        public void Delete(string Id)
        {
            var ticket = _context.Tickets.Find(Id);
            if(ticket != null)
            {
                _context.Tickets.Remove(ticket);
                _context.SaveChanges();
            }
        }

        public TicketEntity GetById(string id)
        {
            return _context.Tickets.Where(x => x.Id == id).SingleOrDefault();
        }

        public IList<SeatPlan> ReteriveByTicketOrderedDateAndRouteId(DateTime ticketOrderedDate, string routeId)
        {
            return _context.Tickets.Where(x => x.TicketOrderedDate == ticketOrderedDate && x.RouteId == routeId).Select(z => new SeatPlan(z.SeatNo)).ToList();
        }

        public IList<TicketEntity> RetrieveAll()
        {
            return _context.Tickets.ToList();
        }

        public void Update(TicketEntity ticket)
        {
            _context.Tickets.Update(ticket);
            _context.SaveChanges();
        }
    }
}
