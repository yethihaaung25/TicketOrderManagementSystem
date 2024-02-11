using System.ComponentModel.DataAnnotations.Schema;

namespace TOMS.Models.DataModel
{
    [Table("PaymentType")]
    public class PaymentTypeEntity : BaseEntity
    {
        public string Type { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
    }
}
