﻿using Entities.DBModels.AccountTeamModels;
using Entities.RequestFeatures;


namespace Repository.DBModels.AccountTeamModels
{
    public class TeamPlayerTypeRepository : RepositoryBase<TeamPlayerType>
    {
        public TeamPlayerTypeRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<TeamPlayerType> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<TeamPlayerType> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.TeamPlayerTypeLang)
                        .FirstOrDefaultAsync();
        }

        public new void Create(TeamPlayerType entity)
        {
            entity.TeamPlayerTypeLang ??= new TeamPlayerTypeLang
            {
                Name = entity.Name,
            };
            base.Create(entity);
        }
    }

    public static class TeamPlayerTypeRepositoryExtension
    {
        public static IQueryable<TeamPlayerType> Filter(this IQueryable<TeamPlayerType> TeamPlayerTypes, int id)
        {
            return TeamPlayerTypes.Where(a => id == 0 || a.Id == id);
        }

    }
}
