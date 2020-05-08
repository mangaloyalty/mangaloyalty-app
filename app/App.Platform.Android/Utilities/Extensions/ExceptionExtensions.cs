using System;

namespace App.Platform.Android.Utilities.Extensions
{
    public static class ExceptionExtensions
    {
        public static string FullStack(this Exception exception)
        {
            if (exception is AggregateException aggregate) exception = aggregate.Flatten();
            return string.Join(Environment.NewLine, exception.Message, exception.StackTrace);
        }
    }
}