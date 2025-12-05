using MojTaxi.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MojTaxi.Core.Abstractions
{
    public interface ILocalErrorStore
    {
        Task SaveAsync(ErrorEntry entry);
        Task<List<ErrorEntry>> GetAllAsync();
        Task DeleteAsync(Guid id);
    }
}
