using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DWDW_API.Core.Infrastructure;
using DWDW_API.Core.ViewModels;
using DWDW_API.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DWDW_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public ExtensionSettings extensionSettings { get; }

        public ClaimsPrincipal currentUser => extensionSettings.HttpContextAccessor.HttpContext.User;

        public string CurrentUserId => currentUser.FindFirstValue(ClaimTypes.NameIdentifier);

        public string CurrentUsername => currentUser.FindFirstValue(ClaimTypes.Name);

        public BaseController(ExtensionSettings extensionSettings)
        {
            this.extensionSettings = extensionSettings;
            string name = currentUser.Identity.Name;
            List<Claim> claims = currentUser.Claims.ToList();
            Claim claim = currentUser.Claims.FirstOrDefault(c => c.Value == ClaimTypes.NameIdentifier);
        }
        protected dynamic ExecuteInMonitoring(Func<dynamic> func)
        {
            dynamic result;
            var responseVM = new ResponseViewModel();
            try
            {
                responseVM.StatusCode = StatusCodes.Status200OK;
                responseVM.Data = func();
                result = Ok(responseVM);
            }
            catch (BaseException e)
            {
                responseVM.StatusCode = StatusCodes.Status400BadRequest;
                responseVM.Message = e.Message;
                result = BadRequest(responseVM);
            }
            catch (Exception e)
            {

                responseVM.StatusCode = StatusCodes.Status500InternalServerError;
                responseVM.Message = e.Message;
                result = StatusCode(StatusCodes.Status500InternalServerError,responseVM);
            }

            return result;

        }

        protected async Task<dynamic> ExecuteInMonitoringAsync(Func<Task<dynamic>> func)
        {
            dynamic result;
            var responseVM = new ResponseViewModel();
            try
            {
                responseVM.StatusCode = StatusCodes.Status200OK;
                responseVM.Data = await func();
                result = Ok(responseVM);
            }
            catch (BaseException e)
            {
                responseVM.StatusCode = StatusCodes.Status400BadRequest;
                responseVM.Message = e.Message;
                result = BadRequest(responseVM);
            }
            catch (Exception e)
            {

                responseVM.StatusCode = StatusCodes.Status500InternalServerError;
                responseVM.Message = e.Message;
                result = StatusCode(StatusCodes.Status500InternalServerError, responseVM);
            }

            return result;

        }
    }
}