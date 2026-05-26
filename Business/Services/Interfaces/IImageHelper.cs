using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Interfaces
{
    /// <summary>
    /// Helper interface for processing and saving image uploads.
    /// </summary>
    public interface IImageHelper
    {
        /// <summary>
        /// Saves and optimizes the uploaded image. Resizes to a vertical 9:16 aspect ratio (1080x1920 pixels)
        /// with natural background extension to ensure high-resolution mobile optimization.
        /// </summary>
        Task<string> ProcessAndSaveVerticalMediaAsync(IFormFile file);
    }
}
