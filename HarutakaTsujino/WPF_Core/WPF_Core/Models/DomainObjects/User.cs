namespace WPF_Core.Models.DomainObjects
{
    public class User
    {
        public User(int id, string mailAddress, string password, string name)
        {
            Id = id;

            MailAddress = mailAddress;

            Password = password;
            
            Name = name;
        }

        public int Id { get; }

        public string MailAddress { get; }

        public string Password { get; }
        
        public string Name { get; }
    }
}
