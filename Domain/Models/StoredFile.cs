namespace AssetLocater.Domain.Models
{
    public class StoredFile
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string FileType { get; set; } = default!;
        public byte[] Content { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        public string CreatedAt { get; set; } = default!;
    }
}
