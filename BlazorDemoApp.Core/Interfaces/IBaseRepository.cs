using BlazorDemoApp.Shared.Consts;
using BlazorDemoApp.Shared.Interface;
using System.Linq.Expressions;

namespace BlazorDemoApp.Core.Interfaces
{
    public interface IBaseRepository<T> where T : ITable
    {
		T GetByIdLong(Int64 id);
		T GetByIdShort(Int16 id);
		T GetById(int id);
        Task<T> GetByIdAsync(int id);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T Find(Expression<Func<T, bool>> criteria, string[]? includes = null);
        Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[]? includes = null);
        IEnumerable<T>? FindAll(Expression<Func<T, bool>> criteria, string[]? includes = null);
        IEnumerable<T>? FindAll(Expression<Func<T, bool>> criteria, int take, int skip , string[]? includes = null);
        IEnumerable<T>? FindAll(Expression<Func<T, bool>> criteria,int? take, int? skip, string[]? includes = null,
			Expression<Func<T, object>>? orderBy = null, string orderByDirection = OrderBy.Ascending);

		Task<IEnumerable<T>>? FindAllAsync(Expression<Func<T, bool>> criteria, string[]? includes = null);
        Task<IEnumerable<T>>? FindAllAsync(Expression<Func<T, bool>> criteria, int skip, int take);
        Task<IEnumerable<T>>? FindAllAsync(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>>? orderBy = null, string orderByDirection = OrderBy.Ascending);
        T Add(T entity);
        Task<T> AddAsync(T entity);
        IEnumerable<T> AddRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        T Update(T entity);
        void Delete(T entity);
        void DeleteByID(int Id);
		void DeleteByID(short Id);

		void DeleteByID(long Id);
		void DeleteRange(IEnumerable<T> entities);
        void Attach(T entity);
        void AttachRange(IEnumerable<T> entities);
        int Count();
        int Count(Expression<Func<T, bool>> criteria);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> criteria);

        List<Select2Model> ToSelect2Model(string id, string txt, Expression<Func<T, bool>> criteria,
            bool? top_element = null, string? top_id=null, string? top_name = null);

        public T? AddEntityWithCurrentUser(T entity, int currentUserId);
        public IEnumerable<T>? AddRangeEntityWithCurrentUser(IEnumerable<T> entities, int currentUserId);
        public T? UpdateEntityWithCurrentUser(T entity, int currentUserId);
        public  Task<T>? AddAsyncWithCurrentUser(T entity, int currentUserId);

        public List<T>? GetEntitiesWithChildrenAndGrandchildren(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes);
	}
}