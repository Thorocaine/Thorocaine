#nullable enable

using System;

namespace Thorocaine
{
    public  interface IUpdateStatus
    {
        IUpdateStatus Progress(int key, int complete, int total);
        IUpdateStatus WriteStatus(int key, params Stat[] stats);
    }

    public abstract class Stat
    {

        public abstract class ValueStat<T> : Stat
        {
            public T Value { get; }

            protected ValueStat(T value) => Value = value;
        }


        public class StatText : ValueStat<string>
        {
            public StatText(string value) : base(value) { }
        }

        public class StatColor : ValueStat<ConsoleColor>
        {
            public StatColor(ConsoleColor value) : base(value) { }
        }

        public static implicit operator Stat(string value) => new StatText(value);
        public static implicit operator Stat(ConsoleColor value) => new StatColor(value);
    }
}
