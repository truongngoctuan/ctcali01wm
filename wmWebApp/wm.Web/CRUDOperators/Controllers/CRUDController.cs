using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wm.Web.CRUDOperators.ViewModels;
using wm.Web.Models;

namespace wm.Web.CRUDOperators.Controllers
{
    public class CRUDController : Controller
    {
        public ToolboxListViewModel CreateToolboxLinks(int id)
        {
            ToolboxListViewModel listViewModel = new ToolboxListViewModel();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            listViewModel.Details = Url.Action("Details", controllerName, new { id = id });
            listViewModel.Edit = Url.Action("Edit", controllerName, new { id = id });
            listViewModel.Delete = Url.Action("Delete", controllerName, new { id = id });

            return listViewModel;
        }

        public ToolboxListViewModel CreateToolboxLinks(string id)
        {
            ToolboxListViewModel listViewModel = new ToolboxListViewModel();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            listViewModel.Details = Url.Action("Details", controllerName, new { id = id });
            listViewModel.Edit = Url.Action("Edit", controllerName, new { id = id });
            listViewModel.Delete = Url.Action("Delete", controllerName, new { id = id });

            return listViewModel;
        }
    }
}