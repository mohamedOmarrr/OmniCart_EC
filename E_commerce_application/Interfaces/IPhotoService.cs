namespace E_commerce_application.Interfaces;

public interface IPhotoService
{
    Task<string> UploadAsync(Stream stream, string fileName, string categoryName);

    Task DeleteAsync(string imageUrl);
}