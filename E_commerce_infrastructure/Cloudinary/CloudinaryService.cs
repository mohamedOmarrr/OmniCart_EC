using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using E_commerce_application.Interfaces;
using Microsoft.Extensions.Options;

namespace E_commerce_infrastructure.Cloudinary;

public class CloudinaryService : IPhotoService
{
    private readonly CloudinaryDotNet.Cloudinary _cloudinary;

    public CloudinaryService(IOptions<CloudinarySettings> settings)
    {
        var account = new Account(
            settings.Value.CloudName,
            settings.Value.ApiKey,
            settings.Value.ApiSecret);

        _cloudinary = new CloudinaryDotNet.Cloudinary(account);
    }

    public async Task<string> UploadAsync( Stream stream,
        string fileName)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, stream),
            Folder = "products"
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        return result.SecureUrl.ToString();
    }

    public async Task DeleteAsync(string imageUrl)
    {
        var publicId = ExtractPublicId(imageUrl);

        var deleteParams = new DeletionParams(publicId);

        await _cloudinary.DestroyAsync(deleteParams);
    }

    private static string ExtractPublicId(string imageUrl)
    {
        var uri = new Uri(imageUrl);

        var path = uri.AbsolutePath;

        var uploadIndex = path.IndexOf("/upload/");

        if (uploadIndex == -1)
            throw new ArgumentException("Invalid Cloudinary image URL.");

        var publicId = path[(uploadIndex + "/upload/".Length)..];

        var slashIndex = publicId.IndexOf('/');

        if (slashIndex != -1)
            publicId = publicId[(slashIndex + 1)..];

        return Path.ChangeExtension(publicId, null);
    }
}