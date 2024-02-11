using TOMS.Models.DataModel;

namespace TOMS.Services.Domains
{
    public interface IPaymentTypeService
    {
        void Create(PaymentTypeEntity paymentType);
        IList<PaymentTypeEntity> RetrieveAll();
        PaymentTypeEntity GetByID(string id);
        void Update(PaymentTypeEntity paymentType);
        void Delete(string id);
    }
}
