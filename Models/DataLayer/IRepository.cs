namespace MedicaidEmploymentVerificationApplication.Models.DataLayer
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> List(QueryOptions<T> options);
        int Count { get; }
        T? Get(int id);
        T? Get(string id);
        T? Get(QueryOptions<T> options);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
    }
}
