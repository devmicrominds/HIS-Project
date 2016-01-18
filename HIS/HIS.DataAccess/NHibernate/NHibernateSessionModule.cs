using System;
using System.Configuration;
using System.Web;
using NHibernate.Context;
using NHibernate;
using HIS.DataAccess.Session;

namespace HIS.DataAccess
{
    /// <summary>
    /// Implements the Open-Session-In-View pattern using <see cref="NHibernateSessionManager" />.
    /// Inspiration for this class came from Ed Courtenay at 
    /// http://sourceforge.net/forum/message.php?msg_id=2847509.
    /// </summary>
    public class NHibernateSessionModule : IHttpModule
    {
        OpenSessionInViewSection openSessionInViewSection = GetOpenSessionInViewSection();

        private HttpApplication context;

        public void Init(HttpApplication context) {
            Console.WriteLine("Init!");

            this.context = context;
            context.BeginRequest += new EventHandler(BeginTransaction);
            context.EndRequest += new EventHandler(CloseSession);
        }

        /// <summary>
        /// Opens a session within a transaction at the beginning of the HTTP request.  Note that 
        /// it ONLY begins transactions for those designated as being transactional.
        /// </summary>
        /// <param name="sender"></param>
        private void BeginTransaction(object sender, EventArgs e) {

            //DoSessionPerRequestOn(() =>
            //{
            //    foreach (SessionFactoryElement sessionFactorySettings in openSessionInViewSection.SessionFactories)
            //    {

            //        if (sessionFactorySettings.IsTransactional)    {
            //            System.Diagnostics.Debug.WriteLine("Open New Session");
            //            NHibernateSessionManager.Instance.GetSessionFrom(sessionFactorySettings.FactoryConfigPath);
            //        }
            //    }
            //});
        }

       

        private void DoSessionPerRequestOn(Action Method) 
        {
            try
            {
                //switch (context.Request.CurrentExecutionFilePathExtension.ToLower())
                //{
                //    case ".aspx":
                //    case ".ashx":
                //    case ".ascx":
                // Method();
                //        break;
                //}
            }
            catch (Exception ex) 
            { 
            
            }
           
        }
    

        /// <summary>
        /// Commits and closes the NHibernate session provided by the supplied <see cref="NHibernateSessionManager"/>.
        /// Assumes a transaction was begun at the beginning of the request; but a transaction or session does
        /// not *have* to be opened for this to operate successfully.
        /// </summary>
        private void CloseSession(object sender, EventArgs e) {

            try
            {
                // Commit every session factory that's holding a transactional session  
            
                foreach (SessionFactoryElement sessionFactorySettings in openSessionInViewSection.SessionFactories)
                {
                    if (sessionFactorySettings.IsTransactional)
                    {

                        NHibernateSessionManager.Instance.CommitTransactionOn(sessionFactorySettings.FactoryConfigPath);
                    }
                }

         
            }
            catch (Exception ex) 
            { 
            

            }
            finally {
                // No matter what happens, make sure all the sessions get closed
               
                    foreach (SessionFactoryElement sessionFactorySettings in openSessionInViewSection.SessionFactories)
                    {
                        NHibernateSessionManager.Instance.CloseSessionOn(sessionFactorySettings.FactoryConfigPath);
                    }
                   // System.Diagnostics.Debug.WriteLine("Close Session!");
              
                
            }
        }

        private static void EndSession(ISession session)
        {
            if (session.Transaction != null && session.Transaction.IsActive)
            {
                session.Transaction.Commit();
            }
            session.Dispose();
        }


        private static OpenSessionInViewSection GetOpenSessionInViewSection() {
            OpenSessionInViewSection openSessionInViewSection = ConfigurationManager
                .GetSection("nhibernateSettings") as OpenSessionInViewSection;

            Check.Ensure(openSessionInViewSection != null,
                "The nhibernateSettings section was not found with ConfigurationManager.");
            return openSessionInViewSection;
        }

        public void Dispose() { }
    }
}
