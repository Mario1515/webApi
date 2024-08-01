using Microsoft.AspNetCore.Mvc;
using api.Models; 

[ApiController]
[Route("api/[controller]")]
public class SwiftController : ControllerBase
{
    private readonly SwiftMessageRepository _repository;
    private readonly ILogger<SwiftController> _logger;

    public SwiftController(SwiftMessageRepository repository, ILogger<SwiftController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

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

            // Save to database
            _repository.SaveSwiftMessage(swiftMessage);

            // Log each property of the Swift Message to Console and Log file
            _logger.LogInformation("Swift Message:");
            _logger.LogInformation("Header: {Header}", swiftMessage.Header);
            _logger.LogInformation("Application Header: {ApplicationHeader}", swiftMessage.ApplicationHeader);
            _logger.LogInformation("Text Body: {TextBody}", swiftMessage.TextBody);
            _logger.LogInformation("Trailer: {Trailer}", swiftMessage.Trailer);
            _logger.LogInformation("TrailerEnd: {TrailerEnd}", swiftMessage.TrailerEnd);

            return Ok("File processed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the file.");
            return StatusCode(500, "Internal server error");
        }
    }

    // Parsing logic
    private SwiftMessage ParseSwiftMessage(string message)
    {
        var parts = message.Split(new[] { '}' }, StringSplitOptions.RemoveEmptyEntries);
        var swiftMessage = new SwiftMessage();
        // Your parsing logic
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