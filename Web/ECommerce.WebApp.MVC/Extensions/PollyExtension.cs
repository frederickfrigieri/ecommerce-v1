using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System;
using System.Net.Http;

namespace ECommerce.WebApp.MVC.Extensions
{
    public class PollyExtension
    {
        public static AsyncRetryPolicy<HttpResponseMessage> EspereETente()
        {
            //Global - Tipos de Erro 5XX, Network e 408
            var retryWWaitPolicy = HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(15),

            }, (outcome, timespan, retryCount, context) =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Tentando pela {retryCount} vez");
                Console.ForegroundColor = ConsoleColor.White;
            });

            return retryWWaitPolicy;
        }
    }
}
