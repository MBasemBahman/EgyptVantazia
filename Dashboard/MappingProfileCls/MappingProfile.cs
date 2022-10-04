using Dashboard.Areas.AccountEntity.Models;
using Dashboard.Areas.AppInfoEntity.Models;
using Dashboard.Areas.DashboardAdministration.Models;
using Dashboard.Areas.Location.Models;
using Dashboard.Areas.TeamEntity.Models;
using Dashboard.Areas.LogEntity.Models;
using Dashboard.Areas.UserEntity.Models;
using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.AppInfoModels;
using Entities.CoreServicesModels.LocationModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.CoreServicesModels.LogModels;
using Entities.CoreServicesModels.UserModels;
using Entities.DBModels.AccountModels;
using Entities.DBModels.AppInfoModels;
using Entities.DBModels.TeamModels;
using Entities.DBModels.DashboardAdministrationModels;
using Entities.DBModels.LocationModels;
using Entities.DBModels.SharedModels;
using Entities.RequestFeatures;

namespace Dashboard.MappingProfileCls
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapperConfiguration configuration = new(cfg =>
            {
                cfg.AllowNullCollections = false;
            });

            CreateMap<DateTime, string>().ConvertUsing(new DateTimeTypeConverter());
            CreateMap<DateTime?, string>().ConvertUsing(new DateTimeNullableTypeConverter());
            CreateMap<TimeSpan, string>().ConvertUsing(new TimeSpanTypeConverter());
            CreateMap<TimeSpan?, string>().ConvertUsing(new TimeSpanNullableTypeConverter());
            CreateMap<string, List<string>>().ConvertUsing(new ListOfStringTypeConverter());


            _ = CreateMap<DtParameters, RequestParameters>()
                .ForMember(dest => dest.SearchTerm, opt => opt.MapFrom(src => src.Search == null ? "" : src.Search.Value))
                .ForMember(dest => dest.OrderBy, opt =>
                                   opt.MapFrom(src => src.Order == null ?
                                                      "" :
                                                      (src.Order[0].Dir.ToString().ToLower() == "asc" ?
                                                       src.Columns[src.Order[0].Column].Data :
                                                       (src.Columns[src.Order[0].Column].Data.Contains(',') ?
                                                        src.Columns[src.Order[0].Column].Data.Replace(",", " desc,") :
                                                        src.Columns[src.Order[0].Column].Data + " desc"))))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.Length))
                .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.Length > 0 ? (src.Start / src.Length) + 1 : 0))
                .IncludeAllDerived();

            _ = CreateMap<UserAuthenticatedDto, UserDto>();


            #region Account Models

            #region Account
            _ = CreateMap<Account, AccountCreateModel>();

            _ = CreateMap<AccountCreateModel, Account>();

            _ = CreateMap<AccountModel, AccountDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            _ = CreateMap<AccountFilter, AccountParameters>();

            #endregion

            #endregion

            #region Log Models
            _ = CreateMap<LogModel, LogDto>();

            _ = CreateMap<LogFilter, LogParameters>();

            #endregion

            #region User Models

            _ = CreateMap<UserFilter, UserParameters>();

            _ = CreateMap<UserModel, UserDto>();

            _ = CreateMap<User, UserCreateModel>();

            _ = CreateMap<UserCreateModel, User>();

            _ = CreateMap<UserDeviceFilter, UserDeviceParameters>();

            _ = CreateMap<UserDeviceModel, UserDeviceDto>();

            _ = CreateMap<RefreshTokenFilter, RefreshTokenParameters>();

            _ = CreateMap<RefreshTokenModel, RefreshTokenDto>();

            _ = CreateMap<VerificationFilter, VerificationParameters>();

            _ = CreateMap<VerificationModel, Areas.UserEntity.Models.VerificationDto>();

            _ = CreateMap<UserFilter, UserParameters>();

            #endregion

            #region Dashboard Administration Models

            #region Dashboard View
            _ = CreateMap<DashboardViewModel, DashboardViewDto>();

            _ = CreateMap<DashboardView, DashboardViewCreateOrEditModel>();

            _ = CreateMap<DashboardViewLang, DashboardViewLangModel>();

            _ = CreateMap<DashboardViewCreateOrEditModel, DashboardView>();

            _ = CreateMap<DashboardViewLangModel, DashboardViewLang>();
            #endregion

            #region Dashboard Administration Role
            _ = CreateMap<DashboardAdministrationRoleModel, DashboardAdministrationRoleDto>();

            _ = CreateMap<DashboardAdministrationRole, DashboardAdministrationRoleCreateOrEditModel>();

            _ = CreateMap<DashboardAdministrationRoleLang, DashboardAdministrationRoleLangModel>();

            _ = CreateMap<DashboardAdministrationRoleCreateOrEditModel, DashboardAdministrationRole>();

            _ = CreateMap<DashboardAdministrationRoleLangModel, DashboardAdministrationRoleLang>();

            _ = CreateMap<DashboardAdministrationRoleFilter, DashboardAdministrationRoleRequestParameters>();
            #endregion

            #region Dashboard Access Level
            _ = CreateMap<DashboardAccessLevelModel, DashboardAccessLevelDto>();

            _ = CreateMap<DashboardAccessLevel, DashboardAccessLevelCreateOrEditModel>();

            _ = CreateMap<DashboardAccessLevelLang, DashboardAccessLevelLangModel>();

            _ = CreateMap<DashboardAccessLevelCreateOrEditModel, DashboardAccessLevel>();

            _ = CreateMap<DashboardAccessLevelLangModel, DashboardAccessLevelLang>();
            #endregion

            #region Dashboard Administrator
            _ = CreateMap<DashboardAdministrator, DashboardAdministratorCreateOrEditModel>();

            _ = CreateMap<DashboardAdministratorCreateOrEditModel, DashboardAdministrator>();

            _ = CreateMap<DashboardAdministratorModel, DashboardAdministratorDto>();

            _ = CreateMap<DashboardAdministratorFilter, DashboardAdministratorParameters>();
            #endregion
            #endregion

            #region Location

            #region Country

            _ = CreateMap<CountryModel, CountryDto>();

            _ = CreateMap<Country, CountryCreateOrEditModel>();

            _ = CreateMap<CountryLang, CountryLangModel>();

            _ = CreateMap<CountryCreateOrEditModel, Country>();

            _ = CreateMap<CountryLangModel, CountryLang>();
            #endregion

            #endregion

            #region App Info Models

            #region AppAbout

            _ = CreateMap<AppAboutModel, AppAboutDto>();

            _ = CreateMap<AppAbout, AppAboutCreateOrEditModel>();

            _ = CreateMap<AppAboutLang, AppAboutLangModel>();

            _ = CreateMap<AppAboutCreateOrEditModel, AppAbout>();

            _ = CreateMap<AppAboutLangModel, AppAboutLang>();
            #endregion

            #endregion

            #region Team Models

            #region Team
            _ = CreateMap<Team, TeamCreateOrEditModel>();

            _ = CreateMap<TeamCreateOrEditModel, Team>()
                .ForMember(dest => dest.ImageUrl,opt => opt.Ignore())
                .ForMember(dest => dest.StorageUrl,opt => opt.Ignore());

            _ = CreateMap<TeamModel, TeamDto>();

            _ = CreateMap<TeamFilter, TeamParameters>();

            #endregion

            #endregion


        }
    }

    public class DateTimeNullableTypeConverter : ITypeConverter<DateTime?, string>
    {
        public string Convert(DateTime? source, string destination, ResolutionContext context)
        {
            return source == null ? "" : source.Value.AddHours(2).ToString(ApiConstants.DateTimeStringFormat);
        }
    }

    public class DateTimeTypeConverter : ITypeConverter<DateTime, string>
    {
        public string Convert(DateTime source, string destination, ResolutionContext context)
        {
            return source.AddHours(2).ToString(ApiConstants.DateTimeStringFormat);
        }
    }


    public class TimeSpanNullableTypeConverter : ITypeConverter<TimeSpan?, string>
    {
        public string Convert(TimeSpan? source, string destination, ResolutionContext context)
        {
            return source == null ? "" : new DateTime(source.Value.Ticks).ToString(ApiConstants.TimeFormat);
        }
    }

    public class TimeSpanTypeConverter : ITypeConverter<TimeSpan, string>
    {
        public string Convert(TimeSpan source, string destination, ResolutionContext context)
        {
            return new DateTime(source.Ticks).ToString(ApiConstants.TimeFormat);
        }
    }

    public class ListOfStringTypeConverter : ITypeConverter<string, List<string>>
    {
        public List<string> Convert(string source, List<string> destination, ResolutionContext context)
        {
            return !string.IsNullOrEmpty(source) ? source.Split(',').ToList() : null;
        }
    }

}
