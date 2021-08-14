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
            ModeloBienvenida datosUsario = new ModeloBienvenida()
            {
                codEmpleado = codigoE,
                codTienda = codigoT,
                nombreEmpleado = nombreE,
                nombreTienda = nombreT
            };

         
            return Json(data:codigob+'-'+codigoa+'-'+nombreE+'-'+nombreT);
        }
        public ActionResult IndexUsuario(string text)
        {
            String[] arr = text.Split('-');
            ViewBag.codigoT = arr[0];
            ViewBag.codigoE = arr[1];
            ViewBag.nombreE = arr[2];
            ViewBag.nombreT = arr[3];
            ModeloBienvenida datosUsario = new ModeloBienvenida()
            {
                codEmpleado = int.Parse(arr[1]),
                codTienda = int.Parse(arr[0]),
                nombreEmpleado = arr[2],
                nombreTienda = arr[3]
            };
            return View("GetNombreUsuario");
        }
        
   
       

    }
}