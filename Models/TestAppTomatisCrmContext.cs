using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TomatisCRM_API.Entities;

namespace TomatisCRM_API.Models.Entities;

public partial class TestAppTomatisCrmContext : DbContext
{
    public TestAppTomatisCrmContext()
    {
    }

    public TestAppTomatisCrmContext(DbContextOptions<TestAppTomatisCrmContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppConf> AppConfs { get; set; }

    public virtual DbSet<Klient> Klients { get; set; }

    public virtual DbSet<Operatorzy> Operatorzies { get; set; }

    public virtual DbSet<Slowniki> Slownikis { get; set; }
    public virtual DbSet<Pliki> Plikis { get; set; }

    public virtual DbSet<Wizyty> Wizyties { get; set; }

    public virtual DbSet<Zadanium> Zadania { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = 192.168.2.50; Initial Catalog = TEST_APP_TomatisCRM_2;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("app");

        modelBuilder.Entity<AppConf>(entity =>
        {
            entity.HasKey(e => e.SconfId).HasName("PK__SysConf__9227CE49AE830300");

            entity.ToTable("AppConf");

            entity.HasIndex(e => e.SconfNazwa, "UQ__SysConf__E92B942231ECCE06").IsUnique();

            entity.Property(e => e.SconfId).HasColumnName("sconf_id");
            entity.Property(e => e.SconfNazwa)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sconf_nazwa");
            entity.Property(e => e.SconfWartoscD)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("sconf_wartoscD");
            entity.Property(e => e.SconfWartoscDt)
                .HasColumnType("datetime")
                .HasColumnName("sconf_WartoscDt");
            entity.Property(e => e.SconfWartoscS)
                .IsUnicode(false)
                .HasColumnName("sconf_wartoscS");
        });

        modelBuilder.Entity<Klient>(entity =>
        {
            entity.HasKey(e => e.KntId).HasName("PK__Klient__id");

            entity.ToTable("Klient");

            entity.HasIndex(e => e.KntStatusSlw, "IX_Klient_knt_StatusSlw");

            entity.HasIndex(e => e.KntOpeMod, "IX_Klient_knt_opeMod");

            entity.HasIndex(e => e.KntOpeUtw, "IX_Klient_knt_opeUtw");

            entity.HasIndex(e => e.KntAkronim, "UQ__Klient__A0B2EA6EFF1B567E").IsUnique();

            entity.Property(e => e.KntId).HasColumnName("knt_id");
            entity.Property(e => e.KntAkronim)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("knt_akronim");
            entity.Property(e => e.KntDataKontaktu)
                .HasColumnType("datetime")
                .HasColumnName("knt_dataKontaktu");
            entity.Property(e => e.KntDataMod)
                .HasColumnType("datetime")
                .HasColumnName("knt_dataMod");
            entity.Property(e => e.KntDataUrodzenia).HasColumnName("knt_dataUrodzenia");
            entity.Property(e => e.KntDataUtw)
                .HasColumnType("datetime")
                .HasColumnName("knt_dataUtw");
            entity.Property(e => e.KntDoKontaktu).HasColumnName("knt_doKontaktu");
            entity.Property(e => e.KntEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("knt_email");
            entity.Property(e => e.KntNazwa)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("knt_nazwa");
            entity.Property(e => e.KntOpeMod).HasColumnName("knt_opeMod");
            entity.Property(e => e.KntOpeUtw).HasColumnName("knt_opeUtw");
            entity.Property(e => e.KntOpis)
                .IsUnicode(false)
                .HasColumnName("knt_opis");
            entity.Property(e => e.KntStacjonarny).HasColumnName("knt_stacjonarny");
            entity.Property(e => e.KntStatusSlw).HasColumnName("knt_StatusSlw");
            entity.Property(e => e.KntTel)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("knt_tel");

            entity.HasOne(d => d.KntOpeModNavigation).WithMany(p => p.KlientKntOpeModNavigations)
                .HasForeignKey(d => d.KntOpeMod)
                .HasConstraintName("FK__Klient__knt_opeM__ope_id");

            entity.HasOne(d => d.KntOpeUtwNavigation).WithMany(p => p.KlientKntOpeUtwNavigations)
                .HasForeignKey(d => d.KntOpeUtw)
                .HasConstraintName("FK__Klient__knt_opeU__ope_id");

            entity.HasOne(d => d.KntStatusSlwNavigation).WithMany(p => p.Klients)
                .HasForeignKey(d => d.KntStatusSlw)
                .HasConstraintName("FK_Klient_Slowniki");
            entity.Ignore(e => e.KntStatus);
            entity.Ignore(e => e.KntOpeUNazwa);
            entity.Ignore(e => e.Plikis);
        });

        modelBuilder.Entity<Operatorzy>(entity =>
        {
            entity.HasKey(e => e.OpeId).HasName("PK__Operator__73A7AA01F34DC888");

            entity.ToTable("Operatorzy");

            entity.Property(e => e.OpeId).HasColumnName("ope_id");
            entity.Property(e => e.OpeDataUtw)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ope_dataUtw");
            entity.Property(e => e.OpeEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ope_email");
            entity.Property(e => e.OpeGoogleapikey)
                .IsUnicode(false)
                .HasColumnName("ope_googleapikey");
            entity.Property(e => e.OpeHaslo)
                .IsUnicode(false)
                .HasColumnName("ope_haslo");
            entity.Property(e => e.OpeIsAdmin).HasColumnName("ope_IsAdmin");
            entity.Property(e => e.OpeLogin)
                .IsUnicode(false)
                .HasColumnName("ope_login");
            entity.Property(e => e.OpeNazwa)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("ope_nazwa");
        });

        modelBuilder.Entity<Pliki>(entity =>
        {
            entity.HasKey(e => e.FileId).HasName("PK__Pliki__0FFFC99601C9FF05");

            entity.ToTable("Pliki", "app");

            entity.HasIndex(e => e.FileGuid, "UQ__Pliki__12E9E40437197B5A").IsUnique();

            entity.Property(e => e.FileId).HasColumnName("File_Id");
            entity.Property(e => e.FileFullPath)
                .IsUnicode(false)
                .HasColumnName("File_FullPath");
            entity.Property(e => e.FileGuid)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("File_GUID");
            entity.Property(e => e.FileKntId).HasColumnName("File_KntId");
            entity.Property(e => e.FileNazwa)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("File_Nazwa");
            entity.Property(e => e.FileWizId).HasColumnName("File_WizId");
            entity.Property(e => e.FilePath)
                .IsUnicode(false)
                .HasColumnName("File_Path");
            entity.Property(e => e.FileWizId).HasColumnName("File_WizId");

            entity.HasOne(d => d.FileKnt).WithMany(p => p.Plikis)
                .HasForeignKey(d => d.FileKntId)
                .HasConstraintName("FK_Pliki_Knt");

            entity.HasOne(d => d.FileWiz).WithMany(p => p.Plikis)
                .HasForeignKey(d => d.FileWizId)
                .HasConstraintName("FK_Pliki_Wiz");
        });

        modelBuilder.Entity<Slowniki>(entity =>
        {
            entity.HasKey(e => e.SlwId).HasName("PK__Slowniki__37D060C973C4146B");

            entity.ToTable("Slowniki");

            entity.HasIndex(e => e.SlwWartoscS, "UQ__Slowniki__534547542580D551").IsUnique();

            entity.Property(e => e.SlwId).HasColumnName("slw_id");
            entity.Property(e => e.SlwDataMod)
                .HasColumnType("datetime")
                .HasColumnName("slw_DataMod");
            entity.Property(e => e.SlwDataUtw)
                .HasColumnType("datetime")
                .HasColumnName("slw_DataUtw");
            entity.Property(e => e.SlwOpis)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("slw_Opis");
            entity.Property(e => e.SlwOpeMod).HasColumnName("slw_opeMod");
            entity.Property(e => e.SlwOpeUtw).HasColumnName("slw_opeUtw");
            entity.Property(e => e.SlwTyp).HasColumnName("slw_typ");
            entity.Property(e => e.SlwWartoscD)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("slw_wartoscD");
            entity.Property(e => e.SlwWartoscS)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("slw_wartoscS");
            entity.Property(e => e.SlwNextSlw).HasColumnName("slw_nextSlw");
            entity.HasOne(d => d.SlwOpeUtwNavigation).WithMany(p => p.Slownikis)
                .HasForeignKey(d => d.SlwOpeUtw)
                .HasConstraintName("FK__Slowniki__slw_op__47DBAE45");
        });

        modelBuilder.Entity<Wizyty>(entity =>
        {
            entity.HasKey(e => e.WizId).HasName("PK__Wizyty__wiz_id");

            entity.ToTable("Wizyty");

            entity.HasIndex(e => e.WizKntid, "IX_Wizyty_wiz_kntid");

            entity.HasIndex(e => e.WizOpeMod, "IX_Wizyty_wiz_opeMod");

            entity.HasIndex(e => e.WizOpeUtw, "IX_Wizyty_wiz_opeUtw");

            entity.HasIndex(e => e.WizTyp, "IX_Wizyty_wiz_typ");

            entity.Property(e => e.WizId).HasColumnName("wiz_id");
            entity.Property(e => e.WizDataKoniec)
                .HasColumnType("datetime")
                .HasColumnName("wiz_dataKoniec");
            entity.Property(e => e.WizDataMod)
                .HasColumnType("datetime")
                .HasColumnName("wiz_dataMod");
            entity.Property(e => e.WizDataStart)
                .HasColumnType("datetime")
                .HasColumnName("wiz_dataStart");
            entity.Property(e => e.WizDataUtw)
                .HasColumnType("datetime")
                .HasColumnName("wiz_dataUtw");
            entity.Property(e => e.WizGooglesync).HasColumnName("wiz_googlesync");
            entity.Property(e => e.WizKntid).HasColumnName("wiz_kntid");
            entity.Property(e => e.WizOpeMod).HasColumnName("wiz_opeMod");
            entity.Property(e => e.WizOpeUtw).HasColumnName("wiz_opeUtw");
            entity.Property(e => e.WizOpis)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("wiz_opis");
            entity.Property(e => e.WizPoprzedniawizId)
                .HasDefaultValue(0)
                .HasColumnName("wiz_poprzedniawizID");
            entity.Property(e => e.WizTyp).HasColumnName("wiz_typ");

            entity.HasOne(d => d.WizKnt).WithMany(p => p.Wizyties)
                .HasForeignKey(d => d.WizKntid)
                .HasConstraintName("FK__Wizyty__wiz_knti__knt_id");

            entity.HasOne(d => d.WizOpeModNavigation).WithMany(p => p.WizytyWizOpeModNavigations)
                .HasForeignKey(d => d.WizOpeMod)
                .HasConstraintName("FK__Wizyty__wiz_opeM__ope_id");

            entity.HasOne(d => d.WizOpeUtwNavigation).WithMany(p => p.WizytyWizOpeUtwNavigations)
                .HasForeignKey(d => d.WizOpeUtw)
                .HasConstraintName("FK__Wizyty__wiz_opeU__ope_id");

            entity.HasOne(d => d.WizTypNavigation).WithMany(p => p.Wizyties)
                .HasForeignKey(d => d.WizTyp)
                .HasConstraintName("FK__Wizyty__wiz_typ__slw_id");
            entity.Ignore(e => e.WizKntNazwa);
            entity.Ignore(e => e.WizOpeNazwa);
            entity.Ignore(e => e.WizTypSlw);
            entity.Ignore(e => e.WizKntTel);
            entity.Ignore(e => e.WizKntEmail);
            entity.Ignore(e => e.WizKntAkronim);
            entity.Ignore(e => e.WizKntStatus);
            entity.Ignore(e => e.WizNextEtap);
            entity.Ignore(e => e.WizNextEtapId);
        });

        modelBuilder.Entity<Zadanium>(entity =>
        {
            entity.HasKey(e => e.ZadId).HasName("PK__Zadania__AAB64147E26E387D");

            entity.Property(e => e.ZadId).HasColumnName("zad_id");
            entity.Property(e => e.ZadDataUkonczenia)
                .HasColumnType("datetime")
                .HasColumnName("zad_dataUkonczenia");
            entity.Property(e => e.ZadNazwa)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("zad_nazwa");
            entity.Property(e => e.ZadOpeid).HasColumnName("zad_opeid");
            entity.Property(e => e.ZadOpis)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("zad_opis");
        });

        //OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
