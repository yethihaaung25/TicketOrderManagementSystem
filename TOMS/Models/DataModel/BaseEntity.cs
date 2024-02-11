using TOMS.Services.Utilities;

namespace TOMS.Models.DataModel
{
	public class BaseEntity
	{
		public string Id { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string IpAddress { get; set; } = NetworkHelper.GetLocalIPAddress();
	}
}
