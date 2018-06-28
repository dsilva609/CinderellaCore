using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CinderellaCore.Model.Models.BoardGameGeek
{
    [XmlType("items")]
    public class BGGGame
    {
        [XmlElement("item")]
        public List<Item> Items { get; set; }
    }
}
