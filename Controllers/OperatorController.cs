using Azure;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using System.Text;
using TomatisCRM_API.Models;
using TomatisCRM_API.Models.Entities;
using TomatisCRM_API.Service;

namespace TomatisCRM_API.Controllers
{
    public class OperatorController : ControllerBase
    {
        
        private readonly TestAppTomatisCrmContext context;

        public OperatorController(TestAppTomatisCrmContext context)
        {
            this.context = context;
        }

        static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes("Tomatis" + rawData + "!@$"));
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
        [Route("/api/operatorzy")]
        public async Task<ActionResult<List<Operatorzy>>> GetOperatorzy()
        {
            var result = await this.context.Operatorzies.ToListAsync();
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        [HttpGet]
        //[EnableCors("AllowAll")]
        [Route("/api/operatorzy/encode/{value}")]
        public async Task<ActionResult<String>> GetHash(String value)
        {
            return ComputeSha256Hash(value);
        }

        [HttpPost]
        //[EnableCors("AllowAll")]
        [Route("/api/operatorzy/add")]
        public async Task<ActionResult<Operatorzy>> PostOperator(Operatorzy operatorzy)
        {
            if (operatorzy == null)
            {
                return NotFound("Invalid operator data.");
            }

            context.Operatorzies.Add(operatorzy);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        //[EnableCors("AllowAll")]
        [Route("/api/operatorzy/update/{id}")]
        public async Task<ActionResult<Operatorzy>> PutOperator(int id, Operatorzy ope)
        {

            this.context.Entry(ope).State = EntityState.Modified;

            try{
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException){
                if (await this.context.Operatorzies.Where(c => c.OpeId == id).FirstOrDefaultAsync() == null){
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpPatch]
        //[EnableCors("AllowAll")]
        [Route("/api/operatorzy/patch/{id}")]
        public async Task<ActionResult<Operatorzy>> PatchSlowniki(int id, [FromBody] JsonPatchDocument<Operatorzy> ope)
        {

            if (ope == null)
            {
                return BadRequest("Brak dokumentu patch.");
            }

            var existingOpe = await context.Operatorzies.FirstOrDefaultAsync(c => c.OpeId == id);
            if (existingOpe == null)
            {
                return NotFound();
            }

            // Tworzymy kopię istniejącego rekordu, aby zastosować zmiany
            var modifiedOpe = new Operatorzy
            {
               OpeId = existingOpe.OpeId,
               OpeNazwa = existingOpe.OpeNazwa,
               OpeEmail = existingOpe.OpeEmail,
               OpeHaslo = existingOpe.OpeHaslo,
               OpeGoogleapikey = existingOpe.OpeGoogleapikey,
               OpeIsAdmin = existingOpe.OpeIsAdmin,
               OpeLogin = existingOpe.OpeLogin
            };

            // Aplikujemy zmiany na kopię rekordu
            ope.ApplyTo(modifiedOpe, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Aktualizujemy istniejący rekord na podstawie zmodyfikowanego
            existingOpe.OpeId = modifiedOpe.OpeId;
            existingOpe.OpeNazwa = modifiedOpe.OpeNazwa;
            existingOpe.OpeEmail = modifiedOpe.OpeEmail;
            existingOpe.OpeHaslo = modifiedOpe.OpeHaslo;
            existingOpe.OpeGoogleapikey = modifiedOpe.OpeGoogleapikey;
            existingOpe.OpeIsAdmin = modifiedOpe.OpeIsAdmin;
            existingOpe.OpeLogin = modifiedOpe.OpeLogin;


            context.Entry(existingOpe).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Operatorzies.Any(c => c.OpeId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [HttpGet]
        //[EnableCors("AllowAll")]
        [Route("/api/operatorzy/{id}")]
        public async Task<ActionResult<Operatorzy>> GetOperator(int id)
        {
            var result = await this.context.Operatorzies.Where(c => c.OpeId == id).FirstOrDefaultAsync();
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        [HttpDelete]
        //[EnableCors("AllowAll")]
        [Route("/api/operatorzy/delete/{id}")]
        public async Task<ActionResult> DeleteOperator(int id)
        {
            var ope = await this.context.Operatorzies.FindAsync(id);
            
            if (ope == null){
                return NotFound();
            }

            this.context.Operatorzies.Remove(ope);
            await this.context.SaveChangesAsync();

            return NoContent();
        }
    }
}
