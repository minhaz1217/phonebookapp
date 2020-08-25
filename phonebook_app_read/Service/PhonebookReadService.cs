using Autofac;
using PhonebookRead.Persistence;
using PhonebookRead.Persistence.model;
using PhonebookRead.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhonebookRead.Service
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
