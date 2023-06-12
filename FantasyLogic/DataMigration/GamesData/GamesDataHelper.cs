using Contracts.Services;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.NotificationModels;
using Entities.DBModels.SeasonModels;
using Entities.ServicesModels;
using FantasyLogic.Calculations;
using FantasyLogic.DataMigration.PlayerScoreData;
using IntegrationWith365.Entities.GamesModels;
using IntegrationWith365.Helpers;
using Services;
using static Entities.EnumData.LogicEnumData;

namespace FantasyLogic.DataMigration.GamesData
{
    public class GamesDataHelper
    {
        private readonly _365Services _365Services;
        private readonly UnitOfWork _unitOfWork;
        private readonly GamesHelper _gamesHelper;
        private readonly AccountTeamCalc _accountTeamCalc;
        private readonly IFirebaseNotificationManager _notificationManager;

        public GamesDataHelper(
            UnitOfWork unitOfWork,
            _365Services _365Services,
            IFirebaseNotificationManager firebaseNotificationManager)
        {
            this._365Services = _365Services;
            _unitOfWork = unitOfWork;

            _gamesHelper = new GamesHelper(unitOfWork, _365Services);

            _accountTeamCalc = new(_unitOfWork);

            _notificationManager = firebaseNotificationManager;
        }

        public void RunUpdateGames()
        {
            List<TeamModelForCalc> teams = _unitOfWork.Team.GetTeams(new TeamParameters { })
                                             .Select(a => new TeamModelForCalc
                                             {
                                                 Id = a.Id,
                                                 _365_TeamId = a._365_TeamId
                                             }).ToList();

            _ = BackgroundJob.Enqueue(() => UpdateGames(teams));
        }

        public void UpdateGames(List<TeamModelForCalc> teams)
        {
            SeasonModelForCalc season = _unitOfWork.Season.GetCurrentSeason();

            List<GameWeakModelForCalc> gameWeaks = _unitOfWork.Season.GetGameWeaks(new GameWeakParameters
            {
                Fk_Season = season.Id
            }).Select(a => new GameWeakModelForCalc
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
                    jobId = jobId.IsExisting()
                        ? BackgroundJob.ContinueJobWith(jobId, () => UpdateGame(game, fk_Home, fk_Away, fk_GameWeak))
                        : BackgroundJob.Enqueue(() => UpdateGame(game, fk_Home, fk_Away, fk_GameWeak));
                }
            }

            jobId = jobId.IsExisting()
                    ? BackgroundJob.ContinueJobWith(jobId, () => UpdateGameWeakDeadline(season.Id))
                    : BackgroundJob.Enqueue(() => UpdateGameWeakDeadline(season.Id));
        }

        public async Task UpdateGame(Games game, int fk_Home, int fk_Away, int fk_GameWeak)
        {
            DateTime startTime = game.StartTimeVal;

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
                string secondJobId = BackgroundJob.Schedule(() => gameResultDataHelper.RunUpdateGameResult(new TeamGameWeakParameters { _365_MatchId = game.Id.ToString() }, true, false, false, false), startTime.AddMinutes(120));
                string thirdJobId = BackgroundJob.Schedule(() => gameResultDataHelper.RunUpdateGameResult(new TeamGameWeakParameters { _365_MatchId = game.Id.ToString() }, true, false, false, true), startTime.AddMinutes(200));

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
            }).GroupBy(a => a.Fk_GameWeak)
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
                    if (gameWeak.SecondJobId.IsExisting())
                    {
                        _ = BackgroundJob.Delete(gameWeak.SecondJobId);
                    }

                    if (deadline > DateTime.UtcNow)
                    {
                        gameWeak.JobId = BackgroundJob.Schedule(() => UpdateCurrentGameWeak(gameWeak.Id, false, 0, false, false), deadline);

                        gameWeak.SecondJobId = BackgroundJob.Schedule(() => SendNotificationForGameWeakDeadLine(gameWeak.Id), deadline.AddMinutes(-60));
                    }

                    gameWeak.Deadline = deadline;

                    await _unitOfWork.Save();
                }
            }
        }
        public async Task UpdateCurrentGameWeak(int id)
        {
            await UpdateCurrentGameWeak(id, resetTeam: false);
        }
        public async Task UpdateCurrentGameWeak(int id, bool resetTeam, int fk_AccounTeam = 0, bool inDebug = false, bool skipReset = false)
        {
            if (!skipReset)
            {
                _unitOfWork.Season.ResetCurrentGameWeaks();
            }

            GameWeak gameWeak = await _unitOfWork.Season.FindGameWeakbyId(id, trackChanges: !skipReset);
            if (!skipReset)
            {
                gameWeak.IsCurrent = true;
            }

            GameWeak nextGameWeak = await _unitOfWork.Season.FindGameWeakby365Id((gameWeak._365_GameWeakId.ParseToInt() + 1).ToString(), gameWeak.Fk_Season, trackChanges: !skipReset);
            if (!skipReset && nextGameWeak != null)
            {
                nextGameWeak.IsNext = true;
            }

            if (!skipReset)
            {
                GameWeak prevGameWeak = await _unitOfWork.Season.FindGameWeakby365Id((gameWeak._365_GameWeakId.ParseToInt() - 1).ToString(), gameWeak.Fk_Season, trackChanges: !skipReset);
                if (prevGameWeak != null)
                {
                    prevGameWeak.IsPrev = true;
                }
            }

            if (!skipReset)
            {
                await _unitOfWork.Save();
            }

            if (inDebug)
            {
                TransferAccountTeamPlayers(nextGameWeak.Id, gameWeak.Id, gameWeak._365_GameWeakId.ParseToInt(), nextGameWeak.Fk_Season, resetTeam, fk_AccounTeam, inDebug);
            }
            else
            {
                _ = BackgroundJob.Enqueue(() => TransferAccountTeamPlayers(nextGameWeak.Id, gameWeak.Id, gameWeak._365_GameWeakId.ParseToInt(), nextGameWeak.Fk_Season, resetTeam, fk_AccounTeam, inDebug));
            }
        }

        public async Task SendNotificationForGameWeakDeadLine(int id)
        {
            Notification notification = new()
            {
                Title = "باقي على نهاية الجوله ساعه",
                Description = " إلحق ضبط تشكيلتك",
                OpenType = NotificationOpenTypeEnum.DeadLine,
                OpenValue = id.ToString(),
                NotificationLang = new NotificationLang
                {
                    Title = "باقي على نهاية الجوله ساعه",
                    Description = " إلحق ضبط تشكيلتك",
                }
            };
            _unitOfWork.Notification.CreateNotification(notification);
            await _unitOfWork.Save();

            _notificationManager.SendToTopic(new FirebaseNotificationModel
            {
                MessageHeading = notification.Title,
                MessageContent = notification.Description,
                ImgUrl = notification.StorageUrl + notification.ImageUrl,
                OpenType = notification.OpenType.ToString(),
                OpenValue = notification.OpenValue,
                Topic = "all"
            }).Wait();
        }

        public void TransferAccountTeamPlayers(int fk_CurrentGameWeak, int fk_PrevGameWeak, int prev_365_GameWeakId, int fk_Season, bool resetTeam, int fk_AccounTeam, bool inDebug)
        {
            List<int> accounTeams = _unitOfWork.AccountTeam
                                         .GetAccountTeams(new AccountTeamParameters
                                         {
                                             Id = fk_AccounTeam
                                         })
                                         .Select(a => a.Id)
                                         .ToList();

            foreach (int accounTeam in accounTeams)
            {
                if (inDebug)
                {
                    TransferAccountTeamPlayers(accounTeam, fk_CurrentGameWeak, fk_PrevGameWeak, prev_365_GameWeakId, fk_Season, resetTeam).Wait();
                }
                else
                {
                    BackgroundJob.Enqueue(() => TransferAccountTeamPlayers(accounTeam, fk_CurrentGameWeak, fk_PrevGameWeak, prev_365_GameWeakId, fk_Season, resetTeam));
                }
            }
        }

        public async Task TransferAccountTeamPlayers(int fk_AccounTeam, int fk_CurrentGameWeak, int fk_PrevGameWeak, int prev_365_GameWeakId, int fk_Season, bool resetTeam)
        {
            int players = _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new AccountTeamPlayerGameWeakParameters
            {
                Fk_GameWeak = fk_PrevGameWeak,
                Fk_AccountTeam = fk_AccounTeam,
                IsTransfer = false
            }).Count();

            if (players != 0)
            {
                if (!_unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
                {
                    Fk_GameWeak = fk_CurrentGameWeak,
                    Fk_AccountTeam = fk_AccounTeam
                }).Any())
                {
                    _unitOfWork.AccountTeam.CreateAccountTeamGameWeak(new AccountTeamGameWeak
                    {
                        Fk_GameWeak = fk_CurrentGameWeak,
                        Fk_AccountTeam = fk_AccounTeam,
                    });
                    await _unitOfWork.Save();
                }

                AccountTeamGameWeakModelForCalc accountTeamGameWeakModel = _unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
                {
                    Fk_GameWeak = fk_CurrentGameWeak,
                    Fk_AccountTeam = fk_AccounTeam
                }).Select(a => new AccountTeamGameWeakModelForCalc
                {
                    Id = a.Id,
                    FreeHit = a.FreeHit,
                }).FirstOrDefault();

                if (accountTeamGameWeakModel != null)
                {
                    bool freeHit = false;

                    if (_unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
                    {
                        Fk_GameWeak = fk_PrevGameWeak,
                        Fk_AccountTeam = fk_AccounTeam,
                        FreeHit = true,
                    }).Any())
                    {
                        freeHit = true;
                    }

                    if (resetTeam ||
                        accountTeamGameWeakModel.FreeHit ||
                        _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new AccountTeamPlayerGameWeakParameters
                        {
                            Fk_GameWeak = fk_CurrentGameWeak,
                            Fk_AccountTeam = fk_AccounTeam,
                            IsTransfer = false
                        }).Count() < 15)
                    {
                        List<AccountTeamPlayerGameWeakModel> prevPlayers = new();

                        do
                        {
                            prevPlayers = _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new AccountTeamPlayerGameWeakParameters
                            {
                                Fk_GameWeak = fk_PrevGameWeak,
                                Fk_AccountTeam = fk_AccounTeam,
                                IsTransfer = false
                            })
                            .Select(a => new AccountTeamPlayerGameWeakModel
                            {
                                Fk_AccountTeamPlayer = a.Fk_AccountTeamPlayer,
                                Fk_TeamPlayerType = a.Fk_TeamPlayerType,
                                IsPrimary = a.IsPrimary,
                                Order = a.Order,
                                Points = a.Points,
                                PlayerName = a.AccountTeamPlayer.Player.Name
                            })
                            .ToList();

                            if (freeHit || prevPlayers.Count < 15)
                            {
                                freeHit = false;
                                prev_365_GameWeakId--;

                                fk_PrevGameWeak = _unitOfWork.Season.GetGameWeaks(new GameWeakParameters
                                {
                                    _365_GameWeakId = prev_365_GameWeakId.ToString()
                                }).Select(a => a.Id).FirstOrDefault();
                            }
                            else
                            {
                                fk_PrevGameWeak = 0;
                            }

                        } while (fk_PrevGameWeak > 0);

                        if (prevPlayers.Any())
                        {
                            _unitOfWork.AccountTeam.ResetTeamPlayers(fk_AccounTeam, fk_CurrentGameWeak, accountTeamGameWeakModel.Id);
                            await _unitOfWork.Save();

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


    public class GameWeakModelForCalc
    {
        public int Id { get; set; }

        public string _365_GameWeakId { get; set; }
    }

    public class TeamModelForCalc
    {
        public int Id { get; set; }

        public string _365_TeamId { get; set; }
    }
}
