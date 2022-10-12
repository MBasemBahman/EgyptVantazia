using Dashboard.Areas.UserEntity.Models;
using Entities.CoreServicesModels.UserModels;
using System.ComponentModel;

namespace Dashboard.Areas.DashboardAdministration.Models
{
    public class DashboardAdministratorFilter : DtParameters
    {
        public int Id { get; set; }
        public int Fk_DashboardAdministrationRole { get; set; }

    }
    public class DashboardAdministratorDto : DashboardAdministratorModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        public new UserDto User { get; set; }

    }

    public class DashboardAdministratorCreateOrEditModelDto
    {
        public DashboardAdministratorCreateOrEditModelDto()
        {
            DashboardAdministrator = new();
            User = new();
        }
        public DashboardAdministratorCreateOrEditModel DashboardAdministrator { get; set; }
        public UserCreateModel User { get; set; }

    }


}
