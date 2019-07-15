#nullable enable

using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Thorocaine.Cli
{
    public class ConsoleWriter : IUpdateStatus
    {
        readonly ConcurrentDictionary<int, int> rowKeys = new ConcurrentDictionary<int, int>();

        public ConsoleWriter Clear()
        {
            Console.Clear();
            return this;
        }

        IUpdateStatus IUpdateStatus.Progress(int key, int complete, int total) =>
            Progress(key, complete, total);

        public ConsoleWriter Progress(int key, int complete, int total)
        {
            if (complete > total) complete = total;
            var text = $"[{complete}/{total}]";
            var num = Console.WindowWidth - text.Length - 2;
            var num2 = (int) Math.Round(complete / (double) total * num);
            var count = num - num2;
            var bar = "[" + new string('=', num2) + new string(' ', count) + "]";
            return WriteStatus(key, text, bar);
        }

        IUpdateStatus IUpdateStatus.WriteStatus(int key, params Stat[] stats) =>
            WriteStatus(key, stats);

        public ConsoleWriter WriteStatus(int key, params Stat[] stats)
        {
            var baseColor = Console.ForegroundColor;
            var rk = rowKeys[key] = rowKeys.ContainsKey(key) ? rowKeys[key] : GetFirstRowGap();
            Console.SetCursorPosition(0, rk);
            foreach (var stat in stats) Write(stat);
            ClearLine();
            Console.ForegroundColor = baseColor;
            return this;
        }

        static void ClearLine() =>
            Enumerable
               .Range(0, Console.WindowWidth - Console.CursorLeft)
               .Skip(1)
               .Select(_ => ' ')
               .ToArray()
               .Pipe(Console.Write);

        static void Write(Stat stat)
        {
            switch (stat)
            {
                case Stat.StatText t:
                    Console.Write(t.Value);
                    return;
                case Stat.StatColor c:
                    Console.ForegroundColor = c.Value;
                    return;
            }
        }

        int GetFirstRowGap()
        {
            if (!rowKeys.Any()) return 0;
            var num = rowKeys.Values.Max();
            for (var i = 0; i <= num; i++)
                if (!rowKeys.Values.Contains(i))
                    return i;
            return num + 1;
        }
    }
}