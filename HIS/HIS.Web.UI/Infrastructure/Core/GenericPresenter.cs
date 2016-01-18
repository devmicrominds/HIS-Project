using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI 
{
    public class GenericPresenter<TView> : Presenter 
        where TView : class,IView
    {
        public TView View
        {
            get { return base.ViewInternal as TView; }
        }
    }
}