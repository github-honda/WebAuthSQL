/*
ZADB.cs

20190228, Honda, Copy from ZLib.
 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using System.Collections.Specialized;
using System.Data;

namespace ZLib.DAbstract
{
    /// <summary>
    /// 通用資料庫抽象物件
    /// </summary>
    public abstract class ZADB
    {
        public string msConnection { get; private set; }
        bool mbTransaction { get; }
        /// <summary>
        /// The time in seconds to wait for the command to execute. The default is 30 seconds.  這不是 ConnectionTimeout. ConnectionTimeout 是唯讀不可設定. 預設為15秒.
        /// </summary>
        public int miCommandTimeout { get; set; }
        public string msError { get; protected set; }
        public ZADB(string sConnectionString)
        {
            msConnection = sConnectionString;
        }


        abstract public void OpenConnection();
        abstract public void CloseConnection();
        abstract public Boolean IsClosed();

        /// <summary>
        /// 測試連接資料庫. 若連接成功, 則回傳空字串, 否則回傳錯誤訊息.
        /// </summary>
        /// <returns></returns>
        public string TryConnect()
        {
            msError = string.Empty;
            try
            {
                OpenConnection();
            }
            catch (Exception ex)
            {
                msError = ex.Message;
            }
            finally
            {
                CloseConnection();
            }
            return msError;
        }

        // Excute.
        abstract public int Execute(string sCmd, Dictionary<string, object> parameters);
        public int Execute(string sCmd) { return Execute(sCmd, null); }
        abstract public int ExecuteContinue(string sCmd, Dictionary<string, object> parameters);
        public int ExecuteContinue(string sCmd) { return Execute(sCmd, null); }

        // Query (string, value).
        abstract public object QueryValue(string sCmd, Dictionary<string, object> parameters);
        public object QueryValue(string sCmd) { return QueryValue(sCmd, null); }
        public string QueryString(string sCmd, Dictionary<string, object> parameters) { return ZData.FetchString(QueryValue(sCmd, parameters)); }
        public string QueryString(string sCmd) { return ZData.FetchString(QueryValue(sCmd, null)); }

        // Query (DataSet, DataTable, DataRow)
        abstract public DataSet QueryDataSet(string sCmd, Dictionary<string, object> parameters);
        public DataSet QueryDataSet(string sCmd)
        {
            return QueryDataSet(sCmd, null);
        }


        // Transaction.
        abstract public void BeginTransaction();
        abstract public void Commit();
        abstract public void Rollback();
        abstract public Boolean IsTransaction();

    }
}
