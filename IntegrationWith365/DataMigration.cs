using Contracts.Extensions;
using CoreServices;
using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.StandingsModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.StandingsModels;
using Entities.DBModels.TeamModels;
using IntegrationWith365.Entities.GameModels;
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

        #region Team Models

        public async Task InsertOrEditTeams()
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
                if (_unitOfWork.Team.GetTeams(new TeamParameters { _365_TeamId = competitorsInArabic[i].Id.ToString() }, otherLang: false).Any())
                {
                    Team team = await _unitOfWork.Team.FindTeamby365Id(competitorsInArabic[i].Id.ToString(), trackChanges: true);

                    team.Name = competitorsInArabic[i].Name.IsExisting() ? competitorsInArabic[i].Name : team.Name;
                    team.TeamLang.Name = competitorsInEnglish[i].Name.IsExisting() ? competitorsInEnglish[i].Name : team.TeamLang.Name;
                }
                else
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
        }

        public async Task InsertOrEditPositions()
        {
            List<TeamModel> teams = _unitOfWork.Team.GetTeams(new TeamParameters
            {
            }, otherLang: false).ToList();

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
                    if (_unitOfWork.Team.GetPlayerPositions(new PlayerPositionParameters { _365_PositionId = positionsInArabic[i].Id.ToString() }, otherLang: false).Any())
                    {
                        PlayerPosition playerPosition = await _unitOfWork.Team.FindPlayerPositionby365Id(positionsInArabic[i].Id.ToString(), trackChanges: true);
                        playerPosition.Name = positionsInArabic[i].Name.IsExisting() ? positionsInArabic[i].Name : playerPosition.Name;
                        playerPosition.PlayerPositionLang.Name = positionsInEnglish[i].Name.IsExisting() ? positionsInEnglish[i].Name : playerPosition.PlayerPositionLang.Name;
                    }
                    else if (positionsInArabic[i].Name.IsExisting() &&
                             positionsInEnglish[i].Name.IsExisting())
                    {
                        _unitOfWork.Team.CreatePlayerPosition(new PlayerPosition
                        {
                            Name = positionsInArabic[i].Name,
                            _365_PositionId = positionsInArabic[i].Id.ToString(),
                            PlayerPositionLang = new PlayerPositionLang
                            {
                                Name = positionsInEnglish[i].Name
                            }
                        });
                    }
                    else
                    {
                        _unitOfWork.Team.CreatePlayerPosition(new PlayerPosition
                        {
                            Name = "مدرب",
                            _365_PositionId = positionsInArabic[i].Id.ToString(),
                            PlayerPositionLang = new PlayerPositionLang
                            {
                                Name = "Coach"
                            }
                        });
                    }
                    await _unitOfWork.Save();
                }
            }
        }

        public async Task InsertOrEditPlayers()
        {
            List<TeamModel> teams = _unitOfWork.Team.GetTeams(new TeamParameters
            {
            }, otherLang: false).ToList();

            List<PlayerPositionModel> positions = _unitOfWork.Team.GetPlayerPositions(new PlayerPositionParameters
            {
            }, otherLang: false).ToList();

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
                    if (_unitOfWork.Team.GetPlayers(new PlayerParameters { _365_PlayerId = athletesInArabic[i].Id.ToString() }, otherLang: false).Any())
                    {
                        Player player = await _unitOfWork.Team.FindPlayerby365Id(athletesInArabic[i].Id.ToString(), trackChanges: true);

                        player.Name = athletesInArabic[i].Name.IsExisting() ? athletesInArabic[i].Name : player.Name;
                        player.ShortName = athletesInArabic[i].ShortName.IsExisting() ? athletesInArabic[i].ShortName : player.ShortName;
                        player.Age = athletesInArabic[i].Age;
                        player.PlayerNumber = athletesInArabic[i].JerseyNum.ToString();
                        player.Fk_PlayerPosition = positions.Where(a => a._365_PositionId == athletesInArabic[i].Position.Id.ToString())
                                                     .Select(a => a.Id)
                                                     .First();
                        player.Fk_Team = team.Id;
                        player.PlayerLang.Name = athletesInEnglish[i].Name.IsExisting() ? athletesInEnglish[i].Name : player.PlayerLang.Name;
                        player.PlayerLang.ShortName = athletesInEnglish[i].ShortName.IsExisting() ? athletesInEnglish[i].ShortName : player.PlayerLang.ShortName;

                    }
                    else
                    {
                        _unitOfWork.Team.CreatePlayer(new Player
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
                    await _unitOfWork.Save();
                }
            }

        }

        #endregion

        #region Standings Models

        public async Task InsertOrEditStandings()
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
                int fk_Team = teams.Where(a => a._365_TeamId == row.Competitor.Id.ToString())
                                   .Select(a => a.Id)
                                   .First();

                if (_unitOfWork.Standings.GetStandings(new StandingsParameters { Fk_Season = season.Id, Fk_Team = fk_Team }, otherLang: false).Any())
                {
                    Standings standing = await _unitOfWork.Standings.FindBySeasonAndTeam(season.Id, fk_Team, trackChanges: true);

                    standing.GamePlayed = row.GamePlayed;
                    standing.GamesWon = row.GamesWon;
                    standing.GamesLost = row.GamesLost;
                    standing.GamesEven = row.GamesEven;
                    standing.For = row.For;
                    standing.Against = row.Against;
                    standing.Ratio = row.Ratio;
                    standing.Strike = row.Strike;
                    standing.Position = row.Position;

                }
                else
                {
                    _unitOfWork.Standings.CreateStandings(new Standings
                    {
                        Fk_Season = season.Id,
                        Fk_Team = fk_Team,
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

        #endregion

        #region Season Models

        public async Task InsertOrEditRounds()
        {
            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            List<GameWeakModel> gameWeaks = _unitOfWork.Season.GetGameWeaks(new GameWeakParameters
            {
                Fk_Season = season.Id
            }, otherLang: false).ToList();

            List<int> rounds = GetAllGames().Result.Select(a => a.RoundNum).Distinct().OrderBy(a => a).ToList();

            foreach (int round in rounds)
            {
                if (_unitOfWork.Season.GetGameWeaks(new GameWeakParameters { _365_GameWeakId = round.ToString() }, otherLang: false).Any())
                {
                    GameWeak gameWeak = await _unitOfWork.Season.FindGameWeakby365Id(round.ToString(), trackChanges: true);
                    gameWeak.Name = round.ToString();
                    gameWeak.GameWeakLang.Name = round.ToString();
                }
                else
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
                await _unitOfWork.Save();
            }
        }

        public async Task InsertOrEditGames()
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
                if (_unitOfWork.Season.GetTeamGameWeaks(new TeamGameWeakParameters { _365_MatchId = game.Id.ToString() }, otherLang: false).Any())
                {
                    TeamGameWeak teamGameWeak = await _unitOfWork.Season.FindTeamGameWeakby365Id(game.Id.ToString(), trackChanges: true);

                    teamGameWeak.StartTime = game.StartTimeVal;
                    teamGameWeak.IsEnded = game.IsEnded;
                    teamGameWeak.AwayScore = (int)game.AwayCompetitor.Score;
                    teamGameWeak.HomeScore = (int)game.HomeCompetitor.Score;
                }
                else
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
        }

        #endregion

        public async Task InsertStates()
        {
            List<TeamGameWeakModel> teamGameWeaks = _unitOfWork.Season.GetTeamGameWeaks(new TeamGameWeakParameters
            {
            }, otherLang: false).ToList();

            foreach (TeamGameWeakModel teamGameWeak in teamGameWeaks)
            {
                List<Stat> statsInArabic = new();
                List<Stat> statsInEnglish = new();

                GameReturn gameReturnInArabic = await _365Services.GetGame(new _365GameParameters
                {
                    GameId = int.Parse(teamGameWeak._365_MatchId),
                    IsArabic = true
                });

                GameReturn gameReturnInEnglish = await _365Services.GetGame(new _365GameParameters
                {
                    GameId = int.Parse(teamGameWeak._365_MatchId),
                });

                if (gameReturnInArabic.Game != null)
                {
                    statsInArabic.AddRange(gameReturnInArabic.Game
                                                             .HomeCompetitor
                                                             .Lineups
                                                             .Members
                                                             .Where(a => a.Stats != null && a.Stats.Any())
                                                             .SelectMany(a => a.Stats)
                                                             .GroupBy(a => new Stat
                                                             {
                                                                 Type = a.Type,
                                                                 Name = a.Name,
                                                             })
                                                             .Select(a => a.Key)
                                                             .ToList());

                    statsInArabic.AddRange(gameReturnInArabic.Game
                                                            .AwayCompetitor
                                                            .Lineups
                                                            .Members
                                                            .Where(a => a.Stats != null && a.Stats.Any())
                                                            .SelectMany(a => a.Stats)
                                                            .GroupBy(a => new Stat
                                                            {
                                                                Type = a.Type,
                                                                Name = a.Name,
                                                            })
                                                            .Select(a => a.Key)
                                                            .ToList());

                    statsInArabic = statsInArabic.GroupBy(a => new Stat
                    {
                        Type = a.Type,
                        Name = a.Name,
                    }).Select(a => a.Key).ToList();
                }
                if (gameReturnInEnglish.Game != null)
                {
                    statsInEnglish.AddRange(gameReturnInEnglish.Game
                                                             .HomeCompetitor
                                                             .Lineups
                                                             .Members
                                                             .Where(a => a.Stats != null && a.Stats.Any())
                                                             .SelectMany(a => a.Stats)
                                                              .GroupBy(a => new Stat
                                                              {
                                                                  Type = a.Type,
                                                                  Name = a.Name,
                                                              })
                                                             .Select(a => a.Key)
                                                             .ToList());

                    statsInEnglish.AddRange(gameReturnInEnglish.Game
                                                            .AwayCompetitor
                                                            .Lineups
                                                            .Members
                                                            .Where(a => a.Stats != null && a.Stats.Any())
                                                            .SelectMany(a => a.Stats)
                                                            .GroupBy(a => new Stat
                                                            {
                                                                Type = a.Type,
                                                                Name = a.Name,
                                                            })
                                                            .Select(a => a.Key)
                                                            .ToList());

                    statsInEnglish = statsInEnglish.GroupBy(a => new Stat
                    {
                        Type = a.Type,
                        Name = a.Name,
                    }).Select(a => a.Key).ToList();
                }

                for (int i = 0; i < statsInArabic.Count; i++)
                {
                    _unitOfWork.PlayerScore.CreateScoreType(new ScoreType
                    {
                        Name = statsInArabic[i].Name,
                        _365_TypeId = statsInArabic[i].Type.ToString(),
                        ScoreTypeLang = new ScoreTypeLang
                        {
                            Name = statsInEnglish[i].Name,
                        }
                    });
                    await _unitOfWork.Save();
                }
            }
        }

        public async Task InsertGameResult()
        {
            List<TeamGameWeakModel> teamGameWeaks = _unitOfWork.Season.GetTeamGameWeaks(new TeamGameWeakParameters
            {
            }, otherLang: false).ToList();

            List<ScoreTypeModel> scoreTypes = _unitOfWork.PlayerScore.GetScoreTypes(new ScoreTypeParameters(), false).ToList();

            foreach (TeamGameWeakModel teamGameWeak in teamGameWeaks)
            {
                GameReturn gameReturn = await _365Services.GetGame(new _365GameParameters
                {
                    GameId = int.Parse(teamGameWeak._365_MatchId)
                });

                if (gameReturn.Game != null)
                {
                    List<GameMember> allMembers = gameReturn.Game.Members;

                    IQueryable<PlayerModel> playersQuary = _unitOfWork.Team.GetPlayers(new PlayerParameters
                    {
                        _365_PlayerIds = allMembers.Select(a => a.AthleteId.ToString()).ToList(),
                    }, otherLang: false);

                    if (playersQuary.Any())
                    {
                        var players = playersQuary.Select(a => new
                        {
                            a.Id,
                            a._365_PlayerId,
                        }).ToList();

                        TeamGameWeak match = await _unitOfWork.Season.FindTeamGameWeakbyId(teamGameWeak.Id, trackChanges: true);
                        match.IsEnded = gameReturn.Game.IsEnded;
                        match.AwayScore = (int)gameReturn.Game.AwayCompetitor.Score;
                        match.HomeScore = (int)gameReturn.Game.HomeCompetitor.Score;
                        await _unitOfWork.Save();

                        List<Member> allMembersResults = new();
                        allMembersResults.AddRange(gameReturn.Game.HomeCompetitor.Lineups.Members);
                        allMembersResults.AddRange(gameReturn.Game.AwayCompetitor.Lineups.Members);

                        foreach (GameMember member in allMembers)
                        {
                            int fk_Player = players.Where(a => a._365_PlayerId == member.AthleteId.ToString()).Select(a => a.Id).FirstOrDefault();
                            Member memberResult = allMembersResults.Where(a => a.Id == member.Id).FirstOrDefault();

                            if (fk_Player > 0 && memberResult != null)
                            {
                                if (_unitOfWork.PlayerScore.GetPlayerGameWeaks(new PlayerGameWeakParameters { Fk_TeamGameWeak = teamGameWeak.Id, Fk_Player = fk_Player }, false).Any())
                                {
                                    var fk_TeamGameWeak = _unitOfWork.PlayerScore.GetPlayerGameWeaks(new PlayerGameWeakParameters { Fk_TeamGameWeak = teamGameWeak.Id, Fk_Player = fk_Player }, false).Select(a => a.Id).First();
                                    foreach (var Stat in memberResult.Stats)
                                    {
                                        _unitOfWork.PlayerScore.CreatePlayerGameWeakScore(new PlayerGameWeakScore
                                        {
                                            Fk_PlayerGameWeak = fk_TeamGameWeak,
                                            Fk_ScoreType = scoreTypes.Where(a => a._365_TypeId == Stat.Type.ToString()).Select(a => a.Id).First(),
                                            Value = Stat.Value,
                                        });
                                    }
                                }
                                else
                                {
                                    PlayerGameWeak playerGameWeak = new()
                                    {
                                        Fk_TeamGameWeak = teamGameWeak.Id,
                                        Fk_Player = fk_Player,
                                        Ranking = memberResult.Ranking
                                    };
                                    if (memberResult.Stats != null && memberResult.Stats.Any())
                                    {
                                        playerGameWeak.PlayerGameWeakScores = new List<PlayerGameWeakScore>();

                                        foreach (var Stat in memberResult.Stats)
                                        {
                                            playerGameWeak.PlayerGameWeakScores.Add(new PlayerGameWeakScore
                                            {
                                                Fk_ScoreType = scoreTypes.Where(a => a._365_TypeId == Stat.Type.ToString()).Select(a => a.Id).First(),
                                                Value = Stat.Value,
                                            });
                                        }
                                    }
                                    _unitOfWork.PlayerScore.CreatePlayerGameWeak(playerGameWeak);
                                }
                            }
                        }
                        await _unitOfWork.Save();
                    }
                }
            }
        }

        #region Games helper method

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
