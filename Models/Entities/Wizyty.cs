using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TomatisCRM_API.Entities;

namespace TomatisCRM_API.Models.Entities;

public partial class Wizyty
{
    public int WizId { get; set; }

    [JsonPropertyName("WizTyp")]
    public int? WizTyp { get; set; }
    [JsonPropertyName("WizKntid")]
    public int? WizKntid { get; set; }
    [JsonPropertyName("WizDataStart")]
    public DateTime? WizDataStart { get; set; }
    [JsonPropertyName("WizDataKoniec")]
    public DateTime? WizDataKoniec { get; set; }
    [JsonPropertyName("WizOpeUtw")]
    public int? WizOpeUtw { get; set; }
    [JsonPropertyName("WizDataUtw")]
    public DateTime? WizDataUtw { get; set; } = DateTime.Now;
    [JsonPropertyName("WizOpeMod")]
    public int? WizOpeMod { get; set; }
    [JsonPropertyName("WizDataMod")]
    public DateTime? WizDataMod { get; set; }
    [JsonPropertyName("WizOpis")]
    public string? WizOpis { get; set; }
    [JsonPropertyName("WizGooglesync")]
    public bool? WizGooglesync { get; set; } = false;
    [JsonPropertyName("WizPoprzedniawizId")]
    public int? WizPoprzedniawizId { get; set; } = 0;
    [JsonPropertyName("WizKntAkronim")]
    public string? WizKntAkronim { get; set; }
    [JsonPropertyName("WizKntNazwa")]
    public string? WizKntNazwa { get; set; }
    [JsonPropertyName("WizOpeNazwa")]
    public string? WizOpeNazwa { get; set; }

    public string? WizTypSlw { get; set; }
    public string? WizKntTel { get; set; }
    public string? WizKntEmail { get; set; }

    public string? WizKntStatus { get; set; }

    public int? WizNextEtapId { get; set; }

    public string? WizNextEtap { get; set; }

    [JsonIgnore]
    public virtual Klient? WizKnt { get; set; }

    [JsonIgnore]
    public virtual Operatorzy? WizOpeModNavigation { get; set; }
    [JsonIgnore]
    public virtual Operatorzy? WizOpeUtwNavigation { get; set; }
    [JsonIgnore]
    public virtual Slowniki? WizTypNavigation { get; set; }

    [JsonIgnore]
    public virtual ICollection<Pliki> Plikis { get; set; } = new List<Pliki>();
}
