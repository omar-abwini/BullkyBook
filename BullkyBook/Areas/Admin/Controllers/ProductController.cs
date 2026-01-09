using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BullkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        //you don't need to register this in the program container because it's bult in .net core.
        //THIS IS WHAT IS USED TO ACCESS THE wwroot FOLDER.
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Porduct.GetAll(includeProperaties:"Category").ToList();

            return View(objProductList);
        }
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Upsert(int? id) /* update + insert = Upsert*/
        {
            

            //ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;
            ProductVM productVM = new()
            {
                //this is called projection in .net
                /*IEnumerable < SelectListItem >*/ CategoryList = _unitOfWork.Category.
                GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }), 
                Product = new Product()
            };
            //this means create or insert
            if(id == null || id == 0)
            {
                return View(productVM);
            }
            //this means update
            else
            {
                productVM.Product = _unitOfWork.Porduct.Get(u => u.Id == id);
                return View(productVM);
            }
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                //this is what gives the wwwRoot folder path.
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images/product");

                    //if ther's already an image:
                    //if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    //{
                    //    //delete the old image
                    //    var oldImagePath = 
                    //        Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                    //    if (System.IO.File.Exists(oldImagePath))
                    //    {
                    //        System.IO.File.Delete(oldImagePath);
                    //    }
                    //}

                    //using (var fileStream = new FileStream(Path.Combine(productPath,fileName), FileMode.Create))
                    //{
                    //    file.CopyTo(fileStream);
                    //}
                   // productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }
                if (productVM.Product.Id == 0) {
                    _unitOfWork.Porduct.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Porduct.Update(productVM.Product);
                }
                
                _unitOfWork.Save();
                TempData["Success"] = "product Created Successfully";
                return RedirectToAction("Index");
            }
            //THE ELSE PART IS DONE TO REPOPULATE THE LIST
            else
            {
                productVM.CategoryList = _unitOfWork.Category.
                GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
            
        }

        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product obj= _unitOfWork.Porduct.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    string wwwRootPath = _webHostEnvironment.WebRootPath;
        //    if (!string.IsNullOrEmpty(obj.ImageUrl))
        //    {
        //        //delete the old image
        //        var oldImagePath =
        //            Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart('\\'));
        //        if (System.IO.File.Exists(oldImagePath))
        //        {
        //            System.IO.File.Delete(oldImagePath);
        //        }
        //    }

        //    _unitOfWork.Porduct.Remove(obj);
        //    _unitOfWork.Save();
        //    return RedirectToAction("Index");

        //}

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Porduct.GetAll(includeProperaties: "Category").ToList();
            return Json(new {data = objProductList});
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productTobeDeleted = _unitOfWork.Porduct.Get(u => u.Id == id);
            if(productTobeDeleted == null)
            {
                return Json(new { Success = false, message = "Error while deleting" });

            }
            //var oldImagePath =
            //        Path.Combine(_webHostEnvironment.WebRootPath, productTobeDeleted.ImageUrl.TrimStart('\\'));
            //if (System.IO.File.Exists(oldImagePath))
            //{
            //    System.IO.File.Delete(oldImagePath);
            //}
            _unitOfWork.Porduct.Remove(productTobeDeleted);
            _unitOfWork.Save();
            return Json(new { Success = true, message = "Deleted successfully" });

            
        }
        #endregion
    }
}

//LEGACY EDIT

//public IActionResult Edit(int? id)
//{
//    if (id == null)
//    {
//        return NotFound();
//    }
//    Product productFromDb = _unitOfWork.Porduct.Get(u=>u.Id == id);
//    if (productFromDb == null)
//    {
//        return NotFound();
//    }
//    return View(productFromDb);

//}
//[HttpPost]
//public IActionResult Edit(Product obj)
//{
//    if (ModelState.IsValid)
//    {
//        _unitOfWork.Porduct.Update(obj);
//        _unitOfWork.Save();
//        return RedirectToAction("Index");
//    }
//    return View() ;
//}
