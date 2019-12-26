using System;
using System.IO;
using System.Text;
using System.Threading;

namespace SH_OBD {
    public abstract class CommLine : CommBase {
        private static readonly Object locker = new Object();
        private int m_RxIndex = 0;
        private string m_RxString = "";
        private readonly ManualResetEvent m_TransFlag = new ManualResetEvent(true);
        private byte[] m_RxBuffer;
        private CommBase.ASCII m_RxTerm;
        private CommBase.ASCII[] m_TxTerm;
        private CommBase.ASCII[] m_RxFilter;
        private int m_TransTimeout;
        public string RxLine { get; set; } // 单独接收到的ELM327发来的消息

        protected CommLine(Logger log) : base(log) { }

        protected void SetTransTimeout(int iTimeOut) {
            m_TransTimeout = iTimeOut;
        }

        protected void SetRxFilter(CommBase.ASCII[] RxFilter) {
            m_RxFilter = RxFilter;
        }

        protected void Send(string data) {
            int len_data = Encoding.ASCII.GetByteCount(data);

            int len_term = 0;
            if (m_TxTerm != null) {
                len_term = m_TxTerm.Length;
            }

            byte[] sending = new byte[len_data + len_term];
            Encoding.ASCII.GetBytes(data).CopyTo(sending, 0);

            if (m_TxTerm != null) {
                m_TxTerm.CopyTo(sending, len_data);
            }
            base.Send(sending);
        }

        protected string Transact(string data) {
            Send(data);
            m_TransFlag.Reset();
            if (!m_TransFlag.WaitOne(m_TransTimeout, false)) {
                ThrowException("Timeout");
            }
            lock (locker) {
                RxLine = "";
                return m_RxString;
            }
        }

        protected void Setup(CommLine.CommLineSettings settings) {
            m_RxBuffer = new byte[settings.RxStringBufferSize];
            m_RxTerm = settings.RxTerminator;
            m_RxFilter = settings.RxFilter;
            m_TransTimeout = settings.TransactTimeout;
            m_TxTerm = settings.TxTerminator;
        }

        protected virtual void OnRxLine(string strLine) {
            RxLine = m_RxString;
        }

        protected override void OnRxChar(byte ch) {
            CommBase.ASCII ascii = (CommBase.ASCII)ch;
            if (ascii == m_RxTerm || m_RxIndex >= m_RxBuffer.Length) {
                lock (locker) {
                    m_RxString = Encoding.ASCII.GetString(m_RxBuffer, 0, m_RxIndex);
                }
                m_RxIndex = 0;
                // 检测是否已经m_TransFlag.Set()了
                if (m_TransFlag.WaitOne(0, false)) {
                    // 已经m_TransFlag.Set()了，表示是单独接收ELM327发来的消息
                    OnRxLine(m_RxString);
                } else {
                    // 没有m_TransFlag.Set()，表示是给ELM327发送命令后返回的消息
                    m_TransFlag.Set();
                }
                m_TransFlag.Set();
            } else {
                if (m_RxFilter != null) {
                    for (int idx = 0; idx < m_RxFilter.Length; ++idx) {
                        if (m_RxFilter[idx] == ascii) {
                            return;
                        }
                    }
                }
                m_RxBuffer[m_RxIndex] = ch;
                ++m_RxIndex;
            }
        }

        protected class CommLineSettings : CommBase.CommBaseSettings {
            public int RxStringBufferSize = 256;
            public CommBase.ASCII RxTerminator = CommBase.ASCII.CR;
            public int TransactTimeout = 500;
            public CommBase.ASCII[] RxFilter;
            public CommBase.ASCII[] TxTerminator;
        }
    }
}
