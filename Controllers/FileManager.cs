using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using TomatisCRM_API.Entities;
using TomatisCRM_API.Models.Entities;

namespace TomatisCRM_API.Controllers
{
    public class FileManager : ControllerBase
    {
        private readonly TestAppTomatisCrmContext context;
        private readonly String FilePath = Directory.GetCurrentDirectory();

        public FileManager(TestAppTomatisCrmContext context)
        {
            this.context = context;
        }

        public bool UploadFile(String FileName, String FileGUID, String KntName, int StageId, String uploadPath, String uploadFullPath)
        {
            var FileModel = new Pliki
            {

                FileKntId = context.Klients.Where(q => q.KntAkronim == KntName).Select(s => s.KntId).First(),
                FileWizId = StageId,
                FileGuid = FileGUID,
                FileNazwa = FileName,
                FilePath = uploadPath,
                FileFullPath = uploadFullPath

            };
            if (FileModel == null)
            {
                return false;
            }
            context.Plikis.Add(FileModel);
            context.SaveChangesAsync();
            return true;
        }

        public bool DeleteFile(Pliki file)
        {
            if (file == null)
            {
                return false;
            }
            context.Plikis.Remove(file);
            context.SaveChanges();
            return true;
        }

        [HttpPost]
        [Route("/api/file/upload/{KntId}/{StageId}")]
        public ActionResult<String> Upload(IFormFile file,int KntId, int StageId)
        {
            if (file.FileName.Contains("..") || Path.GetInvalidFileNameChars().Any(file.FileName.Contains))
            {
                return BadRequest("Nieprawidłowa nazwa pliku");
            }

            var knt = context.Klients.Where(q => q.KntId == KntId).FirstOrDefault();
            if(knt == null)
            {
                return BadRequest("Nieprawidłowy ID klienta");
            }

            var stage = context.Wizyties.Where(q => q.WizId == StageId).Include(s => s.WizTypNavigation).Select(s => s.WizTypNavigation.SlwWartoscS).FirstOrDefault().ToString();

            List<String> valext = new List<string> {".pdf",".png",".jpg",".jpeg" };
            String ext = Path.GetExtension(file.FileName);
            if(!valext.Contains(ext))
            {
                return BadRequest("Niepoprawne rozszerzenie pliku");
            }
            String FileName = Guid.NewGuid().ToString() + ext;
            string stageFolder = StageId.ToString() + "_" + stage;
            string uploadPath = Path.Combine(FilePath, "Uploads", knt.KntAkronim, stageFolder);
            string upload_directory = Path.Combine(knt.KntAkronim, stageFolder, FileName);
            if (UploadFile(file.FileName, FileName, knt.KntAkronim, StageId, upload_directory, Path.Combine(uploadPath, FileName)))
            {
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                using FileStream stream = new FileStream(Path.Combine(uploadPath, FileName), FileMode.Create);
                file.CopyTo(stream);



                return Ok(uploadPath);
            }

            return BadRequest("Nie udało się załadować pliku");
            
        }

        [HttpGet]
        [Route("/api/file/list")]
        public ActionResult<List<Pliki>> GetFiles()
        {
            var File = context.Plikis.ToList();
            if(File == null)
            {
                return NotFound("");
            }
            return Ok(File);
        }

        [HttpGet]
        [Route("/api/file/list/{KntId}/{WizId}")]
        public ActionResult<List<Pliki>> GetFileListWiz(int KntId, int WizId)
        {
            var File = context.Plikis.Where(q => q.FileKntId == KntId && q.FileWizId == WizId).ToList();
            if (File == null)
            {
                return NotFound("");
            }
            return Ok(File);
        }

        [HttpGet]
        [Route("/api/file/list/{KntId}")]
        public ActionResult<List<Pliki>> GetFileListKnt(int KntId)
        {
            var File = context.Plikis.Where(q => q.FileKntId == KntId).ToList();
            if (File == null)
            {
                return NotFound("");
            }
            return Ok(File);
        }

        [HttpDelete]
        [Route("/api/file/delete/{FileId}")]
        public ActionResult<String> Delete(int FileId)
        {
            var File = context.Plikis.Where(q => q.FileId == FileId).FirstOrDefault();
            if (File == null)
            {
                return BadRequest("Niepoprawne ID pliku");
            }
            if (DeleteFile(File))
            {
                if (!System.IO.File.Exists(File.FileFullPath))
                {
                    return NotFound("Nie znaleziono pliku na serwerze");
                }
                 System.IO.File.Delete(File.FileFullPath);

                return Ok();
            }

            return BadRequest("Nie udało się usunąć pliku");

        }

        [HttpGet]
        [Route("/api/file/download/{FileId}")]
        public IActionResult Download(int FileId)
        {
            try { 
                var File_db = context.Plikis.Where(q => q.FileId == FileId).First();
                if (File_db == null)
                {
                    return NotFound("Nie znaleziono pliku");
                }

                if (File_db.FileNazwa.Contains("..") || Path.GetInvalidFileNameChars().Any(File_db.FileNazwa.Contains))
                {
                    return BadRequest(new { message = "Nieprawidłowa nazwa pliku" });
                }

                var fileBytes = System.IO.File.ReadAllBytes(File_db.FileFullPath);
                var fileExtension = Path.GetExtension(File_db.FileNazwa);
                var contentType = GetContentType(fileExtension);

                return File(fileBytes, contentType, File_db.FileNazwa);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Błąd podczas pobierania pliku", error = ex.Message});
            }
        }

        [HttpGet]
        [Route("/api/file/media/{FileId}")]
        public IActionResult GetMedia(int FileId)
        {
            try
            {
                var File_db = context.Plikis.Where(q => q.FileId == FileId).First();
                if (File_db == null)
                {
                    return NotFound("Nie znalziono pliku");
                }
                var file_path = File_db.FilePath;
                if (File_db.FilePath.Contains('\\'))
                {
                    file_path = file_path.Replace('\\', '/');
                }
                string fileUrl = $"{Request.Scheme}://{Request.Host}/media/{file_path}";
                return Ok(new { fileUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Błąd podczas pobierania pliku", error = ex.Message });
            }
        }


        private string GetContentType(string extension)
        {
            var types = new Dictionary<string, string>
        {
            { ".txt", "text/plain" },
            { ".pdf", "application/pdf" },
            { ".jpg", "image/jpeg" },
            { ".png", "image/png" },
            { ".doc", "application/msword" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }
        };

            return types.ContainsKey(extension) ? types[extension] : "application/octet-stream";
        }
    }
}
