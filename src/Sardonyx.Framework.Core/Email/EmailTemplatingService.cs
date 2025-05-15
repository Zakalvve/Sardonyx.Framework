using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace Sardonyx.Framework.Core.Email
{
    public class EmailTemplatingService : IEmailTemplatingService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly IMemoryCache? _cache;

        public EmailTemplatingService(IWebHostEnvironment env, IConfiguration config, IMemoryCache? cache)
        {
            _env = env;
            _config = config;
            _cache = cache;
        }

        public Email GetEmailFromTemplate<TParams>(EmailTemplate<TParams> template, string toEmail, string? cc = null, string? fromEmail = null) where TParams : class
        {
            var templateParameters = new Dictionary<string, string>();

            var properties = typeof(TParams).GetProperties();

            foreach (var property in properties)
            {
                var key = property.Name;

                var value = property.GetValue(template.Params)?.ToString() ?? string.Empty;

                templateParameters[key] = value;
            }

            if (!IsParamsValid(templateParameters)) {
                throw new InvalidOperationException("Some template parameters are missing, cannot send email without all template parameters being filled.");
            }

            string htmlTemplateFullPath = Path.Combine(_env.ContentRootPath, "EmailTemplates", template.HtmlTemplateFileName);
            string finalFromEmail = fromEmail ?? _config.GetValue<string>("Sardonyx:Emails:FromEmail") ?? Constants.EmailTemplates.FromEmail;
            string ccs = !String.IsNullOrWhiteSpace(cc) ? cc : string.Empty;

            return new Email(toEmail, finalFromEmail, ccs, template.Subject, HydrateTemplate(htmlTemplateFullPath, templateParameters));
        }

        private bool IsParamsValid(Dictionary<string, string> parameters)
        {
            var missingParameters = parameters.Where(kvp => string.IsNullOrEmpty(kvp.Value)).Select(kvp => kvp.Key).ToList();

            if (missingParameters.Any())
            {
                throw new InvalidOperationException($"Missing or empty parameters: {string.Join(", ", missingParameters)}");
            }

            return true;
        }

        private string HydrateTemplate(string filePath, Dictionary<string, string> templateParameters, string identifier = "--")
        {
            string? template = GetOrLoad(filePath);

            if (string.IsNullOrEmpty(template))
            {
                throw new InvalidOperationException("Template file is empty or could not be loaded.");
            }

            template = ReplacePlaceholders(template, templateParameters, identifier);

            return template;
        }

        private string? GetOrLoad(string filePath)
        {
            // Cache HTML if cache is available
            if (_cache != null)
            {
                return _cache.GetOrCreate(filePath, entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);

                    if (!File.Exists(filePath))
                        throw new FileNotFoundException("Template file not found.", filePath);

                    return File.ReadAllText(filePath);
                });
            }

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Template file not found.", filePath);

            return File.ReadAllText(filePath);
        }

        private string ReplacePlaceholders(string template, Dictionary<string, string> fieldParameters, string identifier)
        {
            string pattern = identifier + @"(\w+)" + identifier;

            MatchEvaluator evaluator = match =>
            {
                string fieldKey = match.Groups[1].Value.Trim();
                if (fieldParameters.ContainsKey(fieldKey))
                {
                    return fieldParameters[fieldKey];
                }
                return match.Value;
            };

            return Regex.Replace(template, pattern, evaluator);
        }
    }
}
