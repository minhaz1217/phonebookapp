using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nest;
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
        private IPhonebookElasticSearch elasticsearch = null;
        private string elasticSearchIndex = "";
        public PhonebookController(IConfiguration config, ILifetimeScope container)
        {
            this._appConfig = config;
            this.container = container;
            this.elasticsearch = this.container.Resolve<IPhonebookElasticSearch>();
            this.elasticSearchIndex = ConfigReader.GetValue<string>("ElasticPhonebookIndex");
        }
        // GET: api/<Phonebook>
        [HttpGet]
        public ActionResult Get()
        {

            IPhonebookReadService phonebookReadService = this.container.Resolve<IPhonebookReadService>();
            IEnumerable<PhonebookReadName> phonebookReadNames = new List<PhonebookReadName>();
            List<PhonebookReadName> output = new List<PhonebookReadName>();
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["name"]))
            {
                string value = HttpContext.Request.Query["name"];
                phonebook_practice_app.Helper.Print($"Searching : {value}");
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
                phonebook_practice_app.Helper.Print($"Searching : {value}");
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
    }
}
