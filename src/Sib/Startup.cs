// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="BFBVM">
//   © 2017 BFBVM
// </copyright>
// <summary>
//   The web application startup file.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sib
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;

    using Core.Authentication;
    using Core.Models;
    using Core.Settings;

    using Microsoft.AspNetCore.Authentication.OAuth;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.MongoDB;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Models;

    using MongoDB.Driver;

    using Services;
    using Repository;

    using IServiceRepository = Core.Interfaces.IServiceRepository;

    /// <summary>
    /// The startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// The environment variable.
        /// </summary>
        private readonly IHostingEnvironment _env;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">
        /// The env.
        /// </param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            this.Configuration = builder.Build();
            this._env = env;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDbSettings"));

            services.AddSingleton<IUserStore<ApplicationUser>>(provider =>
            {
                var options = provider.GetService<IOptions<MongoDbSettings>>();
                var client = new MongoClient(options.Value.ConnectionString);
                var database = client.GetDatabase(options.Value.DatabaseName);
                var collection = database.GetCollection<ApplicationUser>(nameof(ApplicationUser));

                return new UserStore<ApplicationUser>(collection);
            });

            services.Configure<IdentityOptions>(options =>
            {
                var dataProtectionPath = Path.Combine(_env.WebRootPath, "identity-artifacts");
                options.Cookies.ApplicationCookie.AuthenticationScheme = "ApplicationCookie";

                var directory = new DirectoryInfo(dataProtectionPath);
                var provider = DataProtectionProvider.Create(directory);
                options.Cookies.ApplicationCookie.DataProtectionProvider = provider;

                options.Lockout.AllowedForNewUsers = true;
            });

            services.AddSingleton<IRoleStore<IdentityRole>>(
                provider =>
                    {
                        var options = provider.GetService<IOptions<MongoDbSettings>>();
                        var client = new MongoClient(options.Value.ConnectionString);
                        var database = client.GetDatabase(options.Value.DatabaseName);
                        var collection = database.GetCollection<IdentityRole>(nameof(IdentityRole));

                        var roleStore = new RoleStore<IdentityRole>(collection);

                        var admin = roleStore.FindByNameAsync(Roles.Administrator, CancellationToken.None).Result;
                        if (admin == null)
                        {
                            roleStore.CreateAsync(new IdentityRole(Roles.Administrator), CancellationToken.None).Wait();
                        }

                        return roleStore;
                    });


            services.AddSingleton<IServiceRepository>(
                provider =>
                    {
                        var options = provider.GetService<IOptions<MongoDbSettings>>();
                        return new ServiceRepository(options.Value);
                    });

            // Services used by identity
            services.AddAuthentication(options =>
            {
                // This is the Default value for ExternalCookieAuthenticationScheme
                options.SignInScheme = new IdentityCookieOptions().ExternalCookieAuthenticationScheme;
            });

            // Hosting doesn't add IHttpContextAccessor by default
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddOptions();
            services.AddDataProtection();

            services.TryAddSingleton<IdentityMarkerService>();
            services.TryAddSingleton<IUserValidator<ApplicationUser>, UserValidator<ApplicationUser>>();
            services.TryAddSingleton<IPasswordValidator<ApplicationUser>, PasswordValidator<ApplicationUser>>();
            services.TryAddSingleton<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
            services.TryAddSingleton<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            services.TryAddSingleton<IdentityErrorDescriber>();
            services.TryAddSingleton<ISecurityStampValidator, SecurityStampValidator<ApplicationUser>>();
            services
                .TryAddSingleton<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory<ApplicationUser>>();
            services.TryAddSingleton<SibUserManager<ApplicationUser>, SibUserManager<ApplicationUser>>();
            services.TryAddScoped<SignInManager<ApplicationUser>, SibSignInManager<ApplicationUser>>();



            services.AddSingleton<IUserRoleStore<ApplicationUser>>(
                provider =>
                    {
                        var options = provider.GetService<IOptions<MongoDbSettings>>();
                        var client = new MongoClient(options.Value.ConnectionString);
                        var database = client.GetDatabase(options.Value.DatabaseName);
                        var collection = database.GetCollection<ApplicationUser>(nameof(ApplicationUser));

                        return new UserStore<ApplicationUser>(collection);
                    });

            AddDefaultTokenProviders(services);

            services.AddMvc();

            // policies
            services.AddAuthorization(
                options =>
                    {
                        options.AddPolicy("Administrator", builder => builder.RequireRole(Roles.Administrator));
                    });

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }


        /// <summary>
        /// The configure. This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">
        /// The app.
        /// </param>
        /// <param name="env">
        /// The env.
        /// </param>
        /// <param name="loggerFactory">
        /// The logger factory.
        /// </param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            var facebookAppSecrets = this.Configuration["FbAppSecret"];

            app.UseIdentity()
                .UseFacebookAuthentication(new FacebookOptions
                {
                    AppId = "1483019898393253",
                    AppSecret = facebookAppSecrets,
                    Scope = { "public_profile", "email", "user_birthday" },
                    Events = new OAuthEvents
                    {
                        OnTicketReceived = context =>
                        {
                            var name = context.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                            var firstName = context.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName);
                            var lastName = context.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname);

                            context.HttpContext.Items.Add("name", name.Value);
                            context.HttpContext.Items.Add("firstname", firstName.Value);
                            context.HttpContext.Items.Add("lastname", lastName.Value);

                            return Task.CompletedTask;
                        }
                    }
                });

            //// Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// The add default token providers.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        private static void AddDefaultTokenProviders(IServiceCollection services)
        {
            var dataProtectionProviderType =
                typeof(DataProtectorTokenProvider<>).MakeGenericType(typeof(ApplicationUser));
            ////var phoneNumberProviderType = typeof(PhoneNumberTokenProvider<>).MakeGenericType(typeof(ApplicationUser));
            var emailTokenProviderType = typeof(EmailTokenProvider<>).MakeGenericType(typeof(ApplicationUser));
            AddTokenProvider(services, TokenOptions.DefaultProvider, dataProtectionProviderType);
            AddTokenProvider(services, TokenOptions.DefaultEmailProvider, emailTokenProviderType);
            ////AddTokenProvider(services, TokenOptions.DefaultPhoneProvider, phoneNumberProviderType);
        }

        /// <summary>
        /// The add token provider.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        /// <param name="providerName">
        /// The provider name.
        /// </param>
        /// <param name="provider">
        /// The provider.
        /// </param>
        private static void AddTokenProvider(IServiceCollection services, string providerName, Type provider)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Tokens.ProviderMap[providerName] = new TokenProviderDescriptor(provider);
            });

            services.AddSingleton(provider);
        }
    }
}
