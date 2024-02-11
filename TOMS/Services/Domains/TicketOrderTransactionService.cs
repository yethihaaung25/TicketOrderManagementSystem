using TOMS.DAO;
using TOMS.Models.DataModel;

namespace TOMS.Services.Domains
{
    public class TicketOrderTransactionService : ITicketOrderTransactionService
    {
        private readonly ApplicationDbContext _context;
        public TicketOrderTransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Create(TicketOrderTransactionEntity ticket)
        {
            _context.TicketOrderTransactions.Add(ticket);
            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            var ticket = _context.TicketOrderTransactions.Find(id);
            if (ticket != null) 
            {
                _context.TicketOrderTransactions.Remove(ticket);
                _context.SaveChanges();
            }
        }

        public TicketOrderTransactionEntity GetById(string id)
        {
            return _context.TicketOrderTransactions.Where(x => x.Id == id).FirstOrDefault();
        }

        public IList<TicketOrderTransactionEntity> RetrieveAll()
        {
            return _context.TicketOrderTransactions.ToList();
        }

        public void Update(TicketOrderTransactionEntity ticket)
        {
            _context.TicketOrderTransactions.Update(ticket);
        }
    }
}
