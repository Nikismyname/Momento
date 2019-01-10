//namespace Momento.Web.Controllers.Data
//{
//    using Microsoft.AspNetCore.Authorization;
//    using Microsoft.AspNetCore.Http;
//    using Microsoft.AspNetCore.Mvc;
//    using Momento.Services.Contracts.Other;
//    using System.IO;
//    using System.Text;
//    using System.Threading.Tasks;

//    public class DataController : Controller
//    {
//        private readonly ISaveData saveData;
//        //private readonly ILayoutViewService layoutService;

//        public DataController(ISaveData saveData/*, ILayoutViewService layoutService*/)
//        {
//            this.saveData = saveData;
//            //this.layoutService = layoutService;
//        }

//        [HttpPost]
//        [Authorize]
//        public IActionResult Download(int id)
//        {
//            var byteArray = Encoding.UTF8.GetBytes(saveData.GetDirectoryData(id));
//            Stream stream = new MemoryStream(byteArray);

//            var dirName = saveData.GetDirectoryName(id);

//            if (stream == null)
//                return NotFound();

//            return File(stream, "application/text", dirName + ".txt"); // returns a FileStreamResult
//        }

//        [HttpGet]
//        [Authorize]
//        public IActionResult Upload(int dirId)
//        {
//            return View(dirId);
//        }

//        [HttpPost]
//        [Authorize]
//        public async Task<IActionResult> Upload(IFormFile file, int dirId)
//        {
//            long size = file.Length;

//            var text = string.Empty;

//            if (file.Length > 0)
//            {
//                using (var reader = new StreamReader(file.OpenReadStream()))
//                {
//                    text = await reader.ReadToEndAsync();
//                }
//            }

//            if(text == string.Empty)
//            {
//                //TODO: return warning or something
//                return Ok();
//            }

//            saveData.UploadData(text,dirId);

//            return Ok();
//        }
//    }
//}
