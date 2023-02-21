using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using IdentityServer.Application.Models;
using WebApiDemo.Common.Notification;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Web;
using IdentityModel.Client;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using IdentityServer.Application;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using IdentityServer4.EntityFramework.Entities;
using IdentityModel;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeprovider;
        private readonly IEventService _events;
        private readonly IEmailService _emailservice;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IdentityServerSettings _optionsCommon;

        public AccountController(IIdentityServerInteractionService interaction,
                                 IClientStore clientStore,
                                 IAuthenticationSchemeProvider schemeprovider,
                                 IEventService events,
                                 IOptions<IdentityServerSettings> optionscommon,
                                 IEmailService emailservice,
                                 SignInManager<ApplicationUser> signInManager)
        {
            _interaction = interaction;
            _clientStore= clientStore;
            _schemeprovider= schemeprovider;
            _events= events;
            _emailservice= emailservice;
            _signInManager= signInManager;
            _optionsCommon = optionscommon.Value ?? throw new ArgumentNullException(nameof(optionscommon));
        }
        //[HttpPost]
        //[AllowAnonymous]
        //[Route("LogIn")]
        //public async Task<IActionResult> LogIn(LoginUserModel model)
        //{
        //    var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);
        //    if (user != null)
        //    {
        //        var userLogin = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        //        if(userLogin== Microsoft.AspNetCore.Identity.SignInResult.Success)
        //        {
        //            return Ok(user);
        //        }

        //    }
        //    return Unauthorized("Incorrect Email address or password");
        //}
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserModel model, CancellationToken cancellationToken = default)
        {
            var client = new HttpClient();
            var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);
            if(user is null)
                return BadRequest("Incorrect Email address");
            var disco = await client.GetDiscoveryDocumentAsync(_optionsCommon.IdentityDiscoveryUrl);
            if (disco.IsError) throw new Exception(disco.Error);
            var response = await new HttpClient().RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = _optionsCommon.IdentityClientId,
                ClientSecret = _optionsCommon.IdentityClientPassword,
                Scope = _optionsCommon.IdentityScope,

                UserName=user.UserName,
                Password = model.Password
            });
            if (response.IsError)
                return BadRequest("Incorrect Email address or password");
            else
            {
                var data = JsonConvert.DeserializeObject<dynamic>(response.Json.ToString());
                var access_token = (string)data.access_token;
                var refresh_token = (string)data.refresh_token;
                var expires_in = (string)data.expires_in;
                var res = new
                {
                    access_token,
                    refresh_token,
                    expires_in,
                };
                return Ok(res);
            }
           

        }
        [HttpGet("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                await HttpContext.SignOutAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        
        }

        [HttpGet("ExternalLogin")]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account",new {ReturnUrl=returnUrl});
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            var result= new ChallengeResult(provider, properties);
            return result;
        }

        [HttpGet("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl=null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return BadRequest();
            var user = await _signInManager.UserManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    EmailConfirmed = true,
                };

                var identResult = await _signInManager.UserManager.CreateAsync(user,user.Id.Sha256());
                //await _signInManager.UserManager.AddClaimAsync(user, new System.Security.Claims.Claim(JwtClaimTypes.Email, user.Email));
                if (identResult.Succeeded)
                {
                    identResult = await _signInManager.UserManager.AddLoginAsync(user, info);
                    if (identResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                    }
                }
            }
            //var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            var tokenEndPoint = _optionsCommon.IdentityDiscoveryUrl + "/connect/token";
            var response = await new HttpClient().RequestPasswordTokenAsync(new PasswordTokenRequest()
            {
                Address= tokenEndPoint,
                ClientId=_optionsCommon.IdentityClientId,
                ClientSecret=_optionsCommon.IdentityClientPassword,
                Scope=_optionsCommon.IdentityScope,
                UserName=user.Email,
                Password=user.Id.ToSha256()

            });
            if (response.IsError)
                return BadRequest("Incorrect client credentials");
            var data = JsonConvert.DeserializeObject<dynamic>(response.Json.ToString());
            var access_token = (string)data.access_token;
            var refresh_token = (string)data.refresh_token;
            var expires_in = (string)data.expires_in;
            var returnUri = returnUrl + $"?access_token={access_token}&refresh_token={refresh_token}&expires_in={expires_in}";
         
            return Redirect(returnUri);
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserModel model)
        {
            var userbyemail = await _signInManager.UserManager.FindByEmailAsync(model.Email);
            if (userbyemail is not null)
                return BadRequest("User with this email address already exists");
            var user = await _signInManager.UserManager.FindByNameAsync(model.UserName);
            if (user is null)
            {
                user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber=model.PhoneNumber,
                    EmailConfirmed = true,
                };
                var result = await  _signInManager.UserManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                return Ok(result);
            }
            else
                return BadRequest("The username already exists");
        }
        [HttpGet]
        [Route("SendLinkResetPassword")]
        public async Task<IActionResult> SendLinkResetPassword(string email,string url)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(email);
            if (user == null) { return BadRequest("User with this email address does not exist"); }
            var token = await  _signInManager.UserManager.GeneratePasswordResetTokenAsync(user);
            var encodedtoken=HttpUtility.UrlEncode(token);
            var link = url + $"?token={encodedtoken}&email={email}";
            var message = System.IO.File.ReadAllText("Views/ResetPassword.html");
            message = message.Replace("#linkreset", link.ToString());
            bool send=await  _emailservice.SendEmailAsync(new WebApiDemo.Common.Models.MessageEmail()
                       {
                           EmailTo= user.Email,
                           Subject="Reset Password",
                           Content=message
                       });
            if (send)
            {
                return Ok();
            }
            else
                return BadRequest("Email not sent");
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(x=>x.Errors));
            var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);
            if (user is null)
                return BadRequest("User does not exist");
            var result = await  _signInManager.UserManager.ResetPasswordAsync(user,model.Token,model.NewPassword);
            if (result.Succeeded)
                return Ok();
            else return BadRequest();
        }
        [HttpGet]
        [Route("SendSecurityCode")]
        public async Task<bool> SendSecurityCode(string email)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(email);
            if (user is null)
                return false;
            var code = await _signInManager.UserManager.GenerateTwoFactorTokenAsync(user,"Email");
            var message = System.IO.File.ReadAllText("Views/SecurityCodeContent.html");
            message = message.Replace("#SecurityCode",code);
            bool send = await _emailservice.SendEmailAsync(new WebApiDemo.Common.Models.MessageEmail()
            {
                EmailTo = user.Email,
                Subject = "Sign in with security code",
                Content = message
            });
            return send;
        }
        [HttpPost]
        [Route("SigninViaSecurityCode")]
        public async Task<IActionResult> SigninViaSecurityCode(SignInSecurityCodeModel model)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);
            if (user is null)
                return BadRequest("User with this email does not exist");
            var isvalid = await _signInManager.UserManager.VerifyTwoFactorTokenAsync(user,"Email",model.Code);
            if (isvalid)
                return Ok();
            else
                return BadRequest();
        }
        //private JwtSecurityToken GetToken(List<Claim> authClaims)
        //{
        //    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        //    var token = new JwtSecurityToken(
        //        issuer: _configuration["JWT:ValidIssuer"],
        //        audience: _configuration["JWT:ValidAudience"],
        //        expires: DateTime.Now.AddHours(3),
        //        claims: authClaims,
        //        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        //        );

        //    return token;
        //}
    }
}
