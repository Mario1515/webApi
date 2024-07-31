using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.IO;
using Serilog;

namespace api.Controller{ 
public class SwiftMessage
{
    public string Header { get; set; }
    public string ApplicationHeader { get; set; }
    public string TextBody { get; set; }
    public string Trailer { get; set; }
    public string TrailerEnd { get; set; }
}


[ApiController]
[Route("[controller]")]
public class SwiftController : ControllerBase
{
[HttpPost("upload")]
public IActionResult UploadSwiftMessage(IFormFile file)
{
    if (file == null || file.Length == 0)
        return BadRequest("No file uploaded");

    try
    {
        using var reader = new StreamReader(file.OpenReadStream());
        var message = reader.ReadToEnd();

        // Parse the SWIFT MT799 message  
        var swiftMessage = ParseSwiftMessage(message);

         // Loggin each propery of the Swift Message to Console and Log file
                Log.Information("Swift Message:");
                Log.Information("Header: {Header}", swiftMessage.Header);
                Log.Information("Application Header: {ApplicationHeader}", swiftMessage.ApplicationHeader);
                Log.Information("Text Body: {TextBody}", swiftMessage.TextBody);
                Log.Information("Trailer: {Trailer}", swiftMessage.Trailer);
                Log.Information("TrailerEnd: {TrailerEnd}", swiftMessage.TrailerEnd);

        return Ok("File processed successfully");
    }
    catch (Exception ex)
    {

     Log.Error(ex, "An error occurred while processing the file.");
        return StatusCode(500, "Internal server error");
         }
    }

    //parsing the message
    private SwiftMessage ParseSwiftMessage(string message){
        var parts = message.Split(new[] { '}' }, StringSplitOptions.RemoveEmptyEntries);
        var swiftMessage = new SwiftMessage();
        //my parsing logic 
            foreach (var part in parts)
            {
                var cleanPart = part.TrimStart('{').Trim();

                if (cleanPart.StartsWith("1:"))
                {
                    swiftMessage.Header = cleanPart.Substring(2).Trim();
                }
                else if (cleanPart.StartsWith("2:"))
                {
                    swiftMessage.ApplicationHeader = cleanPart.Substring(2).Trim();
                }
                else if (cleanPart.StartsWith("4:"))
                {
                    swiftMessage.TextBody = cleanPart.Substring(2).Trim();
                }
                else if (cleanPart.StartsWith("5:"))
                {
                    swiftMessage.Trailer = cleanPart.Substring(3).Trim();
                }
                else if (cleanPart.StartsWith("CHK:"))
                {
                    swiftMessage.TrailerEnd = cleanPart.Trim();
                }
            }

            return swiftMessage;
        }
    }       
}