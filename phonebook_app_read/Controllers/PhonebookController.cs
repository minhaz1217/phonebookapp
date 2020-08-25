using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Authorization;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nest;
using PhonebookRead.Persistence;
using PhonebookRead.Persistence.mapper;
using PhonebookRead.Persistence.model;
using PhonebookRead.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PhonebookRead.Controllers
{
    [Route("phonebook/")]
    [ApiController]
    public class PhonebookController : ControllerBase
    {
        private readonly IConfiguration _appConfig;
        private readonly ILifetimeScope container;
        private IPhonebookElasticSearch elasticsearch = null;
        private string elasticSearchIndex = "";

        private IUserService _userService;
        public PhonebookController(IConfiguration config, ILifetimeScope container, IUserService userService)
        {
            this._appConfig = config;
            this.container = container;
            this._userService = userService;
            this.elasticsearch = this.container.Resolve<IPhonebookElasticSearch>();
            this.elasticSearchIndex = ConfigReader.GetValue<string>("ElasticPhonebookIndex");
        }
        // GET: api/<Phonebook>
        [Authorize]
        [HttpGet]
        public ActionResult Get()
        {

            IPhonebookReadService phonebookReadService = this.container.Resolve<IPhonebookReadService>();
            IEnumerable<PhonebookReadName> phonebookReadNames = new List<PhonebookReadName>();
            List<PhonebookReadName> output = new List<PhonebookReadName>();
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["name"]))
            {
                string value = HttpContext.Request.Query["name"];
                PhonebookWrite.Helper.Print($"Searching : {value}");
                var phonebooks = this.elasticsearch.GetAll(this.elasticSearchIndex, "name", value);
                foreach(Phonebook pb in phonebooks)
                {
                    phonebookReadNames.Append(PhonebookMapper.PhonebookToPhonebookReadName(pb));
                    output.Add(PhonebookMapper.PhonebookToPhonebookReadName(pb));
                }
            }
            else if (!String.IsNullOrEmpty(HttpContext.Request.Query["number"]))
            {
                string value = HttpContext.Request.Query["number"];
                PhonebookWrite.Helper.Print($"Searching : {value}");
                var phonebooks = this.elasticsearch.GetAll(this.elasticSearchIndex, "number", value);
                foreach (Phonebook pb in phonebooks)
                {
                    phonebookReadNames.Append(PhonebookMapper.PhonebookToPhonebookReadName(pb));
                    output.Add(PhonebookMapper.PhonebookToPhonebookReadName(pb));
                }
            }
            else
            {
                phonebookReadNames = phonebookReadService.GetAll();
                foreach (PhonebookReadName pb in phonebookReadNames)
                {
                    output.Add(pb);
                }
            }
            //phonebook_practice_app.Helper.Print($"Total : {output.Count.ToString()}");
            return Ok(output);
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(Authorization.AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}
