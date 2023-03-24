using Contracts.Services;
using FantasyLogic.Calculations;
using FantasyLogic.DataMigration.GamesData;
using FantasyLogic.DataMigration.PlayerScoreData;
using FantasyLogic.DataMigration.SeasonData;
using FantasyLogic.DataMigration.StandingsData;
using FantasyLogic.DataMigration.TeamData;

namespace FantasyLogic
{
    public class FantasyUnitOfWork
    {
        private readonly _365Services _365Services;
        private readonly UnitOfWork _unitOfWork;
        private readonly IFirebaseNotificationManager _notificationManager;

        public FantasyUnitOfWork(UnitOfWork unitOfWork, _365Services _365Services, IFirebaseNotificationManager firebaseNotificationManager)
        {
            this._365Services = _365Services;
            _unitOfWork = unitOfWork;
            _notificationManager = firebaseNotificationManager;
        }

        #region Private
        #region SeasonData
        private GameWeakDataHelper _gameWeakDataHelper;
        #endregion

        #region StandingsData
        private StandingsDataHelper _standingsDataHelper;
        #endregion

        #region TeamData
        private TeamDataHelper _teamDataHelper;
        private PlayerDataHelper _playerDataHelper;
        #endregion

        #region GamesData
        private GamesDataHelper _gamesDataHelper;
        #endregion

        #region PlayerScoreData
        private GameResultDataHelper _gameResultDataHelper;
        private ScoreTypeDataHelper _scoreTypeDataHelper;
        #endregion

        #region PlayerStateCalculations
        private PlayerStateCalc _PlayerStateCalc;
        #endregion

        #region AccountTeamCalculations
        private AccountTeamCalc _AccountTeamCalc;
        private PrivateLeagueClac _PrivateLeagueClac;
        #endregion

        #endregion

        #region Public

        #region SeasonData
        public GameWeakDataHelper GameWeakDataHelper
        {
            get
            {
                _gameWeakDataHelper ??= new GameWeakDataHelper(_unitOfWork, _365Services);
                return _gameWeakDataHelper;
            }
        }

        #endregion

        #region StandingsData
        public StandingsDataHelper StandingsDataHelper
        {
            get
            {
                _standingsDataHelper ??= new StandingsDataHelper(_unitOfWork, _365Services);
                return _standingsDataHelper;
            }
        }
        #endregion

        #region TeamData
        public TeamDataHelper TeamDataHelper
        {
            get
            {
                _teamDataHelper ??= new TeamDataHelper(_unitOfWork, _365Services);
                return _teamDataHelper;
            }
        }
        public PlayerDataHelper PlayerDataHelper
        {
            get
            {
                _playerDataHelper ??= new PlayerDataHelper(_unitOfWork, _365Services);
                return _playerDataHelper;
            }
        }
        #endregion

        #region GamesData
        public GamesDataHelper GamesDataHelper
        {
            get
            {
                _gamesDataHelper ??= new GamesDataHelper(_unitOfWork, _365Services, _notificationManager);
                return _gamesDataHelper;
            }
        }
        #endregion

        #region PlayerScoreData
        public GameResultDataHelper GameResultDataHelper
        {
            get
            {
                _gameResultDataHelper ??= new GameResultDataHelper(_unitOfWork, _365Services);
                return _gameResultDataHelper;
            }
        }
        public ScoreTypeDataHelper ScoreTypeDataHelper
        {
            get
            {
                _scoreTypeDataHelper ??= new ScoreTypeDataHelper(_unitOfWork, _365Services);
                return _scoreTypeDataHelper;
            }
        }
        #endregion

        #region PlayerStateCalculations
        public PlayerStateCalc PlayerStateCalc
        {
            get
            {
                _PlayerStateCalc ??= new PlayerStateCalc(_unitOfWork);
                return _PlayerStateCalc;
            }
        }
        #endregion

        #region AccountTeamCalculations
        public AccountTeamCalc AccountTeamCalc
        {
            get
            {
                _AccountTeamCalc ??= new AccountTeamCalc(_unitOfWork);
                return _AccountTeamCalc;
            }
        }
        public PrivateLeagueClac PrivateLeagueClac
        {
            get
            {
                _PrivateLeagueClac ??= new PrivateLeagueClac(_unitOfWork);
                return _PrivateLeagueClac;
            }
        }
        #endregion

        #endregion

    }
}
