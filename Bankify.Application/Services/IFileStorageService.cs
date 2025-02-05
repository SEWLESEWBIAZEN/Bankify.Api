using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Renci.SshNet;

namespace Bankify.Application.Services
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(IFormFile file);
        //Task<string> UpdateFileAsync(string existingFilePath, IFormFile newFile);
        //Task<bool> DeleteFileAsync(string filePath);
        //Task<bool> DeleteRangeFilesAsync(IEnumerable<string> filePaths);
        //Task<Stream> DownloadFileAsync(string filePath);
        //Task<string> GetDownloadUrlAsync(string filePath);
        //Task<Stream> OpenFileAsync(string filePath);
        //Task<string> GeneratePublicUrl(string filePath);
    }

    //public class SftpSettings
    //{
    //    public string Host { get; set; }
    //    public int Port { get; set; }
    //    public string Username { get; set; }
    //    public string Password { get; set; }
    //    public string RemotePath { get; set; }
    //    public string BaseUrl { get; set; }
    //}

    public class FileStorageService : IFileStorageService
    {
        //private readonly SftpSettings _sftpSettings;
        //private readonly SftpClient _sftpClient;
        private string _localTempPath;

        //public SftpFileStorageService(IOptions<SftpSettings> sftpSettings)
        //{
        //    _sftpSettings = sftpSettings.Value;

        //    _sftpClient = new SftpClient(_sftpSettings.Host, _sftpSettings.Port, _sftpSettings.Username, _sftpSettings.Password);

        //    ConnectAndEnsureDirectory(_sftpSettings.RemotePath);
        //}

        //private void ConnectAndEnsureDirectory(string remotePath)
        //{
        //    _sftpClient.Connect();
        //    if (!_sftpClient.Exists(remotePath))
        //    {
        //        _sftpClient.CreateDirectory(remotePath);
        //    }
        //    _sftpClient.Disconnect();
        //}

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return "";

            // Generate a unique file name
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            // Define the local directory where the file will be saved
            var localDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            // Ensure the directory exists
            if (!Directory.Exists(localDirectory))
            {
                Directory.CreateDirectory(localDirectory);
            }

            // Combine the directory and file name to get the full local file path
            var localFilePath = Path.Combine(localDirectory, fileName);

            // Save the file to the local directory
            using (var stream = new FileStream(localFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return the relative path of the saved file
            return Path.Combine("Uploads", fileName).Replace("\\", "/");
        }
        //public async Task<string> UpdateFileAsync(string existingFilePath, IFormFile newFile)
        //{
        //    if (newFile == null || newFile.Length == 0)
        //        return existingFilePath;

        //    var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(newFile.FileName);
        //    var newRemoteFilePath = Path.Combine(_sftpSettings.RemotePath, newFileName);

        //    using (var sftp = new SftpClient(_sftpSettings.Host, _sftpSettings.Port, _sftpSettings.Username, _sftpSettings.Password))
        //    {
        //        sftp.Connect();

        //        if (!string.IsNullOrEmpty(existingFilePath))
        //        {
        //            var existingAbsolutePath = existingFilePath.TrimStart('/');
        //            if (sftp.Exists(existingAbsolutePath))
        //            {
        //                sftp.DeleteFile(existingAbsolutePath);
        //            }
        //        }

        //        using (var stream = newFile.OpenReadStream())
        //        {
        //            await Task.Run(() => sftp.UploadFile(stream, newRemoteFilePath));
        //        }
        //        sftp.Disconnect();
        //    }

        //    return newRemoteFilePath.Replace("\\", "/");
        //}

        //public async Task<bool> DeleteFileAsync(string filePath)
        //{
        //    if (string.IsNullOrEmpty(filePath))
        //        return false;

        //    var absoluteFilePath = filePath.TrimStart('/');

        //    using (var sftp = new SftpClient(_sftpSettings.Host, _sftpSettings.Port, _sftpSettings.Username, _sftpSettings.Password))
        //    {
        //        sftp.Connect();
        //        if (sftp.Exists(absoluteFilePath))
        //        {
        //            sftp.DeleteFile(absoluteFilePath);
        //            sftp.Disconnect();
        //            return true;
        //        }
        //        sftp.Disconnect();
        //    }
        //    return false;
        //}

        //public async Task<bool> DeleteRangeFilesAsync(IEnumerable<string> filePaths)
        //{
        //    bool allDeleted = true;

        //    foreach (var filePath in filePaths)
        //    {
        //        var isDeleted = await DeleteFileAsync(filePath);
        //        allDeleted = allDeleted && isDeleted;
        //    }

        //    return allDeleted;
        //}
        //public async Task<Stream> DownloadFileAsync(string filePath)
        //{
        //    if (!_sftpClient.IsConnected)
        //    {
        //        _sftpClient.Connect();
        //    }

        //    var memoryStream = new MemoryStream();
        //    await Task.Run(() => _sftpClient.DownloadFile(filePath, memoryStream));
        //    memoryStream.Position = 0;
        //    return memoryStream;
        //}
        //public async Task<string> GetDownloadUrlAsync(string filePath)
        //{
        //    var encodedFilePath = Uri.EscapeDataString(filePath);

        //    return await Task.FromResult($"{_sftpSettings.BaseUrl}/api/documents/download?path={encodedFilePath}");
        //}
        //public async Task<Stream> OpenFileAsync(string filePath)
        //{
        //    if (string.IsNullOrEmpty(filePath))
        //    {
        //        throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
        //    }

        //    if (!_sftpClient.IsConnected)
        //    {
        //        _sftpClient.Connect();
        //    }

        //    var memoryStream = new MemoryStream();
        //    try
        //    {
        //        await Task.Run(() => _sftpClient.DownloadFile(filePath, memoryStream));
        //        memoryStream.Position = 0; // Reset position before returning
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log exception here
        //        throw new Exception($"Error opening file: {ex.Message}");
        //    }

        //    return memoryStream;
        //}

        //public async Task<string> GeneratePublicUrl(string filePath)
        //{
        //    var encodedFilePath = Uri.EscapeDataString(filePath);
        //    return await Task.FromResult($"{_sftpSettings.BaseUrl}/api/documents/url?path={encodedFilePath}");
        //}
    }
    }
