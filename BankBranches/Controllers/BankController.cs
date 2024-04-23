using BankBranches.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace BankBranches.Controllers
{
    public class BankController : Controller

    {
        private readonly BankContext _context;

        public BankController(BankContext context)
        {
            _context = context;
        }
        static List<BankBranch> branches = new List<BankBranch>()
  {
      new BankBranch {Id = 1,EmployeeCount = 10, Name = "KFH Salwa",Location = "https://maps.app.goo.gl/GSnB2U7kzSrcZ8Jt8",BranchManager = "Fatmah Buyabes"},
      new BankBranch {Id=2,EmployeeCount = 20, Name = "KFH Mishref",Location = "https://g.co/kgs/Y86f6VW",BranchManager = "Fatmah Alghannam"}
  };
        public IActionResult Index(string searchString)
        {
            var viewModel = new BankDashboardViewModel();

            IQueryable<BankBranch> query = _context.Branches.Include(b => b.Employees);  

            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(branch => branch.Name.Contains(searchString));
            }

            viewModel.BranchList = query.ToList();
            viewModel.TotalBranches = _context.Branches.Count();
            viewModel.TotalEmployees = _context.Employees.Count();
            viewModel.BranchWithMostEmployees = _context.Branches
                .OrderByDescending(b => b.Employees.Count)
                .FirstOrDefault();
            return View(viewModel);
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
                using (_context)
                {
                    _context.Branches.Add(branch);
                    _context.SaveChanges();
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

            using (_context)
            {
                var branch = _context.Branches
                                    .Include(b => b.Employees)  
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
            using (_context)
            {
                branch = _context.Branches.FirstOrDefault(b => b.Id == id);
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
                using (_context)
                {
                    try
                    {
                        _context.Update(branch);
                        _context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BranchExists(branch.Id, _context))
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
            using (_context)
            {
                var branch = _context.Branches.FirstOrDefault(b => b.Id == id);
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

                using (_context)
                {
                    var branch = _context.Branches.Include(b => b.Employees).FirstOrDefault(b => b.Id == id);
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
                        _context.SaveChanges();
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