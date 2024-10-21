using motoRental.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

public class StorageServiceTests
{
    private readonly StorageService _storageService;
    private readonly string _storagePath;

    public StorageServiceTests()
    {
        // Inicializa o serviço
        _storageService = new StorageService();
        _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
    }

    [Theory]
    [InlineData("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAABoElEQVR42mNkoBAwUqifw0En7CTHh1tMwQiYZdAKBThqJJtXU2zLVYAFFwDg4EPsk1jG1BUCFllUS4cBFTsKlGyOrjkpCnkDO9AJCWpFgVQbiDGImh0s+4BR1lAJoK5mUiwIAX+MykoVROcWgSVuox8IVsXzGWWcI1djGwWcRLLhZJAQ1oF4QdBoMQwZBF2GLSh4AG2+GMoeImRMFvEl4xAsU9IVDDCzgCaAgNOiIS7CQgcHqIbAABBgUkMdVIwpBpTNNDEEUgA4FBfGFkKTcCASeWsPioOQJoK5UEKwx2MxAiDkIAUYBWCE6HlDcU4kNZ6IiZQVbDA3El6QqZIBg9iRhDZEBFAaRJNDFAoLgAQPg0IXyAhlPSAmsT5GJQMxNcygSMhCE2oCaKp9oIqqCmyKBZoAmowAUYBAghwLoCpCnmRGgCbhRR3ANqiwM6RMRHhMJ4zMzMzAAALeb3HAbAcHBwAAAABJRU5ErkJggg==")]
    public async Task SaveImage_ValidBase64Image_ShouldReturnRelativePath(string base64Image)
    {
        // Act
        var result = await _storageService.SaveImage(base64Image);

        // Assert
        Assert.False(string.IsNullOrEmpty(result));
        Assert.StartsWith("/images/", result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task SaveImage_InvalidBase64Image_ShouldThrowArgumentNullException(string base64Image)
    {
        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _storageService.SaveImage(base64Image));
    }

    [Theory]
    [InlineData("/images/test_image1.jpg")]
    [InlineData("/images/test_image2.jpg")]
    public void DeleteImage_ExistingFile_ShouldDeleteFile(string relativePath)
    {
        // Arrange
        var fullPath = Path.Combine(_storagePath, Path.GetFileName(relativePath));
        File.WriteAllText(fullPath, "test content");

        // Act
        _storageService.DeleteImage(relativePath);

        // Assert
        Assert.False(File.Exists(fullPath));
    }

    [Theory]
    [InlineData("/images/non_existent_image.jpg")]
    public void DeleteImage_FileDoesNotExist_ShouldDoNothing(string relativePath)
    {
        // Arrange
        var fullPath = Path.Combine(_storagePath, Path.GetFileName(relativePath));

        // Act
        _storageService.DeleteImage(relativePath);

        // Assert
        Assert.False(File.Exists(fullPath)); // Nenhuma exceção deve ser lançada
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void DeleteImage_InvalidPath_ShouldDoNothing(string invalidPath)
    {
        // Act
        _storageService.DeleteImage(invalidPath);

        // Assert
        // Nenhuma exceção deve ser lançada, e o caminho inválido é simplesmente ignorado
    }
}
