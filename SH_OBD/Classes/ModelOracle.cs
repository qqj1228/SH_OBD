using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SH_OBD {
    public class ModelOracle {
        public readonly Logger m_log;
        public OracleMESSetting m_oracleMESSetting;
        private string Connection { get; set; }
        public bool Connected { get; set; }

        public ModelOracle(OracleMESSetting oracleMESSetting, Logger log) {
            m_log = log;
            m_oracleMESSetting = oracleMESSetting;
            this.Connection = "";
            ReadConfig();
            Connected = false;
        }

        void ReadConfig() {
            Connection = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=";
            Connection += m_oracleMESSetting.Host + ")(PORT=";
            Connection += m_oracleMESSetting.Port + "))(CONNECT_DATA=(SERVICE_NAME=";
            Connection += m_oracleMESSetting.ServiceName + ")));";
            Connection += "Persist Security Info=True;";
            Connection += "User ID=" + m_oracleMESSetting.UserID + ";";
            Connection += "Password=" + m_oracleMESSetting.PassWord + ";";
        }

        public bool ConnectOracle() {
            Connected = false;
            try {
                OracleConnection con = new OracleConnection(Connection);
                con.Open();
                Connected = true;
                con.Close();
                con.Dispose();
            } catch (Exception ex) {
                m_log.TraceError("Connection error: " + ex.Message);
                return Connected;
            }
            return Connected;
        }

        /// <summary>
        /// 执行update insert delete语句，失败了返回-1，成功了返回影响的行数,注意：自动commit
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        private int ExecuteNonQuery(string strSQL) {
            using (OracleConnection connection = new OracleConnection(Connection)) {
                int val = -1;
                try {
                    connection.Open();
                    OracleCommand cmd = new OracleCommand(strSQL, connection);
                    val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                } catch (OracleException ex) {
                    m_log.TraceError("Error SQL: " + strSQL);
                    m_log.TraceError(ex.Message);
                    throw new Exception(ex.Message);
                } finally {
                    if (connection.State != ConnectionState.Closed) {
                        connection.Close();
                    }
                }
                return val;
            }
        }

        private void Query(string strSQL, DataTable dt) {
            using (OracleConnection connection = new OracleConnection(Connection)) {
                try {
                    connection.Open();
                    OracleDataAdapter adapter = new OracleDataAdapter(strSQL, connection);
                    adapter.Fill(dt);
                } catch (OracleException ex) {
                    m_log.TraceError("Error SQL: " + strSQL);
                    m_log.TraceError(ex.Message);
                    throw new Exception(ex.Message);
                } finally {
                    if (connection.State != ConnectionState.Closed) {
                        connection.Close();
                    }
                }
            }
        }

        private object QueryOne(string strSQL) {
            using (OracleConnection connection = new OracleConnection(Connection)) {
                using (OracleCommand cmd = new OracleCommand(strSQL, connection)) {
                    try {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value))) {
                            return null;
                        } else {
                            return obj;
                        }
                    } catch (OracleException ex) {
                        m_log.TraceError("Error SQL: " + strSQL);
                        m_log.TraceError(ex.Message);
                        throw new Exception(ex.Message);
                    } finally {
                        if (connection.State != ConnectionState.Closed) {
                            connection.Close();
                        }
                    }
                }
            }
        }

        public int InsertRecords(string strTable, DataTable dt) {
            int iRet = 0;
            for (int iRow = 0; iRow < dt.Rows.Count; iRow++) {
                string strSQL = "insert into " + strTable + " (ID,";
                for (int iCol = 1; iCol < dt.Columns.Count; iCol++) {
                    if (dt.Rows[iRow][iCol].ToString().Length != 0) {
                        strSQL += dt.Columns[iCol].ColumnName + ",";
                    }
                }
                strSQL = strSQL.Trim(',');
                strSQL += ") values (SEQ_EM_WQPF_ID.NEXTVAL,";
                for (int iCol = 1; iCol < dt.Columns.Count; iCol++) {
                    if (dt.Rows[iRow][iCol].ToString().Length != 0) {
                        if (dt.Columns[iCol].DataType == typeof(DateTime)) {
                            strSQL += "to_date('" + ((DateTime)dt.Rows[iRow][iCol]).ToString("yyyyMMdd-HHmmss") + "', 'yyyymmdd-HH24MISS'),";
                        } else {
                            strSQL += "'" + dt.Rows[iRow][iCol].ToString() + "',";
                        }
                    }
                }
                strSQL = strSQL.Trim(',');
                strSQL += ")";
                iRet += ExecuteNonQuery(strSQL);
            }
            return iRet;
        }

        public int UpdateRecords(string strTable, DataTable dt, string strWhereKey, string[] strWhereValues) {
            int iRet = 0;
            if (dt.Rows.Count != strWhereValues.Length) {
                return -1;
            }
            for (int iRow = 0; iRow < dt.Rows.Count; iRow++) {
                string strSQL = "update " + strTable + " set ";
                for (int iCol = 1; iCol < dt.Columns.Count; iCol++) {
                    if (dt.Rows[iRow][iCol].ToString().Length != 0 && dt.Rows[iRow][iCol].ToString() != dt.Columns[iCol].ColumnName) {
                        if (dt.Columns[iCol].DataType == typeof(DateTime)) {
                            strSQL += dt.Columns[iCol].ColumnName + "=" + "to_date('" + ((DateTime)dt.Rows[iRow][iCol]).ToString("yyyyMMdd-HHmmss") + "', 'yyyymmdd-HH24MISS'),";
                        } else {
                            strSQL += dt.Columns[iCol].ColumnName + "='" + dt.Rows[iRow][iCol].ToString() + "',";
                        }
                    }
                }
                strSQL = strSQL.Trim(',');
                strSQL += " where " + strWhereKey + "='" + strWhereValues[iRow] + "'";
                iRet += ExecuteNonQuery(strSQL);
            }
            return iRet;
        }

        public string[] GetValue(string strTable, string strField, string strWhereKey, string strWhereValue) {
            string strSQL = "select " + strField + " from " + strTable + " where " + strWhereKey + " = '" + strWhereValue + "'";
            DataTable dt = new DataTable();
            Query(strSQL, dt);
            string[] values = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++) {
                values[i] = dt.Rows[i][0].ToString();
            }
            Array.Sort(values);
            dt.Dispose();
            return values;
        }

    }
}
