using Entities.DBModels.AppInfoModels;

namespace ModelBuilderConfig.Configurations.AppInfoModels
{
    public class AppAboutConfiguration : IEntityTypeConfiguration<AppAbout>
    {
        public void Configure(EntityTypeBuilder<AppAbout> builder)
        {
            _ = builder.HasData(new AppAbout
            {
                Id = 1,
            });
        }
    }

    public class AppAboutLangConfiguration : IEntityTypeConfiguration<AppAboutLang>
    {
        public void Configure(EntityTypeBuilder<AppAboutLang> builder)
        {
            _ = builder.HasData(new AppAboutLang
            {
                Id = 1,
                Fk_Source = 1
            });
        }
    }
}
