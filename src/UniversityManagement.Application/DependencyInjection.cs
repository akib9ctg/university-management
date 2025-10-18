using Microsoft.Extensions.DependencyInjection;

namespace UniversityManagement.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

                //configuration.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            });

            //services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);
        }
    }
}
