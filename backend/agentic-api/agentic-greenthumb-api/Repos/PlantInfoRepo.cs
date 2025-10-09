using AgenticGreenthumbApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;




namespace AgenticGreenthumbApi.Repos
{
    public class PlantInfoRepo
    {
        private readonly PlantInfoContext _context;

        public PlantInfoRepo(PlantInfoContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<PlantInfoModel>>> GetPlantInfos()
        {
            return await _context.PlantInfos.ToListAsync();
        }

        public async Task<ActionResult<PlantInfoModel>?> GetPlantInfoModel(long id)
        {
            var plantInfoModel = await _context.PlantInfos.FindAsync(id);

            if (plantInfoModel == null)
            {
                return null;
            }

            return plantInfoModel;
        }

        public async Task<IEnumerable<PlantInfoModel>> GetPlantInfoModelsByLike(string likeName)
        {
            var plantInfoModels = await _context.PlantInfos.Where(p => EF.Functions.Like(p.Name, $"%{likeName}%")).ToListAsync();

            return plantInfoModels;
        }

        public async Task<HttpStatusCode> PutPlantInfoModel(long id, PlantInfoModel plantInfoModel)
        {
            if (id != plantInfoModel.Id)
            {
                return HttpStatusCode.BadRequest;
            }

            _context.Entry(plantInfoModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlantInfoModelExists(id))
                {
                    return HttpStatusCode.NotFound;
                }
                else
                {
                    throw;
                }
            }

            return HttpStatusCode.NoContent;
        }

        public async Task<PlantInfoModel> PostPlantInfoModel(PlantInfoModel plantInfoModel)
        {
            _context.PlantInfos.Add(plantInfoModel);
            await _context.SaveChangesAsync();

            return await Task.FromResult(plantInfoModel);
        }

        public async Task<HttpStatusCode> DeletePlantInfoModel(long id)
        {
            var plantInfoModel = await _context.PlantInfos.FindAsync(id);
            if (plantInfoModel == null)
            {
                return HttpStatusCode.NotFound;
            }

            _context.PlantInfos.Remove(plantInfoModel);
            
            await _context.SaveChangesAsync();

            return HttpStatusCode.NoContent;
        }

        private bool PlantInfoModelExists(long id)
        {
            return _context.PlantInfos.Any(e => e.Id == id);
        }
    }
}
