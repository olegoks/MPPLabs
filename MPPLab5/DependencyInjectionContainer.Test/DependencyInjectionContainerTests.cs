using System.Collections.Generic;
using System.Linq;
using DependencyInjectionContainer.DependenciesProvider;
using DependencyInjectionContainer.Enums;
using DependencyInjectionContainer.Test.TestClasses;
using NUnit.Framework;

namespace DependencyInjectionContainer.Test
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void DependencyInjectionUsingInterfaceTest()
        {
            var dependencies = new DependenciesConfig();
            dependencies.Register<IService, ServiceImpl>();
            
            var provider = new DependencyProvider(dependencies);

            var service = provider.Resolve<IService>();
            
            Assert.IsInstanceOf(typeof(ServiceImpl), service);
        }
        
        [Test]
        public void DependencyInjectionUsingAbstractClassTest()
        {
            var dependencies = new DependenciesConfig();
            dependencies.Register<AbstractService, AbstractServiceImpl>();

            var provider = new DependencyProvider(dependencies);

            var service = provider.Resolve<AbstractService>();
            
            Assert.IsInstanceOf(typeof(AbstractServiceImpl), service);
        }
        
        [Test]
        public void DependencyInjectionAsSelfTest()
        {
            var dependencies = new DependenciesConfig();
            dependencies.Register<RepositoryImpl, RepositoryImpl>();
            
            var provider = new DependencyProvider(dependencies);

            var repository = provider.Resolve<RepositoryImpl>();
            Assert.IsInstanceOf(typeof(RepositoryImpl), repository);
        }

        [Test]
        public void RecursiveDependencyInjectionTest()
        {
            var dependencies = new DependenciesConfig();
            dependencies.Register<IRepository, RepositoryImpl>();
            dependencies.Register<IService, ServiceImpl>();
            
            var provider = new DependencyProvider(dependencies);
            
            var service1 = provider.Resolve<IService>();

            Assert.NotNull(service1);
        }

        [Test]
        public void RecursiveDependencyInjectionAsSelfTest()
        {
            
            var dependencies = new DependenciesConfig();
            dependencies.Register<ServiceImpl, ServiceImpl>();
            dependencies.Register<IRepository, RepositoryImpl>();

            var provider = new DependencyProvider(dependencies);
            var serviceImpl = provider.Resolve<ServiceImpl>();

            Assert.NotNull(serviceImpl);
        }
        
        [Test]
        public void GetListDependenciesTest()
        {
            var dependencies = new DependenciesConfig();
            dependencies.Register<IService, ServiceImpl>();
            dependencies.Register<IService, ServiceImpl1>();

            var provider = new DependencyProvider(dependencies);
            var services = provider.Resolve<IEnumerable<IService>>().ToList();
            
            Assert.AreEqual(2, services.Count);
            Assert.IsInstanceOf(typeof(ServiceImpl),services[0]);
            Assert.IsInstanceOf(typeof(ServiceImpl1),services[1]);
        }
        
        [Test]
        public void SingletonScopeTest()
        {
            var dependencies = new DependenciesConfig();
            dependencies.Register<IService, ServiceImpl>(Scope.Singleton);
            
            var provider = new DependencyProvider(dependencies);

            var service1 = provider.Resolve<IService>();
            var service2 = provider.Resolve<IService>();
            
            Assert.AreEqual(service1, service2);
        }
        
        [Test]
        public void PrototypeScopeTest()
        {
            var dependencies = new DependenciesConfig();
            dependencies.Register<IService, ServiceImpl>(Scope.Prototype);
            
            var provider = new DependencyProvider(dependencies);

            var service1 = provider.Resolve<IService>();
            var service2 = provider.Resolve<IService>();
            
            Assert.AreNotEqual(service1, service2);
        }

        [Test]
        public void GenericClassTest()
        {
            var dependencies = new DependenciesConfig();
            dependencies.Register<IRepository, MySqlRepository>();
            dependencies.Register<IService<IRepository>, ServiceImpl<IRepository>>();
            dependencies.Register(typeof(IService<>), typeof(ServiceImpl<>));

            var provider = new DependencyProvider(dependencies);
            
            var service = provider.Resolve<IService<IRepository>>();
            
            Assert.IsInstanceOf(typeof(ServiceImpl<IRepository>),service);
        }
        
        [Test]
        public void GenericListTest()
        {
            var dependencies = new DependenciesConfig();
            dependencies.Register<IRepository, MySqlRepository>();
            dependencies.Register<IService<IRepository>, ServiceImpl<IRepository>>();
            dependencies.Register<IService<IRepository>, ServiceImpl1<IRepository>>();
            
            var provider = new DependencyProvider(dependencies);
            
            var serviceList = provider.Resolve<IEnumerable<IService<IRepository>>>().ToList();
            
            Assert.True( serviceList.Count == 2);
            Assert.IsInstanceOf(typeof(ServiceImpl<IRepository>),serviceList[0]);
            Assert.IsInstanceOf(typeof(ServiceImpl1<IRepository>),serviceList[1]);
        }
        
        [Test]
        public void OrderMarkedDependencyTest()
        {
            var dependencies = new DependenciesConfig();
            
            dependencies.Register<IService, ServiceImpl>(Scope.Singleton, OrdinalMark.Second);
            dependencies.Register<IService, ServiceImpl1>(Scope.Singleton, OrdinalMark.First);
            dependencies.Register<IService, ServiceImpl2>();
            
            var provider = new DependencyProvider(dependencies);
            
            var serviceSecond = provider.Resolve<IService>(OrdinalMark.Second);
            var serviceFirst = provider.Resolve<IService>(OrdinalMark.First);
            var service = provider.Resolve<IService>();
            
            Assert.IsInstanceOf(typeof(ServiceImpl), serviceSecond);
            Assert.IsInstanceOf(typeof(ServiceImpl1), serviceFirst);
            Assert.IsInstanceOf(typeof(ServiceImpl2), service);
        }
        
        [Test]
        public void OrderMarkedGenericDependencyTest()
        {
            var dependencies = new DependenciesConfig();
           
            dependencies.Register<IRepository,RepositoryImpl>();
            dependencies.Register<IService<IRepository>, ServiceImpl<IRepository>>();
            dependencies.Register<IService<IRepository>, ServiceImpl1<IRepository>>(Scope.Singleton, OrdinalMark.First);
            
            var provider = new DependencyProvider(dependencies);
            
            var service2 = provider.Resolve<IService<IRepository>>(OrdinalMark.First);
            
            Assert.IsInstanceOf(typeof(ServiceImpl1<IRepository>),service2);
        }
        
        [Test]
        public void OrderMarkedListDependencyTest()
        {
            var dependencies = new DependenciesConfig();
           
            dependencies.Register<IService, ServiceImpl>(Scope.Singleton, OrdinalMark.Second);
            dependencies.Register<IService, ServiceImpl1>(Scope.Singleton, OrdinalMark.Second);
            dependencies.Register<IService, ServiceImpl2>();
            
            var provider = new DependencyProvider(dependencies);
            
            var service2 = provider.Resolve<IEnumerable<IService>>(OrdinalMark.Second).ToList();
            
            Assert.AreEqual(2, service2.Count);
            Assert.IsInstanceOf(typeof(IEnumerable<IService>),service2);
        }
        
    }
}