using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Services
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // controller execute hoga
            }
            catch (DbUpdateException dbEx)
            {
                var message = dbEx.InnerException?.Message ?? dbEx.Message;

                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new
                {
                    message
                });
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
