namespace API.Utility
{
    public class _365Utils
    {
        private readonly ServicesHttpClient _servicesHttp;
        public _365Utils(ServicesHttpClient servicesHttp)
        {
            _servicesHttp = servicesHttp;
            _servicesHttp.BaseUri = "https://webws.365scores.com/web/";
        }

        public async Task GetGames()
        {
            var content = await _servicesHttp.OnGet("games/?langId=1&timezoneId=12&userCountryId=131&competitions=552&aftergame=3555948&direction=-1&withmainodds=true");

            //GitHubBranches = await JsonSerializer.DeserializeAsync
            //  <IEnumerable<GitHubBranch>>(contentStream);
        }

        public async Task GetGame()
        {
            var content = await _servicesHttp.OnGet("game/?appTypeId=5&langId=1&timezoneName=Africa/Cairo&userCountryId=131&gameId=3555923&matchupId=8300-8306-55");
        }

        public async Task GetStandings()
        {
            var content = await _servicesHttp.OnGet("standings/?appTypeId=5&langId=1&timezoneName=Africa/Cairo&userCountryId=131&competitions=552&live=false&stageNum=1&seasonNum=26");
        }
    }
}
