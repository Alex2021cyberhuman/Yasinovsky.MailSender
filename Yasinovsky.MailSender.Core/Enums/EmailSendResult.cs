namespace Yasinovsky.MailSender.Core.Enums
{
    public enum EmailSendResult
    {
        Success,
        Unauthorized,
        Timeout,
        ProtocolError,
        CommandError
    }
}
