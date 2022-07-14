using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RegistrationAdmin.Models.Application
{
    public class LogSettings
    {
        private static string _datasource = null;
        public string DataSource
        {
            get => _datasource;
            set => _datasource = value;
        }

        private static string _initialCatalog = null;
        public string InitialCatalog
        {
            get => _initialCatalog;
            set => _initialCatalog = value;
        }

        private static string _databaseUser = null;
        public string DatabaseUser
        {
            get => _databaseUser;
            set => _databaseUser = value;
        }

        private static string _databaseUserPassword = null;
        public string DatabaseUserPassword
        {
            get => _databaseUserPassword;
            set => _databaseUserPassword = value;
        }


        public string GetLoggingDatabaseConnectionString()
        {
            var csb = new SqlConnectionStringBuilder
            {
                DataSource = this.DataSource,
                InitialCatalog = this.InitialCatalog,
                UserID = this.DatabaseUser,
                Password = this.DatabaseUserPassword,
                ConnectTimeout = 30,
                Encrypt = true,
                TrustServerCertificate = false,
                ApplicationIntent = ApplicationIntent.ReadWrite,
                MultiSubnetFailover = false
            };

            return csb.ConnectionString;
        }
    }
}
