
using Bankify.Domain.Common;
using Bankify.Domain.Common.Shared;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Validators;
using System.ComponentModel.DataAnnotations;
namespace Bankify.Domain.Models.Users
{
    public class BUser:BaseEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public IEnumerable<Account> Accounts { get; set; }
        public string? ProfilePicture { get; set; }
        
        public static BUser Create(string firstName, string lastName, 
            string email, string password, string phoneNumber, 
            string? address, string? profilePicture,int id=0)
        {
            var userDataValidator = new UserDataValidator();
            
            var newUser= new BUser
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                PhoneNumber = phoneNumber,
                Address = address,
                ProfilePicture = profilePicture
            };
            var validationResult = userDataValidator.Validate(newUser);
            if (validationResult.IsValid) return newUser;

            var exception = new NotValidException("User has no valid information");
            validationResult.Errors.ForEach(vr => exception.ValidationErrors.Add(vr.ErrorMessage));
            throw exception;

        }
    }
}
