using System.Linq;
using BaseProject.Core.Communication;
using BaseProject.WebApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.WebApp.MVC.Controllers
{
    public class MainController : Controller
    {
        protected bool ResponseHasErrors(ResponseResult resposta)
        {
            if (resposta != null && resposta.Errors.Messages.Any())
            {
                foreach (var mensagem in resposta.Errors.Messages)
                {
                    ModelState.AddModelError(string.Empty, mensagem);
                }
                return true;
            }
            return false;
        }
        protected void AddValidationError(string mensagem)
        {
            ModelState.AddModelError(string.Empty, mensagem);
        }

        protected bool IsValidOperation()
        {
            return ModelState.ErrorCount == 0;
        }
    }
}
