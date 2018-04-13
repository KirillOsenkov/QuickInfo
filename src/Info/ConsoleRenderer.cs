using System;
using System.Collections.Generic;
using QuickInfo;

namespace Info
{
    public class ConsoleRenderer
    {
        public static ConsoleRenderer Instance = new ConsoleRenderer();

        public static void RenderObject(string processorName, object resultNode)
        {
            Instance.Render(resultNode);
        }

        private void Render(object result)
        {
            if (result is Node node)
            {
                RenderNode(node);
            }
            else if (result is string text)
            {
                Write(text);
            }
            else if (result is IEnumerable<object> list)
            {
                RenderList(list);
            }
        }

        private void RenderNode(Node node)
        {
            var list = node.List;

            if (list != null)
            {
                RenderList(list);
            }
            else
            {
                var text = GetText(node);
                WriteLine(text);
            }
        }

        private string GetText(Node node)
        {
            return node.Text;
        }

        private void RenderList(IEnumerable<object> list)
        {
            foreach (var item in list)
            {
                Render(item);
            }
        }

        private void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        private void Write(string text)
        {
            Console.Write(text);
        }
    }
}
