using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System;
using BlazorDemoApp.Shared;
using static BlazorDemoApp.Shared.App_Strings;
using System.Globalization;
using System.Threading;

namespace BlazorDemoApp.API.Helpers
{
    public interface IAppServicesProvider
    {
        AppServices cur_AppService { get; }

    }

    [ApiController]
    [Route("Basecontroller")]
    public abstract class MyLoggerbaseController : ControllerBase
    {
        protected readonly ILogger<MyLoggerbaseController> _logger;

        public MyLoggerbaseController(ILogger<MyLoggerbaseController> logger)
        {
            _logger = logger;
        }

        protected MyJsonRsult Get_JsonResultOfDelete(DeleteResult value)
        {
            if (value == DeleteResult.deleted) return  new MyJsonRsult() { Success = true, Message = str_delete_yes, Message2 = "", Url = "", Deleted = true };
            //

            if (value == DeleteResult.failed) return new MyJsonRsult() { Success = false, Message = load_data_error, Message2 = "", Url = "" };
            if (value == DeleteResult.cannot_delete) return new MyJsonRsult() { Success = false, Message = str_delete_edit_cannot, Message2 = "", Url = "", Deleted = false };
             return new MyJsonRsult() { Success = false, Message = str_delete_no, Message2 = "", Url = "" };
        }

        protected MyJsonRsult Get_JsonResult(UserActions _action, bool? value)
        {
            if (value.HasValue == false)

                return  new MyJsonRsult() { Success = false, Message = load_data_error, Message2 = "", Url = "" };

            else if (value.HasValue && value.Value)
            {
                string msg = _action == UserActions.Insert ? str_save_yes : _action == UserActions.Update ? str_edit_yes : str_delete_yes;
                //
                return new MyJsonRsult() { Success = true, Message = msg, Message2 = "", Url = "" };
            }
            else if (value.HasValue && value.Value == false)
            {
                string msg = _action == UserActions.Insert ? str_save_no : _action == UserActions.Update ? str_edit_no : str_delete_no;
                //
                return new MyJsonRsult() { Success = false, Message = msg, Message2 = "", Url = "" };

            }
            //
            return new MyJsonRsult() { Success = false, Message = load_data_error, Message2 = "", Url = "" };
        }

        
        protected MyJsonRsult Get_JsonResult(UserActions _action, bool? value, string? msg2, string? url)
        {
            if (value.HasValue == false)

                return new MyJsonRsult() { Success = false, Message = load_data_error, Message2 = string.IsNullOrEmpty(msg2) ? "" : msg2, Url = string.IsNullOrEmpty(url) ? "" : url };

            else if (value.HasValue && value.Value)
            {
                string msg = _action == UserActions.Insert ? str_save_yes : _action == UserActions.Update ? str_edit_yes : str_delete_yes;
                //
                return new MyJsonRsult() { Success = true, Message = msg, Message2 = string.IsNullOrEmpty(msg2) ? "" : msg2, Url = string.IsNullOrEmpty(url) ? "" : url };
            }
            else if (value.HasValue && value.Value == false)
            {
                string msg = _action == UserActions.Insert ? str_save_no : _action == UserActions.Update ? str_edit_no : str_delete_no;
                //
                return new MyJsonRsult() { Success = false, Message = msg, Message2 = string.IsNullOrEmpty(msg2) ? "" : msg2, Url = "" };

            }
            //
            return new MyJsonRsult() { Success = false, Message = load_data_error, Message2 = string.IsNullOrEmpty(msg2) ? "" : msg2, Url = string.IsNullOrEmpty(url) ? "" : url };
        }

        protected MyJsonRsult Get_JsonResult(bool value, string msg, string? msg2, string? url)
        {
            var sucess = value;
            return new MyJsonRsult() { Success = sucess, Message = load_data_error, Message2 = string.IsNullOrEmpty(msg2) ? "" : msg2, Url = string.IsNullOrEmpty(url) ? "" : url };
        }

        protected void WriteLog_Controller(Exception ex, string ControllerName, string ActionName)
        {
            _logger.LogInformation(
                    $"********** Logging New Error of Controllers ********** \n" +
                    $"Error on : Controller:{ControllerName} -- Function: {ActionName} \n" +
                    $"Current_Exception : {ex.Message} \n" +
                    $"Inner Exception: {ex.InnerException?.Message ?? "No Inner Exception"}  \n" +
                    $"Done at:  {DateTime.Now.ToLongTimeString()} \n" +
                    $"********** End of Error **********");
        }


        protected MyJsonRsult GeneralCatch(Exception ex, string ControllerName, string ActionName, [Optional] string? msg, [Optional] string? url)
        {
            WriteLog_Controller(ex, ControllerName, ActionName);
            //
            var cur_msg = string.IsNullOrEmpty(msg) ? load_data_error : msg;
            //var _url = url; //(string.IsNullOrEmpty(url)) ? Url.Action(nameof(AdminController.Index), Nameof<AdminController>()) :
            //return ActionName.Contains(aj_call) ? Get_JsonResult(false, load_data_error, "", _url) : RedirectToAction(_url);
            return new MyJsonRsult()  { Success = false, Message = load_data_error, Message2 = "",  Url=url };
        }
    }


    [ApiController]
    [Route("Basecontroller1")]
    public class MyAppServicesController3 : MyLoggerbaseController, IAppServicesProvider
    {

        public AppServices _AppServices_id;

        public MyAppServicesController3(ILogger<MyAppServicesController3> logger, AppServices appServices) : base(logger)
        {

            _AppServices_id = appServices;
        }
        public AppServices cur_AppService => _AppServices_id;

        protected cur_lang GetRequestLang()
        {
            cur_lang lang = cur_lang.ar;
            if (HttpContext.Request.Headers.ContainsKey("Accept-Language"))
            {
                var acceptLanguage = HttpContext.Request.Headers["Accept-Language"].ToString();
                var cultureName = acceptLanguage.Split(',')[0]; // Take the first language tag
                var culture = new CultureInfo(cultureName);

                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;

                // Setting culture for the thread
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;


                var thread = Thread.CurrentThread.CurrentCulture;
                 lang = (thread.Name == null) ? cur_lang.ar : (thread.Name.ToLower().Contains("en")) ?  cur_lang.en : cur_lang.ar;

            }
            return lang;
        }
    }
}
