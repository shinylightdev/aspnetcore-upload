using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Upload.Models;

namespace Upload.Controllers
{
  public class HomeController : Controller
  {

    // Details on uploading in ASP.NET Core
    // https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-5.0

    /// <summary>
    /// Uploads file to a location on server.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
      return await UploadFileAsync(file);
    }


    public async Task<JsonResult> UploadFileAsync(IFormFile file)
    {
      JsonResult uploadResult = Json(new { Status = "SUCCESS", Message = "Upload successful" });

      if (file?.Length > 0)
      {
        string fileName = Path.GetFileName(file.FileName);
        string locationFileName = $"C:\\Temp\\Uploaded\\{fileName}";
        try
        {
          if (System.IO.File.Exists(locationFileName))
          {
            System.IO.File.Delete(locationFileName);
          }

          using (FileStream localFile = System.IO.File.OpenWrite(fileName))
          {
            using (FileStream output = System.IO.File.Create(locationFileName))
            {
              await file.CopyToAsync(output);
            }
          }
        }
        catch (Exception e)
        {
          uploadResult = Json(new { Status = "ERROR", Message = e.Message });
        }
      }
      else
      {
        uploadResult = Json(new { Status = "ERROR", Message = "0 Bytes Uploaded." });
      }
      return uploadResult;
    }


    // ORIGINAL ...

    public IActionResult Index()
    {
      return View();
    }


    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }


    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
