using System;
using System.Collections.Generic;

namespace TomatisCRM_API.Models.Entities;

public partial class Klient
{
    public int KntId { get; set; }

    public string? KntNazwa { get; set; }

    public string? KntTel { get; set; }

    public string? KntEmail { get; set; }

    public DateTime? KntDataUtw { get; set; } = DateTime.Now;

    public int? KntOpeUtw { get; set; }

    public int? KntOpeMod { get; set; }
    public string? KntOpeUNazwa { get; set; }

    public DateTime? KntDataMod { get; set; }

    public virtual Operatorzy? KntOpeModNavigation { get; set; }

    public virtual Operatorzy? KntOpeUtwNavigation { get; set; }

    public virtual ICollection<Wizyty> Wizyties { get; set; } = new List<Wizyty>();
}
