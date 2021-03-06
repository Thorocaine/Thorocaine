﻿#nullable enable

using System;
using System.Threading.Tasks;

namespace Thorocaine
{
    public static class Extentions
    {
        public static TOut Pipe<TIn, TOut>(this TIn @this, Func<TIn, TOut> func) => func(@this);
        
        public static T Pipe<T>(this T @this, Action<T> action)
        {
            action(@this);
            return @this;
        }

        public static Task<T> AsTask<T>(this T @this) => Task.FromResult(@this);
    }
}
