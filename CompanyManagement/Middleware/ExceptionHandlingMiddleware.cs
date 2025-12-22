using CompanyManagement.Api.Responses;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace CompanyManagement.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Middleware zodpovedny za centralizovane spracovanie vynimiek
        /// vzniknutych pocas spracovania HTTP poziadavky.
        /// </summary>
        /// <param name="next">
        /// Nasledujuci middleware v pipeline.
        /// </param>
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Zachyti vynimky vzniknute v dalsich castiach pipeline
        /// a prevedie ich na standardizovanu HTTP odpoved.
        /// </summary>
        /// <param name="context">
        /// Aktualny HTTP kontext poziadavky.
        /// </param>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentException ex)
            {

                // Chyba sposobena nespravnymi vstupnymi udajmi klienta
                await WriteError(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (ValidationException ex)
            {

                // Konflikt aplikacneho stavu (napr. porusenie business logiky)
                await WriteError(context, HttpStatusCode.Conflict, ex.Message);
            }
            catch (Exception)
            {
                // Neocakavana chyba na strane servera
                await WriteError(context, HttpStatusCode.InternalServerError, "Unexpected error occurred.");
            }
        }

        /// <summary>
        /// Zapise chybovu odpoved do HTTP response v JSON formate.
        /// </summary>
        /// <param name="context">HTTP kontext odpovede.</param>
        /// <param name="status">HTTP status kod odpovede.</param>
        /// <param name="message">Chybova sprava pre klienta.</param>
        private static async Task WriteError(HttpContext context, HttpStatusCode status, string message)
        {
            context.Response.StatusCode = (int)status;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<object>.Fail(message);

            await context.Response.WriteAsync(JsonSerializer.Serialize(response)); // JsonSerializer zdroj : ChatGPT
        }
    }
}
