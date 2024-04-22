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
               // context.Employees.RemoveRange(context.Employees.ToList());
                // context.SaveChanges();

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
            if (id == 0)
            {
                return NotFound();
            }

            using (var context = new BankContext())
            {
                var branch = context.Branches
                                    .Include(b => b.Employees)  // Include the Employees navigation property
                                    .FirstOrDefault(b => b.Id == id);

                if (branch == null)
                {
                    return NotFound();
                }

                return View(branch);
            }
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

        [HttpGet]
        public IActionResult AddEmployee(int id)
        {
            using (var context = new BankContext())
            {
                var branch = context.Branches.FirstOrDefault(b => b.Id == id);
                if (branch == null)
                {
                    return NotFound();
                }

                ViewBag.BranchId = id;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEmployee(int id, AddEmployeeForm model)
        {
            if(ModelState.IsValid)
    {
                //if (ModelState.ContainsKey("CivilId") && ModelState["CivilId"].Errors.Any(e => e.ErrorMessage == "Duplicate CivilId"))
                //{
                //    ModelState.AddModelError("CivilId", "A duplicate Civil ID was provided.");
                //}
                // return View(model);

                using (var context = new BankContext())
                {
                    var branch = context.Branches.Include(b => b.Employees).FirstOrDefault(b => b.Id == id);
                    if (branch == null)
                    {
                        ModelState.AddModelError("", $"Branch with ID {id} not found.");
                        return View(model);
                    }

                    var employee = new Employee
                    {
                        Name = model.Name,
                        Position = model.Position,
                        CivilId = model.CivilId,
                        BankBranchId = id
                    };

                    branch.Employees.Add(employee);
                    try
                    {
                        context.SaveChanges();
                        return RedirectToAction("Details", new { id = id });

                    }
                    catch (DbUpdateException errors)
                    {
                        ModelState.AddModelError("CivilId", "A duplicate Civil ID was provided.");

                    }
                }

            }
            return View(model);

        }
    }
}