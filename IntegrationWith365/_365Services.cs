using IntegrationWith365.Entities.GameModels;
using IntegrationWith365.Entities.GamesModels;
using IntegrationWith365.Entities.SquadsModels;
using IntegrationWith365.Entities.StandingsModels;
using IntegrationWith365.Parameters;
using Newtonsoft.Json;
using Services;

namespace IntegrationWith365
{
    public class _365Services
    {
        private readonly ServicesHttpClient _servicesHttp;
        public _365Services(ServicesHttpClient servicesHttp)
        {
            _servicesHttp = servicesHttp;
            _servicesHttp.BaseUri = "https://webws.365scores.com/web/";
        }

        private static string GetUri(string uri, _365Parameters parameters)
        {
            return $"{uri}/?" +
                   $"{nameof(parameters.LangId)}={parameters.LangId}&" +
                   $"{nameof(parameters.UserCountryId)}={parameters.UserCountryId}&" +
                   $"{nameof(parameters.Competitions)}={parameters.Competitions}&" +
                   $"{nameof(parameters.AppTypeId)}={parameters.AppTypeId}&" +
                   $"{nameof(parameters.TimezoneName)}={parameters.TimezoneName}&";
        }

        // ترتيب الفرق
        public async Task<StandingsReturn> GetStandings(_365StandingsParameters parameters)
        {
            string uri = GetUri("standings", parameters) +
                         $"{nameof(parameters.SeasonNum)}={parameters.SeasonNum}&" +
                         $"{nameof(parameters.StageNum)}={parameters.StageNum}&" +
                         $"{nameof(parameters.Live)}={parameters.Live}&";
            string content = await _servicesHttp.OnGet(uri);

            StandingsReturn data = JsonConvert.DeserializeObject<StandingsReturn>(content);

            return data;
        }

        // لاعيبه الفرق
        public async Task<SquadReturn> GetSquads(_365SquadsParameters parameters)
        {
            string uri = GetUri("squads", parameters) +
                         $"{nameof(parameters.Competitors)}={parameters.Competitors}&";
            string content = await _servicesHttp.OnGet(uri);

            SquadReturn data = JsonConvert.DeserializeObject<SquadReturn>(content);

            return data;
        }

        // مواعيد الماتشات , ونتائج الماتشات
        public async Task<GamesReturn> GetGames(_365GamesParameters parameters)
        {
            string uri = GetUri("games", parameters) +
                         $"{nameof(parameters.TimezoneId)}={parameters.TimezoneId}&" +
                         $"{nameof(parameters.Aftergame)}={parameters.Aftergame}&" +
                         $"{nameof(parameters.Direction)}={parameters.Direction}&" +
                         $"{nameof(parameters.Withmainodds)}={parameters.Withmainodds}&";
            string content = await _servicesHttp.OnGet(uri);

            GamesReturn data = JsonConvert.DeserializeObject<GamesReturn>(content);

            return data;
        }

        // السابقة
        public async Task<GamesReturn> GetGamesResults(_365Parameters parameters)
        {
            string uri = GetUri("games/results", parameters);
            string content = await _servicesHttp.OnGet(uri);

            GamesReturn data = JsonConvert.DeserializeObject<GamesReturn>(content);

            return data;
        }

        // القادمة
        public async Task<GamesReturn> GetGamesFixtures(_365Parameters parameters)
        {
            string uri = GetUri("games/fixtures", parameters);
            string content = await _servicesHttp.OnGet(uri);

            GamesReturn data = JsonConvert.DeserializeObject<GamesReturn>(content);

            return data;
        }

        // بيانات الماتش نفسه
        public async Task<GameReturn> GetGame(_365GameParameters parameters)
        {
            string uri = GetUri("game", parameters) +
                         $"{nameof(parameters.GameId)}={parameters.GameId}&";
            string content = await _servicesHttp.OnGet(uri);

            GameReturn data = JsonConvert.DeserializeObject<GameReturn>(content);

            return data;
        }
    }
}
