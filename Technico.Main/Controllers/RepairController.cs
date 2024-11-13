using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Technico.Main.DTOs;
using Technico.Main.DTOs.RepairDtos;
using Technico.Main.Models.Enums;
using Technico.Main.Services;
using Technico.Main.Services.Implementations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Technico.Main.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RepairController : ControllerBase
{
    private readonly IRepairService _repairService;

    public RepairController(IRepairService repairService)
    {
        _repairService = repairService;
    }

    // GET: api/<RepairController>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var repairsDto = await _repairService.GetAllAsync();

        if (repairsDto == null)
        {
            NotFound();
        }

        return Ok(repairsDto);
    }

    // GET api/<RepairController>/5
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var dto = await _repairService.GetAsync(id);

        if (dto == null) 
        {
            return NotFound("Not found repair with this id.");
        }

        return Ok(dto);
    }

    // POST api/<RepairController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PostRepairDto repairDto)
    {
        var createdDto = await _repairService.CreateAsync(repairDto);

        if (createdDto == null)
        {
            return NotFound("not found the property's id.");
        }

        return Ok(createdDto);
    }

    // PUT api/<RepairController>/5
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UpdateRepairDto dto)
    {
        var updatedDto = await _repairService.UpdateAsync(dto);

        if (updatedDto == null)
        {
            return NotFound("Could not find property or repair.");
        }

        return Ok(updatedDto);
    }

    // DELETE api/<RepairController>/5
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _repairService.DeleteAsync(id);

        if (success == false)
        {
            return NotFound("Not found repair with this id");
        }

        return Ok("successfully delete this repair.");
    }
    //Search Repairs

    [HttpGet, Route("search/{Vat}")]
    public async Task<IActionResult> Search(string Vat)
    {
        var repairs = await _repairService.SearchWithVatAsync(Vat);

        if (repairs == null || !repairs.Any())
        {
            NotFound();
        }

        return Ok(repairs);

    }

    [HttpGet, Route("search/active")]
    public async Task<IActionResult> SearchForActiveAsync()
    {
        var repairs = await _repairService.SearchForActiveAsync();

        if (repairs == null || !repairs.Any())
        {
            NotFound();
        }

        return Ok(repairs);

    }

    [HttpGet, Route("search/{CreatedAt:datetime}")]
    public async Task<IActionResult> SearchForDateAsync(DateTime CreatedAt)
    {
        var repairs = await _repairService.SearchForDateAsync(CreatedAt);

        if (repairs == null || !repairs.Any())
        {
            NotFound();
        }

        return Ok(repairs);

    }
}
