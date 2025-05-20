using System;
using System.Collections.Generic;

namespace TomatisCRM_API.Models.Entities;

public partial class Operatorzy
{
    public int OpeId { get; set; }

    public string OpeLogin { get; set; } = null!;

    public string OpeHaslo { get; set; } = null!;

    public string OpeNazwa { get; set; } = null!;

    public string? OpeEmail { get; set; }

    public string? OpeGoogleapikey { get; set; }
    public bool? OpeIsAdmin { get; set; }

    public DateTime? OpeDataUtw { get; set; }

    public virtual ICollection<Klient> KlientKntOpeModNavigations { get; set; } = new List<Klient>();

    public virtual ICollection<Klient> KlientKntOpeUtwNavigations { get; set; } = new List<Klient>();

    public virtual ICollection<Slowniki> Slownikis { get; set; } = new List<Slowniki>();

    public virtual ICollection<Wizyty> WizytyWizOpeModNavigations { get; set; } = new List<Wizyty>();

    public virtual ICollection<Wizyty> WizytyWizOpeUtwNavigations { get; set; } = new List<Wizyty>();
}
