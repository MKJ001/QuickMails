namespace Services.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common.Validators;

    public class NewEmailGroupDto
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [EmailAddressBulk]
        public IEnumerable<string> Emails { get; set; }
    }
}
