using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using optionsReportApi.Models;
using System.Text.Json;

namespace optionsReportApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MRAoptionsController : ControllerBase
    {
        private readonly string filePath = "MRAOptions.json";
        [HttpGet]
        [Route("Getjson")]
        public ActionResult<IEnumerable<MRAOptionsData>> Get()
        {
            if (System.IO.File.Exists(filePath))
            {
                var json = System.IO.File.ReadAllText(filePath);
                MRAOptionsData option = JsonSerializer.Deserialize<MRAOptionsData>(json);

                return Ok(option);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        [Route("Putjson")]
        public async Task<IActionResult> Post([FromBody] MRAOptionsData newData)
        {
            MRAOptionsData existingData = new MRAOptionsData();
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
        [HttpPost]
        [Route("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationtoken)
        {
            try
            {
                var result = await WriteFile(file);
                return Ok(new { success = true, message = "File uploaded successfully", data = result });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filename);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contenttype))
            {
                contenttype = "application/octet-stream";
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, Path.GetFileName(filepath));
        }
        private async Task<string> WriteFile(IFormFile file)
        {
            string filename = "";
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                filename = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return filename;

            }
            catch (Exception ex)
            {
            }
            return filename;
        }
    }
}
