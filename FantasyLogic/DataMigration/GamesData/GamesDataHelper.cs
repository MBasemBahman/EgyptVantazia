using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.SeasonModels;
using FantasyLogic.Calculations;
using FantasyLogic.DataMigration.PlayerScoreData;
using FantasyLogic.SharedLogic;
using IntegrationWith365.Entities.GamesModels;
using IntegrationWith365.Helpers;

namespace FantasyLogic.DataMigration.GamesData
{
    public class GamesDataHelper
    {
        private readonly _365Services _365Services;
        private readonly UnitOfWork _unitOfWork;
        private readonly GamesHelper _gamesHelper;
        private readonly AccountTeamCalc _accountTeamCalc;

        public GamesDataHelper(
            UnitOfWork unitOfWork,
            _365Services _365Services)
        {
            this._365Services = _365Services;
            _unitOfWork = unitOfWork;

            _gamesHelper = new GamesHelper(unitOfWork, _365Services);

            _accountTeamCalc = new(_unitOfWork);
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

            List<Games> games = _gamesHelper.GetAllGames(season._365_SeasonId.ParseToInt()).Result.OrderByDescending(a => a.StartTimeVal).ToList();

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
                else
                {

                }
            }

            //UpdateGameWeakDeadline(season.Id).Wait();

            jobId = jobId.IsExisting()
                    ? BackgroundJob.ContinueJobWith(jobId, () => UpdateGameWeakDeadline(season.Id))
                    : BackgroundJob.Enqueue(() => UpdateGameWeakDeadline(season.Id));
        }

        public async Task UpdateGame(Games game, int fk_Home, int fk_Away, int fk_GameWeak)
        {
            DateTime startTime = game.StartTimeVal;

            //GameWeak checkGameWeek = _unitOfWork.Season.GetGameWeak(startTime);

            //if (checkGameWeek != null && checkGameWeek.Id != fk_GameWeak)
            //{
            //    fk_GameWeak = checkGameWeek.Id;
            //    isDelayed = true;
            //}

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
            });
            await _unitOfWork.Save();

            if (startTime > DateTime.UtcNow.ToEgypt())
            {
                TeamGameWeak match = await _unitOfWork.Season.FindTeamGameWeakby365Id(game.Id.ToString(), trackChanges: true);
                if (match.JobId.IsExisting())
                {
                    _ = BackgroundJob.Delete(match.JobId);
                }

                if (match.SecondJobId.IsExisting())
                {
                    _ = BackgroundJob.Delete(match.SecondJobId);
                }

                if (match.ThirdJobId.IsExisting())
                {
                    _ = BackgroundJob.Delete(match.ThirdJobId);
                }

                GameResultDataHelper gameResultDataHelper = new(_unitOfWork, _365Services);

                string jobId = BackgroundJob.Schedule(() => gameResultDataHelper.RunUpdateGameResult(new TeamGameWeakParameters { _365_MatchId = game.Id.ToString() }, false, false, false, false), startTime);
                string secondJobId = BackgroundJob.Schedule(() => gameResultDataHelper.RunUpdateGameResult(new TeamGameWeakParameters { _365_MatchId = game.Id.ToString() }, true, false, false, false), startTime.AddMinutes(90));
                string thirdJobId = BackgroundJob.Schedule(() => gameResultDataHelper.RunUpdateGameResult(new TeamGameWeakParameters { _365_MatchId = game.Id.ToString() }, true, false, false, false), startTime.AddMinutes(120));
                BackgroundJob.Schedule(() => gameResultDataHelper.RunUpdateGameResult(new TeamGameWeakParameters { _365_MatchId = game.Id.ToString() }, false, false, false, true), startTime.AddMinutes(123));

                match.JobId = jobId;
                match.SecondJobId = secondJobId;
                match.ThirdJobId = thirdJobId;

                _unitOfWork.Save().Wait();
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
                    Deadline = a.Select(b => b.StartTime).OrderBy(b => b).FirstOrDefault(), // بداية الجوله
                    EndTime = a.Select(b => b.StartTime).OrderByDescending(b => b).FirstOrDefault() // نهاية الجوله
                })
                .ToList();

            foreach (var teamGameWeak in teamGameWeaks)
            {
                if (teamGameWeak.Deadline > DateTime.MinValue)
                {
                    DateTime deadline = teamGameWeak.Deadline.AddMinutes(-60);

                    GameWeak gameWeak = await _unitOfWork.Season.FindGameWeakbyId(teamGameWeak.Fk_GameWeak, trackChanges: true);

                    if (gameWeak.JobId.IsExisting())
                    {
                        _ = BackgroundJob.Delete(gameWeak.JobId);
                    }

                    if (deadline > DateTime.UtcNow)
                    {
                        gameWeak.JobId = BackgroundJob.Schedule(() => UpdateCurrentGameWeak(gameWeak.Id), deadline);
                    }

                    gameWeak.Deadline = deadline;

                    await _unitOfWork.Save();
                }
                if (teamGameWeak.EndTime > DateTime.MinValue)
                {
                    DateTime endTime = teamGameWeak.EndTime.AddHours(6);

                    GameWeak gameWeak = await _unitOfWork.Season.FindGameWeakbyId(teamGameWeak.Fk_GameWeak, trackChanges: true);

                    if (gameWeak.EndTimeJobId.IsExisting())
                    {
                        _ = BackgroundJob.Delete(gameWeak.EndTimeJobId);
                    }

                    if (endTime > DateTime.UtcNow)
                    {
                        gameWeak.EndTimeJobId = BackgroundJob.Schedule(() => _accountTeamCalc.RunAccountTeamsCalculations(gameWeak.Id, 0, null, false), endTime);
                    }

                    gameWeak.EndTime = endTime;

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
                    bool freeHit = accountTeamGameWeakModel.FreeHit;

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

                            if (freeHit || prevPlayers.Count < 15)
                            {
                                freeHit = false;
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
