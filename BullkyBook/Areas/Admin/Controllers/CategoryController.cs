using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BullkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            TempData["Success"] = "Category created successfully";
            if (obj.DisplayOrder.ToString() == obj.Name)
            {
                ModelState.AddModelError("DisplayOrder", "Display order can't be the same as Name");

            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
        //Get method
        //this will get the record we want to edit.
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category CategorFormDb = _unitOfWork.Category.Get(u => u.Id == id);//equal to find now where we find by id

            if (CategorFormDb == null)
            {
                return NotFound();
            }
            return View(CategorFormDb);
        }
        //post method
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            TempData["Success"] = "Category Edited successfully";
            if (obj.DisplayOrder.ToString() == obj.Name)
            {
                ModelState.AddModelError("DisplayOrder", "Display order can't be the same as Name");

            }
            if (ModelState.IsValid)
            {
                //beacause we pass the obj the .net will know which recorde to be updated.
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            TempData["Success"] = "Category Deleted successfully";
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category obj = _unitOfWork.Category.Get(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            return RedirectToAction("Index");

        }
    }
}
