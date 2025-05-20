using System;
using System.Collections.Generic;

namespace TomatisCRM_API.Models.Entities;

public partial class Klient
{
    public int KntId { get; set; }

    public string KntAkronim { get; set; } = null!;

    public string? KntNazwa { get; set; }

    public string? KntTel { get; set; }

    public string? KntEmail { get; set; }

    public DateTime? KntDataUtw { get; set; }

    public int? KntOpeUtw { get; set; }

    public int? KntOpeMod { get; set; }

    public DateTime? KntDataMod { get; set; }

    public DateOnly? KntDataUrodzenia { get; set; }

    public int? KntStatusSlw { get; set; }

    public bool? KntStacjonarny { get; set; }

    public string? KntOpis { get; set; }

    public bool? KntDoKontaktu { get; set; }

    public string? KntOpeUNazwa { get; set; }

    public DateTime? KntDataKontaktu { get; set; }

    public virtual Operatorzy? KntOpeModNavigation { get; set; }

    public virtual Operatorzy? KntOpeUtwNavigation { get; set; }

    public virtual ICollection<Wizyty> Wizyties { get; set; } = new List<Wizyty>();
}
