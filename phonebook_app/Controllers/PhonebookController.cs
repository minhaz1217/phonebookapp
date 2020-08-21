using System;
using System.Collections.Generic;
using System.Text.Json;
using KafkaConnection.model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using phonebook_practice_app;
using phonebook_practice_app.Persistence;
using phonebook_practice_app.Persistence.wrapper;
using Phonebook_Practice_App.model;
using KafkaConnection.Messangerwrapper;
using KafkaConnection.kafkawrapper;
using Autofac;
using phonebook_app.Service;

namespace Phonebook_Practice_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhonebookController : ControllerBase
    {

        private ILifetimeScope container = null;
        private IPhonebookService phonebookService = null;
        public PhonebookController(IConfiguration config, ILifetimeScope container)
        {
            this.container = container;
            phonebookService = this.container.Resolve<IPhonebookService>();
        }
        // GET api/phonebook
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            List<string> myList = new List<string>();
            List<Phonebook> myBooks = (List<Phonebook>)phonebookService.GetAll("select * from phonebook;");
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
            List<string> myList = new List<string>();
            List<Phonebook> myBooks = (List<Phonebook>)phonebookService.GetAll($"select * from phonebook where id='{id}';");
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
            
            if (phonebook.Name == null || phonebook.Name == "")
            {
                Helper.Print($"POST2 {phonebook.Id} => {phonebook.Name} {phonebook.Number}");
                return BadRequest();
            }
            if (phonebook.Number == null || phonebook.Number == "")
            {
                Helper.Print($"POST3 {phonebook.Id} => {phonebook.Name} {phonebook.Number}");
                return BadRequest();
            }
            this.phonebookService.Create(phonebook);
            this.phonebookService.PublishPost(phonebook);
            return Accepted("Saved successfully");
        }

        // PUT api/phonebook/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Phonebook phonebook)
        {
            Helper.Print($"REACHED put {id} => {phonebook.ToString()}");
            Guid phonebookId = new Guid();
            Guid.TryParse(id, out phonebookId);
            if(phonebookId == new Guid())
            {
                return BadRequest();
            }
            if (id == null || id == "")
            {
                return BadRequest();
            }
            if (phonebook.Name == null || phonebook.Name == "")
            {
                return BadRequest();
            }
            if (phonebook.Number == null || phonebook.Number == "")
            {
                return BadRequest();
            }
            phonebook.Id = id;
            this.phonebookService.Update(phonebook);
            Helper.Print($"END put {id} {phonebook.Id}");
            this.phonebookService.PublishPut(phonebook);
            return Accepted();

        }
        // PATCH api/phonebook/5
        [HttpPatch("{id}")]
        public IActionResult Patch(string id, [FromBody] Phonebook phonebook)
        {
            Helper.Print($"REACHED PATCH {id} => {phonebook.ToString()}");
            Guid phonebookId = new Guid();
            Guid.TryParse(id, out phonebookId);
            if (phonebookId == new Guid())
            {
                return BadRequest();
            }
            if (id == null || id == "")
            {
                return BadRequest();
            }
            phonebook.Id = id;
            if(phonebook.Name == null || phonebook.Number == null)
            {
                List<Phonebook> phonebooks = (List<Phonebook>)this.phonebookService.GetAll($"select * from phonebook where id='{phonebook.Id}'");
                foreach (Phonebook pb in phonebooks)
                {
                    if (pb.Id == phonebook.Id)
                    {
                        if (phonebook.Name == null)
                        {
                            phonebook.Name = pb.Name;
                        }
                        if (phonebook.Number == null)
                        {
                            phonebook.Number = pb.Number;
                        }
                        break;
                    }
                }
            }            
            this.phonebookService.Update(phonebook);
            this.phonebookService.PublishPatch(phonebook);
            return Accepted();
        }

        // DELETE api/phonebook/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            Guid phonebookId = new Guid();
            Guid.TryParse(id, out phonebookId);
            if (phonebookId == new Guid())
            {
                return BadRequest();
            }
            if (id == null || id == "")
            {
                return BadRequest();
            }
            List<Phonebook> phonebooks = (List<Phonebook>)this.phonebookService.GetAll($"select * from phonebook where id='{id}'");
            if(phonebooks.Count <= 0)
            {
                return BadRequest();
            }
            phonebooks.ForEach(delegate (Phonebook phonebook)
            {
                this.phonebookService.Delete(phonebook);
                this.phonebookService.PublishDelete(phonebook);
            });
            return Accepted();
        }
    }
}
