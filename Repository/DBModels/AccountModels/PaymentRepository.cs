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
                       parameters.TransactionId,
                       parameters.DashboardSearch);
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
            string transactionId,
            string dashboardSearch)
        {
            return Payments.Where(a => (id == 0 || a.Id == id) &&
                                       
                                       (string.IsNullOrEmpty(dashboardSearch) || 
                                            a.Account.FullName.Contains(dashboardSearch) ||
                                            a.Id.ToString().Contains(dashboardSearch) ||
                                            a.Amount.ToString().Contains(dashboardSearch) ||
                                            a.TransactionId.Contains(dashboardSearch)) &&
                                       
                                       (fk_Account == 0 || a.Fk_Account == fk_Account) &&
                                       (string.IsNullOrEmpty(transactionId) || a.TransactionId == transactionId));

        }

    }
}
