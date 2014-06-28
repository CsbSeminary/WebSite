using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

using Csbs.Web;
using Csbs.Web.UI;
using System.Web.UI.WebControls;

using Csbs.Data;
using Csbs.Utilities;
using Csbs.XmlStorage;

using ContactFormData = Csbs.XmlStorage.ContactFormXml.ContactFormData;

namespace Csbs
{
    public partial class ContactUs :System.Web.UI.Page
    {
        #region Constants

        private const String FormNameQueryStringKey = "form";
        private const String AllowSendEmailSessionKey = "CSBS.ContactUs.AllowSendEmail";

        #endregion

        #region Properties

        private String FormName
        {
            get { return Request.QueryString[FormNameQueryStringKey]; }
        }

        private Boolean AllowSendEmail
        {
            get { return Session[AllowSendEmailSessionKey] == null || (Boolean)Session[AllowSendEmailSessionKey]; }
            set { Session[AllowSendEmailSessionKey] = value; }
        }

        #endregion

        #region Initialization

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SubmitButton.Click += SubmitButton_Click;

            InitPageContent();
        }

        private void InitPageContent()
        {
            RequestForm.Visible = false;
            ErrorMessage.Visible = false;
            ThanksMessage.Visible = false;

            ContactFormData item = ContactFormXml.Current.GetItemByID(FormName);

            if (item != null)
            {
                if (AllowSendEmail)
                {
                    DisplayContent("Send us an E-mail", item.Title, item.Description);
                    RequestForm.Visible = true;
                }
                else
                {
                    DisplayContent("Confirmation", "Thank You. Your e-mail was sent.", null);
                    ThanksMessage.Visible = true;
                    AllowSendEmail = true;
                }
            }
            else
            {
                ShowError("Contact form not found!");
            }

            RenderMenu();
        }

        #endregion

        #region Event handlers

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (!AllowSendEmail)
            {
                AllowSendEmail = true;
                return;
            }

            try
            {
                SendEmail();

                AllowSendEmail = false;

                Response.Redirect(Request.Url.PathAndQuery);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message.Replace("\r\n", "<br >"));
            }
        }

        #endregion

        #region Send email

        private void SendEmail()
        {
            ContactFormData item = ContactFormXml.Current.GetItemByID(FormName);

            EmailItem emailItem = new EmailItem();

            emailItem.Subject = String.IsNullOrEmpty(Settings.FeedbackEmail.Subject) ? Subject.Text : Settings.FeedbackEmail.Subject;
            emailItem.From = String.IsNullOrEmpty(Settings.FeedbackEmail.SenderEmail) ? EmailAddress.Text : Settings.FeedbackEmail.SenderEmail;
            emailItem.To = String.IsNullOrEmpty(Settings.FeedbackEmail.RecipientEmail) && item != null ? item.Email : Settings.FeedbackEmail.RecipientEmail;
            emailItem.Body = GetEmailBody(item);

            EmailSender.Send(emailItem);
        }

        #endregion

        #region Helper methods

        private void RenderMenu()
        {
            String[] defaultItems = Settings.Forms.ContactUs.MenuItems;

            if (defaultItems == null)
                return;

            List<Object> menuItems = new List<Object>(defaultItems);
            menuItems.Add(ContactFormXml.Current.GetMenuInfo());

            StringBuilder sb = new StringBuilder();

            MenuManager.Current.Render(sb, menuItems, null, null, MenuRenderingType.Vertical);

            MenuHolder.Controls.Add(new LiteralControl(sb.ToString()));
        }

        private String GetEmailBody(ContactFormData item)
        {
            StringBuilder body = new StringBuilder(FileManager.ReadEmailTemplate("ContactUs.html"));

            body.Replace("$FormName", item != null ? item.Name : String.Empty);
            body.Replace("$Subject", Subject.Text);
            body.Replace("$FirstName", FirstName.Text);
            body.Replace("$LastName", LastName.Text);
            body.Replace("$EmailAddress", EmailAddress.Text);
            body.Replace("$Message", Message.Text.Replace("\r", "").Replace("\n", "<br/>"));
            body.Replace("$DateUTC", DateTime.UtcNow.ToString());
            body.Replace("$Date", DateTime.Now.ToString());            

            return body.ToString();
        }

        private void ShowError(String message)
        {
            DisplayContent("Send us an E-mail", "Error", null);

            ErrorMessage.Controls.Clear();
            ErrorMessage.Controls.Add(new LiteralControl(String.Format("<p><div style=\"height: 439px;\">{0}</div></p>", message)));

            ErrorMessage.Visible = true;
            RequestForm.Visible = false;
            ThanksMessage.Visible = false;
        }

        private void DisplayContent(String headerTitle, String title, String description)
        {
            Page.Title = title;

            PageTopTitle.Controls.Clear();
            PageTopTitle.Controls.Add(new LiteralControl(headerTitle));

            ContentTitle.Controls.Clear();
            ContentTitle.Controls.Add(new LiteralControl(title));

            ContentDescription.Controls.Clear();
            ContentDescription.Controls.Add(new LiteralControl(description));
        }

        #endregion
    }
}