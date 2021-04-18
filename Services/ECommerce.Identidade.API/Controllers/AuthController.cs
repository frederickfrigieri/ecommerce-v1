using ECommerce.Identidade.API.Extensions;
using ECommerce.Identidade.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.Identidade.API.Controllers
{
    [Route("api/identidade")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IGerarRespostaToken _gerarRespostaToken;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IGerarRespostaToken gerarRespostaToken)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _gerarRespostaToken = gerarRespostaToken;
        }

        [HttpPost]
        [Route("nova-conta")]
        public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, usuarioRegistro.Senha);

            if (result.Succeeded)
            {
                return CustomResponse(await _gerarRespostaToken.GerarJWT(usuarioRegistro.Email));
            }

            foreach (var error in result.Errors)
                AdicionarErro(error.Description);

            return CustomResponse();
        }

        [HttpPost]
        [Route("autenticar")]
        public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, false, true);

            if (result.Succeeded)
                return CustomResponse(await _gerarRespostaToken.GerarJWT(usuarioLogin.Email));

            if (result.IsLockedOut)
            {
                AdicionarErro("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse();
            }

            AdicionarErro("Usuário ou senha incorretos");
            return CustomResponse();
        }


    }
}
