namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Deconstructs the given result into success and failure out parameters
        /// </summary>
        public void Deconstruct(out bool isSuccess, out bool isFailure)
        {
            isSuccess = result.IsSuccess;
            isFailure = result.IsFailure;
        }

        /// <summary>
        ///     Deconstructs the given result into success, failure and value out parameters
        /// </summary>
        public void Deconstruct(out bool isSuccess, out bool isFailure, out T? value)
        {
            isSuccess = result.IsSuccess;
            isFailure = result.IsFailure;
            value = result.IsSuccess ? result.Value : default;
        }

        /// <summary>
        ///     Deconstructs the given result into success, failure, value and error out parameters
        /// </summary>
        public void Deconstruct(out bool isSuccess, out bool isFailure, out T? value, out TE? error)
        {
            isSuccess = result.IsSuccess;
            isFailure = result.IsFailure;
            value = result.IsSuccess ? result.Value : default;
            error = result.IsFailure ? result.Error : default;
        }
    }
}


