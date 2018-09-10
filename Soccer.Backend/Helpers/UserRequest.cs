namespace Soccer.Backend.Helpers
{
    using Domain;
    using System.ComponentModel.DataAnnotations.Schema;

    [NotMapped]
    public class UserRequest : User
    {
        public string Password { get; set; }

        public byte[] ImageArray { get; set; }
    }
}