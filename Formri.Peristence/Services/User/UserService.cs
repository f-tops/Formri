using Formri.Domain.Clients.JsonPlaceholderApi;
using Formri.Domain.Models.ContactForm;
using Formri.Domain.Models.User;
using Formri.Domain.Repositories.User;
using Formri.Domain.Services.Email;
using Formri.Domain.Services.User;
using Formri.Domain.Services.UserCache;
using Entities = Formri.Domain.Entities;

namespace Formri.Persistence.Services.User
{
    public class UserService(
        IUserCache userCache,
        IUserRepository userRepository,
        IJsonPlaceholderApiClient jsonPlaceholderApiClient,
        IEmailService emailService
        ) : IUserService
    {
        public async Task<bool> AddContactFormUser(ContactFormModel contactFormModel, CancellationToken cancellationToken)
        {
            var newUser = new Entities.User()
            {
                Email = contactFormModel.Email,
                Name = $"{contactFormModel.FirstName} {contactFormModel.LastName}"
            };

            if (userCache.HasUserBeenCreatedRecently(newUser))
            {
                return false;
            }

            var userData = await jsonPlaceholderApiClient.GetUserByEmail(newUser.Email);
            bool existsInJsonPlaceholder = userData != null && userData.Count > 0;

            if (existsInJsonPlaceholder)
            {
                var existingUser = userData[0];

                newUser.Username = existingUser.Username;
                newUser.Phone = existingUser.Phone;
                newUser.Website = existingUser.Website;
                newUser.Address = new Entities.Address()
                {
                    City = existingUser.Address.City,
                    Street = existingUser.Address.Street,
                    Suite = existingUser.Address.Suite,
                    Zipcode = existingUser.Address.Zipcode,
                    Lat = existingUser.Address.Geo.Lat,
                    Lng = existingUser.Address.Geo.Lng
                };
                newUser.Company = new Entities.Company()
                {
                    Bs = existingUser.Company.Bs,
                    CatchPhrase = existingUser.Company.CatchPhrase,
                    Name = existingUser.Company.Name
                };
            }

            await userRepository.AddUser(newUser, cancellationToken, existsInJsonPlaceholder);
            userCache.AddUserToCache(newUser);

            var emailUser = new EmailUser()
            {
                Name = newUser.Name,
                Email = newUser.Email,
                Phone = newUser.Phone ?? "Phone not set",
                Website = newUser.Website ?? "Website not set",
                Address = $"{newUser.Address?.Street ?? "Street not set"} {newUser.Address?.City ?? "City not set"}"
            };

            await emailService.SendEmail(emailUser);

            return true;
        }

        public async Task<Entities.User> GetUserByEmail(string email, CancellationToken cancellationToken)
        {
            return await userRepository.GetUserByEmail(email, cancellationToken);
        }

        public async Task<IEnumerable<Entities.User>> GetUsersWithBizEmail(CancellationToken cancellationToken)
        {
            return await userRepository.GetUsersWithBizEmail(cancellationToken);
        }

        public async Task<IEnumerable<Entities.User>> GetAllUsers(CancellationToken cancellationToken)
        {
            return await userRepository.GetAllUsers(cancellationToken);
        }
    }
}
