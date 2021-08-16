using examenDDLH.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace examenDDLH.Controllers
{
    public class LoginController : Controller
    {
        public List<ModeloTienda> lista { get; set; }
        public List<ModeloPedido> listaPedidos { get; set; }
        public List<ModeloInfoSku> infoSkus { get; set; }
        // GET: Login 
        public ActionResult Index()
        {
            
            if(lista == null)
            {
                using (Models.examenEntities1 db = new Models.examenEntities1())
                {

                    lista =
                        (from d in db.gentiendascat
                         where d.estatus == "A" && d.tipo == "T" && d.regional != "Cerradas"
                         select new ModeloTienda
                         {
                             codigotienda = d.codigoTienda,
                             descripciontienda = d.descripcionTienda
                         }).ToList();

                }

                List<SelectListItem> items = lista.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = a.descripciontienda.ToString(),
                        Value = a.codigotienda.ToString(),
                        Selected = false
                    };

                });


                ViewBag.itemst = items;
            }
            else
            {
                ViewBag.itemst = lista.ConvertAll(a => {

                    return new SelectListItem()
                    {
                        Text = a.descripciontienda.ToString(),
                        Value = a.codigotienda.ToString(),
                        Selected = false
                    };
                });
            }
            
            return View();
        }

        public JsonResult GetTiendas()
        {
            
            return Json(true);
        }

        public JsonResult GetUsuarios(int codigoTienda)
        {
            List<ModeloEmpleado> lstEmp = null;
            using (Models.examenEntities2 db = new Models.examenEntities2())
            {
                lstEmp = (from d in db.segempleadoscat
                          where d.estatus == "A" && d.codigoTiendaAsignada == codigoTienda
                          select new ModeloEmpleado
                          {
                              codigoEmpleado = d.codigoEmpleado,
                              nombreEmpleado = d.nombre
                          }).ToList();
            }
            List<SelectListItem> itemsEmp = lstEmp.ConvertAll(e => {

                return new SelectListItem()
                {
                    Text = e.nombreEmpleado.ToString(),
                    Value = e.codigoEmpleado.ToString()
                };
            });
            ViewBag.itemse = itemsEmp;
            return Json(new SelectList(itemsEmp,"Value","Text"));
        }
        public JsonResult GetNombreUsuario(int codigoT,string nombreT,int codigoE,string nombreE)
        {

            ViewBag.codigoE = codigoE;
            ViewBag.codigoT = codigoT;
            ViewBag.nombreE = nombreE;
            ViewBag.nombreT = nombreT;
            string codigoa = codigoE.ToString();
            string codigob = codigoT.ToString();

         
          
         
            return Json(data:codigob+'-'+codigoa+'-'+nombreE+'-'+nombreT);
        }

        public JsonResult GetInfo(int codsku)
        {
            String[] arr = null;
            try
            {
                if (infoSkus == null)
                {
                    using (Models.examenEntities3 db = new Models.examenEntities3())
                    {
                        infoSkus = (from d in db.genskuscat
                                    where d.sku == codsku
                                    select new ModeloInfoSku()
                                    {
                                        codSku = d.sku.ToString(),
                                        descripcion = d.descripcionCorta,
                                        precioUnitario = d.precioOriginal
                                    }).ToList();

                    }
                    if (infoSkus.Count == 0) throw new Exception("No se encontro informacion");
            
                    arr = new string[infoSkus.Count];
                    foreach (var item in infoSkus)
                    {
                        for (int i = 0; i < arr.Length; i++)
                        {
                            arr[i] = item.codSku + "-" + item.descripcion + "-" + item.precioUnitario;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
           


            return Json(data: arr[0]);
        }


        public JsonResult GetTotal(int cantidad, int codsku)
        {
            decimal iva = 0M, resultado = 0M;
            if (infoSkus == null)
            {
                using (Models.examenEntities3 db = new Models.examenEntities3())
                {
                    infoSkus = (from d in db.genskuscat
                                where d.sku == codsku
                                select new ModeloInfoSku()
                                {
                                    codSku = d.sku.ToString(),
                                    descripcion = d.descripcionCorta,
                                    precioUnitario = d.precioOriginal
                                }).ToList();

                }
                foreach (var item in infoSkus)
                {
                    iva  = (item.precioUnitario * cantidad)* 0.3M;
                    resultado = (item.precioUnitario * cantidad) + iva;
                }
            }
            

            return Json(resultado);
        }
        public ActionResult IndexUsuario(string text)
        {
            String[] arr = text.Split('-');
            ViewBag.codigoT = arr[0];
            ViewBag.codigoE = arr[1];
            ViewBag.nombreE = arr[2];
            ViewBag.nombreT = arr[3];
            ModeloPedido modelo = new ModeloPedido();
            if(listaPedidos == null)
            {
                using (Models.examenEntities3 db = new Models.examenEntities3())
                {
                    listaPedidos = (from d in db.genskuscat where d.sku > 0 && d.sku<100
                                    select new ModeloPedido
                                    {
                                        codSku = d.sku.ToString()
                                        
                                    }).ToList();
                }
                List<SelectListItem> itemsp = listaPedidos.ConvertAll(a => {
                    return new SelectListItem()
                    {
                        Value = a.codSku,
                        Text = a.codSku
                    };
                
                });
                ViewBag.itemsPedidos = itemsp;
            }
            
            return View("GetNombreUsuario",modelo);
        }


        public ActionResult enviardatos(string codSku,int piezas)
        {
            return null;
        }





    }
}