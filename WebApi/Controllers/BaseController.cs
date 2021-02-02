using Application.Models.Roles;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace bookstore_api.Controllers
{
	public class BaseController : ControllerBase
	{
		protected string BearerToken { get { return Request.Headers["roles"]; } }
		protected RequestToken RequestToken { get { return DeserializeToken(BearerToken ?? ""); } }

		private RequestToken DeserializeToken(string json)
		{

			if (string.IsNullOrEmpty(json)) return new RequestToken();

			var token = JsonConvert.DeserializeObject<RequestToken>(json);

			return token;
		}

	}
}

	

