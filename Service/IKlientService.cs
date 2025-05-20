using TomatisCRM_API.Models.Entities;
using TomatisCRM_API.Models;

namespace TomatisCRM_API.Service
{
    public interface IKlientService
    {
        List<Klient> KlientToList();
        List<KlientModel> GetKlients();
        List<KlientModel> GetKlient(int kntId);
    }
}
