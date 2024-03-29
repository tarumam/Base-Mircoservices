﻿using BaseProject.WebAPI.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace BaseProject.Clients.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        IHostEnvironment _env;
        public StatusController(IHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        public object Get()
        {
            return new
            {
                status = "ICHOOSE-CLIENTS ONLINE",
                HEROKU = HerokuConnection.GetHerokuConnection(),
                Environment = _env.EnvironmentName
            };
        }
    }
}
