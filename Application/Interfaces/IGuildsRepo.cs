﻿using HLTV_API.Domain.DTOs;
using HLTV_API.Domain.Models;

namespace HLTV_API.Application.Interfaces
{
    public interface IGuildsRepo
    {
        public Task<Guild?> GetGuildAsync(string GuildId);
        public Task AddAsync(string guildId);
        public Task RemoveAsync(string guildId);
    }
}