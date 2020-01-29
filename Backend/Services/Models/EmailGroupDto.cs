namespace Services.Models
{
    using System.Collections.Generic;

    public class EmailGroupDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> Emails { get; set; }
    }
}
