using System;
using System.Collections.Generic;
using System.ComponentModel;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Npgsql;
using phonebook_practice_app;
using phonebook_practice_app.Persistence;
using phonebook_practice_app.Persistence.wrapper;
using Phonebook_Practice_App.model;

namespace Phonebook_Practice_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhonebookController : ControllerBase
    {
        private readonly IConfiguration _appConfig;
        private string _connectionString = "";
        public PhonebookController(IConfiguration config)
        {
            this._appConfig = config;
            _connectionString = _appConfig.GetValue<string>("DapperConnectionString");
        }
        // GET api/phonebook
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            Phonebook phonebook = new Phonebook();
            phonebook.Id = "123";
            phonebook.Name = "Minhaz";
            phonebook.Number = "0168364654";


            IServerConnectionProvider connectionProvider = new ServerConnectionProvider(this._connectionString);
            List<string> myList = new List<string>();
            List<Phonebook> myBooks = (List<Phonebook>)(new ConnectionWrapper(_appConfig)).GetAll<Phonebook>("select * from phonebook;");
            foreach (Phonebook book in myBooks)
            {
                myList.Add(book.ToString());
            }

            return myList;
        }

        // GET api/phonebook/5
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<string>> Get(string id)
        {
            // if (id != item.Id)
            // {
            //     return BadRequest();
            // }
            IServerConnectionProvider connectionProvider = new ServerConnectionProvider(this._connectionString);
            List<string> myList = new List<string>();
            List<Phonebook> myBooks = (List<Phonebook>)(new ConnectionWrapper(_appConfig)).GetAll<Phonebook>($"select * from phonebook where id='{id}';");
            foreach (Phonebook book in myBooks)
            {
                myList.Add(book.ToString());
            }

            return myList;
        }

        // POST api/phonebook
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Post([FromBody] Phonebook phonebook)
        {
            Utils.Print("REACHED POST");
            if(phonebook.Id == "" || phonebook.Id == null)
            {
                Utils.Print($"POST1 {phonebook.Id} => {phonebook.Name} {phonebook.Number}");
                return BadRequest();
            }
            if (phonebook.Name == null || phonebook.Name == "")
            {
                Utils.Print($"POST2 {phonebook.Id} => {phonebook.Name} {phonebook.Number}");
                return BadRequest();
            }
            if (phonebook.Number == null || phonebook.Number == "")
            {
                Utils.Print($"POST3 {phonebook.Id} => {phonebook.Name} {phonebook.Number}");
                return BadRequest();
            }
            ConnectionWrapper wrapper = new ConnectionWrapper(this._appConfig);
            Utils.Print($"POST {phonebook.Id} => {phonebook.Name} {phonebook.Number}");
            wrapper.Create<Phonebook>(phonebook);
            return Accepted("Saved successfully");
        }

        // PUT api/phonebook/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Phonebook phonebook)
        {
            Utils.Print($"REACHED put {id}");
            //Utils.Print($"PUT {phonebook.Id} => {phonebook.Name} {phonebook.Number}");
            if(id != phonebook.Id)
            {
                return BadRequest();
            }
            ConnectionWrapper wrapper = new ConnectionWrapper(this._appConfig);
            phonebook.Id = id;
            wrapper.Update<Phonebook>(phonebook);
            return Accepted();

        }
        // PATCH api/phonebook/5
        [HttpPatch("{id}")]
        public IActionResult Patch(string id, [FromBody] Phonebook phonebook)
        {
            Utils.Print($"REACHED Patch {id}");
            phonebook.Id = id;
            Utils.Print($"Patch {phonebook.Id} => {phonebook.Name} {phonebook.Number}");
            if (id != phonebook.Id)
            {
                return BadRequest();
            }
            ConnectionWrapper wrapper = new ConnectionWrapper(this._appConfig);
            wrapper.Update<Phonebook>(phonebook);
            return Accepted();
        }

        // DELETE api/phonebook/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            ConnectionWrapper wrapper = new ConnectionWrapper(this._appConfig);
            List<Phonebook> phonebooks = (List<Phonebook>)wrapper.GetAll<Phonebook>($"select * from phonebook where id='{id}'");
            if(phonebooks.Count <= 0)
            {
                return BadRequest();
            }
            phonebooks.ForEach(delegate (Phonebook e)
            {
                wrapper.Delete<Phonebook>(e);
                //Utils.Print(e.ToString());
            });
            return Accepted();
        }
    }
}
