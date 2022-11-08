using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Serilog;

namespace HospitalManagementSystem.Api.Helpers
{
    public static class ModelValidation
    {
        public static async Task<IActionResult?> ValidateModelAsync(ModelStateDictionary modelState, string commandName, HttpContext context)
        {
            if (modelState.IsValid) return null;

            var exceptionList = new List<string>();

            modelState.Values.Select(m => m.Errors).ToList().ForEach(c => c.ToList().ForEach(e => exceptionList.Add(e.ErrorMessage)));
            var exception = $"The {commandName} is invalid.\n{exceptionList.Count} error(s) found:\n{JsonConvert.SerializeObject(exceptionList)}";
               
            await AddExceptionMessageAsync(context, exception);

            return new BadRequestObjectResult(exception);
        }

        public static async Task AddExceptionMessageAsync(HttpContext context, string exception)
        {
            Log.Warning($"route={context.Request.Path} status={context.Response.StatusCode} exception={exception}");

            var responseStream = new MemoryStream();
            var bytesToWrite = Encoding.UTF8.GetBytes(exception);
            await responseStream.WriteAsync(bytesToWrite, 0, bytesToWrite.Length);
            responseStream.Seek(0, SeekOrigin.Begin);
            context.Response.Body = responseStream;
        }
    }
}
