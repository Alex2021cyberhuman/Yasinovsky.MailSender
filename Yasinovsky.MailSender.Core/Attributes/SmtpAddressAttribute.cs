using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Yasinovsky.MailSender.Core.Attributes
{
    public class MailAddressAttribute : ValidationAttribute
    {
        private static Regex _mailRegex =
            new(
                @"^[-a-z0-9!#$%&'*+/=?^_`{|}~]+(\.[-a-z0-9!#$%&'*+/=?^_`{|}~]+)*@([a-z0-9]([-a-z0-9]{0,61}[a-z0-9])?\.)*(aero|arpa|asia|biz|cat|com|coop|edu|gov|info|int|jobs|mil|mobi|museum|name|net|org|pro|tel|travel|[a-z][a-z])$");
        public MailAddressAttribute() : this("Invalid mail address")
        {
            
        }

        public MailAddressAttribute(string errorMessage) : base(errorMessage)
        {
        }

        public override bool IsValid(object value)
        {
            if (value is not string addressString)
                return false;
            try
            {
                var match = _mailRegex.Match(addressString);
                var isMatch = match.Success;
                return isMatch;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }

    public class SmtpAddressAttribute : ValidationAttribute
    {
        private static Regex _smtpRegex =
            new(
                @"^(smtp)\.([\w\-]+\w)\.(aero|arpa|asia|biz|cat|com|coop|edu|gov|info|int|jobs|mil|mobi|museum|name|net|org|pro|tel|travel|[a-z][a-z])$");
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
