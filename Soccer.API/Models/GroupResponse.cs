﻿namespace Soccer.API.Models
{
    using Domain;
    using System.Collections.Generic;

    public class GroupResponse
    {
        public int GroupId { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public int OwnerId { get; set; }

        public User Owner { get; set; }

        public List<GroupUserResponse> GroupUsers { get; set; }
    }
}