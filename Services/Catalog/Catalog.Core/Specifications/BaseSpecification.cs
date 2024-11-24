using Catalog.Core.Specifications;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public FilterDefinition<T> Criteria { get; private set; } = Builders<T>.Filter.Empty;
        public int? Skip { get; private set; }
        public int? Take { get; private set; }
        public SortDefinition<T>? Sort { get; private set; }

        protected void AddCriteria(FilterDefinition<T> criteria)
        {
            Criteria = Criteria == Builders<T>.Filter.Empty
                ? criteria
                : Builders<T>.Filter.And(Criteria, criteria);
        }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }

        protected void ApplySorting(SortDefinition<T> sort)
        {
            Sort = sort;
        }
    }
}
