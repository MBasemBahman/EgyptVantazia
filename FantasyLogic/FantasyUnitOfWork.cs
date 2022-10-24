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

        private GameWeakDataHelper _gameWeakDataHelper;

        private StandingsDataHelper _standingsDataHelper;

        private PlayerPositionDataHelper _playerPositionDataHelper;
        private TeamDataHelper _teamDataHelper;
        private PlayerDataHelper _playerDataHelper;

        private GamesDataHelper _gamesDataHelper;

        private GameResultDataHelper _gameResultDataHelper;
        private ScoreTypeDataHelper _scoreTypeDataHelper;

        public FantasyUnitOfWork(UnitOfWork unitOfWork, _365Services _365Services)
        {
            this._365Services = _365Services;
            _unitOfWork = unitOfWork;
        }

        public GameWeakDataHelper GameWeakDataHelper
        {
            get
            {
                _gameWeakDataHelper ??= new GameWeakDataHelper(_unitOfWork, _365Services);
                return _gameWeakDataHelper;
            }
        }

        public StandingsDataHelper StandingsDataHelper
        {
            get
            {
                _standingsDataHelper ??= new StandingsDataHelper(_unitOfWork, _365Services);
                return _standingsDataHelper;
            }
        }

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

        public GamesDataHelper GamesDataHelper
        {
            get
            {
                _gamesDataHelper ??= new GamesDataHelper(_unitOfWork, _365Services);
                return _gamesDataHelper;
            }
        }
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

    }
}
