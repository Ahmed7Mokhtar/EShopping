using MongoDB.Driver;

namespace Catalog.Core.Specifications
{
    public interface ISpecification<T>
    {
        FilterDefinition<T> Criteria { get; }
        int? Skip { get; }
        int? Take { get; }
        SortDefinition<T>? Sort { get; }
    }

    public interface ISpecificationEvaluator<T>
    {
        Task<T> ApplySpecification();
    }
}
