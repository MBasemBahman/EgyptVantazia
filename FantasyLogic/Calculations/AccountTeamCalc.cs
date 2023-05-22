using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.PlayerStateModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.AccountTeamModels;
using System.Linq.Dynamic.Core;
using static Contracts.EnumData.DBModelsEnum;

namespace FantasyLogic.Calculations
{
    public class AccountTeamCalc
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly HangFireCustomJob _hangFireCustomJob;

        private readonly string AccountGameWeakTeamId = "AccountTeamGameWeakCalc-";
        private readonly string AccountTeamId = "AccountTeamCalc-";

        public AccountTeamCalc(
            UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _hangFireCustomJob = new HangFireCustomJob(unitOfWork);
        }

        public void RunAccountTeamsCalculations(int fk_GameWeak, int fk_AccountTeam, List<int> fk_Players, List<int> fk_Teams, bool inDebug = false)
        {
            if (inDebug == false && fk_GameWeak == 0 && fk_AccountTeam == 0)
            {
                fk_GameWeak = _unitOfWork.Season.GetCurrentGameWeak().Id;
            }

            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();
            GameWeakModel nextGameWeek = _unitOfWork.Season.GetNextGameWeak();

            List<GameWeakModel> gameWeaks = _unitOfWork.Season
                                             .GetGameWeaks(new GameWeakParameters
                                             {
                                                 Fk_Season = season.Id,
                                                 Id = fk_GameWeak,
                                                 GameWeakTo = nextGameWeek._365_GameWeakIdValue
                                             }, otherLang: false)
                                             .Select(a => new GameWeakModel
                                             {
                                                 Id = a.Id,
                                                 _365_GameWeakId = a._365_GameWeakId
                                             }).ToList();

            foreach (GameWeakModel gameWeak in gameWeaks)
            {
                if (inDebug)
                {
                    AccountTeamGameWeakCalculations(gameWeak, season.Id, fk_AccountTeam, fk_Players, fk_Teams, inDebug);
                }
                else
                {
                    BackgroundJob.Enqueue(() => AccountTeamGameWeakCalculations(gameWeak, season.Id, fk_AccountTeam, fk_Players, fk_Teams, inDebug));
                }
            }
        }

        public void AccountTeamGameWeakCalculations(GameWeakModel gameWeak, int fk_Season, int fk_AccountTeam, List<int> fk_Players, List<int> fk_Teams, bool inDebug)
        {
            var accountTeamGameWeaks = _unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
            {
                Fk_Season = fk_Season,
                Fk_GameWeak = gameWeak.Id,
                Fk_AccountTeam = fk_AccountTeam,
                Fk_Players = fk_Players,
                Fk_Teams = fk_Teams
            }, otherLang: false)
                    .Select(a => new
                    {
                        a.Id,
                        a.Fk_AccountTeam
                    })
                    .ToList();

            if (!accountTeamGameWeaks.Any())
            {
                return;
            }

            foreach (var accountTeamGameWeak in accountTeamGameWeaks)
            {
                if (accountTeamGameWeak.Id == 0 || accountTeamGameWeak.Fk_AccountTeam == 0)
                {
                    continue;
                }
                if (inDebug)
                {
                    _ = AccountTeamPlayersCalculations(accountTeamGameWeak.Id, accountTeamGameWeak.Fk_AccountTeam, gameWeak, fk_Season, true);
                }
                else
                {
                    string recurringId = AccountGameWeakTeamId + $"{accountTeamGameWeak.Id}";

                    string hangfireJobId = BackgroundJob.Enqueue(() => AccountTeamPlayersCalculations(accountTeamGameWeak.Id, accountTeamGameWeak.Fk_AccountTeam, gameWeak, fk_Season, true));

                    _hangFireCustomJob.ReplaceJob(hangfireJobId, recurringId);
                }
            }

            List<int> accountTeams = accountTeamGameWeaks.GroupBy(a => a.Fk_AccountTeam).Select(a => a.Key).ToList();

            string jobId = null;
            foreach (int accountTeam in accountTeams)
            {
                if (inDebug)
                {
                    UpdateAccountTeamPoints(accountTeam, fk_Season);
                }
                else
                {
                    string recurringId = AccountTeamId + $"{accountTeam}";

                    string hangfireJobId = BackgroundJob.Enqueue(() => UpdateAccountTeamPoints(accountTeam, fk_Season));

                    _hangFireCustomJob.ReplaceJob(hangfireJobId, recurringId);

                    jobId = hangfireJobId;
                }
            }

            //if (inDebug)
            //{
            //    UpdateAccountTeamGameWeakRanking(gameWeak, fk_Season);
            //    UpdateAccountTeamRanking(fk_Season);
            //}
            //else
            //{
            //    jobId = jobId.IsExisting()
            //            ? BackgroundJob.ContinueJobWith(jobId, () => UpdateAccountTeamGameWeakRanking(gameWeak, fk_Season))
            //            : BackgroundJob.Enqueue(() => UpdateAccountTeamGameWeakRanking(gameWeak, fk_Season));

            //    jobId = jobId.IsExisting()
            //                ? BackgroundJob.ContinueJobWith(jobId, () => UpdateAccountTeamRanking(fk_Season))
            //                : BackgroundJob.Enqueue(() => UpdateAccountTeamRanking(fk_Season));
            //}
        }

        public AccountTeamCustemClac AccountTeamPlayersCalculations(int fk_AccountTeamGameWeak, int fk_AccountTeam, GameWeakModel gameWeak, int fk_Season, bool saveChanges = true)
        {
            bool gameWeakEnd = _unitOfWork.Season.CheckIfGameWeakEnded(gameWeak.Id);

            List<AccountTeamPlayersCalculationPoints> playersFinalPoints = new();
            List<AccountTeamPlayersCalculationPoints> flagListPoints = new();

            double totalPoints = 0;
            double benchPoints = 0;

            List<AccountTeamPlayerGameWeakDto> players = _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new AccountTeamPlayerGameWeakParameters
            {
                Fk_AccountTeam = fk_AccountTeam,
                Fk_Season = fk_Season,
                Fk_GameWeak = gameWeak.Id,
                IsTransfer = false
            }, otherLang: false).Select(a => new AccountTeamPlayerGameWeakDto
            {
                Fk_Player = a.AccountTeamPlayer.Fk_Player,
                Fk_PlayerPosition = a.AccountTeamPlayer.Player.Fk_PlayerPosition,
                Fk_AccountTeamPlayer = a.Fk_AccountTeamPlayer,
                Fk_TeamPlayerType = a.Fk_TeamPlayerType,
                Order = a.Order,
                IsPrimary = a.IsPrimary,
                IsParticipate = a.IsParticipate,
                IsPlayed = a.IsPlayed,
                IsDelayed = a.IsDelayed,
                NotHaveMatch = a.NotHaveMatch,
                Points = a.Points,
                PlayerName = a.AccountTeamPlayer.Player.Name
            }).ToList()
              .OrderByDescending(a => a.IsPrimary == true)
              .ThenByDescending(a => a.IsParticipate == true)
              .ThenByDescending(a => a.Fk_TeamPlayerType == (int)TeamPlayerTypeEnum.Captian)
              .ThenByDescending(a => a.Fk_TeamPlayerType == (int)TeamPlayerTypeEnum.ViceCaptian)
              .ThenBy(a => a.Order)
              .ToList();

            var playersPoints = _unitOfWork.PlayerState.GetPlayerGameWeakScoreStates(new PlayerGameWeakScoreStateParameters
            {
                Fk_Players = players.Select(a => a.Fk_Player).ToList(),
                Fk_GameWeak = gameWeak.Id,
                Fk_ScoreState = (int)ScoreStateEnum.Total
            }, otherLang: false)
                .Select(a => new
                {
                    a.Fk_Player,
                    a.Points
                })
            .ToList();

            bool captianPointsFlag = players.Any(a => a.Fk_TeamPlayerType == (int)TeamPlayerTypeEnum.Captian && (a.IsPlayed ||
                                                                                                                 a.IsDelayed ||
                                                                                                                 a.NotHaveMatch));
            bool havePointsInTotal = true;

            AccountTeamGameWeak accountTeamGameWeak = _unitOfWork.AccountTeam.FindAccountTeamGameWeakbyId(fk_AccountTeamGameWeak, trackChanges: true).Result;

            if (accountTeamGameWeak == null)
            {
                return null;
            }
            players.ForEach(player => player.Points = playersPoints.Where(points => points.Fk_Player == player.Fk_Player).Select(a => a.Points).FirstOrDefault());

            int playerPrimaryAndPlayed = 11;

            if (accountTeamGameWeak.Top_11)
            {
                players = players.OrderByDescending(a => a.Points).ToList();
            }
            else
            {
                playerPrimaryAndPlayed = accountTeamGameWeak.BenchBoost ? players.Count(a => a.IsParticipate) : players.Count(a => a.IsPrimary &&
                                                                                                                                  (a.IsPlayed ||
                                                                                                                                   a.IsDelayed ||
                                                                                                                                   a.NotHaveMatch));

                playerPrimaryAndPlayed = playerPrimaryAndPlayed > 11 ? 11 : playerPrimaryAndPlayed;
            }

            foreach (AccountTeamPlayerGameWeakDto player in players)
            {
                havePointsInTotal = true;
                int captianPoints = 1;

                if (havePointsInTotal && accountTeamGameWeak.BenchBoost == false)
                {
                    if (playersFinalPoints.Count(a => a.HavePointsInTotal == true) >= playerPrimaryAndPlayed)
                    {
                        havePointsInTotal = false;
                    }
                    else if (player.Fk_PlayerPosition == (int)PlayerPositionEnum.Goalkeeper &&
                    playersFinalPoints.Count(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Goalkeeper &&
                                                  a.HavePointsInTotal == true) == 1)
                    {
                        havePointsInTotal = false;
                    }
                    else if (player.Fk_PlayerPosition == (int)PlayerPositionEnum.Defender &&
                       playersFinalPoints.Count(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Defender &&
                                                     a.HavePointsInTotal == true) == 5)
                    {
                        havePointsInTotal = false;
                    }
                    else if (player.Fk_PlayerPosition == (int)PlayerPositionEnum.Midfielder &&
                      playersFinalPoints.Count(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Midfielder &&
                                                    a.HavePointsInTotal == true) == 5)
                    {
                        havePointsInTotal = false;
                    }
                    else if (player.Fk_PlayerPosition == (int)PlayerPositionEnum.Attacker &&
                     playersFinalPoints.Count(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Attacker &&
                                                   a.HavePointsInTotal == true) == 3)
                    {
                        havePointsInTotal = false;
                    }
                }

                if (havePointsInTotal &&
                    captianPointsFlag &&
                    ((player.Fk_TeamPlayerType == (int)TeamPlayerTypeEnum.Captian && player.IsParticipate) ||
                     (player.Fk_TeamPlayerType == (int)TeamPlayerTypeEnum.ViceCaptian && player.IsParticipate)))
                {
                    captianPointsFlag = false;
                    captianPoints = accountTeamGameWeak.TripleCaptain ? 3 : 2;
                }

                double points = player.Points.Value * captianPoints;

                if (gameWeakEnd &&
                    player.IsPrimary == false &&
                    accountTeamGameWeak.BenchBoost == false &&
                    accountTeamGameWeak.Top_11 == false)
                {
                    havePointsInTotal = false;

                    AccountTeamPlayersCalculationPoints changedPlayer = null;

                    if (player.Fk_PlayerPosition == (int)PlayerPositionEnum.Goalkeeper)
                    {
                        havePointsInTotal = playersFinalPoints.Any(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Goalkeeper &&
                                                                         a.IsPrimary &&
                                                                         a.HavePointsInTotal &&
                                                                         a.IsParticipate == false);
                        if (havePointsInTotal)
                        {
                            changedPlayer = playersFinalPoints.First(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Goalkeeper &&
                                                          a.IsPrimary &&
                                                          a.HavePointsInTotal &&
                                                          a.IsParticipate == false);

                            playersFinalPoints.First(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Goalkeeper &&
                                                          a.IsPrimary &&
                                                          a.HavePointsInTotal &&
                                                          a.IsParticipate == false).HavePointsInTotal = false;
                        }
                    }
                    else if (player.Fk_PlayerPosition == (int)PlayerPositionEnum.Attacker)
                    {
                        havePointsInTotal = playersFinalPoints.Any(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Attacker &&
                                                                        a.IsPrimary &&
                                                                        a.HavePointsInTotal &&
                                                                        a.IsParticipate == false);

                        if (havePointsInTotal)
                        {
                            changedPlayer = playersFinalPoints.First(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Attacker &&
                                                          a.IsPrimary &&
                                                          a.HavePointsInTotal &&
                                                          a.IsParticipate == false);

                            playersFinalPoints.First(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Attacker &&
                                                          a.IsPrimary &&
                                                          a.HavePointsInTotal &&
                                                          a.IsParticipate == false).HavePointsInTotal = false;
                        }

                        if (havePointsInTotal == false)
                        {
                            havePointsInTotal = playersFinalPoints.Any(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Midfielder &&
                                                                            a.IsPrimary &&
                                                                            a.HavePointsInTotal &&
                                                                            a.IsParticipate == false);

                            if (havePointsInTotal)
                            {

                                changedPlayer = playersFinalPoints.First(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Midfielder &&
                                                              a.IsPrimary &&
                                                              a.HavePointsInTotal &&
                                                              a.IsParticipate == false);

                                playersFinalPoints.First(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Midfielder &&
                                                              a.IsPrimary &&
                                                              a.HavePointsInTotal &&
                                                              a.IsParticipate == false).HavePointsInTotal = false;
                            }
                        }

                        if (havePointsInTotal == false)
                        {
                            havePointsInTotal = playersFinalPoints.Any(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Defender &&
                                                                            a.IsPrimary &&
                                                                            a.HavePointsInTotal &&
                                                                            a.IsParticipate == false) &&
                                                playersFinalPoints.Count(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Defender &&
                                                                              a.HavePointsInTotal) > 3;

                            if (havePointsInTotal)
                            {
                                changedPlayer = playersFinalPoints.First(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Defender &&
                                                              a.IsPrimary &&
                                                              a.HavePointsInTotal &&
                                                              a.IsParticipate == false);

                                playersFinalPoints.First(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Defender &&
                                                              a.IsPrimary &&
                                                              a.HavePointsInTotal &&
                                                              a.IsParticipate == false).HavePointsInTotal = false;
                            }
                        }
                    }
                    else if (player.Fk_PlayerPosition == (int)PlayerPositionEnum.Midfielder)
                    {
                        havePointsInTotal = playersFinalPoints.Any(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Midfielder &&
                                                                        a.IsPrimary &&
                                                                        a.HavePointsInTotal &&
                                                                        a.IsParticipate == false);

                        if (havePointsInTotal)
                        {

                            changedPlayer = playersFinalPoints.First(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Midfielder &&
                                                          a.IsPrimary &&
                                                          a.HavePointsInTotal &&
                                                          a.IsParticipate == false);
                            playersFinalPoints.First(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Midfielder &&
                                                          a.IsPrimary &&
                                                          a.HavePointsInTotal &&
                                                          a.IsParticipate == false).HavePointsInTotal = false;
                        }

                        if (havePointsInTotal == false)
                        {
                            havePointsInTotal = playersFinalPoints.Any(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Attacker &&
                                                                       a.IsPrimary &&
                                                                       a.HavePointsInTotal &&
                                                                       a.IsParticipate == false) &&
                                                playersFinalPoints.Count(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Attacker &&
                                                                              a.HavePointsInTotal) > 1;

                            if (havePointsInTotal)
                            {
                                changedPlayer = playersFinalPoints.First(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Attacker &&
                                                              a.IsPrimary &&
                                                              a.HavePointsInTotal &&
                                                              a.IsParticipate == false);

                                playersFinalPoints.First(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Attacker &&
                                                              a.IsPrimary &&
                                                              a.HavePointsInTotal &&
                                                              a.IsParticipate == false).HavePointsInTotal = false;
                            }
                        }

                        if (havePointsInTotal == false)
                        {
                            havePointsInTotal = playersFinalPoints.Any(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Defender &&
                                                                            a.IsPrimary &&
                                                                            a.HavePointsInTotal &&
                                                                            a.IsParticipate == false) &&
                                                playersFinalPoints.Count(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Defender &&
                                                                              a.HavePointsInTotal) > 3;

                            if (havePointsInTotal)
                            {
                                changedPlayer = playersFinalPoints.First(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Defender &&
                                                              a.IsPrimary &&
                                                              a.HavePointsInTotal &&
                                                              a.IsParticipate == false);

                                playersFinalPoints.First(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Defender &&
                                                              a.IsPrimary &&
                                                              a.HavePointsInTotal &&
                                                              a.IsParticipate == false).HavePointsInTotal = false;
                            }
                        }
                    }
                    else if (player.Fk_PlayerPosition == (int)PlayerPositionEnum.Defender)
                    {
                        havePointsInTotal = playersFinalPoints.Any(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Defender &&
                                                                        a.IsPrimary &&
                                                                        a.HavePointsInTotal &&
                                                                        a.IsParticipate == false);

                        if (havePointsInTotal)
                        {
                            changedPlayer = playersFinalPoints.First(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Defender &&
                                                          a.IsPrimary &&
                                                          a.HavePointsInTotal &&
                                                          a.IsParticipate == false);

                            playersFinalPoints.First(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Defender &&
                                                          a.IsPrimary &&
                                                          a.HavePointsInTotal &&
                                                          a.IsParticipate == false).HavePointsInTotal = false;
                        }


                        if (havePointsInTotal == false)
                        {
                            havePointsInTotal = playersFinalPoints.Any(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Midfielder &&
                                                                            a.IsPrimary &&
                                                                            a.HavePointsInTotal &&
                                                                            a.IsParticipate == false);

                            if (havePointsInTotal)
                            {

                                changedPlayer = playersFinalPoints.First(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Midfielder &&
                                                              a.IsPrimary &&
                                                              a.HavePointsInTotal &&
                                                              a.IsParticipate == false);

                                playersFinalPoints.First(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Midfielder &&
                                                              a.IsPrimary &&
                                                              a.HavePointsInTotal &&
                                                              a.IsParticipate == false).HavePointsInTotal = false;
                            }
                        }

                        if (havePointsInTotal == false)
                        {
                            havePointsInTotal = playersFinalPoints.Any(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Attacker &&
                                                                       a.IsPrimary &&
                                                                       a.HavePointsInTotal &&
                                                                       a.IsParticipate == false) &&
                                                playersFinalPoints.Count(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Attacker &&
                                                                              a.HavePointsInTotal) > 1;

                            if (havePointsInTotal)
                            {
                                changedPlayer = playersFinalPoints.First(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Attacker &&
                                                              a.IsPrimary &&
                                                              a.HavePointsInTotal &&
                                                              a.IsParticipate == false);

                                playersFinalPoints.First(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Attacker &&
                                                              a.IsPrimary &&
                                                              a.HavePointsInTotal &&
                                                              a.IsParticipate == false).HavePointsInTotal = false;
                            }
                        }
                    }
                }

                playersFinalPoints.Add(new AccountTeamPlayersCalculationPoints
                {
                    Fk_Player = player.Fk_Player,
                    Fk_AccountTeamPlayer = player.Fk_AccountTeamPlayer,
                    Fk_PlayerPosition = player.Fk_PlayerPosition,
                    Fk_TeamPlayerType = player.Fk_TeamPlayerType,
                    IsPrimary = player.IsPrimary,
                    Order = player.Order,
                    Points = points,
                    HavePointsInTotal = havePointsInTotal,
                    PlayerName = player.PlayerName,
                    IsPlayed = player.IsPlayed,
                    IsParticipate = player.IsParticipate
                });

                if (havePointsInTotal)
                {
                    totalPoints += points;
                }
                else
                {
                    benchPoints += points;
                }
            }

            _unitOfWork.AccountTeam.ResetAccountTeamPlayerGameWeakPoints(fk_AccountTeam, gameWeak.Id);

            List<AccountTeamPlayerGameWeak> playersGameWeekPoints = new();

            foreach (AccountTeamPlayersCalculationPoints playersFinalPoint in playersFinalPoints)
            {
                AccountTeamPlayerGameWeak accountTeamPlayerGameWeak = new()
                {
                    Fk_GameWeak = gameWeak.Id,
                    Fk_AccountTeamPlayer = playersFinalPoint.Fk_AccountTeamPlayer,
                    Fk_TeamPlayerType = playersFinalPoint.Fk_TeamPlayerType,
                    Order = playersFinalPoint.Order,

                    IsPrimary = playersFinalPoint.IsPrimary,
                    Points = (int)playersFinalPoint.Points,
                    HavePoints = true,
                    HavePointsInTotal = playersFinalPoint.HavePointsInTotal
                };

                playersGameWeekPoints.Add(accountTeamPlayerGameWeak);
                _unitOfWork.AccountTeam.CreateAccountTeamPlayerGameWeak(accountTeamPlayerGameWeak);
            }

            int prevPoints = _unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
            {
                Fk_AccountTeam = fk_AccountTeam,
                Fk_Season = fk_Season,
                _365_GameWeakId = (gameWeak._365_GameWeakId.ParseToInt() + 1).ToString()
            }, otherLang: false)
                .Select(a => a.TotalPoints ?? 0)
                .FirstOrDefault();

            if (accountTeamGameWeak.DoubleGameWeak)
            {
                totalPoints *= 2;
            }

            accountTeamGameWeak.TotalPoints = (int)totalPoints + accountTeamGameWeak.TansfarePoints;
            accountTeamGameWeak.BenchPoints = (int)benchPoints;

            accountTeamGameWeak.PrevPoints = prevPoints;

            if (saveChanges)
            {
                _unitOfWork.Save().Wait();
            }
            else
            {
                return new AccountTeamCustemClac
                {
                    BenchPoints = accountTeamGameWeak.BenchPoints,
                    PrevPoints = accountTeamGameWeak.PrevPoints,
                    TotalPoints = accountTeamGameWeak.TotalPoints,
                    Players = playersGameWeekPoints
                };
            }

            return null;
        }

        public void RunUpdateAccountTeamGameWeakRanking()
        {
            var gameWeak = _unitOfWork.Season.GetCurrentGameWeak();

            BackgroundJob.Enqueue(() => UpdateAccountTeamGameWeakRanking(gameWeak, gameWeak.Fk_Season));
        }

        public void RunUpdateAccountTeamRanking()
        {
            var gameWeak = _unitOfWork.Season.GetCurrentGameWeak();

            BackgroundJob.Enqueue(() => UpdateAccountTeamRanking(gameWeak.Fk_Season));
        }

        public void UpdateAccountTeamGameWeakRanking(GameWeakModel gameWeak, int fk_Season)
        {
            List<AccountTeamRanking> accountTeamGameWeakRankings = new();
            int ranking = 1;

            List<AccountTeamRanking> accountTeamGameWeaks = _unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
            {
                Fk_Season = fk_Season,
                Fk_GameWeak = gameWeak.Id,
            }, otherLang: false)
                .OrderByDescending(a => a.TotalPoints)
                .Select(a => new AccountTeamRanking
                {
                    Id = a.Id,
                    Fk_Country = a.AccountTeam.Account.Fk_Country,
                    Fk_FavouriteTeam = a.AccountTeam.Account.Fk_FavouriteTeam
                })
                .ToList();

            foreach (AccountTeamRanking accountTeamGameWeak in accountTeamGameWeaks)
            {
                accountTeamGameWeakRankings.Add(new AccountTeamRanking
                {
                    Id = accountTeamGameWeak.Id,
                    Fk_Country = accountTeamGameWeak.Fk_Country,
                    Fk_FavouriteTeam = accountTeamGameWeak.Fk_FavouriteTeam,
                    GlobalRanking = ranking++
                });
            }

            foreach (IGrouping<int, AccountTeamRanking> country in accountTeamGameWeaks.GroupBy(a => a.Fk_Country))
            {
                ranking = 1;
                foreach (AccountTeamRanking accountTeamGameWeak in country.ToList())
                {
                    accountTeamGameWeakRankings.First(a => a.Id == accountTeamGameWeak.Id).CountryRanking = ranking++;
                }
            }

            foreach (IGrouping<int, AccountTeamRanking> favouriteTeam in accountTeamGameWeaks.GroupBy(a => a.Fk_FavouriteTeam))
            {
                ranking = 1;
                foreach (AccountTeamRanking accountTeamGameWeak in favouriteTeam.ToList())
                {
                    accountTeamGameWeakRankings.First(a => a.Id == accountTeamGameWeak.Id).FavouriteTeamRanking = ranking++;
                }
            }

            foreach (AccountTeamRanking accountTeamGameWeakRanking in accountTeamGameWeakRankings)
            {
                AccountTeamGameWeak accountTeam = _unitOfWork.AccountTeam.FindAccountTeamGameWeakbyId(accountTeamGameWeakRanking.Id, trackChanges: true).Result;
                accountTeam.GlobalRanking = accountTeamGameWeakRanking.GlobalRanking;
                accountTeam.CountryRanking = accountTeamGameWeakRanking.CountryRanking;
                accountTeam.FavouriteTeamRanking = accountTeamGameWeakRanking.FavouriteTeamRanking;

                _unitOfWork.Save().Wait();
            }
        }

        public void UpdateAccountTeamPoints(int fk_AccountTeam, int fk_Season)
        {
            int totalPoints = _unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
            {
                Fk_AccountTeam = fk_AccountTeam,
                Fk_Season = fk_Season,
            }, otherLang: false)
                .Select(a => a.TotalPoints ?? 0)
                .ToList()
                .Sum();

            AccountTeam accountTeam = _unitOfWork.AccountTeam.FindAccountTeambyId(fk_AccountTeam, trackChanges: true).Result;
            accountTeam.TotalPoints = totalPoints;
            _unitOfWork.Save().Wait();
        }

        public void UpdateAccountTeamRanking(int fk_Season)
        {
            GameWeakModel currentGameWeak = _unitOfWork.Season.GetCurrentGameWeak();

            List<AccountTeamRanking> accountTeamRankings = new();
            int ranking = 1;

            List<AccountTeamRanking> accountTeams = _unitOfWork.AccountTeam.GetAccountTeams(new AccountTeamParameters
            {
                Fk_Season = fk_Season
            }, otherLang: false)
                .OrderByDescending(a => a.TotalPoints)
                .Select(a => new AccountTeamRanking
                {
                    Id = a.Id,
                    Fk_Country = a.Account.Fk_Country,
                    Fk_FavouriteTeam = a.Account.Fk_FavouriteTeam,
                    TotalPoints = a.TotalPoints
                })
                .ToList();

            foreach (AccountTeamRanking accountTeam in accountTeams)
            {
                accountTeamRankings.Add(new AccountTeamRanking
                {
                    Id = accountTeam.Id,
                    Fk_Country = accountTeam.Fk_Country,
                    Fk_FavouriteTeam = accountTeam.Fk_FavouriteTeam,
                    GlobalRanking = ranking++,
                    TotalPoints = accountTeam.TotalPoints
                });
            }

            foreach (IGrouping<int, AccountTeamRanking> country in accountTeams.GroupBy(a => a.Fk_Country))
            {
                ranking = 1;
                foreach (AccountTeamRanking accountTeam in country.ToList())
                {
                    accountTeamRankings.First(a => a.Id == accountTeam.Id).CountryRanking = ranking++;
                }
            }

            foreach (IGrouping<int, AccountTeamRanking> favouriteTeam in accountTeams.GroupBy(a => a.Fk_FavouriteTeam))
            {
                ranking = 1;
                foreach (AccountTeamRanking accountTeam in favouriteTeam.ToList())
                {
                    accountTeamRankings.First(a => a.Id == accountTeam.Id).FavouriteTeamRanking = ranking++;
                }
            }

            foreach (AccountTeamRanking accountTeamRanking in accountTeamRankings)
            {
                AccountTeam accountTeam = _unitOfWork.AccountTeam.FindAccountTeambyId(accountTeamRanking.Id, trackChanges: true).Result;
                accountTeam.GlobalRanking = accountTeamRanking.GlobalRanking;
                accountTeam.CountryRanking = accountTeamRanking.CountryRanking;
                accountTeam.FavouriteTeamRanking = accountTeamRanking.FavouriteTeamRanking;

                AccountTeamGameWeakModel accountTeamGameWeakModel = _unitOfWork.AccountTeam.GetTeamGameWeak(accountTeam.Fk_Account, currentGameWeak.Id);

                if (accountTeamGameWeakModel != null)
                {
                    AccountTeamGameWeak accountTeamGameWeak = _unitOfWork.AccountTeam.FindAccountTeamGameWeakbyId(accountTeamGameWeakModel.Id, trackChanges: true).Result;

                    accountTeamGameWeak.SeasonGlobalRanking = accountTeam.GlobalRanking;
                    accountTeamGameWeak.SeasonTotalPoints = accountTeam.TotalPoints;
                }

                _unitOfWork.Save().Wait();
            }
        }
    }
}

public class AccountTeamPlayersCalculationPoints
{
    public int Fk_Player { get; set; }
    public int Fk_PlayerPosition { get; set; }

    public int Fk_AccountTeamPlayer { get; set; }
    public int Fk_TeamPlayerType { get; set; }
    public int Order { get; set; }
    public bool IsPrimary { get; set; }
    public double Points { get; set; }

    public bool HavePointsInTotal { get; set; }

    public string PlayerName { get; set; }

    public bool IsPlayed { get; set; }
    public bool IsParticipate { get; set; }
}

public class AccountTeamRanking
{
    public int Id { get; set; }
    public int Fk_Country { get; set; }
    public int Fk_FavouriteTeam { get; set; }
    public int CountryRanking { get; set; }
    public int FavouriteTeamRanking { get; set; }
    public int GlobalRanking { get; set; }
    public int TotalPoints { get; set; }
}

public class AccountTeamPlayerGameWeakDto
{
    public int Fk_Player { get; set; }
    public int Fk_PlayerPosition { get; set; }
    public int Fk_AccountTeamPlayer { get; set; }
    public int Fk_TeamPlayerType { get; set; }
    public int Order { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsParticipate { get; set; }
    public bool IsPlayed { get; set; }

    public bool IsDelayed { get; set; }

    public bool NotHaveMatch { get; set; }

    public double? Points { get; set; }

    public string PlayerName { get; set; }

    public bool IsTransfare { get; set; }
}
