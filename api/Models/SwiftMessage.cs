namespace api.Models 
{
    public class SwiftMessage
    {
        public int ?Id { get; set; } 
        public string ?Header { get; set; }
        public string ?ApplicationHeader { get; set; }
        public string ?TextBody { get; set; }
        public string ?Trailer { get; set; }
        public string ?TrailerEnd { get; set; }
    }
}