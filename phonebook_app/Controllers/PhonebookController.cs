using System;
using System.Collections.Generic;
using System.Text.Json;
using MessageCarrier.model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PhonebookWrite.model;
using MessageCarrier.Messangerwrapper;
using MessageCarrier.kafkawrapper;
using Autofac;
using Authorization;
using PhonebookWrite.Service;

namespace PhonebookWrite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhonebookController : ControllerBase
    {

        private ILifetimeScope container = null;
        private IPhonebookService phonebookService = null;
        private IUserService _userService;
        public PhonebookController(IConfiguration config, IPhonebookService  phonebookService, IUserService userService)
        {
            this.phonebookService = phonebookService;
            this._userService = userService;
        }



        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
        // GET api/phonebook
        [Authorize]
        [HttpGet]
        public ActionResult Get()
        {
            List<Phonebook> myBooks = (List<Phonebook>)phonebookService.GetAllPhonebooks();
            return Ok(myBooks);
        }

        // GET api/phonebook/5
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            // if (id != item.Id)
            // {
            //     return BadRequest();
            // }
            Guid guid = new Guid();
            Guid.TryParse(id, out guid);
            if(guid == new Guid())
            {
                return BadRequest();
            }
            List<Phonebook> myBooks = (List<Phonebook>)phonebookService.GetPhonebooksById(id);
            return Ok(myBooks);
        }

        // POST api/phonebook
        [Authorize]
        [HttpPost]
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
            long number = 0;
            long.TryParse(phonebook.Number, out number);
            if (number <= 0)
            {
                return BadRequest();
            }
            this.phonebookService.Create(phonebook);
            return Ok("Saved successfully");
        }

        // PUT api/phonebook/5
        [Authorize]
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
            long number = 0;
            long.TryParse(phonebook.Number, out number);
            if (number <= 0)
            {
                return BadRequest();
            }
            phonebook.Id = id;
            this.phonebookService.Put(phonebook);
            Helper.Print($"END put {id} {phonebook.Id}");
            return Ok("Put successful");
        }
        // PATCH api/phonebook/5
        [Authorize]
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
                List<Phonebook> phonebooks = (List<Phonebook>)phonebookService.GetPhonebooksById(phonebook.Id);
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
            long number = 0;
            long.TryParse(phonebook.Number, out number);
            if (number <= 0)
            {
                return BadRequest();
            }
            this.phonebookService.Patch(phonebook);
            return Ok("Patch successful");
        }


        // DELETE api/phonebook/5
        [Authorize]
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
            List<Phonebook> phonebooks = (List<Phonebook>)phonebookService.GetPhonebooksById(id);
            if(phonebooks.Count <= 0)
            {
                return BadRequest();
            }
            phonebooks.ForEach(delegate (Phonebook phonebook)
            {
                this.phonebookService.Delete(phonebook);
            });
            return Ok("Delete successful");
        }
    }
}
