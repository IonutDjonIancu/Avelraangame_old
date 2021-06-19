using Avelraangame.Services.ServiceUtils;
using System;
using System.ComponentModel.DataAnnotations;

namespace Avelraangame.Models
{
    public partial class TemporaryData
    {
        public Guid Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
