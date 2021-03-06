using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Yasinovsky.MailSender.Core.Attributes
{
    public class SmtpAddressAttribute : ValidationAttribute
    {
        private static readonly Regex _smtpRegex =
            new(
                @"^(smtp)\.([\w\-]+\w)\.(aero|arpa|asia|biz|cat|com|coop|edu|gov|info|int|jobs|mil|mobi|museum|name|net|org|pro|tel|travel|[a-z][a-z])$", RegexOptions.Compiled);
        public SmtpAddressAttribute() : this("Invalid mail address")
        {

        }

        public SmtpAddressAttribute(string errorMessage) : base(errorMessage)
        {
        }

        public override bool IsValid(object value)
        {
            if (value is not string addressString)
                return false;
            try
            {
                var match = _smtpRegex.Match(addressString);
                var isMatch = match.Success;
                return isMatch;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
