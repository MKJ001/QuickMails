namespace DataAccess.Models
{
    using System.Collections.Generic;

    public class EmailGroup 
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Email> Emails { get; set; }
    }
}
