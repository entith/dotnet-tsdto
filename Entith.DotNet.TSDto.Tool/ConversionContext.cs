using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entith.DotNet.TSDto
{
    internal class ConversionContext
    {
        private readonly IEnumerable<Type> _types;
        private readonly string _outputFile;
        private readonly Func<Type, string> _nameGenerator;

        internal ConversionContext(IEnumerable<Type> types, string outputFile, Func<Type, string> nameGenerator = null)
        {
            _types = types;
            _outputFile = outputFile;
            _nameGenerator = nameGenerator ?? ((t) => t.Name);
        }

        internal bool HasType(Type type)
        {
            return _types.Any(t => t == type);
        }

        internal IEnumerable<TsType> GetTsTypes()
        {
            foreach(var type in _types)
                yield return new TsType(type, this);
        }

        internal string GenerateName(Type type)
        {
            return _nameGenerator(type);
        }
    }
}
