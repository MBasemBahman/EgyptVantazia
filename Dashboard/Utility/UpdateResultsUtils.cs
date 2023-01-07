namespace Dashboard.Utility
{
    public class UpdateResultsUtils
    {
        private readonly ServicesHttpClient _servicesHttp;

        public UpdateResultsUtils(ServicesHttpClient servicesHttp)
        {
            _servicesHttp = servicesHttp;
            _servicesHttp.BaseUri = "https://fantasy-hangfirev2.azurewebsites.net/";
        }

        public void UpdateStandings()
        {
            _servicesHttp.OnPost("Standings/v1/Standings/UpdateStandings", null).Wait();
        }

        public void UpdateGames()
        {
            _servicesHttp.OnPost("​Games​/v1​/Games​/UpdateGames", null).Wait();
        }
    }
}
