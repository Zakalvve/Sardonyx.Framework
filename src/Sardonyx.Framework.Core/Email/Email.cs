namespace Sardonyx.Framework.Core.Email
{
    public struct Email
    {
        public Email(string toEmail, string fromEmail, string ccs, string subject, string message)
        {
            ToEmail = toEmail;
            FromEmail = fromEmail;
            Subject = subject;
            Message = message;
            CCs = ccs;
        }
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string CCs { get; set; }
    }
}
