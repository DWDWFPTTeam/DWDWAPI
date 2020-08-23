using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DWDW_API.Core.Constants;
using DWDW_API.Core.Entities;
using DWDW_API.Core.ViewModels;
using DWDW_API.Providers;
using DWDW_Service.Repositories;
using DWDW_Service.Services;
using DWDW_Service.UnitOfWorks;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace DWDW_API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private AppSettings appSettings;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddMvc();

            SetupSwagger(services);
            SetupDbContext(services);
            SetupAutoMapper();
            SetupAuthentication(services);
            SetupDI(services);

            //Hangfire
            //string ConnectionString = "Server=.;Database=DWDB;Trusted_Connection=True;User Id=sa;Password=123;MultipleActiveResultSets=True;";
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();

        }

        private void SetupDI(IServiceCollection services)
        {
            //config services
            services.AddHttpContextAccessor();
            services.AddSingleton(Configuration);
            services.AddScoped<ExtensionSettings>();
            services.AddScoped<JwtTokenProvider>();
            services.AddScoped<UnitOfWork>();
            services.AddScoped<DbContext, DWDBContext>();

            //initialize implement instances

            //Notification instances
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationService, NotificationService>();


            //Arrangement instances
            services.AddScoped<IArrangementRepository, ArrangementRepository>();

            //Device instances
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IDeviceService, DeviceService>();

            //Location instances
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ILocationRepository, LocationRepository>();

            //Record instances
            services.AddScoped<IRecordRepository, RecordRepository>();
            services.AddScoped<IRecordService, RecordService>();

            //Role instances
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();

            //Room instances
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoomService, RoomService>();

            //RoomDevice instances
            services.AddScoped<IRoomDeviceRepository, RoomDeviceRepository>();

            //Shift instances
            services.AddScoped<IShiftRepository, ShiftRepository>();
            services.AddScoped<IShiftService, ShiftService>();

            //User instances
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
        }

        private void SetupAuthentication(IServiceCollection services)
        {
            const string scheme = JwtBearerDefaults.AuthenticationScheme;

            // Add authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = scheme;
                options.DefaultChallengeScheme = scheme;
                options.DefaultScheme = scheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidAudience = appSettings.Audience,
                    ValidIssuer = appSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(appSettings.SecretKey))
                };
            });
        }

        private void SetupAutoMapper()
        {
            Mapper.Initialize(cfg =>
            {
                //Mapping <TSource, TDestination>() 

                //User Mapping
                cfg.CreateMap<User, UserViewModel>(); //Mapping User fields to UserViewModel fields
                cfg.CreateMap<UserViewModel, User>();
                cfg.CreateMap<UserCreateModel, User>();
                cfg.CreateMap<UserUpdateModel, User>();
                cfg.CreateMap<UserActiveModel, User>();
                cfg.CreateMap<UserGetAllViewModel, User>();
                cfg.CreateMap<User, UserGetAllViewModel>();
                cfg.CreateMap<UserPersonalUpdateModel, User>();
                cfg.CreateMap<User, UserPersonalUpdateModel>();

                //Location Mapping
                cfg.CreateMap<Location, LocationViewModel>();
                cfg.CreateMap<LocationViewModel, Location>();
                cfg.CreateMap<LocationInsertModel, Location>();
                cfg.CreateMap<LocationUpdateStatusModel, Location>();
                cfg.CreateMap<LocationUserViewModel, Location>();
                cfg.CreateMap<Location, LocationUserViewModel>();



                //Device Mapping
                cfg.CreateMap<Device, DeviceViewModel>();
                cfg.CreateMap<DeviceViewModel, Device>();
                cfg.CreateMap<DeviceCreateModel, Device>();
                cfg.CreateMap<DeviceUpdateModel, Device>();
                cfg.CreateMap<DeviceActiveModel, Device>();

                //Arrangement Mapping
                cfg.CreateMap<Arrangement, ArrangementViewModel>();
                cfg.CreateMap<ArrangementViewModel, Arrangement>();
                cfg.CreateMap<Arrangement, ArrangementReceivedViewModel>();
                cfg.CreateMap<ArrangementReceivedViewModel, Arrangement>();
                cfg.CreateMap<Arrangement, ArrangementDisableViewModel>();
                cfg.CreateMap<ArrangementDisableViewModel, Arrangement>();


                //Record Mapping
                cfg.CreateMap<Record, RecordViewModel>();
                cfg.CreateMap<RecordViewModel, Record>();
                cfg.CreateMap<RecordImageViewModel, Record>();
                cfg.CreateMap<Record, RecordImageViewModel>();
                cfg.CreateMap<Record, RecordRoomCodeViewModel>();
                cfg.CreateMap<RecordRoomCodeViewModel, Record>();
                cfg.CreateMap<RecordStatusModel, Record>();

                //Role Mapping
                cfg.CreateMap<Role, RoleViewModel>();
                cfg.CreateMap<RoleViewModel, Role>();
                cfg.CreateMap<RoleCreateModel, Role>();
                cfg.CreateMap<RoleUpdateModel, Role>();
                cfg.CreateMap<RoleActiveModel, Role>();

                //Room Mapping
                cfg.CreateMap<Room, RoomViewModel>();
                cfg.CreateMap<RoomViewModel, Room>();

                //RoomDevice Mapping
                cfg.CreateMap<RoomDevice, RoomDeviceViewModel>();
                cfg.CreateMap<RoomDeviceViewModel, RoomDevice>();
                cfg.CreateMap<RoomDevice, RoomDeviceAssignModel>();

                //Shift Mapping
                cfg.CreateMap<Shift, ShiftViewModel>();
                cfg.CreateMap<ShiftViewModel, Shift>();
                cfg.CreateMap<ShiftCreateModel, Shift>();
                cfg.CreateMap<ShiftUpdateModel, Shift>();
                cfg.CreateMap<ShiftActiveModel, Shift>();


                //User Mapping
                cfg.CreateMap<User, UserViewModel>();
                cfg.CreateMap<UserViewModel, User>();
                cfg.CreateMap<UserPersonalUpdateModel, User>();
                cfg.CreateMap<UserAssignViewModel, User>();
                cfg.CreateMap<User, UserAssignViewModel>();

                //Notification Mapping
                cfg.CreateMap<Notifications, NotificationViewModel>();
                cfg.CreateMap<NotificationViewModel, Notifications>();
            });
        }

        private void SetupDbContext(IServiceCollection services)
        {
            services.AddDbContext<DWDBContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
        }

        private void SetupSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = Constant.PROJECTNAME, Version = "v1" });
                c.AddSecurityDefinition(Constant.BEARER, new OpenApiSecurityScheme
                {

                    Description = Constant.JWT_DESCRIPTION,
                    Name = Constant.AUTHORIZATION,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = Constant.BEARER
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement(){
                  {
                        new OpenApiSecurityScheme
                         {
                              Reference = new OpenApiReference
                                {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = Constant.BEARER
                                },
                                Scheme = Constant.OAUTH2,
                                Name = Constant.BEARER,
                                In = ParameterLocation.Header,

                          },
                          new List<string>()
                  }
                });

            });
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", appSettings.Audience);
            });

            app.UseRouting();

            app.UseCors(options =>
            {
                options.AllowAnyMethod();
                options.AllowAnyOrigin();
                options.AllowAnyHeader();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            //Hangfire
            var options = new BackgroundJobServerOptions
            {
                SchedulePollingInterval = TimeSpan.FromMilliseconds(1000)
            };

            app.UseHangfireServer(options);
            app.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization = new List<IDashboardAuthorizationFilter>()
            {
                new HangfireDashboardFilter()
            },
                IsReadOnlyFunc = CapstoneContext => false
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
