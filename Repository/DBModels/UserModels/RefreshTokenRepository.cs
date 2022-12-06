using Entities.CoreServicesModels.UserModels;

namespace Repository.DBModels.UserModels
{
    public class RefreshTokenRepository : RepositoryBase<RefreshToken>
    {
        public RefreshTokenRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<RefreshToken> FindAll(
          RefreshTokenParameters parameters,
          bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Fk_User, parameters.refreshTokenTTL);

        }
    }

    public static class RefreshTokenRepositoryExtensions
    {
        public static IQueryable<RefreshToken> Filter(
            this IQueryable<RefreshToken> tokens,
            int fk_User, int refreshTokenTTL)
        {
            return tokens.Where(a => a.Fk_User == fk_User &&
            (refreshTokenTTL == 0 || a.CreatedAt.AddDays(refreshTokenTTL) >= DateTime.UtcNow));
        }
    }
}
