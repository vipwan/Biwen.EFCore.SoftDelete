using Microsoft.EntityFrameworkCore;

namespace Biwen.EFCore.SoftDelete.TestConsole.Domains
{

    [PrimaryKey("Id")]
    public class Blog : ISoftDeleted
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public int AuthorId { get; set; }

        /// <summary>
        /// abc,bcd,efg
        /// </summary>
        public string Tags { get; set; } = null!;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;
    }
}