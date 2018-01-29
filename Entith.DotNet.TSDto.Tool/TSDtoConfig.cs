using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entith.DotNet.TSDto
{
    internal class TSDtoConfig : ITSDtoConfig
    {
        private readonly List<(IEnumerable<Type> types, string output, Func<Type, string> nameGenerator)> _toConvert;

        public TSDtoConfig()
        {
            _toConvert = new List<(IEnumerable<Type> types, string output, Func<Type, string> nameGenerator)>();
        }

        public void Add(IEnumerable<Type> types, string output, Func<Type, string> nameGenerator = null)
        {
            _toConvert.Add((types, output, nameGenerator));
        }

        public IEnumerable<(IEnumerable<Type> types, string output, Func<Type, string> nameGenerator)> GetAll()
        {
            return _toConvert.AsEnumerable();
        }
    }
}
