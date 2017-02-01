using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace DefCon42
{
    public class ACRConfig : IRocketPluginConfiguration
    {
        [XmlElement("steamapikey")]
        public string steamapikey;
        [XmlElement("messagecolor")]
        public string messagecolor;

        public void LoadDefaults()
        {
            steamapikey = "yoursteamapikeygoeshere";
            messagecolor = "red";
        }
    }
}
