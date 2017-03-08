using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FakeTrello.Models
{
    public class AddedUser
    {
        [Key]
        public int AddedUserId { get; set; }

        public string AddedUserName { get; set; }

        public int UserId { get; set; }
    }
}