using System.ComponentModel;

namespace Desktop.Dto
{
    internal class MessageDto
    {
        /// <summary>
        /// Message id
        /// </summary>
        [Description("Message id")]
        public int Id { get; set; }

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
