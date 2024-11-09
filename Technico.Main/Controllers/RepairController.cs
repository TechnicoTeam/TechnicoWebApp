using Microsoft.AspNetCore.Mvc;
using Technico.Main.DTOs;
using Technico.Main.DTOs.RepairDtos;
using Technico.Main.Services;

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
}
