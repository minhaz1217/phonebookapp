using Autofac;
using phonebook_app_read.Persistence;
using phonebook_app_read.Persistence.model;
using phonebook_app_read.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phonebook_app_read.Service
{
    public class PhonebookReadService : IPhonebookReadService
    {
        IDBRepository db = null;
        //private readonly ILifetimeScope container;
        public PhonebookReadService(IDBRepository dBRepository)
        {
            this.db = dBRepository;
        }
        public IEnumerable<PhonebookReadName> GetAll()
        {
            
            IEnumerable<PhonebookReadName> phonebookReadNames = db.GetAllPhonebookReadName();
            return phonebookReadNames;
        }
    }
}
