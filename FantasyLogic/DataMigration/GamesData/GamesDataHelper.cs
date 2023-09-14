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
using IntegrationWith365.Entities.GameModels;
using IntegrationWith365.Entities.GamesModels;
using IntegrationWith365.Helpers;
using Services;
using static Contracts.EnumData.DBModelsEnum;
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

        public void RunUpdateGames(_365CompetitionsEnum _365CompetitionsEnum)
        {
            List<TeamModelForCalc> teams = _unitOfWork.Team.GetTeams(new TeamParameters
            {
                _365_CompetitionsId = (int)_365CompetitionsEnum
            }).Select(a => new TeamModelForCalc
            {
                Id = a.Id,
                _365_TeamId = a._365_TeamId
            }).ToList();

            _ = BackgroundJob.Enqueue(() => UpdateGames(_365CompetitionsEnum, teams));
        }

        public void UpdateGames(_365CompetitionsEnum _365CompetitionsEnum, List<TeamModelForCalc> teams)
        {
            SeasonModelForCalc season = _unitOfWork.Season.GetCurrentSeason(_365CompetitionsEnum);

            List<GameWeakModelForCalc> gameWeaks = _unitOfWork.Season.GetGameWeaks(new GameWeakParameters
            {
                Fk_Season = season.Id
            }).Select(a => new GameWeakModelForCalc
            {
                Id = a.Id,
                _365_GameWeakId = a._365_GameWeakId
            }).ToList();

            List<Games> games = _gamesHelper.GetAllGames(_365CompetitionsEnum, season._365_SeasonId.ParseToInt()).Result.OrderByDescending(a => a.StartTimeVal).ToList();

            string jobId = null;

            foreach (Games game in games)
            {
                int fk_Home = teams.Where(a => a._365_TeamId == game.HomeCompetitor.Id.ToString()).Select(a => a.Id).FirstOrDefault();
                int fk_Away = teams.Where(a => a._365_TeamId == game.AwayCompetitor.Id.ToString()).Select(a => a.Id).FirstOrDefault();
                int fk_GameWeak = gameWeaks.Where(a => a._365_GameWeakId == game.RoundNum.ToString()).Select(a => a.Id).FirstOrDefault();

                if (fk_GameWeak > 0)
                {
                    jobId = jobId.IsExisting()
                        ? BackgroundJob.ContinueJobWith(jobId, () => UpdateGame(_365CompetitionsEnum, game, fk_Home, fk_Away, fk_GameWeak))
                        : BackgroundJob.Enqueue(() => UpdateGame(_365CompetitionsEnum, game, fk_Home, fk_Away, fk_GameWeak));
                }
            }

            jobId = jobId.IsExisting()
                    ? BackgroundJob.ContinueJobWith(jobId, () => UpdateGameWeakDeadline(_365CompetitionsEnum, season.Id))
                    : BackgroundJob.Enqueue(() => UpdateGameWeakDeadline(_365CompetitionsEnum, season.Id));
        }

        public async Task UpdateGame(
            _365CompetitionsEnum _365CompetitionsEnum,
            Games game,
            int fk_Home,
            int fk_Away,
            int fk_GameWeak)
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

                if (match.FirstNotificationJobId.IsExisting())
                {
                    _ = BackgroundJob.Delete(match.FirstNotificationJobId);
                }
                if (match.SecondNotificationJobId.IsExisting())
                {
                    _ = BackgroundJob.Delete(match.SecondNotificationJobId);
                }
                if (match.ThirdNotificationJobId.IsExisting())
                {
                    _ = BackgroundJob.Delete(match.ThirdNotificationJobId);
                }

                GameResultDataHelper gameResultDataHelper = new(_unitOfWork, _365Services);

                match.JobId = BackgroundJob.Schedule(() => gameResultDataHelper.RunUpdateGameResult(_365CompetitionsEnum, new TeamGameWeakParameters { _365_MatchId = game.Id.ToString() }, false, false, false, false, false), startTime); // بداية الماتش;
                match.SecondJobId = BackgroundJob.Schedule(() => gameResultDataHelper.RunUpdateGameResult(_365CompetitionsEnum, new TeamGameWeakParameters { _365_MatchId = game.Id.ToString() }, true, false, false, false, false), startTime.AddMinutes(120)); // احتساب البونص;
                match.ThirdJobId = BackgroundJob.Schedule(() => gameResultDataHelper.RunUpdateGameResult(_365CompetitionsEnum, new TeamGameWeakParameters { _365_MatchId = game.Id.ToString() }, true, false, false, true, false), startTime.AddMinutes(200)); // احتساب البونص;

                match.FirstNotificationJobId = BackgroundJob.Schedule(() => SendNotificationForMatch(match.Id, MatchNotificationEnum.StartMatch), startTime);
                match.SecondNotificationJobId = BackgroundJob.Schedule(() => SendNotificationForMatch(match.Id, MatchNotificationEnum.HalfTime), startTime.AddMinutes(45));
                match.ThirdNotificationJobId = BackgroundJob.Schedule(() => SendNotificationForMatch(match.Id, MatchNotificationEnum.EndMatch), startTime.AddMinutes(105));

                _unitOfWork.Save().Wait();
            }
        }

        public async Task UpdateGameWeakDeadline(_365CompetitionsEnum _365CompetitionsEnum, int fk_Season)
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
                        gameWeak.JobId = BackgroundJob.Schedule(() => UpdateCurrentGameWeak(_365CompetitionsEnum, gameWeak.Id, false, 0, false, false), deadline);

                        gameWeak.SecondJobId = BackgroundJob.Schedule(() => SendNotificationForGameWeakDeadLine(gameWeak.Id), deadline.AddMinutes(-60));
                    }

                    gameWeak.Deadline = deadline;

                    await _unitOfWork.Save();
                }
            }
        }

        public async Task UpdateCurrentGameWeak(_365CompetitionsEnum _365CompetitionsEnum, int id, bool resetTeam, int fk_AccounTeam = 0, bool inDebug = false, bool skipReset = false)
        {
            if (!skipReset)
            {
                _unitOfWork.Season.ResetCurrentGameWeaks(_365CompetitionsEnum);
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
            //Notification notification = new()
            //{
            //    Title = "باقي على نهاية الجوله ساعه",
            //    Description = " إلحق ضبط تشكيلتك",
            //    OpenType = NotificationOpenTypeEnum.DeadLine,
            //    OpenValue = id.ToString(),
            //    NotificationLang = new NotificationLang
            //    {
            //        Title = "There is an hour left in the tour",
            //        Description = "Configure your formation",
            //    }
            //};
            //_unitOfWork.Notification.CreateNotification(notification);
            //await _unitOfWork.Save();

            //if (notification.Id > 0)
            //{
            //    _notificationManager.SendToTopic(new FirebaseNotificationModel
            //    {
            //        MessageHeading = notification.Title,
            //        MessageContent = notification.Description,
            //        ImgUrl = notification.StorageUrl + notification.ImageUrl,
            //        OpenType = notification.OpenType.ToString(),
            //        OpenValue = notification.OpenValue,
            //        Topic = "all"
            //    }).Wait();
            //}
        }

        public async Task SendNotificationForMatch(int id, MatchNotificationEnum matchNotificationEnum)
        {
            //TeamGameWeakModel match = _unitOfWork.Season.GetTeamGameWeaksForNotification(new TeamGameWeakParameters
            //{
            //    Id = id
            //}).FirstOrDefault();

            //if (match != null && match.IsActive)
            //{
            //    TeamGameWeak teamGameWeak = await _unitOfWork.Season.FindTeamGameWeakbyId(id, trackChanges: true);

            //    Notification notification = new()
            //    {
            //        OpenType = NotificationOpenTypeEnum.MatchProfile,
            //        OpenValue = id.ToString(),
            //        NotificationLang = new NotificationLang()
            //    };

            //    if (matchNotificationEnum == MatchNotificationEnum.StartMatch)
            //    {
            //        notification.Title = $"انتباه! بدء مباراة فى الأسبوع {match.GameWeak.Name} الآن. اضبط التردد واستمتع بالمباراة!";
            //        notification.Description = $"بين فريقي {match.Home.Name} و {match.Away.Name}";

            //        notification.NotificationLang.Title = $"Attention! The match in week {match.GameWeak.OtherName} is starting now. Tune in and enjoy the game!";
            //        notification.NotificationLang.Description = $"Between {match.Home.OtherName} and {match.Away.OtherName}";

            //        teamGameWeak.FirstNotificationJobId = null;

            //        _unitOfWork.Save().Wait();
            //    }
            //    else if (matchNotificationEnum == MatchNotificationEnum.HalfTime)
            //    {
            //        if (match.HalfTimeEnded)
            //        {
            //            if (teamGameWeak.FirstNotificationJobId.IsExisting())
            //            {
            //                _ = BackgroundJob.Delete(teamGameWeak.FirstNotificationJobId);
            //                teamGameWeak.FirstNotificationJobId = null;
            //            }

            //            teamGameWeak.SecondNotificationJobId = null;
            //            _unitOfWork.Save().Wait();
            //        }
            //        else
            //        {
            //            teamGameWeak.SecondNotificationJobId = BackgroundJob.Schedule(() => SendNotificationForMatch(match.Id, MatchNotificationEnum.HalfTime), DateTime.UtcNow.AddMinutes(5));
            //            _unitOfWork.Save().Wait();

            //            return;
            //        }

            //        notification.Title = $"انتهت فترة الشوط الأول! النتائج حتى الآن";
            //        notification.Description = $"[{match.Home.Name} ({match.HomeScore})] [{match.Away.Name} ({match.AwayScore})]";

            //        notification.NotificationLang.Title = $"Halftime is over! The results so far";
            //        notification.NotificationLang.Description = $"[{match.Home.OtherName} ({match.HomeScore})] [{match.Away.OtherName} ({match.AwayScore})]";
            //    }
            //    else if (matchNotificationEnum == MatchNotificationEnum.EndMatch)
            //    {
            //        if (match.IsEnded)
            //        {
            //            if (teamGameWeak.FirstNotificationJobId.IsExisting())
            //            {
            //                _ = BackgroundJob.Delete(teamGameWeak.FirstNotificationJobId);
            //                teamGameWeak.FirstNotificationJobId = null;
            //            }
            //            if (teamGameWeak.SecondNotificationJobId.IsExisting())
            //            {
            //                _ = BackgroundJob.Delete(teamGameWeak.SecondNotificationJobId);
            //                teamGameWeak.SecondNotificationJobId = null;
            //            }

            //            teamGameWeak.ThirdNotificationJobId = null;
            //            _unitOfWork.Save().Wait();
            //        }
            //        else
            //        {
            //            teamGameWeak.ThirdNotificationJobId = BackgroundJob.Schedule(() => SendNotificationForMatch(match.Id, MatchNotificationEnum.EndMatch), DateTime.UtcNow.AddMinutes(5));
            //            _unitOfWork.Save().Wait();

            //            return;
            //        }

            //        notification.Title = $"انتهت المباراة! النتيجة النهائية";
            //        notification.Description = $"[{match.Home.Name} ({match.HomeScore})] [{match.Away.Name} ({match.AwayScore})]";

            //        notification.NotificationLang.Title = $"The football match has ended. Final score";
            //        notification.NotificationLang.Description = $"[{match.Home.OtherName} ({match.HomeScore})] [{match.Away.OtherName} ({match.AwayScore})]";
            //    }

            //    _unitOfWork.Notification.CreateNotification(notification);
            //    await _unitOfWork.Save();

            //    if (notification.Id > 0)
            //    {
            //        _notificationManager.SendToTopic(new FirebaseNotificationModel
            //        {
            //            MessageHeading = notification.Title,
            //            MessageContent = notification.Description,
            //            ImgUrl = notification.StorageUrl + notification.ImageUrl,
            //            OpenType = notification.OpenType.ToString(),
            //            OpenValue = notification.OpenValue,
            //            Topic = "all"
            //        }).Wait();
            //    }
            //}
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

                                if (prev_365_GameWeakId > 0)
                                {
                                    GameWeak prevGameWeak = await _unitOfWork.Season.FindGameWeakby365Id(prev_365_GameWeakId.ToString(), fk_Season, trackChanges: false);

                                    fk_PrevGameWeak = prevGameWeak.Id;
                                }
                                else
                                {
                                    fk_PrevGameWeak = 0;
                                }
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

        public enum MatchNotificationEnum
        {
            StartMatch,
            HalfTime,
            EndMatch
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
