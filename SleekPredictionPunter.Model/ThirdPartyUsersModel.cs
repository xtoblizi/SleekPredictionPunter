using SleekPredictionPunter.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
    public class ThirdPartyUsersModel: BaseEntity
    {

        public long AuthId { get; set; }
        public string  EmailAddress { get; set; }
        public string PhoneNumbers { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public RoleEnum UserRole { get; set; }
        public string UserRoleName { get; set; }
        public string ProviderName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateTimeCreated { get; set; }
    }
}
