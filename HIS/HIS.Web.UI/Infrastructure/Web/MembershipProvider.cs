using Autofac;
using Autofac.Integration.Web;
using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace HIS.Web.UI
{
    public class AppMembershipProvider : MembershipProvider
    {
        private readonly MD5SaltEncryption md5SaltEncryption;
        private readonly IRepositoryFactory factory;

        public AppMembershipProvider(IRepositoryFactory factory,MD5SaltEncryption md5SaltEncryption)
        {
            this.md5SaltEncryption = md5SaltEncryption;
            this.factory = factory;
        }

        /*
        public AppMembershipProvider(IRepositoryFactory factory, MD5SaltEncryption md5SaltEncryption)
        {

            this.factory = factory;
            this.md5SaltEncryption = md5SaltEncryption;

        }
        * */

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            

            status = MembershipCreateStatus.Success;
            return new MembershipUser("customProvider", username, Guid.NewGuid(), email, passwordQuestion, "", true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, new DateTime(1980, 1, 1));
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return true;
        }

        public override string GetPassword(string username, string answer)
        {
            //return (from u in repository
            //        where u.UserName == username
            //        select u.Password).First();
            return "";
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            //var u = repository.First(x => x.UserName == username && x.Password == oldPassword);
            //u.Password = newPassword;
            return true;
        }

        public override string ResetPassword(string username, string answer)
        {
            return Guid.NewGuid().ToString();
        }

        public override void UpdateUser(MembershipUser user)
        {
        }

        /*
        private Users GetUser(string username, ParameterRevision revision)
        {

            var container = factory.GetRepositoryInstance<StaffParameterContainerRepository>().GetQueryable()
                                    .Where(x => x.RevisionNo == revision.RevisionNo)
                                    .Where(x => x.RevisionDateTime == revision.RevisionDateTime)
                                    .FirstOrDefault();

            return container.GetParameterByType<StaffParameters>().UserList.FirstOrDefault(x => x.StaffId == username);

        }

        private UserAuthentication GetUserAuthentication(string userName)
        {

            var userAuth = factory.GetRepositoryInstance<UserAuthenticationRepository>().GetQueryable()
                                    .Where(x => x.StaffId == userName)
                                    .FirstOrDefault();

            return userAuth;

        }
         * */

        public override bool ValidateUser(string username, string password)
        {

            var repository = factory.GetRepository<Repository<Users>>();

            var user = repository.GetQueryable().Where(o => o.UserName == username)
                      .FirstOrDefault();

            if (null == user)
            {
                return false;
            }

            //UserList staff = repository.Single(new UserListSpec().ByUserId(username));
            /*
            var revisions = factory.GetRepositoryInstance<ParameterRevisionRepository>();
            var queryable = revisions.GetQueryable();
            var oSRevision = queryable.FirstOrDefault(x => x.ParameterKind == DataLibrary.ParameterKind.Staff);

            var staff = GetUser(username, oSRevision);

            if (null == staff)
            {
                return false;
            }
            else
            {

                // checks if user is online
                var authentication = GetUserAuthentication(username);

                //if (authentication.IsOnline)
                //{
                //    return false;
                //}

                bool authentic = md5SaltEncryption.Verify(username + password, authentication.PasswordHash);

                if (!authentic)
                {

                    return false;
                }
                * */

            FormsAuthentication.SetAuthCookie(username, true);
                HttpCookie cookie = FormsAuthentication.GetAuthCookie(username, true);
                FormsAuthenticationTicket auth = FormsAuthentication.Decrypt(cookie.Value);

                // Store UserData inside the Forms Ticket with all the attributes in sync with the web.config
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(auth.Version,
                                                                                    auth.Name,
                                                                                    auth.IssueDate,
                                                                                    auth.Expiration,
                                                                                    true, // always persistent
                                                                                    "",//staff.GetPrivileges(),  // RoleId
                                                                                    auth.CookiePath);


                cookie.Value = FormsAuthentication.Encrypt(ticket);
                cookie.Expires = ticket.Expiration;
                HttpContext.Current.Response.Cookies.Set(cookie);
                //GetSessionUserRoles(staff);
                //authentication.IsOnline = true;

          
                return true;

            //}
        }

        /*
        private void GetSessionUserRoles(Users staff)
        {

            ILifetimeScope scope = (ILifetimeScope)HttpContext.Current.Session["sessionScope"];

            if (null != scope)
            {
                var sessionProvider = scope.Resolve<ISessionProvider>();
                if (null != sessionProvider)
                {
                    var roles = staff.GetPrivileges();
                    sessionProvider["userroles"] = roles;
                }

            }


        }
         * */

        public override bool UnlockUser(string userName)
        {
            return true;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return null;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return null;
        }

        public override string GetUserNameByEmail(string email)
        {
            return null;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            return true;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            return 0;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }

        public override bool EnablePasswordReset
        {
            get { return false; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        public override string ApplicationName { get; set; }

        public override int MaxInvalidPasswordAttempts
        {
            get { return 10; }
        }

        public override int PasswordAttemptWindow
        {
            get { return 10; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return false; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Clear; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 1; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return null; }
        }

    }

    public abstract class AutofacMembershipProvider : MembershipProvider
    {
        private string providerId;

        public abstract ILifetimeScope GetContainer();

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);
            providerId = config["providerId"];
            if (string.IsNullOrWhiteSpace(providerId))
                throw new Exception("Please configure the providerId from the membership provider " + name);
        }

        public MembershipProvider GetProvider()
        {
            try
            {
                var provider = GetContainer().ResolveKeyed<MembershipProvider>(providerId);

                if (provider == null)
                    throw new Exception(string.Format("Component '{0}' does not inherit MembershipProvider", providerId));
                return provider;
            }
            catch (Exception e)
            {
                throw new Exception("Error resolving MembershipProvider " + providerId, e);
            }
        }

        private T WithProvider<T>(Func<MembershipProvider, T> f)
        {
            var provider = GetProvider();
            try
            {
                return f(provider);
            }
            finally
            {
                //GetContainer().Disposer.AddInstanceForDisposal((IDisposable)provider);
            }
        }

        private void WithProvider(Action<MembershipProvider> f)
        {
            var provider = GetProvider();
            try
            {
                f(provider);
            }
            finally
            {
                //GetContainer().Disposer.AddInstanceForDisposal((IDisposable)provider);
            }
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            var provider = GetProvider();
            try
            {
                return provider.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
            }
            finally
            {
                //GetContainer().Disposer.AddInstanceForDisposal((IDisposable)provider);
            }
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return WithProvider(p => p.ChangePasswordQuestionAndAnswer(username, password, newPasswordAnswer, newPasswordAnswer));
        }

        public override string GetPassword(string username, string answer)
        {
            return WithProvider(p => p.GetPassword(username, answer));
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return WithProvider(p => p.ChangePassword(username, oldPassword, newPassword));
        }

        public override string ResetPassword(string username, string answer)
        {
            return WithProvider(p => p.ResetPassword(username, answer));
        }

        public override void UpdateUser(MembershipUser user)
        {
            WithProvider(p => p.UpdateUser(user));
        }

        public override bool ValidateUser(string username, string password)
        {
            return WithProvider(p => p.ValidateUser(username, password));
        }

        public override bool UnlockUser(string userName)
        {
            return WithProvider(p => p.UnlockUser(userName));
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return WithProvider(p => p.GetUser(providerUserKey, userIsOnline));
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return WithProvider(p => p.GetUser(username, userIsOnline));
        }

        public override string GetUserNameByEmail(string email)
        {
            return WithProvider(p => p.GetUserNameByEmail(email));
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            return WithProvider(p => p.DeleteUser(username, deleteAllRelatedData));
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var provider = GetProvider();
            try
            {
                return provider.GetAllUsers(pageIndex, pageSize, out totalRecords);
            }
            finally
            {
                //GetContainer().Release(provider);
            }
        }

        public override int GetNumberOfUsersOnline()
        {
            return WithProvider(p => p.GetNumberOfUsersOnline());
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var provider = GetProvider();
            try
            {
                return provider.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);
            }
            finally
            {
                //GetContainer().Release(provider);
            }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var provider = GetProvider();
            try
            {
                return provider.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords);
            }
            finally
            {
                //GetContainer().Release(provider);
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return WithProvider(p => p.EnablePasswordRetrieval); }
        }

        public override bool EnablePasswordReset
        {
            get { return WithProvider(p => p.EnablePasswordReset); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return WithProvider(p => p.RequiresQuestionAndAnswer); }
        }

        public override string ApplicationName { get; set; }

        public override int MaxInvalidPasswordAttempts
        {
            get { return WithProvider(p => p.MaxInvalidPasswordAttempts); }
        }

        public override int PasswordAttemptWindow
        {
            get { return WithProvider(p => p.PasswordAttemptWindow); }
        }

        public override bool RequiresUniqueEmail
        {
            get { return WithProvider(p => p.RequiresUniqueEmail); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return WithProvider(p => p.PasswordFormat); }
        }

        public override int MinRequiredPasswordLength
        {
            get { return WithProvider(p => p.MinRequiredPasswordLength); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return WithProvider(p => p.MinRequiredNonAlphanumericCharacters); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return WithProvider(p => p.PasswordStrengthRegularExpression); }
        }
    }

    public class WebMembershipProvider : AutofacMembershipProvider
    {
        public override ILifetimeScope GetContainer()
        {
            var context = HttpContext.Current;
            if (context == null)
                throw new Exception("No HttpContext");

            var accessor = context.ApplicationInstance as IContainerProviderAccessor;
            if (accessor == null)
                throw new Exception("The global HttpApplication instance needs to implement " + typeof(IContainerProviderAccessor).FullName);
            if (accessor.ContainerProvider.ApplicationContainer == null)
                throw new Exception("HttpApplication has no container initialized");
            return accessor.ContainerProvider.ApplicationContainer;
        }
    }
}