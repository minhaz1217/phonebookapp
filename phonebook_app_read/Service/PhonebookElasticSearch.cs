using Nest;
using phonebook_app_read.Persistence.model;
using phonebook_practice_app;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace phonebook_app_read.Service
{
    public class PhonebookElasticSearch: IPhonebookElasticSearch
    {
        private static PhonebookElasticSearch instance = null;
        private static ElasticClient client = null;

        private PhonebookElasticSearch() {
        }
        public static PhonebookElasticSearch Instance()
        {
            if(instance == null)
            {
                instance = new PhonebookElasticSearch();
            }
            if(client == null)
            {
                var node = new Uri(ConfigReader.GetValue<string>("ElasticSearchHost"));
                var settings = new ConnectionSettings(node);
                client = new ElasticClient(settings);
                
            }

            return instance;
        }


        public bool Insert(string index, Phonebook phonebook)
        {
            IndexResponse response = client.Index<Phonebook>(phonebook, idx => idx.Index(index));
            Helper.Print($"Update {(response.Result == Result.Created).ToString()} {phonebook.ToString()} ");
            return (response.Result == Result.Created);

        }
        public bool Update(string index, Phonebook phonebook)
        {
            this.Delete(index, phonebook);
            bool response = this.Insert(index, phonebook);
            Helper.Print($"Update {(response).ToString()} {phonebook.ToString()}");
            return (response);
        }
        public bool Delete(string index, Phonebook phonebook)
        {
            var response = client.Delete<Phonebook>(phonebook.Id, idx => idx.Index(index));
            Helper.Print($"Update {(response.Result == Result.Created).ToString()} {phonebook.ToString()} ");
            return (response.Result == Result.Deleted);
        }

        // TODO: use reflection over the type to dynamically get the field property
        public IReadOnlyCollection<Phonebook> GetAll(string index, string type, string value = "")
        {
            ISearchResponse<Phonebook> searchResponse = null;
            if (type == "name")
            {

            searchResponse = client.Search<Phonebook>(s => s
            .Index(index)
            .Query(q => q
                 .Match(m => m
                    .Field(f => f.Name)
                    .Query(value)
                 )
            )
            );
            }else if(type == "number")
            {

                searchResponse = client.Search<Phonebook>(s => s
                    .Index(index)
                    .Query(q => q
                         .Match(m => m
                            .Field(f => f.Number)
                            .Query(value)
                         )
                    )
                );
            }
            return searchResponse.Documents;
        }

    }
}
