//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Momento.Tests.Utilities
//{
//    using Microsoft.Extensions.DependencyInjection;
//    using Momento.Data;

//    public class TestDataConfiguration
//    {
//        public static MomentoDbContext GetContex()
//        {
//            var serviceCollection = new ServiceCollection();
//            IocConfig.RegisterContext(serviceCollection, "", null);
//            var serviceProvider = serviceCollection.BuildServiceProvider();
//            return serviceProvider.GetService<IMyContext>();
//        }
//    }
//}
