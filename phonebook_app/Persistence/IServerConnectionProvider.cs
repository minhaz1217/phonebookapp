using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phonebook_practice_app.Persistence
{
    interface IServerConnectionProvider
    {
        NpgsqlConnection GetConnection();
    }
}
