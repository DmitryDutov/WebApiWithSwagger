using Newtonsoft.Json;
using System.Text;
//using Serilog;

namespace WebApiWithSwagger.Models
{
    public class Settings
    {
        private static volatile Settings _instance;
        private readonly List<Eqp> _currentSettings;
        private readonly string _mainPath;
        private static Rootobject _settingsObject;

        //Создаём публичные поля, которые будут считываться в качестве настроек
        public static List<Eqp> CurrentSettings => _instance._currentSettings;
        public static string MainPath => _instance._mainPath;
        public static Rootobject SettingsObject => _settingsObject;

        public static void Init()
        {
            //Log.Warning("Пытаюсь инициализировать сервис Settings");
            _instance = new Settings();
            //Log.Warning("Инициализирован сервис Settings");
        }

        public static void InitByCommand(string loadPath)
        {
            //Log.Warning("Пытаюсь инициализировать сервис Settings");
            _instance = new Settings(loadPath);
            //Log.Warning("Инициализирован сервис Settings");
        }

        public Settings()
        {
            var settings = GetSettings();
            _currentSettings = settings;

            var mainPath = GetMainPath();
            _mainPath = mainPath;
        }
        public Settings(string loadPath)
        {
            var settings = LoadSettings(loadPath);
            _currentSettings = settings;

            var mainPath = LoadMainPath(loadPath);
            _mainPath = mainPath;
        }

        //Получаем настройки из файла
        private static List<Eqp> GetSettings()
        {
            //Log.Warning("Пытаюсь прочитать настройки по стандартному пути (сервис Settings)");
            var listEqp = new List<Eqp>();

            try
            {
                var settings = JsonConvert.DeserializeObject<Rootobject>(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "appsettings.json"))); //todo: Какой путь тут должен быть, если настройки можно будет получать из разных мест?
                _settingsObject = settings;
                listEqp = (List<Eqp>)settings.Eqps;

                //Log.Warning($"Настройки по стандартному пути прочитаны (сервис Settings)");
            }
            catch (Exception e)
            {
                //Log.Error($"Ошибка чтения настроек по стандартному пути (сервис Settings): {e.Message}");
            }
            return listEqp;
        }

        private static string GetMainPath()
        {
            //Log.Warning("Пытаюсь прочитать основную директорию по стандартному пути (сервис Settings)");
            var mainPath = string.Empty;
            try
            {
                var settings = JsonConvert.DeserializeObject<Rootobject>(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "appsettings.json")));
                mainPath = settings.MainPath;

                //Log.Warning($"Основная директория прочитана по стандартному пути (сервис Settings)");
            }
            catch (Exception e)
            {
                //Log.Error($"Ошибка чтения директории по стандартному пути (сервис Settings): {e.Message}");
            }
            return mainPath;
        }

        public static void SaveSettings(string savePath)
        {
            try
            {
                //Log.Information($"Сериализация завершена объекта по пути {savePath}");
                var serialized = JsonConvert.SerializeObject(_settingsObject);

                if (serialized.Length > 0)
                {
                    File.Create(savePath).Close();
                    File.WriteAllText(savePath, serialized, Encoding.Unicode);
                    //Log.Information($"Сериализованный объект записан в файл по пути: {savePath}");
                }
            }
            catch (Exception e)
            {
                //Log.Error($"{e.Message}");
            }
        }
        public static List<Eqp> LoadSettings(string loadPath)
        {
            var settings = new List<Eqp>();
            try
            {
                var getSettings = JsonConvert.DeserializeObject<Rootobject>(File.ReadAllText(loadPath));
                _settingsObject = getSettings;
                settings = (List<Eqp>)getSettings.Eqps;
                //Log.Information($"Настройки из файла {loadPath} прочитаны");
            }
            catch (Exception e)
            {
                //Log.Error($"Ошибка чтения настроек по пути {loadPath} (сервис Settings): {e.Message}");
            }

            return settings;
        }
        public static string LoadMainPath(string loadPath)
        {
            var mainPath = string.Empty;
            try
            {
                var getSettings = JsonConvert.DeserializeObject<Rootobject>(File.ReadAllText(loadPath));
                mainPath = getSettings.MainPath;
            }
            catch (Exception e)
            {
                //Log.Error($"Ошибка чтения общей директории по пути {loadPath} (сервис Settings): {e.Message}");
            }

            return mainPath;
        }
    }

}
