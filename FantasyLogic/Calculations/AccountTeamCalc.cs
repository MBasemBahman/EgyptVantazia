using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.PlayerStateModels;
using Entities.CoreServicesModels.PrivateLeagueModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.AccountTeamModels;
using System.Linq.Dynamic.Core;
using static Contracts.EnumData.DBModelsEnum;

namespace FantasyLogic.Calculations
{
    public class AccountTeamCalc
    {
        private readonly UnitOfWork _unitOfWork;

        public AccountTeamCalc(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void RunAccountTeamsCalculations(int fk_GameWeak)
        {
            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            List<GameWeakModel> gameWeaks = _unitOfWork.Season
                                             .GetGameWeaks(new GameWeakParameters
                                             {
                                                 Fk_Season = season.Id,
                                                 Id = fk_GameWeak
                                             }, otherLang: false)
                                             .Select(a => new GameWeakModel
                                             {
                                                 Id = a.Id,
                                                 _365_GameWeakId = a._365_GameWeakId
                                             }).ToList();

            //AccountTeamsCalculations(gameWeaks, season.Id);
            _ = BackgroundJob.Enqueue(() => AccountTeamsCalculations(gameWeaks, season.Id));
        }

        public void AccountTeamsCalculations(List<GameWeakModel> gameWeaks, int fk_Season)
        {
            string jobId = null;

            foreach (GameWeakModel gameWeak in gameWeaks)
            {
                //AccountTeamGameWeakCalculations(gameWeak, fk_Season, jobId);
                jobId = jobId.IsExisting()
                            ? BackgroundJob.ContinueJobWith(jobId, () => AccountTeamGameWeakCalculations(gameWeak, fk_Season, jobId))
                            : BackgroundJob.Enqueue(() => AccountTeamGameWeakCalculations(gameWeak, fk_Season, jobId));

                jobId = jobId.IsExisting()
                            ? BackgroundJob.ContinueJobWith(jobId, () => UpdateAccountTeamGameWeakRanking(gameWeak, fk_Season, jobId))
                            : BackgroundJob.Enqueue(() => UpdateAccountTeamGameWeakRanking(gameWeak, fk_Season, jobId));
            }

            List<int> accountTeams = _unitOfWork.AccountTeam.GetAccountTeams(new AccountTeamParameters
            {
                Fk_Season = fk_Season
            }, otherLang: false)
                .Select(a => a.Id)
                .ToList();

            foreach (int accountTeam in accountTeams)
            {
                //_ = UpdateAccountTeamPoints(accountTeam, fk_Season, jobId);
                jobId = jobId.IsExisting()
                            ? BackgroundJob.ContinueJobWith(jobId, () => UpdateAccountTeamPoints(accountTeam, fk_Season, jobId))
                            : BackgroundJob.Enqueue(() => UpdateAccountTeamPoints(accountTeam, fk_Season, jobId));
            }

            jobId = jobId.IsExisting()
                            ? BackgroundJob.ContinueJobWith(jobId, () => UpdateAccountTeamRanking(fk_Season, jobId))
                            : BackgroundJob.Enqueue(() => UpdateAccountTeamRanking(fk_Season, jobId));
        }

        public string AccountTeamGameWeakCalculations(GameWeakModel gameWeak, int fk_Season, string jobId)
        {
            var accountTeamGameWeaks = _unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
            {
                Fk_Season = fk_Season,
                Fk_GameWeak = gameWeak.Id
            }, otherLang: false)
                    .Select(a => new
                    {
                        a.Fk_AccountTeam,
                        a.Id
                    })
                    .ToList();

            foreach (var accountTeamGameWeak in accountTeamGameWeaks)
            {
                //AccountTeamPlayersCalculations(accountTeamGameWeak.Id, accountTeamGameWeak.Fk_AccountTeam, fk_GameWeak, fk_Season, jobId);

                jobId = jobId.IsExisting()
                           ? BackgroundJob.ContinueJobWith(jobId, () => AccountTeamPlayersCalculations(accountTeamGameWeak.Id, accountTeamGameWeak.Fk_AccountTeam, gameWeak, fk_Season, jobId))
                           : BackgroundJob.Enqueue(() => AccountTeamPlayersCalculations(accountTeamGameWeak.Id, accountTeamGameWeak.Fk_AccountTeam, gameWeak, fk_Season, jobId));
            }

            return jobId;
        }

        public string AccountTeamPlayersCalculations(int fk_AccountTeamGameWeak, int fk_AccountTeam, GameWeakModel gameWeak, int fk_Season, string jobId)
        {
            List<AccountTeamPlayersCalculationPoints> playersFinalPoints = new();
            double totalPoints = 0;

            var players = _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new AccountTeamPlayerGameWeakParameters
            {
                Fk_AccountTeam = fk_AccountTeam,
                Fk_Season = fk_Season,
                Fk_GameWeak = gameWeak.Id,
                IsTransfer = false
            }, otherLang: false).Select(a => new
            {
                a.AccountTeamPlayer.Fk_Player,
                a.AccountTeamPlayer.Player.Fk_PlayerPosition,
                a.Fk_AccountTeamPlayer,
                a.Fk_TeamPlayerType,
                a.Order,
                a.IsPrimary
            }).OrderByDescending(a => a.IsPrimary == true)
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

            bool captianPointsFlag = true;
            bool havePointsInTotal = true;

            AccountTeamGameWeak accountTeamGameWeak = _unitOfWork.AccountTeam.FindAccountTeamGameWeakbyId(fk_AccountTeamGameWeak, trackChanges: true).Result;

            foreach (var player in players)
            {
                //if (playersFinalPoints.Count == 11)
                //{
                //    break;
                //}

                //if (player.Fk_PlayerPosition == (int)PlayerPositionEnum.Goalkeeper &&
                //    playersFinalPoints.Any(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Goalkeeper))
                //{
                //    continue;
                //}


                //if (playersFinalPoints.Count == 10 &&
                //       !playersFinalPoints.Any(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Goalkeeper) &&
                //       player.Fk_PlayerPosition != (int)PlayerPositionEnum.Goalkeeper)
                //{
                //    continue;
                //}

                if (playersPoints.Any(a => a.Fk_Player == player.Fk_Player))
                {
                    int captianPoints = 1;

                    if (captianPointsFlag &&
                      (player.Fk_TeamPlayerType == (int)TeamPlayerTypeEnum.Captian ||
                      player.Fk_TeamPlayerType == (int)TeamPlayerTypeEnum.ViceCaptian))
                    {
                        captianPointsFlag = false;
                        captianPoints = accountTeamGameWeak.TripleCaptain ? 3 : 2;
                    }

                    double points = playersPoints.Where(a => a.Fk_Player == player.Fk_Player).Select(a => a.Points).First() * captianPoints;

                    if (havePointsInTotal &&
                        accountTeamGameWeak.BenchBoost == false &&
                        (playersFinalPoints.Count > 11 ||
                        playersFinalPoints.Any(a => a.Fk_PlayerPosition == (int)PlayerPositionEnum.Goalkeeper)))
                    {
                        havePointsInTotal = false;
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
                        HavePointsInTotal = havePointsInTotal
                    });

                    if (havePointsInTotal)
                    {
                        totalPoints += points;
                    }
                }
            }

            _unitOfWork.AccountTeam.ResetAccountTeamPlayerGameWeakPoints(fk_AccountTeam, gameWeak.Id);

            foreach (AccountTeamPlayersCalculationPoints playersFinalPoint in playersFinalPoints)
            {
                _unitOfWork.AccountTeam.CreateAccountTeamPlayerGameWeak(new AccountTeamPlayerGameWeak
                {
                    Fk_GameWeak = gameWeak.Id,
                    Fk_AccountTeamPlayer = playersFinalPoint.Fk_AccountTeamPlayer,
                    Fk_TeamPlayerType = playersFinalPoint.Fk_TeamPlayerType,
                    Order = playersFinalPoint.Order,
                    IsPrimary = playersFinalPoint.IsPrimary,
                    Points = (int)playersFinalPoint.Points,
                    HavePoints = true,
                    HavePointsInTotal = playersFinalPoint.HavePointsInTotal
                });
            }

            var prevPoints = _unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
            {
                Fk_AccountTeam = fk_AccountTeam,
                Fk_Season = fk_Season,
                _365_GameWeakId = (gameWeak._365_GameWeakId.ParseToInt() + 1).ToString()
            }, otherLang: false)
                .Select(a => a.TotalPoints)
                .FirstOrDefault();


            accountTeamGameWeak.TotalPoints = (int)totalPoints + accountTeamGameWeak.TansfarePoints;

            if (accountTeamGameWeak.DoubleGameWeak)
            {
                accountTeamGameWeak.TotalPoints *= 2;
            }

            accountTeamGameWeak.PrevPoints = prevPoints;
            _unitOfWork.Save().Wait();

            return jobId;
        }

        public string UpdateAccountTeamGameWeakRanking(GameWeakModel gameWeak, int fk_Season, string jobId)
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

            return jobId;
        }

        public string UpdateAccountTeamPoints(int fk_AccountTeam, int fk_Season, string jobId)
        {
            int totalPoints = _unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
            {
                Fk_AccountTeam = fk_AccountTeam,
                Fk_Season = fk_Season,
            }, otherLang: false)
                .Select(a => a.TotalPoints)
                .Sum();

            AccountTeam accountTeam = _unitOfWork.AccountTeam.FindAccountTeambyId(fk_AccountTeam, trackChanges: true).Result;
            accountTeam.TotalPoints = totalPoints;
            _unitOfWork.Save().Wait();

            return jobId;
        }

        public string UpdateAccountTeamRanking(int fk_Season, string jobId)
        {
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
                    Fk_FavouriteTeam = a.Account.Fk_FavouriteTeam
                })
                .ToList();

            foreach (AccountTeamRanking accountTeam in accountTeams)
            {
                accountTeamRankings.Add(new AccountTeamRanking
                {
                    Id = accountTeam.Id,
                    Fk_Country = accountTeam.Fk_Country,
                    Fk_FavouriteTeam = accountTeam.Fk_FavouriteTeam,
                    GlobalRanking = ranking++
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

                _unitOfWork.Save().Wait();
            }

            return jobId;
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
    }

    public class AccountTeamRanking
    {
        public int Id { get; set; }
        public int Fk_Country { get; set; }
        public int Fk_FavouriteTeam { get; set; }
        public int CountryRanking { get; set; }
        public int FavouriteTeamRanking { get; set; }
        public int GlobalRanking { get; set; }
    }
}
