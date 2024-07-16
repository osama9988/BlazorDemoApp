using BlazorDemoApp.Core;
using BlazorDemoApp.Core.Interfaces;
using BlazorDemoApp.InfraSructure.Repositories;
using BlazorDemoApp.Shared.Classes.TableClass;
using BlazorDemoApp.Shared.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;


namespace BlazorDemoApp.InfraSructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        private IDbContextTransaction _transaction;

        public IBaseRepository<MyAppUser> MyAppUser=  new BaseRepository<MyAppUser>(_context);

        public IBaseRepository<Add1_Markaz> Add0_Gov = new BaseRepository<Add1_Markaz>(_context);

        public IBaseRepository<Add1_Markaz> Add1_Markaz = new BaseRepository<Add1_Markaz>(_context);
        public IBaseRepository<Add2_City> Add2_City = new BaseRepository<Add2_City>(_context);

      
        //


        public UnitOfWork(ApplicationDbContext context, IDbContextTransaction transaction = null)

        {

            _context = context ?? throw new ArgumentNullException(nameof(context));

            _transaction = transaction;

            // Marking the parameter as unused to suppress the warning
#pragma warning disable CS1711 // Unused parameter
            _ = transaction;
#pragma warning restore CS1711 // Unused parameter
            //



        }

        public IBaseRepository<T>? GetRepository<T>() where T : ITable
        {
            try
            {
                var repositoryType = typeof(T);

                if (_repositories.ContainsKey(repositoryType))
                {
                    return (IBaseRepository<T>)_repositories[repositoryType];
                }

                var repository = new BaseRepository<T>(_context);
                _repositories.Add(repositoryType, repository);
                return repository;
            }
            catch (Exception ex)
            {

                return null;
            }


            //if (!repositories.ContainsKey(typeof(T)))
            //{
            //	throw new InvalidOperationException($"Repository for {typeof(T)} not registered.");
            //}

            //return (IBaseRepository<T>)repositories[typeof(T)];
            //var repository = new BaseRepository<T>(_dbContext);
            //_repositories.Add(repositoryType, repository);
            //return repository;
        }

        //public IBaseRepository<T> GetRepository<T>() where T : class
        //{
        //          var tt = (IBaseRepository<T>)typeof(UnitOfWork).GetProperty($"{typeof(T).Name}Repository")?.GetValue(this);
        //          return tt;
        //}
        public bool chk_db()
        {
            try { _context.Database.OpenConnection(); _context.Database.CloseConnection(); return true; }
            catch { return false; }
        }

        public bool chk_db_Version()
        {
            try
            {
                //var db_Ver = _context.DBSettings.OrderByDescending(e => e.Id).ToList().FirstOrDefault().ver;

                //// return (db_Ver.Equals("1"))? true:false ; 
                ////return (db_Ver.Equals("1.1.1"))? true:false ; 
                //// return (db_Ver.Equals("1.1.2")) ? true : false;
                return (db_Ver.Equals("1.1.3")) ? true : false;
            }
            catch { return false; }
        }

        public int Complete()
        {
            return _context.SaveChanges();

        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public bool Commit()
        {
            try
            {
                _transaction?.Commit();
                return true;
            }
            catch (SqlException ex)
            {
                Rollback();
                throw new Exception("Transaction failed: " + ex.Message, ex); // Wrap and rethrow with details

            }
            catch (Exception ex) // Catch other potential exceptions
            {
                Rollback();
                return false;
            }
        }

        public void Rollback()
        {
            _transaction?.Rollback();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

        public List<string> FindMissingNonNullDetailProps<T>(T obj)
        {
            var missingProps = new List<string>();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;
                var isNullable = Nullable.GetUnderlyingType(propertyType) != null;
                var isRequired = typeof(T).GetProperty(property.Name).GetCustomAttributes(typeof(RequiredAttribute), true).Any();

                if (isNullable || isRequired)
                {
                    var value = property.GetValue(obj);
                    if (value == null)
                    {
                        missingProps.Add(property.Name); // Top-level property
                    }
                    else
                    {
                        missingProps.AddRange(FindMissingNonNullDetailProps(value)); // Recursively check nested objects
                    }
                }
            }
            return missingProps;

        }

        public List<string> get_dbsets_with_AuditEntity()
        {
            //Console.WriteLine("Entity: {0} [{1}]", entityType.Name, entityType.ClrType.FullName);
            //foreach (IProperty property in entityType.GetProperties())
            //{
            //    Console.WriteLine("* {0} [{1}]", property.Name, property.ClrType.FullName);
            //}
            var rslt = new List<string>();
            foreach (EntityType item in _context.Model.GetEntityTypes())
            {
                Type entityType = typeof(ApplicationDbContext).GetProperty(item.Name).PropertyType.GetGenericArguments()[0];

                Type baseEntityType = typeof(AuditEntity_EmpID_INT);

                if (baseEntityType.IsAssignableFrom(entityType))
                    rslt.Add(item.Name);
            }


            return rslt;
        }
    }
}