using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_App.Models;
using System.IO;
using System.Drawing;
namespace Mvc_App.Controllers
{
    public class HomeController : Controller
    {
        ilhanDemirtepeContext ılhanDemirtepeContext = new ilhanDemirtepeContext();
        public ActionResult Index()
        {
            List<News> newsList = ılhanDemirtepeContext.News.ToList();
            return View(newsList);
        }

        [HttpPost]
        public JsonResult NewsDelete(string ID)
        {
            int id = Convert.ToInt32(ID);
            var stud = (from s in ılhanDemirtepeContext.News
                        where s.Id == id
                        select s).FirstOrDefault();
            ılhanDemirtepeContext.News.Remove(stud);

            string filePath = Server.MapPath(stud.ImageUrl);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            int num = ılhanDemirtepeContext.SaveChanges();
            return Json(num, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddOrEditNews(int id = 0)
        {
            if (id == 0)
            {
                News n = new News();
                return View(n);
            }
            else
            {
                News n = ılhanDemirtepeContext.News.FirstOrDefault(x => x.Id == id);

                return View(n);

            }



        }
        [HttpPost]
        public ActionResult AddOrEditNews(int uid, News news, HttpPostedFileBase ImageUrl)
        {
            if (uid > 0)//Update
            {
                News n = ılhanDemirtepeContext.News.FirstOrDefault(x => x.Id == uid);
                ılhanDemirtepeContext.News.Remove(n);
                string filePath = Server.MapPath(n.ImageUrl);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                if (ImageUrl != null)
                {


                    Image image = Image.FromStream(ImageUrl.InputStream);
                    Bitmap resim = new Bitmap(image, ImageSetting.NewsBoy);
                    string yol = "/Content/NewsResim/" + Guid.NewGuid() + Path.GetExtension(ImageUrl.FileName);
                    resim.Save(Server.MapPath(yol));
                    news.ImageUrl = yol;
                    ılhanDemirtepeContext.News.Add(news);
                    ılhanDemirtepeContext.SaveChanges();
                }

            }
            else //insert 
            {
                if (ImageUrl != null)
                {
                    Image image = Image.FromStream(ImageUrl.InputStream);
                    Bitmap resim = new Bitmap(image, ImageSetting.NewsBoy);
                    string yol = "/Content/NewsResim/" + Guid.NewGuid() + Path.GetExtension(ImageUrl.FileName);
                    resim.Save(Server.MapPath(yol));
                    news.ImageUrl = yol;
                    ılhanDemirtepeContext.News.Add(news);
                    ılhanDemirtepeContext.SaveChanges();
                }
            }
            return RedirectToAction("Index");

        }


    }
}





