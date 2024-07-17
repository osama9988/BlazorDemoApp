using BlazorDemoApp.API.Helpers;
using BlazorDemoApp.Core;
using BlazorDemoApp.Shared.Classes.TableClass;
using System.Reflection;
using static BlazorDemoApp.Shared.App_Strings;

namespace BlazorDemoApp.API.Services.IdeleteChecker
{
    public class DeleteCheckerOf_Add0_Gov : BaseOfService, ICanDeleteChecker<Add0_Gov>
    {


        public DeleteCheckerOf_Add0_Gov(IUnitOfWork unitOfWork, ILogger<BaseOfService> logger) : base(unitOfWork, logger)
        {

        }
        public DeleteResult CanDelete<TId>(TId id)
        {
            try
            {
                //if (chk2(id)) goto Fail;
                int _id = Convert.ToInt32(id);
                if (chk1(_id)) goto Fail;
                //if (chk2(_id)) goto Fail;



                return DeleteResult.can_delete;


            Fail:
                return DeleteResult.cannot_delete;
            }
            catch
            {

                return DeleteResult.failed;
            }


        }
        private bool chk1(int id)
        {
            var currentInterface = this.GetType();
            try
            {
                var l = _unitOfWork.GetRepository<Add1_Markaz>()?.FindAll(a => a.IdFK_Gov == id);
                return l.Any();
            }
            catch (Exception ex)
            {
                WriteLog_Service(ex, GetType().Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
    }
}
