using System;

namespace TrainzInfoShared.DTO
{
    public class CommentsDTO
    {
        public int Id { get; set; }

        public string Comment { get; set; } = default!;

        public DateTime DateTime { get; set; }

        // If you need to keep the navigation objects inside the DTO,
        // you can expose them as read‑only properties or nested DTOs.
        // For a simple API payload we usually only send FK identifiers.

        /// <summary>
        /// Id of the related NewsInfo entity.
        /// </summary>
        public string? NewsName { get; set; }
        public int? NewsID { get; set; }

        /// <summary>
        /// Id of the user (IdentityUser) who authored the comment.
        /// </summary>
        public string? AuthorEmail { get; set; }
        public string? Authorname { get; set; }
    }
}
