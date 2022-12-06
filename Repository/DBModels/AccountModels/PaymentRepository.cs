using Entities.CoreServicesModels.AccountModels;
using Entities.DBModels.AccountModels;

namespace Repository.DBModels.AccountModels
{
    public class PaymentRepository : RepositoryBase<Payment>
    {
        public PaymentRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<Payment> FindAll(PaymentParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                       parameters.Fk_Account,
                       parameters.TransactionId);
        }

        public async Task<Payment> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }


    }

    public static class PaymentRepositoryExtension
    {
        public static IQueryable<Payment> Filter(
            this IQueryable<Payment> Payments,
            int id,
            int fk_Account,
            string transactionId)
        {
            return Payments.Where(a => (id == 0 || a.Id == id) &&
                                       (fk_Account == 0 || a.Fk_Account == fk_Account) &&
                                       (string.IsNullOrEmpty(transactionId) || a.TransactionId == transactionId));

        }

    }
}
