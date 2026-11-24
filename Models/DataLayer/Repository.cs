using Microsoft.EntityFrameworkCore;

namespace MedicaidEmploymentVerificationApplication.Models.DataLayer
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected MevaContext context { get; set; }
        private DbSet<T> dbSet { get; set; }

        public Repository(MevaContext ctx)
        {
            context = ctx;
            dbSet = context.Set<T>();
        }

        public int Count => dbSet.Count();

        public virtual IEnumerable<T> List(QueryOptions<T> options) => BuildQuery(options).ToList();

        public virtual T? Get(int id) => dbSet.Find(id);
        public virtual T? Get(string id) => dbSet.Find(id);
        public virtual T? Get(QueryOptions<T> options) => BuildQuery(options).FirstOrDefault();

        public virtual void Delete(T entity) => dbSet.Remove(entity);


        public virtual void Insert(T entity) => dbSet.Add(entity);



        public virtual void Save() => context.SaveChanges();

        public virtual void Update(T entity) => dbSet.Update(entity);


        public IEnumerable<T> BuildQuery(QueryOptions<T> options)
        {
            IQueryable<T> query = dbSet;

            foreach (string include in options.GetIncludes())
            {
                query = query.Include(include);
            }

            if (options.HasWhere)
            {
                query = query.Where(options.Where);
            }
            if (options.HasOrderBy)
            {
                var orderedQuery = options.OrderByDirection == "asc"
                    ? query.OrderBy(options.OrderBy) : query.OrderByDescending(options.OrderBy);

                if (options.ThenBy != null)
                {
                    orderedQuery = orderedQuery.ThenBy(options.ThenBy);
                }

                query = orderedQuery;
            }
            if (options.HasPaging)
            {
                query = query.PageBy(options.PageNumber, options.PageSize);
            }
            return query;
        }
    }
}
