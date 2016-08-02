﻿using Predictions.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Predictions.DAL
{
    public class PredictionsContext : DbContext
    {
        public PredictionsContext() : base("PredictionsContext")
        {
        }

        public DbSet<Expert> Experts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}