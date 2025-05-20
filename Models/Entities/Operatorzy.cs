using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TomatisCRM_API.Models.Entities;

public partial class Operatorzy
{
    public int OpeId { get; set; }
    [JsonPropertyName("OpeLogin")]
    public string OpeLogin { get; set; } = null!;
    [JsonPropertyName("OpeHaslo")]
    public string OpeHaslo { get; set; } = null!;
    [JsonPropertyName("OpeNazwa")]
    public string OpeNazwa { get; set; } = null!;
    [JsonPropertyName("OpeEmail")]
    public string? OpeEmail { get; set; }
    [JsonPropertyName("OpeGoogleapikey")]
    public string? OpeGoogleapikey { get; set; }
    [JsonPropertyName("OpeDataUtw")]
    public DateTime? OpeDataUtw { get; set; } = DateTime.Now;
    [JsonPropertyName("OpeIsAdmin")]
    public bool? OpeIsAdmin { get; set; }

    [JsonIgnore]
    public virtual ICollection<Klient> KlientKntOpeModNavigations { get; set; } = new List<Klient>();

    [JsonIgnore]
    public virtual ICollection<Klient> KlientKntOpeUtwNavigations { get; set; } = new List<Klient>();

    [JsonIgnore]
    public virtual ICollection<Slowniki> Slownikis { get; set; } = new List<Slowniki>();

    [JsonIgnore]
    public virtual ICollection<Wizyty> WizytyWizOpeModNavigations { get; set; } = new List<Wizyty>();

    [JsonIgnore]
    public virtual ICollection<Wizyty> WizytyWizOpeUtwNavigations { get; set; } = new List<Wizyty>();
}
