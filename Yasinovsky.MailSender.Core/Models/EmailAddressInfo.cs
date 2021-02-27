using System;
using System.ComponentModel.DataAnnotations;
using Yasinovsky.MailSender.Core.Attributes;
using Yasinovsky.MailSender.Core.Models.Base;

namespace Yasinovsky.MailSender.Core.Models
{
    public abstract class EmailAddressInfo : VerifiableViewModel, IHasId, ICloneable
    {
        private int _id;

        [Key]
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _name;

        [Required, StringLength(500), RegularExpression(@"(\w+)\s(\w+)(\s\w+)?", ErrorMessage = "Формат имени: Фамилия Имя[ Отчество]")]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _address;

        [Required, StringLength(500), MailAddress]
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        private string _description;

        [StringLength(1000)]
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public override string ToString() => $"{Name} {Address}";
        public virtual object Clone()
        {
            return MemberwiseClone();
        }
    }
}