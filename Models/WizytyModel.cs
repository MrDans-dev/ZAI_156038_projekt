namespace TomatisCRM_API.Models.Entities
{
    public class WizytyModel
    {
        public int WizId { get; set; }
        public int WizKntId { get; set; }
        public string WizNazwa { get; set; }
        public string? WizTel { get; set; }
        public string? WizEmail { get; set; }
        public DateTime? WizDataStart { get; set; }
        public DateTime? WizNextDate { get; set; }
        public DateTime? WizModDate { get; set; }
        public DateTime? WizDataKoniec { get; set; }
        public string? WizOpis { get; set; }
        public bool? WizGooglesync { get; set; }
        public int? WizTyp { get; set; }
        public string WizTypSlw { get; set; }
        public string? WizOpeNazwa { get; set; }
        public string? WizKntAkronim { get; set; }
        public string WizKntNazwa { get; set; }
        public string WizKntTel { get; set; }
        public string WizKntEmail { get; set; }
        public string WizKntStatus { get; set; }
        public virtual Klient? WizKnt { get; set; }

        public virtual Operatorzy? WizOpeModNavigation { get; set; }

        public virtual Operatorzy? WizOpeUtwNavigation { get; set; }

        public virtual Slowniki? WizTypNavigation { get; set; }
    }
}
