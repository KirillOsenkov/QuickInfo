using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QuickInfo
{
    public class Engine
    {
        private List<IProcessor> processors = new List<IProcessor>();

        public Engine() : this(new[] { typeof(Engine).Assembly })
        {
        }

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

            SortProcessors();
        }

        public Engine(params Type[] types)
        {
            var instances = types.Select(t => (IProcessor)Activator.CreateInstance(t));
            processors.AddRange(instances);
            SortProcessors();
        }

        private void SortProcessors()
        {
            processors.Sort((l, r) => string.CompareOrdinal(l.GetType().Name, r.GetType().Name));
        }

        public IEnumerable<IProcessor> Processors => processors;

        public IEnumerable<(string processorName, object resultNode)> GetResults(Query query)
        {
            List<(string processorName, object resultNode)> results = new List<(string, object)>();
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
