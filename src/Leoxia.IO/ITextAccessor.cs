namespace Leoxia.IO
{
    /// <summary>
    /// Interface for classes saving and loading text from a file.
    /// </summary>
    public interface ITextAccessor
    {
        void Save(string fileName, string content);
        string Load(string fileName);
        bool Exists(string fileName);
    }
}