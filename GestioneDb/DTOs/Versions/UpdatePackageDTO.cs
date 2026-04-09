namespace GestioneDb.DTOs.Versions
{
    /// <summary>
    /// Represents the data returned to the client of the latest app version zip
    /// </summary>
    public class UpdatePackageDTO
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }
}
