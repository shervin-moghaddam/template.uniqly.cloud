using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using static template.Code.LogService;
using System.IO;
using System.Threading.Tasks;
using template.Code;
using template.Models;
using template.Services;
using static template.Code.Helpers.SQLHelperClass;
namespace template.SQL
{
    public class SQLService
    {
        public SQLService(
            ProjectSetupClass projectSetup,
            GlobalContainerService GCS)
        {
            GlobalContainer = GCS;
            ProjectSetup = projectSetup;
        }

        private readonly ProjectSetupClass ProjectSetup;
        private readonly GlobalContainerService GlobalContainer;
        public LogService Logger; // Injected the logservice from logservice itself

        private SqlConnection SQLConnect()
        {
            try
            {
                SqlConnection SQLCon = new SqlConnection(ProjectSetup.SQLConnection);
                SQLCon.Open();
                return SQLCon;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
            
        //GET SCALAR (no parameter)
        public object GetScalar(string queryString)
        {
            try
            {
                object returnObject;
                using (SqlConnection SQLCon = SQLConnect())
                {
                    SqlCommand command = new SqlCommand(queryString, SQLCon);
                    returnObject = command.ExecuteScalar();
                    return returnObject;
                }
            }
            catch (Exception ex)
            {
                Logger.GeneralLog("GetScalar", queryString, ex.Message, 2);
                throw new Exception(ex.Message);
            }
        }

        //GET SCALAR (With parameter)
        public object GetScalar(string queryString, params DbParameter[] ParamArray)
        {
            try
            {
                object returnObject;
                using (SqlConnection SQLCon = SQLConnect())
                {
                    SqlCommand command = new SqlCommand(queryString, SQLCon);
                    foreach (SqlParameter p in ParamArray)
                    {
                        command.Parameters.Add(p);
                    }

                    returnObject = command.ExecuteScalar();
                    return returnObject;
                }
            }
            catch (Exception ex)
            {
                Logger.GeneralLog("GetScalar2", queryString, ex.Message, 2);
                throw new Exception(ex.Message);
            }
        }

        // GET DATATABLE (No Parameter)
        public DataTable GetDataTable(string queryString)
        {
            try
            {
                DataTable dt = new DataTable();
                int returnvalue = 0;
                using (SqlConnection SQLCon = SQLConnect()) //Open SQL connection
                {
                    SqlCommand command = new SqlCommand(queryString, SQLCon);
                    dt.BeginLoadData();
                    dt.Clear();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        returnvalue = adapter.Fill(dt);
                    }
                }

                dt.EndLoadData();
                return dt;
            }
            catch (Exception ex)
            {
                Logger.GeneralLog("GetDataTable", queryString, ex.Message, 2);
                throw new Exception(ex.Message);
                return new DataTable();
            }
        }

        // GET DATATABLE (With Parameter)
        public DataTable GetDataTable(string queryString, params DbParameter[] ParamArray)
        {
            try
            {
                DataTable dt = new DataTable();
                int returnvalue = 0;

                //using (SqlConnection SQLCon = SQLConnect()) //Open SQL connection
                using (SqlConnection SQLCon = SQLConnect())
                {
                    SqlCommand command = new SqlCommand(queryString, SQLCon);
                    dt.BeginLoadData();
                    dt.Clear();
                    foreach (SqlParameter p in ParamArray)
                    {
                        command.Parameters.Add(p);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        returnvalue = adapter.Fill(dt);
                    }
                }

                dt.EndLoadData();
                return dt;
            }
            catch (Exception ex)
            {
                Logger.GeneralLog("GetDataTable2", queryString, ex.Message, 2);
                throw new Exception(ex.Message);
                return new DataTable();
            }
        }

        // Get datatable Async class
        public async Task<DataTable> GetDataTableAsync(string queryString, params DbParameter[] ParamArray)
        {
            try
            {
                DataTable dt = new DataTable();
                int returnvalue = 0;

                //using (SqlConnection SQLCon = SQLConnect()) //Open SQL connection
                await using (SqlConnection SQLCon = SQLConnect())
                {
                    SqlCommand command = new SqlCommand(queryString, SQLCon);
                    dt.BeginLoadData();
                    dt.Clear();
                    foreach (SqlParameter p in ParamArray)
                    {
                        command.Parameters.Add(p);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        returnvalue = adapter.Fill(dt);
                    }
                }

                dt.EndLoadData();
                return dt;
            }
            catch (Exception ex)
            {
                Logger.GeneralLog("GetDataTableAsync", queryString, ex.Message, 2);
                throw new Exception(ex.Message);
            }
        }

        //NONQUERY (no parameter)
        public void ExecQuery(string queryString)
        {
            try
            {
                using (SqlConnection SQLCon = SQLConnect())
                {
                    SqlCommand command = new SqlCommand(queryString, SQLCon);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.GeneralLog("ExecQuery", queryString, ex.Message, 2);
                throw new Exception(ex.Message);
            }
        }

        // NONQUERY (Parameter LIST)
        public void ExecQuery(string queryString, List<DbParameter> ParameterList)
        {
            try
            {
                using (SqlConnection SQLCon = SQLConnect())
                {
                    SqlCommand command = new SqlCommand(queryString, SQLCon);
                    foreach (SqlParameter p in ParameterList)
                    {
                        command.Parameters.Add(p);
                    }

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.GeneralLog("ExecQuery2", queryString, ex.Message, 2);
                throw new Exception(ex.Message);
            }
        }

        //NONQUERY (With parameter)
        public void ExecQuery(string queryString, params DbParameter[] ParamArray)
        {
            try
            {
                using (SqlConnection SQLCon = SQLConnect())
                {
                    SqlCommand command = new SqlCommand(queryString, SQLCon);
                    foreach (SqlParameter p in ParamArray)
                    {
                        command.Parameters.Add(p);
                    }

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.GeneralLog("ExecQuery2", queryString, ex.Message, 2);
                throw new Exception(ex.Message);
            }
        }

        public SqlCommand ExecStoredProcedure(string queryString)
        {
            try
            {
                using (SqlConnection SQLCon = SQLConnect())
                {
                    SqlCommand command = new SqlCommand(queryString, SQLCon);
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                    return command;
                }
            }
            catch (Exception ex)
            {
                Logger.GeneralLog("ExecStoredProcedure", queryString, ex.Message, 2);
                throw new Exception(ex.Message);
            }
        }

        //ExecStoredProcedure (With parameter)
        public SqlCommand ExecStoredProcedure(string queryString, params DbParameter[] ParamArray)
        {
            try
            {
                using (SqlConnection SQLCon = SQLConnect())
                {
                    SqlCommand command = new SqlCommand(queryString, SQLCon);
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter p in ParamArray)
                    {
                        command.Parameters.Add(p);
                    }

                    command.ExecuteNonQuery();
                    return command;
                }
            }
            catch (Exception ex)
            {
                Logger.GeneralLog("ExecStoredProcedure2", queryString, ex.Message, 2);
                throw new Exception(ex.Message);
            }
        }


        //ExecStoredProcedure (With datatable return)
        public DataTable ExecStoredProcedureReturnDT(string queryString, params DbParameter[] ParamArray)
        {
            try
            {
                DataTable dt = new DataTable();
                int returnvalue = 0;

                //using (SqlConnection SQLCon = SQLConnect()) //Open SQL connection
                using (SqlConnection SQLCon = SQLConnect())
                {
                    SqlCommand command = new SqlCommand(queryString, SQLCon);
                    command.CommandType = CommandType.StoredProcedure;
                    dt.BeginLoadData();
                    dt.Clear();
                    foreach (SqlParameter p in ParamArray)
                    {
                        command.Parameters.Add(p);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        returnvalue = adapter.Fill(dt);
                    }
                }

                dt.EndLoadData();
                return dt;
            }
            catch (Exception ex)
            {
                Logger.GeneralLog("ExecStoredProcedure2", queryString, ex.Message, 2);
                throw new Exception(ex.Message);
            }
        }

        public Tuple<SqlCommand, DataTable> ExecStoredProcedureReturnDTAndExec(string queryString,
            params DbParameter[] ParamArray)
        {
            try
            {
                SqlCommand ReturnSQLCommand;
                DataTable dt = new DataTable();
                int returnvalue = 0;

                //using (SqlConnection SQLCon = SQLConnect()) //Open SQL connection
                using (SqlConnection SQLCon = SQLConnect())
                {
                    SqlCommand command = new SqlCommand(queryString, SQLCon);
                    command.CommandType = CommandType.StoredProcedure;
                    dt.BeginLoadData();
                    dt.Clear();
                    foreach (SqlParameter p in ParamArray)
                    {
                        command.Parameters.Add(p);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        returnvalue = adapter.Fill(dt);
                    }

                    ReturnSQLCommand = command;
                }

                dt.EndLoadData();

                return new Tuple<SqlCommand, DataTable>(ReturnSQLCommand, dt);
            }
            catch (Exception ex)
            {
                Logger.GeneralLog("ExecStoredProcedure2", queryString, ex.Message, 2);
                throw new Exception(ex.Message);
            }
        }



        /// <summary>
        /// A class for execution of stored procedure, to give 100% customization of 
        /// parameters.
        /// usage: 
        /// 1: CreateSQLCommand(SP_Name)
        /// returns: SqlCommand
        /// 2: ExecuteCommand
        /// </summary>
        // public class ExecStoredProcedureClass
        // {
        //     string[] ConnVariables;
        //     SqlCommand command;
        //
        //     public SqlCommand CreateSQLCommand(string queryString, string[] _ConnVariables)
        //     {
        //         try
        //         {
        //             ConnVariables = _ConnVariables;
        //             command = new SqlCommand(queryString, SQLConnect());
        //             command.CommandType = CommandType.StoredProcedure;
        //             //foreach (SqlParameter p in ParamArray)
        //             //{
        //             //    command.Parameters.Add(p.SourceColumn, p.SqlDbType, p.Size);
        //             //}
        //             //command.Parameters.Add(p.SourceColumn, p.SqlDbType, p.Size);
        //             return command;
        //         }
        //         catch (Exception ex)
        //         {
        //             Logger.GeneralLog("CreateSQLCommand", queryString, ex.Message, 2);
        //             throw new Exception(ex.Message);
        //         }
        //     }
        //
        //     public void ExecuteCommand()
        //     {
        //         using (SqlConnection SQLCon = SQLConnect())
        //         {
        //             command.ExecuteNonQuery();
        //         }
        //     }
        // }

        /// <summary>
        /// Executes a store procedure and saves everything returned in a dataset.
        /// </summary>
        /// <param name="ProcedureName">Name of SP</param>
        /// <param name="ConnVariables">Connection Variables</param>
        /// <returns></returns>
        public DataSet GetDataSetFromStoredProcedure(string ProcedureName)
        {
            try
            {
                DataSet ds = new DataSet();

                using (SqlConnection SQLCon = SQLConnect())
                {
                    SqlCommand command = new SqlCommand(ProcedureName, SQLCon);
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        //   adapter.TableMappings.Add("Customers1", "Orders");
                        adapter.Fill(ds, ProcedureName);
                    }

                    return ds;
                }
            }
            catch (Exception ex)
            {
                Logger.GeneralLog("GetDataSetFromStoredProcedure", ProcedureName, ex.Message, 2);
                throw new Exception(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Executes a store procedure and saves everything returned in a dataset.
        /// </summary>
        /// <param name="ProcedureName">Name of SP</param>
        /// <param name="ConnVariables">Connection Variables</param>
        /// <param name="ParamArray">Parameters</param>
        /// <returns></returns>
        public DataSet GetDataSetFromStoredProcedure(string ProcedureName, params DbParameter[] ParamArray)
        {
            try
            {
                DataSet ds = new DataSet();

                using (SqlConnection SQLCon = SQLConnect())
                {
                    SqlCommand command = new SqlCommand(ProcedureName, SQLCon);
                    command.CommandType = CommandType.StoredProcedure;

                    foreach (SqlParameter p in ParamArray)
                    {
                        command.Parameters.Add(p);
                    }

                    command.ExecuteNonQuery();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(ds, ProcedureName);
                    }

                    return ds;
                }
            }
            catch (Exception ex)
            {
                Logger.GeneralLog("GetDataSetFromStoredProcedure", ProcedureName, ex.Message, 2);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Copies a datatable directly into the database by using bulkcopy.
        /// NOTE: DataTable columns names MUST be identical to destination column names
        /// </summary>
        /// <param name="ConnVariables">Connection string (e.g. LocalConn)</param>
        /// <param name="dtSource">Source Datatable</param>
        /// <param name="DestinationTableName">Destination tablename</param>
        /// <param name="Sync">If used for sync then TRUE. (Checks for SyncGUID)</param>
        public bool InsertDatatableToDB(string[] ConnVariables, DataTable dtSource,
            string DestinationTableName)
        {
            try
            {
                using (var bulkCopy = new SqlBulkCopy(GetSQLConnectionString(ConnVariables),
                           SqlBulkCopyOptions.KeepIdentity))
                {
                    foreach (DataColumn col in dtSource.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    }

                    bulkCopy.BulkCopyTimeout = 600;
                    bulkCopy.DestinationTableName = DestinationTableName;
                    bulkCopy.WriteToServer(dtSource);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.GeneralLog("BulkCopyDataTable", DestinationTableName, ex.Message, 2);
                throw new Exception(ex.Message);
            }
        }

        #region Bitmaps

        /// <summary>
        /// This function is made speicifically for getting a binary imagefile out of
        /// the database from the table: "BIN01_Images" and return it as memorystream.
        /// It can either be called with an ImageGUID or ImageName.
        /// </summary>
        /// <returns></returns>
        public MemoryStream GetImageFromDBToMemoryStream(Guid ImageGUID)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                // Either search by GUID or by Name
                string SQLStr = "";
                SQLStr = @"SELECT [Image] FROM BIN01_Images WHERE ImageGUID=@ImageGUID";

                using (var varConnection = SQLConnect())
                using (var sqlQuery = new SqlCommand(SQLStr, varConnection))
                {
                    // SQL Parameter
                    sqlQuery.Parameters.AddWithValue("@ImageGUID", ImageGUID);

                    using (var sqlQueryResult = sqlQuery.ExecuteReader())
                        if (sqlQueryResult != null)
                        {
                            sqlQueryResult.Read();
                            var blob = new Byte[(sqlQueryResult.GetBytes(0, 0, null, 0, int.MaxValue))];
                            sqlQueryResult.GetBytes(0, 0, blob, 0, blob.Length);
                            memoryStream.Write(blob, 0, blob.Length);
                        }
                }

                return memoryStream;
            }
            catch (Exception ex)
            {
                Logger.GeneralLog("GetImageFromDBToMemoryStream", "ImageGUID=" + ImageGUID, ex.Message, 2);
                throw new Exception(ex.Message);
            }

            return memoryStream;
        }

        public MemoryStream GetImageFromDBToMemoryStream(string ImageName)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                // Either search by GUID or by Name
                string SQLStr = "";
                SQLStr = @"SELECT [Image] from BIN01_Images WHERE [Name]=@Name";

                using (var varConnection = SQLConnect())
                using (var sqlQuery = new SqlCommand(SQLStr, varConnection))
                {
                    // SQL Parameter

                    sqlQuery.Parameters.AddWithValue("@Name", ImageName);

                    using (var sqlQueryResult = sqlQuery.ExecuteReader())
                        if (sqlQueryResult != null)
                        {
                            sqlQueryResult.Read();
                            var blob = new Byte[(sqlQueryResult.GetBytes(0, 0, null, 0, int.MaxValue))];
                            sqlQueryResult.GetBytes(0, 0, blob, 0, blob.Length);
                            memoryStream.Write(blob, 0, blob.Length);
                        }
                }

                return memoryStream;
            }
            catch (Exception ex)
            {
                Logger.GeneralLog("GetImageFromDBToMemoryStream", ImageName, ex.Message, 2);
                throw new Exception(ex.Message);
            }

            return memoryStream;
        }


        public MemoryStream GetImageFromDBToMemoryStream(Guid ImageGUID, params DbParameter[] ParamArray)
        {
            // Either search by GUID or by Name
            string SQLStr = "";
            SQLStr = @"SELECT [Image] FROM BIN01_Images WHERE ImageGUID=@ImageGUID";


            MemoryStream memoryStream = new MemoryStream();
            using (var varConnection = SQLConnect())
            using (var sqlQuery = new SqlCommand(SQLStr, varConnection))
            {
                // SQL Parameter
                sqlQuery.Parameters.AddWithValue("@ImageGUID", ImageGUID);
                foreach (SqlParameter p in ParamArray)
                {
                    sqlQuery.Parameters.Add(p);
                }

                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                    if (sqlQueryResult != null)
                    {
                        sqlQueryResult.Read();
                        var blob = new Byte[(sqlQueryResult.GetBytes(0, 0, null, 0, int.MaxValue))];
                        sqlQueryResult.GetBytes(0, 0, blob, 0, blob.Length);
                        memoryStream.Write(blob, 0, blob.Length);
                    }
            }

            return memoryStream;
        }

        public MemoryStream GetImageFromDBToMemoryStream(string queryString, params DbParameter[] ParamArray)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                using (var varConnection = SQLConnect())
                using (var sqlQuery = new SqlCommand(queryString, varConnection))
                {
                    // SQL Parameter
                    foreach (SqlParameter p in ParamArray)
                    {
                        sqlQuery.Parameters.Add(p);
                    }

                    using (var sqlQueryResult = sqlQuery.ExecuteReader())
                        if (sqlQueryResult != null)
                        {
                            sqlQueryResult.Read();
                            var blob = new Byte[(sqlQueryResult.GetBytes(0, 0, null, 0, int.MaxValue))];
                            sqlQueryResult.GetBytes(0, 0, blob, 0, blob.Length);
                            memoryStream.Write(blob, 0, blob.Length);
                        }
                }
            }
            catch (Exception ex)
            {
                Logger.GeneralLog("GetImageFromDBToMemoryStream", queryString, ex.Message, 2);
                throw new Exception(ex.Message);
            }

            return memoryStream;
        }


        /// <summary>
        /// Returns binary data from the DataBase as byte array.
        /// </summary>
        public byte[] GetBinary(string queryString, params DbParameter[] ParamArray)
        {
            using (SqlConnection SQLCon = SQLConnect()) //Open SQL connection
            {
                SqlCommand command = new SqlCommand(queryString, SQLCon);
                foreach (SqlParameter p in ParamArray)
                {
                    command.Parameters.Add(p);
                }

                using (var sqlQueryResult = command.ExecuteReader())
                    if (sqlQueryResult != null)
                    {
                        sqlQueryResult.Read();
                        var blob = new Byte[(sqlQueryResult.GetBytes(0, 0, null, 0, int.MaxValue))];
                        return blob;
                    }
            }

            return null;
        }

        #endregion

        public string GetSQLConnectionString(string[] ConnVariables)
        {
            string SQLIP = ConnVariables[0];
            string SQLDB = ConnVariables[1];
            string UserName = ConnVariables[2];
            string Password = ConnVariables[3];

            //Build connection string
            string SQLConnectionString = "";
            SQLConnectionString = "Data Source =" + SQLIP + ";Initial Catalog=" + SQLDB + "; User Id=" + UserName +
                                  "; Password=" + Password;
            return SQLConnectionString;
        }

     
        public class SQLReturnModel
        {
            public int StatusCode { get; set; } = 0;
            public string Identifier { get; set; }
            public string ErrorMessage { get; set; }
        }

        public DbParameter[] ReuseParameters(DbParameter[] Parameters)
        {
            SqlParameter[] ReturnParameters = new SqlParameter[Parameters.Length];
            SqlParameterCollection bab = null;
            List<DbParameter> sqlParameters = new List<DbParameter>();
            foreach (DbParameter p in Parameters)
            {
                sqlParameters.Add(CreateParam(p.Value, p.ParameterName));
                //ReturnParameters.AddInParameter(result, p.ParameterName, p.DbType, p.Value);
            }

            return sqlParameters.ToArray();
        }
    }
}