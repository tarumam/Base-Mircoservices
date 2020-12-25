using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseProject.WebApp.MVC.Models;
using Refit;

namespace BaseProject.WebApp.MVC.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<ProductViewModel>> GetAll();
        Task<ProductViewModel> GetById(Guid id);
    }

    public interface ICatalogServiceRefit
    {
        [Get("/catalogo/produtos/")]
        Task<IEnumerable<ProductViewModel>> GetAll();

        [Get("/catalogo/produtos/{id}")]
        Task<ProductViewModel> GetById(Guid id);
    }
}
