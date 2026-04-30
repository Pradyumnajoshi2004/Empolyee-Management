using EmpolyeeManagement.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpolyeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DepartmentController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET ALL
        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {
                var departments = await _context.Departments.ToListAsync();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            try
            {
                var department = await _context.Departments.FindAsync(id);

                if (department == null)
                    return NotFound("Department not found");

                return Ok(department);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ CREATE
        [HttpPost]
        public async Task<IActionResult> CreateDepartment(Department department)
        {
            try
            {
                await _context.Departments.AddAsync(department);
                await _context.SaveChangesAsync();

                return Ok(department);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, Department department)
        {
            try
            {
                var existing = await _context.Departments
                    .FirstOrDefaultAsync(d => d.DepartmentId == id);

                if (existing == null)
                    return NotFound("Department not found");

                existing.DepartmentName = department.DepartmentName;
                existing.IsActive = department.IsActive;

                await _context.SaveChangesAsync();

                return Ok("Updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var existing = await _context.Departments.FindAsync(id);

                if (existing == null)
                    return NotFound("Department not found");

                // 🔥 Check if any Designation is using this department
                var isUsed = await _context.designations
                    .AnyAsync(d => d.DepartmentId == id);

                if (isUsed)
                    return BadRequest("Cannot delete. Department is assigned to designations.");

                _context.Departments.Remove(existing);
                await _context.SaveChangesAsync();

                return Ok("Deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}