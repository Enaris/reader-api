namespace Reader.API.Services.Services
{
    public interface IFileDeleteService
    {
        FileDeleteResult DeleteFile(string url);
    }
}