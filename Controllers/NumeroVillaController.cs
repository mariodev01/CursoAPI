using AutoMapper;
using CursoWebAPI.Data;
using CursoWebAPI.Models;
using CursoWebAPI.Models.DTO;
using CursoWebAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CursoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumeroVillaController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IVillaRepositorio _villa;
        private readonly INumeroVillaRepositorio _numeroRepo;
        protected APIResponse _response;
        

        public NumeroVillaController(ILogger<NumeroVillaController> logger,IMapper mapper, IVillaRepositorio villa, 
            INumeroVillaRepositorio numeroRepo)
        {
            _logger = logger;
            _mapper = mapper;
            _villa = villa;
            _numeroRepo = numeroRepo;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetNumeroVillas()
        {
            try
            {
                _logger.LogInformation("Obtener los numeros de las villas");

                IEnumerable<NumeroVilla> NumeroVillaList = await _numeroRepo.GetAll();

                _response.Resultado = _mapper.Map<IEnumerable<NumeroVillaDto>>(NumeroVillaList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("id",Name ="GetNumeroVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer Numero Villa con Id " + id);
                    _response.StatusCode=HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var Numerovilla = await _numeroRepo.Get(v => v.VillaNo == id);
                //var villa = VillaStore.villaDtos.FirstOrDefault(v => v.Id == id);
                if (Numerovilla == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<NumeroVillaDto>(Numerovilla);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response); 
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<APIResponse>> InsertNumeroVilla([FromBody] NumeroVillaCreateDto villaDto)
        {
            try
            {
                if(await _numeroRepo.Get(v=>v.VillaNo == villaDto.VillaNo) != null)
                {
                    ModelState.AddModelError("NombreExiste", "El Numero de Villa ya existe!");
                    return BadRequest(ModelState);
                }

                if(await _villa.GetAll(v=>v.Id == villaDto.VillaId) == null)
                {
                    ModelState.AddModelError("ClaveForanea", "El Id de la Villa no Existe!");
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

                NumeroVilla modelo = _mapper.Map<NumeroVilla>(villaDto);

                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;
                await _numeroRepo.Add(modelo);
                _response.Resultado= modelo;
                _response.StatusCode = HttpStatusCode.Created;
                
                return CreatedAtRoute("GetNumeroVilla", new {id = modelo.VillaNo}, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso= false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso= false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var Numerovilla = await _numeroRepo.Get(v => v.VillaNo == id);
                if (Numerovilla == null)
                {
                    _response.IsExitoso= false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _numeroRepo.Delete(Numerovilla);
                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso= false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateNumeroVilla(int id, [FromBody] NumeroVillaUpdateDto villaDto)
        {
            try
            {
                if(villaDto == null || id!= villaDto.VillaNo)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if(await _villa.Get(v => v.Id == villaDto.VillaId) == null)
                {
                    ModelState.AddModelError("ClaveForanea", "El Id de la Villa No existe!");
                    return BadRequest(ModelState);
                }

                NumeroVilla modelo = _mapper.Map<NumeroVilla>(villaDto);

                await _numeroRepo.Update(modelo);

                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso= false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return BadRequest(_response);
            }
        }
    }
}
