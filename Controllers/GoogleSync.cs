using Microsoft.AspNetCore.Mvc;
using TomatisCRM_API.Entities;
using TomatisCRM_GoogleSync_pack;

namespace TomatisCRM_API.Controllers
{
    public class GoogleSync : Controller
    {
        [HttpGet]
        [Route("/api/googlesync")]
        public ActionResult<String> StartSync()
        {
            var sync = new TomatisCRM_GoogleSync();
            return sync.GoogleSync();   
        }
    }
}
