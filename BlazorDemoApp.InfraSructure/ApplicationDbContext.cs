﻿using BlazorDemoApp.Shared.Classes.TableClass;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlazorDemoApp.InfraSructure
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<MyAppUser> MyAppUsers { get; set; }
        public virtual DbSet<MyAppUserPermission> MyAppUserPermission { get; set; }
        public virtual DbSet<Add2_City> Add2Cities { get; set; }
        public virtual DbSet<Add0_Gov> Add0Govs { get; set; }
        public virtual DbSet<Add1_Markaz> Add1Markazs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }


    public partial class ApplicationDbContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            Configure_MyAppUsers(modelBuilder);
            Configure_AddClasses(modelBuilder);
        }

        private void Configure_AddClasses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Add0_Gov>() //Detail
            .HasOne(d => d.MyAppUser)// child_navigation_list
            .WithMany(m => m.LAdd0_Gov)//parent_navigation_list
            .HasForeignKey(d => d.added_by);//keys

            modelBuilder.Entity<Add1_Markaz>() //Detail
                    .HasOne(d => d.MyAppUser)// child_navigation_list
                    .WithMany(m => m.LAdd1_Markaz)//parent_navigation_list
                    .HasForeignKey(d => d.added_by);//keys

            modelBuilder.Entity<Add2_City>() //Detail
                   .HasOne(d => d.MyAppUser)// child_navigation_list
                   .WithMany(m => m.LAdd2_City)//parent_navigation_list
                   .HasForeignKey(d => d.added_by);//keys


            //
            modelBuilder.Entity<Add1_Markaz>() //Detail
                   .HasOne(d => d.Add0_Gov)// child_parent_prop
                   .WithMany(m => m.Markazs)//parent_navigation_list
                   .HasForeignKey(d => d.IdFK_Gov);//keys

            //
            modelBuilder.Entity<Add2_City>() //Detail
                  .HasOne(d => d.Add1_Markaz)// child_parent_prop
                  .WithMany(m => m.Add2_City)//parent_navigation_list
                  .HasForeignKey(d => d.IdFK_Markaz);//keys
        }

        private void Configure_MyAppUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MyAppUser>()
                      .HasOne(c => c.Reg_by) // child_navigation_list
                      .WithMany(p => p.LMyAppUser)//parent_navigation_list
                      .HasForeignKey(c => c.IDFK_MyAppUser); //keys

            modelBuilder.Entity<MyAppUserPermission>()
                     .HasOne(c => c.MyAppUser) // child_navigation_list
                     .WithMany(p => p.LMyAppUserPermission)//parent_navigation_list
                     .HasForeignKey(c => c.added_by); //keys


            modelBuilder.Entity<MyAppUser>().HasData(
               new MyAppUser { Id = 1, UserName = "admin", UserPass = "admin@12345", Q1 = 1, Q2 = 2, Ans1 = "admin1", Ans2 = "admin2", isEmp = true, isPasswordReset = false, IsActive = true, IDFK_MyAppUser = 1 },
               new MyAppUser { Id = 2, UserName = "emp1", UserPass = "emp1@12345", Q1 = 1, Q2 = 2, Ans1 = "emp1emp1", Ans2 = "emp11emp11", isEmp = true, isPasswordReset = true, IsActive = true, IDFK_MyAppUser = 2 },
               new MyAppUser { Id = 3, UserName = "emp2", UserPass = "emp2@12345", Q1 = 1, Q2 = 2, Ans1 = "emp2emp2", Ans2 = "emp22emp22", isEmp = true, isPasswordReset = false, IsActive = true, IDFK_MyAppUser = 3 },
               new MyAppUser { Id = 4, UserName = "Parent", UserPass = "parent@12345", Q1 = 1, Q2 = 2, Ans1 = "parent", Ans2 = "parent1", isEmp = false, isParent = true, isPasswordReset = false, IsActive = true, IDFK_MyAppUser = 4 }
           );

        }
    }




   

}

        
    