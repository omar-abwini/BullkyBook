using BullkyWebRazor_Temp.Data;
using BullkyWebRazor_Temp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BullkyWebRazor_Temp.Pages.Categories
{
  
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Category Categories { get; set; }
        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            
        }
     
        public IActionResult OnPost()
        {
           
                _db.Categories.Add(Categories);
                _db.SaveChanges();
                return RedirectToPage("Index");

        }
    }
}
