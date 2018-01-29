using System;
using System.Collections.Generic;
using System.Text;

namespace Entith.DotNet.TSDto
{
    public interface ITSDtoConfig
    {
        void Add(IEnumerable<Type> types, string output, Func<Type, string> nameGenerator = null);
    }
}
