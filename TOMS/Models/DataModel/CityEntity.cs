using System.ComponentModel.DataAnnotations.Schema;

namespace TOMS.Models.DataModel
{
    [Table("City")]
    public class CityEntity : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ZipCode { get; set; }

    }
}
