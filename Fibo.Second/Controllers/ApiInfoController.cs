using System.Web.Http;

namespace Fibo.Second.Controllers
{
    public class ApiInfoController : ApiController
    {
        [HttpGet, Route("~/")]
        public IHttpActionResult Get() => Ok(new { Name = "Fibo second" });
    }
}
