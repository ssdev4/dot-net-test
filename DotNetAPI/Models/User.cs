using System;
using System.ComponentModel.DataAnnotations;

namespace DotNetAPI.Models
{
    public class User
    {
        public User()
        {
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
    }
}