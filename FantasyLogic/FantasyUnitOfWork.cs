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

        public FantasyUnitOfWork(UnitOfWork unitOfWork, _365Services _365Services)
        {
            this._365Services = _365Services;
            _unitOfWork = unitOfWork;
        }

        #region Private
        #region SeasonData
        private GameWeakDataHelper _gameWeakDataHelper;
        #endregion

        #region StandingsData
        private StandingsDataHelper _standingsDataHelper;
        #endregion

        #region TeamData
        private PlayerPositionDataHelper _playerPositionDataHelper;
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
        public PlayerPositionDataHelper PlayerPositionDataHelper
        {
            get
            {
                _playerPositionDataHelper ??= new PlayerPositionDataHelper(_unitOfWork, _365Services);
                return _playerPositionDataHelper;
            }
        }
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
                _gamesDataHelper ??= new GamesDataHelper(_unitOfWork, _365Services);
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

        #endregion

    }
}
