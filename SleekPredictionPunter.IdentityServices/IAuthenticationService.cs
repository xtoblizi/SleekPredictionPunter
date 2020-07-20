using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.IdentityServices
{
	public interface IAuthenticationService
	{
		Task<dynamic> Login(dynamic loginModel);

		Task<dynamic> ChangePassword(dynamic changePasswordModel);

		Task<dynamic> LogOut();

		Task<dynamic> ResetPassword(dynamic resetPasswordModel);


	}
}
