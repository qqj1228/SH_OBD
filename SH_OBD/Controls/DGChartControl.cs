using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DGChart {
    public class DGChartControl : UserControl {
        private string strName;
        private Color colorGrid = new Color();
        private Color colorBg = new Color();
        private Color colorAxis = new Color();
        private string xLabel;
        private string yLabel;
        private Font fontAxis;
        private int penWidth;
        private DGChartControl.DrawModeType drawMode;
        private int borderTop;
        private int borderLeft;
        private int borderBottom;
        private int borderRight;
        private double xRangeStart;
        private double xRangeEnd;
        private double yRangeStart;
        private double yRangeEnd;
        private double xGrid;
        private double yGrid;
        private int xLogBase;
        private int yLogBase;
        private bool showData1;
        private bool showData2;
        private bool showData3;
        private bool showData4;
        private bool showData5;
        private Color colorSet1 = new Color();
        private Color colorSet2 = new Color();
        private Color colorSet3 = new Color();
        private Color colorSet4 = new Color();
        private Color colorSet5 = new Color();
        private double[] xData1;
        private double[] yData1;
        private double[] xData2;
        private double[] yData2;
        private double[] xData3;
        private double[] yData3;
        private double[] xData4;
        private double[] yData4;
        private double[] xData5;
        private double[] yData5;

        public DGChartControl() {
            InitializeComponent();

            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }

        [Description("The y values for the fifth data set.")]
        [Category("Chart")]
        public double[] YData5 {
            get { return yData5; }
            set {
                yData5 = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [Description("The x values for the fifth data set.")]
        public double[] XData5 {
            get { return xData5; }
            set {
                xData5 = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [Description("The y values for the fourth data set.")]
        public double[] YData4 {
            get { return yData4; }
            set {
                yData4 = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [Description("The x values for the fourth data set.")]
        public double[] XData4 {
            get { return xData4; }
            set {
                xData4 = value;
                Invalidate();
            }
        }

        [Description("The y values for the third data set.")]
        [Category("Chart")]
        public double[] YData3 {
            get { return yData3; }
            set {
                yData3 = value;
                Invalidate();
            }
        }

        [Description("The x values for the third data set.")]
        [Category("Chart")]
        public double[] XData3 {
            get { return xData3; }
            set {
                xData3 = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [Description("The y values for the second data set.")]
        public double[] YData2 {
            get { return yData2; }
            set {
                yData2 = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [Description("The x values for the second data set.")]
        public double[] XData2 {
            get { return xData2; }
            set {
                xData2 = value;
                Invalidate();
            }
        }

        [Description("The y values for the first data set.")]
        [Category("Chart")]
        public double[] YData1 {
            get { return yData1; }
            set {
                yData1 = value;
                Invalidate();
            }
        }

        [Description("The x values for the first data set.")]
        [Category("Chart")]
        public double[] XData1 {
            get { return xData1; }
            set {
                xData1 = value;
                Invalidate();
            }
        }

        [DefaultValue("Magenta")]
        [Category("Chart")]
        [Description("The color to represent the fifth data set.")]
        public Color ColorSet5 {
            get { return colorSet5; }
            set {
                colorSet5 = value;
                Invalidate();
            }
        }

        [Description("The color to represent the fourth data set.")]
        [Category("Chart")]
        [DefaultValue("Gold")]
        public Color ColorSet4 {
            get { return colorSet4; }
            set {
                colorSet4 = value;
                Invalidate();
            }
        }

        [DefaultValue("Lime")]
        [Category("Chart")]
        [Description("The color to represent the third data set.")]
        public Color ColorSet3 {
            get { return colorSet3; }
            set {
                colorSet3 = value;
                Invalidate();
            }
        }

        [DefaultValue("Red")]
        [Description("The color to represent the second data set.")]
        [Category("Chart")]
        public Color ColorSet2 {
            get { return colorSet2; }
            set {
                colorSet2 = value;
                Invalidate();
            }
        }

        [Description("The color to represent the first data set.")]
        [DefaultValue("DarkBlue")]
        [Category("Chart")]
        public Color ColorSet1 {
            get { return colorSet1; }
            set {
                colorSet1 = value;
                Invalidate();
            }
        }

        [DefaultValue(0)]
        [Description("Display the fifth data set?")]
        [Category("Chart")]
        public bool ShowData5 {
            get { return showData5; }
            set {
                showData5 = value;
                Invalidate();
            }
        }

        [DefaultValue(0)]
        [Description("Display the fourth data set?")]
        [Category("Chart")]
        public bool ShowData4 {
            get { return showData4; }
            set {
                showData4 = value;
                Invalidate();
            }
        }

        [Description("Display the third data set?")]
        [Category("Chart")]
        [DefaultValue(0)]
        public bool ShowData3 {
            get { return showData3; }
            set {
                showData3 = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [Description("Display the second data set?")]
        [DefaultValue(0)]
        public bool ShowData2 {
            get { return showData2; }
            set {
                showData2 = value;
                Invalidate();
            }
        }

        [Description("Display the first data set?")]
        [Category("Chart")]
        [DefaultValue(1)]
        public bool ShowData1 {
            get { return showData1; }
            set {
                showData1 = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [DefaultValue(0)]
        [Description("The base for log. views in y direction. If < 2 then a linear view is displayed")]
        public int YLogBase {
            get { return yLogBase; }
            set {
                yLogBase = value;
                Invalidate();
            }
        }

        [Description("The base for log. views in x direction. If < 2 then a linear view is displayed")]
        [DefaultValue(0)]
        [Category("Chart")]
        public int XLogBase {
            get { return xLogBase; }
            set {
                xLogBase = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [Description("The spacing for the linear grid in y direction. Ingnored for log. views")]
        [DefaultValue(10)]
        public double YGrid {
            get { return yGrid; }
            set {
                yGrid = value;
                Invalidate();
            }
        }

        [DefaultValue(10)]
        [Category("Chart")]
        [Description("The spacing for the linear grid in x direction. Ingnored for log. views")]
        public double XGrid {
            get { return xGrid; }
            set {
                xGrid = value;
                Invalidate();
            }
        }

        [Description("The end of the data range on the y axis")]
        [DefaultValue(100)]
        [Category("Chart")]
        public double YRangeEnd {
            get { return yRangeEnd; }
            set {
                yRangeEnd = value;
                Invalidate();
            }
        }

        [Description("The start of the data range on the y axis")]
        [Category("Chart")]
        [DefaultValue(0)]
        public double YRangeStart {
            get { return yRangeStart; }
            set {
                yRangeStart = value;
                Invalidate();
            }
        }

        [DefaultValue(100)]
        [Description("The end of the data range on the x axis")]
        [Category("Chart")]
        public double XRangeEnd {
            get { return xRangeEnd; }
            set {
                xRangeEnd = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [DefaultValue(0)]
        [Description("The start of the data range on the x axis")]
        public double XRangeStart {
            get { return xRangeStart; }
            set {
                xRangeStart = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [DefaultValue(30)]
        [Description("The internal border at the right")]
        public int BorderRight {
            get { return borderRight; }
            set {
                borderRight = value;
                Invalidate();
            }
        }

        [Description("The internal border at the bottom")]
        [DefaultValue(50)]
        [Category("Chart")]
        public int BorderBottom {
            get { return borderBottom; }
            set {
                borderBottom = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [DefaultValue(50)]
        [Description("The internal border at the left")]
        public int BorderLeft {
            get { return borderLeft; }
            set {
                borderLeft = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [DefaultValue(30)]
        [Description("The internal border at the top")]
        public int BorderTop {
            get { return borderTop; }
            set {
                borderTop = value;
                Invalidate();
            }
        }

        [Description("Draw mode for the data points")]
        [Category("Chart")]
        [DefaultValue("DrawModeType::Line")]
        public DGChartControl.DrawModeType DrawMode {
            get { return drawMode; }
            set {
                drawMode = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [DefaultValue(2)]
        [Description("The width of the data lines.")]
        public int PenWidth {
            get { return penWidth; }
            set {
                penWidth = value;
                Invalidate();
            }
        }

        [DefaultValue("Arial, 8pt")]
        [Description("The font for the text")]
        [Category("Chart")]
        public Font FontAxis {
            get { return fontAxis; }
            set {
                fontAxis = value;
                Invalidate();
            }
        }

        [Description("The y axis label.")]
        [DefaultValue("")]
        [Category("Chart")]
        public string YLabel {
            get { return yLabel; }
            set {
                yLabel = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [DefaultValue("")]
        [Description("The x axis label.")]
        public string XLabel {
            get { return xLabel; }
            set {
                xLabel = value;
                Invalidate();
            }
        }

        [DefaultValue("Black")]
        [Category("Chart")]
        [Description("The color of the axes and text.")]
        public Color ColorAxis {
            get { return colorAxis; }
            set {
                colorAxis = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [Description("The background color.")]
        [DefaultValue("White")]
        public Color ColorBg {
            get { return colorBg; }
            set {
                colorBg = value;
                Invalidate();
            }
        }

        [DefaultValue("LightGray")]
        [Category("Chart")]
        [Description("The color of the grid lines.")]
        public Color ColorGrid {
            get { return colorGrid; }
            set {
                colorGrid = value;
                Invalidate();
            }
        }

        [Description("The name to display above the chart.")]
        [DefaultValue("")]
        [Category("Chart")]
        public override string Text {
            get { return strName; }
            set {
                strName = value;
                Invalidate();
            }
        }

        protected override void Dispose([MarshalAs(UnmanagedType.U1)] bool disposing) {
            fontAxis.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            colorGrid = Color.LightGray;
            colorBg = Color.White;
            colorAxis = Color.Black;
            fontAxis = new Font("Arial", 8f);
            penWidth = 2;
            drawMode = DGChartControl.DrawModeType.Line;
            borderTop = 30;
            borderLeft = 50;
            borderBottom = 50;
            borderRight = 30;
            xRangeStart = 0.0;
            xRangeEnd = 100.0;
            yRangeStart = 0.0;
            yRangeEnd = 100.0;
            xGrid = 10.0;
            yGrid = 10.0;
            xLogBase = 0;
            yLogBase = 0;
            showData1 = true;
            showData2 = false;
            showData3 = false;
            showData4 = false;
            showData5 = false;
            colorSet1 = Color.DarkBlue;
            colorSet2 = Color.Red;
            colorSet3 = Color.Lime;
            colorSet4 = Color.Gold;
            colorSet5 = Color.Magenta;
            Resize += new EventHandler(Chart_Resize);
            Paint += new PaintEventHandler(Chart_Paint);
        }

        private void Chart_Paint(object sender, PaintEventArgs e) {
            try {
                if (xRangeEnd <= xRangeStart || yRangeEnd <= yRangeStart) {
                    return;
                }
                if (xLogBase < 2) {
                    if (xGrid <= 0.0) {
                        return;
                    }
                } else if (xRangeStart <= 0.0) {
                    return;
                }
                if (yLogBase < 2) {
                    if (yGrid <= 0.0) {
                        return;
                    }
                } else if (yRangeStart <= 0.0) {
                    return;
                }
                Graphics graphics = e.Graphics;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                PaintControl(graphics);
            } catch (Exception) {
                MessageBox.Show("Chart_Paint() in DGChartControl occurred ERROR");
            }
        }

        private void PaintControl(Graphics g) {
            double[] numArray1 = null;
            double[] numArray2 = null;
            Pen pen1 = null;
            Pen pen2 = null;
            Pen pen3 = null;
            SolidBrush solidBrush = null;
            StringFormat format = null;

            try {
                pen1 = new Pen(colorGrid, 1f);
                pen2 = new Pen(colorAxis, 1f);
                solidBrush = new SolidBrush(colorAxis);
                Rectangle clientRectangle = ClientRectangle;
                Color color1 = colorBg;
                g.FillRectangle(new SolidBrush(color1), clientRectangle);
                int left = clientRectangle.Left + borderLeft;
                int top = clientRectangle.Top + borderTop;
                int width = clientRectangle.Width - borderLeft - borderRight;
                int height = clientRectangle.Height - borderTop - borderBottom;
                int right = clientRectangle.Right - borderRight;
                int bottom = clientRectangle.Bottom - borderBottom;
                int xCount = 1;
                int xGridPixel = 0;
                format = new StringFormat();

                // 画x轴
                if (xLogBase < 2) {
                    xCount = Convert.ToInt32((xRangeEnd - xRangeStart) / xGrid);
                    if (xCount == 0) {
                        xCount = 1;
                    }
                    xGridPixel = width / xCount;
                    for (int i = 0; i <= xCount; ++i) {
                        int xCoor = i * xGridPixel + left;
                        g.DrawLine(pen1, xCoor, top, xCoor, bottom);
                        string str = Convert.ToString((xRangeEnd - xRangeStart) * i / xCount + xRangeStart);
                        SizeF sizeF = g.MeasureString(str, fontAxis);
                        g.DrawString(str, fontAxis, solidBrush, xCoor - sizeF.Width * 0.5f, sizeF.Height * 0.5f + bottom);
                    }
                } else {
                    xCount = Convert.ToInt32(Math.Log(xRangeEnd, xLogBase) - Math.Log(xRangeStart, xLogBase));
                    if (xCount == 0) {
                        xCount = 1;
                    }
                    xGridPixel = width / xCount;
                    for (int i = 0; i <= xCount; ++i) {
                        int xCoor = i * xGridPixel + left;
                        if (i < xCount) {
                            for (int index2 = 1; index2 < xLogBase; ++index2) {
                                int xCoorLog = xCoor + Convert.ToInt32(Math.Log(index2, xLogBase) * xGridPixel);
                                g.DrawLine(pen1, xCoorLog, top, xCoorLog, bottom);
                            }
                        }
                        string str = Convert.ToString(Math.Pow(xLogBase, Math.Log(xRangeStart, xLogBase) + i));
                        SizeF sizeF = g.MeasureString(str, fontAxis);
                        g.DrawString(str, fontAxis, solidBrush, xCoor - sizeF.Width * 0.5f, sizeF.Height * 0.5f + bottom);
                    }
                }

                // 画y轴
                int yCount = 1;
                int yGridPixel = 0;
                if (yLogBase < 2) {
                    yCount = Convert.ToInt32((yRangeEnd - yRangeStart) / yGrid);
                    if (yCount == 0) {
                        yCount = 1;
                    }
                    yGridPixel = height / yCount;
                    for (int i = 0; i <= yCount; ++i) {
                        int yCoor = bottom - i * yGridPixel;
                        g.DrawLine(pen1, left, yCoor, right, yCoor);
                        string str = Convert.ToString((yRangeEnd - yRangeStart) * i / yCount + yRangeStart);
                        SizeF sizeF = g.MeasureString(str, fontAxis);
                        g.DrawString(str, fontAxis, solidBrush, (float)(left - sizeF.Width - sizeF.Height * 0.25), yCoor - sizeF.Height * 0.5f);
                    }
                } else {
                    yCount = Convert.ToInt32(Math.Log(yRangeEnd, yLogBase) - Math.Log(yRangeStart, yLogBase));
                    if (yCount == 0) {
                        yCount = 1;
                    }
                    yGridPixel = height / yCount;
                    for (int i = 0; i <= yCount; ++i) {
                        int yCoor = bottom - i * yGridPixel;
                        if (i < yCount) {
                            for (int j = 1; j < yLogBase; ++j) {
                                int yCoorLog = yCoor - Convert.ToInt32(Math.Log(j, yLogBase) * yGridPixel);
                                g.DrawLine(pen1, left, yCoorLog, right, yCoorLog);
                            }
                        }
                        string str = Convert.ToString(Math.Pow(yLogBase, Math.Log(yRangeStart, yLogBase) + i));
                        SizeF sizeF = g.MeasureString(str, fontAxis);
                        g.DrawString(str, fontAxis, solidBrush, (float)(left - sizeF.Width - sizeF.Height * 0.25), yCoor - sizeF.Height * 0.5f);
                    }
                }

                // 画外框
                g.DrawRectangle(pen2, left, top, width, height);
                // 画x轴标签
                SizeF sizeF1 = g.MeasureString(xLabel, fontAxis);
                g.DrawString(xLabel, fontAxis, solidBrush, ((right - left) / 2 + left) - sizeF1.Width * 0.5f, sizeF1.Height * 2f + bottom);
                // 画图表名称
                RectangleF layoutRectangle = new RectangleF(0.0f, 5f, Width, 20f);
                format.Alignment = StringAlignment.Center;
                g.DrawString(strName, fontAxis, solidBrush, layoutRectangle, format);

                // 画曲线
                int xPixel = xGridPixel * xCount;
                int yPixel = yGridPixel * yCount;
                for (int i = 0; i < 5; ++i) {
                    Color color2 = new Color();
                    bool flag = false;
                    switch (i) {
                    case 0:
                        numArray1 = xData1;
                        numArray2 = yData1;
                        flag = showData1;
                        color2 = colorSet1;
                        break;
                    case 1:
                        numArray1 = xData2;
                        numArray2 = yData2;
                        flag = showData2;
                        color2 = colorSet2;
                        break;
                    case 2:
                        numArray1 = xData3;
                        numArray2 = yData3;
                        flag = showData3;
                        color2 = colorSet3;
                        break;
                    case 3:
                        numArray1 = xData4;
                        numArray2 = yData4;
                        flag = showData4;
                        color2 = colorSet4;
                        break;
                    case 4:
                        numArray1 = xData5;
                        numArray2 = yData5;
                        flag = showData5;
                        color2 = colorSet5;
                        break;
                    }
                    if (flag && numArray1 != null && (numArray2 != null && numArray1.Length == numArray2.Length)) {
                        Point[] pointArray = new Point[numArray1.Length];
                        Point point2 = new Point(left, bottom);
                        for (int j = 0; j < pointArray.Length; ++j) {
                            try {
                                if (xLogBase >= 2) {
                                    pointArray[j].X = Convert.ToInt32((Math.Log(numArray1[j], xLogBase) - Math.Log(xRangeStart, xLogBase)) / (Math.Log(xRangeEnd, xLogBase) - Math.Log(xRangeStart, xLogBase)) * xPixel + left);
                                } else {
                                    pointArray[j].X = Convert.ToInt32((numArray1[j] - xRangeStart) / (xRangeEnd - xRangeStart) * xPixel + left);
                                }
                                if (yLogBase >= 2) {
                                    pointArray[j].Y = Convert.ToInt32(bottom - (Math.Log(numArray2[j], yLogBase) - Math.Log(yRangeStart, yLogBase)) / (Math.Log(yRangeEnd, yLogBase) - Math.Log(yRangeStart, yLogBase)) * yPixel);
                                } else {
                                    pointArray[j].Y = Convert.ToInt32(bottom - (numArray2[j] - yRangeStart) / (yRangeEnd - yRangeStart) * yPixel);
                                }
                                point2 = pointArray[j];
                            } catch (Exception) {
                                pointArray[j] = point2;
                            }
                        }
                        pen3 = new Pen(color2, penWidth);
                        for (int j = 0; j < pointArray.Length; ++j) {
                            switch (drawMode) {
                            case DGChartControl.DrawModeType.Dot:
                                g.DrawEllipse(pen3, pointArray[j].X - penWidth / 2, pointArray[j].Y - penWidth / 2, penWidth, penWidth);
                                break;
                            case DGChartControl.DrawModeType.Bar:
                                g.DrawLine(pen3, new Point(pointArray[j].X, bottom), pointArray[j]);
                                break;
                            default:
                                if (j > 0) {
                                    g.DrawLine(pen3, pointArray[j - 1], pointArray[j]);
                                }
                                break;
                            }
                        }
                    }
                }
            } catch (Exception) {
                MessageBox.Show("PaintControl() in DGChartControl occurred ERROR");
            } finally {
                if (pen1 != null) {
                    pen1.Dispose();
                }
                if (pen2 != null) {
                    pen2.Dispose();
                }
                if (pen3 != null) {
                    pen3.Dispose();
                }
                if (solidBrush != null) {
                    solidBrush.Dispose();
                }
                if (format != null) {
                    format.Dispose();
                }
            }
        }

        public Image GetImage() {
            Bitmap bitmap = new Bitmap(Width, Height);
            PaintControl(Graphics.FromImage(bitmap));
            return bitmap as Image;
        }

        public void Chart_Resize(object sender, EventArgs e) {
            Refresh();
        }

        public enum DrawModeType {
            Line = 1,
            Dot = 2,
            Bar = 3,
        }
    }
}
