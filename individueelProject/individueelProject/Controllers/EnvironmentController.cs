using individueelProject.Repository.Environment2DRepo;
using individueelProject.Repository.Models;
using individueelProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;



//Deze controller beheert 2D omgevingen voor ingelogde gebruikers.
//Gebruikers kunnen hun omgevingen aanmaken, ophalen en verwijderen.

//Functies:

//Alle omgevingen van de ingelogde gebruiker ophalen
//Een specifieke omgeving ophalen op naam
//Een nieuwe omgeving aanmaken met validatie regels
//Een omgeving verwijderen op naam

//Belangrijke regels:

//Elke gebruiker mag maximaal 5 omgevingen hebben
//De naam van een omgeving moet tussen 1 en 25 tekens zijn
//De naam van een omgeving moet uniek zijn per gebruiker
//Alle endpoints vereisen authenticatie


namespace individueelProject.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class EnvironmentController : ControllerBase
{
    private readonly IEnivronmentRepository _repository;
    private readonly IAuthenticationService _services;

    public EnvironmentController(IEnivronmentRepository repository, IAuthenticationService services)
    {
        _repository = repository;
        _services = services;
    }

    [HttpGet("all")]
 
    public async Task<ActionResult<IEnumerable<Environment2D>>> GetByUserId()
    {

        string? userId = _services.GetCurrentAuthenticatedUserId();

        if (userId == null)
            return BadRequest("User Id can't be null! ");

        var environments = await _repository.GetByUserIdAsync(userId);
        if (environments == null || !environments.Any())
        {
            return Ok("No environments found for the given user");
        }

        return Ok(environments);
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<Environment2D>> GetById(string name)
    {

        string? userId = _services.GetCurrentAuthenticatedUserId();

        if (userId == null)
            return BadRequest("User Id can't be null! ");

        var environmentId = await _repository.GetEnvironmentIdAsync(name, userId);

        if (environmentId == Guid.Empty)
            return NotFound();

        var environment = await _repository.GetByIdAsync(environmentId, userId);


        return Ok(environment);
    }

    [HttpPost]
    public async Task<ActionResult> Create(Environment2DDTO environment)
    {
        if (string.IsNullOrWhiteSpace(environment.Name) || environment.Name.Length < 1 || environment.Name.Length > 25)
        {
            return BadRequest("Environment name must be between 1 and 25 characters");
        }

        string? userId = _services.GetCurrentAuthenticatedUserId();

        if (userId == null)
            return BadRequest("User Id can't be null! ");


        var existingEnvironment = await _repository.GetByUserAndNameAsync(userId , environment.Name);
        if (existingEnvironment != null)
        {
            return Conflict("Environment name already exists");
        }

        var userEnvironmentsCount = await _repository.CountByUserAsync(userId);
        if (userEnvironmentsCount >= 5)
        {
            return BadRequest("A user cannot have more than 5 environments");
        }

        Environment2D newEnvironment = new Environment2D {
            Id = Guid.NewGuid(),
            Name = environment.Name,
            OwnerUserId = userId.ToString(),
            MaxLength = environment.MaxLength,
            MaxHeight = environment.MaxHeight

        };

        await _repository.AddAsync(newEnvironment);
        return Ok();
    }

   

    [HttpDelete("{name}")]
    public async Task<ActionResult> Delete(string name)
    {
        string? userId = _services.GetCurrentAuthenticatedUserId();

        if (userId == null)
            return BadRequest("User Id can't be null! ");


        var environment = await _repository.GetEnvironmentIdAsync(name , userId);
        if (environment == Guid.Empty)
            return NotFound();

        await _repository.DeleteAsync(environment, userId);
        return NoContent();
    }
}
