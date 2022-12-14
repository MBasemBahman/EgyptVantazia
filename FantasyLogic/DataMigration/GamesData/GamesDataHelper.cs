using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.SeasonModels;
using FantasyLogic.Calculations;
using FantasyLogic.DataMigration.PlayerScoreData;
using FantasyLogic.DataMigration.StandingsData;
using IntegrationWith365.Entities.GamesModels;
using IntegrationWith365.Helpers;

namespace FantasyLogic.DataMigration.GamesData
{
    public class GamesDataHelper
    {
        private readonly _365Services _365Services;
        private readonly UnitOfWork _unitOfWork;
        private readonly GamesHelper _gamesHelper;

        public GamesDataHelper(UnitOfWork unitOfWork, _365Services _365Services)
        {
            this._365Services = _365Services;
            _unitOfWork = unitOfWork;
            _gamesHelper = new GamesHelper(unitOfWork, _365Services);
        }

        public void RunUpdateGames()
        {
            List<TeamDto> teams = _unitOfWork.Team.GetTeams(new TeamParameters
            {
            }, otherLang: false).Select(a => new TeamDto
            {
                Id = a.Id,
                _365_TeamId = a._365_TeamId
            }).ToList();

            //UpdateGames(teams);
            _ = BackgroundJob.Enqueue(() => UpdateGames(teams));
        }

        public void UpdateGames(List<TeamDto> teams)
        {
            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            List<GameWeakDto> gameWeaks = _unitOfWork.Season.GetGameWeaks(new GameWeakParameters
            {
                Fk_Season = season.Id
            }, otherLang: false).Select(a => new GameWeakDto
            {
                Id = a.Id,
                _365_GameWeakId = a._365_GameWeakId
            }).ToList();

            List<Games> games = _gamesHelper.GetAllGames(season._365_SeasonId.ParseToInt()).Result.OrderBy(a => a.RoundNum).ThenBy(a => a.StartTimeVal).ToList();

            string jobId = null;

            foreach (Games game in games)
            {
                int fk_Home = teams.Where(a => a._365_TeamId == game.HomeCompetitor.Id.ToString()).Select(a => a.Id).FirstOrDefault();
                int fk_Away = teams.Where(a => a._365_TeamId == game.AwayCompetitor.Id.ToString()).Select(a => a.Id).FirstOrDefault();
                int fk_GameWeak = gameWeaks.Where(a => a._365_GameWeakId == game.RoundNum.ToString()).Select(a => a.Id).FirstOrDefault();

                if (fk_GameWeak > 0)
                {
                    //UpdateGame(game, fk_Home, fk_Away, fk_GameWeak).Wait();

                    jobId = jobId.IsExisting()
                        ? BackgroundJob.ContinueJobWith(jobId, () => UpdateGame(game, fk_Home, fk_Away, fk_GameWeak))
                        : BackgroundJob.Enqueue(() => UpdateGame(game, fk_Home, fk_Away, fk_GameWeak));
                }
            }

            //UpdateGameWeakDeadline(season.Id).Wait();

            jobId = jobId.IsExisting()
                    ? BackgroundJob.ContinueJobWith(jobId, () => UpdateGameWeakDeadline(season.Id))
                    : BackgroundJob.Enqueue(() => UpdateGameWeakDeadline(season.Id));
        }

        public async Task UpdateGame(Games game, int fk_Home, int fk_Away, int fk_GameWeak)
        {
            DateTime startTime = game.StartTimeVal.AddHours(2);
            bool isDelayed = false;

            GameWeak checkGameWeek = _unitOfWork.Season.GetGameWeak(startTime);

            if (checkGameWeek != null && checkGameWeek.Id != fk_GameWeak)
            {
                fk_GameWeak = checkGameWeek.Id;
                isDelayed = true;
            }

            _unitOfWork.Season.CreateTeamGameWeak(new TeamGameWeak
            {
                Fk_Away = fk_Away,
                Fk_Home = fk_Home,
                Fk_GameWeak = fk_GameWeak,
                StartTime = startTime,
                IsEnded = game.IsEnded,
                _365_MatchId = game.Id.ToString(),
                AwayScore = (int)game.AwayCompetitor.Score,
                HomeScore = (int)game.HomeCompetitor.Score,
                IsDelayed = isDelayed
            });
            await _unitOfWork.Save();

            if (startTime > DateTime.UtcNow.AddHours(2) && startTime < DateTime.UtcNow.AddDays(10))
            {
                StandingsDataHelper standingsDataHelper = new(_unitOfWork, _365Services);
                GameResultDataHelper gameResultDataHelper = new(_unitOfWork, _365Services);
                PlayerStateCalc playerStateCalc = new(_unitOfWork);
                AccountTeamCalc accountTeamCalc = new(_unitOfWork);
                PrivateLeagueClac privateLeagueClac = new(_unitOfWork);

                string updateGameResultTime = startTime.AddHours(-2).ToCronExpression(startTime.AddMinutes(60), 5);

                string updateGameWeakResultTime = startTime.ToCronExpression(startTime.AddHours(4), 360);

                string updateStandingsTime = startTime.AddHours(-2).ToCronExpression(startTime.AddMinutes(60), 10);
                string playersStateCalculationsTime = startTime.AddHours(-2).ToCronExpression(startTime.AddMinutes(60), 15);
                string accountTeamCalculationsTime = startTime.AddHours(-2).ToCronExpression(startTime.AddMinutes(60), 20);
                string privateLeagueCalculationsTime = startTime.AddHours(-2).ToCronExpression(startTime.AddMinutes(60), 30);

                RecurringJob.AddOrUpdate("UpdateGameResult-" + game.Id.ToString(), () => gameResultDataHelper.RunUpdateGameResult(new TeamGameWeakParameters { _365_MatchId = game.Id.ToString() }), updateGameResultTime, TimeZoneInfo.Utc);

                RecurringJob.AddOrUpdate("UpdateGameWeakResult-" + fk_GameWeak.ToString(), () => gameResultDataHelper.RunUpdateGameResult(new TeamGameWeakParameters { Fk_GameWeak = fk_GameWeak }), updateGameWeakResultTime, TimeZoneInfo.Utc);

                RecurringJob.AddOrUpdate("UpdateGameWeakDailyResult-" + fk_GameWeak.ToString(), () => gameResultDataHelper.RunUpdateGameResult(new TeamGameWeakParameters { Fk_GameWeak = fk_GameWeak }), "0 6 * * *", TimeZoneInfo.Utc);

                RecurringJob.AddOrUpdate("UpdateStandings-" + game.Id.ToString(), () => standingsDataHelper.RunUpdateStandings(), updateStandingsTime, TimeZoneInfo.Utc);

                RecurringJob.AddOrUpdate("PlayersStateCalculations-" + game.Id.ToString(), () => playerStateCalc.RunPlayersStateCalculations(fk_GameWeak, game.Id.ToString()), playersStateCalculationsTime, TimeZoneInfo.Utc);

                RecurringJob.AddOrUpdate("AccountTeamCalculations-" + game.Id.ToString(), () => accountTeamCalc.RunAccountTeamsCalculations(fk_GameWeak, 0, false), accountTeamCalculationsTime, TimeZoneInfo.Utc);

                RecurringJob.AddOrUpdate("PrivateLeagueCalculations-" + game.Id.ToString(), () => privateLeagueClac.RunPrivateLeaguesRanking(), privateLeagueCalculationsTime, TimeZoneInfo.Utc);
            }
        }

        public async Task UpdateGameWeakDeadline(int fk_Season)
        {
            var teamGameWeaks = _unitOfWork.Season.GetTeamGameWeaks(new TeamGameWeakParameters
            {
                Fk_Season = fk_Season
            }, otherLang: false)
                .GroupBy(a => a.Fk_GameWeak)
                .Select(a => new
                {
                    Fk_GameWeak = a.Key,
                    Deadline = a.Select(b => b.StartTime).OrderBy(b => b).FirstOrDefault()
                })
                .ToList();

            foreach (var teamGameWeak in teamGameWeaks)
            {
                if (teamGameWeak.Deadline > DateTime.MinValue)
                {
                    DateTime deadline = teamGameWeak.Deadline.AddMinutes(-90);

                    GameWeak gameWeak = await _unitOfWork.Season.FindGameWeakbyId(teamGameWeak.Fk_GameWeak, trackChanges: true);

                    if (gameWeak.JobId.IsExisting())
                    {
                        _ = BackgroundJob.Delete(gameWeak.JobId);
                    }

                    if (deadline > DateTime.UtcNow)
                    {
                        gameWeak.JobId = BackgroundJob.Schedule(() => UpdateCurrentGameWeak(gameWeak.Id), deadline.AddHours(-2));
                    }

                    gameWeak.Deadline = deadline;

                    await _unitOfWork.Save();
                }
            }
        }

        public async Task UpdateCurrentGameWeak(int id)
        {
            _unitOfWork.Season.ResetCurrentGameWeaks();
            await _unitOfWork.Save();

            GameWeak gameWeak = await _unitOfWork.Season.FindGameWeakbyId(id, trackChanges: true);
            gameWeak.IsCurrent = true;
            await _unitOfWork.Save();

            GameWeak nextGameWeak = await _unitOfWork.Season.FindGameWeakby365Id((gameWeak._365_GameWeakId.ParseToInt() + 1).ToString(), gameWeak.Fk_Season, trackChanges: true);
            if (nextGameWeak != null)
            {
                nextGameWeak.IsNext = true;
                await _unitOfWork.Save();
            }

            GameWeak prevGameWeak = await _unitOfWork.Season.FindGameWeakby365Id((gameWeak._365_GameWeakId.ParseToInt() - 1).ToString(), gameWeak.Fk_Season, trackChanges: true);
            if (prevGameWeak != null)
            {
                prevGameWeak.IsPrev = true;
                await _unitOfWork.Save();
            }

            //TransferAccountTeamPlayers(nextGameWeak.Id, gameWeak.Id, gameWeak._365_GameWeakId.ParseToInt(), nextGameWeak.Fk_Season);
            _ = BackgroundJob.Enqueue(() => TransferAccountTeamPlayers(nextGameWeak.Id, gameWeak.Id, gameWeak._365_GameWeakId.ParseToInt(), nextGameWeak.Fk_Season));
        }

        public void TransferAccountTeamPlayers(int fk_CurrentGameWeak, int fk_PrevGameWeak, int prev_365_GameWeakId, int fk_Season)
        {
            List<int> accounTeams = _unitOfWork.AccountTeam
                                         .GetAccountTeams(new AccountTeamParameters(), otherLang: false)
                                         .Select(a => a.Id)
                                         .ToList();

            string jobId = "";
            foreach (int accounTeam in accounTeams)
            {
                //TransferAccountTeamPlayers(accounTeam, fk_CurrentGameWeak, fk_PrevGameWeak, prev_365_GameWeakId, fk_Season).Wait();

                jobId = jobId.IsExisting()
                        ? BackgroundJob.ContinueJobWith(jobId, () => TransferAccountTeamPlayers(accounTeam, fk_CurrentGameWeak, fk_PrevGameWeak, prev_365_GameWeakId, fk_Season))
                        : BackgroundJob.Enqueue(() => TransferAccountTeamPlayers(accounTeam, fk_CurrentGameWeak, fk_PrevGameWeak, prev_365_GameWeakId, fk_Season));
            }
        }

        public async Task TransferAccountTeamPlayers(int fk_AccounTeam, int fk_CurrentGameWeak, int fk_PrevGameWeak, int prev_365_GameWeakId, int fk_Season)
        {
            int players = _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new AccountTeamPlayerGameWeakParameters
            {
                Fk_GameWeak = fk_PrevGameWeak,
                Fk_AccountTeam = fk_AccounTeam,
                IsTransfer = false
            }, otherLang: false).Count();

            if (players != 0)
            {
                if (!_unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
                {
                    Fk_GameWeak = fk_CurrentGameWeak,
                    Fk_AccountTeam = fk_AccounTeam
                }, otherLang: false).Any())
                {
                    _unitOfWork.AccountTeam.CreateAccountTeamGameWeak(new AccountTeamGameWeak
                    {
                        Fk_GameWeak = fk_CurrentGameWeak,
                        Fk_AccountTeam = fk_AccounTeam,
                    });
                    await _unitOfWork.Save();
                }

                AccountTeamGameWeakModel accountTeamGameWeakModel = _unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
                {
                    Fk_GameWeak = fk_CurrentGameWeak,
                    Fk_AccountTeam = fk_AccounTeam
                }, otherLang: false).FirstOrDefault();

                if (accountTeamGameWeakModel != null)
                {
                    if (accountTeamGameWeakModel.FreeHit ||
                        _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new AccountTeamPlayerGameWeakParameters
                        {
                            Fk_GameWeak = fk_CurrentGameWeak,
                            Fk_AccountTeam = fk_AccounTeam,
                            IsTransfer = false
                        }, otherLang: false).Count() < 15)
                    {
                        List<AccountTeamPlayerGameWeakModel> prevPlayers = new();

                        do
                        {
                            prevPlayers = _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new AccountTeamPlayerGameWeakParameters
                            {
                                Fk_GameWeak = fk_PrevGameWeak,
                                Fk_AccountTeam = fk_AccounTeam,
                                IsTransfer = false
                            }, otherLang: false)
                            .Select(a => new AccountTeamPlayerGameWeakModel
                            {
                                Fk_AccountTeamPlayer = a.Fk_AccountTeamPlayer,
                                Fk_TeamPlayerType = a.Fk_TeamPlayerType,
                                IsPrimary = a.IsPrimary,
                                Order = a.Order,
                                Points = a.Points,
                            })
                            .ToList();

                            if (prevPlayers.Count < 15)
                            {
                                prev_365_GameWeakId--;

                                fk_PrevGameWeak = _unitOfWork.Season.GetGameWeaks(new GameWeakParameters
                                {
                                    _365_GameWeakId = prev_365_GameWeakId.ToString()
                                }, otherLang: false)
                                    .Select(a => a.Id)
                                    .FirstOrDefault();
                            }
                            else
                            {
                                fk_PrevGameWeak = 0;
                            }

                        } while (fk_PrevGameWeak > 0);

                        if (prevPlayers.Any())
                        {
                            foreach (AccountTeamPlayerGameWeakModel player in prevPlayers)
                            {
                                _unitOfWork.AccountTeam.CreateAccountTeamPlayerGameWeak(new AccountTeamPlayerGameWeak
                                {
                                    Fk_AccountTeamPlayer = player.Fk_AccountTeamPlayer,
                                    Fk_GameWeak = fk_CurrentGameWeak,
                                    Fk_TeamPlayerType = player.Fk_TeamPlayerType,
                                    IsPrimary = player.IsPrimary,
                                    Order = player.Order
                                });
                            }
                            await _unitOfWork.Save();
                        }
                    }
                }
            }
        }
    }


    public class GameWeakDto
    {
        public int Id { get; set; }

        public string _365_GameWeakId { get; set; }
    }

    public class TeamDto
    {
        public int Id { get; set; }

        public string _365_TeamId { get; set; }
    }
}
