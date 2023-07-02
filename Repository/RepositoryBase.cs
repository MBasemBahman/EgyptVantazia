namespace Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected BaseContext DBContext;
        public RepositoryBase(BaseContext dBContext)
        {
            DBContext = dBContext;
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            return trackChanges ? DBContext.Set<T>() : DBContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            return trackChanges ? DBContext.Set<T>().Where(expression) : DBContext.Set<T>().Where(expression).AsNoTracking();
        }

        public int Count()
        {
            return DBContext.Set<T>().Count();
        }

        public void Create(T entity)
        {
            _ = DBContext.Set<T>().Add(entity);
        }

        public void Create(List<T> entities)
        {
            DBContext.Set<T>().AddRange(entities);
        }

        public void Delete(T entity)
        {
            _ = DBContext.Set<T>().Remove(entity);
        }

        public void Delete(List<T> entities)
        {
            DBContext.Set<T>().RemoveRange(entities);
        }

        public void Delete(IQueryable<T> quary)
        {
            quary.ExecuteDelete();
        }

    }
}
