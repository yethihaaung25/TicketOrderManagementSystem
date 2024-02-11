using TOMS.Models.DataModel;

namespace TOMS.Services.Domains
{
    public interface ITicketOrderTransactionService
    {
        void Create(TicketOrderTransactionEntity ticket);
        IList<TicketOrderTransactionEntity> RetrieveAll();
        void Update(TicketOrderTransactionEntity ticket);
        TicketOrderTransactionEntity GetById(string id);
        void Delete(string id);
    }
}
