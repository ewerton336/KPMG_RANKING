using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RankingAPIWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameResultController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveGameScore(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        // GET: GameResultController
        public ActionResult Index()
        {
            return View();
        }

        // GET: GameResultController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GameResultController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GameResultController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GameResultController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: GameResultController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GameResultController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GameResultController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
