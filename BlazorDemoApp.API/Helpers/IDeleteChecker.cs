using BlazorDemoApp.Shared.Interface;
using static BlazorDemoApp.Shared.App_Strings;

namespace BlazorDemoApp.API.Helpers
{
    public interface ICanDeleteChecker<T> where T : class, ITable
    {
        DeleteResult CanDelete<TId>(TId id);
    }


}
