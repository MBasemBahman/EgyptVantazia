using Entities.CoreServicesModels.UserModels;

namespace Repository.DBModels.UserModels
{
    public class VerificationRepository : RepositoryBase<Verification>
    {
        public VerificationRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<Verification> FindAll(
          VerificationParameters parameters,
          bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Fk_User);

        }

        public bool CheckVerificationCodeExisting(string code)
        {
            return FindByCondition(a => a.Code == code, trackChanges: false).Any();
        }

    }

    public static class VerificationRepositoryExtensions
    {
        public static IQueryable<Verification> Filter(
            this IQueryable<Verification> verifications,
            int fk_User)
        {
            return verifications.Where(a => a.Fk_User == fk_User);
        }
    }
}
