namespace DependencyInjectionContainer.Test.TestClasses
{
    interface IService<TRepository> where TRepository : IRepository
    {
    }

    class ServiceImpl<TRepository> : IService<TRepository> 
        where TRepository : IRepository
    {
        public ServiceImpl(TRepository repository)
        {
        }
    }
    
    
    class ServiceImpl1<TRepository> : IService<TRepository> 
        where TRepository : IRepository
    {
        private IRepository _repository;

        public ServiceImpl1(TRepository repository)
        {
            _repository = repository;
        }
    }
}