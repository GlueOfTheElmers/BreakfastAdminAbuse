using Rocket.API;
using Rocket.API.Serialisation;
using Rocket.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace DefCon42
{
    [Serializable]
    public class ACRConfig : IRocketPluginConfiguration
    {
        [XmlElement("SayV")]
        public bool SayV;

        [XmlElement("SayKick")]
        public bool SayKick;

        [XmlElement("SaySlay")]
        public bool SaySlay;

        [XmlElement("SayHeal")]
        public bool SayHeal;

        [XmlElement("SayGod")]
        public bool SayGod;

        [XmlElement("SayVanish")]
        public bool SayVanish;

        [XmlElement("SayAirdrop")]
        public bool SayAirdrop;

        [XmlElement("SayMassAirdrop")]
        public bool SayMassAirdrop;

        [XmlElement("SayTP")]
        public bool SayTP;

        [XmlElement("SayTeleport")]
        public bool SayTeleport;

        [XmlElement("SayI")]
        public bool SayI;

        [XmlElement("SayAdmin")]
        public bool SayAdmin;

        [XmlElement("SaySpy")]
        public bool SaySpy;


        [XmlElement("LogAbuse")]
        public bool LogAbuse;

        [XmlElement("UseIgnorePermission")]
        public bool UseIgnorePermission;

        [XmlElement("IgnoreTrueAdmins")]
        public bool IgnoreTrueAdmins;


        [XmlElement("messagecolor")]
        public string messagecolor;
        [XmlElement("steamapikey")]
        public string steamapikey;

        public void LoadDefaults()
        {
            SayV = true;
            SayKick = true;
            SaySlay = true;
            SayHeal = true;
            SaySpy = true;
            SayGod = true;
            SayVanish = true;
            SayAirdrop = true;
            SayMassAirdrop = true;
            SayTP = true;
            SayTeleport = true;
            SayI = true;
            SayAdmin = true;

            LogAbuse = true;
            UseIgnorePermission = false;

            IgnoreTrueAdmins = false;

            steamapikey = "your steam apikey goes here";
            messagecolor = "red";
        }
    }
}
