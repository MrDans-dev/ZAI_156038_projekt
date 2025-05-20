using TomatisCRM_API.Models;
using TomatisCRM_API.Models.Entities;

namespace TomatisCRM_API.Service
{
    public interface IOperatorService
    {
        List<Operatorzy> GetOperatorzy();

        //Operatorzy GetOperator(int id);
    }
}
