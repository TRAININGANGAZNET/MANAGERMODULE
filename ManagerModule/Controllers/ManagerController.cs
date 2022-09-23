using ManagerModule.DAL;
using ManagerModule.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IGetDataReadersAsync _getDataReadersAsync;
        private readonly IConfiguration _configuration;
        private readonly string myConnectionString;

        public ManagerController(IConfiguration configuration, IGetDataReadersAsync getDataReadersAsync)
        {
            _configuration = configuration;
            _getDataReadersAsync = getDataReadersAsync;
            myConnectionString = _configuration["ConnectionStrings:DBConnectionString"];
        }

        [Route("ExecutiveCreation")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ManagerResponse> ExecutiveCreation([FromBody] ExcecutiveRequest excecutiveRequest)
        {
            ManagerResponse listres = new ManagerResponse();
            var Savemangdata = _configuration["Queries:InsertmangData"];
            try
            {
                listres = await Task.Run(() => _getDataReadersAsync.SaveExecutive<ManagerResponse, ExcecutiveRequest>(Savemangdata, excecutiveRequest, myConnectionString));
                if (listres.id != null)
                {
                    listres.Responses = _configuration["Responses:ExecutiveSucess"];
                }
                else
                {
                    listres.Responses = _configuration["Responses:FailMessage"];
                }
                return listres;
            }
            catch (Exception ex)
            {
                return listres;
            }

        }

        [Route("GetAllExecutives")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetAllExecutives()
        {
            List<GetExecresponse> getpropresponse = new List<GetExecresponse>();
            try
            {
                var Execsql = _configuration["Queries:GetExecutive"];
                getpropresponse = (List<GetExecresponse>)await Task.Run(() => _getDataReadersAsync.GetChildDataAsync<GetExecresponse, dynamic>(Execsql, myConnectionString));
                if (getpropresponse.Count() != 0)
                {
                    return Ok(getpropresponse);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Executive details are not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Executive details are not found.");
            }
        }

        [Route("GetAllExecutivesByLocality")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> getAllPropertiesByLocality([FromBody] LocalityType localityType)
        {
            List<GetExecresponse> getbyloc = new List<GetExecresponse>();
            try
            {
                var losql = _configuration["Queries:GetProperty"];
                getbyloc = (List<GetExecresponse>)await Task.Run(() => _getDataReadersAsync.GetChildDataAsync<GetExecresponse, dynamic>(losql, localityType, myConnectionString));
                if (getbyloc.Count() != 0)
                {
                    return Ok(getbyloc);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Executive details not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Executive details not found.");
            }
        }


        [Route("AssignExecutive")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AssignExecutive([FromBody] Customer customer)
        {
            AssignExecResponse asingexcepr = new AssignExecResponse();
            try
            {
                var assignexequery = _configuration["Queries:"];
                asingexcepr=await Task.Run(() => _getDataReadersAsync.AssignExec<AssignExecResponse, Customer>(assignexequery, customer, myConnectionString));
                if (asingexcepr.ExcutiveID!=0)
                {
                    asingexcepr.Responses = _configuration["Responses:UpdateExecutive"];
                }
                else
                {
                    asingexcepr.Responses = _configuration["Responses:failedExecutive"];
                }
                return Ok(asingexcepr);
            }
            catch (Exception ex)
            {
                asingexcepr.Responses = _configuration["Responses:failedExecutive"];
                return Ok(asingexcepr);
            }
            
        }
    }
}
