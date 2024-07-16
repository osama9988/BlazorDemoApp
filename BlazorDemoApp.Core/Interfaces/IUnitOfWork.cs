using BlazorDemoApp.Core.Interfaces;
using BlazorDemoApp.Shared.Classes.TableClass;
using System;
using System.Collections.Generic;

namespace BlazorDemoApp.Core
{
	public interface IUnitOfWork : IDisposable
    {
		
        bool chk_db();
        bool chk_db_Version();

        int Complete();

		void BeginTransaction();
		bool Commit();
		void Rollback();
        //
        IBaseRepository<MyAppUser> MyAppUser { get; }
        IBaseRepository<Add0_Gov> Add0_Gov { get; }
        IBaseRepository<Add1_Markaz> Add1_Markaz { get; }
        IBaseRepository<Add2_City> Add2_City { get; }

    }
}