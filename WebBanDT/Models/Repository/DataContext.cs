﻿using Microsoft.EntityFrameworkCore;
using WebBanDT.Models;

namespace WebBanDT.Models.Repository
{
	public class DataContext : Microsoft.EntityFrameworkCore.DbContext
	{
		public DataContext(DbContextOptions<DataContext> options):base(options) 
		{
		
		}
		public DbSet<BrandModel> Brands { get; set; }
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<CategoryModel> Categories { get; set; }	
	}
}
