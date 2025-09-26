using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgenticGreenthumbApi.Models;

namespace AgenticGreenthumbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantInfoController : ControllerBase
    {
        private readonly PlantInfoContext _context;

        public PlantInfoController(PlantInfoContext context)
        {
            _context = context;
        }

        // GET: api/PlantInfo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlantInfoModel>>> GetPlantInfos()
        {
            return await _context.PlantInfos.ToListAsync();
        }

        // GET: api/PlantInfo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlantInfoModel>> GetPlantInfoModel(long id)
        {
            var plantInfoModel = await _context.PlantInfos.FindAsync(id);

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

            _context.Entry(plantInfoModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlantInfoModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PlantInfo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PlantInfoModel>> PostPlantInfoModel(PlantInfoModel plantInfoModel)
        {
            _context.PlantInfos.Add(plantInfoModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlantInfoModel", new { id = plantInfoModel.Id }, plantInfoModel);
        }

        // DELETE: api/PlantInfo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlantInfoModel(long id)
        {
            var plantInfoModel = await _context.PlantInfos.FindAsync(id);
            if (plantInfoModel == null)
            {
                return NotFound();
            }

            _context.PlantInfos.Remove(plantInfoModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlantInfoModelExists(long id)
        {
            return _context.PlantInfos.Any(e => e.Id == id);
        }
    }
}
