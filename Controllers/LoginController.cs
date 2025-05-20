using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TomatisCRM_API.Models;
using TomatisCRM_API.Models.Entities;

namespace TomatisCRM_API.Controllers
{
    public class LoginController: ControllerBase
    {
        private readonly TestAppTomatisCrmContext context;
        public LoginController(TestAppTomatisCrmContext context)
        {
            this.context = context;
        }

        static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes("Tomatis"+rawData+"!@$"));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        [HttpGet]
        //[EnableCors("AllowAll")]
        [Route("/api/login/{login};{pass}")]
        public  ActionResult<List<Operatorzy>> GetOperatorzy(string login, string pass)
        {
            var result = this.context.Operatorzies.Where(c => c.OpeLogin == login && c.OpeHaslo == pass).ToList();
            if (result.Count == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
