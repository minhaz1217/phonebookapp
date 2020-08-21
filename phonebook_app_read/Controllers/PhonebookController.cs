using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using phonebook_app_read.Persistence;
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
        public PhonebookController(IConfiguration config)
        {
            this._appConfig = config;
        }
        // GET: api/<Phonebook>
        [HttpGet]
        public IEnumerable<string> Get()
        {

            IDBRepository db = CassandraDBRepository.Instance(_appConfig.GetValue<string>("CASSANDRA_SERVER_NAME"), _appConfig.GetValue<string>("CASSANDRA_KEYSPACE_NAME"));

            IEnumerable<PhonebookReadName> phonebookReadNames = db.GetAllPhonebookReadName();
            List<string> output = new List<string>();
            foreach(PhonebookReadName pb in phonebookReadNames)
            {
                output.Add(pb.ToString());
            }
            return output;
        }

        // GET api/<Phonebook>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [HttpGet("search/{searchTerm}/{searchValue}")]
        public string Get(string searchTerm, string searchValue)
        {
            System.Text.StringBuilder output = new System.Text.StringBuilder();
            if(searchTerm.ToLower() == "name")
            {
                var phonebooks = PhonebookElasticSearch.Instance().GetAll(ConfigReader.GetValue<string>("ElasticPhonebookIndex"), "name", searchValue);
                foreach(Phonebook pb in phonebooks)
                {
                    output.Append(pb.ToString());
                }
            }
            else if(searchTerm.ToLower() == "number")
            {
                var phonebooks = PhonebookElasticSearch.Instance().GetAll(ConfigReader.GetValue<string>("ElasticPhonebookIndex"), "number", searchValue);
                foreach (Phonebook pb in phonebooks)
                {
                    output.Append(pb.ToString());
                }
            }
            return output.ToString();
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
