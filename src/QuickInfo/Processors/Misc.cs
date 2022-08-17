using System;
using System.Collections.Generic;
using static QuickInfo.NodeFactory;

namespace QuickInfo
{
    public class Misc : IProcessor
    {
        private HashSet<string> triggerStrings = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "about",
        };

        public object GetResult(Query query)
        {
            if (query.IsHelp)
            {
                return HelpTable(("about", "Displays information about the app"));
            }

            if (triggerStrings.Contains(query.OriginalInput))
            {
                return new[]
                {
                    Paragraph($"About:"),
                    NameValueTable(
                        null,
                        right =>
                        {
                            if (right.Text.StartsWith("http"))
                            {
                                right.Link = right.Text;
                            }
                        },
                        ("Git commit:", $"{ThisAssembly.Git.RepositoryUrl}/commit/{ThisAssembly.Git.Commit}"),
                        ("Date:", $"{System.DateTime.Parse(ThisAssembly.Git.CommitDate):G}")
                    )
                };
            }

            return null;
        }
    }
}
