using System;
using System.Data;

namespace Core.Interfaces.Repository
{
    public interface IUnitOfWork : IDisposable
	{

		void Begin();
		void Commit();

	}
}
