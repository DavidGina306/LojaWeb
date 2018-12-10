using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Web_Loja.Models;

namespace Web_Loja.Controllers
{
    public class ProdutoController : Controller
    {
        LojaVirtualEntities context = new LojaVirtualEntities();
        string conexao = WebConfigurationManager.ConnectionStrings["LojaVirtualEntities"].ConnectionString;
        // GET: Produto
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult createProduto(Produtos ct)
        {
           try
            {
                ct.status = 1;
                ct.data_cri_prod = DateTime.Now;
                context.Produtos.Add(ct);

                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            string message = "SUCCESS";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });

            //return Json(ct.data_cri_cat, JsonRequestBehavior.AllowGet);

        }

        public ActionResult showProdutos()
        {

            try
            {
                var data = context.Produtos.Select(p => new
                {
                    id_prod_pk = p.id_prod_pk,
                    desc_prod = p.desc_prod,
                    cod_barras = p.cod_barras,
                    status = p.status,
                    fk_cat= p.fk_cat,
                    fk_prod_super=p.fk_prod_super,
                    qtd_prod=p.qtd_prod

                }).ToList();

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }

        }

        public ActionResult listProdutos()
        {

            try
            {
                var data = context.Produtos.Select(p => new
                {
                    id_prod_pk = p.id_prod_pk,
                    desc_prod = p.desc_prod,
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
        public ActionResult updateProduto(Produtos ct)
        {
            string message = "";
            try
            {
                Produtos c = context.Produtos.First(i => i.id_prod_pk == ct.id_prod_pk);
                if (ct.fk_prod_super > 0)
                {
                    c.fk_prod_super = ct.fk_prod_super;
                }
                if (ct.desc_prod != "")
                {
                    c.desc_prod = ct.desc_prod;
                }
                if (ct.cod_barras != "") {
                    c.cod_barras = ct.cod_barras;
                }

                if (ct.qtd_prod>0)
                {
                    c.qtd_prod = ct.qtd_prod;
                }
                if (ct.fk_cat > 0) {
                    c.fk_cat = ct.fk_cat;
                }

                c.data_att_prod = DateTime.Now;
                context.SaveChanges();
                message = "sucesso";
                Console.WriteLine("SAve");


            }
            catch (Exception)
            {
                message = "error";
                throw;
            }


          
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });

            //return Json(ct.data_cri_cat, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult getProdutos(int id)
        {

            try
            {
                var result = from r in context.Produtos
                             where r.id_prod_pk == id
                             select new
                             {
                                 id_prod_pk = r.id_prod_pk,
                                 desc_prod = r.desc_prod,
                                 fk_prod_super = r.fk_prod_super,
                                 cod_barras = r.cod_barras,
                                 qtd_prod=r.qtd_prod,
                                 fk_cat=r.fk_cat,
                             };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }

        }
        public ActionResult deleteProduto(int id)
        {
            Produtos c = context.Produtos.First(i => i.id_prod_pk == id);
            context.Produtos.Remove(c);
            context.SaveChanges();
            return Json(new { Message = id, JsonRequestBehavior.AllowGet });
        }

        public ActionResult statusProduto(int id)
        {
            Produtos c = context.Produtos.First(i => i.id_prod_pk == id);
            c.data_att_prod = DateTime.Now;
            if (c.status == 1)
            {
                c.status = 0;
            }
            else
            {
                c.status = 1;
            }

            context.SaveChanges();

            Console.WriteLine("SAve");
            return Json(new { Message = id, JsonRequestBehavior.AllowGet });
        }
    }
}