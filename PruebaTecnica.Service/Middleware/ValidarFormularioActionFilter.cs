using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PruebaTecnica.Service.Middleware;
public class ValidarFormularioActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
            context.Result = new JsonResult(new { Retorno = 1, Mensaje = "Los datos proporcionados son incorrectos" }) { StatusCode = 400 };
    }

    public void OnActionExecuted(ActionExecutedContext context)
    { }
}
