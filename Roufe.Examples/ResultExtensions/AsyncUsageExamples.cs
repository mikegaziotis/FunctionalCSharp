using System;
using System.Threading.Tasks;

namespace Roufe.Examples.ResultExtensions
{
    public class AsyncUsageExamples
    {
        public async Task<string> Promote_with_async_methods_in_the_beginning_of_the_chain(long id)
        {
            var gateway = new EmailGateway();

            return (await GetByIdAsync(id).ConfigureAwait(DefaultConfigureAwait))
                .ToResult(new Error("Customer with such Id is not found: " + id))
                .Ensure(customer => customer.CanBePromoted(), new Error("The customer has the highest status possible"))
                .Tap(customer => customer.Promote())
                .Bind(customer => gateway.SendPromotionNotification(customer.Email))
                .Finally(result => result.IsSuccess ? "Ok" : result.Error.Message);
        }

        public async Task<string> Promote_with_async_methods_in_the_beginning_and_in_the_middle_of_the_chain(long id)
        {
            var gateway = new EmailGateway();

            return await GetById(id)
                .ToResult(new Error("Customer with such Id is not found: " + id))
                .Ensure(customer => customer.CanBePromoted(), new Error("The customer has the highest status possible"))
                .Tap(customer => customer.Promote())
                .Tap(customer => Log($"Customer {customer.Email} promoted"))
                .Bind(async customer => await gateway.SendPromotionNotificationAsync(customer.Email).ConfigureAwait(DefaultConfigureAwait))
                .Finally(result => result.IsSuccess ? "Ok" : result.Error.Message);
        }


        private static void Log(string message)
        {
            Console.Write(message);
        }

        private static Task<Result<Customer,Unit>> AskManagerAsync(long id)
        {
            return Task.FromResult(Result.Success(new Customer()));
        }

        private static Task<Customer?> GetByIdAsync(long id)
        {
            return Task.FromResult(GetById(id));
        }

        private static Customer? GetById(long id)
            => new Random().Next(0, 1) == 0
                ? null
                : new Customer();

        private class Customer(string? email)
        {
            public Customer(): this(null) { }
            public string? Email { get; } = email;

            public bool CanBePromoted()
            {
                return true;
            }

            public void Promote()
            {
            }

            public Task PromoteAsync()
            {
                return Task.CompletedTask;
            }
        }

        private class EmailGateway
        {
            private readonly Random _random = new();
            public Result<Unit,Error> SendPromotionNotification(string? email)
                =>  _random.Next(0,1) == 0
                    ? Result.Failure(new Error($"Failed to send email to {email??"unknown"}"))
                    : Result.Success<Error>();


            public Task<Result<Unit,Error>> SendPromotionNotificationAsync(string? email)
            {
                return Task.FromResult(Result.Success<Error>());
            }
        }

        private record Error(string Message);
    }
}
