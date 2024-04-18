using BankBranches.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankBranches.Controllers
{
    public class BankController : Controller

    {
        static List<BankBranch> branches = new List<BankBranch>()
  {
      new BankBranch {Id = 1,EmployeeCount = 10, Name = "KFH Salwa",Location = "https://maps.app.goo.gl/GSnB2U7kzSrcZ8Jt8",BranchManager = "Fatmah Buyabes"},
      new BankBranch {Id=2,EmployeeCount = 20, Name = "KFH Mishref",Location = "https://g.co/kgs/Y86f6VW",BranchManager = "Fatmah Alghannam"}
  };
        public IActionResult Index()
        {
            return View(branches);
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create(NewBranchForm model)
        {
            if (ModelState.IsValid)
            {
                var name = model.Name;
                var branchManager = model.BranchManager;
                var employeeCount = model.EmployeeCount;
                var location = model.Location;

                var branch = new BankBranch();
                branch.Name = name;
                branch.BranchManager = branchManager;
                branch.EmployeeCount = employeeCount;
                branch.Location = location;
               
                branches.Add(branch);


                return RedirectToAction("Index");

            }

            return View("Create");

        }
        public IActionResult Details(int id)
        {
            var branch = branches.SingleOrDefault(a => a.Id == id);
            if (branches == null)
            {
                return RedirectToAction("Index");

            }
            return View(branch);
        }
        public IActionResult Register()
        {
            return View();
        }




    }

}