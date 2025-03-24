using LoginEkrani.Models.Admin;
using LoginEkrani.Models.Login;
using Microsoft.EntityFrameworkCore;

namespace LoginEkrani
{
    public class Database : DbContext
	{
		public Database(DbContextOptions<Database> options)
	  : base(options)
		{
		}

        public DbSet<FormTabloModel> kpsft_formtablo { get; set; }
        public DbSet<FormHareketiModel> kpsft_form_haraketi { get; set; }
        public DbSet<DepoTuru> kpsft_depo_turu {  get; set; }
        public DbSet<Role> kpsft_roller { get; set; }
        public DbSet<UserRole> kpsft_kullanıcı_rolleri { get; set; }
		public DbSet<VergiDairesiModel> kpsft_vergidairesi { get; set; }
		public DbSet<FisHaraketleriCariModel> kpsft_fisharaketi_cari { get; set; }

        public DbSet<FisHaraketleriDepoModel> kpsft_fisharaketi_depo { get; set; }
		public DbSet<StokKartıModel> kpsft_stok_karti {  get; set; }
        public DbSet<BirimModel> kpsft_birimler { get; set; }
		public DbSet<TurModel> kpsft_turler {  get; set; }
		public DbSet<LogKayıtlarıModelDis> kpsft_log_kayitlari_dis	{ get; set; }
		public DbSet<LoginPageModel> kpsft_boskay {  get; set; }
		public DbSet<AlısKdvOraniModel> kpsft_alis_kdv_orani { get; set; }
		public DbSet<SatisKdvOrani> kpsft_satis_kdv_orani { get; set; }
        public DbSet <GrupModel>  kpsft_grup_kodu {  get; set; }
		public DbSet <GumrukKoduModel> kpsft_gumruk_kodu { get; set; }
		public DbSet <KaliteModel> kpsft_kalite { get; set; }
		public DbSet <StokTipi> kpsft_stoktipi { get; set; }
		public DbSet <DurumModel> kpsft_durum { get; set; }
		public DbSet <FisTabloCariModel> kpsft_fistablo_cari {  get; set; }

        public DbSet<FisTabloDepoModel> kpsft_fistablo_depo { get; set; }

		public DbSet<CariModel> kpsft_cariler {  get; set; }

        public DbSet<DepoModel> kpsft_depobilgisi { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User ve Role arasındaki ilişki
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.id_kpsft, ur.kpsft_rol_id });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.id_kpsft);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.kpsft_rol_id);
        }

    }
}