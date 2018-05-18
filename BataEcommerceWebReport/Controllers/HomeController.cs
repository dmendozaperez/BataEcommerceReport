using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using BataEcommerceWebReport.Helpers;
using System.Threading.Tasks;
using BataEcommerceWebReport.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace BataEcommerceWebReport.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public string reportServerUrl = "http://" + ConfigurationManager.AppSettings["BataRSHost"].ToString() + "/ReportServer/";
        public ReportCredentials reportCredential = new ReportCredentials(ConfigurationManager.AppSettings["BataRSUser"].ToString(), ConfigurationManager.AppSettings["BataRSPass"].ToString(), ConfigurationManager.AppSettings["BataRSHost"].ToString());
        public string reportFolder = ConfigurationManager.AppSettings["BataRSFolder"].ToString();

        public HomeController()
        {
        }

        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }




        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Index(string returnUrl = "")
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Title = "Iniciar Sesión";
            return View();
        }


        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // No cuenta los errores de inicio de sesión para el bloqueo de la cuenta
            // Para permitir que los errores de contraseña desencadenen el bloqueo de la cuenta, cambie a shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Intento de inicio de sesión no válido.");
                    return View(model);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }


        #region Sistema
        public ActionResult Panel()
        {
            ViewBag.Title = "Panel de Administración";
            return View();
        }
        #endregion



        #region Reportes de Sistemas

        public ActionResult ReporteVentas()
        {
            // ** Procesar Reporte en Remoto
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Remote;
            reportViewer.SizeToReportContent = true;
            reportViewer.ZoomMode = ZoomMode.FullPage;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Pixel(1000);
            reportViewer.AsyncRendering = true;
            reportViewer.ServerReport.ReportServerUrl = new Uri(reportServerUrl);
            reportViewer.ServerReport.ReportPath = "/" + reportFolder + "/RptPedidoEcommerce";
            reportViewer.ServerReport.ReportServerCredentials = reportCredential;
            ViewBag.ReportViewer = reportViewer;
            ViewBag.Title = "Reporte de Pedidos Ecommerce";
            return View();
        }

        public ActionResult ReporteStocks()
        {
            // ** Procesar Reporte en Remoto
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Remote;
            reportViewer.SizeToReportContent = true;
            reportViewer.ZoomMode = ZoomMode.FullPage;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Pixel(1000);
            reportViewer.AsyncRendering = true;
            reportViewer.ServerReport.ReportServerUrl = new Uri(reportServerUrl);
            reportViewer.ServerReport.ReportPath = "/" + reportFolder + "/RptStockProducto";
            reportViewer.ServerReport.ReportServerCredentials = reportCredential;
            ViewBag.ReportViewer = reportViewer;
            ViewBag.Title = "Reporte de Stocks de Productos";
            return View();
        }


        public ActionResult ReportePedido()
        {
            // ** Procesar Reporte en Remoto
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Remote;
            reportViewer.SizeToReportContent = true;
            reportViewer.ZoomMode = ZoomMode.FullPage;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Pixel(1000);
            reportViewer.AsyncRendering = true;
            reportViewer.ServerReport.ReportServerUrl = new Uri(reportServerUrl);
            reportViewer.ServerReport.ReportPath = "/" + reportFolder + "/RptPedidosCaidos";
            reportViewer.ServerReport.ReportServerCredentials = reportCredential;
            ViewBag.ReportViewer = reportViewer;
            ViewBag.Title = "Reporte de Pedidos Caídos";
            return View();
        }


        public ActionResult ReporteRanking()
        {
            // ** Procesar Reporte en Remoto
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Remote;
            reportViewer.SizeToReportContent = true;
            reportViewer.ZoomMode = ZoomMode.FullPage;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Pixel(1000);
            reportViewer.AsyncRendering = true;
            reportViewer.ServerReport.ReportServerUrl = new Uri(reportServerUrl);
            reportViewer.ServerReport.ReportPath = "/" + reportFolder + "/RptRankingProducto";
            reportViewer.ServerReport.ReportServerCredentials = reportCredential;
            ViewBag.ReportViewer = reportViewer;
            ViewBag.Title = "Reporte de Ranking de Productos";
            return View();
        }

        #endregion


        #region Aplicaciones auxiliares
        // Se usa para la protección XSRF al agregar inicios de sesión externos
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

    }
}