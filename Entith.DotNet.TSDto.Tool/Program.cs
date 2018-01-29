using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Entith.DotNet.TSDto
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
                throw new ArgumentException("One argument expected.");

            var config = new TSDtoConfig();

            using (var dynamicContext = new AssemblyResolver(args[0]))
            {
                foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var t in a.GetTypes().Where(t => t.GetInterfaces().Any(i => i == typeof(ITSDtoSetup))))
                    {
                        var setup = Activator.CreateInstance(t) as ITSDtoSetup;

                        setup.Setup(config);
                    }
                }
            }


            foreach (var c in config.GetAll())
            {
                var context = new ConversionContext(c.types, c.output, c.nameGenerator);
                var tsTypes = context.GetTsTypes();

                var sb = new StringBuilder();
                foreach(var type in tsTypes)
                {
                    sb.AppendLine(type.ToString());
                }

                File.WriteAllText(c.output, sb.ToString());
            }
        }
    }
}
