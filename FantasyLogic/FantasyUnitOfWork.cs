using FantasyLogic.DataMigration.SeasonData;

namespace FantasyLogic
{
    public class FantasyUnitOfWork
    {
        private readonly _365Services _365Services;
        private readonly UnitOfWork _unitOfWork;

        private GameWeakDataHelper _gameWeakDataHelper;


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
    }
}
