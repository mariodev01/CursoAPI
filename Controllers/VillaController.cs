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

        public VillaController(ILogger<VillaController>logger, ApplicationDbcontex db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            try
            {
                _logger.LogInformation("Obtener las villas");

                var villas = _db.Villas.ToList();

                return Ok(villas);
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
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest("Error!, no Ingrese el id cero");
            }
            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);
            //var villa = VillaStore.villaDtos.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                _logger.LogError("No se encontro la villa con ese id " + id);
                return NotFound("No se encontro la villa con el id " + id);
            }
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult<VillaDto> InsertVilla([FromBody] VillaDto villaDto)
        {
            try
            {
                if(_db.Villas.FirstOrDefault(v=>v.Nombre.ToLower()== villaDto.Nombre.ToLower()) != null)
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

                if (villaDto.Id > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                Villa modelo = new()
                {
                    
                    Nombre = villaDto.Nombre,
                    Detalle = villaDto.Detalle,
                    Ocupantes = villaDto.Ocupantes,
                    MetrosCuadrados = villaDto.MetrosCuadrados,
                    Tarifa = villaDto.Tarifa,
                    Amenidad = villaDto.Amenidad,
                };

                _db.Villas.Add(modelo);
                _db.SaveChanges();

                //return Ok(villaDto);
                return CreatedAtRoute("GetVilla", new {id = villaDto.Id},villaDto);
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
        public IActionResult DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);
            if(villa == null)
            {
                return NotFound();
            }

            _db.Villas.Remove(villa); 
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto)
        {
            try
            {
                if(villaDto==null || villaDto.Id == 0)
                {
                    return BadRequest();
                }

                Villa modelo = new()
                {
                    Id = villaDto.Id,
                    Nombre = villaDto.Nombre,
                    Detalle = villaDto.Detalle,
                    ImagenUrl = villaDto.ImagenUrl,
                    Ocupantes = villaDto.Ocupantes,
                    Tarifa = villaDto.Tarifa,
                    MetrosCuadrados = villaDto.MetrosCuadrados,
                    Amenidad = villaDto.Amenidad
                };

                _db.Villas.Update(modelo);
                _db.SaveChanges();

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
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchDto)
        {
            try
            {
                if (patchDto == null || id == 0)
                {
                    return BadRequest();
                }

                var villa = _db.Villas.AsNoTracking().FirstOrDefault(v=>v.Id==id);

                VillaDto villaDto = new()
                {
                    Id = villa.Id,
                    Nombre = villa.Nombre,
                    Detalle = villa.Detalle,
                    ImagenUrl = villa.ImagenUrl,
                    Ocupantes = villa.Ocupantes,
                    Tarifa = villa.Tarifa,
                    MetrosCuadrados = villa.MetrosCuadrados,
                    Amenidad = villa.Amenidad
                };

                if (villa == null)
                {
                    return BadRequest();
                }

                patchDto.ApplyTo(villaDto, ModelState);

                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Villa modelo = new()
                {
                    Id = villaDto.Id,
                    Nombre = villaDto.Nombre,
                    Detalle = villaDto.Detalle,
                    ImagenUrl = villaDto.ImagenUrl,
                    Ocupantes = villaDto.Ocupantes,
                    Tarifa = villaDto.Tarifa,
                    MetrosCuadrados = villaDto.MetrosCuadrados,
                    Amenidad = villaDto.Amenidad
                };

                _db.Villas.Update(modelo);
                _db.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
