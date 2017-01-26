using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DefCon42
{
    public class ACRConfig : IRocketPluginConfiguration
    {
        [XmlElement("steamapikey")]
        public string steamapikey;

        public void LoadDefaults()
        {
            steamapikey = "yoursteamapikeygoeshere";
        }
    }
}
