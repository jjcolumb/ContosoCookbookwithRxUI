using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ContosoCookbook.Business
{
    public class Recipe
    {
        [JsonProperty("UniqueId")]
        public string ID { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string TileImagePath { get; set; }
        public int PrepTime { get; set; }
        public string Directions { get; set; }
        public List<string> Ingredients { get; set; }

        public override string ToString()
        {
            return String.Format("{0}", Title);
        }
    }
}
