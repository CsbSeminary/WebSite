using System;
using System.Web.Security;
using System.Web.UI;

namespace Csbs.Web.UI
{
    public partial class UserSignIn : UserControl
    {
        #region Constants

        private const Int32 MaxFailedAttemptCounts = 5;

        private const String FailedAttemptCountSessionKey = "FailedAttemptCount";

        #endregion

        #region Properties

        public Int32 FailedAttemptCount
        {
            get
            {
                if (Page.Session[FailedAttemptCountSessionKey] == null)
                    Page.Session[FailedAttemptCountSessionKey] = 0;

                return (Int32)Page.Session[FailedAttemptCountSessionKey];
            }
            set { Page.Session[FailedAttemptCountSessionKey] = value; }
        }

        #endregion

        #region Initialization

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SubmitCommand.Click += SubmitCommand_Click;
        }

        #endregion

        #region Loading

        protected void Page_Load(Object sender, EventArgs e)
        {
            CheckFailedAttemptCount();

            if (!Page.IsPostBack)
                UserName.Focus();
        }

        #endregion

        #region Event handlers

        void SubmitCommand_Click(Object sender, System.EventArgs e)
        {
            if (FormsAuthentication.Authenticate(UserName.Text, UserPassword.Text))
            {
                FailedAttemptCount = 0;

                FormsAuthentication.RedirectFromLoginPage(UserName.Text, false);
            }
            else
            {
                FailedAttemptCount++;

                ErrorMessage.InnerHtml = "Incorrect sign-in name or password. Please re-enter your name and password.";

                CheckFailedAttemptCount();
            }
        }

        #endregion

        #region Helpers

        private void CheckFailedAttemptCount()
        {
            if (FailedAttemptCount >= MaxFailedAttemptCounts)
            {
                SubmitCommand.Visible = false;
                ErrorMessage.InnerHtml = String.Format("To prevent brute-force attacks on this system, the Submit button is removed from this page after {0} failed attempts to sign in. Please close your web browser and restart it to try again.", MaxFailedAttemptCounts);
            }
        }

        #endregion
    }
}