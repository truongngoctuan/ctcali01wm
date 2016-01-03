using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wm.Web.Models;

namespace wm.Web.Controllers
{
    public class ExtendedController: Controller
    {
        public _ToolboxListViewModel CreateToolboxLinks(int id)
        {
            _ToolboxListViewModel listViewModel = new _ToolboxListViewModel();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            listViewModel.Details = Url.Action("Details", controllerName, new { id = id });
            listViewModel.Edit = Url.Action("Edit", controllerName, new { id = id });
            listViewModel.Delete = Url.Action("Delete", controllerName, new { id = id });

            return listViewModel;
        }
    }
}