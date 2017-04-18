using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pokemon.Models;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Threading.Tasks;

namespace Pokemon.Controllers
{
    public class PokemonController : Controller
    {

        Entities db = new Entities();



        public async Task<ActionResult> Index(SortState sortOrder = SortState.TitleAsc)
        {
            IQueryable<Pokemon> pokemons = db.Pokemon;

            ViewData["TitleSort"] = sortOrder == SortState.TitleAsc ? SortState.TitleDesc : SortState.TitleAsc;

            switch (sortOrder)
            {

                case SortState.TitleAsc:
                    pokemons = pokemons.OrderBy(s => s.title);
                    break;
                case SortState.TitleDesc:
                    pokemons = pokemons.OrderByDescending(s => s.title);
                    break;

                default:
                    pokemons = pokemons.OrderBy(s => s.title);
                    break;
            }
            return View("List",await pokemons.AsNoTracking().ToListAsync());
        }


        /* метод создания покемона */
        public ActionResult Create()
        {
            List<PType> types = new List<PType>(db.PType);

            ViewBag.Types = types;

            return View(new Pokemon());
        }

        [HttpPost]
        public ActionResult Create(Pokemon pokemon, int[] selectedTypes)
        {

            try
            {
                if (ModelState.IsValid)
                {



                    if (selectedTypes != null)
                    {

                        foreach (var c in db.PType.Where(co => selectedTypes.Contains(co.idType)))
                        {
                            pokemon.PType.Add(c);
                        }
                    }

                    db.Pokemon.Add(pokemon);

                    db.SaveChanges();

                }
            }
            catch (System.Data.DataException)
            {

                ModelState.AddModelError("", "Изменить объект не удалось!");
            }
            return RedirectToAction("Index");
        }

        /* метод для показа отдельного покемона */
        public ViewResult Details(int id)
        {
            var p = db.Pokemon.Find(id);


            return View(p);
        }

        /* метод для редактирования покемона */
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }


            var pokemon = db.Pokemon.Find(id);

            PokemonViewModels p = new PokemonViewModels();

            p.PokemonId = pokemon.IdPokemon;

            p.title = pokemon.title;

            p.PType = pokemon.PType;

            List<PType> types = new List<PType>(db.PType);

            ViewBag.Types = types;

            return View(p);
        }


        [HttpPost]
        public ActionResult Edit(PokemonViewModels pokemon, int[] selectedTypes)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var p = db.Pokemon.Find(pokemon.PokemonId);

                    if (p == null)
                        return HttpNotFound();

                    p.title = pokemon.title;


                    if (selectedTypes != null)
                    {

                        foreach (var c in db.PType.Where(co => selectedTypes.Contains(co.idType)))
                        {
                            p.PType.Add(c);
                        }
                    }

                    db.Entry(p).State = EntityState.Modified;
                    db.SaveChanges();

                }
                catch (System.Data.DataException)
                {
                    ModelState.AddModelError("", "Изменить объект не удалось!");
                }
            }

            return RedirectToAction("Index");

        }

        /* удаление покемона */
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var pok = db.Pokemon.Find(id);

            if (pok == null)
                return HttpNotFound();


            db.Entry(pok).Collection(c => c.PType).Load();

            db.Pokemon.Remove(pok);

            db.SaveChanges();

            return RedirectToAction("Index");
        }


        /* запись и чтение файлов */

        public ActionResult ReadFile()
        {


            using (StreamReader fs = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/list.txt"), System.Text.Encoding.GetEncoding(1251)))
            {
                while (true)
                {

                    string temp = fs.ReadLine();
                    if (temp == null) break;


                    string[] t = temp.Split(' ');


                    Pokemon p = new Pokemon();

                    var name = t[0];
                    var tp = db.Pokemon.Where(ti => ti.title == name).FirstOrDefault();
                    if (tp == null)
                    {
                        p.title = t[0];


                        for (int i = 1; i < t.Count(); i++)
                        {

                            string n = t[i];

                            var type = db.PType.Where(ta => ta.type == n).FirstOrDefault();

                            if (type != null) p.PType.Add(type);
                            else
                            {
                                PType typ = new PType();
                                typ.type = t[i];
                                db.PType.Add(typ);
                                db.SaveChanges();

                                var type2 = db.PType.Where(ta => ta.type == n).FirstOrDefault();

                                p.PType.Add(type2);
                            }

                        }

                        db.Pokemon.Add(p);
                        db.SaveChanges();
                    }


                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult writeXMLFromDB()
        {
            string pathToXml = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/pokemons.xml");

            try
            {
                if (!System.IO.File.Exists(pathToXml))
                {
                    XmlTextWriter textWritter = new XmlTextWriter(pathToXml, System.Text.Encoding.UTF8);


                    textWritter.WriteStartDocument();

                    textWritter.WriteStartElement("pokemons");

                    textWritter.WriteEndElement();

                    textWritter.Close();
                }


                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(pathToXml);

                XmlNode rootNode = xmlDoc.CreateElement("pokemons");
               // xmlDoc.AppendChild(rootNode);

                var pokemons = db.Pokemon.Include("PType");

                foreach (Pokemon p in pokemons)
                {


                    XmlNode pokNode = xmlDoc.CreateElement("pokemon");
                    XmlNode name = xmlDoc.CreateAttribute("name");

                    name.InnerText = p.title;
                    pokNode.AppendChild(name);

                    XmlNode grouptype = xmlDoc.CreateElement("grouptyp");
                    pokNode.AppendChild(grouptype);

                    foreach (PType t in p.PType)
                    {
                        XmlNode type = xmlDoc.CreateElement("type");
                        type.InnerText = t.type;
                        grouptype.AppendChild(type);

                    }

                    xmlDoc.Save(pathToXml);

                }
            }

            catch (System.Data.DataException)
            {
                ModelState.AddModelError("", "Неудалось создать файл!");
            }

            return RedirectToAction("Index");
        }


        public ActionResult ReadXMLToDB()
        {

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/pokemons.xml"));

            XmlElement xRoot = xDoc.DocumentElement;

            foreach (XmlNode xnode in xRoot)
            {

                Pokemon temp = new Pokemon();

                bool flag = true;

                foreach (XmlNode childnode in xnode.ChildNodes)
                {


                    if (childnode.Name == "name")
                    {
                        var pok = db.Pokemon.Where(n => n.title == childnode.InnerText).FirstOrDefault();

                        if (pok == null) temp.title = childnode.InnerText;
                        else { flag = false; break; }
                    }

                    if (childnode.Name == "grouptype" && flag == true)
                    {
                        foreach (XmlNode childnode2 in childnode.ChildNodes)
                        {
                            var type = db.PType.Where(t => t.type == childnode2.InnerText).FirstOrDefault();

                            if (type != null)
                            {
                                temp.PType.Add(type);
                            }
                            else
                            {
                                PType t = new PType();
                                t.type = childnode2.InnerText;
                                db.PType.Add(t);
                                db.SaveChanges();

                                var typ = db.PType.Where(ta => ta.type == childnode2.InnerText).FirstOrDefault();

                                temp.PType.Add(typ);
                            }

                        }
                    }

                }

                if (flag)
                {
                    db.Pokemon.Add(temp);
                    db.SaveChanges();
                }

            }

            return RedirectToAction("Index");
        }



    }
}