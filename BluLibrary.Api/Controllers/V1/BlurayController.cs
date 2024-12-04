using Microsoft.AspNetCore.Mvc;
using BluLibrary.Core.DTOs.Requests;
using BluLibrary.Core.Entities;
using BluLibrary.Core.Interfaces.Repositories;
using BluLibrary.Core.Exceptions;

namespace BluLibrary.Api.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class BlurayController : ControllerBase
{
    private readonly IBlurayRepository _blurayRepository;
    private readonly ILogger<BlurayController> _logger;

    public BlurayController(
        IBlurayRepository blurayRepository,
        ILogger<BlurayController> logger)
    {
        _blurayRepository = blurayRepository;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BlurayResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var blurays = await _blurayRepository.GetAllAsync();
        var response = blurays.Select(MapToResponseDto);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BlurayResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var bluray = await _blurayRepository.GetByIdAsync(id);
        if (bluray == null)
            return NotFound();

        return Ok(MapToResponseDto(bluray));
    }

    [HttpPost]
    [ProducesResponseType(typeof(BlurayResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateBlurayDto dto)
    {
        try
        {
            // Vérifier si l'ISBN existe déjà
            if (await _blurayRepository.IsbnExistsAsync(dto.ISBN))
                return BadRequest($"A Bluray with ISBN {dto.ISBN} already exists");

            var bluray = Bluray.Create(
                dto.Title,
                dto.Director,
                dto.ISBN,
                dto.ReleaseYear,
                dto.Genre,
                dto.DurationMinutes
            );

            await _blurayRepository.AddAsync(bluray);

            // Retourner le DTO avec l'URL de la nouvelle ressource
            return CreatedAtAction(
                nameof(GetById),
                new { id = bluray.Id },
                MapToResponseDto(bluray)
            );
        }
        catch (DomainValidationException ex)
        {
            _logger.LogWarning(ex, "Validation failed when creating Bluray");
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBlurayDto dto)
    {
        try
        {
            var bluray = await _blurayRepository.GetByIdAsync(id);
            if (bluray == null)
                return NotFound();

            bluray.Update(
                dto.Title,
                dto.Director,
                dto.Genre,
                dto.DurationMinutes,
                dto.Description,
                dto.Studio
            );

            await _blurayRepository.UpdateAsync(bluray);
            return NoContent();
        }
        catch (DomainValidationException ex)
        {
            _logger.LogWarning(ex, "Validation failed when updating Bluray {Id}", id);
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!await _blurayRepository.ExistsAsync(id))
            return NotFound();

        await _blurayRepository.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<BlurayResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] string term)
    {
        var blurays = await _blurayRepository.SearchAsync(term);
        var response = blurays.Select(MapToResponseDto);
        return Ok(response);
    }

    private static BlurayResponseDto MapToResponseDto(Bluray bluray)
    {
        return new BlurayResponseDto
        {
            Id = bluray.Id,
            Title = bluray.Title,
            Director = bluray.Director,
            ISBN = bluray.ISBN,
            ReleaseYear = bluray.ReleaseYear,
            Description = bluray.Description,
            Genre = bluray.Genre,
            DurationMinutes = bluray.DurationMinutes,
            Studio = bluray.Studio,
            DateAdded = bluray.DateAdded,
            LastModified = bluray.LastModified
        };
    }
}