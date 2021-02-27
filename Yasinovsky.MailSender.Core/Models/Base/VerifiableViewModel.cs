using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Yasinovsky.MailSender.Core.Models.Base
{
    public class VerifiableViewModel : BaseViewModel, IDataErrorInfo
    {
        [IgnoreDataMember, JsonIgnore, SoapIgnore, XmlIgnore]
        public string Error => throw new InvalidOperationException("Unsupported. Use this[string]");

        [IgnoreDataMember, JsonIgnore, SoapIgnore, XmlIgnore]
        public string this[string propertyName] => GetPropertyError(propertyName);

        private string GetPropertyError(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("Invalid property name", propertyName);

            PropertyInfo propInfo = GetType().GetProperty(propertyName);
            if (propInfo is null)
                throw new InvalidOperationException("Property not found");
            var value = propInfo.GetValue(this);
            var results = new List<ValidationResult>(1);
            var result = Validator.TryValidateProperty(
                value,
                new ValidationContext(this, null, null)
                {
                    MemberName = propertyName
                },
                results);

            return result ? string.Empty : results[0].ErrorMessage;
        }
        
    }
}