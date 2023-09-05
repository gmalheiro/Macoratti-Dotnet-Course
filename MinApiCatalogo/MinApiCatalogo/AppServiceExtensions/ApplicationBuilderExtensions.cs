namespace MinApiCatalogo.AppServiceExtensions
{
    public static class ApplicationBuilderExtensions
    {
        public static  IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app,
            IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            return app;
        }
    }
}
