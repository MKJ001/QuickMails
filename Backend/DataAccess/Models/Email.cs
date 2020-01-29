namespace DataAccess.Models
{
    public class Email
    {
        public int Id { get; set; }

        public string Address { get; set; }

        public EmailGroup EmailGroup { get; set; }

        public int EmailGroupId { get; set; }
    }
}
