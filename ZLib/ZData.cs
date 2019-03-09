/*
ZData.cs

20190228, Honda, Copy from ZLib.
 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using System.IO;
using System.Data;
using System.Globalization;
using System.Collections.Specialized;

namespace ZLib
{
    /// <summary>
    /// Common functions for DataSet/DataTable/DataRow/Field.
    /// </summary>
    public static class ZData
    {
        static ZData()
        {
        }

        /// <summary>
        /// 排序欄位. 依照排序指令, 例如: "State ASC, ZipCode ASC"
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="sSortExpress"></param>
        /// <returns></returns>
        public static DataTable ZSortTable(this DataTable dt1, string sSortExpress)
        {
            // example: view.Sort = "State ASC, ZipCode ASC";
            DataView dv1 = dt1.DefaultView;
            dv1.Sort = sSortExpress;
            return dv1.ToTable();
        }

        /// <summary>
        /// 排序指定欄位.
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="iColumn"></param>
        /// <param name="bAscending"></param>
        /// <returns></returns>
        public static DataTable ZSortTable(this DataTable dt1, int iColumn, Boolean bAscending)
        {
            string sSort = string.Empty;
            // example: view.Sort = "State ASC, ZipCode ASC";
            if (bAscending)
                sSort = dt1.Columns[iColumn].ColumnName + " ASC";
            else
                sSort = dt1.Columns[iColumn].ColumnName + " DESC";

            return ZSortTable(dt1, sSort);
        }
        public static DataRow[] ZSelect(this DataTable dt1, string sSelect)
        {
            //myDataTable.Select("columnName1 like '%" + value + "%'");
            // Select: 查詢非PK, 若要查PK, 則使用Find()
            return dt1.Select("columnName1 like '%KeyWord%' and columnName2 like '%KeyWord%'");
        }
        public static DataTable ZSelectToTable(this DataTable dt1, string sSelect)
        {
            DataTable dt2 = dt1.ZClone();
            DataRow[] adr1 = dt1.ZSelect(sSelect);
            foreach (DataRow dr1 in adr1)
            {
                DataRow dr2 = dt2.NewRow();
                dr1.ItemArray.CopyTo(dr1.ItemArray, 0);
                dt2.Rows.Add(dr2);
            }
            return dt2;
        }


        /// <summary>
        /// Copy table structure only.
        /// </summary>
        /// <param name="dt1"></param>
        /// <returns></returns>
        public static DataTable ZClone(this DataTable dt1)
        {
            return dt1.Clone();
        }


        /// <summary>
        /// Copy table structure and data.
        /// </summary>
        /// <param name="dt1"></param>
        /// <returns></returns>
        public static DataTable ZCopy(this DataTable dt1)
        {
            return dt1.Copy();
        }

        public static void ZDeleteAllRows(this DataTable dt1)
        {
            dt1.Rows.Clear();
        }
        public static void ZDeleteRow(this DataTable dt1, int iRow)
        {
            dt1.Rows[iRow].Delete();
        }

        /// <summary>
        /// 依據DataColumn[]建立DataTable. Usage: CreateDataTable(new DataColumn[3] { new DataColumn("Id", typeof(int)), new DataColumn("Name", typeof(string)), new DataColumn("Country",typeof(string)) });
        /// </summary>
        /// <param name="aDataColumn"></param>
        /// <returns></returns>
        public static DataTable NewDataTable(DataColumn[] aDataColumn)
        {
            DataTable dt1 = new DataTable();
            dt1.Columns.AddRange(aDataColumn);
            return dt1;
        }

        /// <summary>
        /// 建立欄位為 string type 的 DataTable, 欄位名稱為F1, F2,...FN.
        /// </summary>
        /// <param name="iColumns"></param>
        /// <returns></returns>
        public static DataTable NewDataTableByColumnCount(int iColumns)
        {
            DataTable dt1 = new DataTable();
            for (int i = 0; i < iColumns; i++)
                dt1.Columns.Add("F" + i.ToString(), typeof(string));
            return dt1;
        }

        /// <summary>
        /// 建立欄位為 string type 的 DataTable, 欄位名稱輸入的字串陣列.
        /// </summary>
        /// <param name="saColumnName"></param>
        /// <returns></returns>
        public static DataTable NewDataTableByColumnNames(string[] saColumnName)
        {
            DataTable dt1 = new DataTable();
            for (int i = 0; i < saColumnName.Length; i++)
                dt1.Columns.Add(saColumnName[i], typeof(string));
            return dt1;
        }
        public static string[] ZGetColumnNames(this DataTable dt)
        {
            string[] sa1 = new string[dt.Columns.Count];
            for (int iCol = 0; iCol < dt.Columns.Count; iCol++)
                sa1[iCol] = dt.Columns[iCol].ColumnName;
            return sa1;
        }
        public static int ZGetColumnIndex(this DataTable dt, string sColName)
        {
            return dt.Columns.IndexOf(sColName);
        }
        public static int ZGetColumnIndex(this DataRow dr, string sColName)
        {
            return dr.Table.Columns.IndexOf(sColName);
        }
        public static int ZGetRowsCount(this DataSet ds, int iTable)
        {
            if (ds == null)
                return -1;
            if (ds.Tables.Count < 1)
                return -1;
            if (iTable < 0 || iTable > ds.Tables.Count - 1)
                return -1;
            return ds.Tables[iTable].Rows.Count;
        }
        public static int ZGetRowsCount(this DataTable dt)
        {
            if (dt == null)
                return -1;
            return dt.Rows.Count;
        }
        public static int ZGetColumnsCount(this DataTable dt)
        {
            if (dt == null)
                return -1;
            return dt.Columns.Count;
        }
        public static DataTable ZGetTable(this DataSet ds, int iTable)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count < 1)
                return null;
            if (iTable < 0 || iTable > ds.Tables.Count - 1)
                return null;
            return ds.Tables[iTable];
        }
        public static DataRow ZGetRow(this DataSet ds, int iTable, int iRow)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count < 1)
                return null;
            if (iTable < 0 || iTable > ds.Tables.Count - 1)
                return null;
            if (iRow < 0 || iRow > ds.Tables[iTable].Rows.Count - 1)
                return null;
            return ds.Tables[iTable].Rows[iRow];
        }
        public static DataRow ZGetRow(this DataTable dt, int iRow)
        {
            if (dt == null)
                return null;
            if (iRow < 0 || iRow > dt.Rows.Count - 1)
                return null;
            return dt.Rows[iRow];
        }

        #region Convert DataTable to array, collection, List...
        public static string ZGetString(this DataTable dt)
        {
            return dt.ZGetString(Environment.NewLine, "\t", -1);
        }
        public static string ZGetString(this DataTable dt, int iRows)
        {
            return dt.ZGetString(Environment.NewLine, "\t", iRows);
        }
        public static string ZGetString(this DataTable dt, string sDelimeterRow, string sDelimeterCol, int iRows)
        {
            //if (dt == null)
            //    return string.Empty;

            StringBuilder sb1 = new StringBuilder(string.Empty);
            // header

            if (iRows < 0)
                iRows = dt.Rows.Count;
            else
                if (iRows > dt.Rows.Count)
                iRows = dt.Rows.Count;

            if (iRows < 0)
                return null;

            sb1.Append(string.Join(sDelimeterCol, dt.ZGetColumnNames()));
            sb1.Append(sDelimeterRow);
            for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
            {
                sb1.Append(string.Join(sDelimeterCol, dt.Rows[iRow].ZFetchRowString()));
                sb1.Append(sDelimeterRow);
            }
            return sb1.ToString();

        }

        /// <summary>
        /// 指定1個欄位建立 List&lt;string&gt; 
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="iColumnKey"></param>
        /// <returns></returns>
        public static List<string> ZGetListString(this DataTable dt1, int iCol)
        {
            List<string> list1 = new List<string>();
            if (dt1 == null)
                return null;
            if (iCol < 0 || iCol > dt1.Columns.Count - 1)
                return null;
            foreach (DataRow dr1 in dt1.Rows)
                list1.Add(dr1.ZFetchString(iCol));
            return list1;
        }


        /// <summary>
        /// 指定2欄位建立StringDictionary. 若Key值重複或空白, 則略過. 
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="iColumnKey"></param>
        /// <param name="iColumnValue"></param>
        /// <returns></returns>
        public static StringDictionary ZGetStringDictionary(this DataTable dt1, int iColumnKey, int iColumnValue)
        {
            StringDictionary dic1 = new StringDictionary();

            string sKey;
            string sValue;
            foreach (DataRow dr1 in dt1.Rows)
            {
                sKey = dr1.ZFetchString(iColumnKey).Trim();
                if (string.IsNullOrWhiteSpace(sKey))
                    continue;
                if (dic1.ContainsKey(sKey))
                    continue;
                sValue = dr1.ZFetchString(iColumnValue).Trim();
                dic1.Add(sKey, sValue);
            }
            return dic1;
        }

        /// <summary>
        /// 指定1個欄位建立HashSet&lt;string&gt; 若Key值重複或空白, 則略過. 可做為listbox,combobox等資料來源.
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="iColumnKey"></param>
        /// <returns></returns>
        public static HashSet<string> ZGetHashSetString(this DataTable dt1, int iColumnKey)
        {
            HashSet<string> list1 = new HashSet<string>();
            string sKey;
            foreach (DataRow dr1 in dt1.Rows)
            {
                sKey = dr1.ZFetchString(iColumnKey).Trim();
                if (string.IsNullOrWhiteSpace(sKey))
                    continue;
                if (list1.Contains(sKey))
                    continue;
                list1.Add(sKey);
            }
            return list1;
        }

        /// <summary>
        /// 指定1個欄位建立Index = Dictionary&lt;string, int&gt; 若Key值重複或空白, 則略過. Value欄位=Row Index.
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="iColumnKey"></param>
        /// <returns></returns>
        public static Dictionary<string, int> ZGetDictionaryIndex(this DataTable dt1, int iColumnKey)
        {
            Dictionary<string, int> list1 = new Dictionary<string, int>();
            string sKey;
            for (int iRow = 0; iRow < dt1.Rows.Count; iRow++)
            {
                DataRow dr1 = dt1.Rows[iRow];
                sKey = dr1.ZFetchString(iColumnKey).Trim();
                if (string.IsNullOrWhiteSpace(sKey))
                    continue;
                if (list1.ContainsKey(sKey))
                    continue;
                list1.Add(sKey, iRow);
            }
            return list1;
        }
        public static object[,] ZGetArray2D(this DataTable dt1)
        {
            int iRows = dt1.Rows.Count;
            int iCols = dt1.Columns.Count;

            iRows++;  // add header.
            object[,] oa1 = new object[iRows, iCols];

            // header
            int iRow = 0;
            for (int iCol = 0; iCol < iCols; iCol++)
                oa1[iRow, iCol] = dt1.Columns[iCol].ColumnName;

            // body
            iRow++;
            for (int i = iRow; i < iRows; i++)
                for (int iCol = 0; iCol < iCols; iCol++)
                    oa1[i, iCol] = dt1.Rows[iRow][iCol];

            return oa1;
        }
        public static string[,] ZGetArray2DString(this DataTable dt1)
        {
            int iRows = dt1.Rows.Count;
            int iCols = dt1.Columns.Count;

            iRows++;  // add header.
            string[,] sa1 = new string[iRows, iCols];

            // header
            int iRow = 0;
            for (int iCol = 0; iCol < iCols; iCol++)
                sa1[iRow, iCol] = dt1.Columns[iCol].ColumnName;

            // body
            iRow++;
            for (int i = iRow; i < iRows; i++)
                for (int iCol = 0; iCol < iCols; iCol++)
                    sa1[i, iCol] = dt1.Rows[i].ZFetchString(iCol);

            return sa1;
        }


        #endregion

        #region ZFetch, ZFlush extensions

        public static Boolean ZFetchBoolean(this DataRow dr1, int iCol, Boolean bDefaultValue)
        {
            if (dr1.IsNull(iCol))
                return bDefaultValue;
            return dr1.Field<Boolean>(iCol);
        }

        public static Boolean ZFetchBoolean(this DataRow dr1, string sColName, Boolean bDefaultValue)
        {
            if (dr1.IsNull(sColName))
                return bDefaultValue;
            return dr1.Field<Boolean>(sColName);
        }
        public static DateTime ZFetchDateTime(this DataRow dr1, int iCol, DateTime dDefaultValue)
        {
            if (dr1.IsNull(iCol))
                return dDefaultValue;
            return dr1.Field<DateTime>(iCol);
        }

        public static DateTime ZFetchDateTime(this DataRow dr1, string sColName, DateTime dDefaultValue)
        {
            if (dr1.IsNull(sColName))
                return dDefaultValue;
            return dr1.Field<DateTime>(sColName);
        }
        public static int ZFetchInt(this DataRow dr1, int iCol, int iDefaultValue)
        {
            if (dr1.IsNull(iCol))
                return iDefaultValue;
            return dr1.Field<int>(iCol);
        }

        public static int ZFetchInt(this DataRow dr1, string sColName, int iDefaultValue)
        {
            if (dr1.IsNull(sColName))
                return iDefaultValue;
            return dr1.Field<int>(sColName);
        }
        public static long ZFetchLong(this DataRow dr1, int iCol, long lDefaultValue)
        {
            if (dr1.IsNull(iCol))
                return lDefaultValue;
            return dr1.Field<long>(iCol);
        }

        public static long ZFetchLong(this DataRow dr1, string sColName, long lDefaultValue)
        {
            if (dr1.IsNull(sColName))
                return lDefaultValue;
            return dr1.Field<long>(sColName);
        }
        public static decimal ZFetchDecimal(this DataRow dr1, int iCol, decimal dDefaultValue)
        {
            if (dr1.IsNull(iCol))
                return dDefaultValue;
            return dr1.Field<decimal>(iCol);
        }

        public static decimal ZFetchDecimal(this DataRow dr1, string sColName, decimal dDefaultValue)
        {
            if (dr1.IsNull(sColName))
                return dDefaultValue;
            return dr1.Field<decimal>(sColName);
        }

        public static string ZFetchString(this DataRow dr1, int iField)
        {
            return ZFetchString(dr1, iField, null);
        }
        public static string ZFetchString(this DataRow dr1, int iField, string sDefaulValue)
        {
            if (dr1.IsNull(iField))
                return sDefaulValue;
            else if (dr1.Table.Columns[iField].DataType == typeof(System.Boolean))
                return ZData.FetchString(dr1.Field<Boolean>(iField));
            else if (dr1.Table.Columns[iField].DataType == typeof(System.DateTime))
                return ZData.FetchString(dr1.Field<DateTime>(iField));
            else
                return dr1[iField].ToString();
        }
        public static string ZFetchString(this DataRow dr1, string sColName)
        {
            return ZFetchString(dr1, sColName, null);
        }
        public static string ZFetchString(this DataRow dr1, string sColName, string sDefaultValue)
        {
            int iCol = dr1.ZGetColumnIndex(sColName);
            if (iCol < 0)
                return sDefaultValue;
            return ZFetchString(dr1, iCol, sDefaultValue);
        }
        public static object[] ZFetchRow(this DataRow dr1)
        {
            return dr1.ItemArray; // is DBNull ?
        }
        public static string[] ZFetchRowString(this DataRow dr1)
        {
            //object[] oa1 = dr1.ItemArray;
            int iColumns = dr1.Table.ZGetColumnsCount();
            string[] saCol = new string[iColumns];
            for (int iCol = 0; iCol < iColumns; iCol++)
                saCol[iCol] = dr1.ZFetchString(iCol);
            return saCol;
        }

        #endregion

        #region ObjectTo, GetField, FieldTo
        // Field = Database field value from DataRow, DataReader...
        //       = (Excel or Matrix) cell value. 
        // refer to implements of reader:
        //   reader.GetBoolean(iField);
        //   reader.GetByte(iField);
        //   xx reader.GetSqlBytes(iField);
        //   xx reader.GetSqlChars(iField);
        //   reader.GetDateTime(iField);
        //   reader.GetDecimal(iField);
        //   reader.GetDouble(iField);
        //   reader.GetFloat(iField);
        //   reader.GetInt16(iField);
        //   reader.GetInt32(iField);
        //   reader.GetInt64(iField);

        public static Boolean IsNullOrDBNull(object oField)
        {
            if (oField == null)
                return true;
            if (oField.Equals(DBNull.Value))
                return true;
            return false;
        }
        public static Boolean? GetBoolean(object oField)
        {
            return IsNullOrDBNull(oField) ? null : (Boolean?)oField;
        }
        public static Byte? GetByte(object oField)
        {
            return IsNullOrDBNull(oField) ? null : (Byte?)oField;
        }
        public static DateTime? GetDateTime(object oField)
        {
            return IsNullOrDBNull(oField) ? null : (DateTime?)oField;
        }
        public static Decimal? GetDecimal(object oField)
        {
            return IsNullOrDBNull(oField) ? null : (Decimal?)oField;
        }
        public static Double? GetDouble(object oField)
        {
            return IsNullOrDBNull(oField) ? null : (Double?)oField;
        }
        public static float? GetFloat(object oField)
        {
            return IsNullOrDBNull(oField) ? null : (float?)oField;
        }
        public static Int16? GetInt16(object oField)
        {
            return IsNullOrDBNull(oField) ? null : (Int16?)oField;
        }
        public static Int32? GetInt32(object oField)
        {
            return IsNullOrDBNull(oField) ? null : (Int32?)oField;
        }
        public static Int64? GetInt64(object oField)
        {
            return IsNullOrDBNull(oField) ? null : (Int64?)oField;
        }

        public static Boolean Fetch(object oField, Boolean bDefault)
        {
            return IsNullOrDBNull(oField) ? bDefault : (Boolean)oField;
        }
        public static Byte Fetch(object oField, Byte bDefault)
        {
            return IsNullOrDBNull(oField) ? bDefault : (Byte)oField;
        }
        public static DateTime Fetch(object oField, DateTime dDefault)
        {
            return IsNullOrDBNull(oField) ? dDefault : (DateTime)oField;
        }
        public static Decimal Fetch(object oField, Decimal dDefault)
        {
            return IsNullOrDBNull(oField) ? dDefault : (Decimal)oField;
        }
        public static Double Fetch(object oField, Double dDefault)
        {
            return IsNullOrDBNull(oField) ? dDefault : (Double)oField;
        }
        public static float Fetch(object oField, float fDefault)
        {
            return IsNullOrDBNull(oField) ? fDefault : (float)oField;
        }
        public static Int16 Fetch(object oField, Int16 iDefault)
        {
            return IsNullOrDBNull(oField) ? iDefault : (Int16)oField;
        }
        public static Int32 Fetch(object oField, Int32 iDefault)
        {
            return IsNullOrDBNull(oField) ? iDefault : (Int32)oField;
        }
        public static Int64 Fetch(object oField, Int64 iDefault)
        {
            return IsNullOrDBNull(oField) ? iDefault : (Int64)oField;
        }

        public static string FetchString(object oField, string sDefaultValue)
        {
            if (IsNullOrDBNull(oField))
                return sDefaultValue;

            Type t1 = oField.GetType();
            if (t1 == typeof(System.Boolean))
                return ((Boolean)GetBoolean(oField)) ? "1" : "0";
            if (t1 == typeof(System.DateTime))
                return ((DateTime)GetDateTime(oField)).ToString("yyyyMMddHHmmssfff");

            return oField.ToString();
        }
        public static string FetchString(object oField)
        {
            return FetchString(oField, null);
        }
        public static string FetchString(Boolean? bInput, string sDefaultValue)
        {
            if (bInput == null)
                return sDefaultValue;
            else
                return (Boolean)bInput ? "1" : "0";
        }
        public static string FetchString(Boolean? bInput)
        {
            return FetchString(bInput, null);
        }

        public static string FetchString(DateTime? dInput, string sDefaultValue)
        {
            if (dInput == null)
                return sDefaultValue;
            else
                return ((DateTime)dInput).ToString("yyyyMMddHHmmssfff");
        }
        public static string FetchString(DateTime? dInput)
        {
            return FetchString(dInput, null);
        }
        #endregion Field

        #region StringToField
        public static Boolean? StringToFieldBoolean(string sInput)
        {
            if (string.IsNullOrWhiteSpace(sInput))
                return null;

            string s1 = sInput.Trim();
            if (s1 == "1")
                return true;
            if (s1.ToUpper() == "TRUE")
                return true;

            return false;
        }

        public static DateTime? StringToFieldDateTime(string sInput)
        {
            if (string.IsNullOrWhiteSpace(sInput))
                return null;

            DateTime dOutput;
            if (DateTime.TryParseExact(sInput, "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture, DateTimeStyles.None, out dOutput))
                return dOutput;
            return null;
        }

        public static int? StringToFieldInt(string sInput)
        {
            if (string.IsNullOrWhiteSpace(sInput))
                return null;

            int iValue = 0;
            if (int.TryParse(sInput, out iValue))
                return iValue;

            return null;
        }
        public static long? StringToFieldLong(string sInput)
        {
            if (string.IsNullOrWhiteSpace(sInput))
                return null;

            long lValue = 0;
            if (long.TryParse(sInput, out lValue))
                return lValue;

            return null;
        }

        public static Decimal? StringToFieldDecimal(string sInput)
        {
            if (string.IsNullOrWhiteSpace(sInput))
                return null;

            Decimal dValue = 0M;
            if (Decimal.TryParse(sInput, out dValue))
                return dValue;

            return null;
        }

        public static Boolean StringToField(string sInput, Boolean bDefaultValue)
        {
            return StringToFieldBoolean(sInput) ?? bDefaultValue;
        }

        public static DateTime StringToField(string sInput, DateTime dDefaultValue)
        {
            return StringToFieldDateTime(sInput) ?? dDefaultValue;
        }

        public static int StringToField(string sInput, int iDefaultValue)
        {
            return StringToFieldInt(sInput) ?? iDefaultValue;
        }
        public static long StringToField(string sInput, long lDefaultValue)
        {
            return StringToFieldLong(sInput) ?? lDefaultValue;
        }

        public static Decimal StringToField(string sInput, Decimal dDefaultValue)
        {
            return StringToFieldDecimal(sInput) ?? dDefaultValue;
        }
        #endregion

    }
}
