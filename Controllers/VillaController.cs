using AutoMapper;
using CursoWebAPI.Data;
using CursoWebAPI.Models;
using CursoWebAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CursoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbcontex _db;
        private readonly IMapper _mapper;

        public VillaController(ILogger<VillaController>logger, ApplicationDbcontex db, IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            try
            {
                _logger.LogInformation("Obtener las villas");

                IEnumerable<Villa> villas = await _db.Villas.ToListAsync();

                return Ok(_mapper.Map<IEnumerable<VillaDto>>(villas));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            
        }

        [HttpGet("id",Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest("Error!, no Ingrese el id cero");
            }
            var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
            //var villa = VillaStore.villaDtos.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                _logger.LogError("No se encontro la villa con ese id " + id);
                return NotFound("No se encontro la villa con el id " + id);
            }
            return Ok(_mapper.Map<VillaDto>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<VillaDto>> InsertVilla([FromBody] VillaCreateDto villaDto)
        {
            try
            {
                if(await _db.Villas.FirstOrDefaultAsync(v=>v.Nombre.ToLower()== villaDto.Nombre.ToLower()) != null)
                {
                    ModelState.AddModelError("NombreExiste", "La Villa con ese nombre ya existe!");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (villaDto == null)
                {
                    return BadRequest("Los datos no pueden estar vacios!");
                }

                Villa modelo = _mapper.Map<Villa>(villaDto);

                await _db.Villas.AddAsync(modelo);
                await _db.SaveChangesAsync();

                //return Ok(villaDto);
                return CreatedAtRoute("GetVilla", new {id = modelo.Id}, modelo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
            if(villa == null)
            {
                return NotFound();
            }

            _db.Villas.Remove(villa); 
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto villaDto)
        {
            try
            {
                if(villaDto==null || villaDto.Id == 0)
                {
                    return BadRequest();
                }

                Villa modelo = _mapper.Map<Villa>(villaDto);

                _db.Villas.Update(modelo);
                await _db.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            try
            {
                if (patchDto == null || id == 0)
                {
                    return BadRequest();
                }

                var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(v=>v.Id==id);

                VillaUpdateDto updateDto = _mapper.Map<VillaUpdateDto>(villa);

                if (villa == null)
                {
                    return BadRequest();
                }

                patchDto.ApplyTo(updateDto, ModelState);

                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Villa modelo = _mapper.Map<Villa>(updateDto);

                _db.Villas.Update(modelo);
                await _db.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
