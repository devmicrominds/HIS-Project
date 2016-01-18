using Breeze.ContextProvider.NH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Sym.Web.UI
{
    [BreezeNHController]
    public class ApplicationController : ApiController
    {
        SymDataContext dataContext;

        public ApplicationController(SymDataContext dataContext) {

            this.dataContext = dataContext;
        }

        [HttpGet]
        public string GetSettings() {

            return "Good!";
        }
        
    }
}