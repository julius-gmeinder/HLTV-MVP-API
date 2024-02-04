using HLTV_API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace HLTV_API.Infrastructure
{
    public class HltvContext : DbContext
    {
        public HltvContext(DbContextOptions<HltvContext> options) : base(options)
        {

        }

        public virtual DbSet<Guild> Guilds { get; set; } = null!;
    }
}
