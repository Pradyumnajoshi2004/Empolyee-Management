using EmpolyeeManagement.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpolyeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationController : ControllerBase
    {
        public readonly AppDbContext _context;
        public DesignationController(AppDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDesignations()
        {
            try
            {
                var Results = await _context.designations.ToListAsync();
                return Ok(Results);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDesignation(int id)
        {
            try
            {
                var result = await _context.designations.FindAsync(id);
                if (result == null) {
                    return BadRequest("Designation not found");
                }
                return Ok(result);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostDesignation(Designation designation)
        {
            try
            {
                var existingDesignation = await _context.designations.AnyAsync(n => n.DesignationName == designation.DesignationName);
                if (existingDesignation)
                {
                    return BadRequest("Designation already exists");
                }
                 await _context.designations.AddAsync(designation);
                 await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetDesignation), new { id = designation.DesignationId }, designation);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> updateDesgination(int id, Designation designation)
        {
            try
            {
                var existingDesignation = await _context.designations
                    .FirstOrDefaultAsync(n => n.DesignationId == id);

                if (existingDesignation == null)
                    return NotFound("Designation not exists");

                // ✅ Update only if value is provided
                if (!string.IsNullOrEmpty(designation.DesignationName))
                {
                    existingDesignation.DesignationName = designation.DesignationName;
                }

                // ✅ Update DepartmentId only if it's not 0
                if (designation.DepartmentId != 0)
                {
                    existingDesignation.DepartmentId = designation.DepartmentId;
                }

                await _context.SaveChangesAsync();

                return Ok("Fields updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDesignation(int id) {
            try {
                var result = await _context.designations.FindAsync(id);
                if (result == null) return NotFound("Employee Not Found");
                else {
                    _context.designations.Remove(result);
                    _context.SaveChanges();
                    return Ok(result);
                }
                

            } catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
