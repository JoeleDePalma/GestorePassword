namespace Libreria.DTOs.Versions
{
    /// <summary>
    /// Represents the data returned to the client of the latest app version zip
    /// </summary>
    public class UpdatePackageDTO
    {
        public required string FileName { get; set; }
        public required byte[] Content { get; set; }
    }
}
