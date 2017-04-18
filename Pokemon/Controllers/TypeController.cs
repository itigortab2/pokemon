using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pokemon.Models;
using System.IO;

namespace Pokemon.Controllers
{
    public class TypeController: Controller
    {

        Entities db = new Entities();


        public ActionResult ReadFile()
        {


            using (StreamReader fs = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/list_type.txt"), System.Text.Encoding.GetEncoding(1251)))
            {
                while (true)
                {
                   
                    string temp = fs.ReadLine();
                    if (temp == null) break;

                    var typ = db.PType.Where(t => t.type == temp).FirstOrDefault();

                    if(typ == null) { 
                        PType type = new PType();

                        type.type = temp;
                        db.PType.Add(type);
                        db.SaveChanges();
                    }
                    

                }
            }

            return RedirectToAction("Index");
        }


        /* метод показа списка типов покемонов */
        public ViewResult Index()
        {
            return View("List", db.PType);
        }

        /* метод создания типа покемона */
        public ActionResult Create()
        {
            PType p = new PType();
            return View(p);
        }

        [HttpPost]
        public ActionResult Create(PType p)
        {
            try
            {
               
                    db.PType.Add(p);
                    db.SaveChanges();
                

            }
            catch (System.Data.DataException)
            {

                ModelState.AddModelError("", "Создать объект не удалось!");
            }
            return RedirectToAction("Index");
        }

        /* метод для показа отдельного типа покемона */
        public ViewResult Details(int id)
        {
            PType type = db.PType.Find(id);

            return View(type);
        }

        /* метод для редактирования типа покемона */
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var type = db.PType.Find(id);

            PTypeViewModels p = new PTypeViewModels();
            p.idType = type.idType;
            p.type = type.type;
            
            return View(p);
        }

        [HttpPost]
        public ActionResult Edit(PTypeViewModels p)
        {

                try
                {
                    var type = db.PType.Find(p.idType);

                    if (type == null)
                        return HttpNotFound();

                    type.type = p.type;
                    type.Pokemon = p.Pokemon;

                    db.SaveChanges();

                }
                catch (System.Data.DataException )
                {
                    
                    ModelState.AddModelError("", "Изменить объект не удалось!");
                }
            
            return RedirectToAction("Index");

        }

        /* удаление типа покемона */
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var pok = db.PType.Find(id);

            if (pok == null)
                return HttpNotFound();

            db.Entry(pok).Collection(c => c.Pokemon).Load();


            db.PType.Remove(pok);
            db.SaveChanges();

            return RedirectToAction("Index");
        }



    }
}