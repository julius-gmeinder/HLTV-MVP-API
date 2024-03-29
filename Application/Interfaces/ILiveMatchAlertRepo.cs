﻿using HLTV_API.Domain.DTOs;

namespace HLTV_API.Application.Interfaces
{
    public interface ILiveMatchAlertRepo
    {
        public Task<List<string>> GetChannelsAsync();
        public Task SetupAsync(SetupLiveMatchAlertDTO setupDTO);
        public Task RemoveAsync(string GuildId);
    }
}
