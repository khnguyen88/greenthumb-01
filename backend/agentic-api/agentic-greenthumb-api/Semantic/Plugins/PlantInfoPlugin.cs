using System.ComponentModel;
using System.Text.Json;
using AgenticGreenthumbApi.Models;
using AgenticGreenthumbApi.Services;
using Microsoft.SemanticKernel;

namespace AgenticGreenthumbApi.Semantic.Plugins
{
    public class PlantInfoPlugin
    {
        private readonly PlantInfoService _plantInfoService;

        public PlantInfoPlugin(PlantInfoService plantInfoService) { 
            _plantInfoService = plantInfoService;
        }

        [KernelFunction(nameof(SearchPlantInfosFromDatabase))]
        [Description("Searches for a list of plant information that fit the name provided")]
        [return: Description("Returns a list of PlantInfoModel objects that fit the name of the plant")] //Optional
        public List<PlantInfoModel> SearchPlantInfosFromDatabase(string likeName)
        {
            var results =  _plantInfoService.GetPlantInfoModelsByLike(likeName).Result.ToList();

            return results;
        }

        [KernelFunction(nameof(GeneratePlantInfo))]
        [Description("Searches for a list of plant information that fit the plant name provided")]
        [return: Description("Returns a list of PlantInfoModel objects that fit the name of the plant")] //Optional
        public string GeneratePlantInfo(string nameOrDescription)
        {
            var modelTemplate = JsonSerializer.Serialize(new PlantInfoModel());

            return $"Give me plant information based on this template object format: {modelTemplate}. You can provide a short description on the plant afterwards.";
        }
    }
}
