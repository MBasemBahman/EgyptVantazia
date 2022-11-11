using API.Areas.UserArea.Models;
using Entities.CoreServicesModels.AccountModels;
using Entities.DBModels.AccountModels;
using Entities.ServicesModels;
using BC = BCrypt.Net.BCrypt;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Authentication")]
    [Route("v{version:apiVersion}/[controller]")]
    public class AuthenticationController : ExtendControllerBase
    {
        private readonly IAuthenticationManager _authManager;
        private readonly IEmailSender _emailSender;

        public AuthenticationController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IAuthenticationManager authManager,
        IEmailSender emailSender,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        {
            _authManager = authManager;
            _emailSender = emailSender;
        }

        [HttpPost]
        [Route(nameof(Login))]
        [AllowAnonymous]
        public async Task<UserDto> Login([FromBody] UserForAuthenticationDto model)
        {
            model.UserName = RegexService.GetUserName(model.UserName);

            UserAuthenticatedDto auth = await _authManager.Authenticate(model, IpAddress());

            SetToken(auth.TokenResponse);
            SetRefresh(auth.RefreshTokenResponse);

            UserDto usersDto = _mapper.Map<UserDto>(auth);

            return usersDto;
        }

        [HttpPost]
        [Route(nameof(RegisterAnonymouse))]
        [AllowAnonymous]
        public async Task<UserDto> RegisterAnonymouse(
          [FromBody] UserForAnonymouseDto model)
        {
            string password = "123456";

            if ((await _unitOfWork.User.FindByUserName(model.UserName, trackChanges: false)) == null)
            {
                User user = _mapper.Map<User>(new UserForRegistrationDto
                {
                    UserName = model.UserName,
                    Culture = model.Culture,
                    EmailAddress = "Anonymouse@mail.com",
                    Name = "Anonymouse",
                    Password = password
                });
                await _unitOfWork.User.CreateUser(user);
                await _unitOfWork.Save();
            }

            UserAuthenticatedDto auth = await _authManager.Authenticate(new UserForAuthenticationDto
            {
                UserName = model.UserName,
                Password = password,
            }, IpAddress());

            SetToken(auth.TokenResponse);
            SetRefresh(auth.RefreshTokenResponse);

            UserDto usersDto = _mapper.Map<UserDto>(auth);

            return usersDto;
        }

        [HttpPost]
        [Route(nameof(RegisterUserWithAccount))]
        [AllowAnonymous]
        public async Task<UserDto> RegisterUserWithAccount(
           [FromBody] UserWithAccountForRegistrationDto model)
        {
            if (!RegexService.ValidateEmail(model.User.EmailAddress))
            {
                throw new Exception("Email address not valid!");
            }

            model.User.UserName = RegexService.GetUserName(model.User.UserName);

            User user = _mapper.Map<User>(model.User);

            model.Account.FullName = model.Account.FullName.IsExisting() ? model.Account.FullName : model.User.Name;

            Account account = _mapper.Map<Account>(model.Account);

            do
            {
                account.RefCode = RandomGenerator.GenerateString(8);
            } while (_unitOfWork.Account.GetAccounts(new AccountParameters { RefCode = account.RefCode }, otherLang: false).Any());

            user.Account = account;

            await _unitOfWork.User.CreateUser(user);
            await _unitOfWork.Save();

            await _unitOfWork.Account.CreateAccountRefCode(model.Account.RefCode, user.Account.Id);
            await _unitOfWork.Save();

            UserAuthenticatedDto auth = await _authManager.Authenticate(new UserForAuthenticationDto
            {
                UserName = model.User.UserName,
                Password = model.User.Password,
            }, IpAddress());

            SetToken(auth.TokenResponse);
            SetRefresh(auth.RefreshTokenResponse);

            UserDto usersDto = _mapper.Map<UserDto>(auth);

            return usersDto;
        }

        [HttpPost]
        [Route(nameof(CheckNotRegistered))]
        [AllowAnonymous]
        public async Task<bool> CheckNotRegistered(
           [FromBody] UserForResetDto model)
        {
            User user = await _unitOfWork.User.FindByUserName(model.UserName, trackChanges: false);

            return user != null ? throw new Exception("Account already registered!") : true;
        }

        [HttpPost]
        [Route(nameof(ResetUser))]
        [AllowAnonymous]
        public async Task<string> ResetUser(
           [FromBody] UserForResetDto model)
        {
            model.UserName = RegexService.GetUserName(model.UserName);

            User user = await _unitOfWork.User.FindByUserName(model.UserName, trackChanges: true);

            if (user == null)
            {
                throw new Exception("Not Found!");
            }

            Verification verification = _unitOfWork.User.GenerateVerification(IpAddress(), _appSettings.VerificationTTL);

            user.Verifications = new List<Verification>
            {
                verification
            };

            //await SendVerification(user.EmailAddress, user.Verifications.Single().Code);

            await _unitOfWork.Save();

            return verification.Code;
        }

        [HttpPost]
        [Route(nameof(VerificateUser))]
        [AllowAnonymous]
        public async Task<bool> VerificateUser(
           [FromBody] UserForVerificationDto model)
        {
            User user = await _unitOfWork.User.Verificate(model, _appSettings.VerificationTTL);

            user.Password = BC.HashPassword(model.Password);

            await _unitOfWork.Save();

            return true;
        }

        [HttpPut]
        [Route(nameof(ChangePassword))]
        public async Task<bool> ChangePassword(
           [FromBody] ChangePasswordDto model)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            await _unitOfWork.User.ChangePassword(auth.Id, model);
            await _unitOfWork.Save();

            return true;
        }

        [HttpPut]
        [Route(nameof(UpdateUserWithAccount))]
        public async Task<bool> UpdateUserWithAccount(
          [FromBody] UserWithAccountForEditDto model)
        {
            if (!RegexService.ValidateEmail(model.User.EmailAddress))
            {
                throw new Exception("Email address not valid!");
            }

            model.User.UserName = RegexService.GetUserName(model.User.UserName);

            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            if (auth.UserName != model.User.UserName && _unitOfWork.User.FindByUserName(model.User.UserName, trackChanges: false) != null)
            {
                throw new Exception("User name already registered");
            }

            User user = await _unitOfWork.User.FindById(auth.Id, trackChanges: true);

            _ = _mapper.Map(model.User, user);

            Account account = await _unitOfWork.Account.FindByUserId(auth.Id, trackChanges: true);

            _ = _mapper.Map(model.Account, account);

            await _unitOfWork.Save();

            return true;
        }

        [HttpPut]
        [Route(nameof(SetCulture))]
        public async Task<bool> SetCulture(
           [FromBody] UserForEditCultureDto model)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            User user = await _unitOfWork.User.FindById(auth.Id, trackChanges: true);

            _ = _mapper.Map(model, user);

            await _unitOfWork.Save();

            return true;
        }

        [HttpPost]
        [Route(nameof(RefreshToken))]
        [AllowAnonymous]
        public async Task<UserDto> RefreshToken(
            [FromBody] UserForTokenDto model)
        {
            model.Token = System.Net.WebUtility.UrlDecode(model.Token);
            model.Token = model.Token.Replace(" ", "+");

            UserAuthenticatedDto auth = await _authManager.Authenticate(model.Token, IpAddress());

            SetToken(auth.TokenResponse);
            SetRefresh(auth.RefreshTokenResponse);

            UserDto usersDto = _mapper.Map<UserDto>(auth);

            return usersDto;
        }

        [HttpPost]
        [Route(nameof(RevokeToken))]
        [AllowAnonymous]
        public async Task<bool> RevokeToken(
            [FromBody] UserForTokenDto model)
        {
            _ = await _authManager.Authenticate(model.Token, IpAddress());

            await _authManager.RevokeToken(model.Token, IpAddress());

            return true;
        }

        // Helper Methods
        private async Task SendVerification(string emailAddress, string code)
        {
            if (!string.IsNullOrWhiteSpace(emailAddress))
            {
                bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

                string template = otherLang ? "email" : "email-rtl";

                EmailMessage message = new(new string[] { emailAddress }, "Verification", code, _environment.WebRootPath, $"/Templates/EmailTemplateV2/Verify/{template}.html");
                await _emailSender.SendHtmlEmail(message);
            }
        }
    }
}
