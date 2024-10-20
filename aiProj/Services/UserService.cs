using aiProj.DAL.Interfaces;
using aiProj.Domian;
using aiProj.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace aiProj.DAL
{
    public class UserService
    {
        public IUserRepository _userRepository;

        private readonly IConfiguration _configuration;


        public static string Code;

        private static User _userBuff;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<User> GetUserByLogin(string login)
        {
            return await _userRepository.GetByName(login);
        }      
      
        public async Task<string> Register(User user)
        {
            if(!CheckExist(user.Email, user.Login).Result)
            {
                Cryptographer cryptographer = new Cryptographer();
                Code = cryptographer.GenerateEmailCode();

                EmailSender emailSender = new EmailSender();
                emailSender.SendEmailCode(user.Email, Code, true);

                user.Password = cryptographer.CryptPassword(user.Password);
                _userBuff = user;

                return Code;
            }

            return string.Empty;
        }

        public async Task<bool> ConfirmCode()
        {
            
            await _userRepository.Create(_userBuff);
            return true;
         
            return false;
        }

        public async Task<string> Login(LoginVeiwModel model)

        {
            try
            {
                var user = _userRepository.Select().Result.FirstOrDefault(x => x.Login == model.Username || x.Email == model.Username);

                if (user == null)
                {
                    return "Пользователь не найден";
                }
                Cryptographer cryptographer = new Cryptographer();
                string pas = cryptographer.DecryptPassword(model.Password);

                if (user.Password != pas)
                {
                    return "Неверный пароль";
                }
                var resp = GenerateAuthorizationToken(user);
                return resp;
            }
            catch (Exception ex)
            {
                return "Неизвестная ошибка";
            }
        }       

        private string GenerateAuthorizationToken(User user)
        {
            var secret = "verylongsecretkeyof32characters12345";
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

            var userClaims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
            };

            var jwt = new JwtSecurityToken(
                    claims: userClaims,
                    expires: DateTime.Now.AddHours(2),
                    audience: "https://localhost:7000/",
                    issuer: "https://localhost:7000/",
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        private async Task<bool> CheckExist(string email, string login)
        {
            var users = await _userRepository.Select();
            var Email = users.FirstOrDefault(x => x.Email == email);
            var Login = users.FirstOrDefault(x => x.Login == login);

            if (Email != null || Login != null)
            {
                return true;
            }

            return false;
        }
    }
}