using Entities.CoreServicesModels.LocationModels;
using System.ComponentModel;
namespace Dashboard.Areas.Location.Models
{
    public class CountryFilter : DtParameters
    {
        public int Id { get; set; }
    }
    public class CountryDto : CountryModel
    {
        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }
    }
}
