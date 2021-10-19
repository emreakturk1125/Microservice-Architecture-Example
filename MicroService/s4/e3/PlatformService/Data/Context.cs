using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class Context : DbContext
    {
      public Context(DbContextOptions<Context> opt) : base(opt)
      {
          
      }

      public DbSet<Platform> Platforms {get; set;}

    }
}