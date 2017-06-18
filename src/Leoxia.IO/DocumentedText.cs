namespace Leoxia.IO
{
    /// <summary>
    /// Represents text with a content and a header
    /// </summary>
    /// <typeparam name="T">type of header</typeparam>
    /// <seealso cref="Leoxia.IO.IDocumentedText{T}" />
    public class DocumentedText<T> : IDocumentedText<T>
        where T : class
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        public T Header { get; set; }
    }
}