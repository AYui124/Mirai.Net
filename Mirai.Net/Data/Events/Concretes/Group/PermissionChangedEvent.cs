﻿using Mirai.Net.Data.Shared;
using Newtonsoft.Json;

namespace Mirai.Net.Data.Events.Concretes.Group
{
    public class PermissionChangedEvent : EventBase
    {
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public override Events Type { get; set; } = Events.PermissionChanged;
        
        [JsonProperty("origin")]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public Permissions Origin {get; set;}
        
        [JsonProperty("current")]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public Permissions Current {get; set;}  
        
        [JsonProperty("group")]
        public Shared.Group Group {get; set;}
    }
}