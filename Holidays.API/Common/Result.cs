using System;
using System.Collections.Generic;
using System.Linq;

namespace Holidays.API.Common
{
    public class Result<T>
    {
        public Result(T data)
        {
            Data = data;
        }

        public Result(IEnumerable<string> errors)
        {
            Data = default;
            Errors = new List<string>(errors);
        }

        public T Data { get; }
        public bool HasErrors { get { return Errors.Any(); } }
        public List<string> Errors { get; } = new List<string>();
    }

    public static class ResultOps
    {
        public static Result<TR> Select<T, TR>(
            this Result<T> result,
            Func<T, Result<TR>> transform)
        {
            if (result.HasErrors)
                return new Result<TR>(result.Errors);

            return transform(result.Data);
        }

        public static Result<TR> Select<T, TR>(
            this Result<T> result,
            Func<T, TR> transform)
        {
            if (result.HasErrors)
                return new Result<TR>(result.Errors);

            return new Result<TR>(transform(result.Data));
        }
    }
}
