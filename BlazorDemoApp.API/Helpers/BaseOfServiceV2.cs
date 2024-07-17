using BlazorDemoApp.Core.Interfaces;
using BlazorDemoApp.Shared.Consts;
using static BlazorDemoApp.Shared.App_Strings;
using System.Linq.Expressions;
using BlazorDemoApp.Shared.Interface;
using BlazorDemoApp.Core;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BlazorDemoApp.API.Helpers
{
    public interface IBaseServiceV2
    {
        void WriteLog_Service(Exception ex, string ServiceName, string MethodName);
    }

    public abstract class BaseOfServiceV2 : IBaseServiceV2
    {
        public ILogger _Logger;

        protected BaseOfServiceV2(ILogger logger)
        {
            _Logger = logger;
            //
        }

        public void WriteLog_Service(Exception ex, string ServiceName, string MethodName)
        {
            _Logger.LogInformation(
                    $"********** Logging New Error of Services ********** \n" +
                    $"Error on : Service:{ServiceName} -- Method: {MethodName} \n" +
                    $"Current_Exception : {ex.Message} \n" +
                    $"Inner Exception: {(ex.InnerException?.Message ?? "No Inner Exception")}  \n" +
                    $"Done at:  {DateTime.Now.ToLongTimeString()} \n" +
                    $"********** End of Error ********** \n" +
                    $"////////////////////////////////////////////////////////////////////////////////////// \n");
            ;
        }
    }
    public interface IBaseOfServiceV2<T> where T : class, ITable
    {

        #region functions

        List<string> FindMissingNonNullDetailProps<T>(T obj);

        IBaseRepository<T>? GetRepository<T>() where T : class, ITable;
        IEnumerable<T>? TFindAll(Expression<Func<T, bool>> criteria, string[] includes = null);
        IEnumerable<T>? TFindAll(Expression<Func<T, bool>> criteria, int skip, int take);
        IEnumerable<T>? TFindAll(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>>? orderBy = null, string orderByDirection = OrderBy.Ascending);
        IEnumerable<T>? TFindAll(Expression<Func<T, bool>> criteria, int skip, int take, string[]? includes = null);
        IEnumerable<T>? TFindAll(Expression<Func<T, bool>> criteria, int? take, int? skip, string[] includes = null,
            Expression<Func<T, object>>? orderBy = null, string orderByDirection = OrderBy.Ascending);
        IEnumerable<T>? TGetAll();
        T? TGetById(int id);
        T? TGetById(Int16 id);
        T? TGetById(Int64 id);
        T? TAddEntityWithCurrentUser(T entity, int cur_u_id);
        T TAddEntityWithCurrentUserTransaction(T entity, int cur_u_id);
        T? TUpdateEntityWithCurrentUser(T entity, int cur_u_id);
        T TUpdateEntityWithCurrentUserTransaction(T entity, int cur_u_id);
        DeleteResult TdeleteById(int id);
        DeleteResult TdeleteById(Int16 id);
        DeleteResult TdeleteById(Int64 id);
        DeleteResult TDeleteRange(IEnumerable<T> entities);

        DeleteResult TdeleteByIdWithChilds(Int16 id, string[]? childern);
        DeleteResult TdeleteByIdWithChilds(Int32 id, string[]? childern);
        DeleteResult TdeleteByIdWithChilds(Int64 id, string[]? childern);
        bool? Tchange_Status(string prop_fk_name, object prop_fk_val, int cur_u_id, string prop_isActive = null);
        bool? Tchange_Status(int id, int cur_u_id, string prop_isActive = null);
        bool? Tchange_Status(Int16 id, int cur_u_id, string prop_isActive = null);

        //TDest? MapTo<TDest>(T source) where TDest : class;

        List<Select2Model>? GetSelectList(cur_lang lang, Expression<Func<T, bool>> criteria, bool? top_element = false, string? top_id = null, string? top_name = null);
        List<Select2Model>? GetSelectList(string PropName_Id, string PropName_Txt, Expression<Func<T, bool>> criteria, bool? top_element = false, string? top_id = null, string? top_name = null);

        T? Tsave(T entity);

        int? TCount(Expression<Func<T, bool>> criteria);
        #endregion
    }
    public abstract class BaseOfServiceV2<T> : BaseOfServiceV2, IBaseOfServiceV2<T> where T : class, ITable
    {
        public ICanDeleteChecker<T>? _deleteChecker;
        public IUnitOfWork _unitOfWork;


        protected BaseOfServiceV2(IUnitOfWork unitOfWork, ILogger logger) : base(logger)
        {
            _unitOfWork = unitOfWork;
        }
        protected BaseOfServiceV2(IUnitOfWork unitOfWork, ILogger logger, ICanDeleteChecker<T> canDelete) : base(logger)
        {
            _unitOfWork = unitOfWork;
            _deleteChecker = canDelete;
        }

      


        #region interface_implementation
        //
        public List<string> FindMissingNonNullDetailProps<T>(T obj)
        {
            var missingProps = new List<string>();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;
                //var isNullable = Nullable.GetUnderlyingType(propertyType) != null;
                var isNullable = propertyType.IsValueType && Nullable.GetUnderlyingType(propertyType) != null;
                var isRequired = typeof(T).GetProperty(property.Name).GetCustomAttributes(typeof(RequiredAttribute), true).Any();

                if (isRequired && !isNullable)
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

        public IBaseRepository<T>? GetRepository<T>() where T : class,ITable
        {

            try
            {
                Type typeOfT = typeof(T);

                var rr = _unitOfWork?.GetRepository<T>();
                return rr;
            }
            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public int? TCount(Expression<Func<T, bool>> criteria)
        {

            try
            {
                var t = GetRepository<T>();
                if (t == null) throw new Exception("load Repo. Error");
                var ll = t.Count(criteria);

                return ll;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public IEnumerable<T>? TFindAll(Expression<Func<T, bool>> criteria, string[] includes = null)
        {

            try
            {
                IEnumerable<T> ll = Enumerable.Empty<T>();
                //
                var t = GetRepository<T>();
                if (t == null) throw new Exception("load Repo. Error");
                ll = t.FindAll(criteria, includes);

                return ll;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public IEnumerable<T>? TGetAll()
        {

            try
            {
                IEnumerable<T> ll = Enumerable.Empty<T>();
                //
                var t = GetRepository<T>();
                if (t == null) throw new Exception("load Repo. Error");
                ll = t.GetAll();

                return ll;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public T? TGetById(int id)
        {

            try
            {
                var t = GetRepository<T>();
                if (t == null)
                    throw new Exception("load Repo. Error");

                var ll = t.GetById(id);

                return ll;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public T? TGetById(short id)
        {

            try
            {
                var t = GetRepository<T>();
                if (t == null) throw new Exception("load Repo. Error");
                var ll = t.GetByIdShort(id);

                return ll;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public T? TGetById(long id)
        {

            try
            {
                var t = GetRepository<T>();
                if (t == null) throw new Exception("load Repo. Error");
                var ll = t.GetByIdLong(id);

                return ll;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public T? TAddEntityWithCurrentUser(T entity, int cur_u_id)
        {

            try
            {
                var t = GetRepository<T>();
                if (t == null) throw new Exception("load Repo. Error");
                var ll = t.AddEntityWithCurrentUser(entity, cur_u_id);
                _unitOfWork.Complete();
                return ll;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public T? TUpdateEntityWithCurrentUser(T entity, int cur_u_id)
        {
            try
            {
                var t = GetRepository<T>();
                if (t == null) throw new Exception("load Repo. Error");
                var ll = t.UpdateEntityWithCurrentUser(entity, cur_u_id);
                _unitOfWork.Complete();

                return ll;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public DeleteResult TDeleteRange(IEnumerable<T> entities)
        {
            try
            {



                var t = GetRepository<T>();
                if (t == null) return DeleteResult.failed;
                _unitOfWork.BeginTransaction();
                t.DeleteRange(entities);
                var r = _unitOfWork.Commit();

                return (r == true) ? DeleteResult.deleted : DeleteResult.failed;


            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return DeleteResult.failed;
            }
        }

        public DeleteResult TdeleteById(int id)
        {
            try
            {
                if (_deleteChecker is null)
                    throw new Exception("deleteChecker not found");

                var d32 = _deleteChecker;
                var chk = d32.CanDelete(id);
                if (chk == DeleteResult.can_delete)
                {
                    var t = GetRepository<T>();
                    if (t == null) return DeleteResult.failed;
                    t.DeleteByID(id);
                    _unitOfWork.Complete();
                    return DeleteResult.deleted;
                }
                else
                    return chk;

            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return DeleteResult.failed;
            }
        }

        public DeleteResult TdeleteById(short id)
        {

            try
            {
                if (_deleteChecker is null)
                    throw new Exception("deleteChecker not found");

                var chk = _deleteChecker.CanDelete(id);
                if (chk == DeleteResult.can_delete)
                {
                    var t = GetRepository<T>();
                    if (t == null) return DeleteResult.failed;
                    t.DeleteByID(id);
                    _unitOfWork.Complete();
                    return DeleteResult.deleted;
                }
                else
                    return chk;

            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return DeleteResult.failed;
            }
        }

        public bool? Tchange_Status(int id, int cur_u_id, string prop_isActive = null)
        {

            try
            {
                var t = GetRepository<T>();
                if (t == null) throw new Exception("load Repo. Error");

                var entity = t.GetById(id);
                string srch = (prop_isActive is not null) ? prop_isActive : "isActive";
                Type type = typeof(T);

                PropertyInfo property = type.GetProperty(srch);

                // Check if the property was found and is of type bool
                if (property != null && (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?)))
                {
                    // Get the current value of the property
                    bool? currentValue = (bool?)property.GetValue(entity);

                    property.SetValue(entity, (currentValue.HasValue) ? !currentValue : false);
                    var mm = _unitOfWork.GetRepository<T>().UpdateEntityWithCurrentUser(entity, cur_u_id);
                    _unitOfWork.Complete();
                    return (mm != null);

                }

                return false;
            }
            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public bool? Tchange_Status(short id, int cur_u_id, string prop_isActive = null)
        {

            try
            {
                var t = GetRepository<T>();
                if (t == null) throw new Exception("load Repo. Error");

                var entity = t.GetByIdShort(id);
                string srch = (prop_isActive is not null) ? prop_isActive : "isActive";
                Type type = typeof(T);

                PropertyInfo property = type.GetProperty(srch);

                // Check if the property was found and is of type bool
                if (property != null && (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?)))
                {
                    // Get the current value of the property
                    bool? currentValue = (bool?)property.GetValue(entity);

                    property.SetValue(entity, (currentValue.HasValue) ? !currentValue : false);
                    var mm = _unitOfWork.GetRepository<T>().UpdateEntityWithCurrentUser(entity, cur_u_id);
                    _unitOfWork.Complete();
                    return (mm != null);

                }

                return false;
            }
            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        //public TDest? MapTo<TDest>(T source) where TDest : class
        //{
        //	return _mapper.Map<TDest>(source);
        //}

        public bool? Tchange_Status(string prop_fk, object prop_val, int cur_u_id, string prop_isActive = null)
        {

            try
            {
                return true;
            }
            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public T TAddEntityWithCurrentUserTransaction(T entity, int cur_u_id)
        {

            try
            {
                var t = GetRepository<T>();
                if (t == null) return null;
                _unitOfWork.BeginTransaction();
                var ll = t.AddEntityWithCurrentUser(entity, cur_u_id);
                return ll;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public T TUpdateEntityWithCurrentUserTransaction(T entity, int cur_u_id)
        {

            try
            {
                var t = GetRepository<T>();
                if (t == null) return null;
                var ll = t.UpdateEntityWithCurrentUser(entity, cur_u_id);
                return ll;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public List<Select2Model>? GetSelectList(cur_lang lang, Expression<Func<T, bool>> criteria, bool? top_element = false, string? top_id = null, string? top_name = null)
        {

            var rslt = new List<Select2Model>();

            if (top_element.Value)
            {
                rslt.Add(new Select2Model { Id = string.IsNullOrWhiteSpace(top_id) ? string.Empty : top_id, Text = string.IsNullOrWhiteSpace(top_name) ? " " : top_name });
            }

            try
            {
                var t = GetRepository<T>();
                if (t == null) return null;
                Type type = typeof(T);
                var l = _unitOfWork.GetRepository<T>().FindAll(criteria);

                if (l.Any())
                {

                    foreach (var item in l)
                    {
                        PropertyInfo property_id = type.GetProperty("Id");
                        PropertyInfo property_NameAr = type.GetProperty("NameAr");
                        PropertyInfo property_NameEn = type.GetProperty("NameEn");

                        rslt.Add(new Select2Model()
                        {
                            Id = property_id.GetValue(item).ToString(),
                            Text =
                             $"{((lang == cur_lang.ar) ? GetFirstNOrSpaces(property_NameAr.GetValue(item).ToString(), 20) : GetFirstNOrSpaces(property_NameEn.GetValue(item).ToString(), 20))}"
                        });
                    }
                }
                return rslt;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public List<Select2Model>? GetSelectList(string PropName_Id, string PropName_Txt, Expression<Func<T, bool>> criteria, bool? top_element = false, string? top_id = null, string? top_name = null)
        {

            var rslt = new List<Select2Model>();

            if (top_element.Value)
            {
                rslt.Add(new Select2Model { Id = string.IsNullOrWhiteSpace(top_id) ? string.Empty : top_id, Text = string.IsNullOrWhiteSpace(top_name) ? " " : top_name });
            }

            try
            {
                var t = GetRepository<T>();
                if (t == null) return null;
                Type type = typeof(T);
                var l = _unitOfWork.GetRepository<T>().FindAll(criteria);

                if (l.Any())
                {

                    foreach (var item in l)
                    {
                        PropertyInfo property_id = type.GetProperty(PropName_Id);
                        PropertyInfo property_txt = type.GetProperty(PropName_Txt);

                        rslt.Add(new Select2Model()
                        {
                            Id = property_id.GetValue(item).ToString(),
                            Text = GetFirstNOrSpaces(property_txt.GetValue(item).ToString(), 20)
                        });
                    }
                }
                return rslt;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public T Tsave(T entity)
        {


            try
            {
                var t = GetRepository<T>();
                if (t == null)
                    throw new Exception("load entity error");

                _unitOfWork.GetRepository<T>()?.Add(entity);
                _unitOfWork.Complete();

                return entity;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public DeleteResult TdeleteById(long id)
        {

            try
            {
                if (_deleteChecker is null)
                    throw new Exception("deleteChecker not found");

                var chk = _deleteChecker.CanDelete(id);
                if (chk == DeleteResult.can_delete)
                {
                    var t = GetRepository<T>();
                    if (t == null) return DeleteResult.failed;
                    t.DeleteByID(id);
                    _unitOfWork.Complete();
                    return DeleteResult.deleted;
                }
                else
                    return chk;

            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return DeleteResult.failed;
            }
        }

        public DeleteResult TdeleteByIdWithChilds(short id, string[]? childern)
        {

            try
            {

                var t = GetRepository<T>();
                if (t == null) return DeleteResult.failed;
                t.DeleteByID(id);
                _unitOfWork.Complete();
                return DeleteResult.deleted;


            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return DeleteResult.failed;
            }
        }

        public DeleteResult TdeleteByIdWithChilds(int id, string[]? childern)
        {

            try
            {

                var t = GetRepository<T>();
                if (t == null) return DeleteResult.failed;
                t.DeleteByID(id);
                _unitOfWork.Complete();
                return DeleteResult.deleted;


            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return DeleteResult.failed;
            }
        }

        public DeleteResult TdeleteByIdWithChilds(long id, string[]? childern)
        {

            try
            {

                var t = GetRepository<T>();
                if (t == null) return DeleteResult.failed;
                t.DeleteByID(id);
                _unitOfWork.Complete();
                return DeleteResult.deleted;


            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return DeleteResult.failed;
            }
        }

        public IEnumerable<T>? TFindAll(Expression<Func<T, bool>> criteria, int skip, int take)
        {

            try
            {
                var t = GetRepository<T>();
                if (t == null) throw new Exception("load Repo. Error");
                var ll = t.FindAll(criteria, skip, take);
                if (ll.Any()) return ll;
                else
                    return null;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public IEnumerable<T>? TFindAll(Expression<Func<T, bool>> criteria, int? skip, int? take, Expression<Func<T, object>>? orderBy = null, string orderByDirection = "ASC")
        {

            try
            {
                var t = GetRepository<T>();
                if (t == null) throw new Exception("load Repo. Error");
                var ll = t.FindAll(criteria, skip, take, null, orderBy, orderByDirection);
                if (ll.Any()) return ll;
                else
                    return null;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public IEnumerable<T>? TFindAll(Expression<Func<T, bool>> criteria, int skip, int take, string[]? includes = null)
        {

            try
            {
                var t = GetRepository<T>();
                if (t == null) throw new Exception("load Repo. Error");
                var ll = t.FindAll(criteria, skip, take, includes);
                if (ll.Any()) return ll;
                else
                    return null;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public IEnumerable<T>? TFindAll(Expression<Func<T, bool>> criteria, int? skip, int? take, string[] includes = null, Expression<Func<T, object>>? orderBy = null, string orderByDirection = "ASC")
        {

            try
            {
                var t = GetRepository<T>();
                if (t == null) throw new Exception("load Repo. Error");
                var ll = t.FindAll(criteria, skip, take, includes, orderBy, orderByDirection);
                if (ll.Any()) return ll;
                else
                    return null;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }
        #endregion interface_implementation
    }

    public class BaseOfServiceV2RankAttribute : Attribute
    {
        public int Rank { get; }

        public BaseOfServiceV2RankAttribute(int rank)
        {
            Rank = rank;
        }

        public void ValidateUsage(Type target)
        {
            bool isDerivedFromBaseOfService = target.BaseType != null &&
                                              target.BaseType.IsGenericType &&
                                              target.BaseType.GetGenericTypeDefinition() == typeof(BaseOfService<>);

            if (!isDerivedFromBaseOfService)
            {
                throw new InvalidOperationException($"The RankAttribute can only be applied to classes derived from BaseOfService<T>. {target.Name} does not meet this criterion.");
            }
        }
    }
    public static class ServiceRegistration
    {
        public static void Register_DeleteCheckers(IServiceCollection services, Assembly assembly)
        {
            // Register DeleteService<T> implementations using reflection
            var deleteServiceTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICanDeleteChecker<>)));

            foreach (var deleteServiceType in deleteServiceTypes)
            {
                var interfaceType = deleteServiceType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICanDeleteChecker<>));
                services.AddScoped(interfaceType, deleteServiceType);
            }
        }

        public static void Register_BaseOfServiceV2(IServiceCollection services, Assembly assembly)
        {
            var baseType = typeof(BaseOfServiceV2<>);
            var derivedTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == baseType)
                .OrderBy(a => a.Name.Length).ToList();

            var typesWithRankAttribute = Assembly.GetExecutingAssembly().GetTypes()
                                                 .Where(type => Attribute.IsDefined(type, typeof(BaseOfServiceV2RankAttribute))).OrderBy(a => a.Name.Length).ToList();

            var ranked = new Dictionary<Type, int>();
            foreach (var type in typesWithRankAttribute)
            {
                // Get the rank value from the attribute
                var rankAttribute = (BaseOfServiceV2RankAttribute)Attribute.GetCustomAttribute(type, typeof(BaseOfServiceV2RankAttribute));
                if (rankAttribute != null)
                {
                    var rankValue = rankAttribute.Rank;
                    ranked.Add(type, rankValue);
                }
            }

            //services.AddScoped(typeof(IEntityMappingService<,>), typeof(EntityMappingService32<,>));

            var ordered = ranked.OrderBy(r => r.Value).ThenBy(r => r.Key.Name.Length);
            foreach (var item in ordered)
            {
                services.AddScoped(item.Key);
            }
        }
    }
}