using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.ViewModels;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BullkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Index()
        {
            List<Company> objCompanytList = _unitOfWork.Company.GetAll().ToList();

            return View(objCompanytList);
        }
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Upsert(int? id) /* update + insert = Upsert*/
        {
            

            
            //this means create or insert
            if (id == null || id == 0)
            {
                Company comp = new Company();
                return View(comp);
            }
            //this means update
            else
            {
                Company companyFromDb = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyFromDb);
            }
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {
                
                if (obj.Id == 0)
                {
                    _unitOfWork.Company.Add(obj);
                }
                else
                {
                    _unitOfWork.Company.Update(obj);
                }
                _unitOfWork.Save();
                TempData["Success"] = "product Created Successfully";
                return RedirectToAction("Index");

            }
            
            return View(obj);
        }

        

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanytList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanytList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyTobeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if (companyTobeDeleted == null)
            {
                return Json(new { Success = false, message = "Error while deleting" });

            }
            
            _unitOfWork.Company.Remove(companyTobeDeleted);
            _unitOfWork.Save();
            return Json(new { Success = true, message = "Deleted successfully" });

            
        }
        #endregion

    }
}
