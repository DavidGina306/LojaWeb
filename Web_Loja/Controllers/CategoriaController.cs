using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using Web_Loja.Models;
using Newtonsoft.Json;
using System.Net;
using System.Data.Entity;
using System.Web.Configuration;

namespace Web_Loja.Controllers
{
    public class CategoriaController : Controller
    {
        LojaVirtualEntities context = new LojaVirtualEntities();
        string conexao = WebConfigurationManager.ConnectionStrings["LojaVirtualEntities"].ConnectionString;
        // GET: Categoria
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult listCategorias()
        {

            try
            {
                var data = context.Categorias.Select(p => new
                {
                    id_cat_pk = p.id_cat_pk,
                    desc_cat = p.desc_cat,
                }).ToList();

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }

        }

        [HttpGet]
        public ActionResult getCategorias(int id)
        {

            try
            {
                var result = from r in context.Categorias
                             where r.id_cat_pk == id
                             select new
                             {
                                 id_cat_pk = r.id_cat_pk,
                                 desc_cat = r.desc_cat,
                                 fk_cat_super = r.fk_cat_super,
                             };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }

        }

        public ActionResult showCategorias()
        {

            try
            {
                var data = context.Categorias.Select(p => new
                {
                    id_cat_pk = p.id_cat_pk,
                    desc_cat = p.desc_cat,
                    fk_cat_super = p.fk_cat_super,
                    status = p.status,

                }).ToList();

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }

        }


        [HttpPost]
        public ActionResult createCategoria(Categorias ct)
        {
            ct.data_cri_cat = DateTime.Now;
            ct.status = 1;

            context.Categorias.Add(ct);
            context.SaveChanges();
            string message = "SUCCESS";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });

            //return Json(ct.data_cri_cat, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult updateCategoria(Categorias ct)
        {
            try
            {
                Categorias c = context.Categorias.First(i => i.id_cat_pk == ct.id_cat_pk);
                if (ct.fk_cat_super>0) {
                    c.fk_cat_super = ct.fk_cat_super;
                }
                if (ct.desc_cat !="") {
                    c.desc_cat= ct.desc_cat;
                }
                c.data_cri_att = DateTime.Now;
                context.SaveChanges();

                Console.WriteLine("SAve");
                

            }
            catch (Exception)
            {

                throw;
            }


            string message = "SUCCESS";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });

            //return Json(ct.data_cri_cat, JsonRequestBehavior.AllowGet);

        }

        public ActionResult deleteCategoria(int id) {
            Categorias c = context.Categorias.First(i => i.id_cat_pk == id);
            context.Categorias.Remove(c);
            context.SaveChanges();
            return Json(new { Message = id, JsonRequestBehavior.AllowGet });
        }

        public ActionResult statusCategoria(int id)
        {
            Categorias c = context.Categorias.First(i => i.id_cat_pk == id);
            c.data_cri_att = DateTime.Now;
            if (c.status == 1)
            {
                c.status = 0;
            }
            else {
                c.status = 1;
            }

            context.SaveChanges();

            Console.WriteLine("SAve");
            return Json(new { Message = id, JsonRequestBehavior.AllowGet });
        }

    }
}