using System;
using System.Collections.Generic;

namespace TomatisCRM_API.Models.Entities;

public partial class Wizyty
{
    public int WizId { get; set; }

    public int? WizTyp { get; set; }

    public int? WizKntid { get; set; }

    public DateTime? WizDataStart { get; set; }

    public DateTime? WizDataKoniec { get; set; }

    public int? WizOpeUtw { get; set; }

    public DateTime? WizDataUtw { get; set; } = DateTime.Now;

    public int? WizOpeMod { get; set; }

    public DateTime? WizDataMod { get; set; }

    public string? WizOpis { get; set; }

    public bool? WizGooglesync { get; set; } = false;

    public int? WizPoprzedniawizId { get; set; }

    public string? WizKntNazwa { get; set; }

    public string? WizOpeNazwa { get; set; }

    public string? WizTypSlw { get; set; }
    public string? WizKntTel { get; set; }
    public string? WizKntEmail { get; set; }

    public virtual Klient? WizKnt { get; set; }

    public virtual Operatorzy? WizOpeModNavigation { get; set; }

    public virtual Operatorzy? WizOpeUtwNavigation { get; set; }

    public virtual Slowniki? WizTypNavigation { get; set; }
}
