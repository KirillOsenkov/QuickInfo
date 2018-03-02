using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QuickInfo
{
    public class Engine
    {
        private List<IProcessor> processors = new List<IProcessor>();

        public Engine(params Assembly[] assemblies)
        {
            if (assemblies.Length == 0)
            {
                assemblies = new[] { typeof(Engine).Assembly };
            }

            foreach (var assembly in assemblies)
            {
                var processorTypes = assembly
                    .GetTypes()
                    .Where(t => t.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IProcessor)));
                processors.AddRange(processorTypes.Select(t => (IProcessor)Activator.CreateInstance(t)));
            }

            processors.Sort((l, r) => string.CompareOrdinal(l.GetType().Name, r.GetType().Name));
        }

        public IEnumerable<IProcessor> Processors => processors;

        public List<(string processorName, object resultText)> GetResults(Query query)
        {
            List<(string processorName, object resultText)> results = new List<(string, object)>();
            foreach (var processor in Processors)
            {
                var result = processor.GetResult(query);
                if (result != null)
                {
                    results.Add((processor.GetType().Name, result));
                }
            }

            return results;
        }
    }
}
