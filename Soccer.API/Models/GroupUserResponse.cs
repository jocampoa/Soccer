namespace Soccer.API.Models
{
    using Domain;

    public class GroupUserResponse
    {
        public int GroupUserId { get; set; }

        public int GroupId { get; set; }

        public int UserId { get; set; }

        public bool IsAccepted { get; set; }

        public bool IsBlocked { get; set; }

        public int Points { get; set; }

        public UserResponse User
        {
            get; set;
        }
    }
}