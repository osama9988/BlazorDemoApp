

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

        [HttpGet("GetAllGovsSelect2")]
        public IActionResult GetAllGovs_Select2()
        {
            try
            {
                var currentCulture = CultureInfo.CurrentCulture;

                var thread = Thread.CurrentThread.CurrentCulture;
              

                return null;
            }
            catch (Exception ex)
            {
                WriteLog_Controller(ex, ControllerContext.ActionDescriptor.ControllerName, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }
    }
}
