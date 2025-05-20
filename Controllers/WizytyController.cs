using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TomatisCRM_API.Models;
using TomatisCRM_API.Models.Entities;

namespace TomatisCRM_API.Controllers
{
    public class WizytyController: ControllerBase
    {
        private readonly TestAppTomatisCrmContext context;

        public WizytyController(TestAppTomatisCrmContext context)
        {
            this.context = context;
        }

        private async Task<List<Wizyty>> WizytyToList(int page = 0, int maxPerPage = 0, int DefYear = -1,int DefFromMonth = 1, int DefToMonth = 12)
        {
            if (DefYear == -1) DefYear = DateTime.Now.Year;
            var wiz = new List<Wizyty>();
            if (page == 0 && maxPerPage == 0)
            {
                 wiz = await this.context.Wizyties
                                        .Include(s => s.WizOpeUtwNavigation)
                                        .Include(s => s.WizTypNavigation)
                                        .Include(s => s.WizKnt)
                                        .Select(s => new Wizyty
                                        {
                                            WizId = s.WizId,
                                            WizKntid = s.WizKnt.KntId,
                                            WizKntAkronim = s.WizKnt.KntAkronim,
                                            WizKntNazwa = s.WizKnt.KntNazwa,
                                            WizKntStatus = s.WizKnt.KntStatusSlwNavigation.SlwWartoscS,
                                            WizOpis = s.WizOpis,
                                            WizDataStart = s.WizDataStart,
                                            WizDataKoniec = s.WizDataKoniec,
                                            WizDataMod = s.WizDataMod,
                                            WizTyp = s.WizTyp,
                                            WizTypSlw = s.WizTypNavigation.SlwWartoscS,
                                            WizOpeNazwa = s.WizOpeUtwNavigation.OpeNazwa,
                                            WizGooglesync = s.WizGooglesync,
                                            WizKntTel = s.WizKnt.KntTel,
                                            WizKntEmail = s.WizKnt.KntEmail
                                        })
                                        .Where(s => s.WizDataStart.Value.Year == DefYear && 
                                                    s.WizDataStart.Value.Month >= DefFromMonth && 
                                                    s.WizDataStart.Value.Month <= DefToMonth)
                                        //.OrderByDescending(s => s.WizDataStart)
                                        .ToListAsync();
            }
            else
            {
                int PagesToSkip = (page * maxPerPage) - maxPerPage;
                wiz = await this.context.Wizyties
                                        .Include(s => s.WizOpeUtwNavigation)
                                        .Include(s => s.WizTypNavigation)
                                        .Include(s => s.WizKnt)
                                        .Select(s => new Wizyty
                                        {
                                            WizId = s.WizId,
                                            WizKntid = s.WizKnt.KntId,
                                            WizKntAkronim = s.WizKnt.KntAkronim,
                                            WizKntNazwa = s.WizKnt.KntNazwa,
                                            WizKntStatus = s.WizKnt.KntStatusSlwNavigation.SlwWartoscS,
                                            WizOpis = s.WizOpis,
                                            WizDataStart = s.WizDataStart,
                                            WizDataKoniec = s.WizDataKoniec,
                                            WizDataMod = s.WizDataMod,
                                            WizTyp = s.WizTyp,
                                            WizTypSlw = s.WizTypNavigation.SlwWartoscS,
                                            WizOpeNazwa = s.WizOpeUtwNavigation.OpeNazwa,
                                            WizGooglesync = s.WizGooglesync,
                                            WizKntTel = s.WizKnt.KntTel,
                                            WizKntEmail = s.WizKnt.KntEmail
                                        })
                                        //.OrderByDescending(s => s.WizDataStart)
                                        .Where(s => s.WizDataStart.Value.Year == DefYear &&
                                                    s.WizDataStart.Value.Month >= DefFromMonth &&
                                                    s.WizDataStart.Value.Month <= DefToMonth)
                                        .Skip(PagesToSkip)
                                        .Take(maxPerPage)
                                        .ToListAsync();
            }
            return wiz;
        }

        /* Pobieranie następne daty następnej wizyty jeżeli istnieje - może być problem */
        private async Task<DateTime?> GetNextWizAsync(int knt_id, DateTime WizDataStart)
        {
            var wizytyList = WizytyToList(DefYear: DateTime.Now.Year).Result;
            var data = wizytyList.FirstOrDefault(s => s.WizKntid == knt_id && s.WizDataStart > WizDataStart);
            return data?.WizDataStart;
        }

        private async Task<List<WizytyModel>> WizytyToModel(int FromYear = -1,int FromMonth = 1, int ToMonth=12, int page = 0,int maxPerPage = 0)
        {
            var wizResult = WizytyToList(page,maxPerPage, FromYear, FromMonth).Result;

            var wizytymod = new List<WizytyModel>();

            foreach (var wizyta in wizResult)
            {
                var nextDate = GetNextWizAsync(wizyta.WizKntid.Value, wizyta.WizDataStart.Value).Result;

                wizytymod.Add(new WizytyModel
                {
                    WizId = wizyta.WizId,
                    WizKntId = wizyta.WizKntid.Value,
                    WizKntAkronim = wizyta.WizKntAkronim,
                    WizNazwa = wizyta.WizKntNazwa,
                    WizKntStatus = wizyta.WizKntStatus,
                    WizOpis = wizyta.WizOpis,
                    WizTyp = wizyta.WizTyp.Value,
                    WizTypSlw = wizyta.WizTypSlw,
                    WizModDate = wizyta.WizDataMod,
                    WizOpeNazwa = wizyta.WizOpeNazwa,
                    WizDataStart = wizyta.WizDataStart,
                    WizDataKoniec = wizyta.WizDataKoniec,
                    WizGooglesync = wizyta.WizGooglesync ?? false,
                    WizEmail = wizyta.WizKntEmail,
                    WizTel = wizyta.WizKntTel,
                    WizNextDate = nextDate ?? null

                });
            }

            return wizytymod;
        }

        public async Task<ActionResult<List<AppConf>>> AppConfToList()
        {
            return await this.context.AppConfs.ToListAsync();
        }

        /* Sprawdzanie czy wybrany termin nie koliduje z istniejącymi wizytami */
        private bool IsDatePosible(DateTime dateTime)
        {
            if(dateTime == null) return false;
            var wizytyModels = WizytyToModel(DateTime.Now.Year, DateTime.Now.Month).Result;
            var appConfig = AppConfToList().Result.Value;
            int ilosc_wiz = wizytyModels.Where(q => q.WizDataStart.Value.Date == dateTime).Count();
            if (dateTime < DateTime.Now) return false;
            if (wizytyModels.Where(q => q.WizDataStart.Value == dateTime).Count() > 0) return false; // więcej niż jedna wizyta na tą samą godzinę
            if(dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                if (appConfig.Where(q => q.SconfId == 5).Select(s => s.SconfWartoscS).ToList()[0] == "NIE") return false;
            }
            int ilosc_dni_conf = 0;
            var conf_wynik = appConfig.Where(q => q.SconfId == 3).Select(s => s.SconfWartoscD).ToList()[0];
            if (conf_wynik != null) ilosc_dni_conf = (int)conf_wynik;
            if (ilosc_wiz > ilosc_dni_conf) return false;
            return true;
        }

        /* Wybieranie następnego dostępnego terminu wizyty + dni z konfiguracji */
        private DateTime NextPosibleData(DateTime dateTime)
        {
            var nastepna_data = dateTime;
            var appConfig = AppConfToList().Result.Value;

            int godz_start = appConfig.Where(q => q.SconfId == 1).Select(s => s.SconfWartoscDt).ToList()[0].Value.Hour;
            int min_start = appConfig.Where(q => q.SconfId == 1).Select(s => s.SconfWartoscDt).ToList()[0].Value.Minute;

            int godz_koniec = appConfig.Where(q => q.SconfId == 2).Select(s => s.SconfWartoscDt).ToList()[0].Value.Hour;
            int min_koniec = appConfig.Where(q => q.SconfId == 2).Select(s => s.SconfWartoscDt).ToList()[0].Value.Minute;

            double min_dodanie = (double)appConfig.Where(q => q.SconfId == 6).Select(s => s.SconfWartoscD).ToList()[0];
            var ilocsdni = appConfig.Where(q => q.SconfId == 4).Select(s => s.SconfWartoscD).ToList()[0];

            nastepna_data = nastepna_data.Date + new TimeSpan(godz_start, min_start, 0);
            while (!IsDatePosible(nastepna_data))
            {
                if (nastepna_data.Hour < godz_koniec && nastepna_data.Minute < min_koniec)
                {
                    nastepna_data = nastepna_data.AddMinutes(min_dodanie);
                }
                else
                {
                    nastepna_data = nastepna_data.Date + new TimeSpan(godz_start, min_start, 0);
                    nastepna_data = nastepna_data.AddDays(1);
                }
            }
            return nastepna_data;
        }

        private async Task<int> UpdateKntStatus(int kntId, int SlwId)
        {
            var klient = await this.context.Klients.FirstOrDefaultAsync(k => k.KntId == kntId);
            if (klient == null)
            {
                return 1;
            }
            klient.KntStatusSlw = SlwId;
            context.Klients.Entry(klient).Property(k => k.KntStatusSlw).IsModified = true;

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return 2;
            }

            return 0;
        }

        [HttpGet]
        /* Lista wszystkich wizyt - (Dodanie stronicowania) */
        [Route("/api/wizyty")]
        public async Task<ActionResult<List<WizytyModel>>> GetWizytyAsync()
        {
            var wizytyModel = await WizytyToModel(DateTime.Now.Year); // Awaiting the async method
            return Ok(wizytyModel);
        }

        [HttpGet]
        /* Lista wszystkich wizyt - (Dodanie stronicowania) */
        [Route("/api/wizyty/{page};{maxPerPage};{year};{FromMonth};{ToMonth}")]
        public async Task<ActionResult<List<WizytyModel>>> GetWizytyPageAsync(int page, int maxPerPage,int year,int FromMonth,int ToMonth)
        {
            var wizytyModel = await WizytyToModel(year, FromMonth, ToMonth,page, maxPerPage);
            return Ok(wizytyModel);
        }

        [HttpGet]
        /* Dane wybranej wizyty */
        [Route("/api/wizyty/{wizId}")]
        public async Task<ActionResult<List<WizytyModel>>> GetWizyta(int wizId)
        {
            var wizytyModel = await WizytyToModel();
            var result = wizytyModel.Where(q => q.WizId == wizId).ToList();

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpGet]
        /* Dane wybranej wizyty */
        [Route("/api/wizyty/count/{FromYear}/{FromMonth}/{ToMonth}")]
        public async Task<ActionResult<int>> GetCountWizyta(int FromYear = -1, int FromMonth = 1, int ToMonth = 12)
        {
            var wizytyModel = await WizytyToModel(FromYear,FromMonth,ToMonth);
            var result = wizytyModel.Count();
            return result;
        }

        [HttpGet]
        /* Dane wybranej wizyty po ID Klienta*/
        [Route("/api/wizyty/knt/{KntId}")]
        public async Task<ActionResult<List<WizytyModel>>> GetWizytaByKlient(int KntId)
        {
            var klientModels = WizytyToModel().Result;
            var result = klientModels.Where(q => q.WizKntId == KntId).OrderByDescending(q => q.WizDataStart).ToList();

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        public async Task<ActionResult<int>> GetLastWizID(int KntId,int TypWiz)
        {
            if (TypWiz == 1) return 0;

            var klientModels = WizytyToModel().Result;
            var result = klientModels.Where(q => q.WizKntId == KntId).Last();

            if (result == null)
            {
                return 0;
            }

            return result.WizId;
        }

        [HttpGet]
        /* Wizyty umówione na dziś */
        [Route("/api/wizyty/dzis")]
        public async Task<ActionResult<List<WizytyModel>>> GetTodayWizits()
        {
            var wizytyModels =  await WizytyToModel(DateTime.Now.Year,DateTime.Now.Month);
            var result = wizytyModels.Where(q => q.WizDataStart.Value.Date == DateTime.Today && q.WizDataKoniec == null).ToList();

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpGet]
        /* Lista niezakończonych wizyt -- Możliwe że do wyrzucenia */
        [Route("/api/wizyty/niezakonczone")]
        public async Task<ActionResult<List<WizytyModel>>> GetNotEndedWiz()
        {
            //var wizyty = await WizytyToList();
            var klientModels =  await WizytyToModel(DateTime.Now.Year);
            var result = klientModels.Where(q => q.WizDataKoniec == null && q.WizDataStart.Value.Date < DateTime.Now).ToList();

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpGet]
        /* Mozliwa następna data wizyty po zakończeniu */
        [Route("/api/wizyty/datanastepnejwizyty/{kntId}")]
        public async Task<ActionResult<DateTime>> GetNexWizDate(int kntId)
        {
            var wizModels = await WizytyToModel(FromYear: DateTime.Now.Year);
            var result = wizModels.Where(q => q.WizKntId == kntId).Last();

            if (result == null)
            {
                return NotFound("Brak wizyt");
            }
            var nastepna_data = NextPosibleData(result.WizDataKoniec.Value);
            return Ok(nastepna_data);

        }

        //[HttpGet]
        /* Mozliwa następna data wizyty po zakończeniu */
        /*[Route("/api/wizyty/nastepnyetap/{kntId}")]
        public async Task<ActionResult<DateTime>> GetNextStage(int kntId)
        {
            var wizModels = await WizytyToModel(FromYear: DateTime.Now.Year);
            var result = wizModels.Where(q => q.WizKntId == kntId).Last();

            if (result == null)
            {
                return NotFound("Brak wizyt");
            }
            return Ok(nastepny_etap);

        }*/

        [HttpGet]
        [Route("/api/wizyty/DoKontaktu")]

        /* Lista Klientów do kontaktu po upłynięciu określonego czasu w konfiguracji od daty rozpoczęcia wizyty */
        public async Task<ActionResult<DoKontaktu>> GetConcatFromWiz()
        {
            var wizModels = await WizytyToModel(DateTime.Now.Year);
            var wiz = wizModels.Where(q => NextPosibleData(q.WizDataStart.Value) >= DateTime.Now.Date && q.WizDataKoniec == null && q.WizKntStatus != "Zakończony").ToList();
            if (wiz == null)
            {
                NotFound();
            }

            var kontakt = new List<DoKontaktu>();

            foreach(var dokwiz in wiz)
            {
                kontakt.Add(new DoKontaktu
                {
                    KontaktData = dokwiz.WizDataStart,
                    KontaktEmail = dokwiz.WizEmail,
                    KontaktTel = dokwiz.WizTel,
                    KontaktNazwa = dokwiz.WizNazwa,
                    KontaktTyp = dokwiz.WizTypSlw
                });
            }

            var knt = this.context.Klients
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
                                   .Where(s => s.KntDataKontaktu >= DateTime.Now && s.KntDoKontaktu == true)
                                   .ToListAsync().Result;


            foreach (var dokknt in knt)
            {
                kontakt.Add(new DoKontaktu
                {
                    KontaktData = dokknt.KntDataKontaktu,
                    KontaktEmail = dokknt.KntEmail,
                    KontaktTel = dokknt.KntTel,
                    KontaktNazwa = dokknt.KntAkronim,
                    KontaktTyp = "Przypomnienie kontaktu"
                });
            }
            return Ok(kontakt);
        }

        [HttpPost]
        [Route("/api/wizyty/add")]
        public async Task<ActionResult<Wizyty>> PostWizyty(Wizyty wiz)
        {
            if (wiz == null)
            {
                return BadRequest("Przesłany pusty formularz");
            }
            /*if(!IsDatePosible(wiz.WizDataStart.Value))
            {
                return BadRequest("Nieprawidłowa data lub godzina rozpoczęcia wizyty");
            }*/
            if(wiz.WizKntid == null)
            {
                return BadRequest("Nie podano klienta");
            }
            if(wiz.WizTyp == null)
            {
                return BadRequest("Nieprawidłowy etap");
            }
            if(wiz.WizOpeUtw == null)
            {
                return BadRequest("Bład operatora");
            }
            wiz.WizPoprzedniawizId = GetLastWizID(wiz.WizKntid.Value, wiz.WizTyp.Value).Result.Value;

            //Dodać aktualizację statusu Klienta po przejściu na etap E1 - 4 status powinien być zmieniony na Aktywny - 8
            if (wiz.WizTyp == 4)
            {
                switch (UpdateKntStatus(wiz.WizKntid.Value, 8).Result)
                {
                    case 1: return NotFound("Nie znaleziono klienta");
                    case 2: return BadRequest("Wystąpił problem podczas aktualizacji statusu klienta.");
                }
            }

            context.Wizyties.Add(wiz);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("/api/wizyty/delete/{id}")]
        public async Task<ActionResult> DeleteWizyty(int id)
        {
            var wiz = await this.context.Wizyties.FindAsync(id);

            if (wiz == null)
            {
                return NotFound();
            }

            this.context.Wizyties.Remove(wiz);
            await this.context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [Route("/api/wizyty/update/{id}")]
        public async Task<ActionResult<Wizyty>> PutWizyta(int id, Wizyty wiz)
        {
            var wizDataStart = await context.Wizyties.Where(c => c.WizId == id).Select(c => c.WizDataStart).FirstOrDefaultAsync();
            if (wizDataStart == null)
            {
                return NotFound();
            }

            /*if (!IsDatePosible(wiz.WizDataStart.Value) && wizDataStart.Value != wiz.WizDataStart.Value)
            {
                return BadRequest("Nieprawidłowa data lub godzina.");
            }*/
            if (wiz.WizKntid == null)
            {
                return BadRequest("Nie podano klienta");
            }
            if (wiz.WizTyp == null)
            {
                return BadRequest("Nieprawidłowy etap");
            }
            if (wiz.WizOpeUtw == null)
            {
                var wiz_ope_id = context.Operatorzies.Where(q => q.OpeNazwa == wiz.WizOpeNazwa).Select(s => s.OpeId).First();
                if(wiz_ope_id == null)
                {
                    return BadRequest("Brak operatora");
                }
                wiz.WizOpeUtw = wiz_ope_id;
            }

            if(wiz.WizDataKoniec != null && wiz.WizTyp == 1 && wiz.WizKntStatus != "Aktywny")
            {
                switch (UpdateKntStatus(wiz.WizKntid.Value, 9).Result)
                {
                    case 1: return NotFound();
                    case 2: return BadRequest("Wystąpił problem podczas aktualizacji statusu klienta.");
                }
            }

            context.Entry(wiz).State = EntityState.Modified;


            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.context.Wizyties.Where(c => c.WizId == id).FirstOrDefaultAsync() == null)
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
        [Route("/api/wizyty/patch/{id}/{OpeId}")]
        public async Task<ActionResult<Wizyty>> PatchWizyta(int id, int OpeId, [FromBody] JsonPatchDocument<Wizyty> wiz)
        {

            if (wiz == null)
            {
                return BadRequest("Brak dokumentu patch.");
            }

            var existingWizyta = await context.Wizyties.FirstOrDefaultAsync(c => c.WizId == id);
            if (existingWizyta == null)
            {
                return NotFound();
            }

            // Tworzymy kopię istniejącego rekordu, aby zastosować zmiany
            var modifiedWizyta = new Wizyty
            {
                WizId = existingWizyta.WizId,
                WizDataStart = existingWizyta.WizDataStart,
                WizDataKoniec = existingWizyta.WizDataKoniec,
                WizDataMod = DateTime.Now,
                WizOpeMod = OpeId,
                WizKntid = existingWizyta.WizKntid,
                WizTyp = existingWizyta.WizTyp,
                WizKntStatus = existingWizyta.WizKntStatus,
                WizOpeUtw = existingWizyta.WizOpeUtw,
                WizOpeNazwa = existingWizyta.WizOpeNazwa,
                WizOpis = existingWizyta.WizOpis,
                WizGooglesync = false
            };

            // Aplikujemy zmiany na kopię rekordu
            wiz.ApplyTo(modifiedWizyta, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            /*// Walidacja po zastosowaniu patcha
            if (!IsDatePosible(modifiedWizyta.WizDataStart.Value) && existingWizyta.WizDataStart != modifiedWizyta.WizDataStart)
            {
                return BadRequest("Nieprawidłowa data lub godzina.");
            }

            if (modifiedWizyta.WizKntid == null)
            {
                return BadRequest("Brak Klienta");
            }

            if (modifiedWizyta.WizTyp == null)
            {
                return BadRequest("Brak etapu");
            }*/

            if (modifiedWizyta.WizOpeUtw == null)
            {
                var wizOpeId = context.Operatorzies
                    .Where(q => q.OpeNazwa == modifiedWizyta.WizOpeNazwa)
                    .Select(s => s.OpeId)
                    .FirstOrDefault();

                if (wizOpeId == null)
                {
                    return BadRequest("Brak operatora");
                }

                modifiedWizyta.WizOpeUtw = wizOpeId;
            }

            if (modifiedWizyta.WizDataKoniec != null && modifiedWizyta.WizTyp == 1 && modifiedWizyta.WizKntStatus != "Aktywny")
            {
                switch (await UpdateKntStatus(modifiedWizyta.WizKntid.Value, 9))
                {
                    case 1: return NotFound();
                    case 2: return BadRequest("Wystąpił problem podczas aktualizacji statusu klienta.");
                }
            }

            // Aktualizujemy istniejący rekord na podstawie zmodyfikowanego
            existingWizyta.WizDataStart = modifiedWizyta.WizDataStart;
            existingWizyta.WizDataKoniec = modifiedWizyta.WizDataKoniec;
            existingWizyta.WizKntid = modifiedWizyta.WizKntid;
            existingWizyta.WizDataMod = modifiedWizyta.WizDataMod;
            existingWizyta.WizTyp = modifiedWizyta.WizTyp;
            existingWizyta.WizOpeMod = modifiedWizyta.WizOpeMod;
            existingWizyta.WizKntStatus = modifiedWizyta.WizKntStatus;
            existingWizyta.WizOpeUtw = modifiedWizyta.WizOpeUtw;
            existingWizyta.WizOpeNazwa = modifiedWizyta.WizOpeNazwa;
            existingWizyta.WizOpis = modifiedWizyta.WizOpis;
            existingWizyta.WizGooglesync = modifiedWizyta.WizGooglesync;


            context.Entry(existingWizyta).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Wizyties.Any(c => c.WizId == id))
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
