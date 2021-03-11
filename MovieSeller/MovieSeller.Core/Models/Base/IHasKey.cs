using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSeller.Core.Models.Base
{
    public interface IHasKey<TKey> where TKey : IEquatable<TKey>
    {
        [Key]
        TKey Id { get; set; }
    }
}
