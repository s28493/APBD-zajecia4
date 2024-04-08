using System;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ICreditLimitService _creditService;
        private readonly IUserDataAccessAdapter _userDataAccessAdapter;

        //[Obsolete]
        public UserService()
        {
            _clientRepository = new ClientRepository();
            _creditService = new UserCreditService();
            _userDataAccessAdapter = new UserDataAccessAdapter();
        }
        
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            //WALIDACJA
            if (!User.ValidateUser(firstName,lastName,email,dateOfBirth))
                return false;
            //Infrastruktura
            var client = _clientRepository.GetById(clientId);
            var user = User.CreateUser(client, dateOfBirth, email, firstName, lastName);
            //Logika biznesowa + infrastruktura
            SetUserCreditLimit(user, client);
            //Logika biznesowa
            if (!User.IsBigEnoughUserCreditLimit(user))
                return false;
            //infrastruktura
            _userDataAccessAdapter.AddUser(user);
            return true;
        }
        
        private void SetUserCreditLimit(User user, Client client)
        {
            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                int creditLimit = _creditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                creditLimit = creditLimit * 2; 
                user.CreditLimit = creditLimit;
            }
            else
            {
                user.HasCreditLimit = true;
                int creditLimit = _creditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                user.CreditLimit = creditLimit;
  
            }
        }
    }
}
