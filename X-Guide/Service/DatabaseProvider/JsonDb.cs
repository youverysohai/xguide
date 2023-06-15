using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using X_Guide.MVVM.Model;

namespace X_Guide.Service.DatabaseProvider
{
    internal class JsonDb : IJsonDb
    {
        private static readonly string specialFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/X-Guide";

        private readonly Dictionary<Type, string> filepaths = new Dictionary<Type, string>()
        {
            {typeof(GeneralModel), "general.json"},
            {typeof(HikVisionModel), "hikvision.json" },
            {typeof(HalconModel), "halcon.json" },
            {typeof(SmartCamModel), "smartcam.json" },
        };

        private readonly HikVisionConfiguration _visionConfiguration;

        public T Get<T>() where T : new()
        {
            filepaths.TryGetValue(typeof(T), out var path);
            path = Path.Combine(specialFolder, path);
            if (!File.Exists(path)) Update(new T());
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }

        public void Update<T>(T data)
        {
            filepaths.TryGetValue(typeof(T), out var path);
            path = Path.Combine(specialFolder, path);
            File.WriteAllText(path, JsonConvert.SerializeObject(data));
        }
    }
}