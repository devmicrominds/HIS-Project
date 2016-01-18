 
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace HIS.Web.UI
{
    public class WebSessionProvider : DynamicObject, ISessionProvider
    {

        private HttpSessionState Session
        {

            get { return HttpContext.Current.Session; }
        }

        public void Add(string name, object value)
        {

            Session.Add(name, value);
        }

        public void Clear()
        {
            Session.Clear();
        }

        public bool Contains(string name)
        {
            return Session[name] != null;
        }

        public void Remove(string name)
        {

            Session.Remove(name);
        }

        #region ISessionProvider Members

        public dynamic this[string name]
        {
            get
            {

                return Session[name];
            }
            set
            {
                Session[name] = value;
            }
        }

        public dynamic this[int index]
        {
            get
            {
                return Session[index];
            }
            set
            {
                Session[index] = value;
            }
        }

        #endregion

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            Session[binder.Name] = true;
            return true;

        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = Session[binder.Name];
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            int index = (int)indexes[0];
            result = Session[index];
            return result != null;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            int index = (int)indexes[0];
            Session[index] = value;
            return true;
        }


        public WebSessionProvider()
        {

            this.Session.Timeout = 999;
        }
    }
}