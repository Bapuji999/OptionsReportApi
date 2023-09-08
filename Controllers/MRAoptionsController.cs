using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using optionsReportApi.Models;
using System.Text.Json;

namespace optionsReportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MRAoptionsController : ControllerBase
    {
        private readonly string filePath = "MRAOptions.json";

        private readonly string templatefilePath = "MRAteplates.json";
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
        [HttpGet]
        [Route("GetTemplate")]
        public ActionResult<IEnumerable<MRAteplates>> GetTemplate()
        {
            if (System.IO.File.Exists(templatefilePath))
            {
                var json = System.IO.File.ReadAllText(templatefilePath);
                List<MRAteplates> option = JsonSerializer.Deserialize<List<MRAteplates>>(json);

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
                    writer.Write(string.Empty);
                }
            }
            var updatedJson = JsonSerializer.Serialize(newData);
            await System.IO.File.WriteAllTextAsync(filePath, updatedJson);

            var json = System.IO.File.ReadAllText(filePath);
            MRAOptionsData option = JsonSerializer.Deserialize<MRAOptionsData>(json);
            
            string section1temp1 = string.Empty;
            if((bool)option.logo || (bool)option.bordLogo)
            {
                string decodedHtml = DecodeUnicodeEscapes(option.editorContent);
                string logoHtml = "<div> <img src='../assets/Image/mraLogo1.png' alt='ok' /> </div>";
                string schoolNameDesign = "<div style = 'justify-content: center; align-items: center; display: flex; margin-left: 20px;' >"+ decodedHtml + "</div>";
                string boardLogoHtml = "<div style='float: right; margin-right: 2px;'> <img src='../assets/Image/CBSE-Black.png' alt='ok' /></div>";
                if ((bool)option.logo && (bool)option.bordLogo)
                {
                    section1temp1 = logoHtml + schoolNameDesign + boardLogoHtml;
                }
                else {
                    if ((bool)option.logo)
                    {
                        section1temp1 = logoHtml + schoolNameDesign;
                    }
                    else
                    {
                        section1temp1 = schoolNameDesign + boardLogoHtml;
                    }
                }
            }
            else
            {
                string decodedHtml = DecodeUnicodeEscapes(option.editorContent);
                section1temp1 = "<div style='height: 100px; display: flex;'><div style='justify-content: center; align-items: center; display: flex; margin-left: 20px;'>"+ decodedHtml + "</div></div>";
            }
            string section1 = "<div style='display: flex; justify-content: space-between;'>" + section1temp1 + "</div>";

            string section2 = "<div style='font-weight: bold; font-size: 15px; border-color: black; border-width: 2px; border-style: solid; margin: 2px; text-align: center; padding: 5px;'>PROGRESS REPORT ("+option.termI+ ")</div><div style='font-weight: bold; font-size: 15px; border-color: black; border-width: 2px; border-style: solid; margin: 2px; text-align: center; padding: 5px;'>SESSION 2023-24</div>";

            string sectin3Temp1 = "<div style='display: inline-flex; margin-right: 2px; margin-top: 6px; min-width: 49%;'><div style='width: 150px; margin-right: 5px;'><div style='float: left;'>";
            string sectin3Temp2 = "</div><div style='float: right;'>:</div></div><div>";
            string sectin3Temp3 = "</div></div>";

            string section31 = sectin3Temp1 + "Name" + sectin3Temp2 + "StudentName" + sectin3Temp3;
            string section32 = sectin3Temp1 + "Class Section" + sectin3Temp2 + "VIII - A" + sectin3Temp3;
            string section33 = sectin3Temp1 + "Admission No." + sectin3Temp2 + "PR4858" + sectin3Temp3;
            string section34 = sectin3Temp1 + "Roll No." + sectin3Temp2 + "01" + sectin3Temp3;
            string section3Temp = section31 + section32 + section33 + section34;

            if ((bool)option.fatherName)
            {
                string section35 = sectin3Temp1 + "Father's Name" + sectin3Temp2 + "SACHIN KUMAR" + sectin3Temp3;
                section3Temp = section3Temp + section35;
            }
            if ((bool)option.dob)
            {
                string section36 = sectin3Temp1 + "Date of Birth" + sectin3Temp2 + "18-Oct-2009" + sectin3Temp3;
                section3Temp = section3Temp + section36;
            }
            if ((bool)option.motherName)
            {
                string section37 = sectin3Temp1 + "Mother's Name" + sectin3Temp2 + "POOJA" + sectin3Temp3;
                section3Temp = section3Temp + section37;
            }

            string sectio3 = "<div style='font-weight: bold; font-size: 15px; border-color: black; border-width: 2px; border-style: solid; margin: 2px; padding: 5px;'>"+ section3Temp + "</div>";

            string thL = "<th style='width: 33%; border: 1px solid black; border-collapse: collapse;'>";
            string thR = "</th>";

            string th1 = thL + "SUBJECT" + thR;
            string th2 = "<th style='width: 10%; border: 1px solid black; border-collapse: collapse;'>" + option.unitTest + "<br> (10)" + thR;
            string th3 = "<th style='width: 10%; border: 1px solid black; border-collapse: collapse;'>" + option.noteBook + "<br> (5)" + thR;
            string th4 = "<th style='width: 15%; border: 1px solid black; border-collapse: collapse;'>" + option.subjectEnrichment + "<br> (5)" + thR;
            string th5 = "<th style='width: 11%; border: 1px solid black; border-collapse: collapse;'>" + option.halfYearly + "<br> (80)" + thR;

            string AllTh = th1 + th2 + th3 + th4 + th5;
            if ((bool)option.totalPerSubject)
            {
                string th = "<th style='width: 11%; border: 1px solid black; border-collapse: collapse;'>" + "Total<br> (100)" + thR;
                AllTh = AllTh + th;
            }
            if ((bool)option.grade)
            {
                string th = "<th style='width: 10%; border: 1px solid black; border-collapse: collapse;'>" + "Grade" + thR;
                AllTh = AllTh + th;
            }

            string thead = "<thead>" + AllTh + "<thead>";

            string tdL1 = "<td style='width: 33%; border: 1px solid black; border-collapse: collapse; padding-left: 15px;'>";
            string td2 = "<td style='width: 10%; text-align: center; border: 1px solid black; border-collapse: collapse;'>0</td>";
            td2 = td2 + "<td style='width: 10%; text-align: center; border: 1px solid black; border-collapse: collapse;'>0</td>";
            td2 = td2 + "<td style='width: 15%; text-align: center; border: 1px solid black; border-collapse: collapse;'>0</td>";
            td2 = td2 + "<td style='width: 11%; text-align: center; border: 1px solid black; border-collapse: collapse;'>0</td>";
            string td3 = string.Empty;
            string td4 = string.Empty;

            if ((bool)option.totalPerSubject)
            {
                td3 = "<td style='width: 11%; text-align: center; border: 1px solid black; border-collapse: collapse;'>0</td>";
            }
            if ((bool)option.grade)
            {
                td4 = "<td style='width: 10%; text-align: center; border: 1px solid black; border-collapse: collapse;'>E</td></tr>";
            }

            string tr1 = "<tr>"+ tdL1 + option.english + "</td>" + td2 + td3 + td4;
            string tr2 = "<tr>" + tdL1 + option.hindi + "</td>" + td2 + td3 + td4;
            string tr3 = "<tr>" + tdL1 + option.punjabi + "</td>" + td2 + td3 + td4;
            string tr4 = "<tr>" + tdL1 + option.science + "</td>" + td2 + td3 + td4;
            string tr5 = "<tr>" + tdL1 + option.socialScience + "</td>" + td2 + td3 + td4;
            string tr6 = "<tr>" + tdL1 + option.sanskrit + "</td>" + td2 + td3 + td4;
            string tr7 = "<tr>" + tdL1 + option.computerScience + "</td>" + td2 + td3 + td4;
            string tr8 = "<tr>" + tdL1 + option.drawing + "</td>" + td2 + td3 + td4;
            string trAll = tr1 + tr2 + tr3 + tr4 + tr5 + tr6 + tr7 + tr8;

            if ((bool)option.overalTotal)
            {
                trAll = trAll + "<tr><td style='width: 33%;'></td>";
                if ((bool)option.totalPerSubject)
                {
                    trAll = trAll + "<td style='width: 10%;'></td>";
                }
                trAll = trAll + "<td style='width: 10%;'></td><td style='width: 15%;'></td><td style='width: 11%;'>TOTAL</td><td style='width: 11%; text-align: center; border-left: 1px solid black; border-collapse: collapse;'>0/800</td>";
                if ((bool)option.grade)
                {
                    trAll = trAll + "<td style='width: 10%;'></td>";
                }
                trAll = trAll + "</tr>";
            }
            if ((bool)option.overalPercentage)
            {
                trAll = trAll + "<tr style='border-top: 1px solid black; border-collapse: collapse;'><td style='width: 33%;'></td>";
                if ((bool)option.totalPerSubject)
                {
                    trAll = trAll + "<td style='width: 10%;'></td>";
                }
                trAll = trAll + "<td style='width: 10%;'></td><td style='width: 15%;'></td><td style='width: 11%;'>PERCENTAGE</td><td style='width: 11%; text-align: center; border-left: 1px solid black; border-collapse: collapse;'>0/800</td>";
                if ((bool)option.grade)
                {
                    trAll = trAll + "<td style='width: 10%;'></td>";
                }
                trAll = trAll + "</tr>";
            }

            string tBody = "<tbody>" + trAll + "</tbody>";

            string table1 = "<div style='font-weight: bold; font-size: 15px; padding: 3px;'><table style='width: 100%; border: 1px solid black; border-collapse: collapse;'>" + thead + tBody + "</table></div>";

            string tH1 = "<th style='width: 50%; border: 1px solid black; border-collapse: collapse; text-align: left; padding-left: 15px;'>CO-CURRICULAR ACTIVITIES</th>";
            string tH2 = "<th style='width: 50%; border: 1px solid black; border-collapse: collapse; text-align: center;'>" + option.termI + "</th>";
            string tHead2 = "<thead>"+ tH1 + tH2 +"</thead>";

            string tRL = "<tr><td style='width: 50%; border: 1px solid black; border-collapse: collapse; padding-left: 15px;'>";
            string tRR = "</td><td style='width: 50%; border: 1px solid black; border-collapse: collapse; padding-left: 15px;'></td></tr>";

            string tR1 = tRL + option.generalKnowledge + tRR;
            string tR2 = tRL + option.moralValues + tRR;
            string tR3 = tRL + option.workEducation + tRR;
            string tR4 = tRL + option.health + tRR;
            string tR5 = tRL + option.discipline + tRR;

            string tRAll = tR1 + tR2 + tR3 + tR4 + tR5;

            string tBoudy = "<tbody>" + tRAll + "</tbody>";

            string table2 = "<div style='font-weight: bold; font-size: 15px; padding: 3px;'><table style='width: 100%; border: 1px solid black; border-collapse: collapse;'>"+ tHead2+ tBoudy+ "</table></div>";

            string signatureSection = string.Empty;
            
            if((bool)option.parentSig || (bool)option.educatorSig)
            {
                string ConditionString = string.Empty;
                string princiPart = "<div style='float: right; text-align: center;'><div><img src='../assets/Image/Screenshot 2023-09-04 164413.png' alt='ok' /></div><div>" + option.principalDisplay + "</div></div>";
                if ((bool)option.parentSig)
                {
                    string parentPart = "<div style='float: left; text-align: center; margin-top: 110px;'><div>" + option.parentDisplay + "</div></div>";
                    ConditionString = ConditionString + parentPart;
                }
                if ((bool)option.educatorSig)
                {
                    string educatorPart = "<div style='float: left; text-align: center;'><div><img src='../assets/Image/Screenshot 2023-09-04 164413.png' alt='ok' /></div><div>" + option.educatorDisplay + "</div></div>";
                    ConditionString = ConditionString + educatorPart;
                }
                ConditionString = ConditionString + princiPart;
                signatureSection = "<div style='padding: 110px 40px 5px 40px; font-weight: bold; font-size: 15px; overflow: hidden; display: flex; justify-content: space-between;'>"+ ConditionString + "</div>";
            }
            else
            {
                signatureSection = "<div style='padding: 110px 40px 5px 40px; font-weight: bold; font-size: 15px; overflow: hidden;'><div style='float: right; text-align: center;'><div><img src='../assets/Image/Screenshot 2023-09-04 164413.png' alt='ok' /></div><div>" + option.principalDisplay + "</div></div></div>";
            }


            string templateHtml = "<div style='border-color: black; border-width: 2px; border-style: solid; width: 57%; margin: auto;  page-break-before: always;'>" +section1 + section2 + sectio3 + table1 + table2+ signatureSection+"</div>";


            if (System.IO.File.Exists(templatefilePath))
            {
                using (StreamWriter writer = new StreamWriter(templatefilePath, false))
                {
                    writer.Write("[]");
                }
            }
            var json1 = System.IO.File.ReadAllText(templatefilePath);

            List<MRAteplates> options = JsonSerializer.Deserialize<List<MRAteplates>>(json1);

            MRAteplates template = new MRAteplates();
            template.template = templateHtml;
            template.templateName = "Sample1";
            options.Add(template);

            
            var updatedJson1 = JsonSerializer.Serialize(options);
            await System.IO.File.WriteAllTextAsync(templatefilePath, updatedJson1);

            return Ok(updatedJson);
        }
        [HttpPost]
        [Route("PutTemplates")]
        public async Task<IActionResult> PutTemplates([FromBody] MRAteplates htmlContent)
        {
            try
            {
                var json = System.IO.File.ReadAllText(templatefilePath);

                List<MRAteplates> options = JsonSerializer.Deserialize<List<MRAteplates>>(json);

                MRAteplates template = new MRAteplates();
                template.template = htmlContent.template;
                template.templateName = htmlContent.templateName;
                options.Add(template);

                if (System.IO.File.Exists(templatefilePath))
                {
                    using (StreamWriter writer = new StreamWriter(templatefilePath, false))
                    {
                        writer.Write(string.Empty);
                    }
                }
                var updatedJson = JsonSerializer.Serialize(options);
                await System.IO.File.WriteAllTextAsync(templatefilePath, updatedJson);
                return Ok(updatedJson);
            }
            catch (JsonException ex)
            {
                throw;
            }
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
        private string DecodeUnicodeEscapes(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, @"\\u(\d{4})", m => ((char)int.Parse(m.Groups[1].Value, System.Globalization.NumberStyles.HexNumber)).ToString());
        }
    }
}
