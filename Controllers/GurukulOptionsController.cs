using Microsoft.AspNetCore.Mvc;
using optionsReportApi.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

[Route("api/[controller]")]
[ApiController]
public class GurukulOptionsController : ControllerBase
{
    private readonly string filePath = "GurukulOptions.json";
    [HttpGet]
    [Route("Getjson")]
    public ActionResult<IEnumerable<GurukulOptionsData>> Get()
    {
        if (System.IO.File.Exists(filePath))
        {
            var json = System.IO.File.ReadAllText(filePath);
            GurukulOptionsData option = JsonSerializer.Deserialize<GurukulOptionsData>(json);

            return Ok(option);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost]
    [Route("Putjson")]
    public async Task<IActionResult> Post([FromBody] GurukulOptionsData newData)
    {
        GurukulOptionsData existingData = new GurukulOptionsData();
        if (System.IO.File.Exists(filePath))
        {
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                // Truncate the file by writing an empty string
                writer.Write(string.Empty);
            }
        }
        var updatedJson = JsonSerializer.Serialize(newData);
        await System.IO.File.WriteAllTextAsync(filePath, updatedJson);
        return Ok(updatedJson);
    }
}
