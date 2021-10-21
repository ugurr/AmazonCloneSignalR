using AmazonWebService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmazonWebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        public static Dictionary<string, List<Product>> ProductDB = new();
        private static readonly List<Product> ProductList = new List<Product>()
        {
            new Product(){ID=1,CreatedDate=DateTime.Now.AddDays(-10),Price=450.00,Name="xbox one x 1TB",Ratings=4,ImgPath="xboxonex1TB.jpg",Description="Microsoft Xbox One X 1Tb Console With Wireless Controller: Xbox One X Enhanced, Hdr, Native 4K, Ultra Hd"} ,
            new Product(){ID=2,CreatedDate=DateTime.Now.AddDays(-11),Price=350.00,Name="PlayStation 4 Slim 1TB",Ratings=1,ImgPath="PlayStation4Slim1TB.jpg",Description="PlayStation 4 Slim 1TB Console - Only On PlayStation Bundle"} ,
            new Product(){ID=3,CreatedDate=DateTime.Now.AddDays(-12),Price=782.00,Name="Pro Angular 9: Build Powerful and Dynamic Web Apps",Ratings=3,ImgPath="ProdAngular9.jpg",Description="Pro Angular 9: Build Powerful and Dynamic Web Apps 4th ed. Edition"} ,
            new Product(){ID=4,CreatedDate=DateTime.Now.AddDays(-13),Price=165.00,Name="JavaScript: The Definitive Guide: Master the World's Most-Used Programming Language",Ratings=4,ImgPath="Javascript.jpg",Description="JavaScript: The Definitive Guide: Master the World's Most-Used Programming Language 7th Edition" },
            new Product(){ID=5,CreatedDate=DateTime.Now.AddDays(-14),Price=987.00,Name="Lavazza Super Crema Whole Bean Coffee Blend, Medium Espresso Roast",Ratings=5,ImgPath="lavaza.jpg",Description="Lavazza Super Crema Whole Bean Coffee Blend, Medium Espresso Roast, 2.2 Pound (Pack of 1)"} ,
            new Product(){ID=6,CreatedDate=DateTime.Now.AddDays(-15),Price=258.00,Name="ColdWar",Ratings=4,ImgPath="ColdWar.jpg",Description="Call of Duty: Black Ops Cold War (PS5)"} ,
            new Product(){ID=7,CreatedDate=DateTime.Now.AddDays(-16),Price=475.00,Name="Returnal",Ratings=4,ImgPath="Returnal.jpg",Description="Returnal - PlayStation 5"} ,
            new Product(){ID=8,CreatedDate=DateTime.Now.AddDays(-17),Price=986.00,Name="Village",Ratings=4,ImgPath="Village.jpg",Description="An oppressive evil inhabits the forbidden forest"} ,
            new Product(){ID=9,CreatedDate=DateTime.Now.AddDays(-18),Price=852.00,Name="Sackboy",Ratings=4,ImgPath="Sackboy.jpg",Description="Sackboy: A Big Adventure (PS4)"} ,
           new Product(){ID=10,CreatedDate=DateTime.Now.AddDays(-19),Price=159.00,Name="Spiderman",Ratings=4,ImgPath="Spiderman.jpg",Description="Spider-Man [Tom Holland] begins to navigate his new identity as the web-slinging super hero under the watchful eye of his mentor Tony Stark."} ,
           new Product(){ID=11,CreatedDate=DateTime.Now.AddDays(-20),Price=436.00,Name="ValHalla",Ratings=4,ImgPath="ValHalla.jpg",Description="Siblings Roskva and Tjalfe embark on an epic adventure from Midgard to Valhalla with the gods Thor and Loki. "} ,

            new Product(){ID=8,CreatedDate=DateTime.Now.AddDays(-17),Price=986.00,Name="Cyberpunk",Ratings=4,ImgPath="Cyberpunk.jpg",Description="The year is 2047. Most of the world's population live inside corporate-controlled virtual worlds and drift further out of touch with reality."} ,
           
            new Product(){ID=9,CreatedDate=DateTime.Now.AddDays(-18),Price=852.00,Name="Horizon",Ratings=4,ImgPath="Horizon.jpg",Description="A former couple board a single-engine plane for a routine and casual flight to their friend's tropical island wedding."} ,
           new Product(){ID=10,CreatedDate=DateTime.Now.AddDays(-19),Price=159.00,Name="Fifa21",Ratings=4,ImgPath="Fifa21.jpg",Description="Win as one in EA SPORTS FIFA 21 on PlayStation 4, Xbox One"} ,
       

        };
        //{
        //    "ColdWar", "Returnal", "Village", "Sackboy", "Spiderman", "ValHalla", "Cyberpunk", "Horizon", "Control", "Fifa21"
        //};
       
        IHubProductDispatcher _dispatcher;

        public ProductController(IHubProductDispatcher hubProductDispatcher)
        {
            _dispatcher = hubProductDispatcher;
        }

        [HttpGet("{connectionID}")]
        public List<Product> Get(string connectionID)
        { 
            var rng = new Random();
            List<Product> Products = new List<Product>();
            foreach (var item in Enumerable.Range(1, 10))
            {
                var product = ProductList[rng.Next(ProductList.Count())];
                product.CreatedDate = DateTime.Now;
                Products.Add(product);
            }

            var result = Products.GroupBy(item => item.Name).Select(grp => grp.First()).Take(3).ToList();
            ProductDB.Add(connectionID, result);
            return result;
        }

        [HttpPost("UpdateProduct")]
        public async Task UpdateProduct([FromBody] Product product)
        {
            bool isChange = false;
            foreach (var productList in ProductDB)
            {
                var updateProduct = productList.Value.Where(g => g.Name == product.Name).FirstOrDefault();
                if (updateProduct != null)
                {
                    isChange = true;
                    productList.Value.Remove(updateProduct);
                    productList.Value.Add(product);
                    ProductDB[productList.Key] = productList.Value;
                }
            }
            if (isChange) await _dispatcher.ChangeProduct(product);
        }
    }


    public class ProductHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine("GetConnectionId:" + Context.ConnectionId);
            await Clients.Caller.SendAsync("GetConnectionId", Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            ProductController.ProductDB.Remove(Context.ConnectionId);
            Console.WriteLine("DisconnectID:" + Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task ClearProduct(Product product)
        {
            await Clients.All.SendAsync("ChangeProduct", product);
        }
    }
}
