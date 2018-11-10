using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TmsWebApp.Models;
using TranningWebApp.Repository;
using TmsWebApp.Common;
using System.Web.Security;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using TranningWebApp.Repository.DataAccess;
using System.Threading;
using Newtonsoft.Json.Linq;
using PagedList;
using RestSharp;
using TmsWebApp.HelpingUtilities;
using TranningWebApp.Common;
using TranningWebApp.Controllers;
using TranningWebApp.Resource;

namespace TmsWebApp.Controllers
{

    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        public ActionResult Login(string returnUrl, bool? error)
        {
            if (error != null)
            {
                ViewBag.Error = " " + General.invalidmsg;
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [AllowAnonymous]
        public ActionResult Admin(string returnUrl, bool? error)
        {
            if (error != null)
            {
                ViewBag.Error = " " + General.invalidmsg;
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (Request["button"] != null)
            {
                return RedirectToAction("VolunteerProfile","volunteer");
            } 

            var a = 0;
            var repository = new AccountRepository();
            var user = repository.Get().FirstOrDefault(x => x.Username == model.Username.Trim() && !x.IsLocked);
            if (user == null)
            {
                var participantRepo = new ParticipiantRepository();
                var participant = participantRepo.Get().FirstOrDefault(x => x.NationalID == model.Username && x.isActive);
                if (participant != null)
                    user = participant.user;
            }
            if (user != null)
            {
                var password1 = EncryptionKeys.Decrypt(user.Password);
                var password = EncryptionKeys.Encrypt(model.Password);
                if (user.Password.Equals(password))
                {

                    var role = new RoleRepository().Get(user.RoleId);
                    var enumRole = (EnumUserRole)role.Code;
                    string route = Request.Form["route"];
                    if (route == "manager" && enumRole != EnumUserRole.SuperAdmin)
                    {
                        return RedirectToAction("Admin", new { error = true });
                    }
                    if (route != "manager" && enumRole == EnumUserRole.SuperAdmin)
                    {
                        return RedirectToAction("Login", new { error = true });
                    }
                    if (enumRole == EnumUserRole.Coordinator)
                    {
                    }
                    var cu = new ContextUser
                    {
                        OUser = user,
                        EnumRole = enumRole,
                        Role = role,
                        PhotoPath = "/img/avatars/admin.png"
                    };

                    Session["user"] = cu;
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    //var claims = new List<Claim>();
                    //claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Username));
                    //claims.Add(new Claim(ClaimTypes.Name, user.FirstName));
                    //claims.Add(new Claim(ClaimTypes.Email, user.Email));
                    //claims.Add(new Claim(ClaimTypes.Role, userRole.ToString("g")));
                    //claims.Add(new Claim(ClaimTypes.Sid, user.Id.ToString()));

                    //var id = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                    //var ctx = Request.GetOwinContext();
                    //var authenticationManager = ctx.Authentication;
                    //authenticationManager.SignIn(id);


                    return RedirectToPortal(enumRole, user);
                }
            }

            string route1 = Request.Form["route"];
            if (route1 == "manager")
            {

                return RedirectToAction("Admin", new { error = true });
            }
            if (route1 != "manager")
            {
                return RedirectToAction("Login", new { error = true });
            }

            return View(model);

            //// This doesn't count login failures towards account lockout
            //// To enable password failures to trigger account lockout, change to shouldLockout: true
            //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            //    case SignInStatus.Failure:
            //    default:
            //        ModelState.AddModelError("", "Invalid login attempt.");
            //        return View(model);
            //}


        }

        public ActionResult SignInViaLinkedIn()
        {
            string linkedClientId = ConfigurationManager.AppSettings["ClientIdLinkedIn"];
            string redirectUri = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                                 "/Account/LinkedIn";
            string code = "123456";
            string linkedInUrl = $"https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id={linkedClientId}&redirect_uri={redirectUri}&state={code}";

            var client = new RestClient(linkedInUrl);
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            var content = response.Content;
            var token = JObject.Parse(content);

            return null;
        }

        [AllowAnonymous]
        public ActionResult LinkedIn(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
                return RedirectToAction("Login");
            string redirectUri = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                                 "/Account/LinkedIn";
            var client = new RestClient("https://www.linkedin.com/oauth/v2/accessToken");
            var request = new RestRequest(Method.POST);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", redirectUri);
            request.AddParameter("client_id", ConfigurationManager.AppSettings["ClientIdLinkedIn"]);
            request.AddParameter("client_secret", ConfigurationManager.AppSettings["ClientSecretLinkedIn"]);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            JToken token = JObject.Parse(content);
            string accessToken = (string)token.SelectToken("access_token");
            ////Get Profile Details  
            client = new RestClient("https://api.linkedin.com/v1/people/~:(id,first-name,last-name,email-address,public-profile-url)?oauth2_access_token=" + accessToken + "&format=json");
            request = new RestRequest(Method.GET);
            response = client.Execute(request);
            content = response.Content;
            token = JObject.Parse(content);
            string emailAddress = (string)token.SelectToken("emailAddress");
            string firstName = (string)token.SelectToken("firstName");
            string lastName = (string)token.SelectToken("lastName");
            string id = (string)token.SelectToken("id");
            string profileUrl = (string)token.SelectToken("publicProfileUrl");
            return AfterExternalLoginCallBack(firstName + lastName, emailAddress, id, "LinkedIn", profileUrl, firstName + " " + lastName);

        }


        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {

                var repository = new AccountRepository();
                var user = repository.Get().FirstOrDefault(x => x.Email == model.Email);
                if (user != null)
                {
                    string newPassword = Membership.GeneratePassword(8, 2);
                    user.Password = EncryptionKeys.Encrypt(newPassword);
                    user.FirstLogin = false;


                    string url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Account/Login";
                    var bogusController = Util.CreateController<EmailTemplateController>();
                    EmailTemplateModel emodel = new EmailTemplateModel { Title = "Reset Your Password", RedirectUrl = url, UserName = user.Username, Password = newPassword,User = user.FirstName };
                    string body = Util.RenderViewToString(bogusController.ControllerContext, "ResetPassword", emodel);
                    EmailSender.SendSupportEmail(body, user.Email);

                    repository.Put(user.Id, user);
                    ViewBag.message = General.PasswordResetEmailsent;
                }
                else
                {
                    ViewBag.message = General.Usernotfound;
                    ViewBag.notfounderror = true;
                    return View(model);
                }
            }
          
            model.Email = "";
            
            return RedirectToAction("ResetPasswordConfirmation");

        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            string clinetId = ConfigurationManager.AppSettings["ClientId"];
            string redirectUri = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                                "/Account/AfterValidateGoogle";
            string url = $"https://accounts.google.com/o/oauth2/v2/auth?scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fuserinfo.profile+https://www.googleapis.com/auth/userinfo.email&redirect_uri={redirectUri}&client_id={clinetId}&response_type=code";

            return Redirect(url);

            // Request a redirect to the external login provider
            //return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = "/Account/ExternalLogin" }));
        }

        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult AfterValidateGoogle(string code)
        {
            if (code == "access_denied")
            {
                return RedirectToAction("Login");
            }
            string clinetId = ConfigurationManager.AppSettings["ClientId"];
            string clinetSecrect = ConfigurationManager.AppSettings["ClientSecret"];

            string redirectUri = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                              "/Account/AfterValidateGoogle";

            var client = new RestClient("https://www.googleapis.com/oauth2/v4/token");
            var request = new RestRequest(Method.POST);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", code);
            request.AddParameter("client_id", clinetId);
            request.AddParameter("client_secret", clinetSecrect);
            request.AddParameter("redirect_uri", redirectUri);
            IRestResponse response = client.Execute(request);

            var content = response.Content;
            JToken token = JObject.Parse(content);
            string accessToken = (string)token.SelectToken("access_token");


            client = new RestClient("https://www.googleapis.com/oauth2/v2/userinfo");
            request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + accessToken);
            response = client.Execute(request);
            content = response.Content;
            token = JObject.Parse(content);

            return AfterExternalLoginCallBack((string)token["given_name"], (string)token["email"], (string)token["id"], "Google", (string)token["link"], (string)token["name"]);
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            var loginInfo =  AuthenticationManager.GetExternalLoginInfo();
            if (loginInfo == null)
            {
               // return new ChallengeResult("Google", Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = "/Account/ExternalLogin" }));

                return RedirectToAction("Login");
            }
            string username = loginInfo.DefaultUserName;
            string email = loginInfo.Email;
            string providerKey = loginInfo.Login.ProviderKey;

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var name = loginInfo.ExternalIdentity.Claims.Where(c => c.Type == "displayName")
                   .Select(c => c.Value).SingleOrDefault();
            var url = loginInfo.ExternalIdentity.Claims.Where(c => c.Type == "url")
                   .Select(c => c.Value).SingleOrDefault();

            return AfterExternalLoginCallBack(username, email, providerKey, "Google", url, name);
        }

        public ActionResult AfterExternalLoginCallBack(string username, string email, string providerKey, string provider, string prfileurl, string fullname)
        { 
            var cu = new ContextUser
            {
                OUser = new user
                {
                    Username = username,
                    Email = email
                },
                EnumRole = EnumUserRole.Volunteer,
                GoogleId = provider == "Google" ? providerKey : null,
                LinkedInId = provider == "LinkedIn" ? providerKey : null,
                FullName = fullname,
                ProfileUrl = prfileurl
            };


            FormsAuthentication.SetAuthCookie(username, false);
            var repository = new VolunteerRepository();
            volunteer_profile oVoluntee = null;
            Session["user"] = cu;
            if (provider == "Google")
            {
                oVoluntee = repository.Get().FirstOrDefault(x => x.GoogleSigninId == providerKey);
            }
            else if (provider == "user")
            {
                oVoluntee = repository.Get().FirstOrDefault(x=>x.UserId == int.Parse(providerKey)) ;
            }
            else
            {
                oVoluntee = repository.Get().FirstOrDefault(x => x.LinkedInSignInId == providerKey);
            }
            if (oVoluntee != null)
            {
                cu.OUser.Id = oVoluntee.Id;
                Session["user"] = cu;
                if (oVoluntee.IsApprovedAtLevel1 != null && oVoluntee.IsApprovedAtLevel1.Value
                    && oVoluntee.IsApprovedAtLevel2 != null && oVoluntee.IsApprovedAtLevel2.Value
                    && oVoluntee.IsApprovedAtLevel3 != null && oVoluntee.IsApprovedAtLevel3.Value
                    && !oVoluntee.OTAttendenceForVolunteer)
                {
                    return RedirectToAction("Edit", "Supervisor", new { id = oVoluntee.RowGuid });
                }
                if (oVoluntee.OTAttendenceForVolunteer)
                {
                    return RedirectToAction("Index", "Session");
                }
                return RedirectToAction("VolunteerProfile", "Volunteer");
            }
            return RedirectToAction("VolunteerProfile", "Volunteer");
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Request.GetOwinContext().Authentication.SignOut();
            Session["user"] = null;

            return RedirectToAction("Login");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session["user"] = null;
            Request.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Login");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
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

        private ActionResult RedirectToPortal(EnumUserRole userRole, user user)
        {
            switch (userRole)
            {
                case EnumUserRole.SuperAdmin:
                    if (!user.FirstLogin)
                    {
                        return RedirectToAction("UpdatePassword");
                    }
                    return RedirectToAction("DashBoard", "Home");
                case EnumUserRole.Coordinator:
                    {

                        if (!user.FirstLogin)
                        {
                            return RedirectToAction("UpdatePassword");
                        }
                        var oCordinator = new CoordinatorRepository().GetByUserId(user.Id);
                        if (oCordinator.IsProfileComplete == null || !oCordinator.IsProfileComplete.Value)
                        {
                            return RedirectToAction("CoordinatorProfile", "Coordinator");
                        }
                        var cu = Session["user"] as ContextUser;
                        if (cu != null)
                        {
                            cu.PhotoPath = oCordinator.PhotoPath;
                            Session["user"] = cu;
                        }
                        return RedirectToAction("Index", "Session");
                    }
                case EnumUserRole.Volunteer:
                    {
                        if (!user.FirstLogin)
                        {
                            return RedirectToAction("UpdatePassword");
                        }
                        return AfterExternalLoginCallBack(user.FirstName + user.LastName, user.Email, user.Id.ToString(), "user", "", user.FirstName + " " + user.LastName);
                    }
                case EnumUserRole.Participant:
                    {
                        if (!user.FirstLogin)
                        {
                            return RedirectToAction("UpdatePassword");
                        }
                        var participant = new ParticipiantRepository().GetByUserId(user.Id);

                        if (participant.IsProfileComplete == null || !participant.IsProfileComplete.Value)
                        {
                            return RedirectToAction("ParticipantProfile", "Participant");
                        }
                        var cu = Session["user"] as ContextUser;
                        if (cu != null)
                        {
                            cu.PhotoPath = participant.PhotoPath;
                            Session["user"] = cu;
                        }
                        return RedirectToAction("Index", "Session");
                    }
                case EnumUserRole.Approver1:
                    {
                        if (!user.FirstLogin)
                        {
                            return RedirectToAction("UpdatePassword");
                        }
                        return RedirectToAction("Index", "Supervisor");
                    }
                case EnumUserRole.Approver2:
                    {
                        if (!user.FirstLogin)
                        {
                            return RedirectToAction("UpdatePassword");
                        }
                        return RedirectToAction("Index", "Supervisor");
                    }
                case EnumUserRole.Approver3:
                    {
                        if (!user.FirstLogin)
                        {
                            return RedirectToAction("UpdatePassword");
                        }
                        return RedirectToAction("Index", "Supervisor");
                    }
                case EnumUserRole.Funder:
                    {
                        if (!user.FirstLogin)
                        {
                            return RedirectToAction("UpdatePassword");
                        }
                        return RedirectToAction("Index", "Report");
                    }
                default:
                    return RedirectToAction("Index", "Home");
            }

        }
        public ActionResult UpdatePassword()
        {
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            var partnerid = Request["partnerid"];
            if (partnerid != null)
                TempData["partnerId"] =  partnerid;
            return View(model);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdatePassword(ResetPasswordViewModel model,int? id = null)
        {
            var su = Session["user"] as ContextUser;
            var repository = new AccountRepository();
            
            var partnerid = TempData["partnerid"].ToString();
            if (partnerid != null)
            {
                var FunderUserID = new FunderRepository().GetByRowId(Guid.Parse(partnerid)).FunderUserID;
                var user = repository.Get(FunderUserID);
                user.Password = EncryptionKeys.Encrypt(model.Password);
                user.FirstLogin = true;
                repository.Put(user.Id, user);
                su.OUser = null;
            }
            if (su.OUser != null)
            {
                var user = repository.Get(su.OUser.Id);
                user.Password = EncryptionKeys.Encrypt(model.Password);
                user.FirstLogin = true;
                repository.Put(user.Id, user);
            }
            if (su.EnumRole == EnumUserRole.SuperAdmin)
            {
                return RedirectToAction("DashBoard", "Home");
            }
            if (su.EnumRole == EnumUserRole.Approver1 || su.EnumRole == EnumUserRole.Approver2 || su.EnumRole == EnumUserRole.Approver3)
            {
                return RedirectToAction("Index", "Supervisor");
            }
            if (su.EnumRole == EnumUserRole.Coordinator)
            {
                return RedirectToAction("CoordinatorProfile", "Coordinator");
            }
            if (su.EnumRole == EnumUserRole.Participant)
            {
                return RedirectToAction("ParticipantProfile", "Participant");
            }
            if (su.EnumRole == EnumUserRole.Funder)
            {
                return RedirectToAction("Index", "Report");
            }
            return View("Login");
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
        [AuthorizeUser(AccessLevel = "SuperAdmin")]
        public ActionResult UserList(string sortOrder, string filter, string archived, int page = 1, Guid? archive = null)
        {

            ViewBag.searchQuery = string.IsNullOrEmpty(filter) ? "" : filter;
            ViewBag.showArchived = (archived ?? "") == "on";

            page = page > 0 ? page : 1;
            int pageSize = 0;
            pageSize = pageSize > 0 ? pageSize : 10;

            ViewBag.CurrentSort = sortOrder;

            IEnumerable<user> users;
            var repository = new AccountRepository();
            if (archive != null)
            {
                var oUser = repository.GetByGuid(archive.Value);
                oUser.IsLocked = !oUser.IsLocked;
                repository.Put(oUser.Id, oUser);
            }
            int[] rols = { (int)EnumUserRole.SuperAdmin, (int)EnumUserRole.Approver1, (int)EnumUserRole.Approver2, (int)EnumUserRole.Approver3 };
            if (string.IsNullOrEmpty(filter))
            {
                users = repository.Get().Where(x => rols.Contains(x.lk_role.Code));
            }
            else
            {
                users = repository.Get().Where(x => x.Username.ToLower().Contains(filter.ToLower()) && rols.Contains(x.lk_role.Code));
            }
            //Sorting order
            users = users.OrderByDescending(x => x.CreatedAt);
            ViewBag.Count = users.Count();

            return View(users.ToPagedList(page, pageSize));
        }
        #endregion
        [Authorize]
        public ActionResult EditUser(Guid? id)
        {
            AccountRepository repo = new AccountRepository();
            RoleRepository reporole = new RoleRepository();
            int[] rolesCode = { (int)EnumUserRole.SuperAdmin, (int)EnumUserRole.Approver1, (int)EnumUserRole.Approver2, (int)EnumUserRole.Approver3 };
            user us = null;

            ViewBag.rolesdd = reporole.Get().Where(x => rolesCode.Contains(x.Code)).Select(x =>
                   new SelectListItem { Text = x.FrindlyName, Value = x.Id + "" }
            ).ToList();
            if (id != null)
            {
                us = repo.GetByGuid(id.Value);
            }
            else
            {
                us = new user();
                us.Password = Membership.GeneratePassword(8, 2);
            }
            us.IsLocked = !us.IsLocked;
            return View(us);
        }
        [HttpPost]
        public ActionResult EditUser(user user, HttpPostedFileBase file)
        {
            var cu = Session["user"] as ContextUser;
            AccountRepository repo = new AccountRepository();
            user oUser = null;
            if (user.Id == 0)
            {
                oUser = new user();
                oUser.RowGuid = Guid.NewGuid();
                oUser.CreatedAt = DateTime.Now;
                oUser.CreatedBy = cu.OUser.Id;
               
                oUser.Password = EncryptionKeys.Encrypt(user.Password);
                oUser.RegistrationDate = DateTime.Now;
                

            }
            else
            {
                oUser = repo.Get(user.Id);
                oUser.UpdatedBy = cu.OUser.Id;
                oUser.UpdatedAt = DateTime.Now;

            }

            int[] rolesCode = { (int)EnumUserRole.SuperAdmin, (int)EnumUserRole.Approver1, (int)EnumUserRole.Approver2, (int)EnumUserRole.Approver3 };

            RoleRepository reporole = new RoleRepository();
            ViewBag.rolesdd = reporole.Get().Where(x => rolesCode.Contains(x.Code)).Select(x =>
                new SelectListItem { Text = x.FrindlyName, Value = x.Id + "" }
            ).ToList();

            if (oUser.Username != user.Username && repo.UserExist(user.Username))
            {
                ViewBag.userexist = true;
                return View(user);
            }
            if (oUser.Email != user.Email && repo.EmailExist(user.Email))
            {
                ViewBag.emailexist = true;
                return View(user);
            }
            oUser.Username = user.Username;
            oUser.Email = user.Email;
            oUser.FirstName = user.FirstName;
            oUser.LastName = user.LastName;
            oUser.RoleId = user.RoleId; 
            if (file != null)
            {
                string fileName = "~/Uploads/ImageLibrary/" + Guid.NewGuid() + Path.GetExtension(file.FileName);
                string filePath = Server.MapPath(fileName);
                file.SaveAs(filePath);
                oUser.PhotoPath = fileName;
            }
            if (oUser.Id > 0)
                repo.Put(oUser.Id, oUser);
            else
            { 
                string url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                var bogusController = Util.CreateController<EmailTemplateController>();
                EmailTemplateModel model = new EmailTemplateModel { Title = "Account Registraion ", RedirectUrl = url, UserName = oUser.Username, Password = user.Password,User = user.FirstName };
                string body = Util.RenderViewToString(bogusController.ControllerContext, "UserProfile", model);

                EmailSender.SendSupportEmail(body, oUser.Email);
                repo.Post(oUser);
            }
            return RedirectToAction("UserList");
        }
    }
}