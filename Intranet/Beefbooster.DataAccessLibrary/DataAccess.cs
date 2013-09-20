using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Data.Common;
using System.Text;
using System.Collections.Generic;

namespace Beefbooster.DataAccessLibrary
{
    /// <summary>
    /// DataLayer allows to connect to database as well as returns the major provider agnostic and provider dependent objects. It also returns non data objects such as collections, arrays and single values.
    /// DataLayer also takes care of processing parameters, conducting transaction, generating connectoin string, performing BulkCopy, etc.
    /// </summary>
    public static class DataAccess
    {

        #region Structures

        /// <summary>
        /// ConnectionStructure Structure allows to customize the connection string for such parameters
        /// as ConnectionTimeout for instance. The values for connection are kept in the web.config
        /// </summary>
        public struct ConnectionStructure
        {
            #region Declarations

            private int connectionTimeout;
            private string dataSource;
            private string initialCatalog;
            private bool trustedConnection;
            private string userID;
            private string password;
            private bool persistSecurityInfo;
            private int minPoolSize;
            private int maxPoolSize;
            private bool enlist;
            private bool async;
            private bool multipleActiveResultSets;

            #endregion

            #region Properties

            public string InitialCatalog
            {
                get
                {
                    return initialCatalog;
                }
                set
                {
                    initialCatalog = value;
                }
            }

            public string DataSource
            {
                get
                {
                    return dataSource;
                }
                set
                {
                    dataSource = value;
                }
            }

            public int ConnectionTimeout
            {
                get
                {
                    return connectionTimeout;
                }
                set
                {
                    connectionTimeout = value;
                }
            }

            public bool TrustedConnection
            {
                get
                {
                    return trustedConnection;
                }
                set
                {
                    trustedConnection = value;
                }
            }

            public string UserID
            {
                get
                {
                    return userID;
                }
                set
                {
                    userID = value;
                }
            }

            public string Password
            {
                get
                {
                    return password;
                }
                set
                {
                    password = value;
                }
            }

            public bool PersistSecurityInfo
            {
                get
                {
                    return persistSecurityInfo;
                }
                set
                {
                    persistSecurityInfo = value;
                }
            }

            public int MinPoolSize
            {
                get
                {
                    return minPoolSize;
                }
                set
                {
                    minPoolSize = value;
                }
            }

            public int MaxPoolSize
            {
                get
                {
                    return maxPoolSize;
                }
                set
                {
                    maxPoolSize = value;
                }
            }

            public bool Enlist
            {
                get
                {
                    return enlist;
                }
                set
                {
                    enlist = value;
                }
            }

            public bool Async
            {
                get
                {
                    return async;
                }
                set
                {
                    async = value;
                }
            }

            public bool MultipleActiveResultSets
            {
                get
                {
                    return multipleActiveResultSets;
                }
                set
                {
                    multipleActiveResultSets = value;
                }
            }

            #endregion

            #region Constructors

            /// <summary>
            /// Constructor of the ConnectionStructure Structure. The minimum set of values to instantiate the structure are the 
            /// parameters passed to constructor. We always need a datasource, initial catalog, userID, password.
            /// The rest is set to the defaults unless explicitely overriden later on the structure.
            /// Here are the following values that are set by default:
            /// <list type="">
            /// <item>TrustedConnection = false;</item>
            /// <item>ConnectionTimeout = 60;</item>
            /// <item>PersistSecurityInfo = false;</item>
            /// <item>Min Pool Size = 3;</item>
            /// <item>Max Pool Size = 25;</item>
            /// <item>Enlist = false; (Inidicates whether the connection participates in transaction)</item>
            /// <item>MultipleActiveResultSets = false;</item>
            /// </list>
            /// <remarks>The rest of the connection string properties default to the settings predefined on it</remarks>
            /// 
            /// </summary>
            /// <param name="dataSource">IP or the name of the SQL Server</param>
            /// <param name="initialCatalog">Database name</param>
            /// <param name="userID">user id</param>
            /// <param name="password">user password</param>
            public ConnectionStructure(string dataSource, string initialCatalog, string userID, string password)
            {
                this.dataSource = dataSource;
                this.initialCatalog = initialCatalog;
                this.userID = userID;
                this.password = password;
                connectionTimeout = 60;
                trustedConnection = false;
                persistSecurityInfo = false;
                minPoolSize = 3;
                maxPoolSize = 15;
                enlist = false;
                async = false;
                multipleActiveResultSets = false;
            }

            #endregion
        }

        #endregion

        #region Enumerations

        /// <summary>
        /// This enumeration is used to basically determine when to close the DataReader object
        /// </summary>
        public enum SqlConnectionOwnership
        {
            Internal = 0,
            External = 1
        }

        #endregion

        #region Public Static Methods

        public static Decimal SafeGetDecimal(IDataRecord rd, int columnOrdinal, Decimal defaultValue)
        {
            if (rd.IsDBNull(columnOrdinal))
                return defaultValue;
            return rd.GetDecimal(columnOrdinal);
        }

        public static DateTime SafeGetDateTime(IDataRecord rd, int columnOrdinal, DateTime defaultValue)
        {
            if (rd.IsDBNull(columnOrdinal))
                return defaultValue;
            return rd.GetDateTime(columnOrdinal);
        }
        public static Boolean SafeGetBoolean(IDataRecord rd, int columnOrdinal, Boolean defaultValue)
        {
            if (rd.IsDBNull(columnOrdinal))
                return defaultValue;
            return rd.GetBoolean(columnOrdinal);
        }

        public static byte SafeGetTinyInt(IDataRecord rd, int columnOrdinal, byte defaultValue)
        {
            if (rd.IsDBNull(columnOrdinal))
                return defaultValue;
            return rd.GetByte(columnOrdinal);
        }
        public static int SafeGetInt32(IDataRecord rd, int columnOrdinal, Int32 defaultValue)
        {
            if (rd.IsDBNull(columnOrdinal))
                return defaultValue;
            return rd.GetInt32(columnOrdinal);
        }


        public static short SafeGetShort(IDataRecord rd, int columnOrdinal, short defaultValue)
        {
            if (rd.IsDBNull(columnOrdinal))
                return defaultValue;
            return rd.GetInt16(columnOrdinal);
        }
        public static double SafeGetDouble(IDataRecord rd, int columnOrdinal, double defaultValue)
        {
            if (rd.IsDBNull(columnOrdinal))
                return defaultValue;
            return rd.GetDouble(columnOrdinal);
        }
        public static char SafeGetChar(IDataRecord rd, int columnOrdinal, char defaultValue)
        {
            if (rd.IsDBNull(columnOrdinal))
                return defaultValue;
            string v = rd.GetString(columnOrdinal);
            char[] va = v.ToCharArray();
            if (v.Length > 0)
                return va[0];

            return defaultValue;
        }
        

        #region Insert Identity

        /// <summary>
        /// Inserts record and returns the identity of the inserted record. Executes dynamic query.  
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement to execute</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>Identity of the inserted record</returns>
        public static int InsertIdentity(string connectionString, string sqlQuery, SqlParameter[] commandParameters)
        {
            int Return = 0;

            SqlDataReader objDataReader;
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery + "; SELECT SCOPE_IDENTITY();";
            objCommand.CommandType = CommandType.Text;
            SqlConnection objConnection = new SqlConnection();
            try
            {
                objConnection = OpenConnection(objConnection, connectionString);

                objCommand.Connection = objConnection;
                if (commandParameters != null)
                {
                    AttachParameters(objCommand, commandParameters);
                }

                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                if (objDataReader.Read())
                {
                    Return = Convert.ToInt32(objDataReader.GetValue(0));
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return Return;
        }

        /// <summary>
        /// Inserts record and returns the identity of the inserted record. Executes dynamic query.
        /// </summary>
        /// <param name="connectionStructure"></param>
        /// <param name="sqlQuery">Sql statement to execute</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>Identity of the inserted record</returns>
        public static int InsertIdentity(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters)
        {
            int Return;

            Return = InsertIdentity(GetConnectionString(connectionStructure), sqlQuery, commandParameters);

            return Return;
        }

        /// <summary>
        /// Inserts record and returns the identity of the inserted record. Executes dynamic query.  
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement to execute</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Identity of the inserted record</returns>
        public static int InsertIdentity(string connectionString, string sqlQuery, SqlParameter[] commandParameters, int commandTimeout)
        {
            int Return = 0;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery + "; SELECT SCOPE_IDENTITY();";
            objCommand.CommandType = CommandType.Text;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            SqlDataReader objDataReader;
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                if (objDataReader.Read())
                {
                    Return = Convert.ToInt32(objDataReader.GetValue(0));
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return Return;
        }

        /// <summary>
        /// Inserts record and returns the identity of the inserted record. Executes dynamic query.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement to execute</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">SQL parameters array</param>
        /// <returns>Identity of the inserted record</returns>
        public static int InsertIdentity(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, int commandTimeout)
        {
            int Return;

            Return = InsertIdentity(GetConnectionString(connectionStructure), sqlQuery, commandParameters, commandTimeout);

            return Return;
        }

        /// <summary>
        /// Inserts record and returns the identity of the inserted record. Executes stored procedure.  
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Name of the stored procedure</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>Identity of the inserted record</returns>
        public static int InsertIdentityStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters)
        {
            int Return = 0;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            SqlDataReader objDataReader;
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                if (objDataReader.Read())
                {
                    Return = Convert.ToInt32(objDataReader.GetValue(0));
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return Return;
        }

        /// <summary>
        /// Inserts record and returns the identity of the inserted record. Executes stored procedure.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Name of the stored procedure</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>Identity of the inserted record</returns>
        public static int InsertIdentityStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters)
        {
            int Return;

            Return = InsertIdentityStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters);

            return Return;
        }

        /// <summary>
        /// Inserts record and returns the identity of the inserted record. Executes stored procedure. 
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Name of the stored procedure</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Identity of the inserted record</returns>
        public static int InsertIdentityStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, int commandTimeout)
        {
            int Return = 0;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            SqlDataReader objDataReader;
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                if (objDataReader.Read())
                {
                    Return = Convert.ToInt32(objDataReader.GetValue(0));
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return Return;
        }

        /// <summary>
        /// Inserts record and returns the identity of the inserted record. Executes stored procedure. 
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Name of the stored procedure</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Identity of the inserted record</returns>
        public static int InsertIdentityStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, int commandTimeout)
        {
            int Return;

            Return = InsertIdentityStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, commandTimeout);

            return Return;
        }

        #endregion

        #region Hashtable

        /// <summary>
        /// Returns hashtable object. Executes dynamic query.  
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement to execute</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>Hashtable</returns>
        /// <param name="objHashtable"></param>
        public static Hashtable GetHashtable(string connectionString, string sqlQuery, SqlParameter[] commandParameters, Hashtable objHashtable)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = CommandType.Text;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objHashtable.Add(objDataReader[0].ToString(), objDataReader[1].ToString());
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objHashtable;
        }

        /// <summary>
        /// Returns hashtable object. Executes dynamic query.  
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement to execute</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objHashtable">Hashtable to fill with data</param>
        /// <returns>Hashtable</returns>
        public static Hashtable GetHashtable(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, Hashtable objHashtable)
        {
            objHashtable = GetHashtable(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objHashtable);

            return objHashtable;
        }

        /// <summary>
        /// Gets hashtable object. Executes dynamic query.  
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement to execute</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objHashtable">Instantiated Hashtable object</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Hashtable</returns>
        public static Hashtable GetHashtable(string connectionString, string sqlQuery, SqlParameter[] commandParameters, Hashtable objHashtable, int commandTimeout)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = CommandType.Text;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objHashtable.Add(objDataReader[0].ToString(), objDataReader[1].ToString());
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objHashtable;
        }

        /// <summary>
        /// Gets hashtable object. Executes dynamic query
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement to execute</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objHashtable">Instantiated Hashtable object</param>
        /// <param name="commandTimeout">Command Timeout</param>
        /// <returns>Hashtable</returns>
        public static Hashtable GetHashtable(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, Hashtable objHashtable, int commandTimeout)
        {
            objHashtable = GetHashtable(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objHashtable, commandTimeout);

            return objHashtable;
        }

        /// <summary>
        /// Executes dynamic query and fills hashtable object with data. 
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Name of the stored procedure</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objHashtable">Hashtable object</param>
        /// <returns>Hashtable</returns>
        public static Hashtable GetHashtableStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, Hashtable objHashtable)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objHashtable.Add(objDataReader[0].ToString(), objDataReader[1].ToString());
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objHashtable;
        }

        /// <summary>
        /// Executes dynamic query and fills hashtable object with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Name of the stored procedure</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objHashtable">Hashtable object</param>
        /// <returns>Hashtable</returns>
        public static Hashtable GetHashtableStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, Hashtable objHashtable)
        {
            objHashtable = GetHashtableStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objHashtable);

            return objHashtable;
        }

        /// <summary>
        /// Executes stored procedure and fills hashtable object with data. 
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Name of the stored procedure</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objHashtable">Hashtable object</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Hashtable</returns>
        public static Hashtable GetHashtableStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, Hashtable objHashtable, int commandTimeout)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            SqlConnection objConnection = new SqlConnection();
            objCommand.CommandTimeout = commandTimeout;
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objHashtable.Add(objDataReader[0].ToString(), objDataReader[1].ToString());
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objHashtable;
        }

        /// <summary>
        /// Executes stored procedure and fills hashtable object with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Name of the stored procedure</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objHashtable">Hashtable object</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Hashtable</returns>
        public static Hashtable GetHashtableStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, Hashtable objHashtable, int commandTimeout)
        {
            objHashtable = GetHashtableStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objHashtable, commandTimeout);

            return objHashtable;
        }

        #endregion

        #region Execute NonQuery

        /// <summary>
        /// Executes dynamic query.  
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">SQL statement to execute</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>Number of Records affected</returns>
        public static int ExecuteNonQuery(string connectionString, string sqlQuery, SqlParameter[] commandParameters)
        {
            int Return;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = CommandType.Text;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            try
            {
                Return = objCommand.ExecuteNonQuery();
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return Return;
        }

        /// <summary>
        /// Executes non query
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">SQL statement to execute</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>Number of Records affected</returns>
        public static int ExecuteNonQuery(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters)
        {
            int Return;

            Return = ExecuteNonQuery(GetConnectionString(connectionStructure), sqlQuery, commandParameters);

            return Return;
        }

        /// <summary>
        /// Executes dynamic query (non query).  
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">SQL statement to execute</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns> Number of rows affected</returns>
        public static int ExecuteNonQuery(string connectionString, string sqlQuery, SqlParameter[] commandParameters, int commandTimeout)
        {
            int Return;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = CommandType.Text;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            try
            {
                Return = objCommand.ExecuteNonQuery();
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return Return;
        }

        /// <summary>
        /// Executes dynamic query (non query)
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">SQL statement to execute</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Number of Records affected</returns>
        public static int ExecuteNonQuery(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, int commandTimeout)
        {
            int Return;

            Return = ExecuteNonQuery(GetConnectionString(connectionStructure), sqlQuery, commandParameters, commandTimeout);

            return Return;
        }

        /// <summary>
        /// Executes stored procedure (non query)  
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>Number of records affected</returns>
        public static int ExecuteNonQueryStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters)
        {
            int Return;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                Return = objCommand.ExecuteNonQuery();
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return Return;
        }

        /// <summary>
        /// Executes stored procedure (non query)
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>Number of Records affected</returns>
        public static int ExecuteNonQueryStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters)
        {
            int Return;

            Return = ExecuteNonQueryStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters);

            return Return;
        }

        /// <summary>
        /// Executes stored procedure.  
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Number of records affected</returns>
        public static int ExecuteNonQueryStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, int commandTimeout)
        {
            int Return;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                Return = objCommand.ExecuteNonQuery();
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return Return;
        }

        /// <summary>
        /// Executes stored procedure (non query)
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Number of records affected</returns>
        public static int ExecuteNonQueryStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, int commandTimeout)
        {
            int Return;

            Return = ExecuteNonQueryStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, commandTimeout);

            return Return;
        }

        #endregion

        #region SqlDataReader
        public static SqlDataReader GetDataReader(string connectionString, string sqlQuery, IEnumerable<SqlParameter> commandParameters)
        {
            SqlCommand objCommand = new SqlCommand {CommandText = sqlQuery};
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }

            SqlDataReader objDataReader;
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
            }
            return objDataReader;
        }

        /// <summary>
        /// Executes sql statement, puts results into DataReader  
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">SQL Statement to execute</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>SqlDataReader</returns>
        /// <param name="objDataReader"></param>
        public static SqlDataReader GetDataReader(string connectionString, string sqlQuery, SqlParameter[] commandParameters, SqlDataReader objDataReader)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                //AttachParametersStoredProc(objCommand, commandParameters);
                AttachParameters(objCommand, commandParameters);
            }            
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
            }
            return objDataReader;
        }

        /// <summary>
        /// Executes sql statement, puts results into DataReader 
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">SQL Statement to execute</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataReader">Instance of SqlDataReader object</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader GetDataReader(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, SqlDataReader objDataReader)
        {
            objDataReader = GetDataReader(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objDataReader);

            return objDataReader;
        }
        public static SqlDataReader GetDataReaderStoredProc(string connectionString, string storedProcName, IEnumerable<SqlParameter> commandParameters)
        {
            SqlCommand objCommand = new SqlCommand { CommandText = storedProcName, CommandType = CommandType.StoredProcedure };
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParametersStoredProc(objCommand, commandParameters);
                objCommand.Prepare();
            }

            SqlDataReader objDataReader;
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
            }
            return objDataReader;
        }

        /// <summary>
        /// Executes stored procedure, puts results into SqlDataReader  
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>SqlDataReader</returns>
        /// <param name="objDataReader"></param>
        public static SqlDataReader GetDataReaderStoredProc(string connectionString, string storedProcName, IEnumerable<SqlParameter> commandParameters, SqlDataReader objDataReader)
        {
            SqlCommand objCommand = new SqlCommand
                                        {CommandText = storedProcName, CommandType = CommandType.StoredProcedure};
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParametersStoredProc(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
            }
            return objDataReader;
        }

        /// <summary>
        /// Executes stored procedure, puts results into SqlDataReader  
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataReader">Instance of SqlDataReader</param>
        /// <returns>SqlDataReader Instance</returns>
        public static SqlDataReader GetDataReaderStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, SqlDataReader objDataReader)
        {
            objDataReader = GetDataReaderStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objDataReader);

            return objDataReader;
        }

        /// <summary>
        /// Executes SQL Statement, puts results into SqlDataReader  
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">SQL statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataReader"></param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>DataReader</returns>
        public static SqlDataReader GetDataReader(string connectionString, string sqlQuery, SqlParameter[] commandParameters, SqlDataReader objDataReader, int commandTimeout)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParametersStoredProc(objCommand, commandParameters);
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
            }
            return objDataReader;
        }

        /// <summary>
        /// Executes SQL Statement, puts results into SqlDataReader  
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">SQL statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataReader">Instance of SqlDataReader</param>
        /// <param name="commandTimeout">Command Timeout</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader GetDataReader(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, SqlDataReader objDataReader, int commandTimeout)
        {
            objDataReader = GetDataReader(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objDataReader, commandTimeout);

            return objDataReader;
        }

        /// <summary>
        /// Executes stored procedure, puts results into DataReader  
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataReader"></param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader GetDataReaderStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, SqlDataReader objDataReader, int commandTimeout)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParametersStoredProc(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
            }
            return objDataReader;
        }

        /// <summary>
        /// Executes stored procedure, puts results into DataReader  
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataReader"></param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader GetDataReaderStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, SqlDataReader objDataReader, int commandTimeout)
        {
            objDataReader = GetDataReaderStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objDataReader, commandTimeout);

            return objDataReader;
        }

        #endregion

        #region SqlDataReader Single Row

        /// <summary>
        /// Executes sql statement, puts results into DataReader. 
        /// Call when you know that the resultset is a single row
        /// that is selected based on the primary key.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">SQL Statement to execute</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>SqlDataReader</returns>
        /// <param name="objDataReader"></param>
        public static SqlDataReader GetDataReaderSingleRow(string connectionString, string sqlQuery, SqlParameter[] commandParameters, SqlDataReader objDataReader)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParametersStoredProc(objCommand, commandParameters);
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection | CommandBehavior.SingleRow);
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
            }
            return objDataReader;
        }

        /// <summary>
        /// Executes sql statement, puts results into DataReader 
        /// Call when you know that the resultset is a single row
        /// that is selected based on the primary key.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">SQL Statement to execute</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataReader">Instance of SqlDataReader object</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader GetDataReaderSingleRow(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, SqlDataReader objDataReader)
        {
            objDataReader = GetDataReaderSingleRow(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objDataReader);

            return objDataReader;
        }

        /// <summary>
        /// Executes stored procedure, puts results into SqlDataReader.  
        /// Call when you know that the resultset is a single row
        /// that is selected based on the primary key.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>SqlDataReader</returns>
        /// <param name="objDataReader"></param>
        public static SqlDataReader GetDataReaderSingleRowStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, SqlDataReader objDataReader)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParametersStoredProc(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection | CommandBehavior.SingleRow);
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
            }
            return objDataReader;
        }

        /// <summary>
        /// Executes stored procedure, puts results into SqlDataReader.
        /// Call when you know that the resultset is a single row
        /// that is selected based on the primary key.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataReader">Instance of SqlDataReader</param>
        /// <returns>SqlDataReader Instance</returns>
        public static SqlDataReader GetDataReaderSingleRowStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, SqlDataReader objDataReader)
        {
            objDataReader = GetDataReaderSingleRowStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objDataReader);

            return objDataReader;
        }

        /// <summary>
        /// Executes SQL Statement, puts results into SqlDataReader.
        /// Call when you know that the resultset is a single row
        /// that is selected based on the primary key.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">SQL statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataReader"></param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>DataReader</returns>
        public static SqlDataReader GetDataReaderSingleRow(string connectionString, string sqlQuery, SqlParameter[] commandParameters, SqlDataReader objDataReader, int commandTimeout)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParametersStoredProc(objCommand, commandParameters);
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection | CommandBehavior.SingleRow);
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
            }
            return objDataReader;
        }

        /// <summary>
        /// Executes SQL Statement, puts results into SqlDataReader.
        /// Call when you know that the resultset is a single row
        /// that is selected based on the primary key.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">SQL statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataReader">Instance of SqlDataReader</param>
        /// <param name="commandTimeout">Command Timeout</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader GetDataReaderSingleRow(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, SqlDataReader objDataReader, int commandTimeout)
        {
            objDataReader = GetDataReaderSingleRow(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objDataReader, commandTimeout);

            return objDataReader;
        }

        /// <summary>
        /// Executes stored procedure, puts results into DataReader.
        /// Call when you know that the resultset is a single row
        /// that is selected based on the primary key.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataReader"></param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader GetDataReaderSingleRowStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, SqlDataReader objDataReader, int commandTimeout)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParametersStoredProc(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection | CommandBehavior.SingleRow);
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
            }
            return objDataReader;
        }

        /// <summary>
        /// Executes stored procedure, puts results into DataReader.
        /// Call when you know that the resultset is a single row
        /// that is selected based on the primary key.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataReader"></param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader GetDataReaderSingleRowStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, SqlDataReader objDataReader, int commandTimeout)
        {
            objDataReader = GetDataReaderSingleRowStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objDataReader, commandTimeout);

            return objDataReader;
        }

        #endregion

        #region DataSet Stored Procedure

        /// <summary>
        /// Creates a DataSet by running the stored procedure and placing the results
        /// of the query/proc into the given tablename.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Name of stored procedure</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="tableName">Name of the table to return</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcedure(string connectionString, string storedProcName, SqlParameter[] commandParameters, string tableName)
        {
            DataSet objDataSet = new DataSet();

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParametersStoredProc(objCommand, commandParameters);
                objCommand.Prepare();
            }
            SqlDataAdapter objDataAdapter = new SqlDataAdapter(objCommand);
            try
            {
                objDataAdapter.Fill(objDataSet, tableName);
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                objDataAdapter.Dispose();
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objDataSet;
        }

        /// <summary>
        /// Creates a DataSet by running the stored procedure and placing the results
        /// of the query/proc into the given tablename.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Name of stored procedure</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="tableName">Name of the table to return</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcedure(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, string tableName)
        {
            DataSet objDataSet = RunProcedure(GetConnectionString(connectionStructure), storedProcName, commandParameters, tableName);

            return objDataSet;
        }

        /// <summary>
        /// Creates a DataSet by running the stored procedure and placing the results
        /// of the query/proc into the given tablename.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Name of stored procedure</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="tableName">Name of the table to return</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcedure(string connectionString, string storedProcName, SqlParameter[] commandParameters, string tableName, int commandTimeout)
        {
            DataSet objDataSet = new DataSet();

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParametersStoredProc(objCommand, commandParameters);
                objCommand.Prepare();
            }
            SqlDataAdapter objDataAdapter = new SqlDataAdapter(objCommand);
            try
            {
                objDataAdapter.Fill(objDataSet, tableName);
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                objDataAdapter.Dispose();
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objDataSet;
        }

        /// <summary>
        /// Creates a DataSet by running the stored procedure and placing the results
        /// of the query/proc into the given tablename.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Name of stored procedure</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="tableName">Name of the table to return</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcedure(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, string tableName, int commandTimeout)
        {
            DataSet objDataSet = RunProcedure(GetConnectionString(connectionStructure), storedProcName, commandParameters, tableName, commandTimeout);

            return objDataSet;
        }

        #endregion

        #region Int Stored Procedure

        /// <summary>
        /// Runs stored procedure.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Name of stored procedure</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="rowsAffected">Ouput parameters for affected rows</param>
        /// <returns>Integer</returns>
        public static int RunProcedure(string connectionString, string storedProcName, SqlParameter[] commandParameters, out int rowsAffected)
        {
            int Return;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParametersStoredProc(objCommand, commandParameters);
                objCommand.Parameters.Add(new SqlParameter("ReturnValue",
                                                           SqlDbType.Int,
                                                           4, /* Size */
                                                           ParameterDirection.ReturnValue,
                                                           false, /* is nullable */
                                                           0, /* byte precision */
                                                           0, /* byte scale */
                                                           string.Empty,
                                                           DataRowVersion.Default,
                                                           null));
                objCommand.Prepare();
            }
            try
            {
                rowsAffected = objCommand.ExecuteNonQuery();
                Return = (int)objCommand.Parameters["ReturnValue"].Value;
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return Return;
        }

        /// <summary>
        /// Runs stored procedure
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Name of stored procedure</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="rowsAffected">Ouput parameters for affected rows</param>
        /// <returns>Integer</returns>
        public static int RunProcedure(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, out int rowsAffected)
        {
            int Return = RunProcedure(GetConnectionString(connectionStructure), storedProcName, commandParameters, out rowsAffected);

            return Return;
        }

        /// <summary>
        /// Runs stored procedure.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Name of stored procedure</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <param name="rowsAffected">Ouput parameters for affected rows</param>
        /// <returns>Integer</returns>
        public static int RunProcedure(string connectionString, string storedProcName, SqlParameter[] commandParameters, int commandTimeout, out int rowsAffected)
        {
            int Return;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParametersStoredProc(objCommand, commandParameters);
                objCommand.Parameters.Add(new SqlParameter("ReturnValue",
                                                           SqlDbType.Int,
                                                           4, /* Size */
                                                           ParameterDirection.ReturnValue,
                                                           false, /* is nullable */
                                                           0, /* byte precision */
                                                           0, /* byte scale */
                                                           string.Empty,
                                                           DataRowVersion.Default,
                                                           null));
                objCommand.Prepare();
            }
            try
            {
                rowsAffected = objCommand.ExecuteNonQuery();
                Return = (int)objCommand.Parameters["ReturnValue"].Value;
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return Return;
        }

        /// <summary>
        /// Run Stored Procedure
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Name of stored procedure</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <param name="rowsAffected">Ouput parameters for affected rows</param>
        /// <returns>Integer</returns>
        public static int RunProcedure(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, int commandTimeout, out int rowsAffected)
        {
            int Return = RunProcedure(GetConnectionString(connectionStructure), storedProcName, commandParameters, commandTimeout, out rowsAffected);

            return Return;
        }

        #endregion

        #region Stored Procedure Void Return Type

        //TATIANA TO DO: start here
        /// <summary>
        /// Takes an -existing- dataset and fills the given table name with the results
        /// of the stored procedure
        /// </summary>
        /// <param name="connectionString">Connection String to Database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters"></param>
        /// <param name="objDataSet"></param>
        /// <param name="tableName"></param>
        /// <returns>DataSet</returns>
        public static void RunProcedure(string connectionString, string storedProcName, SqlParameter[] commandParameters, DataSet objDataSet, string tableName)
        {

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParametersStoredProc(objCommand, commandParameters);
                objCommand.Parameters.Add(new SqlParameter("ReturnValue",
                                                           SqlDbType.Int,
                                                           4, /* Size */
                                                           ParameterDirection.ReturnValue,
                                                           false, /* is nullable */
                                                           0, /* byte precision */
                                                           0, /* byte scale */
                                                           string.Empty,
                                                           DataRowVersion.Default,
                                                           null));
                objCommand.Prepare();
            }
            SqlDataAdapter objDataAdapter = new SqlDataAdapter(objCommand);
            try
            {
                objDataAdapter.Fill(objDataSet, tableName);
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                objDataAdapter.Dispose();
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
        }

        //TATIANA TO DO: start here
        /// <summary>
        /// Takes an -existing- dataset and fills the given table name with the results
        /// of the stored procedure
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters"></param>
        /// <param name="objDataSet"></param>
        /// <param name="tableName"></param>
        /// <returns>DataSet</returns>
        public static void RunProcedure(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, DataSet objDataSet, string tableName)
        {

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            SqlConnection objConnection = new SqlConnection();
            string ConnectionString = GetConnectionString(connectionStructure);
            objConnection = OpenConnection(objConnection, ConnectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParametersStoredProc(objCommand, commandParameters);
                objCommand.Parameters.Add(new SqlParameter("ReturnValue",
                                                           SqlDbType.Int,
                                                           4, /* Size */
                                                           ParameterDirection.ReturnValue,
                                                           false, /* is nullable */
                                                           0, /* byte precision */
                                                           0, /* byte scale */
                                                           string.Empty,
                                                           DataRowVersion.Default,
                                                           null));
                objCommand.Prepare();
            }
            if (objDataSet == null) throw new ArgumentNullException("objDataSet");
            SqlDataAdapter objDataAdapter = new SqlDataAdapter(objCommand);
            try
            {
                objDataAdapter.Fill(objDataSet, tableName);
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                objDataAdapter.Dispose();
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
        }

        #endregion

        #region Execute Scalar

        /// <summary>
        /// Executes Scalar on the aggregate function. 
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql Statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>object that contains the result of the aggregate calculation</returns>
        public static object ExecuteScalar(string connectionString, string sqlQuery, SqlParameter[] commandParameters)
        {
            object Return;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = CommandType.Text;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;

            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            try
            {
                Return = objCommand.ExecuteScalar();
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return Return;
        }

        /// <summary>
        /// Executes Scalar on the aggregate function
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql Statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>object that contains the result of the aggregate calculation</returns>
        public static object ExecuteScalar(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters)
        {
            object Return = ExecuteScalar(GetConnectionString(connectionStructure), sqlQuery, commandParameters);

            return Return;
        }

        /// <summary>
        /// Executes Scalar on the aggregate function. 
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql Statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>object that contains the result of the aggregate calculation</returns>
        public static object ExecuteScalar(string connectionString, string sqlQuery, SqlParameter[] commandParameters, int commandTimeout)
        {
            object Return;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = CommandType.Text;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;

            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            try
            {
                Return = objCommand.ExecuteScalar();
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return Return;
        }

        /// <summary>
        /// Executes Scalar on the aggregate function
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql Statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>object that contains the result of the aggregate calculation</returns>
        public static object ExecuteScalar(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, int commandTimeout)
        {
            object Return = ExecuteScalar(GetConnectionString(connectionStructure), sqlQuery, commandParameters, commandTimeout);

            return Return;
        }

        /// <summary>
        /// Executes Scalar on the aggregate function. Uses stored procedure.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>object that contains the result of the aggregate calculation</returns>
        public static object ExecuteScalarStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters)
        {
            object Return;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;

            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                Return = objCommand.ExecuteScalar();
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return Return;
        }

        /// <summary>
        /// Executes Scalar on the aggregate function. Uses stored procedure
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>object that contains the result of the aggregate calculation</returns>
        public static object ExecuteScalarStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters)
        {
            object Return = ExecuteScalarStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters);

            return Return;
        }

        /// <summary>
        /// Executes Scalar on the aggregate function. Uses stored procedure.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>object that contains the result of the aggregate calculation</returns>
        public static object ExecuteScalarStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, int commandTimeout)
        {
            object Return;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;

            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                Return = objCommand.ExecuteScalar();
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return Return;
        }

        /// <summary>
        /// Executes Scalar on the aggregate function. Uses stored procedure
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>object that contains the result of the aggregate calculation</returns>
        public static object ExecuteScalarStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, int commandTimeout)
        {
            object Return = ExecuteScalarStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, commandTimeout);

            return Return;
        }

        #endregion

        #region DataTable

        /// <summary>
        /// Executes dynamic SQL statement and fills DataTable with data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataTable">DataTable object</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable(string connectionString, string sqlQuery, SqlParameter[] commandParameters, DataTable objDataTable)
        {
            SqlDataReader objDataReader = null;

            try
            {
                objDataReader = GetDataReader(connectionString, sqlQuery, commandParameters, objDataReader);
                objDataTable.Load(objDataReader, LoadOption.OverwriteChanges);
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            //finally
            //{
            //    //objDataReader = null;
            //}
            return objDataTable;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills DataTable with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataTable">DataTable object</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, DataTable objDataTable)
        {
            objDataTable = GetDataTable(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objDataTable);

            return objDataTable;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills DataTable with data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataTable">DataTable object</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable(string connectionString, string sqlQuery, SqlParameter[] commandParameters, DataTable objDataTable, int commandTimeout)
        {
            SqlDataReader objDataReader = null;

            try
            {
                objDataReader = GetDataReader(connectionString, sqlQuery, commandParameters, objDataReader, commandTimeout);
                //turn off the constrain and internal maintenance checking to boost performance
                objDataTable.BeginLoadData();
                objDataTable.Load(objDataReader, LoadOption.OverwriteChanges);
                //turn constrain and internal maintenance back on
                objDataTable.EndLoadData();
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            //finally
            //{
            //    //objDataReader = null;
            //}
            return objDataTable;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills DataTable with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataTable">DataTable object</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, DataTable objDataTable, int commandTimeout)
        {
            objDataTable = GetDataTable(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objDataTable, commandTimeout);

            return objDataTable;
        }

        /// <summary>
        /// Executes stored procedure and fills DataTable with data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>DataTable</returns>
        /// <param name="objDataTable"></param>
        public static DataTable GetDataTableStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, DataTable objDataTable)
        {
            SqlDataReader objDataReader = null;

            try
            {
                objDataReader = GetDataReaderStoredProc(connectionString, storedProcName, commandParameters, objDataReader);
                //turn off the constrain and internal maintenance checking to boost performance
                objDataTable.BeginLoadData();
                objDataTable.Load(objDataReader, LoadOption.OverwriteChanges);
                //turn constrain and internal maintenance back on
                objDataTable.EndLoadData();
                if (!objDataReader.IsClosed)
                    objDataReader.Close();
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            //finally
            //{
            //    ////objDataReader = null;
            //}
            return objDataTable;
        }

        /// <summary>
        /// Executes stored procedure and fills DataTable with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataTable"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTableStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, DataTable objDataTable)
        {
            objDataTable = GetDataTableStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objDataTable);

            return objDataTable;
        }

        /// <summary>
        /// Executes stored procedure and fills DataTable with data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataTable"></param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTableStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, DataTable objDataTable, int commandTimeout)
        {
            SqlDataReader objDataReader = null;

            try
            {
                objDataReader = GetDataReaderStoredProc(connectionString, storedProcName, commandParameters, objDataReader, commandTimeout);
                //turn off the constrain and internal maintenance checking to boost performance
                objDataTable.BeginLoadData();
                objDataTable.Load(objDataReader, LoadOption.OverwriteChanges);
                //turn constrain and internal maintenance back on
                objDataTable.EndLoadData();
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            //finally
            //{
            //    ////objDataReader = null;
            //}
            return objDataTable;
        }

        /// <summary>
        /// Executes stored procedure and fills DataTable with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <param name="objDataTable"></param>
        /// <param name="commandTimeout"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTableStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, DataTable objDataTable, int commandTimeout)
        {
            objDataTable = GetDataTableStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objDataTable, commandTimeout);

            return objDataTable;
        }

        #endregion

        #region DataSet

        /// <summary>
        /// Executes dynamic SQL statement and fills DataSet with data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataSet">DataSet to fill with data</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSet(string connectionString, string sqlQuery, SqlParameter[] commandParameters, DataSet objDataSet)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;

            objCommand.CommandType = CommandType.Text;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            SqlDataAdapter objDataAdapter = new SqlDataAdapter(objCommand);
            try
            {
                objDataAdapter.Fill(objDataSet);
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                objDataAdapter.Dispose();
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objDataSet;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills DataSet with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataSet">DataSet to fill with data</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSet(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, DataSet objDataSet)
        {
            objDataSet = GetDataSet(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objDataSet);

            return objDataSet;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills DataSet with data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataSet">DataSet to fill with data</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSet(string connectionString, string sqlQuery, SqlParameter[] commandParameters, DataSet objDataSet, int commandTimeout)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandTimeout = commandTimeout;
            objCommand.CommandType = CommandType.Text;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            SqlDataAdapter objDataAdapter = new SqlDataAdapter(objCommand);
            try
            {
                objDataAdapter.Fill(objDataSet);
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                objDataAdapter.Dispose();
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objDataSet;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills DataSet with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataSet">DataSet to fill with data</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSet(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, DataSet objDataSet, int commandTimeout)
        {
            objDataSet = GetDataSet(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objDataSet, commandTimeout);

            return objDataSet;
        }

        #endregion

        #region DataSet Stored Procedure

        /// <summary>
        /// Executes stored procedure and fills DataSet with data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataSet">DataSet to fill with data</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSetStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, DataSet objDataSet)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            SqlDataAdapter objDataAdapter = new SqlDataAdapter(objCommand);
            try
            {
                objDataAdapter.Fill(objDataSet);
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                objDataAdapter.Dispose();
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objDataSet;
        }

        /// <summary>
        /// Executes stored procedure and fills DataSet with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataSet">DataSet to fill with data</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSetStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, DataSet objDataSet)
        {
            objDataSet = GetDataSetStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objDataSet);

            return objDataSet;
        }

        /// <summary>
        /// Executes stored procedure and fills DataSet with data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataSet">DataSet to fill with data</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSetStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, DataSet objDataSet, int commandTimeout)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            SqlDataAdapter objDataAdapter = new SqlDataAdapter(objCommand);
            try
            {
                objDataAdapter.Fill(objDataSet);
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                objDataAdapter.Dispose();
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objDataSet;
        }

        /// <summary>
        /// Executes stored procedure and fills DataSet with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataSet">DataSet to fill with data</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSetStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, DataSet objDataSet, int commandTimeout)
        {
            objDataSet = GetDataSetStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objDataSet, commandTimeout);

            return objDataSet;
        }

        #endregion

        #region DataRow

        /// <summary>
        /// Executes dynamic SQL statement and fills DataRow with data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataRow">DataRow</param>
        /// <returns>DataRow</returns>
        public static DataRow GetDataRow(string connectionString, string sqlQuery, SqlParameter[] commandParameters, DataRow objDataRow)
        {
            DataTable objDataTable = new DataTable();
            try
            {
                objDataTable = GetDataTable(connectionString, sqlQuery, commandParameters, objDataTable);
                if (/*(objDataTable != null) && */(objDataTable.Rows.Count > 0))
                {
                    objDataRow = objDataTable.Rows[0];
                }
            }
            catch (IndexOutOfRangeException)
            {
                objDataRow = null;
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (objDataTable != null)
                {
                    objDataTable.Dispose();
                }
            }
            return objDataRow;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills DataRow with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataRow">DataRow</param>
        /// <returns>DataRow</returns>
        public static DataRow GetDataRow(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, DataRow objDataRow)
        {
            objDataRow = GetDataRow(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objDataRow);

            return objDataRow;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills DataRow with data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataRow">DataRow</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>DataRow</returns>
        public static DataRow GetDataRow(string connectionString, string sqlQuery, SqlParameter[] commandParameters, DataRow objDataRow, int commandTimeout)
        {
            DataTable objDataTable = new DataTable();
            try
            {
                objDataTable = GetDataTable(connectionString, sqlQuery, commandParameters, objDataTable, commandTimeout);
                if (/*(objDataTable != null) && */(objDataTable.Rows.Count > 0))
                {
                    objDataRow = objDataTable.Rows[0];
                }
            }
            catch (IndexOutOfRangeException)
            {
                objDataRow = null;
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (objDataTable != null)
                {
                    objDataTable.Dispose();
                }
            }
            return objDataRow;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills DataRow with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataRow">DataRow</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>DataRow</returns>
        public static DataRow GetDataRow(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, DataRow objDataRow, int commandTimeout)
        {
            objDataRow = GetDataRow(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objDataRow, commandTimeout);

            return objDataRow;
        }

        /// <summary>
        /// Executes SQL stored procedure and fills DataRow with data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Name of stored procedure to execute</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataRow">DataRow</param>
        /// <returns>DataRow</returns>
        public static DataRow GetDataRowStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, DataRow objDataRow)
        {
            DataTable objDataTable = new DataTable();
            try
            {
                objDataTable = GetDataTableStoredProc(connectionString, storedProcName, commandParameters, objDataTable);
                if (/*(objDataTable != null) && */(objDataTable.Rows.Count > 0))
                {
                    objDataRow = objDataTable.Rows[0];
                }
            }
            catch (IndexOutOfRangeException)
            {
                objDataRow = null;
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (objDataTable != null)
                {
                    objDataTable.Dispose();
                }
            }
            return objDataRow;
        }

        /// <summary>
        /// Executes SQL stored procedure and fills DataRow with data.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Name of stored procedure to execute</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataRow">DataRow</param>
        /// <returns>DataRow</returns>
        public static DataRow GetDataRowStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, DataRow objDataRow)
        {
            objDataRow = GetDataRowStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objDataRow);

            return objDataRow;
        }

        /// <summary>
        /// Executes SQL stored procedure and fills DataRow with data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">Name of stored procedure to execute</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataRow">DataRow</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>DataRow</returns>
        public static DataRow GetDataRowStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, DataRow objDataRow, int commandTimeout)
        {
            DataTable objDataTable = new DataTable();
            try
            {
                objDataTable = GetDataTableStoredProc(connectionString, storedProcName, commandParameters, objDataTable, commandTimeout);
                if (/*(objDataTable != null) && */(objDataTable.Rows.Count > 0))
                {
                    objDataRow = objDataTable.Rows[0];
                }
            }
            catch (IndexOutOfRangeException)
            {
                objDataRow = null;
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (objDataTable != null)
                {
                    objDataTable.Dispose();
                }
            }
            return objDataRow;
        }

        /// <summary>
        /// Executes SQL stored procedure and fills DataRow with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">Name of stored procedure to execute</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objDataRow">DataRow</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>DataRow</returns>
        public static DataRow GetDataRowStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, DataRow objDataRow, int commandTimeout)
        {
            objDataRow = GetDataRowStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objDataRow, commandTimeout);
            return objDataRow;
        }

        #endregion

        #region ArrayList

        /// <summary>
        /// Executes dynamic SQL statement and fills ArrayList with data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objArrayList">ArrayList</param>
        /// <returns>ArrayList</returns>
        public static ArrayList GetArrayList(string connectionString, string sqlQuery, SqlParameter[] commandParameters, ArrayList objArrayList)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = CommandType.Text;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objArrayList.Add(objDataReader[0].ToString());
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                ////objDataReader = null;
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objArrayList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills ArrayList with data.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objArrayList">ArrayList</param>
        /// <returns>ArrayList</returns>
        public static ArrayList GetArrayList(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, ArrayList objArrayList)
        {
            objArrayList = GetArrayList(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objArrayList);

            return objArrayList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills ArrayList with data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objArrayList">ArrayList</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>ArrayList</returns>
        public static ArrayList GetArrayList(string connectionString, string sqlQuery, SqlParameter[] commandParameters, ArrayList objArrayList, int commandTimeout)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = CommandType.Text;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objArrayList.Add(objDataReader[0].ToString());
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                ////objDataReader = null;
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objArrayList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills ArrayList with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objArrayList">ArrayList</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>ArrayList</returns>
        public static ArrayList GetArrayList(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, ArrayList objArrayList, int commandTimeout)
        {
            objArrayList = GetArrayList(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objArrayList, commandTimeout);

            return objArrayList;
        }

        /// <summary>
        /// Executes stored procedure and fills ArrayList with data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objArrayList">ArrayList</param>
        /// <returns>ArrayList</returns>
        public static ArrayList GetArrayListStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, ArrayList objArrayList)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objArrayList.Add(objDataReader[0].ToString());
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                ////objDataReader = null;
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objArrayList;
        }

        /// <summary>
        /// Executes stored procedure and fills ArrayList with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objArrayList">ArrayList</param>
        /// <returns>ArrayList</returns>
        public static ArrayList GetArrayListStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, ArrayList objArrayList)
        {
            objArrayList = GetArrayListStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objArrayList);

            return objArrayList;
        }

        /// <summary>
        /// Executes stored procedure and fills ArrayList with data
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objArrayList">ArrayList</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>ArrayList</returns>
        public static ArrayList GetArrayListStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, ArrayList objArrayList, int commandTimeout)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objArrayList.Add(objDataReader[0].ToString());
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                ////objDataReader = null;
                CleanParameters(objCommand);
                objCommand.Dispose();
                KillConnection(objConnection);
            }
            return objArrayList;
        }

        /// <summary>
        /// Executes stored procedure and fills ArrayList with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objArrayList">ArrayList</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>ArrayList</returns>
        public static ArrayList GetArrayListStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, ArrayList objArrayList, int commandTimeout)
        {
            objArrayList = GetArrayListStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objArrayList, commandTimeout);

            return objArrayList;
        }

        #endregion

        #region List<byte>

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <returns>Generic List (byte)</returns>
        public static List<byte> GetGenericList(string connectionString, string sqlQuery, SqlParameter[] commandParameters, List<byte> objList)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = CommandType.Text;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objList.Add(Convert.ToByte(objDataReader[0]));
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                ////objDataReader = null;
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <returns>Generic List (byte)</returns>
        public static List<byte> GetGenericList(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, List<byte> objList)
        {
            objList = GetGenericList(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objList);

            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Generic List (byte)</returns>
        public static List<byte> GetGenericList(string connectionString, string sqlQuery, SqlParameter[] commandParameters, List<byte> objList, int commandTimeout)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = CommandType.Text;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objList.Add(Convert.ToByte(objDataReader[0]));
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                ////objDataReader = null;
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Generic List (byte)</returns>
        public static List<byte> GetGenericList(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, List<byte> objList, int commandTimeout)
        {
            objList = GetGenericList(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objList, commandTimeout);

            return objList;
        }

        /// <summary>
        /// Executes stored procedure and fills ArrayList with data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <returns>Generic List (byte)</returns>
        public static List<byte> GetGenericListStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, List<byte> objList)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objList.Add(Convert.ToByte(objDataReader[0]));
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                ////objDataReader = null;
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <returns>Generic List (byte)</returns>
        public static List<byte> GetGenericListStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, List<byte> objList)
        {
            objList = GetGenericListStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objList);

            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Generic List (byte)</returns>
        public static List<byte> GetGenericListStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, List<byte> objList, int commandTimeout)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objList.Add(Convert.ToByte(objDataReader[0]));
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                //objDataReader = null;
                CleanParameters(objCommand);
                objCommand.Dispose();
                KillConnection(objConnection);
            }
            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Generic List (byte)</returns>
        public static List<byte> GetGenericListStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, List<byte> objList, int commandTimeout)
        {
            objList = GetGenericListStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objList, commandTimeout);

            return objList;
        }

        #endregion

        #region List<short>

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <returns>Generic List (short)</returns>
        public static List<short> GetGenericList(string connectionString, string sqlQuery, SqlParameter[] commandParameters, List<short> objList)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = CommandType.Text;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objList.Add(Convert.ToInt16(objDataReader[0]));
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                //objDataReader = null;
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <returns>Generic List (short)</returns>
        public static List<short> GetGenericList(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, List<short> objList)
        {
            objList = GetGenericList(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objList);

            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Generic List (short)</returns>
        public static List<short> GetGenericList(string connectionString, string sqlQuery, SqlParameter[] commandParameters, List<short> objList, int commandTimeout)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = CommandType.Text;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objList.Add(Convert.ToInt16(objDataReader[0]));
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                //objDataReader = null;
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Generic List (short)</returns>
        public static List<short> GetGenericList(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, List<short> objList, int commandTimeout)
        {
            objList = GetGenericList(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objList, commandTimeout);

            return objList;
        }

        /// <summary>
        /// Executes stored procedure and fills ArrayList with data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <returns>Generic List (short)</returns>
        public static List<short> GetGenericListStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, List<short> objList)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objList.Add(Convert.ToInt16(objDataReader[0]));
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                //objDataReader = null;
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <returns>Generic List (short)</returns>
        public static List<short> GetGenericListStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, List<short> objList)
        {
            objList = GetGenericListStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objList);

            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Generic List (short)</returns>
        public static List<short> GetGenericListStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, List<short> objList, int commandTimeout)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objList.Add(Convert.ToInt16(objDataReader[0]));
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                //objDataReader = null;
                CleanParameters(objCommand);
                objCommand.Dispose();
                KillConnection(objConnection);
            }
            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Generic List (short)</returns>
        public static List<short> GetGenericListStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, List<short> objList, int commandTimeout)
        {
            objList = GetGenericListStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objList, commandTimeout);

            return objList;
        }

        #endregion

        #region List<int>

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <returns>Generic List (int32)</returns>
        public static List<int> GetGenericList(string connectionString, string sqlQuery, SqlParameter[] commandParameters, List<int> objList)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = CommandType.Text;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objList.Add(Convert.ToInt32(objDataReader[0]));
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                //objDataReader = null;
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <returns>Generic List (int32)</returns>
        public static List<int> GetGenericList(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, List<int> objList)
        {
            objList = GetGenericList(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objList);

            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Generic List (int32)</returns>
        public static List<int> GetGenericList(string connectionString, string sqlQuery, SqlParameter[] commandParameters, List<int> objList, int commandTimeout)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = CommandType.Text;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objList.Add(Convert.ToInt32(objDataReader[0]));
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                //objDataReader = null;
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Generic List (int32)</returns>
        public static List<int> GetGenericList(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, List<int> objList, int commandTimeout)
        {
            objList = GetGenericList(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objList, commandTimeout);

            return objList;
        }

        /// <summary>
        /// Executes stored procedure and fills ArrayList with data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <returns>Generic List (int32)</returns>
        public static List<int> GetGenericListStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, List<int> objList)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objList.Add(Convert.ToInt32(objDataReader[0]));
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                //objDataReader = null;
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <returns>Generic List (int32)</returns>
        public static List<int> GetGenericListStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, List<int> objList)
        {
            objList = GetGenericListStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objList);

            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Generic List (int32)</returns>
        public static List<int> GetGenericListStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, List<int> objList, int commandTimeout)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objList.Add(Convert.ToInt32(objDataReader[0]));
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                //objDataReader = null;
                CleanParameters(objCommand);
                objCommand.Dispose();
                KillConnection(objConnection);
            }
            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Generic List (int32)</returns>
        public static List<int> GetGenericListStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, List<int> objList, int commandTimeout)
        {
            objList = GetGenericListStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objList, commandTimeout);

            return objList;
        }

        #endregion

        #region List<byte>

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <returns>Generic List (string)</returns>
        public static List<string> GetGenericList(string connectionString, string sqlQuery, SqlParameter[] commandParameters, List<string> objList)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = CommandType.Text;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objList.Add(Convert.ToString(objDataReader[0]));
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                //objDataReader = null;
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <returns>Generic List (string)</returns>
        public static List<string> GetGenericList(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, List<string> objList)
        {
            objList = GetGenericList(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objList);

            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Generic List (string)</returns>
        public static List<string> GetGenericList(string connectionString, string sqlQuery, SqlParameter[] commandParameters, List<string> objList, int commandTimeout)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = CommandType.Text;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objList.Add(Convert.ToString(objDataReader[0]));
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                //objDataReader = null;
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Generic List (string)</returns>
        public static List<string> GetGenericList(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, List<string> objList, int commandTimeout)
        {
            objList = GetGenericList(GetConnectionString(connectionStructure), sqlQuery, commandParameters, objList, commandTimeout);

            return objList;
        }

        /// <summary>
        /// Executes stored procedure and fills ArrayList with data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <returns>Generic List (string)</returns>
        public static List<string> GetGenericListStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, List<string> objList)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objList.Add(Convert.ToString(objDataReader[0]));
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                //objDataReader = null;
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <returns>Generic List (string)</returns>
        public static List<string> GetGenericListStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, List<string> objList)
        {
            objList = GetGenericListStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objList);

            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Generic List (string)</returns>
        public static List<string> GetGenericListStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, List<string> objList, int commandTimeout)
        {
            SqlDataReader objDataReader;

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (objDataReader.Read())
                {
                    objList.Add(Convert.ToString(objDataReader[0]));
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                //objDataReader = null;
                CleanParameters(objCommand);
                objCommand.Dispose();
                KillConnection(objConnection);
            }
            return objList;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills Generic List with int data.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="objList">Generic list to fill with data</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>Generic List (string)</returns>
        public static List<string> GetGenericListStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, List<string> objList, int commandTimeout)
        {
            objList = GetGenericListStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, objList, commandTimeout);

            return objList;
        }

        #endregion

        #region String[] Array

        /// <summary>
        /// Executes dynamic SQL statement and fills string array with data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>string[]</returns>
        public static string[] GetStringValues(string connectionString, string sqlQuery, SqlParameter[] commandParameters)
        {
            SqlDataReader objDataReader;
            string[] ReturnResults = null;
            DataTable objDataTable = new DataTable();
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = CommandType.Text;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                //turn off the constrain and internal maintenance checking to boost performance
                objDataTable.BeginLoadData();
                objDataTable.Load(objDataReader, LoadOption.OverwriteChanges);
                //turn constrain and internal maintenance back on
                objDataTable.EndLoadData();
                if (/*(objDataTable != null) && */(objDataTable.Rows.Count > 0))
                {
                    ReturnResults = new string[objDataTable.Rows.Count];
                    for (int i = 0; i < objDataTable.Rows.Count; i++)
                    {
                        ReturnResults.SetValue(Convert.ToString(objDataTable.Rows[i][0]), i);
                    }
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                //objDataReader = null;
                //if (objDataTable != null)
                //{
                    objDataTable.Dispose();
                    //objDataTable = null;
                //}
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return ReturnResults;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills string array with data
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>string[]</returns>
        public static string[] GetStringValues(string connectionString, string sqlQuery, SqlParameter[] commandParameters, int commandTimeout)
        {
            SqlDataReader objDataReader;
            string[] ReturnResults = null;
            DataTable objDataTable = new DataTable();
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = CommandType.Text;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                //turn off the constrain and internal maintenance checking to boost performance
                objDataTable.BeginLoadData();
                objDataTable.Load(objDataReader, LoadOption.OverwriteChanges);
                //turn constrain and internal maintenance back on
                objDataTable.EndLoadData();
                if (/*(objDataTable != null) && */(objDataTable.Rows.Count > 0))
                {
                    ReturnResults = new string[objDataTable.Rows.Count];
                    for (int i = 0; i < objDataTable.Rows.Count; i++)
                    {
                        ReturnResults.SetValue(Convert.ToString(objDataTable.Rows[i][0]), i);
                    }
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                //objDataReader = null;
                //if (objDataTable != null)
                //{
                    objDataTable.Dispose();
                    //objDataTable = null;
                //}
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return ReturnResults;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills string array with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>string[]</returns>
        public static string[] GetStringValues(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, int commandTimeout)
        {
            string[] ReturnResults = GetStringValues(GetConnectionString(connectionStructure), sqlQuery, commandParameters, commandTimeout);

            return ReturnResults;
        }

        /// <summary>
        /// Executes stored procedure and fills string array with data
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>string[]</returns>
        public static string[] GetStringValuesStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters)
        {
            SqlDataReader objDataReader;
            string[] ReturnResults = null;
            DataTable objDataTable = new DataTable();
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                //turn off the constrain and internal maintenance checking to boost performance
                objDataTable.BeginLoadData();
                objDataTable.Load(objDataReader, LoadOption.OverwriteChanges);
                //turn constrain and internal maintenance back on
                objDataTable.EndLoadData();
                if (/*(objDataTable != null) && */(objDataTable.Rows.Count > 0))
                {
                    ReturnResults = new string[objDataTable.Rows.Count];
                    for (int i = 0; i < objDataTable.Rows.Count; i++)
                    {
                        ReturnResults.SetValue(Convert.ToString(objDataTable.Rows[i][0]), i);
                    }
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                //objDataReader = null;
                //if (objDataTable != null)
                //{
                    objDataTable.Dispose();
                    //objDataTable = null;
                //}
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return ReturnResults;
        }

        /// <summary>
        /// Executes stored procedure and fills string array with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>string[]</returns>
        public static string[] GetStringValuesStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters)
        {
            string[] ReturnResults = GetStringValuesStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters);

            return ReturnResults;
        }

        /// <summary>
        /// Executes stored procedure and fills string array with data
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>string[]</returns>
        public static string[] GetStringValuesStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, int commandTimeout)
        {
            SqlDataReader objDataReader;
            string[] ReturnResults = null;
            DataTable objDataTable = new DataTable();
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                //turn off the constrain and internal maintenance checking to boost performance
                objDataTable.BeginLoadData();
                objDataTable.Load(objDataReader, LoadOption.OverwriteChanges);
                //turn constrain and internal maintenance back on
                objDataTable.EndLoadData();
                if (/*(objDataTable != null) && */(objDataTable.Rows.Count > 0))
                {
                    ReturnResults = new string[objDataTable.Rows.Count];
                    for (int i = 0; i < objDataTable.Rows.Count; i++)
                    {
                        ReturnResults.SetValue(Convert.ToString(objDataTable.Rows[i][0]), i);
                    }
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                //objDataReader = null;
                //if (objDataTable != null)
                //{
                    objDataTable.Dispose();
                    //objDataTable = null;
                //}
                CleanParameters(objCommand);
                objCommand.Dispose();
                KillConnection(objConnection);
            }
            return ReturnResults;
        }

        /// <summary>
        /// Executes stored procedure and fills string array with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>string[]</returns>
        public static string[] GetStringValuesStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, int commandTimeout)
        {
            string[] ReturnResults = GetStringValuesStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, commandTimeout);

            return ReturnResults;
        }

        #endregion

        #region BulkCopy

        /// <summary>
        /// Performs bulk copy for multiple inserts
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="objDataTable">DataTable with values to insert</param>
        /// <param name="destinationTableName">Name of the destination table</param>
        /// <param name="batchSize">batch size of the bulk copy (number of rows)</param>
        /// <param name="timeOut">Bulk Copy timeout</param>
        public static void InsertBulkCopy(string connectionString, DataTable objDataTable, string destinationTableName, int batchSize, int timeOut)
        {
            SqlConnection objConnection = new SqlConnection();
            SqlBulkCopy objBulkCopy = null;
            try
            {
                objConnection = OpenConnection(objConnection, connectionString);
                objBulkCopy = new SqlBulkCopy(objConnection);
                objBulkCopy.DestinationTableName = destinationTableName;
                objBulkCopy.BatchSize = batchSize;
                objBulkCopy.BulkCopyTimeout = timeOut;
                objBulkCopy.WriteToServer(objDataTable);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (objBulkCopy != null)
                {
                    objBulkCopy.Close();
                    //objBulkCopy = null;
                }
                KillConnection(objConnection);
            }
        }

        /// <summary>
        /// Performs bulk copy for multiple inserts
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="objDataTable">DataTable with values to insert</param>
        /// <param name="destinationTableName">Name of the destination table</param>
        /// <param name="batchSize">batch size of the bulk copy (number of rows)</param>
        /// <param name="timeOut">Bulk Copy timeout</param>
        public static void InsertBulkCopy(ConnectionStructure connectionStructure, DataTable objDataTable, string destinationTableName, int batchSize, int timeOut)
        {
            InsertBulkCopy(GetConnectionString(connectionStructure), objDataTable, destinationTableName, batchSize, timeOut);
        }

        /// <summary>
        /// Performs bulk copy for multiple inserts
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="objDataTable">DataTable with values to insert</param>
        /// <param name="destinationTableName">Name of the destination table</param>
        /// <param name="batchSize">batch size of the bulk copy (number of rows)</param>
        public static void InsertBulkCopy(string connectionString, DataTable objDataTable, string destinationTableName, int batchSize)
        {
            SqlConnection objConnection = new SqlConnection();
            SqlBulkCopy objBulkCopy = null;
            try
            {
                objConnection = OpenConnection(objConnection, connectionString);
                objBulkCopy = new SqlBulkCopy(objConnection);
                objBulkCopy.DestinationTableName = destinationTableName;
                objBulkCopy.BatchSize = batchSize;
                objBulkCopy.WriteToServer(objDataTable);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (objBulkCopy != null)
                {
                    objBulkCopy.Close();
                    //objBulkCopy = null;
                }
                KillConnection(objConnection);
            }
        }

        /// <summary>
        /// Performs bulk copy for multiple inserts within transaction. Previous entries in the table are deleted
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="objDataTable">DataTable with results</param>
        /// <param name="deleteStoredProc"></param>
        /// <param name="commandParameters"></param>
        /// <param name="destinationTableName">Name of the destination table</param>
        /// <param name="batchSize">batch size of the bulk copy (number of rows)</param>
        /// <param name="timeOut">Bulk Copy timeout</param>
        public static void InsertBulkCopyWithDeleteTransaction(string connectionString, DataTable objDataTable, string deleteStoredProc, SqlParameter[] commandParameters, string destinationTableName, int batchSize, int timeOut)
        {
            if (deleteStoredProc == null) throw new ArgumentNullException("deleteStoredProc");
            SqlConnection objConnection = null;
            SqlBulkCopy objBulkCopy = null;
            SqlTransaction objTransaction = null;
            SqlCommand objCommand = new SqlCommand();
            try
            {
                //TATIANA TO DO: fix this one.
                objConnection = new SqlConnection();
                objConnection = OpenConnection(objConnection, connectionString);

                objCommand.Connection = objConnection;

                objTransaction = objConnection.BeginTransaction("BulkCopy");
                objCommand.Transaction = objTransaction;
                objCommand.CommandText = deleteStoredProc;
                objCommand.CommandType = CommandType.StoredProcedure;
                if (commandParameters != null)
                {
                    AttachParameters(objCommand, commandParameters);
                }
                objCommand.ExecuteNonQuery();

                objBulkCopy = new SqlBulkCopy(objConnection, SqlBulkCopyOptions.FireTriggers, objTransaction);
                objBulkCopy.DestinationTableName = destinationTableName;
                objBulkCopy.BatchSize = batchSize;
                objBulkCopy.BulkCopyTimeout = timeOut;
                objBulkCopy.WriteToServer(objDataTable);
                objTransaction.Commit();
            }
            catch (Exception ex)
            {
                //Attempt to roll back the transaction.
                if (objTransaction != null)
                {
                    objTransaction.Rollback();
                    objTransaction.Dispose();
                }
                throw new ApplicationException("InsertBulkCopyWithDeleteTransaction failed",ex);
            }
            finally
            {
                if (objBulkCopy != null)
                {
                    objBulkCopy.Close();
                    //objBulkCopy = null;
                }
                CleanParameters(objCommand);
                objCommand.Dispose();
                KillConnection(objConnection);
            }
        }

        /// <summary>
        /// Performs bulk copy for multiple inserts within transaction. Previous entries in the table are deleted
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="objDataTable">DataTable with results</param>
        /// <param name="deleteStoredProc"></param>
        /// <param name="commandParameters"></param>
        /// <param name="destinationTableName">Name of the destination table</param>
        /// <param name="batchSize">batch size of the bulk copy (number of rows)</param>
        /// <param name="timeOut">Bulk Copy timeout</param>
        public static void InsertBulkCopyWithDelete(string connectionString, DataTable objDataTable, string deleteStoredProc, SqlParameter[] commandParameters, string destinationTableName, int batchSize, int timeOut)
        {
            if (connectionString == null) throw new ArgumentNullException("connectionString");
            SqlConnection objConnection = null;
            SqlBulkCopy objBulkCopy = null;
            SqlTransaction objTransaction = null;
            SqlCommand objCommand = new SqlCommand();
            try
            {
                //TATIANA TO DO: fix this one.
                objConnection = new SqlConnection();
                objConnection = OpenConnection(objConnection, connectionString);

                objCommand.Connection = objConnection;
                objCommand.CommandText = deleteStoredProc;
                objCommand.CommandType = CommandType.StoredProcedure;
                if (commandParameters != null)
                {
                    AttachParameters(objCommand, commandParameters);
                    objCommand.Prepare();
                }
                objCommand.ExecuteNonQuery();

                objBulkCopy = new SqlBulkCopy(objConnection, SqlBulkCopyOptions.FireTriggers, objTransaction);
                objBulkCopy.DestinationTableName = destinationTableName;
                objBulkCopy.BatchSize = batchSize;
                objBulkCopy.BulkCopyTimeout = timeOut;
                objBulkCopy.WriteToServer(objDataTable);
            }
            finally
            {
                if (objBulkCopy != null)
                {
                    objBulkCopy.Close();
                    //objBulkCopy = null;
                }
                CleanParameters(objCommand);
                objCommand.Dispose();
                KillConnection(objConnection);
            }
        }

        /// <summary>
        /// Performs bulk copy for multiple inserts
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="objDataTable">DataTable with values to insert</param>
        /// <param name="destinationTableName">Name of the destination table</param>
        /// <param name="batchSize">batch size of the bulk copy (number of rows)</param>
        public static void InsertBulkCopy(ConnectionStructure connectionStructure, DataTable objDataTable, string destinationTableName, int batchSize)
        {
            InsertBulkCopy(GetConnectionString(connectionStructure), objDataTable, destinationTableName, batchSize);
        }

        /// <summary>
        /// Performs bulk copy for multiple inserts
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="objDataReader">DataReader with results</param>
        /// <param name="destinationTableName">Name of the destination table</param>
        /// <param name="batchSize">batch size of the bulk copy (number of rows)</param>
        /// <param name="timeOut">Bulk Copy timeout</param>
        public static void InsertBulkCopy(string connectionString, SqlDataReader objDataReader, string destinationTableName, int batchSize, int timeOut)
        {
            SqlConnection objConnection = new SqlConnection();
            SqlBulkCopy objBulkCopy = null;
            try
            {
                objConnection = OpenConnection(objConnection, connectionString);
                objBulkCopy = new SqlBulkCopy(objConnection);
                objBulkCopy.DestinationTableName = destinationTableName;
                objBulkCopy.BatchSize = batchSize;
                objBulkCopy.BulkCopyTimeout = timeOut;
                objBulkCopy.WriteToServer(objDataReader);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (objBulkCopy != null)
                {
                    objBulkCopy.Close();
                    //objBulkCopy = null;
                }
                KillConnection(objConnection);
            }
        }

        /// <summary>
        /// Performs bulk copy for multiple inserts
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="objDataReader">DataReader with results</param>
        /// <param name="destinationTableName">Name of the destination table</param>
        /// <param name="batchSize">batch size of the bulk copy (number of rows)</param>
        /// <param name="timeOut">Bulk Copy timeout</param>
        public static void InsertBulkCopy(ConnectionStructure connectionStructure, SqlDataReader objDataReader, string destinationTableName, int batchSize, int timeOut)
        {
            InsertBulkCopy(GetConnectionString(connectionStructure), objDataReader, destinationTableName, batchSize, timeOut);
        }

        /// <summary>
        /// Performs bulk copy for multiple inserts
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="objDataReader">DataReader with results</param>
        /// <param name="destinationTableName">Name of the destination table</param>
        /// <param name="batchSize">batch size of the bulk copy (number of rows)</param>
        public static void InsertBulkCopy(string connectionString, SqlDataReader objDataReader, string destinationTableName, int batchSize)
        {
            SqlConnection objConnection = new SqlConnection();
            SqlBulkCopy objBulkCopy = null;
            try
            {
                objConnection = OpenConnection(objConnection, connectionString);
                objBulkCopy = new SqlBulkCopy(objConnection);
                objBulkCopy.DestinationTableName = destinationTableName;
                objBulkCopy.BatchSize = batchSize;
                objBulkCopy.WriteToServer(objDataReader);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (objBulkCopy != null)
                {
                    objBulkCopy.Close();
                    //objBulkCopy = null;
                }
                KillConnection(objConnection);
            }
        }

        /// <summary>
        /// Performs bulk copy for multiple inserts
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="objDataReader">DataReader with results</param>
        /// <param name="destinationTableName">Name of the destination table</param>
        /// <param name="batchSize">batch size of the bulk copy (number of rows)</param>
        public static void InsertBulkCopy(ConnectionStructure connectionStructure, SqlDataReader objDataReader, string destinationTableName, int batchSize)
        {
            InsertBulkCopy(GetConnectionString(connectionStructure), objDataReader, destinationTableName, batchSize);
        }

        #endregion

        #region Int32[] Array

        /// <summary>
        /// Executes dynamic SQL statement and fills integer32 array with data.
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>int[]</returns>
        public static int[] GetIntegerValues(string connectionString, string sqlQuery, SqlParameter[] commandParameters)
        {
            SqlDataReader objDataReader;
            int[] ReturnResults = null;
            DataTable objDataTable = new DataTable();

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = CommandType.Text;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                //turn off the constrain and internal maintenance checking to boost performance
                objDataTable.BeginLoadData();
                objDataTable.Load(objDataReader, LoadOption.OverwriteChanges);
                //turn constrain and internal maintenance back on
                objDataTable.EndLoadData();
                if (/*(objDataTable != null) && */(objDataTable.Rows.Count > 0))
                {
                    ReturnResults = new int[objDataTable.Rows.Count];
                    for (int i = 0; i < objDataTable.Rows.Count; i++)
                    {
                        ReturnResults.SetValue(Convert.ToInt32(objDataTable.Rows[i][0]), i);
                    }
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                //objDataReader = null;
                //if (objDataTable != null)
                //{
                    objDataTable.Dispose();
                    //objDataTable = null;
                //}
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return ReturnResults;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills integer32 array with data.
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>int[]</returns>
        public static int[] GetIntegerValues(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters)
        {
            int[] ReturnResults = GetIntegerValues(GetConnectionString(connectionStructure), sqlQuery, commandParameters);

            return ReturnResults;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills integer32 array with data
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>int[]</returns>
        public static int[] GetIntegerValues(string connectionString, string sqlQuery, SqlParameter[] commandParameters, int commandTimeout)
        {
            SqlDataReader objDataReader;
            int[] ReturnResults = null;
            DataTable objDataTable = new DataTable();
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = CommandType.Text;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                //turn off the constrain and internal maintenance checking to boost performance
                objDataTable.BeginLoadData();
                objDataTable.Load(objDataReader, LoadOption.OverwriteChanges);
                //turn constrain and internal maintenance back on
                objDataTable.EndLoadData();
                if (/*(objDataTable != null) && */(objDataTable.Rows.Count > 0))
                {
                    ReturnResults = new int[objDataTable.Rows.Count];
                    for (int i = 0; i < objDataTable.Rows.Count; i++)
                    {
                        ReturnResults.SetValue(Convert.ToInt32(objDataTable.Rows[i][0]), i);
                    }
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                //objDataReader = null;
                //if (objDataTable != null)
                //{
                    objDataTable.Dispose();
                    //objDataTable = null;
                //}
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return ReturnResults;
        }

        /// <summary>
        /// Executes dynamic SQL statement and fills integer32 array with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="sqlQuery">Sql statement</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>int[]</returns>
        public static int[] GetIntegerValues(ConnectionStructure connectionStructure, string sqlQuery, SqlParameter[] commandParameters, int commandTimeout)
        {
            int[] ReturnResults = GetIntegerValues(GetConnectionString(connectionStructure), sqlQuery, commandParameters, commandTimeout);

            return ReturnResults;
        }

        /// <summary>
        /// Executes stored procedure and fills integer32 array with data
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <returns>int[]</returns>
        public static int[] GetIntegerValuesStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters)
        {
            SqlDataReader objDataReader;
            int[] ReturnResults = null;
            DataTable objDataTable = new DataTable();
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                //turn off the constrain and internal maintenance checking to boost performance
                objDataTable.BeginLoadData();
                objDataTable.Load(objDataReader, LoadOption.OverwriteChanges);
                //turn constrain and internal maintenance back on
                objDataTable.EndLoadData();
                if (/*(objDataTable != null) && */(objDataTable.Rows.Count > 0))
                {
                    ReturnResults = new int[objDataTable.Rows.Count];
                    for (int i = 0; i < objDataTable.Rows.Count; i++)
                    {
                        ReturnResults.SetValue(Convert.ToInt32(objDataTable.Rows[i][0]), i);
                    }
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                //objDataReader = null;
                //if (objDataTable != null)
                //{
                    objDataTable.Dispose();
                    //objDataTable = null;
                //}
                CleanParameters(objCommand);
                KillConnection(objConnection);
            }
            return ReturnResults;
        }

        /// <summary>
        /// Executes stored procedure and fills integer32 array with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName"></param>
        /// <param name="commandParameters"></param>
        /// <returns>int[]</returns>
        public static int[] GetIntegerValuesStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters)
        {
            int[] ReturnResults = GetIntegerValuesStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters);

            return ReturnResults;
        }

        /// <summary>
        /// Executes stored procedure and fills integer32 array with data
        /// </summary>
        /// <param name="connectionString">Connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>int[]</returns>
        public static int[] GetIntegerValuesStoredProc(string connectionString, string storedProcName, SqlParameter[] commandParameters, int commandTimeout)
        {
            SqlDataReader objDataReader;
            int[] ReturnResults = null;
            DataTable objDataTable = new DataTable();
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandText = storedProcName;
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandTimeout = commandTimeout;
            SqlConnection objConnection = new SqlConnection();
            objConnection = OpenConnection(objConnection, connectionString);

            objCommand.Connection = objConnection;
            if (commandParameters != null)
            {
                AttachParameters(objCommand, commandParameters);
                objCommand.Prepare();
            }
            try
            {
                objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                //turn off the constrain and internal maintenance checking to boost performance
                objDataTable.BeginLoadData();
                objDataTable.Load(objDataReader, LoadOption.OverwriteChanges);
                //turn constrain and internal maintenance back on
                objDataTable.EndLoadData();
                if (/*(objDataTable != null) && */(objDataTable.Rows.Count > 0))
                {
                    ReturnResults = new int[objDataTable.Rows.Count];
                    for (int i = 0; i < objDataTable.Rows.Count; i++)
                    {
                        ReturnResults.SetValue(Convert.ToInt32(objDataTable.Rows[i][0]), i);
                    }
                }
                if (!objDataReader.IsClosed)
                {
                    objDataReader.Close();
                }
            }
            catch (SqlException se)
            {
                //Generic SqlException
                throw new Exception(se.ToString());
            }
            catch (FormatException fe)
            {
                //wrong parameters Data Type
                throw new FormatException(fe.ToString());
            }
            catch (Exception ex)
            {
                //any other exception
                throw new Exception(ex.ToString());
            }
            finally
            {
                //objDataReader = null;
                //if (objDataTable != null)
                //{
                    objDataTable.Dispose();
                    //objDataTable = null;
                //}
                CleanParameters(objCommand);
                objCommand.Dispose();
                KillConnection(objConnection);
            }
            return ReturnResults;
        }

        /// <summary>
        /// Executes stored procedure and fills integer32 array with data
        /// </summary>
        /// <param name="connectionStructure">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <param name="storedProcName">storedProcName</param>
        /// <param name="commandParameters">SQL parameters array</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <returns>int[]</returns>
        public static int[] GetIntegerValuesStoredProc(ConnectionStructure connectionStructure, string storedProcName, SqlParameter[] commandParameters, int commandTimeout)
        {
            int[] ReturnResults = GetIntegerValuesStoredProc(GetConnectionString(connectionStructure), storedProcName, commandParameters, commandTimeout);

            return ReturnResults;
        }

        #endregion

        #endregion

        #region Private Static Methods
        /// <summary>
        /// Clears command parameters if there is no output parameters.
        /// Disposes command object
        /// </summary>
        /// <param name="objCommand">SqlCommand object</param>
        private static void CleanParameters(SqlCommand objCommand)
        {
            bool canClear = true;
            if (objCommand.Parameters.Count > 0)
            {
                foreach (SqlParameter commandParameter in objCommand.Parameters)
                {
                    if (commandParameter.Direction != ParameterDirection.Input)
                    {
                        canClear = false;
                    }
                }
                if (canClear)
                {
                    objCommand.Parameters.Clear();
                }
            }
            objCommand.Dispose();
        }

        /// <summary>
        /// Opens database connection
        /// </summary>
        /// <param name="objConnection">SqlConnection object</param>
        /// <param name="connectionString">Database connection string</param>
        /// <returns>SqlConnection</returns>
        private static SqlConnection OpenConnection(SqlConnection objConnection, string connectionString)
        {
            objConnection.InfoMessage += SqlInfoHandler;
            objConnection.ConnectionString = connectionString;
            if (objConnection.State == ConnectionState.Closed)
            {
                try
                {
                    objConnection.Open();
                }
                catch (InvalidOperationException ioex)
                {
                    //connection cannot be opened
                    throw new Exception("Unable to open database connection", ioex);
                }
                catch (SqlException sqlex)
                {
                    throw new Exception("Connection-level error on connection open", sqlex);
                }
            }
            else
            {
                throw new Exception("Connection already opened");
            }
            return objConnection;
        }

        private static void SqlInfoHandler(object sqlConnection, SqlInfoMessageEventArgs e)
        {
            StringBuilder objStringBuilder = new StringBuilder("Sql Error in Data Layer");
            for (int i = 0; i < e.Errors.Count; i++)
            {
                objStringBuilder.Append(Environment.NewLine);
                objStringBuilder.Append("ERROR #" + Convert.ToString(i + 1));
                objStringBuilder.Append(e.ToString());
            }
            //Steve comment out for now - it gets invoked on warnings and print statements
            //throw new Exception(objStringBuilder.ToString());
        }

        /// <summary>
        /// Closes and disposes SqlConnection object
        /// </summary>
        /// <param name="objConnection">SqlConnection object</param>
        /// <returns>void</returns>
        private static void KillConnection(IDbConnection objConnection)
        {
            try
            {
                if (objConnection != null)
                {
                    if (objConnection.State == ConnectionState.Open)
                    {
                        objConnection.Close();
                    }
                    objConnection.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Adds parameters to the command object
        /// </summary>
        /// <param name="objCommand"></param>
        /// <param name="commandParameters">Array of Sql Parameters</param>
        /// <returns>void</returns>
        private static void AttachParameters(SqlCommand objCommand, IEnumerable<SqlParameter> commandParameters)
        {
            foreach (SqlParameter objPar in commandParameters)
            {
                //check for derived output value with no value assigned
                if ((objPar.Direction == ParameterDirection.InputOutput) && (objPar.Value == null))
                {
                    objPar.Value = DBNull.Value;
                }
                objCommand.Parameters.Add(objPar);
            }
        }

        /// <summary>
        /// Adds parameters to the command object to be used for executing stored procedure
        /// </summary>
        /// <param name="objCommand"></param>
        /// <param name="commandParameters">Array of Sql Parameters</param>
        /// <returns>void</returns>
        private static void AttachParametersStoredProc(SqlCommand objCommand, IEnumerable<SqlParameter> commandParameters)
        {
            foreach (SqlParameter objPar in commandParameters)
            {
                //check for derived output value with no value assigned

                objCommand.Parameters.Add(objPar);
            }
        }

        /// <summary>
        /// Creates a connection string from the connection structure
        /// </summary>
        /// <param name="objConn">Instance of ConnectionStructure structure that holds a connection string to a database</param>
        /// <returns>Connection String</returns>
        private static string GetConnectionString(ConnectionStructure objConn)
        {
            DbConnectionStringBuilder objConnectionBuilder = new DbConnectionStringBuilder();
            string ConnectionString;
            try
            {
                objConnectionBuilder.Add("Data Source", objConn.DataSource);
                objConnectionBuilder.Add("Initial Catalog", objConn.InitialCatalog);
                objConnectionBuilder.Add("Trusted_Connection", objConn.TrustedConnection);
                objConnectionBuilder.Add("User ID", objConn.UserID);
                objConnectionBuilder.Add("Password", objConn.Password);
                objConnectionBuilder.Add("Persist Security Info", objConn.PersistSecurityInfo);
                objConnectionBuilder.Add("Min Pool Size", objConn.MinPoolSize);
                objConnectionBuilder.Add("Max Pool Size", objConn.MaxPoolSize);
                objConnectionBuilder.Add("Enlist", objConn.Enlist);
                objConnectionBuilder.Add("Async", objConn.Async);
                objConnectionBuilder.Add("MultipleActiveResultSets", objConn.MultipleActiveResultSets);
                ConnectionString = objConnectionBuilder.ConnectionString;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            //finally
            //{
            //    objConnectionBuilder = null;
            //}
            return ConnectionString;
        }


        #endregion

    }

    public class ParameterUtils
    {
        const string VALUE_NOT_AS_EXPECTED_2arg = "Value [{0}] is not of the required type [{1}].";

        public static object DbNullStringCheck(string s)
        {
            if (s == null || s.Length == 0)
                return DBNull.Value;
            else
                return s;
        }

        public static object SafeGetValueByName(string columnName, IDataReader source, Type requiredType, object defaultValue)
        {
            // determine the ordinal of the specific column
            try
            {
                int ordinal = source.GetOrdinal(columnName);
                object val = source.GetValue(ordinal);
                return SafeGetValue(val, requiredType, defaultValue);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("SafeGetValueByName could not find column named {0}", columnName), e);
            }
        }

        public static object SafeGetValue(object value, Type requiredType, object defaultValue)
        {
            //try
            //{	
            if (value == DBNull.Value)
            {
                return defaultValue;
            }
            else
            {
                // verify type
                if (value.GetType() != requiredType)
                {
                    throw new Exception(String.Format(VALUE_NOT_AS_EXPECTED_2arg, requiredType, defaultValue));
                }
                else
                {
                    return value;
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("SafeGetValue trapped execption", ex);
            //}
        }

        /// <summary>
        /// Converts deemed 'invalid' values into DBNull to facilitate database null-check rules
        /// </summary>
        /// <param name="value">Any value that will passed into a Parameter</param>
        /// <param name="validValue">A True/False expression defining the valid state that should NOT be converted to a DBNull (i.e. if this is true, leave the value alone)</param>
        /// <returns>The value 'untouched', or a DBNull - depending on whether or not it was valid</returns>
        public static object SafeSetValue(object value, bool validValue)
        {
            // check to see if the value passed in is null,
            // if so convert it to a DBNull
            if (!validValue)
            {
                return DBNull.Value;
            }
            else
            {
                return value;
            }
        }
    }

    public static class ParameterHelper
    {

        #region Input Parameters

        /// <summary>
        /// Creates XML Parameter
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>SqlParameter of XML type</returns>
        public static SqlParameter GetXmlPar(string parameterName, string parameterValue)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = ParameterDirection.Input;
            objPar.SqlDbType = SqlDbType.Xml;
            return objPar;
        }

        /// <summary>
        /// Creates the Integer parameter (Input)
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>SqlParameter of Int type</returns>
        public static SqlParameter GetIntegerPar(string parameterName, int? parameterValue)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = (int)parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = ParameterDirection.Input;
            objPar.SqlDbType = SqlDbType.Int;
            return objPar;
        }

        /// <summary>
        /// Creates Bit Parameter (Input)
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>SqlParameter of Bit type</returns>
        public static SqlParameter GetBitPar(string parameterName, bool parameterValue)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = ParameterDirection.Input;
            objPar.SqlDbType = SqlDbType.Bit;
            return objPar;
        }

        /// <summary>
        /// Creates Float Parameter (Input)
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>SqlParameter of Float type</returns>
        public static SqlParameter GetFloatPar(string parameterName, float parameterValue)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = ParameterDirection.Input;
            objPar.SqlDbType = SqlDbType.Float;
            return objPar;
        }

        /// <summary>
        /// Creates NVarChar Parameter (Input)
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterSize">Parameter size</param>
        /// <returns>SqlParameter of NVarChar type</returns>
        public static SqlParameter GetNVarCharPar(string parameterName, string parameterValue, int parameterSize)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = ParameterDirection.Input;
            objPar.SqlDbType = SqlDbType.NVarChar;
            objPar.Size = parameterSize;
            return objPar;
        }

        /// <summary>
        /// Creates VarChar parameter (Input)
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterSize">Parameter size</param>
        /// <returns>SqlParameter of VarChar</returns>
        public static SqlParameter GetVarCharPar(string parameterName, string parameterValue, int parameterSize)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = ParameterDirection.Input;
            objPar.SqlDbType = SqlDbType.VarChar;
            objPar.Size = parameterSize;
            return objPar;
        }

        /// <summary>
        /// Creates TinyInteger parameter (Input)
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>SqlParameter of TinyInt</returns>
        public static SqlParameter GetTinyIntPar(string parameterName, byte parameterValue)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = ParameterDirection.Input;
            objPar.SqlDbType = SqlDbType.TinyInt;
            return objPar;
        }

        /// <summary>
        /// Creates small integer parameter (Input)
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>SqlParameter of SmallInt type</returns>
        public static SqlParameter GetSmallIntPar(string parameterName, int parameterValue)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = ParameterDirection.Input;
            objPar.SqlDbType = SqlDbType.SmallInt;
            return objPar;
        }

        /// <summary>
        /// Creates Text parameter (Input)
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>SqlParameter of Text type</returns>
        public static SqlParameter GetTextPar(string parameterName, int parameterValue)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = ParameterDirection.Input;
            objPar.SqlDbType = SqlDbType.Text;
            return objPar;
        }

        /// <summary>
        /// Creates Small Money Parameter (Input)
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>SqlParameter of SmallMoney type</returns>
        public static SqlParameter GetSmallMoneyPar(string parameterName, int parameterValue)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = ParameterDirection.Input;
            objPar.SqlDbType = SqlDbType.SmallMoney;
            return objPar;
        }

        /// <summary>
        /// Creates Money Parameter (Input)
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>SqlParameter of Money type</returns>
        public static SqlParameter GetMoneyPar(string parameterName, int parameterValue)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = ParameterDirection.Input;
            objPar.SqlDbType = SqlDbType.Money;
            return objPar;
        }

        /// <summary>
        /// Creates NText Parameter (Input)
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>SqlParameter of NText type</returns>
        public static SqlParameter GetNTextPar(string parameterName, int parameterValue)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = ParameterDirection.Input;
            objPar.SqlDbType = SqlDbType.NText;
            return objPar;
        }

        /// <summary>
        /// Creates Decimal Parameter (Input)
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>SqlParameter of Decimal type</returns>
        public static SqlParameter GetDecimalPar(string parameterName, int parameterValue)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = ParameterDirection.Input;
            objPar.SqlDbType = SqlDbType.Decimal;
            return objPar;
        }

        /// <summary>
        /// Creates DateTime parameter (Input)
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>SqlParameter of DateTime type</returns>
        public static SqlParameter GetDateTimePar(string parameterName, string parameterValue)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = ParameterDirection.Input;
            objPar.SqlDbType = SqlDbType.DateTime;
            return objPar;
        }

        /// <summary>
        /// Creates DateTime parameter (Input)
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>SqlParameter of DateTime type</returns>
        public static SqlParameter GetDateTimePar(string parameterName, DateTime parameterValue)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = ParameterDirection.Input;
            objPar.SqlDbType = SqlDbType.DateTime;
            return objPar;
        }

        /// <summary>
        /// Creates Small DateTime parameter (Input)
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>SqlParameter of SmallDateTime type</returns>
        public static SqlParameter GetSmallDateTimePar(string parameterName, string parameterValue)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = ParameterDirection.Input;
            objPar.SqlDbType = SqlDbType.SmallDateTime;
            return objPar;
        }

        /// <summary>
        /// Creates SmallDateTime parameter (Input)
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>SqlParameter of SmallDateTime type</returns>
        public static SqlParameter GetSmallDateTimePar(string parameterName, DateTime parameterValue)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = ParameterDirection.Input;
            objPar.SqlDbType = SqlDbType.SmallDateTime;
            return objPar;
        }

        /// <summary>
        /// Creates BigInt Parameter (Input)
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>SqlParameter of BigInt type</returns>
        public static SqlParameter GetBigIntPar(string parameterName, Int64 parameterValue)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = ParameterDirection.Input;
            objPar.SqlDbType = SqlDbType.BigInt;
            return objPar;
        }


        /// <summary>
        /// Creates Char Parameter (Input)
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>SqlParameter of Char type</returns>
        /// <param name="parameterSize"></param>
        public static SqlParameter GetCharPar(string parameterName, string parameterValue, int parameterSize)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Size = parameterSize;
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = ParameterDirection.Input;
            objPar.SqlDbType = SqlDbType.Char;
            return objPar;
        }

        /// <summary>
        /// Creates NChar Parameter (Input)
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <returns>SqlParameter of VarChar type</returns>
        /// <param name="parameterSize"></param>
        public static SqlParameter GetNCharPar(string parameterName, string parameterValue, int parameterSize)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Size = parameterSize;
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = ParameterDirection.Input;
            objPar.SqlDbType = SqlDbType.NChar;
            return objPar;
        }

        #endregion

        #region With Parameter Directions

        /// <summary>
        /// Creates XML Parameter
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter Direction</param>
        /// <returns>SqlParameter of XML type</returns>
        public static SqlParameter GetXmlPar(string parameterName, string parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = parameterDirection;
            objPar.SqlDbType = SqlDbType.Xml;
            return objPar;
        }

        /// <summary>
        /// Creates the Integer parameter
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>        
        /// <param name="parameterDirection">Parameter Direction</param>
        /// <returns>SqlParameter of Int type</returns>
        public static SqlParameter GetIntegerPar(string parameterName, int? parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = (int)parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = parameterDirection;
            objPar.SqlDbType = SqlDbType.Int;
            return objPar;
        }

        /// <summary>
        /// Creates Bit Parameter
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter Direction</param>
        /// <returns>SqlParameter of Bit type</returns>
        public static SqlParameter GetBitPar(string parameterName, bool parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = parameterDirection;
            objPar.SqlDbType = SqlDbType.Bit;
            return objPar;
        }

        /// <summary>
        /// Creates Float Parameter
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter Direction</param>
        /// <returns>SqlParameter of Float type</returns>
        public static SqlParameter GetFloatPar(string parameterName, float parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = parameterDirection;
            objPar.SqlDbType = SqlDbType.Float;
            return objPar;
        }

        /// <summary>
        /// Creates NVarChar Parameter
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterSize">Parameter size</param>
        /// <param name="parameterDirection">Parameter Direction</param>
        /// <returns>SqlParameter of NVarChar type</returns>
        public static SqlParameter GetNVarCharPar(string parameterName, string parameterValue, int parameterSize, ParameterDirection parameterDirection)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = parameterDirection;
            objPar.SqlDbType = SqlDbType.NVarChar;
            objPar.Size = parameterSize;
            return objPar;
        }

        /// <summary>
        /// Creates VarChar parameter
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterSize">Parameter size</param>
        /// <param name="parameterDirection">Parameter Direction</param>
        /// <returns>SqlParameter of VarChar</returns>
        public static SqlParameter GetVarCharPar(string parameterName, string parameterValue, int parameterSize, ParameterDirection parameterDirection)
        {
            SqlParameter objPar = new SqlParameter
                                      {
                                          Value = parameterValue,
                                          ParameterName = parameterName,
                                          Direction = parameterDirection,
                                          SqlDbType = SqlDbType.VarChar,
                                          Size = parameterSize
                                      };
            return objPar;
        }

        /// <summary>
        /// Creates TinyInteger parameter
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter Direction</param>
        /// <returns>SqlParameter of TinyInt</returns>
        public static SqlParameter GetTinyIntPar(string parameterName, byte parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = parameterDirection;
            objPar.SqlDbType = SqlDbType.TinyInt;
            return objPar;
        }

        /// <summary>
        /// Creates small integer parameter
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter Direction</param>
        /// <returns>SqlParameter of SmallInt type</returns>
        public static SqlParameter GetSmallIntPar(string parameterName, int parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = parameterDirection;
            objPar.SqlDbType = SqlDbType.SmallInt;
            return objPar;
        }

        /// <summary>
        /// Creates Text parameter
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter Direction</param>
        /// <returns>SqlParameter of Text type</returns>
        public static SqlParameter GetTextPar(string parameterName, int parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = parameterDirection;
            objPar.SqlDbType = SqlDbType.Text;
            return objPar;
        }

        /// <summary>
        /// Creates Small Money Parameter
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter Direction</param>
        /// <returns>SqlParameter of SmallMoney type</returns>
        public static SqlParameter GetSmallMoneyPar(string parameterName, int parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = parameterDirection;
            objPar.SqlDbType = SqlDbType.SmallMoney;
            return objPar;
        }

        /// <summary>
        /// Creates Money Parameter
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter Direction</param>
        /// <returns>SqlParameter of Money type</returns>
        public static SqlParameter GetMoneyPar(string parameterName, int parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = parameterDirection;
            objPar.SqlDbType = SqlDbType.Money;
            return objPar;
        }

        /// <summary>
        /// Creates NText Parameter
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter Direction</param>
        /// <returns>SqlParameter of NText type</returns>
        public static SqlParameter GetNTextPar(string parameterName, int parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = parameterDirection;
            objPar.SqlDbType = SqlDbType.NText;
            return objPar;
        }

        /// <summary>
        /// Creates Decimal Parameter
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter Direction</param>
        /// <returns>SqlParameter of Decimal type</returns>
        public static SqlParameter GetDecimalPar(string parameterName, int parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = parameterDirection;
            objPar.SqlDbType = SqlDbType.Decimal;
            return objPar;
        }

        /// <summary>
        /// Creates Decimal Parameter
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter Direction</param>
        /// <returns>SqlParameter of Decimal type</returns>
        public static SqlParameter GetDecimalPar(string parameterName, decimal parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = parameterDirection;
            objPar.SqlDbType = SqlDbType.Decimal;
            return objPar;
        }

        /// <summary>
        /// Creates DateTime parameter
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter Direction</param>
        /// <returns>SqlParameter of DateTime type</returns>
        public static SqlParameter GetDateTimePar(string parameterName, string parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = parameterDirection;
            objPar.SqlDbType = SqlDbType.DateTime;
            return objPar;
        }

        /// <summary>
        /// Creates DateTime parameter
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter Direction</param>
        /// <returns>SqlParameter of DateTime type</returns>
        public static SqlParameter GetDateTimePar(string parameterName, DateTime parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = parameterDirection;
            objPar.SqlDbType = SqlDbType.DateTime;
            return objPar;
        }

        /// <summary>
        /// Creates Small DateTime parameter
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter Direction</param>
        /// <returns>SqlParameter of SmallDateTime type</returns>
        public static SqlParameter GetSmallDateTimePar(string parameterName, string parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = parameterDirection;
            objPar.SqlDbType = SqlDbType.SmallDateTime;
            return objPar;
        }

        /// <summary>
        /// Creates SmallDateTime parameter
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter Direction</param>
        /// <returns>SqlParameter of SmallDateTime type</returns>
        public static SqlParameter GetSmallDateTimePar(string parameterName, DateTime parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = parameterDirection;
            objPar.SqlDbType = SqlDbType.SmallDateTime;
            return objPar;
        }

        /// <summary>
        /// Creates BigInt Parameter
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterDirection">Parameter Direction</param>
        /// <returns>SqlParameter of BigInt type</returns>
        public static SqlParameter GetBigIntPar(string parameterName, Int64 parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = parameterDirection;
            objPar.SqlDbType = SqlDbType.BigInt;
            return objPar;
        }

        /// <summary>
        /// Creates Char Parameter
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterSize"></param>
        /// <param name="parameterDirection">Parameter Direction</param>
        /// <returns>SqlParameter of Char type</returns>
        public static SqlParameter GetCharPar(string parameterName, string parameterValue, int parameterSize, ParameterDirection parameterDirection)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Size = parameterSize;
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = parameterDirection;
            objPar.SqlDbType = SqlDbType.Char;
            return objPar;
        }

        /// <summary>
        /// Creates NChar Parameter 
        /// </summary>
        /// <param name="parameterName">The Name of the parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        /// <param name="parameterSize"></param>
        /// <param name="parameterDirection">Parameter Direction</param>
        /// <returns>SqlParameter of VarChar type</returns>
        public static SqlParameter GetNCharPar(string parameterName, string parameterValue, int parameterSize, ParameterDirection parameterDirection)
        {
            SqlParameter objPar = new SqlParameter();
            objPar.Size = parameterSize;
            objPar.Value = parameterValue;
            objPar.ParameterName = parameterName;
            objPar.Direction = parameterDirection;
            objPar.SqlDbType = SqlDbType.NChar;
            return objPar;
        }

        #endregion

    }
}