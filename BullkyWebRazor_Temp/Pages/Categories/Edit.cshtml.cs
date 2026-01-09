using BullkyWebRazor_Temp.Data;
using BullkyWebRazor_Temp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BullkyWebRazor_Temp.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Category Categories { get; set; }
        public EditModel(ApplicationDbContext db)
        {
            _db=db;
        }
        public IActionResult OnGet(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            Category category = _db.Categories.Find(id);
            if(category == null)
            {
                return NotFound();
            }
            return Page();
            
        }
        public IActionResult OnPost()
        {
           
                _db.Categories.Update(Categories);
                _db.SaveChanges();
               return RedirectToPage("Index");
            
            
        }
           
    }
}
