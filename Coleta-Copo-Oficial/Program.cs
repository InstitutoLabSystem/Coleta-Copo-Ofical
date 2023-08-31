using Coleta_Copo_Oficial.Data;
using Copo_Coleta.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.Identity.Client;

namespace Copo_Coleta
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<BancoContext>
              (options => options.UseMySql(
                  "server=novolab.c82dqw5tullb.sa-east-1.rds.amazonaws.com;user id=sistema;password=7847awse;database=labdados",
                  Microsoft.EntityFrameworkCore.ServerVersion.Parse("13.2.0-mysql")));

            builder.Services.AddDbContext<CoposContext>
              (options => options.UseMySql(
                  "server=novolab.c82dqw5tullb.sa-east-1.rds.amazonaws.com;user id=sistema;password=7847awse;database=copos",
                  Microsoft.EntityFrameworkCore.ServerVersion.Parse("13.2.0-mysql")));

            builder.Services.AddDbContext<IncertezaContext>
             (options => options.UseMySql(
                 "server=novolab.c82dqw5tullb.sa-east-1.rds.amazonaws.com;user id=sistema;password=7847awse;database=incertezas",
                 Microsoft.EntityFrameworkCore.ServerVersion.Parse("13.2.0-mysql")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
                {
                    option.LoginPath = "/Acess/Login";
                    option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Acess}/{action=Login}/{id?}");

            app.Run();
        }
    }
}