using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TomatisCRM_API.Models;
using TomatisCRM_API.Models.Entities;

namespace TomatisCRM_API.Controllers
{
    public class AppConfigController : ControllerBase
    {
        private readonly TestAppTomatisCrmContext context;

        public AppConfigController(TestAppTomatisCrmContext context)
        {
            this.context = context;
        }
        public async Task<ActionResult<List<AppConf>>> AppConfToList()
        {
            return await this.context.AppConfs.ToListAsync();
        }

        [HttpGet]
        //[EnableCors("AllowAll")]
        [Route("/api/appconf")]
        public async Task<ActionResult<List<KlientModel>>> GetKlients()
        {
            var list = await AppConfToList();
            return Ok(list);
        }

        [HttpGet]
        ////[EnableCors("AllowAll")]
        [Route("/api/appconf/{confID}")]
        public async Task<ActionResult<List<AppConf>>> GetConfById(int confID)
        {
            var wynik = AppConfToList().Result.Value.Where(s => s.SconfId == confID).ToList();

            if (wynik == null || !wynik.Any())
            {
                return NotFound();
            }
            return Ok(wynik);
        }

        [HttpGet]
        //[EnableCors("AllowAll")]
        [Route("/api/appconf/{confNazwa}")]
        public async Task<ActionResult<List<AppConf>>> GetConfByName(string ConfNazwa)
        {
            var wynik = AppConfToList().Result.Value.Where(s => s.SconfNazwa == ConfNazwa).ToList();

            if (wynik == null || !wynik.Any())
            {
                return NotFound();
            }
            return Ok(wynik);
        }

        [HttpPut]
        //[EnableCors("AllowAll")]
        [Route("/api/appconf/update/{id}")]
        public async Task<ActionResult<Klient>> PutAppConfig(int id, AppConf appconf)
        {

            this.context.Entry(appconf).State = EntityState.Modified;

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.context.AppConfs.Where(c => c.SconfId == id).FirstOrDefaultAsync() == null)
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

        [HttpPatch]
        //[EnableCors("AllowAll")]
        [Route("/api/appconf/patch/{id}")]
        public async Task<ActionResult<AppConf>> PatchAppConfig(int id, [FromBody] JsonPatchDocument<AppConf> appconf)
        {

            if (appconf == null)
            {
                return BadRequest("Invalid patch document");
            }

            var appconf_c = this.context.AppConfs.FirstOrDefault(u => u.SconfId == id);

            if (appconf_c == null)
            {
                return NotFound();
            }

            this.context.Entry(appconf_c).Property(p => p.SconfNazwa).IsModified = true;

            try
            {
                appconf.ApplyTo(appconf_c);

                await this.context.SaveChangesAsync();
                return Ok(appconf_c);
            }
            catch (Exception ex)
            {
                // Log the error details
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }



}
