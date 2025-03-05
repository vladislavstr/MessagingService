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
        public required DateTimeOffset SavedAt { get; set; }
    }
}
