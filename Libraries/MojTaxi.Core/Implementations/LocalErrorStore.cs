using MojTaxi.Core.Abstractions;
using MojTaxi.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MojTaxi.Core.Implementations
{
    public class LocalErrorStore : ILocalErrorStore
    {
        private readonly string _path =
            Path.Combine(FileSystem.AppDataDirectory, "errors.json");

        public async Task SaveAsync(ErrorEntry entry)
        {
            var list = await GetAllAsync();
            list.Add(entry);

            var json = JsonSerializer.Serialize(list);
            await File.WriteAllTextAsync(_path, json);
        }

        public async Task<List<ErrorEntry>> GetAllAsync()
        {
            if (!File.Exists(_path))
                return new();

            var json = await File.ReadAllTextAsync(_path);
            return JsonSerializer.Deserialize<List<ErrorEntry>>(json) ?? new();
        }

        public async Task DeleteAsync(Guid id)
        {
            var list = await GetAllAsync();
            list.RemoveAll(x => x.Id == id);

            var json = JsonSerializer.Serialize(list);
            await File.WriteAllTextAsync(_path, json);
        }
    }
}
