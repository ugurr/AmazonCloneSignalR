using System;

namespace AmazonWebService.Models
{
    public class Product
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public DateTime CreatedDate { get; set; }
        public string ImgPath { get; set; }
        public int Ratings { get; set; }
        public string Description { get; set; }

    }
}
