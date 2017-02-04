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
        [XmlElement("SayGod")]
        [XmlElement("SayVanish")]
        [XmlElement("SayAirdrop")]
        [XmlElement("SayMassAirdrop")]
        [XmlElement("SayTP")]
        [XmlElement("SayTeleport")]
        [XmlElement("SayI")]
        [XmlElement("SayV")]
        [XmlElement("SayKick")]
        [XmlElement("SaySlay")]
        [XmlElement("SayHeal")]
        [XmlElement("SayAdmin")]
        [XmlElement("SaySpy")]

        [XmlElement("LogAbuse")]

        [XmlElement("messagecolor")]
        [XmlElement("steamapikey")]

        public bool SayV;
        public bool SayKick;
        public bool SaySlay;       
        public bool SayHeal;
        public bool SayGod;
        public bool SayVanish;
        public bool SayAirdrop;
        public bool SayMassAirdrop;   
        public bool SayTP;    
        public bool SayTeleport;     
        public bool SayI;
        public bool SayAdmin;
        public bool SaySpy;

        public bool LogAbuse;

        public string steamapikey;
        public string messagecolor;

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

            steamapikey = "your steam apikey goes here";
            messagecolor = "red";
        }
    }
}
