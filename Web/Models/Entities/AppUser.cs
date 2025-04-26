using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Web.Models.Entities
{
    public enum Status
    {
        Active = 1, Modified, Passive
    }

    public class AppUser : IdentityUser
    {
        private DateTime _createdDate = DateTime.Now;
        private Status _status = Status.Active;

        [Required]
        [MaxLength(120)]
        [MinLength(2)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(120)]
        [MinLength(2)]
        public string LastName { get; set; }

        public DateOnly Birthdate { get; set; }

        public DateTime CreatedDate { get => _createdDate; set => _createdDate = value; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public Status Status { get => _status; set => _status = value; }
    }
}
