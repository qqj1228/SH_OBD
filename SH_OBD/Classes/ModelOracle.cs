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
        public string Connection { get; set; }
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
            Connection += m_oracleMESSetting.host + ")(PORT=";
            Connection += m_oracleMESSetting.port + "))(CONNECT_DATA=(SERVICE_NAME=";
            Connection += m_oracleMESSetting.serviceName + ")));";
            Connection += "Persist Security Info=True;";
            Connection += "User ID=" + m_oracleMESSetting.userID + ";";
            Connection += "Password=" + m_oracleMESSetting.passWord + ";";
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

        public int InsertRecordIntoMES(string strTable, DataTable dt) {
            string strSQL = "insert into " + strTable + " (ID,";
            for (int i = 1; i < dt.Columns.Count; i++) {
                if (dt.Rows[0][i].ToString().Length != 0) {
                    strSQL += dt.Columns[i].ColumnName + ",";
                }
            }
            strSQL = strSQL.Trim(',');
#if DEBUG
            strSQL += ") values (SEQ_EM_WQPF_ID.NEXTVAL,";
#else
            strSQL += ") values (SEQ_EM_WQPF_ID.Next(),";
#endif
            for (int i = 1; i < dt.Columns.Count; i++) {
                if (dt.Rows[0][i].ToString().Length != 0) {
                    if (dt.Columns[i].DataType == typeof(DateTime)) {
                        strSQL += "to_date('" + ((DateTime)dt.Rows[0][i]).ToString("yyyyMMdd-HHmmss") + "', 'yyyymmdd-HH24MISS'),";
                    } else {
                        strSQL += "'" + dt.Rows[0][i].ToString() + "',";
                    }
                }
            }
            strSQL = strSQL.Trim(',');
            strSQL += ")";
            return ExecuteNonQuery(strSQL);
        }

        public string GetKeyIDByVIN(string strTable, string strVIN) {
            string strSQL = "select ID from " + strTable + " where VIN = '" + strVIN + "'";

            return (string)QueryOne(strSQL);
        }

        public void Select() {
            string strSQL = "select VIN from IF_EM_WQPF_1 where VIN = 'testvincode012345'";
            DataTable dt = new DataTable();
            Query(strSQL, dt);
            string ID = dt.Rows[0]["VIN"].ToString();
            dt.Dispose();
        }
    }
}
