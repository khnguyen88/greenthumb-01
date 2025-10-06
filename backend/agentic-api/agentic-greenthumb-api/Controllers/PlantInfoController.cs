using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgenticGreenthumbApi.Models;
using AgenticGreenthumbApi.Services;
using System.Net;

namespace AgenticGreenthumbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantInfoController : ControllerBase
    {
        private static PlantInfoService _plantInfoService;

        public PlantInfoController(PlantInfoService plantInfoService)
        {
            _plantInfoService = plantInfoService;
        }

        // GET: api/PlantInfo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlantInfoModel>>> GetPlantInfos()
        {
            return await _plantInfoService.GetPlantInfos();

        }

        // GET: api/PlantInfo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlantInfoModel>> GetPlantInfoModel(long id)
        {
            var plantInfoModel = await _plantInfoService.GetPlantInfoModel(id);

            if (plantInfoModel == null)
            {
                return NotFound();
            }

            return plantInfoModel;
        }

        // PUT: api/PlantInfo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlantInfoModel(long id, PlantInfoModel plantInfoModel)
        {
            if (id != plantInfoModel.Id)
            {
                return BadRequest();
            }

            HttpStatusCode httpStatusCode = await _plantInfoService.PutPlantInfoModel(id, plantInfoModel);

            switch (httpStatusCode)
            {
                case HttpStatusCode.NotFound:
                    return NotFound();
                case HttpStatusCode.BadRequest:
                    return BadRequest();
                default:
                    return NoContent();
            }
        }

        // POST: api/PlantInfo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PlantInfoModel>> PostPlantInfoModel(PlantInfoModel plantInfoModel)
        {
            PlantInfoModel postResult = await _plantInfoService.PostPlantInfoModel(plantInfoModel);

            return CreatedAtAction("GetPlantInfoModel", new { id = postResult.Id }, postResult);
        }

        // DELETE: api/PlantInfo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlantInfoModel(long id)
        {
            HttpStatusCode httpStatusCode = await _plantInfoService.DeletePlantInfoModel(id);

            switch (httpStatusCode)
            {
                case HttpStatusCode.NotFound:
                    return NotFound();
                default:
                    return NoContent();
            }
        }
    }
}
