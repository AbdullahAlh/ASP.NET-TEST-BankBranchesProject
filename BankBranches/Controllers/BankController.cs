using BankBranches.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace BankBranches.Controllers
{
    public class BankController : Controller

    {
        static List<BankBranch> branches = new List<BankBranch>()
  {
      new BankBranch {Id = 1,EmployeeCount = 10, Name = "KFH Salwa",Location = "https://maps.app.goo.gl/GSnB2U7kzSrcZ8Jt8",BranchManager = "Fatmah Buyabes"},
      new BankBranch {Id=2,EmployeeCount = 20, Name = "KFH Mishref",Location = "https://g.co/kgs/Y86f6VW",BranchManager = "Fatmah Alghannam"}
  };
        public IActionResult Index(string searchString)
        {
            List<BankBranch> filteredBranches;
            using (var context = new BankContext())
            {
                IQueryable<BankBranch> query = context.Branches; 

                if (!String.IsNullOrEmpty(searchString))
                {
                    query = query.Where(branch => branch.Name.Contains(searchString));
                }

                filteredBranches = query.ToList();
            }

            return View(filteredBranches);
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
                using (var context = new BankContext())
                {
                    context.Branches.Add(branch);
                    context.SaveChanges();  
                }



                return RedirectToAction("Index");

            }

            return View("Create", model);

        }
        public IActionResult Details(int id)


        {
            if (id == null)
            {
                return NotFound();
            }

         
            BankBranch branch = null;
            using (var context = new BankContext())
            {
                branch = context.Branches.FirstOrDefault(b => b.Id == id);
            }
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }
    
    public IActionResult Register()
        {
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BankBranch branch;
            using (var context = new BankContext())
            {
                branch = context.Branches.FirstOrDefault(b => b.Id == id);
            }

            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public IActionResult Edit(int id, BankBranch branch)
        {
            if (id != branch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (var context = new BankContext())
                {
                    try
                    {
                        context.Update(branch);
                        context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BranchExists(branch.Id, context))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return RedirectToAction(nameof(Index));  
                }
            }

         
            return View(branch);
        }

        private bool BranchExists(int id, BankContext context)
        {
            return context.Branches.Any(e => e.Id == id);
        }

       

    }

}