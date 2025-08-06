using AndroidCompound5.BusinessObject.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AndroidCompound5.DataAccess
{
	public class AppDBContex : DbContext
	{
		private readonly string _dbPath;

		public DbSet<CompDescDto> CompDesc => Set<CompDescDto>();
		public DbSet<ConfigAppDto> ConfigApp => Set<ConfigAppDto>();
		public DbSet<CompoundDto> Compounds => Set<CompoundDto>();
		public DbSet<EnforcerDto> Enforcers => Set<EnforcerDto>();
		public DbSet<LogServerDto> LogServers => Set<LogServerDto>();
		public DbSet<GpsDto> Gps => Set<GpsDto>();
		public DbSet<HandheldDto> Handhelds => Set<HandheldDto>();
		public DbSet<InfoDto> Infos => Set<InfoDto>();
		public DbSet<MessageDto> Messages => Set<MessageDto>();
		public DbSet<NoteDto> Notes => Set<NoteDto>();
		public DbSet<OffrateDto> Offrates => Set<OffrateDto>();
		public DbSet<PassBulanDto> PassBulans => Set<PassBulanDto>();
		public DbSet<SemakPassDto> SemakPasses => Set<SemakPassDto>();
		public DbSet<StreetDto> Streets => Set<StreetDto>();
		public DbSet<CarCategoryDto> CarCategories => Set<CarCategoryDto>();
		public DbSet<CarTypeDto> CarTypes => Set<CarTypeDto>();
		public DbSet<CarColorDto> CarColors => Set<CarColorDto>();
		public DbSet<OffendDto> Offends => Set<OffendDto>();
		public DbSet<ZoneDto> Zones => Set<ZoneDto>();
		public DbSet<ActDto> Acts => Set<ActDto>();
		public DbSet<DeliveryDto> Deliveries => Set<DeliveryDto>();
		public DbSet<MukimDto> Mukims => Set<MukimDto>();
		public DbSet<TempatJadiDto> TempatJadis => Set<TempatJadiDto>();

		public AppDBContex(string dbPath)
		{
			_dbPath = dbPath;
			Database.EnsureCreated(); // optional, or use migration
		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseSqlite($"Data Source={_dbPath}");
	}
}
