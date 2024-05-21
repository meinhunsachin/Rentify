// Controllers/PropertiesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PropertiesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProperties([FromQuery] PropertyFilterModel filter)
    {
        var query = _context.Properties.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Place))
        {
            query = query.Where(p => p.Place.Contains(filter.Place));
        }
        if (filter.MinBedrooms.HasValue)
        {
            query = query.Where(p => p.Bedrooms >= filter.MinBedrooms.Value);
        }
        if (filter.MaxBedrooms.HasValue)
        {
            query = query.Where(p => p.Bedrooms <= filter.MaxBedrooms.Value);
        }

        var properties = await query.ToListAsync();
        return Ok(properties);
    }

    [HttpPost]
    public async Task<IActionResult> PostProperty([FromBody] Property property)
    {
        _context.Properties.Add(property);
        await _context.SaveChangesAsync();
        return Ok(property);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProperty(int id, [FromBody] Property property)
    {
        var existingProperty = await _context.Properties.FindAsync(id);
        if (existingProperty == null)
        {
            return NotFound();
        }

        existingProperty.Place = property.Place;
        existingProperty.Area = property.Area;
        existingProperty.Bedrooms = property.Bedrooms;
        existingProperty.Bathrooms = property.Bathrooms;
        existingProperty.NearbyHospitals = property.NearbyHospitals;
        existingProperty.NearbyColleges = property.NearbyColleges;

        await _context.SaveChangesAsync();
        return Ok(existingProperty);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProperty(int id)
    {
        var property = await _context.Properties.FindAsync(id);
        if (property == null)
        {
            return NotFound();
        }

        _context.Properties.Remove(property);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("{id}/interest")]
    public async Task<IActionResult> ExpressInterest(int id, [FromBody] InterestedProperty interest)
    {
        var property = await _context.Properties.FindAsync(id);
        if (property == null)
        {
            return NotFound();
        }

        interest.PropertyId = id;
        _context.InterestedProperties.Add(interest);
        await _context.SaveChangesAsync();

        var seller = await _context.Users.FindAsync(property.SellerId);
        return Ok(new { seller.FirstName, seller.LastName, seller.Email, seller.PhoneNumber });
    }
}