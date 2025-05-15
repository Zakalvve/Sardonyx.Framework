using MediatR;
using Serilog;
using System.Diagnostics;

namespace Sardonyx.Framework.Core.CQRS.Infrastructure
{
    internal sealed class LoggingDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;

        public LoggingDecorator(ILogger logger)
        {
            _logger = logger.ForContext(typeof(LoggingDecorator<TRequest, TResponse>)).ForContext("Module", "CQRS");
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                _logger.Information("Executing {Request}", typeof(TRequest).Name);
                var result = await next();
                stopwatch.Stop();
                _logger.Information("{Request} processed successfully in {Elapsed}ms", typeof(TRequest).Name, stopwatch.ElapsedMilliseconds);
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.Error(ex, "{Request} processing failed after {Elapsed}ms", typeof(TRequest).Name, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
