using IdentityServer.IdentityAdminWeb.Models;
using IdentityServer.Service.Dtos.Configuration;
using IdentityServer.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.IdentityAdminWeb.Controllers
{
    public class ConfigurationController : BaseController
    {
        private readonly IClientService _clientService;
        private readonly IIdentityResourceService _identityResourceService;
        private readonly IApiResourceService _apiResourceService;
        private readonly IApiScopeService _apiScopeService;
        public ConfigurationController(IClientService clientService, IIdentityResourceService identityResourceService, IApiResourceService apiResourceService, IApiScopeService apiScopeService)
        {
            _clientService = clientService;
            _identityResourceService = identityResourceService;
            _apiResourceService = apiResourceService;
            _apiScopeService = apiScopeService;
        }

        /// <summary>
        /// 客户端
        /// </summary>
        public IActionResult Clients()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QueryClients(int pageIndex, int pageSize, string key)
        {
            var pageData = await _clientService.GetClientsAsync(key, pageIndex, pageSize);
            return Json(new { code = 0, data = pageData });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClient(int id)
        {
            if (id == 0) return Json(new { code = -1, msg = "不存在" });

            var client = await _clientService.GetClientAsync(id);
            if (client == null) return Json(new { code = -1, msg = "不存在" });

            var result = await _clientService.RemoveClientAsync(client) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpPost]
        public async Task<IActionResult> Client(ClientDto client)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = -1, msg = "参数错误" });
            }

            //Add new client
            if (client.Id == 0)
            {
                client = _clientService.BuildClientViewModel(client);
                var clientId = await _clientService.AddClientAsync(client);

                return Json(new { code = clientId > 0 ? 0 : -1, msg = clientId > 0 ? "成功" : "失败" });
            }

            //Update client
            var result = await _clientService.UpdateClientAsync(client) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpGet]
        public IActionResult Client(int id)
        {
            ViewBag.ClientId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CloneClient(CloneClientRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = -1, msg = "参数错误" });
            }

            var clientId = await _clientService.CloneClientAsync(request.OriginalId, request.ClientId, request.ClientName);
            return Json(new { code = 0, data = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> QueryClient(int id)
        {
            if (id == 0) Json(new { code = -1, msg = "不存在" });

            var client = await _clientService.GetClientAsync(id);
            return Json(new { code = 0, data = client });
        }

        [HttpPost]
        public async Task<IActionResult> AddClientSecret(AddClientSecretRequest request)
        {
            var result = await _clientService.AddClientSecretAsync(request.ClientId, request.ClientSecret) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClientSecret(int id)
        {
            var result = await _clientService.DeleteClientSecretAsync(id) > 0;
            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpPost]
        public async Task<IActionResult> AddClientProperty(AddClientPropertyRequest request)
        {
            var result = await _clientService.AddClientPropertyAsync(request.ClientId, request.ClientProperty) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClientProperty(int id)
        {
            var result = await _clientService.DeleteClientPropertyAsync(id) > 0;
            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpPost]
        public async Task<IActionResult> AddClientClaim(AddClientClaimRequest request)
        {
            var result = await _clientService.AddClientClaimAsync(request.ClientId, request.ClientClaim) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClientClaim(int id)
        {
            var result = await _clientService.DeleteClientClaimAsync(id) > 0;
            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }


        /// <summary>
        /// 身份资源
        /// </summary>
        public IActionResult IdentityResources()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QueryIdentityResources(int pageIndex, int pageSize, string key)
        {
            var pageData = await _identityResourceService.GetIdentityResourcesAsync(key, pageIndex, pageSize);
            return Json(new { code = 0, data = pageData });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIdentityResource(int id)
        {
            if (id == 0) return Json(new { code = -1, msg = "不存在" });

            var result = await _identityResourceService.DeleteIdentityResourceAsync(id) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpPost]
        public async Task<IActionResult> IdentityResource(IdentityResourceDto identityResource)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = -1, msg = "参数错误" });
            }

            //Add new
            if (identityResource.Id == 0)
            {
                var identityResourceId =await _identityResourceService.AddIdentityResourceAsync(identityResource);
                return Json(new { code = identityResourceId > 0 ? 0 : -1, msg = identityResourceId > 0 ? "成功" : "失败" });
            }

            //Update
            var result = await _identityResourceService.UpdateIdentityResourceAsync(identityResource) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpGet]
        public IActionResult IdentityResource(int id)
        {
            ViewBag.IdentityResourceId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QueryIdentityResource(int id)
        {
            if (id == 0) Json(new { code = -1, msg = "不存在" });

            var client = await _identityResourceService.GetIdentityResourceAsync(id);
            return Json(new { code = 0, data = client });
        }

        [HttpPost]
        public async Task<IActionResult> AddIdentityResourceProperty(AddIdentityResourcePropertyRequest request)
        {
            var result = await _identityResourceService.AddIdentityResourcePropertyAsync(request.IdentityResourceId, request.IdentityResourceProperty) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIdentityResourceProperty(int id)
        {
            var result = await _identityResourceService.DeleteIdentityResourcePropertyAsync(id) > 0;
            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }



        /// <summary>
        /// API资源
        /// </summary>
        public IActionResult ApiResources()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QueryApiResources(int pageIndex, int pageSize, string key)
        {
            var pageData = await _apiResourceService.GetApiResourcesAsync(key, pageIndex, pageSize);
            return Json(new { code = 0, data = pageData });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteApiResource(int id)
        {
            if (id == 0) return Json(new { code = -1, msg = "不存在" });

            var result = await _apiResourceService.DeleteApiResourceAsync(id) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpPost]
        public async Task<IActionResult> ApiResource(ApiResourceDto apiResource)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = -1, msg = "参数错误" });
            }

            //Add new
            if (apiResource.Id == 0)
            {
                var apiResourceId = await _apiResourceService.AddApiResourceAsync(apiResource);
                return Json(new { code = apiResourceId > 0 ? 0 : -1, msg = apiResourceId > 0 ? "成功" : "失败" });
            }

            //Update
            var result = await _apiResourceService.UpdateApiResourceAsync(apiResource) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpGet]
        public IActionResult ApiResource(int id)
        {
            ViewBag.ApiResourceId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QueryApiResource(int id)
        {
            if (id == 0) Json(new { code = -1, msg = "不存在" });

            var client = await _apiResourceService.GetApiResourceAsync(id);
            return Json(new { code = 0, data = client });
        }

        [HttpPost]
        public async Task<IActionResult> AddApiResourceProperty(AddApiResourcePropertyRequest request)
        {
            var result = await _apiResourceService.AddApiResourcePropertyAsync(request.ApiResourceId, request.ApiResourceProperty) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteApiResourceProperty(int id)
        {
            var result = await _apiResourceService.DeleteApiResourcePropertyAsync(id) > 0;
            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpPost]
        public async Task<IActionResult> AddApiResourceSecret(AddApiResourceSecretRequest request)
        {
            var result = await _apiResourceService.AddApiResourceSecretAsync(request.ApiResourceId, request.ApiResourceSecret) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteApiResourceSecret(int id)
        {
            var result = await _apiResourceService.DeleteApiResourceSecretAsync(id) > 0;
            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }


        /// <summary>
        /// Api范围
        /// </summary>
        public IActionResult ApiScopes()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QueryApiScopes(int pageIndex, int pageSize, string key)
        {
            var pageData = await _apiScopeService.GetApiScopesAsync(key, pageIndex, pageSize);
            return Json(new { code = 0, data = pageData });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteApiScope(int id)
        {
            if (id == 0) return Json(new { code = -1, msg = "不存在" });

            var result = await _apiScopeService.DeleteApiScopeAsync(id) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpPost]
        public async Task<IActionResult> ApiScope(ApiScopeDto apiScope)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = -1, msg = "参数错误" });
            }

            //Add new
            if (apiScope.Id == 0)
            {
                var apiScopeId = await _apiScopeService.AddApiScopeAsync(apiScope);
                return Json(new { code = apiScopeId > 0 ? 0 : -1, msg = apiScopeId > 0 ? "成功" : "失败" });
            }

            //Update
            var result = await _apiScopeService.UpdateApiScopeAsync(apiScope) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpGet]
        public IActionResult ApiScope(int id)
        {
            ViewBag.ApiScopeId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QueryApiScope(int id)
        {
            if (id == 0) Json(new { code = -1, msg = "不存在" });

            var client = await _apiScopeService.GetApiScopeAsync(id);
            return Json(new { code = 0, data = client });
        }

        [HttpPost]
        public async Task<IActionResult> AddApiScopeProperty(AddApiScopePropertyRequest request)
        {
            var result = await _apiScopeService.AddApiScopePropertyAsync(request.ApiScopeId, request.ApiScopeProperty) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteApiScopeProperty(int id)
        {
            var result = await _apiScopeService.DeleteApiScopePropertyAsync(id) > 0;
            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }
    }
}
