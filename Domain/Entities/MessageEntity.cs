using Domain.Entities.Base;
using System.ComponentModel;

namespace Domain.Entities
{
    public class MessageEntity : BaseEntity
    {
        /// <summary>
        /// Message content
        /// </summary>
        [Description("Message content")]
        public required string Content { get; set; }

        /// <summary>
        /// Time of writing to the database
        /// </summary>
        [Description("Time of writing to the database")]
        public DateTime SavedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Time of sending by the user
        /// </summary>
        [Description("Time of sending by the user")]
        public required DateTimeOffset SentAt { get; set; }
    }
}
