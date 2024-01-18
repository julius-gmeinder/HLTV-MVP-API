namespace HLTV_API.Domain.Models
{
    public class Match
    {
        public string Length { get; set; } = null!;

        public string TeamOne { get; set; } = null!;
        public int TeamOneScore { get; set; }

        public string TeamTwo { get; set; } = null!;
        public int TeamTwoScore { get; set; }
    }
}
