using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.IO;

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
         // Print the parsed header to the console

        // Save to SQLite
        // SaveToDatabase(swiftMessage);

        return Ok("File processed successfully");
    }
    catch (Exception ex)
    {
        // Log error to console (or replace with desired logging mechanism)
        Console.Error.WriteLine(ex);
        return StatusCode(500, "Internal server error");
     }
    }
        private string ParseSwiftMessage(string message)
    {
        var lines = message.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var messageHeader = lines.FirstOrDefault();

        return messageHeader ?? "No header found in the message.";
    }
}