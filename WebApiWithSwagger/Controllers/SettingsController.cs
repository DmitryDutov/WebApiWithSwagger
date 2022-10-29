using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApiWithSwagger.Models;

namespace WebApiWithSwagger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        // GET: api/<SettingsController>
        [HttpGet]
        public string Get() //GET: "http://localhost:5163/api/Settings"
        {
            var settings = Settings.SettingsObject;
            var serialize = JsonConvert.SerializeObject(settings);
            return serialize;
        }


        // GET api/<SettingsController>/5
        [HttpGet("{guid}")] //GET: "http://localhost:5163/api/Settings/00000000-0000-0000-0000-000000000001"
        public string GetByGuid(Guid guid)
        {
            var settings = Settings.SettingsObject;
            var eqp = settings.Eqps.FirstOrDefault(x => x.EqpGuid == guid);
            var serialize = JsonConvert.SerializeObject(eqp);

            return serialize;
        }

        // POST api/<SettingsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SettingsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SettingsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
