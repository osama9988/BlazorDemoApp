using AutoMapper;
using BlazorDemoApp.API.Helpers;
using BlazorDemoApp.Core;
using BlazorDemoApp.Shared.Classes.DTO;
using BlazorDemoApp.Shared.Classes.TableClass;
using System;
using System.Reflection;
using System.Web.Http.ModelBinding;

namespace BlazorDemoApp.API.Services
{
    [BaseOfServiceV2Rank(1)]
    public class ServiceOf_Add0_Gov : BaseOfServiceV2<Add0_Gov>
    {
        private IMapper _mapper;
        public ServiceOf_Add0_Gov(IUnitOfWork unitOfWork, ILogger<Add0_Gov> logger, ICanDeleteChecker<Add0_Gov> canDeleteChecker, IMapper mapper)
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

        internal DTO_Add.DTO_GovFrm? GovPost(DTO_Add.DTO_GovFrm m)
        {
            try
            {
                var decryptedId = Convert.ToInt32(EncryptionUtility.DecryptId(m.Key));
                var cur_u_id = 1;

                var item = (decryptedId == 0) ? new Add0_Gov() : TGetById(decryptedId);
                item = _mapper.Map<Add0_Gov>(m);

                var r = (decryptedId == 0) ? TAddEntityWithCurrentUser(item, cur_u_id) : TAddEntityWithCurrentUser(item, cur_u_id);

               if (r is not null)
                    return _mapper.Map<DTO_Add.DTO_GovFrm>(r);
                else
                    throw new Exception(load_data_error);
            }
            catch (Exception ex)
            {
                WriteLog_Service(ex, GetType().Name, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }
    }
}
