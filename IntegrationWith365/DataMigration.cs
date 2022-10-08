using CoreServices;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.StandingsModels;
using Entities.DBModels.TeamModels;
using IntegrationWith365.Entities.GamesModels;
using IntegrationWith365.Entities.SquadsModels;
using IntegrationWith365.Entities.StandingsModels;
using IntegrationWith365.Parameters;

namespace IntegrationWith365
{
    public class DataMigration
    {
        private readonly _365Services _365Services;
        protected readonly UnitOfWork _unitOfWork;

        public DataMigration(UnitOfWork unitOfWork, _365Services _365Services)
        {
            this._365Services = _365Services;
            _unitOfWork = unitOfWork;
        }

        public async Task InsertTeams()
        {
            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            StandingsReturn standingsInArabic = await _365Services.GetStandings(new _365StandingsParameters
            {
                SeasonNum = int.Parse(season._365_SeasonId),
                IsArabic = true,
            });

            StandingsReturn standingsInEnglish = await _365Services.GetStandings(new _365StandingsParameters
            {
                SeasonNum = int.Parse(season._365_SeasonId),
                IsArabic = false,
            });

            List<Competitor> competitorsInArabic = standingsInArabic.Standings.SelectMany(a => a.Rows.Select(b => b.Competitor)).ToList();
            List<Competitor> competitorsInEnglish = standingsInEnglish.Standings.SelectMany(a => a.Rows.Select(b => b.Competitor)).ToList();

            for (int i = 0; i < competitorsInArabic.Count; i++)
            {
                _unitOfWork.Team.CreateTeam(new Team
                {
                    Name = competitorsInArabic[i].Name,
                    _365_TeamId = competitorsInArabic[i].Id.ToString(),
                    TeamLang = new TeamLang
                    {
                        Name = competitorsInEnglish[i].Name
                    }
                });
            }
            await _unitOfWork.Save();
        }

        public async Task InsertPositions()
        {
            List<TeamModel> teams = _unitOfWork.Team.GetTeams(new TeamParameters
            {
            }, otherLang: false).ToList();

            List<PlayerPosition> playerPositions = new()
            {
                new PlayerPosition
                {
                    Name = "مدرب",
                    _365_PositionId = "0",
                    PlayerPositionLang = new PlayerPositionLang
                    {
                        Name = "Coach"
                    }
                }
            };

            foreach (TeamModel team in teams)
            {
                SquadReturn squadsInArabic = await _365Services.GetSquads(new _365SquadsParameters
                {
                    Competitors = int.Parse(team._365_TeamId),
                    IsArabic = true,
                });

                SquadReturn squadsInEnglish = await _365Services.GetSquads(new _365SquadsParameters
                {
                    Competitors = int.Parse(team._365_TeamId),
                    IsArabic = false,
                });

                List<Position> positionsInArabic = squadsInArabic.Squads
                                                      .SelectMany(a => a.Athletes.Select(b => b.Position))
                                                      .GroupBy(a => a.Id)
                                                      .Where(a => a.Key > 0)
                                                      .Select(a => new Position
                                                      {
                                                          Id = a.Key,
                                                          Name = a.Select(a => a.Name).First()
                                                      })
                                                      .ToList();

                List<Position> positionsInEnglish = squadsInEnglish.Squads
                                                        .SelectMany(a => a.Athletes.Select(b => b.Position))
                                                        .GroupBy(a => a.Id)
                                                        .Where(a => a.Key > 0)
                                                        .Select(a => new Position
                                                        {
                                                            Id = a.Key,
                                                            Name = a.Select(a => a.Name).First()
                                                        })
                                                        .ToList();

                for (int i = 0; i < positionsInArabic.Count; i++)
                {
                    if (!playerPositions.Any(a => a._365_PositionId == positionsInArabic[i].Id.ToString()))
                    {
                        playerPositions.Add(new PlayerPosition
                        {
                            Name = positionsInArabic[i].Name,
                            _365_PositionId = positionsInArabic[i].Id.ToString(),
                            PlayerPositionLang = new PlayerPositionLang
                            {
                                Name = positionsInEnglish[i].Name
                            }
                        });
                    }
                }
            }

            foreach (PlayerPosition playerPosition in playerPositions)
            {
                _unitOfWork.Team.CreatePlayerPosition(playerPosition);
            }

            await _unitOfWork.Save();
        }

        public async Task InsertPlayers()
        {
            List<TeamModel> teams = _unitOfWork.Team.GetTeams(new TeamParameters
            {
            }, otherLang: false).ToList();

            List<PlayerPositionModel> positions = _unitOfWork.Team.GetPlayerPositions(new PlayerPositionParameters
            {
            }, otherLang: false).ToList();

            List<Player> players = new();

            foreach (TeamModel team in teams)
            {
                SquadReturn squadsInArabic = await _365Services.GetSquads(new _365SquadsParameters
                {
                    Competitors = int.Parse(team._365_TeamId),
                    IsArabic = true,
                });

                SquadReturn squadsInEnglish = await _365Services.GetSquads(new _365SquadsParameters
                {
                    Competitors = int.Parse(team._365_TeamId),
                    IsArabic = false,
                });

                List<Athlete> athletesInArabic = squadsInArabic.Squads.SelectMany(a => a.Athletes).ToList();
                List<Athlete> athletesInEnglish = squadsInEnglish.Squads.SelectMany(a => a.Athletes).ToList();

                for (int i = 0; i < athletesInArabic.Count; i++)
                {
                    players.Add(new Player
                    {
                        Name = athletesInArabic[i].Name,
                        ShortName = athletesInArabic[i].ShortName ?? athletesInEnglish[i].ShortName,
                        Age = athletesInArabic[i].Age,
                        PlayerNumber = athletesInArabic[i].JerseyNum.ToString(),
                        _365_PlayerId = athletesInArabic[i].Id.ToString(),
                        Fk_Team = team.Id,
                        Fk_PlayerPosition = positions.Where(a => a._365_PositionId == athletesInArabic[i].Position.Id.ToString())
                                                     .Select(a => a.Id)
                                                     .First(),
                        PlayerLang = new PlayerLang
                        {
                            Name = athletesInEnglish[i].Name,
                            ShortName = athletesInEnglish[i].ShortName ?? athletesInArabic[i].ShortName,
                        }
                    });
                }
            }

            foreach (var player in players)
            {
                _unitOfWork.Team.CreatePlayer(player);
            }
            await _unitOfWork.Save();
        }

        public async Task InsertStandings()
        {
            List<TeamModel> teams = _unitOfWork.Team.GetTeams(new TeamParameters
            {
            }, otherLang: false).ToList();

            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            StandingsReturn standings = await _365Services.GetStandings(new _365StandingsParameters
            {
                SeasonNum = int.Parse(season._365_SeasonId),
                IsArabic = true,
            });

            List<Row> rows = standings.Standings.SelectMany(a => a.Rows).ToList();

            foreach (var row in rows)
            {
                _unitOfWork.Standings.CreateStandings(new Standings
                {
                    Fk_Season = season.Id,
                    Fk_Team = teams.Where(a => a._365_TeamId == row.Competitor.Id.ToString())
                                   .Select(a => a.Id)
                                   .First(),
                    GamePlayed = row.GamePlayed,
                    GamesWon = row.GamesWon,
                    GamesLost = row.GamesLost,
                    GamesEven = row.GamesEven,
                    For = row.For,
                    Against = row.Against,
                    Ratio = row.Ratio,
                    Strike = row.Strike,
                    Position = row.Position,
                });
            }
            await _unitOfWork.Save();
        }

        public async Task InsertGames()
        {
            List<TeamModel> teams = _unitOfWork.Team.GetTeams(new TeamParameters
            {
            }, otherLang: false).ToList();

            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            StandingsReturn standings = await _365Services.GetStandings(new _365StandingsParameters
            {
                SeasonNum = int.Parse(season._365_SeasonId),
                IsArabic = true,
            });

            List<Row> rows = standings.Standings.SelectMany(a => a.Rows).ToList();

            foreach (var row in rows)
            {
                _unitOfWork.Standings.CreateStandings(new Standings
                {
                    Fk_Season = season.Id,
                    Fk_Team = teams.Where(a => a._365_TeamId == row.Competitor.Id.ToString())
                                   .Select(a => a.Id)
                                   .First(),
                    GamePlayed = row.GamePlayed,
                    GamesWon = row.GamesWon,
                    GamesLost = row.GamesLost,
                    GamesEven = row.GamesEven,
                    For = row.For,
                    Against = row.Against,
                    Ratio = row.Ratio,
                    Strike = row.Strike,
                    Position = row.Position,
                });
            }
            await _unitOfWork.Save();
        }
    }
}
