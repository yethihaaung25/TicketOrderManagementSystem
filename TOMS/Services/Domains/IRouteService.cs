using System.Xml.Serialization;
using TOMS.Models.DataModel;

namespace TOMS.Services.Domains
{
    public interface IRouteService
    {
        void Create(RouteEntity route);
        IList<RouteEntity> RetrieveAll();

        RouteEntity GetByID(string id);
        void Update(RouteEntity route);
        void Delete(string id);

    }
}
