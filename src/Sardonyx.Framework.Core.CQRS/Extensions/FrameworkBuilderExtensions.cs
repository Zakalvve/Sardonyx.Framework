using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sardonyx.Framework.Core.CQRS.Application;
using Sardonyx.Framework.Core.CQRS.Data;
using Sardonyx.Framework.Core.CQRS.Infrastructure;
using System.Reflection;

namespace Sardonyx.Framework.Core.CQRS.Extensions
{
    public static class FrameworkBuilderExtensions
    {
        public static FrameworkBuilder AddCQRS(this FrameworkBuilder builder, params Assembly?[] assemblies)
        {
            var contextType = builder.DbContextType
                ?? throw new InvalidOperationException($"{nameof(builder.DbContextType)} not set. Use {nameof(Sardonyx.Framework.Core.FrameworkExtensions.AddSardonyx)}<T>() or {nameof(AddCQRS)}<TContext>() instead.");

            if (!typeof(ICQRSContext).IsAssignableFrom(contextType))
                throw new ArgumentException($"The type {nameof(builder.DbContextType)} must implement {nameof(ICQRSContext)} in order to be used by the CQRS system.");

            var method = typeof(FrameworkBuilderExtensions)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(m => m.Name == nameof(AddCQRS)
                    && m.IsGenericMethod
                    && m.GetParameters().Length == 2);

            var genericMethod = method.MakeGenericMethod(contextType);

            return (FrameworkBuilder)genericMethod.Invoke(null, new object[] { builder, assemblies! })!;
        }

        public static FrameworkBuilder AddCQRS<TContext>(this FrameworkBuilder builder, params Assembly?[] assemblies)
            where TContext : class, ICQRSContext
        {
            if (builder.ContextAdded) throw new InvalidOperationException(FrameworkBuilder.BuildChainViolationMessage);

            builder.ContextConfigurations.Add(new CQRSModelConfiguration());

            var appBuilder = builder.AppBuilder;

            assemblies = assemblies.Where(a => a != null).ToArray();

            if (assemblies.Length > 0)
            {
                appBuilder.Services.AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssemblies(assemblies);
                });

                appBuilder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingDecorator<,>));

                if (builder.CachingAdded)
                {
                    appBuilder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingDecorator<,>));
                }

                appBuilder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkDecorator<,>));
            }

            appBuilder.Services.AddScoped<IExecutor, Executor>();
            appBuilder.Services.AddScoped<ICQRSContext, TContext>();
            appBuilder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            appBuilder.Services.AddScoped<ICommandAccessor, CommandAccessor>();

            return builder;
        }
    }
}
