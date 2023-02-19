using EmployeeApi2.Data;
using EmployeeApi2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeDbContext _context;

    public EmployeeController(EmployeeDbContext employeeDbContext) 
        => _context = employeeDbContext;
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<Employee>> Get() 
        => await _context.Employees.ToListAsync();

    [HttpGet("/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _context.Employees.FindAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetById", new { Id = employee.Id }, employee);
        // return Ok(employee);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Employee employee)
    {
        if (id == employee.Id) return BadRequest();

        _context.Entry(employee).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _context.Employees.FindAsync(id);
        if (result == null) return BadRequest(); 
        _context.Employees.Remove(result);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}