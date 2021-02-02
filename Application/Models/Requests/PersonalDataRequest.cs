using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Models.Requests
{
    public class PersonalDataRequest
    {
        public string IntermediaryCode { get; set; }
        public string Bio { get; set; }
        public bool? Kids { get; set; }
        public string PhotoUrl { get; set; }
        public string ClientValuePropositionUrl { get; set; }
        public string WhatsappNumber { get; set; }
        public string LinkedinUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string InstagramUrl { get; set; }
    }
}
