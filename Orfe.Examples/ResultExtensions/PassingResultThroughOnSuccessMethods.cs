using System;


namespace Orfe.Examples.ResultExtensions
{
    public class PassingResultThroughOnSuccessMethods
    {
        public void Example1()
        {
            var result = FunctionInt()
                .Bind(x => FunctionString(x))
                .Bind(FunctionDateTime);
        }

        public void Example2()
        {
            var result = FunctionInt()
                .Bind(_ => FunctionString())
                .Bind(FunctionDateTime);
        }

        private Result<int,Unit> FunctionInt()
        {
            return Result.Success(1);
        }

        private Result<string,Unit> FunctionString(int intValue)
        {
            return Result.Success("Ok");
        }

        private Result<string,Unit> FunctionString()
        {
            return Result.Success("Ok");
        }

        private Result<DateTime,Unit> FunctionDateTime(string stringValue)
        {
            return Result.Success(DateTime.Now);
        }
    }
}
