namespace Rift
{
    /// <summary>
    /// Exposes a configurable object. 
    /// </summary>
    public interface IConfigurable
    {
        /// <summary>
        /// Writes the application settings section.
        /// </summary>
        void WriteSettings();
    }
}