using AssetLocater.Domain.Services;
using AssetLocater.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AssetLocater.Controllers
{


    public class FilesController(FileService fileService) : Controller
    {
        private readonly FileService _fileService = fileService;

        public async Task<IActionResult> DownloadVehicles()
        {
            var file = await _fileService.GetVehiclesCsvAsync();
            if (file == null) return NotFound();

            return File(
                file.Content,
                file.ContentType,
                file.Name
            );
        }

        public async Task<IActionResult> DownloadDeeds()
        {
            var file = await _fileService.GetDeedsCsvAsync();
            if (file == null) return NotFound();

            return File(
                file.Content,
                file.ContentType,
                file.Name
            );
        }
    }
}
