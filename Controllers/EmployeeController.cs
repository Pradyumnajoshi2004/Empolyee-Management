using EmpolyeeManagement.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpolyeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;
        public EmployeeController(AppDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var result = await _context.Employees.ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            try
            {
                var result = await _context.Employees.FindAsync(id);
                if (result == null)
                {
                    return NotFound("Employee Not Found");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostEmployee(Employee employee) {
            try
            {
                var emailExists = await _context.Employees.AnyAsync(e => e.Email == employee.Email);
                if (emailExists) {
                    return BadRequest("Employee Is Already Exists");
                }

                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);

            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }

        }
    


    [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee ) {
            try
            {
                var existing = await _context.Employees.FindAsync(id);

                if (existing == null)  return NotFound("Employee Not Found"); 

                existing.EmployeeName = employee.EmployeeName;
                existing.Email = employee.Email;
                existing.DesignationId = employee.DesignationId;

                await _context.SaveChangesAsync();
                return Ok(existing);

            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id) {
            try
            {
                var result = await _context.Employees.FindAsync(id);
                if (result == null) return NotFound("Employee Not Found");

                _context.Employees.Remove(result);
                _context.SaveChanges();
                return Ok(result);

            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }

        }

    }
}
