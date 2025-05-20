using System;
using System.Collections.Generic;

namespace TomatisCRM_API.Models.Entities;

public partial class Slowniki
{
    public int SlwId { get; set; }

    public int? SlwTyp { get; set; }

    public string? SlwWartoscS { get; set; }

    public decimal? SlwWartoscD { get; set; }

    public int? SlwOpeUtw { get; set; }

    public DateTime? SlwDataUtw { get; set; }

    public int? SlwOpeMod { get; set; }

    public DateTime? SlwDataMod { get; set; }
    public string OpeNazwa { get; set; }

    public virtual Operatorzy? SlwOpeUtwNavigation { get; set; }

    public virtual ICollection<Wizyty> Wizyties { get; set; } = new List<Wizyty>();
}
