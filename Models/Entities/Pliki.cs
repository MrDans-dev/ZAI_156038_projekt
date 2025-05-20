using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TomatisCRM_API.Models.Entities;

namespace TomatisCRM_API.Entities;

public partial class Pliki
{
    public int FileId { get; set; }

    public int? FileKntId { get; set; }

    public int? FileWizId { get; set; }

    public string? FileNazwa { get; set; }

    public string? FileGuid { get; set; }
    public string? FilePath { get; set; }
    public string? FileFullPath { get; set; }
    [JsonIgnore]
    public virtual Klient? FileKnt { get; set; }
    [JsonIgnore]
    public virtual Wizyty? FileWiz { get; set; }
}
