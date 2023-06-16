using Entities.CoreServicesModels.AccountTeamModels;
using System.Collections.Specialized;
using System.Web;

namespace Dashboard.Utility
{
    public class UpdateResultsUtils
    {
        private readonly ServicesHttpClient _servicesHttp;

        public UpdateResultsUtils(ServicesHttpClient servicesHttp)
        {
            _servicesHttp = servicesHttp;
            _servicesHttp.BaseUri = "https://egypt-fantasy-automate.azurewebsites.net/";
        }

        public void UpdateStandings()
        {
            _servicesHttp.OnPost("Standings/v1/Standings/UpdateStandings", null).Wait();
        }

        public void UpdateGames()
        {
            _servicesHttp.OnPost("​Games​/v1​/Games​/UpdateGames", null).Wait();
        }

        public void UpdateGameResult(int fk_GameWeak, int fk_TeamGameWeak, string _365_MatchId, bool runBonus)
        {
            NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);

            query.Add("_365_MatchId", _365_MatchId);
            query.Add("Id", fk_TeamGameWeak.ToString());
            query.Add("runBonus", runBonus.ToString().ToLower());
            query.Add("runAll", "true");

            if (fk_GameWeak > 0)
            {
                query.Add("fk_GameWeak", fk_GameWeak.ToString());
                query.Add("IsEnded", "true");
            }

            string queryString = query.ToString();

            _servicesHttp.OnPost("Games/v1/GameResult/UpdateGameResult?" + queryString, null).Wait();
        }

        public void UpdateAccountTeamGameWeakRanking(int fk_GameWeak)
        {
            NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);

            query.Add("fk_GameWeak", fk_GameWeak.ToString());

            string queryString = query.ToString();

            _servicesHttp.OnPost("AccountTeam/v1/AccountTeam/UpdateAccountTeamGameWeakRanking?" + queryString, null).Wait();
        }

        public void UpdateAccountTeamPoints(
            int fk_GameWeak,
            int fk_AccountTeamGameWeak,
            List<int> fk_Players)
        {
            NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);

            query.Add("fk_GameWeak", fk_GameWeak.ToString());
            query.Add("fk_AccountTeamGameWeak", fk_AccountTeamGameWeak.ToString());

            if (fk_Players != null && fk_Players.Any())
            {
                foreach (int fk_Player in fk_Players)
                {
                    query.Add("fk_Players", fk_Player.ToString());
                }
            }

            string queryString = query.ToString();

            _servicesHttp.OnPost("AccountTeam/v1/AccountTeam/UpdateAccountTeamsPoints?" + queryString, null).Wait();
        }

        public void UpdatePrivateLeagueRanking(int fk_GameWeak, int id)
        {
            NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);

            if (fk_GameWeak > 0)
            {
                query.Add("fk_GameWeak", fk_GameWeak.ToString());
            }
            if (id > 0)
            {
                query.Add("id", id.ToString());
            }

            string queryString = query.ToString();

            _servicesHttp.OnPost("AccountTeam/v1/AccountTeam/UpdatePrivateLeaguesRanking?" + queryString, null).Wait();
        }

        public void UpdateAccountTeamUpdateCards(AccountTeamsUpdateCards updateCards)
        {
            NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);

            string queryString = query.ToString();

            _servicesHttp.OnPost("AccountTeam/v1/AccountTeam/UpdateAccountTeamUpdateCards?" + queryString, updateCards).Wait();
        }

    }
}
