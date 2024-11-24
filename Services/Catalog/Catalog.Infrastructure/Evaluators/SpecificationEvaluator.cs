using Catalog.Core.Specifications;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Evaluators
{
    public static class SpecificationEvaluator<T> where T : class
    {
        public static async Task<IReadOnlyList<T>> GetQuery(IMongoCollection<T> mongoCollection, ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var query = mongoCollection.Find(spec.Criteria);

            if (spec.Sort != null)
            {
                query = query.Sort(spec.Sort);
            }

            if (spec.Skip.HasValue)
            {
                query = query.Skip(spec.Skip.Value);
            }

            if (spec.Take.HasValue)
            {
                query = query.Limit(spec.Take.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}
