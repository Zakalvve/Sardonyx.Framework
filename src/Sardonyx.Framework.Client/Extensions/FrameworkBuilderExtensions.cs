using Microsoft.Extensions.DependencyInjection;
using Sardonyx.Framework.Core;

namespace Sardonyx.Framework.Client.Extensions
{
    public static class FrameworkBuilderExtensions
    {
        public static FrameworkBuilder AddTypedHttpClient<TClient, TImplementation>(this FrameworkBuilder builder)
            where TClient : class, IBaseClient
            where TImplementation : BaseClient, TClient
        {
            builder.AppBuilder.Services.AddHttpClient<TClient, TImplementation>();

            return builder;
        }

        public static FrameworkBuilder AddTypedHttpClient<TClient, TImplementation>(this FrameworkBuilder builder, string name)
            where TClient : class, IBaseClient
            where TImplementation : BaseClient, TClient
        {
            builder.AppBuilder.Services.AddHttpClient<TClient, TImplementation>(name);

            return builder;
        }

        public static FrameworkBuilder AddTypedHttpClient<TClient, TImplementation>(this FrameworkBuilder builder, Action<HttpClient> configureClient)
            where TClient : class, IBaseClient
            where TImplementation : BaseClient, TClient
        {
            builder.AppBuilder.Services.AddHttpClient<TClient, TImplementation>(configureClient);

            return builder;
        }

        public static FrameworkBuilder AddTypedHttpClient<TClient, TImplementation>(this FrameworkBuilder builder, Action<IServiceProvider, HttpClient> configureClient)
            where TClient : class, IBaseClient
            where TImplementation : BaseClient, TClient
        {
            builder.AppBuilder.Services.AddHttpClient<TClient, TImplementation>(configureClient);

            return builder;
        }

        public static FrameworkBuilder AddTypedHttpClient<TClient, TImplementation>(this FrameworkBuilder builder, Func<HttpClient, TImplementation> factory)
            where TClient : class, IBaseClient
            where TImplementation : BaseClient, TClient
        {
            builder.AppBuilder.Services.AddHttpClient<TClient, TImplementation>(factory);

            return builder;
        }

        public static FrameworkBuilder AddTypedHttpClient<TClient, TImplementation>(this FrameworkBuilder builder, Func<HttpClient, IServiceProvider, TImplementation> factory)
            where TClient : class, IBaseClient
            where TImplementation : BaseClient, TClient
        {
            builder.AppBuilder.Services.AddHttpClient<TClient, TImplementation>(factory);

            return builder;
        }

        public static FrameworkBuilder AddTypedHttpClient<TClient, TImplementation>(this FrameworkBuilder builder, string name, Action<HttpClient> configureClient)
            where TClient : class, IBaseClient
            where TImplementation : BaseClient, TClient
        {
            builder.AppBuilder.Services.AddHttpClient<TClient, TImplementation>(name, configureClient);

            return builder;
        }

        public static FrameworkBuilder AddTypedHttpClient<TClient, TImplementation>(this FrameworkBuilder builder, string name, Action<IServiceProvider, HttpClient> configureClient)
            where TClient : class, IBaseClient
            where TImplementation : BaseClient, TClient
        {
            builder.AppBuilder.Services.AddHttpClient<TClient, TImplementation>(name, configureClient);

            return builder;
        }

        public static FrameworkBuilder AddTypedHttpClient<TClient, TImplementation>(this FrameworkBuilder builder, string name, Func<HttpClient, TImplementation> factory)
            where TClient : class, IBaseClient
            where TImplementation : BaseClient, TClient
        {
            builder.AppBuilder.Services.AddHttpClient<TClient, TImplementation>(name, factory);

            return builder;
        }

        public static FrameworkBuilder AddTypedHttpClient<TClient, TImplementation>(this FrameworkBuilder builder, string name, Func<HttpClient, IServiceProvider, TImplementation> factory)
            where TClient : class, IBaseClient
            where TImplementation : BaseClient, TClient
        {
            builder.AppBuilder.Services.AddHttpClient<TClient, TImplementation>(name, factory);

            return builder;
        }
    }
}
