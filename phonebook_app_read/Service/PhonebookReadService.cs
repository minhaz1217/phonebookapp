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

        private readonly ILifetimeScope container;
        public PhonebookReadService(ILifetimeScope container)
        {
            this.container = container;
        }
        public IEnumerable<PhonebookReadName> GetAll()
        {
            IDBRepository db = this.container.Resolve<IDBRepository>();
            IEnumerable<PhonebookReadName> phonebookReadNames = db.GetAllPhonebookReadName();
            return phonebookReadNames;
        }
    }
}
