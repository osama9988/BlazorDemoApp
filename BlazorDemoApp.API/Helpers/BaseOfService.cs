using AutoMapper;
using BlazorDemoApp.Core;
using BlazorDemoApp.Core.Interfaces;
using BlazorDemoApp.Shared.Consts;
using static BlazorDemoApp.Shared.App_Strings;
using System.Linq.Expressions;
using BlazorDemoApp.Shared.Interface;
using System.Reflection;

namespace BlazorDemoApp.API.Helpers
{
    public interface IBaseOfService
    {
        void WriteLog_Service(Exception ex, string ServiceName, string MethodName);
    }

    public class BaseOfService0 : IBaseOfService
    {

        public ILogger<BaseOfService0> _Logger;
        public BaseOfService0(ILogger<BaseOfService0> logger)
        { _Logger = logger; }

        public void WriteLog_Service(Exception ex, string ServiceName, string MethodName)
        {
            _Logger.LogInformation(
                    $"********** Logging New Error of Services ********** \n" +
                    $"Error on : Service:{ServiceName} -- Method: {MethodName} \n" +
                    $"Current_Exception : {ex.Message} \n" +
                    $"Inner Exception: {ex.InnerException?.Message ?? "No Inner Exception"}  \n" +
                    $"Done at:  {DateTime.Now.ToLongTimeString()} \n" +
                    $"********** End of Error ********** \n" +
                    $"////////////////////////////////////////////////////////////////////////////////////// \n");
            ;
        }

    }

    public class BaseOfService : BaseOfService0
    {
        public IUnitOfWork _unitOfWork;
        public IMapper? _mapper;




        protected BaseOfService(IUnitOfWork unitOfWork, ILogger<BaseOfService> logger) : base(logger)
        {
            _unitOfWork = unitOfWork;
        }
        protected BaseOfService(IUnitOfWork unitOfWork, ILogger<BaseOfService> logger, IMapper mapper) : base(logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
    }

    public interface IBaseOfService<T>
    {
        IBaseRepository<T>? GetRepository<T>() where T : class, ITable;
        IEnumerable<T>? TFindAll(Expression<Func<T, bool>> criteria, string[] includes = null);
        IEnumerable<T>? TFindAll(Expression<Func<T, bool>> criteria, int skip, int take);
        IEnumerable<T>? TFindAll(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>>? orderBy = null, string orderByDirection = OrderBy.Ascending);
        IEnumerable<T>? TFindAll(Expression<Func<T, bool>> criteria, int skip, int take, string[]? includes = null);
        IEnumerable<T>? TFindAll(Expression<Func<T, bool>> criteria, int? take, int? skip, string[] includes = null,
            Expression<Func<T, object>>? orderBy = null, string orderByDirection = OrderBy.Ascending);
        T? TGetById(int id);
        T? TGetById(short id);
        T? TGetById(long id);
        T? TAddEntityWithCurrentUser(T entity, int cur_u_id);
        T TAddEntityWithCurrentUserTransaction(T entity, int cur_u_id);
        T? TUpdateEntityWithCurrentUser(T entity, int cur_u_id);
        T TUpdateEntityWithCurrentUserTransaction(T entity, int cur_u_id);
        DeleteResult TdeleteById(int id);
        DeleteResult TdeleteById(short id);

        DeleteResult TdeleteById(long id);

        DeleteResult TdeleteByIdWithChilds(short id, string[]? childern);
        DeleteResult TdeleteByIdWithChilds(int id, string[]? childern);
        DeleteResult TdeleteByIdWithChilds(long id, string[]? childern);
        bool? Tchange_Status(string prop_fk_name, object prop_fk_val, int cur_u_id, string prop_isActive = null);
        bool? Tchange_Status(int id, int cur_u_id, string prop_isActive = null);
        bool? Tchange_Status(short id, int cur_u_id, string prop_isActive = null);

        TDest? MapTo<TDest>(T source) where TDest : class;

        List<Select2Model>? GetSelectList(cur_lang lang, Expression<Func<T, bool>> criteria, bool? top_element = false, string? top_id = null, string? top_name = null);
        List<Select2Model>? GetSelectList(string PropName_Id, string PropName_Txt, Expression<Func<T, bool>> criteria, bool? top_element = false, string? top_id = null, string? top_name = null);

        T? Tsave(T entity);

        int? TCount(Expression<Func<T, bool>> criteria);
    }

    public abstract class BaseOfService<T> : BaseOfService, IBaseOfService<T> where T : class, ITable
    {
        private ICanDeleteChecker<T> _deleteChecker;
        public BaseOfService(IUnitOfWork unitOfWork, ILogger<BaseOfService> logger, IMapper mapper, ICanDeleteChecker<T> canDelete) : base(unitOfWork, logger)
        {
            _deleteChecker = canDelete;
            var derivedType = GetType();
            
        }
       
        ///
        //
        //
        public IBaseRepository<T1>? GetRepository<T1>() where T1 : class,ITable
        {
            var currentInterface = GetType();
            try
            {
                return (IBaseRepository<T1>?)_unitOfWork.GetRepository<T>();
            }
            catch (Exception ex)
            {
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public int? TCount(Expression<Func<T, bool>> criteria)
        {
            var currentInterface = GetType();
            try
            {
                var t = GetRepository<T>();
                if (t == null) throw new Exception("load Repo. Error");
                var ll = t.Count(criteria);

                return ll;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public IEnumerable<T>? TFindAll(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            var currentInterface = GetType();
            try
            {
                var t = GetRepository<T>();
                if (t == null) throw new Exception("load Repo. Error");
                var ll = t.FindAll(criteria, includes).AsEnumerable();

                return ll;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public T? TGetById(int id)
        {
            var currentInterface = GetType();
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
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public T? TGetById(short id)
        {
            var currentInterface = GetType();
            try
            {
                var t = GetRepository<T>();
                if (t == null) throw new Exception("load Repo. Error");
                var ll = t.GetByIdShort(id);

                return ll;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public T? TGetById(long id)
        {
            var currentInterface = GetType();
            try
            {
                var t = GetRepository<T>();
                if (t == null) throw new Exception("load Repo. Error");
                var ll = t.GetByIdLong(id);

                return ll;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public T? TAddEntityWithCurrentUser(T entity, int cur_u_id)
        {
            var currentInterface = GetType();
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
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public T? TUpdateEntityWithCurrentUser(T entity, int cur_u_id)
        {
            var currentInterface = GetType();
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
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public DeleteResult TdeleteById(int id)
        {
            var currentInterface = GetType();
            try
            {
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
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return DeleteResult.failed;
            }
        }

        public DeleteResult TdeleteById(short id)
        {
            var currentInterface = GetType();
            try
            {
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
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return DeleteResult.failed;
            }
        }

        public bool? Tchange_Status(int id, int cur_u_id, string prop_isActive = null)
        {
            var currentInterface = GetType();
            try
            {
                var t = GetRepository<T>();
                if (t == null) throw new Exception("load Repo. Error");

                var entity = t.GetById(id);
                string srch = prop_isActive is not null ? prop_isActive : "isActive";
                Type type = typeof(T);

                PropertyInfo property = type.GetProperty(srch);

                // Check if the property was found and is of type bool
                if (property != null && (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?)))
                {
                    // Get the current value of the property
                    bool? currentValue = (bool?)property.GetValue(entity);

                    property.SetValue(entity, currentValue.HasValue ? !currentValue : false);
                    var mm = _unitOfWork.GetRepository<T>().UpdateEntityWithCurrentUser(entity, cur_u_id);
                    _unitOfWork.Complete();
                    return mm != null;

                }

                return false;
            }
            catch (Exception ex)
            {
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public bool? Tchange_Status(short id, int cur_u_id, string prop_isActive = null)
        {
            var currentInterface = GetType();
            try
            {
                var t = GetRepository<T>();
                if (t == null) throw new Exception("load Repo. Error");

                var entity = t.GetByIdShort(id);
                string srch = prop_isActive is not null ? prop_isActive : "isActive";
                Type type = typeof(T);

                PropertyInfo property = type.GetProperty(srch);

                // Check if the property was found and is of type bool
                if (property != null && (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?)))
                {
                    // Get the current value of the property
                    bool? currentValue = (bool?)property.GetValue(entity);

                    property.SetValue(entity, currentValue.HasValue ? !currentValue : false);
                    var mm = _unitOfWork.GetRepository<T>().UpdateEntityWithCurrentUser(entity, cur_u_id);
                    _unitOfWork.Complete();
                    return mm != null;

                }

                return false;
            }
            catch (Exception ex)
            {
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public TDest? MapTo<TDest>(T source) where TDest : class
        {
            return _mapper.Map<TDest>(source);
        }

        public bool? Tchange_Status(string prop_fk, object prop_val, int cur_u_id, string prop_isActive = null)
        {
            var currentInterface = GetType();
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public T TAddEntityWithCurrentUserTransaction(T entity, int cur_u_id)
        {
            var currentInterface = GetType();
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
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public T TUpdateEntityWithCurrentUserTransaction(T entity, int cur_u_id)
        {
            var currentInterface = GetType();
            try
            {
                var t = GetRepository<T>();
                if (t == null) return null;
                var ll = t.UpdateEntityWithCurrentUser(entity, cur_u_id);
                return ll;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public List<Select2Model>? GetSelectList(cur_lang lang, Expression<Func<T, bool>> criteria, bool? top_element = false, string? top_id = null, string? top_name = null)
        {
            var currentInterface = GetType();
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
                             $"{(lang == cur_lang.ar ? GetFirstNOrSpaces(property_NameAr.GetValue(item).ToString(), 20) : GetFirstNOrSpaces(property_NameEn.GetValue(item).ToString(), 20))}"
                        });
                    }
                }
                return rslt;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public List<Select2Model>? GetSelectList(string PropName_Id, string PropName_Txt, Expression<Func<T, bool>> criteria, bool? top_element = false, string? top_id = null, string? top_name = null)
        {
            var currentInterface = GetType();
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
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public T Tsave(T entity)
        {
            var currentInterface = GetType();

            try
            {
                var t = GetRepository<T>();
                if (t == null)
                    throw new Exception("load entity error");
                _unitOfWork.Complete();

                return entity;
            }

            catch (Exception ex)
            {
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public DeleteResult TdeleteById(long id)
        {
            var currentInterface = GetType();
            try
            {
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
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return DeleteResult.failed;
            }
        }

        public DeleteResult TdeleteByIdWithChilds(short id, string[]? childern)
        {
            var currentInterface = GetType();
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
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return DeleteResult.failed;
            }
        }

        public DeleteResult TdeleteByIdWithChilds(int id, string[]? childern)
        {
            var currentInterface = GetType();
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
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return DeleteResult.failed;
            }
        }

        public DeleteResult TdeleteByIdWithChilds(long id, string[]? childern)
        {
            var currentInterface = GetType();
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
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return DeleteResult.failed;
            }
        }

        public IEnumerable<T>? TFindAll(Expression<Func<T, bool>> criteria, int skip, int take)
        {
            var currentInterface = GetType();
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
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public IEnumerable<T>? TFindAll(Expression<Func<T, bool>> criteria, int? skip, int? take, Expression<Func<T, object>>? orderBy = null, string orderByDirection = "ASC")
        {
            var currentInterface = GetType();
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
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public IEnumerable<T>? TFindAll(Expression<Func<T, bool>> criteria, int skip, int take, string[]? includes = null)
        {
            var currentInterface = GetType();
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
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public IEnumerable<T>? TFindAll(Expression<Func<T, bool>> criteria, int? skip, int? take, string[] includes = null, Expression<Func<T, object>>? orderBy = null, string orderByDirection = "ASC")
        {
            var currentInterface = GetType();
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
                WriteLog_Service(ex, currentInterface.Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }
    }
}
