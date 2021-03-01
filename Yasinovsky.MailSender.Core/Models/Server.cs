using System;
using System.ComponentModel.DataAnnotations;
using Yasinovsky.MailSender.Core.Attributes;
using Yasinovsky.MailSender.Core.Models.Base;

namespace Yasinovsky.MailSender.Core.Models
{
    public class Server : VerifiableViewModel, ICloneable, IHasId
    {
        private int _id;

        [Key]
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _name;

        [Required, StringLength(500)]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _address;

        [Required, StringLength(500), SmtpAddress]
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        private int _port = 25;

        [Required]
        public int Port
        {
            get => _port;
            set => SetProperty(ref _port, value);
        }

        private bool _enableSsl = true;

        [Required]
        public bool EnableSsl
        {
            get => _enableSsl;
            set => SetProperty(ref _enableSsl, value);
        }

        private string _login;

        [Required, StringLength(500)]
        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        private string _password;

        [Required, StringLength(500), DataType(DataType.Password)]
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _description;

        [StringLength(1000)]
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public override string ToString() => $"{Name} {Address}:{Port}, {(EnableSsl? "ssl" : "no ssl")}";
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}