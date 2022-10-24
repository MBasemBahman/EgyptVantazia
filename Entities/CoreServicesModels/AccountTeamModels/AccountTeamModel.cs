using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.AccountModels;
using Entities.DBModels.SeasonModels;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;

namespace Entities.CoreServicesModels.AccountTeamModels
{
    public class AccountTeamParameters : RequestParameters
    {
        public int Fk_Account { get; set; }
        public int Fk_Season { get; set; }

        public DateTime? CreatedAtFrom { get; set; }

        public DateTime? CreatedAtTo { get; set; }

        [DisplayName(nameof(AccountFullName))]
        public string AccountFullName { get; set; }

        [DisplayName(nameof(AccountUserName))]
        public string AccountUserName { get; set; }
    }

    public class AccountTeamModel : AuditImageEntity
    {
        [DisplayName(nameof(Account))]
        public int Fk_Account { get; set; }

        [DisplayName(nameof(Account))]
        public AccountModel Account { get; set; }

        [DisplayName(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(Season))]
        public SeasonModel Season { get; set; }

        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(TotalPoints))]
        public int TotalPoints { get; set; }

        [DisplayName(nameof(TotalMoney))]
        public int TotalMoney { get; set; }

        [DisplayName(nameof(GlobalRanking))]
        public double GlobalRanking { get; set; }

        [DisplayName(nameof(CountryRanking))]
        public double CountryRanking { get; set; }

        [DisplayName(nameof(FavouriteTeamRanking))]
        public double FavouriteTeamRanking { get; set; }

        public int CurrentGameWeakPoints { get; set; }

        public int TransferCount { get; set; }

        public int FreeTransferCount { get; set; }

        public int TotalTeamPrice { get; set; }

        public int AveragePoints { get; set; }

        public int MaxPoints { get; set; }

        public int PlayersCount { get; set; }

        [DisplayName(nameof(FreeTransfer))]
        public int FreeTransfer { get; set; }
    }

    public class AccountTeamCreateOrEditModel
    {
        [DisplayName(nameof(Account))]
        [ForeignKey(nameof(Account))]
        public int Fk_Account { get; set; }

        [DisplayName(nameof(Season))]
        [ForeignKey(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
        
        [DisplayName(nameof(ImageUrl))]
        public string ImageUrl { get; set; }
        
        [DisplayName(nameof(StorageUrl))]
        public string StorageUrl { get; set; }

        [DisplayName(nameof(TotalPoints))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public int TotalPoints { get; set; }

        [DisplayName(nameof(TotalMoney))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public int TotalMoney { get; set; }

        [DisplayName(nameof(TotalMoney))]
        public int FreeTransfer { get; set; }
    }

    public class AccountTeamCreateModel
    {
        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(ImageFile))]
        public IFormFile ImageFile { get; set; }
    }
}
