using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using phonebook_app_read.Persistence;
using phonebook_app_read.Persistence.mapper;
using phonebook_app_read.Persistence.model;
using phonebook_app_read.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace phonebook_app_read.Controllers
{
    [Route("phonebook/")]
    [ApiController]
    public class PhonebookController : ControllerBase
    {
        private readonly IConfiguration _appConfig;
        private readonly ILifetimeScope container;
        public PhonebookController(IConfiguration config, ILifetimeScope container)
        {
            this._appConfig = config;
            this.container = container;
        }
        // GET: api/<Phonebook>
        [HttpGet]
        public ActionResult<string> Get()
        {

            IPhonebookReadService phonebookReadService = this.container.Resolve<IPhonebookReadService>();
            IEnumerable<PhonebookReadName> phonebookReadNames = new List<PhonebookReadName>();
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["name"]))
            {
                string value = HttpContext.Request.Query["name"];
                phonebook_practice_app.Helper.Print($"Searching : {value}");
                var phonebooks = this.container.Resolve<IPhonebookElasticSearch>().GetAll(ConfigReader.GetValue<string>("ElasticPhonebookIndex"), "name", value);
                foreach(Phonebook pb in phonebooks)
                {
                    sb.Append( JsonSerializer.Serialize<PhonebookReadName>(PhonebookMapper.PhonebookToPhonebookReadName(pb)) );
                    //phonebookReadNames.Append( PhonebookMapper.PhonebookToPhonebookReadName(pb) );
                }
            }
            else if (!String.IsNullOrEmpty(HttpContext.Request.Query["number"]))
            {
                string value = HttpContext.Request.Query["number"];
                phonebook_practice_app.Helper.Print($"Searching : {value}");
                var phonebooks = this.container.Resolve<IPhonebookElasticSearch>().GetAll(ConfigReader.GetValue<string>("ElasticPhonebookIndex"), "number", value);
                foreach (Phonebook pb in phonebooks)
                {
                    sb.Append(JsonSerializer.Serialize<PhonebookReadName>(PhonebookMapper.PhonebookToPhonebookReadName(pb)));
                    //phonebookReadNames.Append(PhonebookMapper.PhonebookToPhonebookReadName(pb));
                }
            }
            else
            {

                phonebookReadNames = phonebookReadService.GetAll();
                List<string> output = new List<string>();
                foreach (PhonebookReadName pb in phonebookReadNames)
                {
                    sb.Append(JsonSerializer.Serialize<PhonebookReadName>(pb));
                    //output.Add( JsonSerializer.Serialize<ListPhonebookReadName>>(phonebookReadNames) );
                }
            }
            return Ok(sb.ToString());
        }

        // GET api/<Phonebook>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        // POST api/<Phonebook>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<Phonebook>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Phonebook>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
