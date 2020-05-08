using System;

namespace App.Core.Shared.Extensions
{
    public static class ExceptionExtensions
    {
        public static string ToJson(this Exception exception)
        {
            return exception.ToString().ToJson();
        }
    }
}