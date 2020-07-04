﻿using Application.Common;
using Dapper;
using Microsoft.Extensions.Options;
using OdysseyPublishers.Application.Common;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace OdysseyPublishers.Infrastructure.Common
{
    public class SqlRepositoryBase : IRepository
    {
        private readonly PersistenceConfigurations _persistenceconfigurations;

        public SqlRepositoryBase(IOptions<PersistenceConfigurations> persistenceconfigurations)
        {
            _persistenceconfigurations = persistenceconfigurations.Value;
        }

        public DbConnection GetDbConnection()
        {

            return new SqlConnection(_persistenceconfigurations.ConnectionString);

        }


        public IEnumerable<T> QueryDatabase<T>(string commandQuery, DynamicParameters parameters)
        {
            var connection = GetDbConnection();
            IEnumerable<T> result = null;
            using (connection)
            {
                result = connection.Query<T>(commandQuery, parameters);
            }

            return result;

        }

        public IEnumerable<dynamic> QueryDatabase(string command, DynamicParameters parameters)
        {

            var connection = GetDbConnection();
            using (connection)
            {
                var sqlCommand = new SqlCommand(command, (SqlConnection)connection);
                var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    yield return reader;
                }
            }

        }

        public dynamic QueryDatabaseSingle(string command, DynamicParameters parameters)
        {

            var connection = GetDbConnection();
            dynamic res = null;
            using (connection)
            {
                var sqlCommand = new SqlCommand(command, (SqlConnection)connection);
                var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    res = reader;
                }

            }
            return res;

        }

        public T QueryDatabaseSingle<T>(string commandQuery, DynamicParameters parameters)
        {
            var connection = GetDbConnection();
            dynamic result = null;
            using (connection)
            {
                result = connection.Query<T>(commandQuery).FirstOrDefault();
            }

            return result;
        }

    }
}
