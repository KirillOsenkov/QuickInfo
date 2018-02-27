using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QuickInfo
{
    public class Engine
    {
        private List<IProcessor> processors = new List<IProcessor>();

        public Engine()
        {
            var assembly = typeof(Engine).GetTypeInfo().Assembly;
            var processorTypes = assembly.GetTypes()
                .Where(t => t.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IProcessor)));

            processors.AddRange(processorTypes.Select(t => (IProcessor)Activator.CreateInstance(t)).OrderBy(p => p.GetType().Name));
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
