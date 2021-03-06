﻿using System;
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
			_kernel.Bind<IOrderListRepository>().To<EFOrderListRepository>();
			_kernel.Bind<IRoleRepository>().To<EFRoleRepository>();
			_kernel.Bind<IUserRepository>().To<EFUserRepository>();
			_kernel.Bind<ICityRepository>().To<EFCityRepository>();
			_kernel.Bind<IStatusRepository>().To<EFStatusRepository>();
		    _kernel.Bind<IAuthentication>().To<Authentication>();
		    _kernel.Bind<IEmailSender>().To<EmailSender>();
		    _kernel.Bind<IOrderProcessor>().To<OrderProcesssor>();
		}
	}

}
