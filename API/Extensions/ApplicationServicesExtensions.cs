using System.Linq;
using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationService( this IServiceCollection services)
        {
            
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAdminServices, AdminServices>();
            services.AddScoped<ISelDecisionService, SelDecisionServices>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICandidateService, CandidateService>();
            services.AddScoped<ICandidateCategoryService, CandidateCategoryService>();
            services.AddScoped<ICVEvaluationService, CVEvaluationService>();
            services.AddScoped<IAssessmentService, AssessmentService>();
            services.AddScoped<ICVRefService, CVRefService>();
            services.AddScoped<IDLService, DLService>();
            services.AddScoped<IDLForwardService, DLForwardService>();
            services.AddScoped<IEmployeeService, EmployeeServices>();
            services.AddScoped<IInternalHRService, InternalHRService>();
            services.AddScoped<IProcessServices, ProcessServices>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IEnquiryService, EnquiryService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
            services.AddScoped<IEmailService, EmailService>();

            // services.AddControllers().AddNewtonsoftJson();

            services.Configure<ApiBehaviorOptions>(options => 
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany( e => e.Value.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToArray();
                    
                    var errorResponse = new ApiValidationErrorResponse{Errors = errors};

                    return new BadRequestObjectResult(errorResponse);
                };
            });
            
            return services;
        }
    }
}