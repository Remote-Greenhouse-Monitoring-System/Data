
using System.Security.Cryptography;
using System.Text;
using FirebaseAdmin.Auth.Hash;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GreenhouseDataAPI.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter

    {
        private const string Apikeyname = "ApiKey";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(Apikeyname, out var extractedApiKey))
            {
                context.Result = new ContentResult()
                {


                    StatusCode = 401,


                    Content = "Api Key was not provided"


                };
                return;
            }
            HashAlgorithm sha = SHA256.Create();
            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = appSettings.GetValue<string>(Apikeyname);
            var encryptedKey = sha.ComputeHash(Encoding.Default.GetBytes(extractedApiKey!));
            if (!apiKey.Equals(ByteArrayToString(encryptedKey)))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Api Key is not valid"
                };
                return;
            }
            await next();
        }
        static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i=0;i < arrInput.Length -1; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}
