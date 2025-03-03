using System.ComponentModel;

namespace Domain.Models
{
    public class MessageModel
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
        public DateTimeOffset SavedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Time of sending by the user
        /// </summary>
        [Description("Time of sending by the user")]
        public required DateTimeOffset SentAt { get; set; }
    }
}