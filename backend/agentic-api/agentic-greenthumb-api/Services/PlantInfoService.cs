using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Models;
using AgenticGreenthumbApi.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AgenticGreenthumbApi.Services
{
    public class PlantInfoService
    {
        private static PlantInfoRepo _plantInfoRepo;

        public PlantInfoService(PlantInfoRepo plantInfoRepo) 
        {
            _plantInfoRepo = plantInfoRepo;
        }

        public async Task<ActionResult<IEnumerable<PlantInfoModel>>> GetPlantInfos()
        {
            return await _plantInfoRepo.GetPlantInfos();
        }

        public async Task<ActionResult<PlantInfoModel>?> GetPlantInfoModel(long id)
        {
            return await _plantInfoRepo.GetPlantInfoModel(id);
        }

        public async Task<HttpStatusCode> PutPlantInfoModel(long id, PlantInfoModel plantInfoModel)
        {
            return await _plantInfoRepo.PutPlantInfoModel(id, plantInfoModel);
        }

        public async Task<PlantInfoModel> PostPlantInfoModel(PlantInfoModel plantInfoModel)
        {
            return await _plantInfoRepo.PostPlantInfoModel(plantInfoModel);
        }

        public async Task<HttpStatusCode> DeletePlantInfoModel(long id)
        {
            return await _plantInfoRepo.DeletePlantInfoModel(id);
        }
    }
}
