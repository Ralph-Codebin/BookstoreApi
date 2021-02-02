using System.Collections.Generic;

namespace Application.Models.Roles
{
	public class RequestTokenUser
	{
		public string Username { get; set; }
		public string EmployeeNumber { get; set; }
		public string UserId { get; set; }
		public List<string> Roles { get; set; }
	}
}
