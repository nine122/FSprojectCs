using System.Globalization;
using FullstackHW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FullstackHW.Controllers
{
    //[Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly ProductDbContext _context;
        private readonly IWebHostEnvironment environment;
        public ProductController(ProductDbContext productDbContext, IWebHostEnvironment environment)
        {
            _context = productDbContext;
            this.environment = environment;
        }
        [HttpGet]
        public async Task <IActionResult> Index()
        {
            var productList=await _context.NorthRegion.ToListAsync();
            productList = productList.OrderByDescending(x => x.Id).ToList();
            foreach (var product in productList)
            {
                if(product.ImageFileName==""|product.ImageFileName==null){
                    product.ImageFileName="NoData.jpg";
                }
            }
            return View(productList);
        }

        //GET: ProductController/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Create(ProductViewModel addProductViewModel, IFormFile imageFile){
        try
        {
            string? strImageFile = "NoData.jpg";
            if (imageFile!=null)
            {
                string strDateTime = DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                strImageFile = strDateTime+ "_" + imageFile.FileName;
                string? ImageFullPath = this.environment.WebRootPath + "\\images\\" + strImageFile;
                using(var fileStream = new FileStream(ImageFullPath, FileMode.Create)){
                    await imageFile.CopyToAsync(fileStream);
                }
            }
            ProductViewModel productViewModel = new ProductViewModel(){
                Name = addProductViewModel.Name,
                Description = addProductViewModel.Description,
                Price = addProductViewModel.Price,
                ExpiredDate = DateTime.Now.AddDays(7).ToString("dd/MM/yyyy"),
                ImageFileName = strImageFile,
                Source = addProductViewModel.Source,
            };
            await _context.AddAsync(productViewModel);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"({productViewModel.Name}) was added Successfully";
            return RedirectToAction(nameof(Index));
        }
        catch(Exception ex){
                TempData["errorMessage"] = ex.Message+ "<br/>"+ex.StackTrace;
                return View();
            }
        }
        //Get: ProductController/Edit/5
        [HttpGet]
        public async Task <IActionResult> Edit(int id){
            try{
                var product =await _context.NorthRegion.SingleOrDefaultAsync(x => x.Id == id);
                TempData["ImageFilePath"]="/images/"+product.ImageFileName;
                if(product.Source != null){
                    TempData["Source"] = product.Source?.ToString();
                }else{
                    TempData["Source"] = "Unknown";
                }
                return View(product);
            }
            catch(Exception ex){
                TempData["errorMessage"] = ex.Message+ "<br/>"+ex.StackTrace;
                return View();
            }
        }

        [HttpPost]
        public async Task <IActionResult> Edit(ProductViewModel editProductViewModel, IFormFile imageFile){
            try{
                var product =await _context.NorthRegion.SingleOrDefaultAsync(x => x.Id == editProductViewModel.Id);
                if(product == null){
                    TempData["errorMessage"] = "No data";
                    return View();
                }else{
                    product.Name = editProductViewModel.Name;
                    product.Description = editProductViewModel.Description;
                    product.Price = editProductViewModel.Price;
                    product.Source = editProductViewModel.Source;

                    if (imageFile!=null)
                        {
                            string strDateTime = DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                            string strImageFile = strDateTime+ "_" + imageFile.FileName;
                            string? ImageFullPath = this.environment.WebRootPath + "\\images\\" + strImageFile;
                            using(var fileStream = new FileStream(ImageFullPath, FileMode.Create)){
                                await imageFile.CopyToAsync(fileStream);
                            }
                            product.ImageFileName = strImageFile;
                        }
                    else{
                        product.ImageFileName = editProductViewModel.ImageFileName;
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"({editProductViewModel.Name}) was updated Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(Exception ex){
                TempData["errorMessage"] = ex.Message+ "<br/>"+ex.StackTrace;
                return View();
            }
        }

        [HttpGet]
        public async Task <IActionResult> Delete(int id){
            try{
                var product =await _context.NorthRegion.SingleOrDefaultAsync(x => x.Id == id);
                TempData["ImageFilePath"]="/images/"+product.ImageFileName;
                if(product.Source != null){
                    TempData["Source"] = product.Source?.ToString();
                }else{
                    TempData["Source"] = "Unknown";
                }
                return View(product);
            }
            catch(Exception ex){
                TempData["errorMessage"] = ex.Message+ "<br/>"+ex.StackTrace;
                return View();
            }
        }

        [HttpPost]
        public async Task <IActionResult> Delete(ProductViewModel deleteProductViewModel){
            try{
                var product =await _context.NorthRegion.SingleOrDefaultAsync(x => x.Id == deleteProductViewModel.Id);
                if(product == null){
                    TempData["errorMessage"] = "No data";
                    return View();
                }else{
                    _context.NorthRegion.Remove(product);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"({deleteProductViewModel.Name}) was deleted Successfully ";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(Exception ex){
                TempData["ErrorMessage"] = ex.Message+ "<br/>"+ex.StackTrace;
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
        
}