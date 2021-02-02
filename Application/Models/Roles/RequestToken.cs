using System;
using System.Linq;

namespace Application.Models.Roles
{
	public class RequestToken
	{
		public RequestTokenUser LoggedInUser { get; set; }
		public RequestTokenUser SelectedIntermediary { get; set; }
		public bool hasSelectedIntermediary { get => SelectedIntermediary != null; }
		public DateTime RequestDate { get; set; }
		public DateTime ExpirationDate { get; set; }
		public bool? isValid { get => !hasSelectedIntermediary ? LoggedInUser?.Roles?.Any() : SelectedIntermediary?.Roles?.Any(); }

	}
}
