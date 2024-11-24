using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class BaseIntegrationEvent
    {
        public string CorrelationId { get; set; }
        public DateTimeOffset CreationDate { get; private set; }

        public BaseIntegrationEvent()
        {
            CorrelationId = Guid.NewGuid().ToString();
            CreationDate = DateTimeOffset.UtcNow;
        }

        public BaseIntegrationEvent(Guid correlationId, DateTimeOffset creationDate)
        {
            CorrelationId = correlationId.ToString();
            CreationDate = creationDate;
        }
    }
}
