﻿using Newtonsoft.Json;

namespace Mirai.Net.Data.Events.Concretes.Bot
{
    public class ReconnectedEvent : EventBase
    {
        [JsonProperty("qq")]
        public string QQ {get; private set;}

        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public override Events Type { get; set; } = Events.Reconnected;
    }
}