using TOMS.DAO;
using TOMS.Models.DataModel;

namespace TOMS.Services.Domains
{
    public class PaymentTypeService : IPaymentTypeService
    {
        private readonly ApplicationDbContext _dbcontext;
        public PaymentTypeService(ApplicationDbContext dbcontext) 
        {
            _dbcontext = dbcontext;
        }

        public void Create(PaymentTypeEntity paymentType)
        {
            _dbcontext.PaymentTypes.Add(paymentType);
            _dbcontext.SaveChanges();
        }

        public void Delete(string id)
        {
            var payment = _dbcontext.PaymentTypes.Find(id);
            if(payment != null) 
            {
                _dbcontext.PaymentTypes.Remove(payment);
                _dbcontext.SaveChanges();
            }
        }

        public PaymentTypeEntity GetByID(string id)
        {
            return _dbcontext.PaymentTypes.Where(p => p.Id == id).SingleOrDefault();
        }

        public IList<PaymentTypeEntity> RetrieveAll()
        {
            return _dbcontext.PaymentTypes.ToList();
        }

        public void Update(PaymentTypeEntity paymentType)
        {
            _dbcontext.PaymentTypes.Update(paymentType);
            _dbcontext.SaveChanges();
        }
    }
}
