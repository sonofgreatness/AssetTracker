using AssetLocater.Domain.Models;
using AssetLocater.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AssetLocater.Controllers
{
    public class AdminController(FileService fileService) : Controller
    {
        private readonly FileService _fileService = fileService;

        // LIST VIEW
        public async Task<IActionResult> Landing()
        {
            var files = await _fileService.GetAllAsync();
            return View(files);
        }

        // UPLOAD
        [HttpPost]
        [RequestSizeLimit(100 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 100 * 1024 * 1024)]
        public async Task<IActionResult> Upload(IFormFile file, string fileType)
        {
            if (file == null || file.Length == 0)
                return RedirectToAction(nameof(Landing));

            await using var ms = new MemoryStream((int)file.Length);
            await file.CopyToAsync(ms);

            var storedFile = new StoredFile
            {
                Name = file.FileName.Trim(),
                FileType = fileType,
                Content = ms.ToArray(),
                ContentType = file.ContentType
            };

            await _fileService.InsertAsync(storedFile);
            return RedirectToAction(nameof(Landing));
        }

        // DOWNLOAD
        public async Task<IActionResult> Download(int id)
        {
            var files = await _fileService.GetAllAsync();
            var file = files.FirstOrDefault(f => f.Id == id);
            if (file == null) return NotFound();

            // Reload full file including BLOB
            var full = await _fileService
                .GetAllAsync()
                .ContinueWith(t => t.Result.First(f => f.Id == id));

            return File(full.Content!, full.ContentType, full.Name);
        }

        // DELETE
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _fileService.DeleteAsync(id);
            return RedirectToAction(nameof(Landing));
        }
    }
}