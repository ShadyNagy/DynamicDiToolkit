using Ardalis.Specification;

namespace DynamicDiToolkit.Interfaces;
public interface IRepository<T> : IRepositoryBase<T> where T : class
{
}
