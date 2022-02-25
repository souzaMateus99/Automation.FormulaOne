using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Script.FormulaOneCalendar.Model;
using Script.FormulaOneCalendar.Service.Interfaces;

namespace Script.FormulaOneCalendar.Service
{
    public class FileStorageService : IStorageService
    {
        private readonly string _filepath;
        
        public FileStorageService(int seasonYear)
        {
            _filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{seasonYear}_race_storage.json");
        }

        private async Task<Dictionary<string, Race>> GetRacesFromFileAsync()
        {
            if (File.Exists(_filepath))
            {
                using (var reader = new StreamReader(_filepath))
                {
                    var content = await reader.ReadToEndAsync();

                    return JsonConvert.DeserializeObject<Dictionary<string, Race>>(content);
                }
            }

            return new Dictionary<string, Race>();
        }

        private async Task SaveFileAsync(string content)
        {
            using (var writer = new StreamWriter(_filepath))
            {
                await writer.WriteAsync(content);
            }
        }

        public async Task<KeyValuePair<string, Race>> GetRaceAsync(string eventId)
        {
            var races = await GetRacesFromFileAsync();

            return races.FirstOrDefault(r => r.Key.Equals(eventId));
        }

        public async Task SaveRaceAsync(string eventId, Race race)
        {
            var races = await GetRacesFromFileAsync();

            if (races.TryGetValue(eventId, out _))
            {
                races.Remove(eventId);
            }
            
            races.Add(eventId, race);

            var jsonContent = JsonConvert.SerializeObject(races);

            await SaveFileAsync(jsonContent);
        }
    }
}