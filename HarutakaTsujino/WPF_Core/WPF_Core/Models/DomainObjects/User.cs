namespace WPF_Core.Models.DomainObjects
{
    class User
    {
        public User(int id, string mailAddress, string password, string name)
        {
            Id = id;

            Mail_address = mailAddress;

            Password = password;
            
            Name = name;
        }

        public int Id { get; }

        public string Mail_address { get; }

        public string Password { get; }
        
        public string Name { get; }
    }
}
