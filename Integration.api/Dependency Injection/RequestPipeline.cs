namespace Integration.api.Dependency_Injection
{
    public static  class RequestPipeline
    {
        public static  WebApplication App(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();  
            var app = builder.Build();

            // Add services to the container.
          
           

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

          

            app.MapControllers();

            return app; 


        }
    }
}
