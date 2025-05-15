namespace Sardonyx.Framework.Core.Email
{
    public interface IEmailTemplatingService
    {
        Email GetEmailFromTemplate<TParams>(EmailTemplate<TParams> template, string toEmail, string? cc = null, string? fromEmail = null) where TParams : class;
    }
}