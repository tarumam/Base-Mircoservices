using BaseProject.WebAPI.Core.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.Catalog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public object Get()
        {
            return new
            {
                status = "ICHOOSE-CATALOG ONLINE",
                HEROKU = HerokuConnection.GetHerokuConnection()
            };
        }
    }
}
