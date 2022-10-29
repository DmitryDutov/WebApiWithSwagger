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

        [HttpPost("add")] //POST -> "https://localhost:5163/api/Settings/add/NewEqp;NewEqp.txt;127.0.0.3;8888;false;unicode;1;[a-z]"
        public IActionResult Add([FromBody] string value)
        {
            var settings = Settings.SettingsObject;
            var stringGuid = settings.Eqps.Select(x => x.EqpGuid).Max().ToString().Replace("-", string.Empty);
            var num = Convert.ToInt64(stringGuid);
            num++;
            var format = string.Format($"{num:00000000-0000-0000-0000-000000000000}");
            var nextGuid = Guid.Parse(format);

            //Считываем строку, на основе которой будем формироваться новая единица оборудования
            var newEqp = CreateEqp(value);

            if (newEqp != null)
            {
                newEqp.EqpGuid = nextGuid;

                var listEquips = settings.Eqps; //получаем список оборудования
                listEquips.Add(newEqp);         //добавляем новое

                settings.Eqps = listEquips;
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

        private Eqp? CreateEqp(string value)
        {
            var parameters = value.Split(';').ToList();
            switch (parameters.Count)
            {
                case 8:
                    {
                        var newEqp = new Eqp()
                        {
                            Name = parameters[0],
                            Path = parameters[1],
                            Address = parameters[2],
                            Port = Convert.ToInt32(parameters[3]),
                            Active = Convert.ToBoolean(parameters[4]),
                            Encoding = parameters[5],
                            Interval = Convert.ToInt32(parameters[6]),
                            Mask = parameters[7],
                        };

                        return newEqp;
                    }
                case 6:
                    {
                        var newEqp = new Eqp()
                        {
                            Name = parameters[0],
                            Path = parameters[1],
                            Address = parameters[2],
                            Port = Convert.ToInt32(parameters[3]),
                            Active = Convert.ToBoolean(parameters[4]),
                            Encoding = parameters[5],
                        };

                        return newEqp;
                    }
            }

            return null;
        }
    }
}
