using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;
using XDF.Core.Helper.Ajax;
using XDF.Service;
using XDF.Web.AuthHelper.OverWrite;
using XDF.Web.Middleware;

namespace XDF.Web
{
    public class Startup
    {
        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(SecretKey));
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            #region MVC
            services.AddMvc()
                .AddXmlSerializerFormatters()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            #endregion

            #region 跨域
            services.AddCors(options =>
              {
                  options.AddPolicy("any", builder =>
                  {
                      builder.AllowAnyOrigin() //允许任何来源的主机访问
                             .AllowAnyMethod()
                             .AllowAnyHeader()
                             .AllowCredentials(); //指定处理cookie
                  });
              });
            #endregion

            #region 参数验证
            services.Configure<ApiBehaviorOptions>(options =>
              {
                  options.InvalidModelStateResponseFactory = actionContext =>
                  {
                      var errors = actionContext.ModelState
                          .Where(e => e.Value.Errors.Count > 0)
                          .Select(e => new AjaxResultModel<string>
                          {
                              Msg = $"{e.Key}--{e.Value.Errors.First().ErrorMessage}"
                          }).ToArray();

                      return new BadRequestObjectResult(errors);
                  };
              });
            #endregion

            #region Swagger          
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info
                    {
                        Version = "v0.1.0",
                        Title = "测试 API",
                        Description = "框架说明文档",
                        TermsOfService = "None",
                        Contact = new Contact
                        {
                            Name = "学习",
                            Email = "951400721@qq.com",
                            Url = "https://www.baidu.com"
                        }
                    });
                    //var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                    //var xmlPath = Path.Combine(basePath, "Blog.Core.xml");//这个就是刚刚配置的xml文件名
                    var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                    var xmlPath = Path.Combine(basePath, "XDF.Web.xml");//这个就是刚刚配置的xml文件名
                    var xmlModelPath = Path.Combine(basePath, "XDF.Core.xml");//这个就是Model层的xml文件名
                    c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改
                    c.IncludeXmlComments(xmlModelPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改
                    // 发行人
                    var IssuerName = (Configuration.GetSection("Audience"))["Issuer"];
                    var security = new Dictionary<string, IEnumerable<string>> { { IssuerName, new string[] { } }, };
                    c.AddSecurityRequirement(security);

                    //方案名称“Blog.Core”可自定义，上下一致即可
                    c.AddSecurityDefinition(IssuerName, new ApiKeyScheme
                    {
                        Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                        Name = "Authorization",//jwt默认的参数名称
                        In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                        Type = "apiKey"
                    });
                });

            #endregion

            #region Token 服务注册
            services.AddSingleton(factory =>
                {
                    var cache = new MemoryCache(new MemoryCacheOptions());
                    return cache;
                });
            services.AddAuthorization(options =>
             {
                 options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                 options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                 options.AddPolicy("AdminOrClient", policy => policy.RequireRole("Admin,Client").Build());

             });
            #endregion





        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });
            //错误中间件
            // app.UseMiddleware<ErrorHandler>();
            app.UseCustomException(new CustomExceptionMiddleWareOption(
                                handleType: CustomExceptionHandleType.JsonHandle,  //根据url关键字决定处理方式
                        jsonHandleUrlKeys: new PathString[] { "/api" },
                          errorHandingPath: "/home/error"));
            app.UseMiddleware<JwtTokenAuth>();
            //添加NLog
            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");//读取Nlog配置文件

            app.UseMvc();
            app.UseStaticFiles();
            #region Swagger           
            app.UseSwagger(); app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1"); });
            #endregion


        }
    }
}
