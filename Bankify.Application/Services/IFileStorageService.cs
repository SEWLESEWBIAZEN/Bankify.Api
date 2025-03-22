using Microsoft.AspNetCore.Http;

namespace Bankify.Application.Services
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task RemoveFileAsync(string path);
    }
        public class FileStorageService : IFileStorageService    
    {
        
     

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

        public async Task RemoveFileAsync(string filePath)
        {                  
            try
            {
                // Check if the file exists before attempting to delete it
                if (File.Exists(filePath))
                {
                    // Delete the file
                    File.Delete(filePath);
                    Console.WriteLine($"File deleted successfully.");
                }
                else
                {
                    Console.WriteLine($"File does not exist in the Uploads folder.");
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the deletion process
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }      
     

    }
    }
