namespace BlazorDemoApp.API.Helpers
{
    public class EncryptionUtility
    {
        private static IDataProtector? _dataProtector;

        public static void Initialize(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtector = dataProtectionProvider.CreateProtector(App_name);
        }

        public static string? DecryptId(string? encryptedId)
        {
            try
            {
                return (encryptedId == null) ? "0" : _dataProtector.Unprotect(encryptedId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string? EncryptId(string id)
        {
            try
            {
                return _dataProtector.Protect(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
