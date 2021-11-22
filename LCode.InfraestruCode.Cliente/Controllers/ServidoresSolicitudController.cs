using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LCode.InfraestruCode.Core.BD.Contexto.v1_0;
using LCode.InfraestruCode.Core.BD.Contexto.v1_0.Modelos;
using Microsoft.AspNetCore.Authorization;
using NETCore.Base._3._0;

namespace LCode.InfraestruCode.Cliente.Controllers
{
    public class ServidoresSolicitudController : Controller
    {
        private readonly LCODEContexto _context= new LCODEContexto();

        // GET: ServidoresSolicitud
        [Authorize(Roles = "Admin,Usuario-Desarrollo")]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin")) { 
                return View(await _context.SolicitudesServidores.Where(x=>x.Servidor.StartsWith("DEV")).ToListAsync());
            }
            else
            {
                return View(await _context.SolicitudesServidores.Where(x=>x.UsuarioSolicita==(User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.WindowsUserClaim).Value) && x.Servidor.StartsWith("DEV")).ToListAsync());
            }
        }

        [Authorize(Roles = "Admin,Usuario-Certificacion")]
        public async Task<IActionResult> IndexCertificacion()
        {
            if (User.IsInRole("Admin"))
            {
                return View(await _context.SolicitudesServidores.Where(x => x.Servidor.StartsWith("CER")).ToListAsync());
            }
            else
            {
                return View(await _context.SolicitudesServidores.Where(x => x.UsuarioSolicita == (User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.WindowsUserClaim).Value) && x.Servidor.StartsWith("CER")).ToListAsync());
            }
        }

        // GET: ServidoresSolicitud/Details/5
        [Authorize(Roles = "Admin,Usuario-Desarrollo,Usuario-Certificacion")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servidoresSolicitud = await _context.SolicitudesServidores
                .FirstOrDefaultAsync(m => m.GuidSolicitud == id);
            if (servidoresSolicitud == null)
            {
                return NotFound();
            }

            return View(servidoresSolicitud);
        }
        [Authorize(Roles = "Admin,Usuario-Desarrollo")]
        // GET: ServidoresSolicitud/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin,Usuario-Certificacion")]
        // GET: ServidoresSolicitud/Create
        public IActionResult CreateCertificacion()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Usuario-Certificacion")]
        public async Task<IActionResult> CreateCertificacion([Bind("Servidor,TipoSolicitud,Observacion")] ServidoresSolicitud servidoresSolicitud)
        {
            if (ModelState.IsValid)
            {
                BaseConfiguracion BC = new BaseConfiguracion();
                Guid guid = Guid.NewGuid();
                servidoresSolicitud.GuidSolicitud = guid.ToString();
                servidoresSolicitud.Servidor = "CER" + servidoresSolicitud.Servidor.ToUpper();
                servidoresSolicitud.FechaHoraRegistro = DateTime.Now;
                servidoresSolicitud.FechaHoraUltimaModificacion = DateTime.Now;
                servidoresSolicitud.EstadoSolicitud = EstadoSolicitud.Solicitud_Iniciada;
                servidoresSolicitud.UsuarioSolicita = User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.WindowsUserClaim).Value;
                _context.Add(servidoresSolicitud);
                _context.SaveChanges();
                Api api = new Api();
                string MQResp = api.Post(BC.ObtenerValor("URL:SolicitudServidoresMQ"), servidoresSolicitud);
                if (MQResp != null)
                {
                    return Redirect("/ServidoresSolicitud/Details/" + servidoresSolicitud.GuidSolicitud);
                }
                else
                {
                    servidoresSolicitud.Mensaje = "Ha ocurrido un error con el servicio de ejecución, reporte al administrador!";
                    servidoresSolicitud.FechaHoraUltimaModificacion = DateTime.Now;
                    servidoresSolicitud.EstadoSolicitud = EstadoSolicitud.Solicitud_Error;
                    _context.Entry(servidoresSolicitud).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("500", "Error");
                }
            }
            return View(servidoresSolicitud);
        }
        // POST: ServidoresSolicitud/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Usuario-Desarrollo")]
        public async Task<IActionResult> Create([Bind("Servidor,TipoSolicitud,Observacion")] ServidoresSolicitud servidoresSolicitud)
        {
            if (ModelState.IsValid)
            {
                BaseConfiguracion BC = new BaseConfiguracion();
                Guid guid = Guid.NewGuid(); 
                servidoresSolicitud.GuidSolicitud = guid.ToString();
                servidoresSolicitud.Servidor = "DEV"+servidoresSolicitud.Servidor.ToUpper();
                servidoresSolicitud.FechaHoraRegistro = DateTime.Now;
                servidoresSolicitud.FechaHoraUltimaModificacion = DateTime.Now;
                servidoresSolicitud.EstadoSolicitud = EstadoSolicitud.Solicitud_Iniciada;
                servidoresSolicitud.UsuarioSolicita = User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.WindowsUserClaim).Value;
                _context.Add(servidoresSolicitud);
                _context.SaveChanges();
                Api api = new Api();
                string MQResp=api.Post(BC.ObtenerValor("URL:SolicitudServidoresMQ"), servidoresSolicitud);
                if (MQResp != null)
                {
                    return Redirect("/ServidoresSolicitud/Details/"+ servidoresSolicitud.GuidSolicitud);
                }
                else
                {
                    servidoresSolicitud.Mensaje = "Ha ocurrido un error con el servicio de ejecución, reporte al administrador!";
                    servidoresSolicitud.FechaHoraUltimaModificacion = DateTime.Now;
                    servidoresSolicitud.EstadoSolicitud = EstadoSolicitud.Solicitud_Error;
                    _context.Entry(servidoresSolicitud).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("500", "Error");
                }
            }
            return View(servidoresSolicitud);
        }

        // GET: ServidoresSolicitud/Edit/5

        // POST: ServidoresSolicitud/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // GET: ServidoresSolicitud/Delete/5

        private bool ServidoresSolicitudExists(string id)
        {
            return _context.SolicitudesServidores.Any(e => e.GuidSolicitud == id);
        }
    }
}
