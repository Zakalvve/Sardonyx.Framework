namespace Sardonyx.Framework.Core.Email
{
    public class EmailTemplate<TParams> where TParams : class
    {
        public EmailTemplate(TParams parameters, string htmlTemplateFileName, string subject)
        {
            Params = parameters;
            HtmlTemplateFileName = htmlTemplateFileName;
            Subject = subject;
        }
        public TParams Params { get; set; }
        public string HtmlTemplateFileName { get; set; }
        public string Subject { get; set; }
    }
}
