using System.Web.Http;

namespace Cocktails.Api.Controllers
{
    public class ApiInfoController : ApiController
    {
        [HttpGet, Route("~/")]
        public IHttpActionResult Get() => Ok(new { Name = "Fibo second" });
    }
}
