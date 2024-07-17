using AutoMapper;
using BlazorDemoApp.API.Helpers;
using BlazorDemoApp.Core;
using BlazorDemoApp.Shared.Classes.DTO;
using BlazorDemoApp.Shared.Classes.TableClass;
using System.Reflection;

namespace BlazorDemoApp.API.Services
{
    [BaseOfServiceV2Rank(1)]
    public class ServiceOf_Add0_Gov : BaseOfServiceV2<Add0_Gov>
    {
        private IMapper _mapper;
        public ServiceOf_Add0_Gov(IUnitOfWork unitOfWork, ILogger<Add0_Gov> logger, ICanDeleteChecker<Add0_Gov> canDeleteChecker , IMapper mapper)
             : base(unitOfWork, logger, canDeleteChecker)
        {
            _mapper = mapper;
        }

        public IEnumerable<DTO_Add.DTO_Gov>? get_all()
        {
            try
            {
                var l = TGetAll();
                var dto = _mapper.Map<List<DTO_Add.DTO_Gov>>(l);
                return dto;
            }
            catch (Exception ex)
            {
                WriteLog_Service(ex, GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

    }
}
