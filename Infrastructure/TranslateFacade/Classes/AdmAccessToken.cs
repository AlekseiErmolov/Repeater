﻿using System.Runtime.Serialization;

namespace Repeater.Infrastructure.TranslateFacade.Classes
{
    [DataContract]
    public class AdmAccessToken
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }

        [DataMember(Name = "expires_in")]
        public string ExpiresIn { get; set; }

        [DataMember(Name = "scope")]
        public string Scope { get; set; }
    }
}