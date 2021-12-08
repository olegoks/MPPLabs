using System;

namespace DependencyInjectionContainer.Test.TestClasses
{
    interface IService
    {
        object GetTestFieldValue();
    }
    class ServiceImpl : IService
    {
        public IRepository Repository { get; }

        public ServiceImpl(IRepository repository)
        {
            Repository = repository;
        }

        public object GetTestFieldValue()
        {
            return Repository;
        }
    }
    class ServiceImpl1 : IService
    {
        private readonly IRepository _repository;
        
        public ServiceImpl1(IRepository repository)
        {
            _repository = repository;
        }


        public object GetTestFieldValue()
        {
            return _repository;
        }
    }
    
    class ServiceImpl2 : IService
    {
        private readonly IRepository _repository;
        
        public ServiceImpl2(IRepository repository)
        {
            _repository = repository;
        }


        public object GetTestFieldValue()
        {
            return _repository;
        }
    }

    public abstract class AbstractService : IService
    {
        public object GetTestFieldValue()
        {
            return null;
        }
    }

    public class AbstractServiceImpl : AbstractService
    {
        private readonly IRepository _repository;
        
        public AbstractServiceImpl(IRepository repository)
        {
            _repository = repository;
        }
    }

    public interface IRepository{}
    public class RepositoryImpl : IRepository
    {
        public RepositoryImpl()
        {
        }
    }
    
    public class MySqlRepository : IRepository
    {
        public MySqlRepository()
        {
        }
    }
}