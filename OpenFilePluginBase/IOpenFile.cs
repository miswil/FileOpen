namespace OpenFilePluginBase
{
    public interface IOpenFile
    {
        Task OpenAsync(string path);
        Task EditAsync(string path);
    }
}
