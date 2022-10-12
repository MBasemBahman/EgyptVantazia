using CoreServices;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.SeasonModels;
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

            foreach (Player player in players)
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

            foreach (Row row in rows)
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

        public async Task InsertRounds()
        {
            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            List<GameWeakModel> gameWeaks = _unitOfWork.Season.GetGameWeaks(new GameWeakParameters
            {
                Fk_Season = season.Id
            }, otherLang: false).ToList();

            List<int> rounds = GetAllGames().Result.Select(a => a.RoundNum).Distinct().OrderBy(a => a).ToList();

            foreach (int round in rounds)
            {
                if (!gameWeaks.Any(a => a._365_GameWeakId == round.ToString()))
                {
                    _unitOfWork.Season.CreateGameWeak(new GameWeak
                    {
                        Name = round.ToString(),
                        Fk_Season = season.Id,
                        _365_GameWeakId = round.ToString(),
                        GameWeakLang = new GameWeakLang
                        {
                            Name = round.ToString()
                        }
                    });
                }
            }
            await _unitOfWork.Save();
        }

        public async Task InsertGames()
        {
            List<GameWeakModel> gameWeaks = _unitOfWork.Season.GetGameWeaks(new GameWeakParameters
            {
            }, otherLang: false).ToList();

            List<TeamModel> teams = _unitOfWork.Team.GetTeams(new TeamParameters
            {
            }, otherLang: false).ToList();

            List<Games> games = (await GetAllGames()).OrderBy(a => a.RoundNum).ThenBy(a => a.StartTimeVal).ToList();

            foreach (Games game in games)
            {
                int home = teams.Where(a => a._365_TeamId == game.HomeCompetitor.Id.ToString()).Select(a => a.Id).First();
                int away = teams.Where(a => a._365_TeamId == game.AwayCompetitor.Id.ToString()).Select(a => a.Id).First();
                int gameWeak = gameWeaks.Where(a => a._365_GameWeakId == game.RoundNum.ToString()).Select(a => a.Id).First();

                _unitOfWork.Season.CreateTeamGameWeak(new TeamGameWeak
                {
                    Fk_Away = away,
                    Fk_Home = home,
                    Fk_GameWeak = gameWeak,
                    StartTime = game.StartTimeVal,
                    IsEnded = game.IsEnded,
                    _365_MatchId = game.Id.ToString(),
                    AwayScore = (int)game.AwayCompetitor.Score,
                    HomeScore = (int)game.HomeCompetitor.Score,
                });
            }
            await _unitOfWork.Save();
        }

        public async Task InsertGameResult()
        {
            List<TeamGameWeakModel> teamGameWeaks = _unitOfWork.Season.GetTeamGameWeaks(new TeamGameWeakParameters
            {
            }, otherLang: false).ToList();

            foreach (TeamGameWeakModel teamGameWeak in teamGameWeaks)
            {
                _ = await _365Services.GetGame(new _365GameParameters
                {
                    GameId = int.Parse(teamGameWeak._365_MatchId)
                });

                //if (game.Game != null)
                //{


                //}
            }

        }

        #region Games

        private async Task<List<Games>> GetAllGames()
        {
            List<Games> games = new();

            GamesReturn gamesReturn = await _365Services.GetGamesResults(new _365Parameters());

            if (gamesReturn.Games != null && gamesReturn.Games.Any())
            {
                games.AddRange(gamesReturn.Games);

                if (gamesReturn.Paging != null)
                {
                    games.AddRange(await GetGames(gamesReturn.Paging.PreviousAfterGame));
                }
            }

            gamesReturn = await _365Services.GetGamesFixtures(new _365Parameters());

            if (gamesReturn.Games != null && gamesReturn.Games.Any())
            {
                games.AddRange(gamesReturn.Games);

                if (gamesReturn.Paging != null)
                {
                    games.AddRange(await GetGames(gamesReturn.Paging.NextAfterGame));
                }
            }

            return games;
        }

        private async Task<List<Games>> GetGames(int _365_AfterGameStartId)
        {
            int count = 0;

            List<Games> games = new();

            while (_365_AfterGameStartId > 0)
            {
                count++;

                GamesReturn gamesReturn = await _365Services.GetGames(new _365GamesParameters
                {
                    Aftergame = _365_AfterGameStartId
                });

                if (gamesReturn.Games != null && gamesReturn.Games.Any())
                {
                    if (gamesReturn.Paging != null)
                    {
                        _365_AfterGameStartId = gamesReturn.Paging.PreviousAfterGame == 0 ? gamesReturn.Paging.NextAfterGame : gamesReturn.Paging.PreviousAfterGame;
                    }

                    games.AddRange(gamesReturn.Games);
                }
                else
                {
                    _365_AfterGameStartId = 0;
                }
            }

            return games;
        }

        #endregion
    }
}
