using System;
using System.Collections.Generic;

namespace TomatisCRM_API.Models.Entities;

public partial class Zadanium
{
    public int ZadId { get; set; }

    public string? ZadNazwa { get; set; }

    public string? ZadOpis { get; set; }

    public DateTime? ZadDataUkonczenia { get; set; }

    public int? ZadOpeid { get; set; }
}
