﻿using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.LocationModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.ResponseFeatures;

namespace Entities.AuthenticationModels
{
    public class UserForRegistrationDto
    {
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string UserName { get; set; }

        //[Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string PhoneNumber { get; set; }

        [DisplayName(nameof(Culture))]
        public string Culture { get; set; }

        [DisplayName(nameof(IsExternalLogin))]
        [DefaultValue(false)]
        public bool IsExternalLogin { get; set; }
    }

    public class UserForEditCultureDto
    {
        [DisplayName(nameof(Culture))]
        public string Culture { get; set; }
    }

    public class UserForEditDto
    {
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string UserName { get; set; }

        //[DataType(DataType.EmailAddress)]
        //[EmailAddress]
        public string EmailAddress { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string PhoneNumber { get; set; }
    }

    public class ChangePasswordDto
    {
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DataType(DataType.Password)]
        [DisplayName(nameof(OldPassword))]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DataType(DataType.Password)]
        [DisplayName(nameof(NewPassword))]
        public string NewPassword { get; set; }
    }

    public class UserForAuthenticationDto
    {
        //[Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DisplayName(nameof(UserName))]
        public string UserName { get; set; }

        //[Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DataType(DataType.Password)]
        [DisplayName(nameof(Password))]
        public string Password { get; set; }

        [DisplayName(nameof(IsExternalLogin))]
        [DefaultValue(false)]
        public bool IsExternalLogin { get; set; }

        public bool OtherLang { get; set; }
    }

    public class UserForTokenDto
    {
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Token { get; set; }
    }

    public class VerificationDto
    {
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Code { get; set; }
    }

    public class UserForVerificationDto : VerificationDto
    {
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class UserForResetDto
    {
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DisplayName(nameof(UserName))]
        public string UserName { get; set; }
    }

    public class UserAuthenticatedDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public int Fk_Account { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string RefCode { get; set; }

        [DisplayName(nameof(RefCodeCount))]
        public int RefCodeCount { get; set; }

        public bool ShowAds { get; set; }

        public int Fk_AccountTeam { get; set; }

        public AccountTeamModel AccountTeam { get; set; }
        public int Fk_Season { get; set; }
        public SeasonModel Season { get; set; }
        public int Fk_Country { get; set; }
        public CountryModel Country { get; set; }

        [DisplayName(nameof(FavouriteTeam))]
        [ForeignKey(nameof(FavouriteTeam))]
        public int Fk_FavouriteTeam { get; set; }

        [DisplayName(nameof(FavouriteTeam))]
        public TeamModel FavouriteTeam { get; set; }

        public TokenResponse TokenResponse { get; set; }
        public TokenResponse RefreshTokenResponse { get; set; }

        [JsonIgnore]
        public int Fk_DashboardAdministrator { get; set; }
        [JsonIgnore]
        public int Fk_DashboardAdministrationRole { get; set; }
    }
}
