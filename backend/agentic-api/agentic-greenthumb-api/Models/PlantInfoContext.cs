using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

namespace AgenticGreenthumbApi.Models
{
    public class PlantInfoContext : DbContext
    {
        public PlantInfoContext(DbContextOptions<PlantInfoContext> options) 
            : base(options) 
        { 
        }

        public DbSet<PlantInfoModel> PlantInfos { get; set; } = null;
    }
}
