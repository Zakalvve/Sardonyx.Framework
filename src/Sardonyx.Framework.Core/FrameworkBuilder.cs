using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sardonyx.Framework.Core.Caching;
using Sardonyx.Framework.Core.Data;
using Sardonyx.Framework.Core.Email;
using Sardonyx.Framework.Core.Exceptions;
using Sardonyx.Framework.Core.Exceptions.ProblemDetails;
using Serilog;
using System.Reflection;

namespace Sardonyx.Framework.Core
{
    public sealed class FrameworkBuilder
    {
        public WebApplicationBuilder AppBuilder { get; }
        public List<IEntityModelConfiguration> ContextConfigurations { get; set; } = new();
        public string? BuildEnvironment { get; set; }

        public bool ClientAdded = false;
        public bool ContextAdded = false;
        public bool CachingAdded = false;

        public const string BuildChainViolationMessage = "Add core services cannot occur after adding a db context in the build chain.";

        public Type? DbContextType
        {
            get => _dbContextType;
            set
            {
                if (_dbContextType != null && _dbContextType != value)
                    throw new InvalidOperationException($"DbContextType already set to {_dbContextType.Name}, cannot override with {value?.Name}.");
                _dbContextType = value;
            }
        }
        private Type? _dbContextType;

        public FrameworkBuilder(WebApplicationBuilder builder, Type? dbContextType = null)
        {
            AppBuilder = builder;
            DbContextType = dbContextType;

            BuildEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }

        internal FrameworkBuilder AddCore()
        {
            if (ContextAdded) throw new InvalidOperationException(BuildChainViolationMessage);

            AppBuilder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
                config.Enrich.FromLogContext();
            });

            AppBuilder.Services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (ctx, ex) =>
                {
                    return BuildEnvironment == Environments.Development;
                };

                options.Map<ApplicationException>((ctx, ex) => new StatusCodeProblemDetails(StatusCodes.Status500InternalServerError)
                {
                    Title = "Application Exception",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });

                options.Map<BadRequestException>(ex => new BadRequestExceptionProblemDetails(ex));
                options.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
                options.Map<ForbiddenException>(ex => new ForbiddenExceptionProblemDetails(ex));
                options.Map<NotFoundException>(ex => new NotFoundExceptionProblemDetails(ex));
                options.Map<UnauthorizedException>(ex => new UnauthorizedExceptionProblemDetails(ex));
                options.Map<ValidationException>(ex => new ValidationExceptionProblemDetails(ex));
            });

            return this;
        }

        public FrameworkBuilder AddHttpClient()
        {
            if (!ClientAdded)
            {
                AppBuilder.Services.AddHttpClient();
                ClientAdded = true;
            }

            return this;
        }

        public FrameworkBuilder AddCaching()
        {
            AppBuilder.Services.AddMemoryCache();
            AppBuilder.Services.AddSingleton<ICachingService, CachingService>();

            CachingAdded = true;

            return this;
        }

        public FrameworkBuilder AddEmail<TAbstraction, TImplementation>()
            where TAbstraction : class, IEmailService
            where TImplementation : class, TAbstraction
        {
            AppBuilder.Services.AddScoped<IEmailTemplatingService, EmailTemplatingService>();
            AppBuilder.Services.AddScoped<TAbstraction, TImplementation>();

            return this;
        }

        public FrameworkBuilder AddDbContext(Action<DbContextOptionsBuilder>? optionsAction = null)
        {
            var contextType = DbContextType
                ?? throw new InvalidOperationException($"{nameof(DbContextType)} not set. Use {nameof(FrameworkExtensions.AddSardonyx)}<TContext>() or {nameof(AddDbContext)}<TContext>() instead.");

            if (!IsDerivedFromBaseDbContext(contextType))
                throw new ArgumentException($"The type {nameof(DbContextType)} must extend from {nameof(BaseDbContext)} in order to be used within this framework.");

            var method = typeof(FrameworkBuilder)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .First(m => m.Name == nameof(AddDbContext)
                    && m.IsGenericMethod
                && m.GetParameters().Length == 1);

            var genericMethod = method.MakeGenericMethod(contextType);

            return (FrameworkBuilder)genericMethod.Invoke(this, new object[] { optionsAction! })!;
        }

        public FrameworkBuilder AddDbContext<TContext>(Action<DbContextOptionsBuilder>? optionsAction = null)
            where TContext : BaseDbContext
        {
            AppBuilder.Services.AddSingleton(ContextConfigurations);

            AppBuilder.Services.AddDbContextFactory<TContext>((sp, optionsBuilder) =>
            {
                optionsAction?.Invoke(optionsBuilder);
            });

            AppBuilder.Services.AddScoped(sp =>
            {
                var configs = sp.GetRequiredService<IEnumerable<IEntityModelConfiguration>>();
                var factory = sp.GetRequiredService<IDbContextFactory<TContext>>();

                var options = sp.GetRequiredService<DbContextOptions<TContext>>();
                return (TContext)Activator.CreateInstance(typeof(TContext), options, configs)!;
            });

            ContextAdded = true;

            return this;
        }

        public WebApplicationBuilder Build() => AppBuilder;

        private bool IsDerivedFromBaseDbContext(Type contextType)
        {
            var baseDbContextType = typeof(BaseDbContext);

            while (contextType != null && contextType != typeof(object))
            {
                if (contextType == baseDbContextType)
                    return true;

                contextType = contextType.BaseType;
            }

            return false;
        }
    }
}
