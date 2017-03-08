using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Moq;
using Ninject;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;

namespace SportsStore.WebUI.Infrastructure
{

	public class NinjectDependencyResolver : IDependencyResolver
	{
		IKernel _kernel;

		public NinjectDependencyResolver(IKernel kernel)
		{
			_kernel = kernel;
			AddBindings();
		}

		public object GetService(Type serviceType)
		{
			return _kernel.TryGet(serviceType);
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			return _kernel.GetAll(serviceType);
		}

		void AddBindings()
		{
			_kernel.Bind<IProductRepository>().To<EFProductRepository>();
			_kernel.Bind<IAddressRepository>().To<EFAddressRepository>();
			_kernel.Bind<ICategoryRepository>().To<EFCategoryRepository>();
			_kernel.Bind<IOrderRepository>().To<EFOrderRepository>();
			_kernel.Bind<IRoleRepository>().To<EFRoleRepository>();
			_kernel.Bind<IUserRepository>().To<EFUserRepository>();
			_kernel.Bind<ICityRepository>().To<EFCityRepository>();

			var emailSettings = new EmailSettings
			{
				WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Emaill.WriteAsFile"] ?? "false")
			};

			_kernel.Bind<IOrderProcessor>().To<EmailOrderProcesssor>().WithConstructorArgument("emailSettings", emailSettings);
		}
	}

}
