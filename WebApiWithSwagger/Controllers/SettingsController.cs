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

        [HttpPost("add")] //POST -> http://localhost:5229/api/add
        public IActionResult Add([FromBody] string value)
        {
            var settings = Settings.SettingsObject;
            var eqpGuid = settings.Eqps.Select(x => x.EqpGuid).Max();
            var line = Convert.ToString(eqpGuid);
            var num = Convert.ToInt64(line.Replace("-", string.Empty));
            num++;
            var format = string.Format($"{num:00000000-0000-0000-0000-000000000000}"); //00000000-0000-0000-0000-000000000012
            var nextGuid = Guid.Parse(format);

            //var deserialize = JsonConvert.DeserializeObject<Rootobject>(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "appsettings.json")));
            var newEqp = JsonConvert.DeserializeObject<Eqp>(value);
            if (newEqp != null)
            {
                newEqp.EqpGuid = nextGuid;

                var listEquips = settings.Eqps; //получаем список оборудования
                listEquips.Add(newEqp); //добавляем новое

                //Settings.SaveSettings(Path.Combine(AppContext.BaseDirectory, "appsettings.json"));
                Settings.SaveSettings(@"C:\Test\Severstal.DeviceMonitoring\Settings\appsettings.json");
                return Ok();
            }

            return NotFound();
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
