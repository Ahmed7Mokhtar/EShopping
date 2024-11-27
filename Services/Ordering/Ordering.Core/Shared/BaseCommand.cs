using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Core.Shared
{
    public abstract class BaseCommand
    {
        public object? Model { get; protected set; }
    }

    public abstract class BaseCommand<T> : BaseCommand
    {
        public new T? Model { get => (T?)base.Model; set => base.Model = value; }
    }
}
