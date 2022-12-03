using MarketPlace.DataLayer.Entities.Account;
using MarketPlace.DataLayer.Entities.Contact;
using MarketPlace.DataLayer.Entities.Products;
using MarketPlace.DataLayer.Entities.Site;
using MarketPlace.DataLayer.Entities.Store;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.Context
{
    public class MarketPlaceDbContext : DbContext
    {
        public MarketPlaceDbContext(DbContextOptions<MarketPlaceDbContext> options): base(options) { }
        

        #region account
        public DbSet<User> Users { get; set; }
        #endregion

        #region contact
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketMessage> TicketMessages { get; set; }

        #endregion

        #region site
        public DbSet<SiteSetting> SiteSettings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SiteBanner> SiteBanners { get; set; }
        #endregion

        #region store
        public DbSet<Seller> Sellers { get; set; }
        #endregion

        #region products
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductSelectedCategory> ProductSelectedCategories { get; set; }

        #endregion

        #region on model creating

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(s=>s.GetForeignKeys()) )
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}
