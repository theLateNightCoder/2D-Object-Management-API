using individueelProject.Repository.Environment2DRepo;
using individueelProject.Repository.Models;
using individueelProject.Repository.Object2DRepo;
using individueelProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


//Deze controller beheert 2D objecten
//Gebruikers kunnen objecten bekijken, ophalen en aanmaken binnen een specifieke omgeving.

//Functies:

//Alle objecten ophalen binnen een omgeving
//Een specifiek object ophalen op ID
//Een nieuw object aanmaken in een omgeving

//Belangrijke regels:

//Alleen ingelogde gebruikers hebben toegang
//Elke actie is gekoppeld aan een specifieke omgeving
//Een object hoort altijd bij een bestaande omgeving
//Gebruikers kunnen alleen hun eigen omgevingen gebruiken

namespace individueelProject.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class Object2DController : ControllerBase
{
    private readonly IObjectRepository _repository;
    private readonly IEnivronmentRepository _enivronmentRepository;
    private readonly IAuthenticationService _services;
    public Object2DController(IObjectRepository repository, IAuthenticationService services, IEnivronmentRepository enivronmentRepository)
    {
        _repository = repository;
        _services = services;
        _enivronmentRepository = enivronmentRepository;
    }

 
    [HttpGet("all/{environmentName}")]
    public async Task<ActionResult<IEnumerable<Object2D>>> GetByEnvironmentId(string environmentName) { 

        string? userId = _services.GetCurrentAuthenticatedUserId();

        if (userId == null)
            return BadRequest("User Id can't be null! ");

        var environmentId = await _enivronmentRepository.GetEnvironmentIdAsync(environmentName, userId);
 

        var objects = await _repository.GetByEnvironmentIdAsync(environmentId);
       
        return Ok(objects);
    }

 
    [HttpGet("{environmentName}/{id}")]
    public async Task<ActionResult<Object2D>> GetById(Guid id , string environmentName)
    {
        string? userId = _services.GetCurrentAuthenticatedUserId();

        if (userId == null)
            return BadRequest("User Id can't be null! ");

        var environmentId = await _enivronmentRepository.GetEnvironmentIdAsync(environmentName, userId);

        var obj = await _repository.GetByIdAsync(id , environmentId);

        if (obj == null)
            return NotFound($"Object with ID {id} not found.");

        return Ok(obj);
    }

 
    [HttpPost]
    public async Task<ActionResult> Create(Object2DDTO object2DDTO )
    {
        string? userId = _services.GetCurrentAuthenticatedUserId();

        if (userId == null)
            return BadRequest("User Id can't be null! ");

        var environmentId = await _enivronmentRepository.GetEnvironmentIdAsync(object2DDTO.EnvironmentName, userId);

        Object2D object2D = new Object2D()
        {
            Id = Guid.NewGuid(),
            EnvironmentId = environmentId,
            PrefabId = object2DDTO.PrefabId,
            PostionX = object2DDTO.PostionX,
            PostionY = object2DDTO.PostionY,
            ScaleX = object2DDTO.ScaleX,
            ScaleY = object2DDTO.ScaleY,
            RotationZ = object2DDTO.RotationZ,
            SortingLayer = object2DDTO.SortingLayer
        };

       
        await _repository.AddAsync(object2D);
        return CreatedAtAction(nameof(GetById), new { id = object2D.Id , environmentName = object2DDTO.EnvironmentName }, object2D);
    }


}