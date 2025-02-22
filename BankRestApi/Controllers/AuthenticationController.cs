﻿using BankRestApi.Models;
using BankRestApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BankRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly ITokenService _tokenServices;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(ITokenService tokenServices, 
                                        ILogger<AuthenticationController> logger)
        {
            _tokenServices = tokenServices;
            _logger = logger;
        }

        /// <summary>        
        /// </summary>
        /// <returns> </returns>
        /// <remarks>
        /// 
        /// login: user1 | password: password grants access to Transactions/statements and Transactions/accounts endpoints. login: user2 | password: password grans access to any Transactions/ endpoint.
        /// 
        /// </remarks>
        /// <response code="200"></response>
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User user)
        {
            try
            {
                var token = await _tokenServices.GenerateToken(user);
                user.Password = "";

                return Ok(new { user, token });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error message: " + ex.Message + " StackTrace: " + ex.StackTrace);
                return StatusCode(500);
            }
            }
    }
}
