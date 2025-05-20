using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TomatisCRM_API.Models;
using TomatisCRM_API.Models.Entities;
using TomatisCRM_API.Service;

namespace TomatisCRM_API.Controllers
{
    public class KlientController : ControllerBase
    {
        private readonly TestAppTomatisCrmContext context;

        public KlientController(TestAppTomatisCrmContext context)
        {
            this.context = context;
        }
        public async Task<ActionResult<List<Klient>>> KlientToList(int page = 0,int maxPerPage = 0)
        {

            if (page == 0 || maxPerPage == 0)
            {
                return await this.context.Klients
                                   .Include(S => S.KntOpeUtwNavigation)
                                   .Include(S => S.KntStatusSlwNavigation)
                                   .Select(s => new Klient
                                   {
                                       KntId = s.KntId,
                                       KntAkronim = s.KntAkronim,
                                       KntNazwa = s.KntNazwa,
                                       KntTel = s.KntTel,
                                       KntEmail = s.KntEmail,
                                       KntOpeUNazwa = s.KntOpeUtwNavigation.OpeNazwa,
                                       KntDataUtw = s.KntDataUtw,
                                       KntDataUrodzenia = s.KntDataUrodzenia,
                                       KntStatus = s.KntStatusSlwNavigation.SlwWartoscS,
                                       KntStacjonarny = s.KntStacjonarny,
                                       KntOpis = s.KntOpis,
                                       KntDoKontaktu = s.KntDoKontaktu,
                                       KntDataKontaktu = s.KntDataKontaktu
                                   })
                                   .ToListAsync();
            }
            else
            {
                int PagesToSkip = (page * maxPerPage) - maxPerPage;
                return await this.context.Klients
                                   .Include(S => S.KntOpeUtwNavigation)
                                   .Include(S => S.KntOpeUtwNavigation)
                                   .Select(s => new Klient
                                   {
                                       KntId = s.KntId,
                                       KntAkronim = s.KntAkronim,
                                       KntNazwa = s.KntNazwa,
                                       KntTel = s.KntTel,
                                       KntEmail = s.KntEmail,
                                       KntOpeUNazwa = s.KntOpeUtwNavigation.OpeNazwa,
                                       KntDataUtw = s.KntDataUtw,
                                       KntDataUrodzenia = s.KntDataUrodzenia,
                                       KntStatus = s.KntStatusSlwNavigation.SlwWartoscS,
                                       KntStacjonarny = s.KntStacjonarny,
                                       KntOpis = s.KntOpis,
                                       KntDoKontaktu = s.KntDoKontaktu,
                                       KntDataKontaktu = s.KntDataKontaktu
                                   })
                                   .Skip(PagesToSkip)
                                   .Take(maxPerPage)
                                   .ToListAsync();
            }
        }

        public string GetNextEtap(int SlwNextEtap)
        {
            var etap = context.Slownikis.Where(s => s.SlwId == SlwNextEtap)
                .Select(s => s.SlwWartoscS).First();
            if (etap == null) return "";
            return etap;
        }

        private async Task<List<KlientModel>> KlientToModel(List<Klient> knt)
        {
            var kntmod = new List<KlientModel>();

            await Task.Run(() =>
            {
                foreach (Klient klient in knt)
                {
                    var next_etap = "";
                    try
                    {
                        var last_wiz_typ = context.Wizyties.Where(s => s.WizKntid == klient.KntId).ToList().Last().WizTyp;
                        if (last_wiz_typ.HasValue)
                        {
                            last_wiz_typ = context.Slownikis.Where(s => s.SlwId == last_wiz_typ.Value).Select(s => s.SlwNextSlw).First().Value;

                            next_etap = GetNextEtap(last_wiz_typ.Value);
                        }
                    }
                    catch {
                        next_etap = "B1";
                    }

                    var grupa_wiekowa = "";
                    if (klient.KntDataUrodzenia == null) grupa_wiekowa = "Nieznana";
                    else
                    {
                        if (DateTime.Now.Year - klient.KntDataUrodzenia.Value.Year >= 18) grupa_wiekowa = "Dorosły";
                        else grupa_wiekowa = "Dziecko";
                    }
                    kntmod.Add(new KlientModel
                    {
                        KntId = klient.KntId,
                        KntAkronim = klient.KntAkronim,
                        KntNazwa = klient.KntNazwa,
                        KntTel = klient.KntTel,
                        KntEmail = klient.KntEmail,
                        KntOpeUNazwa = klient.KntOpeUNazwa,
                        KntDataUtw = klient.KntDataUtw,
                        KntDataUrodzenia = klient.KntDataUrodzenia,
                        KntStatus = klient.KntStatus,
                        KntStacjonarny = klient.KntStacjonarny,
                        KntOpis = klient.KntOpis,
                        KntDoKontaktu = klient.KntDoKontaktu,
                        KntDataKontaktu = klient.KntDataKontaktu,
                        KntGrupaWiekowa = grupa_wiekowa,
                        KntNextEtap = next_etap ?? ""
                    });
                }
            });
            return kntmod;
        }

        public bool UpdateKnt(int kntid,int status)
        {
            var knt = KlientToList().Result.Value.Where(s => s.KntId == kntid).FirstOrDefault();
            knt.KntStatusSlw = status;
            this.context.Entry(knt).State = EntityState.Modified;

            try
            {
                this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (this.context.Klients.Where(c => c.KntId == kntid).FirstOrDefaultAsync() == null)
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            return true;
        }

        [HttpGet]
        //[EnableCors("AllowAll")]
        [Route("/api/klient")]
        public async Task<ActionResult<List<KlientModel>>> GetKlients()
        {
            var klienci = await KlientToList();
            var klientModels = await KlientToModel(klienci.Value);

            return Ok(klientModels);
        }

        [HttpGet]
        //[EnableCors("AllowAll")]
        [Route("/api/klient/{page};{maxPerPage}")]
        public async Task<ActionResult<List<KlientModel>>> GetKlientsPage(int page, int maxPerPage)
        {
            var klienci = await KlientToList(page,maxPerPage);
            var klientModels = await KlientToModel(klienci.Value);

            return Ok(klientModels);
        }

        [HttpGet]
        //[EnableCors("AllowAll")]
        [Route("/api/klient/{kntId}")]
        public async Task<ActionResult<List<KlientModel>>> GetKlient(int kntId)
        {
            var klienci = await KlientToList();
            var klientModels = await KlientToModel(klienci.Value);

            var wynik = klientModels.Where(s => s.KntId == kntId).ToList();

            if (wynik == null || !wynik.Any())
            {
                return NotFound();
            }
            return Ok(wynik);
        }

        [HttpGet]
        [Route("/api/klient/count")]
        public async Task<ActionResult<int>> GetCountKlient()
        {
            var knt = await context.Klients.ToListAsync();
            var result = knt.Count();
            return result;
        }

        [HttpPost]
        //[EnableCors("AllowAll")]
        [Route("/api/klient/add")]
        public async Task<ActionResult<Klient>> PostKlient(Klient knt)
        {
            if (knt == null)
            {
                return NotFound("Invalid data.");
            }
            context.Klients.Add(knt);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        //[EnableCors("AllowAll")]
        [Route("/api/klient/update/{id}")]
        public async Task<ActionResult<Klient>> PutKlient(int id, Klient knt)
        {

            this.context.Entry(knt).State = EntityState.Modified;
            knt.KntStatusSlw = context.Slownikis.Where(s => s.SlwWartoscS == knt.KntStatus).Select(s => s.SlwId).First();
            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.context.Klients.Where(c => c.KntId == id).FirstOrDefaultAsync() == null)
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
        [Route("/api/klient/patch/{id}")]
        public async Task<ActionResult<Klient>> PatchKnt(int id, [FromBody] JsonPatchDocument<Klient> knt)
        {

            if (knt == null)
            {
                return BadRequest("Invalid patch document");
            }

            var knt_c = this.context.Klients.FirstOrDefault(u => u.KntId == id);
            
            if (knt_c == null)
            {
                return NotFound();
            }

            this.context.Entry(knt_c).Property(p => p.KntAkronim).IsModified = true;
            
            try
            {
                knt.ApplyTo(knt_c);

                await this.context.SaveChangesAsync();
                return Ok(knt_c);
            }
            catch (Exception ex)
            {
                // Log the error details
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        //[EnableCors("AllowAll")]
        [Route("/api/klient/delete/{id}")]
        public async Task<ActionResult> DeleteKlient(int id)
        {
            var ope = await this.context.Klients.FindAsync(id);

            if (ope == null)
            {
                return NotFound();
            }

            this.context.Klients.Remove(ope);
            await this.context.SaveChangesAsync();

            return Ok();
        }
    }
}
