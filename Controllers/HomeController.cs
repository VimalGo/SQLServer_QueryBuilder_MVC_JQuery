using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SQLServer_QueryBuilder_Core_MVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SQLServer_QueryBuilder_Core_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceProvider _serviceProvider;

        public HomeController(ILogger<HomeController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult GetTableList(string connString)
        {
            List<string> lstTables = new List<string>();

            try
            {
                HttpContext.Session.SetString("ConnectionString", connString);
                var model = _serviceProvider.GetRequiredService<IDatabaseModelFactory>();
                var dbOptions = new DatabaseModelFactoryOptions();
                var databaseModelFactory = model.Create(connString, dbOptions);

                lstTables = databaseModelFactory.Tables.Select(x => x.Name).ToList(); 
            }
            catch (Exception ex)
            {

                var exp = ex;
            }

            return new JsonResult(lstTables);
        }

        [HttpPost]
        public IActionResult GetTableColumnList(string tableName)
        {
            List<string> lstColumns = new List<string>();

            try
            {
                var model = _serviceProvider.GetRequiredService<IDatabaseModelFactory>();
                var dbOptions = new DatabaseModelFactoryOptions();
                var databaseModelFactory = model.Create(HttpContext.Session.GetString("ConnectionString"), dbOptions);

                lstColumns = databaseModelFactory.Tables.Where(x => x.Name.Equals(tableName)).SelectMany(x => x.Columns.Select(x => x.Name)).ToList();
            }
            catch (Exception ex)
            {

                var exp = ex;
            }

            return new JsonResult(lstColumns);
        }
    }
}
