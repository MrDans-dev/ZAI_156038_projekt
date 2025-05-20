using TomatisCRM_API.Models;
using TomatisCRM_API.Models.Entities;

namespace TomatisCRM_API.Service
{
    public interface IWizytyService
    {
        List<Wizyty> WizytyToList();
        List<WizytyModel> GetWizyty();
        List<WizytyModel> GetWizyta(int wizId);
    }
}
