namespace CinderellaCore.Model.Models.Import
{
    public class ImportResponse
    {
        public bool Successful { get; set; }
        public int NumRequested { get; set; }
        public int Imported { get; set; }
        public int Failed { get; set; }
        public string Message { get; set; }
    }
}