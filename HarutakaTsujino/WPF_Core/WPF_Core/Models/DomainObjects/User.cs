namespace WPF_Core.Models.DomainObjects
{
    public class User
    {
        public User(int id, int iconId, string iconPath, string mailAddress, string password, string name)
        {
            Id = id;

            IconId = iconId;

            IconPath = iconPath;

            MailAddress = mailAddress;

            Password = password;
            
            Name = name;
        }

        public int Id { get; }

        public int IconId { get; }

        public string IconPath { get; }

        public string MailAddress { get; }

        public string Password { get; }
        
        public string Name { get; }
    }
}
