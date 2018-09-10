﻿namespace Soccer.Models
{
    using System.Collections.Generic;

    public class UserGroup
    {
        public int GroupId { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public int OwnerId { get; set; }

        public User Owner { get; set; }

        public List<GroupUser> GroupUsers { get; set; }

        public string FullLogo
        {
            get
            {
                if (string.IsNullOrEmpty(Logo))
                {
                    return "avatar_group.png";
                }

                return string.Format("http://soccerbackend.azurewebsites.net{0}", Logo.Substring(1));
            }
        }
    }
}
