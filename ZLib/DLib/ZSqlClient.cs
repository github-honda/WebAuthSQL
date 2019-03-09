/*
ZSqlClient.cs

20190228, Honda, Copy from ZLib.
 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using System.Data;
using System.Data.SqlClient;
using ZLib.DAbstract;

namespace ZLib.DLib
{
    public class ZSqlClient : ZADB, IDisposable
    {
        protected SqlConnection mConnection;
        protected SqlTransaction mTransaction; // connection level.
        public ZSqlClient(string sConnectionString)
            : base(sConnectionString)
        {
            miCommandTimeout = 30;
        }

        /// <summary>
        /// Open connection. Use the same connection of the old one has opened.  
        /// </summary>
        public override void OpenConnection()
        {
            if (mConnection == null)
                mConnection = new SqlConnection(msConnection);

            if (string.IsNullOrEmpty(mConnection.ConnectionString))
                mConnection.ConnectionString = msConnection;

            if (mConnection.State == ConnectionState.Closed)
                mConnection.Open();
        }
        public override Boolean IsClosed()
        {
            if (mConnection == null)
                return true;

            if (mConnection.State == ConnectionState.Closed)
                return true;

            return false;
        }
        public override void CloseConnection()
        {
            if (mConnection == null)
                return;

            if (mConnection.State == ConnectionState.Open)
                mConnection.Close();
        }

        public string FetchString(SqlDataReader reader, int iField)
        {
            return FetchString(reader, iField, string.Empty);
        }
        public string FetchString(SqlDataReader reader, int iField, string sDefaultValue)
        {
            if (reader.IsDBNull(iField))
                return sDefaultValue;
            else if (reader.GetFieldType(iField) == typeof(System.Boolean))
                return ZData.FetchString(reader.GetBoolean(iField));
            else if (reader.GetFieldType(iField) == typeof(System.DateTime))
                return ZData.FetchString(reader.GetDateTime(iField));
            else
                return reader.GetValue(iField).ToString();
        }
        public string FetchString(SqlDataReader reader, string sColName)
        {
            return FetchString(reader, sColName, string.Empty);
        }
        public string FetchString(SqlDataReader reader, string sColName, string sDefaultValue)
        {
            int iCol = reader.GetOrdinal(sColName);
            if (iCol < 0)
                return sDefaultValue;
            return FetchString(reader, iCol, sDefaultValue);
        }

        public string[] FetchStrings(SqlDataReader reader)
        {
            string[] sa1 = new string[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; i++)
                sa1[i] = FetchString(reader, i);
            return sa1;
        }

        public SqlCommand CreateCommand(string commandText, Dictionary<string, object> parameters)
        {
            SqlCommand command = mConnection.CreateCommand();
            command.CommandText = commandText;
            command.CommandTimeout = miCommandTimeout;
            command.Transaction = mTransaction; // todo: Transaction control verify.
            AddParameters(command, parameters);

            return command;
        }
        public static void AddParameters(SqlCommand command, Dictionary<string, object> parameters)
        {
            if (parameters == null)
                return;

            foreach (var param in parameters)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = param.Key;
                parameter.Value = param.Value ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }
        }
        public override void BeginTransaction()
        {
            if (IsClosed())
                return;
            mTransaction = mConnection.BeginTransaction();
        }

        public override void Commit()
        {
            if (IsTransaction())
            {
                mTransaction.Commit();
                mTransaction = null;
            }
        }

        public override void Rollback()
        {
            if (IsTransaction())
            {
                mTransaction.Rollback();
                mTransaction = null;
            }
        }

        public override Boolean IsTransaction()
        {
            if (IsClosed())
                return false;

            if (mTransaction == null)
                return false;

            return true;
        }

        #region Dispose Part
        Boolean mbDisposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!mbDisposed)
            {
                if (disposing)
                {
                    if (mConnection != null)
                    {
                        CloseConnection();
                        mConnection.Dispose();
                    }
                }
                mConnection = null;
                mbDisposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }
        #endregion

        /// <summary>
        /// Execute SQL Command and close the connection.
        /// </summary>
        /// <param name="sCmd"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override int Execute(string sCmd, Dictionary<string, object> parameters)
        {
            int iResult = 0;
            try
            {
                OpenConnection();
                var command = CreateCommand(sCmd, parameters);
                iResult = command.ExecuteNonQuery();
            }
            catch (Exception ex1)
            {
                msError = ex1.Message;
            }
            finally
            {
                CloseConnection();
            }
            return iResult;
        }

        /// <summary>
        /// Execute SQL Command but NOT close connection for transaction control. 
        /// </summary>
        /// <param name="sCmd"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override int ExecuteContinue(string sCmd, Dictionary<string, object> parameters)
        {
            int iResult = 0;
            try
            {
                OpenConnection();
                var command = CreateCommand(sCmd, parameters);
                iResult = command.ExecuteNonQuery();
            }
            catch (Exception ex1)
            {
                msError = ex1.Message;
            }
            finally
            {
                // Keep open for transaction.
                //mConnection.Close();  
            }
            return iResult;
        }


        public override object QueryValue(string sCmd, Dictionary<string, object> parameters)
        {
            object oOutput = null;
            try
            {
                OpenConnection();
                var command = CreateCommand(sCmd, parameters);
                oOutput = command.ExecuteScalar();
                //return ZData.FieldToObject(oOutput);
            }
            catch (Exception ex1)
            {
                msError = ex1.Message;
                oOutput = null;
            }
            finally
            {
                CloseConnection();
            }
            return oOutput;
        }


        public override DataSet QueryDataSet(string sCmd, Dictionary<string, object> parameters)
        {
            DataSet ds1 = null;
            try
            {
                OpenConnection();
                var command = CreateCommand(sCmd, parameters);
                SqlDataAdapter adapter1 = new SqlDataAdapter(command);
                ds1 = new DataSet();
                //int iRows = adapter1.Fill(ds1);
                adapter1.Fill(ds1);
            }
            catch (Exception ex1)
            {
                msError = ex1.Message;
                ds1 = null;
            }
            finally
            {
                CloseConnection();
            }
            return ds1;
        }

    }
}
