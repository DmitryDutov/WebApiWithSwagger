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
                return Ok($"{newEqp.Name} добавлено");
            }

            return NotFound();
        }

        [HttpPut("edit/guid:{guid}")] // PUT -> 00000000-0000-0000-0000-000000000001; NewEqp;NewEqp.txt;127.0.0.3;8888;false;unicode;1;[a-z]
        public IActionResult Edit(Guid guid, [FromBody] string value)
        {
            var settings = Settings.SettingsObject;                                    //считываем все настройки
            var delSettings = settings.Eqps;                                           //считываем всё оборудование
            var obj = delSettings.FirstOrDefault(x => x?.EqpGuid == guid);              //находим объект

            var newEqp = CreateEqp(value);
            if (obj != null)
            {
                obj.EqpGuid = guid;
                obj.Name = newEqp.Name;
                obj.Path = newEqp.Path;
                obj.Address = newEqp.Address;
                obj.Port = Convert.ToInt32(newEqp.Port);
                obj.Active = Convert.ToBoolean(newEqp.Active);
                obj.Encoding = newEqp.Encoding;
                obj.Interval = Convert.ToInt32(newEqp.Interval);
                obj.Mask = newEqp.Mask;

                newEqp = null; //обнуляем новое оборудование

                //Settings.SaveSettings(Path.Combine(AppContext.BaseDirectory, "appsettings.json"));
                Settings.SaveSettings(@"C:\Test\Severstal.DeviceMonitoring\Settings\appsettings.json");

                return Ok($"Оборудование с номером {obj.EqpGuid} изменено");
            }

            return NotFound($"Оборудование не изменено");
        }

        [HttpDelete("{guid}")] //DELETE -> 00000000-0000-0000-0000-000000000013
        public IActionResult Delete(Guid guid)
        {
            var settings = Settings.SettingsObject;                                                    //считываем все настройки
            var delSettings = settings.Eqps;                                                           //считываем всё оборудование
            var delObject = delSettings.FirstOrDefault(x => x?.EqpGuid == guid);                        //находим удаляемый объект

            if (delObject != null)
            {
                delSettings.Remove(delObject);                                                                  //удаляем найденный объект из списка
                settings.Eqps = delSettings;                                                                    //обновляем информацию в Settings.Eqps

                //Settings.SaveSettings(Path.Combine(AppContext.BaseDirectory, "appsettings.json"));            //сохраняем в файл
                Settings.SaveSettings(@"C:\Test\Severstal.DeviceMonitoring\Settings\appsettings.json");

                return Ok($"Оборудование {delObject.Name} удалено");
            }

            return NotFound($"Оборудование на удалено");
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
