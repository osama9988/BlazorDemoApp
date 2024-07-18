

using BlazorDemoApp.Shared.Consts;
using System.Globalization;

namespace BlazorDemoApp.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AddressController : MyAppServicesController3
    {
        private ServiceOf_Add0_Gov _cusSer;

        public AddressController(ILogger<MyAppServicesController3> logger, ServiceOf_Add0_Gov serviceOf_Add0_Gov) : base(logger, AppServices.Address)
        {
            _cusSer= serviceOf_Add0_Gov;    
        }

        [HttpGet("GetAllGovs")]
        public IEnumerable<DTO_Add.DTO_Gov>? GetAllGovs()
        {
            try
            {

                var ll = _cusSer.get_all();
                
                return (ll is null) ? null : ll;
            }
            catch (Exception ex)
            {
                WriteLog_Controller(ex, ControllerContext.ActionDescriptor.ControllerName, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        //if (HttpContext.Request.Headers.TryGetValue("Accept-Language", out var acceptLanguage))
        //{
        //    var culture = new CultureInfo(acceptLanguage.ToString());

        //}
        [HttpGet("GetAllGovsSelect2")]
        public IEnumerable<Select2Model> GetAllGovs_Select2()
        {
            try
            {

                var thread = Thread.CurrentThread.CurrentCulture;
                var lang = GetRequestLang();
                var l = _cusSer.GetSelectList(lang, a => a.Id > 0);
                return l;
            }
            catch (Exception ex)
            {
                WriteLog_Controller(ex, ControllerContext.ActionDescriptor.ControllerName, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }
    }
}
