using Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp; // WebP is highly optimized for mobile
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace Business.Services.Helpers
{
    public class ImageHelper : IImageHelper
    {
        private const int TargetWidth = 1080;
        private const int TargetHeight = 1920;

        // Base folder where images will be saved on the server
        private readonly string _uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles");

        public ImageHelper()
        {
            // Ensure the directory exists when the helper is instantiated
            if (!Directory.Exists(_uploadDirectory))
            {
                Directory.CreateDirectory(_uploadDirectory);
            }
        }

        /// <summary>
        /// Saves and optimizes the uploaded image to 1080x1920.
        /// </summary>
        public async Task<string> ProcessAndSaveVerticalMediaAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File cannot be empty.", nameof(file));

            // Generate a unique filename using WebP for optimal compression and quality
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileNameWithoutExtension(file.FileName)}.webp";
            var filePath = Path.Combine(_uploadDirectory, fileName);

            // Open the uploaded file as a stream
            using var stream = file.OpenReadStream();

            // Load the image into ImageSharp
            using var image = await Image.LoadAsync(stream);

            // Resize the image to 1080x1920
            // ResizeMode.Pad will extend the background naturally if the original aspect ratio isn't exactly 9:16
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(TargetWidth, TargetHeight),
                Mode = ResizeMode.Pad
            }));

            // Save the optimized image to disk
            await image.SaveAsWebpAsync(filePath, new WebpEncoder { Quality = 85 });

            // Return the relative path to be saved in the database
            // This path can be appended to the base URL in the controller/service
            return $"uploads/profiles/{fileName}";
        }
    }
}
