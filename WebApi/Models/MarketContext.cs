using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class MarketContext:DbContext
    {
        public MarketContext() : base("name=MarketDBConnectionString")
        {
            //Database.SetInitializer<MarketContext>(new DropCreateDatabaseAlways<MarketContext>());
        }
        public DbSet<UserDetails> Users { get; set; }
        public DbSet<ProductDetails> Products { get; set; }
    }
}