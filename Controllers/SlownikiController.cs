using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using TomatisCRM_API.Models;
using TomatisCRM_API.Models.Entities;

namespace TomatisCRM_API.Controllers
{
    public class SlownikiController : ControllerBase
    {
        private readonly TestAppTomatisCrmContext context;

        public SlownikiController(TestAppTomatisCrmContext context)
        {
            this.context = context;
        }

        [HttpGet]
        //[EnableCors("AllowAll")]
        [Route("/api/slowniki")]
        public async Task<ActionResult<List<Slowniki>>> GetSlowniki()
        {
            var slownikiDtos = await this.context.Slownikis
                                   .Include(s => s.SlwOpeUtwNavigation)
                                   .Select(s => new Slowniki
                                   {
                                       SlwId = s.SlwId,
                                       SlwTyp = s.SlwTyp,
                                       SlwWartoscS = s.SlwWartoscS,
                                       SlwWartoscD = s.SlwWartoscD,
                                       SlwDataUtw = s.SlwDataUtw,
                                       OpeNazwa = s.SlwOpeUtwNavigation.OpeNazwa,
                                       SlwNextSlw = s.SlwNextSlw,
                                       SlwOpis = s.SlwOpis,
                                   })
                                   .ToListAsync();

            return slownikiDtos;
        }

        [HttpGet]
        //[EnableCors("AllowAll")]
        [Route("/api/slowniki/{id}")]
        public async Task<ActionResult<List<Slowniki>>> GetSlownikiById(int id)
        {
            var slownikiDtos = await this.context.Slownikis
                                   .Include(s => s.SlwOpeUtwNavigation)
                                   .Select(s => new Slowniki
                                   {
                                       SlwId = s.SlwId,
                                       SlwTyp = s.SlwTyp,
                                       SlwWartoscS = s.SlwWartoscS,
                                       SlwWartoscD = s.SlwWartoscD,
                                       SlwDataUtw = s.SlwDataUtw,
                                       OpeNazwa = s.SlwOpeUtwNavigation.OpeNazwa,
                                       SlwNextSlw = s.SlwNextSlw,
                                       SlwOpis = s.SlwOpis
                                   }).Where(s => s.SlwId == id)
                                   .ToListAsync();

            return slownikiDtos;
        }

        public string GetNextEtap(int SlwNextEtap)
        {
            var etap = context.Slownikis.Where(s => s.SlwId == SlwNextEtap)
                .Select(s => s.SlwWartoscS).First();
            if (etap == null) return "";
            return etap;
        }

        [HttpGet]
        //[EnableCors("AllowAll")]
        [Route("/api/etapy")]
        public async Task<ActionResult<List<Etapy>>> GetEtapy()
        {
            var slowniki = this.context.Slownikis.Where(s => s.SlwTyp == 1).Include(s => s.SlwOpeUtwNavigation)
                .Select(s => new Slowniki
                {
                    SlwId = s.SlwId,
                    SlwWartoscS = s.SlwWartoscS,
                    SlwNextSlw = s.SlwNextSlw,
                    OpeNazwa = s.SlwOpeUtwNavigation.OpeNazwa,
                    SlwOpis = s.SlwOpis
                }).ToList();

            List<Etapy> etapy = new List<Etapy>();

            foreach (Slowniki slw in slowniki)
            {
                if (slw.SlwNextSlw.HasValue)
                {
                    etapy.Add(new Etapy()
                    {
                        EtapId = slw.SlwId,
                        EtapNazwa = slw.SlwWartoscS,
                        EtapNextNazwa = GetNextEtap(slw.SlwNextSlw.Value),
                        EtapOpis = slw.SlwOpis

                    });
                }
                else
                {
                    etapy.Add(new Etapy()
                    {
                        EtapId = slw.SlwId,
                        EtapNazwa = slw.SlwWartoscS,
                        EtapNextNazwa = "",
                        EtapOpis = slw.SlwOpis

                    });
                }
            }
            return etapy;
        }

        [HttpGet]
        //[EnableCors("AllowAll")]
        [Route("/api/KntStatus")]
        public async Task<ActionResult<List<KntStatus>>> GetKntStatus()
        {
            var slowniki = this.context.Slownikis.Where(s => s.SlwTyp == 2).Include(s => s.SlwOpeUtwNavigation)
                .Select(s => new Slowniki
                {
                    SlwId = s.SlwId,
                    SlwWartoscS = s.SlwWartoscS,
                    OpeNazwa = s.SlwOpeUtwNavigation.OpeNazwa,
                    SlwOpis = s.SlwOpis
                });

            List<KntStatus> kntstatus = new List<KntStatus>();

            foreach (Slowniki s in slowniki)
            {
                kntstatus.Add(new KntStatus()
                {
                    StatusId = s.SlwId,
                    StatusNazwa = s.SlwWartoscS,
                    StatusOpis = s.SlwOpis
                
                });
            }
            return kntstatus;
        }

        [HttpPost]
        //[EnableCors("AllowAll")]
        [Route("/api/slowniki/add")]
        public async Task<ActionResult<Slowniki>> PostSlowniki(Slowniki slowniki)
        {
            if (slowniki == null)
            {
                return NotFound("Invalid slowniki data.");
            }
            context.Slownikis.Add(slowniki);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        //[EnableCors("AllowAll")]
        [Route("/api/slowniki/delete/{id}")]
        public async Task<ActionResult> DeleteSlownik(int id)
        {
            var ope = await this.context.Slownikis.FindAsync(id);

            if (ope == null)
            {
                return NotFound();
            }

            this.context.Slownikis.Remove(ope);
            await this.context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        //[EnableCors("AllowAll")]
        [Route("/api/slowniki/update/{id}")]
        public async Task<ActionResult<Slowniki>> PutSlowniki(int id, Slowniki slw)
        {

            this.context.Entry(slw).State = EntityState.Modified;

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.context.Slownikis.Where(c => c.SlwId == id).FirstOrDefaultAsync() == null)
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
        [Route("/api/slowniki/patch/{id}/{OpeId}")]
        public async Task<ActionResult<Slowniki>> PatchSlowniki(int id, int OpeId, [FromBody] JsonPatchDocument<Slowniki> slw)
        {

            if (slw == null)
            {
                return BadRequest("Brak dokumentu patch.");
            }

            var existingSlw = await context.Slownikis.FirstOrDefaultAsync(c => c.SlwId == id);
            if (existingSlw == null)
            {
                return NotFound();
            }

            // Tworzymy kopię istniejącego rekordu, aby zastosować zmiany
            var modifiedSlw = new Slowniki
            {
                SlwId = existingSlw.SlwId,
                SlwTyp = existingSlw.SlwTyp,
                SlwWartoscS = existingSlw.SlwWartoscS,
                SlwWartoscD = existingSlw.SlwWartoscD,
                SlwDataUtw = existingSlw.SlwDataUtw,
                SlwDataMod = DateTime.Now,
                SlwOpeMod = OpeId,
                SlwNextSlw = existingSlw.SlwNextSlw,
                OpeNazwa = existingSlw.OpeNazwa,
                SlwOpis = existingSlw.SlwOpis
            };

            // Aplikujemy zmiany na kopię rekordu
            slw.ApplyTo(modifiedSlw, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Aktualizujemy istniejący rekord na podstawie zmodyfikowanego
            existingSlw.SlwId = modifiedSlw.SlwId;
            existingSlw.SlwTyp = modifiedSlw.SlwTyp;
            existingSlw.SlwWartoscS = modifiedSlw.SlwWartoscS;
            existingSlw.SlwWartoscD = modifiedSlw.SlwWartoscD;
            existingSlw.SlwDataUtw = modifiedSlw.SlwDataUtw;
            existingSlw.SlwOpeMod = modifiedSlw.SlwOpeMod;
            existingSlw.SlwDataMod = modifiedSlw.SlwDataMod;
            existingSlw.SlwNextSlw = modifiedSlw.SlwNextSlw;
            existingSlw.OpeNazwa = modifiedSlw.OpeNazwa;
            existingSlw.SlwOpis = modifiedSlw.SlwOpis;


            context.Entry(existingSlw).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Slownikis.Any(c => c.SlwId == id))
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
    }
}