using Entities.DBModels.UserModels;
using BC = BCrypt.Net.BCrypt;

namespace ModelBuilderConfig.Configurations.UserModels
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            _ = builder.HasData(new User
            {
                Id = 1,
                Name = DashboardAdministrationRoleEnum.Developer.ToString(),
                UserName = DashboardAdministrationRoleEnum.Developer.ToString(),
                EmailAddress = "user@mail.com",
                Password = BC.HashPassword("dev123456")
            });
        }
    }
}
