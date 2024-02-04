using System.ComponentModel.DataAnnotations;

namespace HLTV_API.Domain.Models
{
    public class Guild
    {
        [Key, Required]
        public string GuildId { get; set; } = null!;

        public string? LiveMatchAlertChannelId { get; set; } = null;
    }
}
