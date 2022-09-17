using API.Areas.PlayerScoreArea.Models;
using API.Areas.SeasonArea.Models;
using API.Areas.TeamArea.Models;
using API.Areas.UserArea.Models;
using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.CoreServicesModels.UserModels;

namespace API.MappingProfileCls
{
    public class MappingProfile : Profile
    {
        private readonly AppSettings _appSettings;
        private TenantEnvironments _tenantEnvironment;

        public MappingProfile(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            SetTenantEnvironment();

            MapperConfiguration configuration = new(cfg =>
            {
                cfg.AllowNullCollections = false;
            });

            CreateMap<DateTime, string>().ConvertUsing(new DateTimeTypeConverter());
            CreateMap<DateTime?, string>().ConvertUsing(new DateTimeNullableTypeConverter());
            CreateMap<string, string>().ConvertUsing(new StringTypeConverter());
            CreateMap<TimeSpan, string>().ConvertUsing(new TimeSpanTypeConverter());
            CreateMap<TimeSpan?, string>().ConvertUsing(new TimeSpanNullableTypeConverter());
            CreateMap<string, List<string>>().ConvertUsing(new ListOfStringTypeConverter());

            #region AuthenticationModels

            _ = CreateMap<UserForRegistrationDto, User>();

            _ = CreateMap<UserAuthenticatedDto, UserDto>();

            #endregion

            #region UserModels

            _ = CreateMap<UserForEditDto, User>();

            _ = CreateMap<UserForEditCultureDto, User>();

            _ = CreateMap<DeviceCreateModel, Device>();

            _ = CreateMap<User, UserDto>();


            #endregion

            #region SeasonModels

            _ = CreateMap<SeasonModel, SeasonDto>();
            _ = CreateMap<GameWeakModel, GameWeakDto>();
            _ = CreateMap<TeamGameWeakModel, TeamGameWeakDto>();

            #endregion

            #region TeamModels

            _ = CreateMap<TeamModel, TeamDto>();
            _ = CreateMap<PlayerModel, PlayerDto>();
            _ = CreateMap<PlayerPositionModel, PlayerPositionDto>();
            _ = CreateMap<PlayerPriceModel, PlayerPriceDto>();

            #endregion

            #region PlayerScoreModels
            _ = CreateMap<ScoreTypeModel, ScoreTypeDto>();
            #endregion
        }

        private void SetTenantEnvironment()
        {
            foreach (TenantEnvironments item in (TenantEnvironments[])Enum.GetValues(typeof(TenantEnvironments)))
            {
                if (_appSettings.TenantEnvironment.ToUpper() == item.ToString())
                {
                    _tenantEnvironment = item;
                    break;
                }
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

        public class StringTypeConverter : ITypeConverter<string, string>
        {
            public string Convert(string source, string destination, ResolutionContext context)
            {
                if (!string.IsNullOrEmpty(source) && source.StartsWith("http"))
                {
                    source = source.Replace(" ", "%20");

                }

                return source;
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
}
