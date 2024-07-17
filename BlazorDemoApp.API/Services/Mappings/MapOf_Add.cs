



namespace BlazorDemoApp.API.Services.Mappings
{
    public class MapOf_Add : Profile
    {
        //private readonly ServiceOf_Users _myService;

        public MapOf_Add(IDataProtectionProvider dataProtectionProvider)
        {
            EncryptionUtility.Initialize(dataProtectionProvider);
            //_myService = myService ?? throw new ArgumentNullException(nameof(myService));
            ////
            ///
            CreateMap<Add0_Gov, DTO_Add.DTO_Gov>()
            .ForMember(dest => dest.Key, opt => opt.MapFrom(src => EncryptionUtility.EncryptId(src.Id.ToString())));
            //.AfterMap((src, dest) => { dest.id= 0; });
        }
    }
}
