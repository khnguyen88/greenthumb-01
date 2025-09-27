using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Models;
using AgenticGreenthumbApi.Repos;
using Microsoft.AspNetCore.Mvc;

namespace AgenticGreenthumbApi.Services
{
    public class PlantInfoService
    {
        private static PlantInfoRepo _plantInfoRepo;

        public PlantInfoService() 
        { 
        }
        public async Task<ActionResult<IEnumerable<Plan>>> GetPlantInfos()
        {
            
        }
    }
}
