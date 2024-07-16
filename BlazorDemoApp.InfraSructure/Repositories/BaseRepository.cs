using BlazorDemoApp.Core.Interfaces;
using BlazorDemoApp.Shared.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;

namespace BlazorDemoApp.InfraSructure.Repositories
{

    public class BaseRepository<T> : IBaseRepository<T> where T : ITable
    {
		protected ApplicationDbContext _context;

		public static void Log(Exception ex, string MethodName)
		{
			Console.WriteLine($"********** Logging New Error of Services ********** \n" +
					$"Error on : BaseRepository_Func Method: {MethodName} \n" +
					$"Current_Exception : {ex.Message} \n" +
					$"Inner Exception: {(ex.InnerException?.Message ?? "No Inner Exception")}  \n" +
					$"Done at:  {DateTime.Now.ToLongTimeString()} \n" +
					$"********** End of Error **********");
		}

		public BaseRepository(ApplicationDbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public IEnumerable<T> GetAll()
		{
			return _context.Set<T>().ToList();
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _context.Set<T>().ToListAsync();
		}

		public T GetByIdShort(short id)
		{
			return _context.Set<T>().Find(id);
		}

		public T GetByIdLong(long id)
		{
			return _context.Set<T>().Find(id);
		}
		public T GetById(int id)
		{
			return _context.Set<T>().Find(id);
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _context.Set<T>().FindAsync(id);
		}

		public T? Find(Expression<Func<T, bool>> criteria, string[] includes = null)
		{
			try
			{
				IQueryable<T> query = _context.Set<T>();

				if (includes != null)
					foreach (var incluse in includes)
						query = query.Include(incluse);

				return query.SingleOrDefault(criteria);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public async Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
		{
			IQueryable<T> query = _context.Set<T>();

			if (includes != null)
				foreach (var incluse in includes)
					query = query.Include(incluse);

			return await query.SingleOrDefaultAsync(criteria);
		}

		public IEnumerable<T>? FindAll(Expression<Func<T, bool>> criteria, string[] includes = null)
		{
			try
			{

				IQueryable<T> query = _context.Set<T>();

				if (includes != null)
					foreach (var include in includes)
						query = query.Include(include);

				var q = query.Where(criteria).AsNoTracking();

				return q.ToList();
			}
			catch (Exception ex)
			{
				Log(ex, MethodBase.GetCurrentMethod().Name);
				return null;
			}
		}

		public List<T>? GetEntitiesWithChildrenAndGrandchildren(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes)
		{
			try
			{
				IQueryable<T> query = _context.Set<T>();
				if (includes != null)
					foreach (var include in includes)
						query = query.Include(include);


				var q = query.Where(criteria).AsNoTracking();
				return q.ToList();
			}
			catch (Exception ex)
			{
				Log(ex, MethodBase.GetCurrentMethod().Name);
				return null;
			}
		}
		public IEnumerable<T>? FindAll(Expression<Func<T, bool>> criteria, int skip, int take , string[] includes = null)
		{
			try
			{
				IQueryable<T> query = _context.Set<T>();
				if (includes != null)
					foreach (var include in includes)
						query = query.Include(include);

				var q = query.Where(criteria).AsNoTracking().Skip(skip).Take(take).ToList();
				return q;
			}
			catch (Exception ex)
			{
				Log(ex, MethodBase.GetCurrentMethod().Name);
				return null;
			}
		}

		public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int? skip, int? take, string[] includes = null,
			Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending)
		{
			try
			{
				IQueryable<T> query = _context.Set<T>();
				if (includes != null)
					foreach (var include in includes)
						query = query.Include(include);

				query = query.Where(criteria);

				if (skip.HasValue)
					query = query.Skip(skip.Value);

				if (take.HasValue)
					query = query.Take(take.Value);

				if (orderBy != null)
				{
					if (orderByDirection == OrderBy.Ascending)
						query = query.OrderBy(orderBy);
					else
						query = query.OrderByDescending(orderBy);
				}

				return query.ToList();
			}
			catch (Exception ex)
			{
				Log(ex, MethodBase.GetCurrentMethod().Name);
				return null;
			}
		}

		public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
		{
			try
			{
				IQueryable<T> query = _context.Set<T>();

				if (includes != null)
					foreach (var include in includes)
						query = query.Include(include);

				return await query.Where(criteria).ToListAsync();
			}
			catch (Exception ex)
			{
				Log(ex, MethodBase.GetCurrentMethod().Name);
				return null;
			}
		}

		public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int take, int skip)
		{
			try
			{
				return await _context.Set<T>().Where(criteria).Skip(skip).Take(take).ToListAsync();
			}
			catch (Exception ex)
			{
				Log(ex, MethodBase.GetCurrentMethod().Name);
				return null;
			}
		}

		public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? take, int? skip,
			Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending)
		{
			try
			{
				IQueryable<T> query = _context.Set<T>().Where(criteria);

				if (take.HasValue)
					query = query.Take(take.Value);

				if (skip.HasValue)
					query = query.Skip(skip.Value);

				if (orderBy != null)
				{
					if (orderByDirection == OrderBy.Ascending)
						query = query.OrderBy(orderBy);
					else
						query = query.OrderByDescending(orderBy);
				}

				return await query.ToListAsync();
			}
			catch (Exception ex)
			{
				Log(ex, MethodBase.GetCurrentMethod().Name);
				return null;
			}
		}

		public T Add(T entity)
		{
			try
			{
				_context.Set<T>().Add(entity);
				return entity;
			}
			catch (Exception ex)
			{
				Log(ex, MethodBase.GetCurrentMethod().Name);
				return null;
			}
		}

		public async Task<T> AddAsync(T entity)
		{
			try
			{
				await _context.Set<T>().AddAsync(entity);
				return entity;
			}
			catch (Exception ex)
			{
				Log(ex, MethodBase.GetCurrentMethod().Name);
				return null;
			}
		}

		public IEnumerable<T> AddRange(IEnumerable<T> entities)
		{
			try
			{
				_context.Set<T>().AddRange(entities);
				return entities;
			}
			catch (Exception ex)
			{
				Log(ex, MethodBase.GetCurrentMethod().Name);
				return null;
			}
		}

		public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
		{
			try
			{
				await _context.Set<T>().AddRangeAsync(entities);
				return entities;
			}
			catch (Exception ex)
			{
				Log(ex, MethodBase.GetCurrentMethod().Name);
				return null;
			}
		}

		public T Update(T entity)
		{
			try
			{
				_context.Update(entity);
				return entity;
			}
			catch (Exception ex)
			{
				Log(ex, MethodBase.GetCurrentMethod().Name);
				return null;
			}
		}

		public void Delete(T entity)
		{
			try
			{
				_context.Set<T>().Remove(entity);
			}
			catch (Exception ex)
			{
				Log(ex, MethodBase.GetCurrentMethod().Name);

			}
		}

		public void DeleteRange(IEnumerable<T> entities)
		{
			try
			{
				_context.Set<T>().RemoveRange(entities);
			}
			catch (Exception ex)
			{
				Log(ex, MethodBase.GetCurrentMethod().Name);

			}
		}

		public void Attach(T entity)
		{
			_context.Set<T>().Attach(entity);
		}

		public void AttachRange(IEnumerable<T> entities)
		{
			_context.Set<T>().AttachRange(entities);
		}

		public int Count()
		{
			return _context.Set<T>().Count();
		}

		public int Count(Expression<Func<T, bool>> criteria)
		{
			return _context.Set<T>().Count(criteria);
		}

		public async Task<int> CountAsync()
		{
			return await _context.Set<T>().CountAsync();
		}

		public async Task<int> CountAsync(Expression<Func<T, bool>> criteria)
		{
			return await _context.Set<T>().CountAsync(criteria);
		}

		public void DeleteByID(int Id)
		{
			_context.Set<T>().Remove(_context.Set<T>().Find(Id));

		}

		public void DeleteByID(short Id)
		{
			_context.Set<T>().Remove(_context.Set<T>().Find(Id));

		}

		public void DeleteByID(long Id)
		{
			_context.Set<T>().Remove(_context.Set<T>().Find(Id));

		}

		public List<Select2Model>? ToSelect2Model(string id, string txt, Expression<Func<T, bool>> criteria, bool? top_element, string? top_id, string? top_name)
		{
			try
			{
				var rslt = new List<Select2Model>();
				if (top_element.Value)
				{
					rslt.Add(new Select2Model { id = string.IsNullOrWhiteSpace(top_id) ? "" : top_id, text = string.IsNullOrWhiteSpace(top_name) ? " " : top_name });
				}

				var l = FindAll(criteria).ToList();
				if (l.Count > 0)
				{
					foreach (T item in l)
					{
						//Type t = item.GetType();
						//PropertyInfo prop_id = t.GetProperty(id);
						//PropertyInfo prop_txt = t.GetProperty(txt);

						rslt.Add(new Select2Model()
						{
							id = item.GetType().GetProperty(id).GetValue(item, null).ToString(),
							text = item.GetType().GetProperty(txt).GetValue(item, null).ToString()
						});
					}

				}
				return rslt;

			}
			catch (Exception ex)
			{
				Log(ex, MethodBase.GetCurrentMethod().Name);
				return null;
			}
		}



		public T? AddEntityWithCurrentUser(T entity, int currentUserId)
		{
			Type entityType = typeof(T);
			Type baseEntityType = typeof(AuditEntity_EmpID_INT);

			if (baseEntityType.IsAssignableFrom(entityType))
			{
				(entity as AuditEntity_EmpID_INT).added_date = DateTime.Now;

				(entity as AuditEntity_EmpID_INT).Modify_date = DateTime.Now;
				(entity as AuditEntity_EmpID_INT).added_by = (entity as AuditEntity_EmpID_INT).Modify_by = currentUserId;

				_context.Set<T>().Add(entity);
				return entity;
			}
			else
				return null;
		}

		public IEnumerable<T>? AddRangeEntityWithCurrentUser(IEnumerable<T> entities, int currentUserId)
		{
			Type entityType = typeof(T);
			Type baseEntityType = typeof(AuditEntity_EmpID_INT);

			if (baseEntityType.IsAssignableFrom(entityType))
			{
				foreach (var entity in entities)
				{
					(entity as AuditEntity_EmpID_INT).added_date = DateTime.Now;

					(entity as AuditEntity_EmpID_INT).Modify_date = DateTime.Now;
					(entity as AuditEntity_EmpID_INT).added_by = (entity as AuditEntity_EmpID_INT).Modify_by = currentUserId;
				}
				_context.Set<T>().AddRange(entities);
				return entities;
			}
			return null;
		}

		public T? UpdateEntityWithCurrentUser(T entity, int currentUserId)
		{
			Type entityType = typeof(T);
			Type baseEntityType = typeof(AuditEntity_EmpID_INT);

			if (baseEntityType.IsAssignableFrom(entityType))
			{

				(entity as AuditEntity_EmpID_INT).Modify_date = DateTime.Now;
				(entity as AuditEntity_EmpID_INT).Modify_by = currentUserId;

				_context.Update(entity);
				return entity;
			}
			else
				return null;
		}

		public async Task<T>? AddAsyncWithCurrentUser(T entity, int currentUserId)
		{
			Type entityType = typeof(T);
			Type baseEntityType = typeof(AuditEntity_EmpID_INT);

			if (baseEntityType.IsAssignableFrom(entityType))
			{
				(entity as AuditEntity_EmpID_INT).added_date = DateTime.Now;

				(entity as AuditEntity_EmpID_INT).Modify_date = DateTime.Now;
				(entity as AuditEntity_EmpID_INT).added_by = (entity as AuditEntity_EmpID_INT).Modify_by = currentUserId;

				await _context.Set<T>().AddAsync(entity);
				return entity;
			}
			else
				return null;
		}


	}
}