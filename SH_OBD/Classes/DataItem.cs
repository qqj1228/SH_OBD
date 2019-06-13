﻿using System;

namespace SH_OBD {
    internal class DataItem {
        public double Value;
        public long Ticks;

        public DataItem(double value, long iTicks) {
            Value = value;
            Ticks = iTicks;
        }

        public DataItem(double value) {
            Value = value;
            Ticks = DateTime.Now.Ticks;
        }
    }
}
