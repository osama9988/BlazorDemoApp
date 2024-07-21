

using BlazorDemoApp.Shared;
using BlazorDemoApp.Shared.Consts;
using System.Globalization;

namespace BlazorDemoApp.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : MyAppServicesController3
    {
        private ServiceOf_Add0_Gov _cusSer;

        public AddressController(ILogger<MyAppServicesController3> logger, ServiceOf_Add0_Gov serviceOf_Add0_Gov) : base(logger, AppServices.Address)
        {
            _cusSer= serviceOf_Add0_Gov;    
        }


       // [MyPermission1(action: UserActions.Delete)]
        [HttpGet("GovGetAll")]
        public IActionResult GetAllGovs()
        {
            try
            {

                var r = _cusSer.get_all();

                if (r is null)
                    throw new Exception(load_data_error);

                return Ok(new ApiResponse<IEnumerable<DTO_Add.DTO_Gov>>() { Success = true, Data= r, DataType= r.GetType().Name });
            }
            catch (Exception ex)
            {
                WriteLog_Controller(ex, ControllerContext.ActionDescriptor.ControllerName, MethodBase.GetCurrentMethod().Name);
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = load_data_error,
                    Data = null,
                    Errors = new List<ApiError> { new ApiError { Field = "", Message = ex.Message } }
                });
            }
        }

        //if (HttpContext.Request.Headers.TryGetValue("Accept-Language", out var acceptLanguage))
        //{
        //    var culture = new CultureInfo(acceptLanguage.ToString());

        //}
        [HttpGet("GovGetAllSelect2")]
        public IActionResult GetAllGovs_Select2()
        {
            try
            {

                var thread = Thread.CurrentThread.CurrentCulture;
                var lang = GetRequestLang();
                var r = _cusSer.GetSelectList(lang, a => a.Id > 0);
                if (r is null)
                    throw new Exception(load_data_error);

                return Ok(new ApiResponse<IEnumerable<Select2Model>>() { Success = true, Data = r, DataType = r.GetType().Name });
            }
            catch (Exception ex)
            {
                WriteLog_Controller(ex, ControllerContext.ActionDescriptor.ControllerName, MethodBase.GetCurrentMethod().Name);
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = load_data_error,
                    Data = null,
                    Errors = new List<ApiError> { new ApiError { Field = "", Message = ex.Message } }
                });
            }
        }


        [HttpPost("GovPost")]
        public IActionResult GovPost(DTO_Add.DTO_GovFrm m)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .SelectMany(kvp => kvp.Value.Errors.Select(e => new ApiError { Field = kvp.Key, Message = e.ErrorMessage }))
                    .ToList();

                return BadRequest(new ApiResponse<DTO_Add.DTO_GovFrm>
                {
                    Success = false,
                    Message = "Validation errors occurred.",
                    Data = null,
                    Errors = errors
                });
            }

            try
            {
                var r = _cusSer.GovPost(m);


                if (r is null)
                    throw new Exception(load_data_error);

                return Ok(new ApiResponse<DTO_Add.DTO_GovFrm>() { Success = true, Data = r, DataType = r.GetType().Name });
            }
            catch (Exception ex)
            {
                WriteLog_Controller(ex, ControllerContext.ActionDescriptor.ControllerName, MethodBase.GetCurrentMethod().Name);
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = load_data_error,
                    Data = null,
                    Errors = new List<ApiError> { new ApiError { Field = "", Message = ex.Message } }
                });
            }

        }
    }
}
