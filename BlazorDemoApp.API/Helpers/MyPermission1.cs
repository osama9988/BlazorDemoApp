using Microsoft.AspNetCore.Mvc.Filters;

namespace BlazorDemoApp.API.Helpers
{
    public class MyPermission1
    {
        //public class MyPermission1Attribute : ActionFilterAttribute
        //{

        //    private bool rslt;
        //    private AppServices _AppServices_id;
        //    private UserActions _action;


        //    public MyPermission1Attribute(UserActions action)
        //    {
        //        _action = action;

        //    }


        //    public override void OnActionExecuting(ActionExecutingContext context)
        //    {

                

        //        //
        //        if (context.Controller is IAppServicesProvider appServicesProvider)
        //        {
        //            var _AppServices_id = (context.Controller as IAppServicesProvider).cur_AppService;

        //            rslt = context.HttpContext.Session.chk_permission(_AppServices_id, _action);


        //            if (rslt == false)
        //            {

        //                return StatusCode(500, r);

        //                context.Result = !(actionName1.Contains(aj_call)) ? new RedirectResult(final_link) :
        //                new JsonResult(new { Success = false, Message = App_Strings.NoPermissions, Message2 = "", Url = final_link });
        //                //{
        //                //	StatusCode = 401 // Unauthorized status code
        //                //};

        //            }
        //        }
        //    }
        //}
    }
}
