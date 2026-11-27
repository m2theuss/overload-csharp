namespace overload.Extensions.Security;

public static class UseHeaderSimpleChecker{
    public static IApplicationBuilder AddSimpleHeaderChecker(this IApplicationBuilder app){
        return app.Use(async(context, next)=>{
            
            await next(context);
        });
    }
}