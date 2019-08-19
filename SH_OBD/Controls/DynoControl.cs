using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DGChart {
    public class DynoControl : UserControl {
        private string strName;
        private Color colorGrid;
        private Color colorBg;
        private Color colorAxis;
        private Font fontAxis;
        private int penWidth;
        private DynoControl.DrawModeType drawMode;
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
        private Color colorSet1;
        private Color colorSet2;
        private Color colorSet3;
        private Color colorSet4;
        private Color colorSet5;
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
        private Image m_imgLogo;
        private double m_dMaxHpValue;
        private double m_dMaxHpRpm;
        private double m_dMaxTqValue;
        private double m_dMaxTqRpm;

        [Category("Chart")]
        [Description("The logo to display in the top left corner.")]
        public Image Logo {
            get { return m_imgLogo; }
            set {
                m_imgLogo = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [Description("The y values for the fifth data set.")]
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

        [Category("Chart")]
        [Description("The y values for the third data set.")]
        public double[] YData3 {
            get { return yData3; }
            set {
                yData3 = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [Description("The x values for the third data set.")]
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

        [Category("Chart")]
        [Description("The y values for the first data set.")]
        public double[] YData1 {
            get { return yData1; }
            set {
                yData1 = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [Description("The x values for the first data set.")]
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

        [DefaultValue("Gold")]
        [Category("Chart")]
        [Description("The color to represent the fourth data set.")]
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
        [Category("Chart")]
        [Description("The color to represent the second data set.")]
        public Color ColorSet2 {
            get { return colorSet2; }
            set {
                colorSet2 = value;
                Invalidate();
            }
        }

        [DefaultValue("DarkBlue")]
        [Category("Chart")]
        [Description("The color to represent the first data set.")]
        public Color ColorSet1 {
            get { return colorSet1; }
            set {
                colorSet1 = value;
                Invalidate();
            }
        }

        [DefaultValue(0)]
        [Category("Chart")]
        [Description("Display the fifth data set?")]
        public bool ShowData5 {
            get { return showData5; }
            set {
                showData5 = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [DefaultValue(0)]
        [Description("Display the fourth data set?")]
        public bool ShowData4 {
            get { return showData4; }
            set {
                showData4 = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [DefaultValue(0)]
        [Description("Display the third data set?")]
        public bool ShowData3 {
            get { return showData3; }
            set {
                showData3 = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [DefaultValue(0)]
        [Description("Display the second data set?")]
        public bool ShowData2 {
            get { return showData2; }
            set {
                showData2 = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [DefaultValue(1)]
        [Description("Display the first data set?")]
        public bool ShowData1 {
            get { return showData1; }
            set {
                showData1 = value;
                Invalidate();
            }
        }

        [DefaultValue(0)]
        [Category("Chart")]
        [Description("The base for log. views in y direction. If < 2 then a linear view is displayed")]
        public int YLogBase {
            get { return yLogBase; }
            set {
                yLogBase = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [DefaultValue(0)]
        [Description("The base for log. views in x direction. If < 2 then a linear view is displayed")]
        public int XLogBase {
            get { return xLogBase; }
            set {
                xLogBase = value;
                Invalidate();
            }
        }

        [DefaultValue(10)]
        [Category("Chart")]
        [Description("The spacing for the linear grid in y direction. Ingnored for log. views")]
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

        [Category("Chart")]
        [DefaultValue(100)]
        [Description("The end of the data range on the y axis")]
        public double YRangeEnd {
            get { return yRangeEnd; }
            set {
                yRangeEnd = value;
                Invalidate();
            }
        }

        [DefaultValue(0)]
        [Category("Chart")]
        [Description("The start of the data range on the y axis")]
        public double YRangeStart {
            get { return yRangeStart; }
            set {
                yRangeStart = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [DefaultValue(100)]
        [Description("The end of the data range on the x axis")]
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

        [DefaultValue(30)]
        [Category("Chart")]
        [Description("The internal border at the right")]
        public int BorderRight {
            get { return borderRight; }
            set {
                borderRight = value;
                Invalidate();
            }
        }

        [DefaultValue(50)]
        [Category("Chart")]
        [Description("The internal border at the bottom")]
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

        [DefaultValue(30)]
        [Category("Chart")]
        [Description("The internal border at the top")]
        public int BorderTop {
            get { return borderTop; }
            set {
                borderTop = value;
                Invalidate();
            }
        }

        [DefaultValue("DrawModeType::Line")]
        [Category("Chart")]
        [Description("Draw mode for the data points")]
        public DynoControl.DrawModeType DrawMode {
            get { return drawMode; }
            set {
                drawMode = value;
                Invalidate();
            }
        }

        [DefaultValue(2)]
        [Category("Chart")]
        [Description("The width of the data lines.")]
        public int PenWidth {
            get { return penWidth; }
            set {
                penWidth = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [DefaultValue("Arial, 8pt")]
        [Description("The font for the text")]
        public Font FontAxis {
            get { return fontAxis; }
            set {
                fontAxis = value;
                Invalidate();
            }
        }

        [Category("Chart")]
        [DefaultValue("Black")]
        [Description("The color of the axes and text.")]
        public Color ColorAxis {
            get { return colorAxis; }
            set {
                colorAxis = value;
                Invalidate();
            }
        }

        [DefaultValue("White")]
        [Category("Chart")]
        [Description("The background color.")]
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

        [Category("Chart")]
        [DefaultValue("")]
        [Description("The information to display above the chart.")]
        public string Label {
            get { return strName; }
            set {
                strName = value;
                Invalidate();
            }
        }

        public DynoControl() {
            colorGrid = new Color();
            colorBg = new Color();
            colorAxis = new Color();
            colorSet1 = new Color();
            colorSet2 = new Color();
            colorSet3 = new Color();
            colorSet4 = new Color();
            colorSet5 = new Color();
            InitializeComponent();

            // 使用双缓冲技术重绘控件，下面三个控件样式都须设置才有效果

            // 为 true，控件将自行绘制，而不是通过操作系统来绘制。此样式仅适用于派生自 Control 的类。
            SetStyle(ControlStyles.UserPaint, true);
            // 为 true，控件将忽略 WM_ERASEBKGND 窗口消息以减少闪烁。仅当 UserPaint 位设置为 true 时，才应当应用该样式。
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            // 为 true，则绘制在缓冲区中进行，完成后将结果输出到屏幕上。
            SetStyle(ControlStyles.DoubleBuffer, true);
        }

        private void InitializeComponent() {
            colorGrid = Color.LightGray;
            colorBg = Color.White;
            colorAxis = Color.Black;
            fontAxis = new Font("Arial", 8f);
            penWidth = 2;
            drawMode = DynoControl.DrawModeType.Line;
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
                    MessageBox.Show("x or y Range error!");
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
                MessageBox.Show("Chart_Paint() in DynoControl occurred ERROR");
            }
        }

        private void PaintControl(Graphics g) {
            double[] numArray1 = null;
            double[] numArray2 = null;
            Pen pen1 = null;
            Pen pen2 = null;
            Pen pen3 = null;
            Pen pen4 = null;
            Pen pen5 = null;
            SolidBrush solidBrush = null;
            StringFormat format = null;
            try {
                //g.FillRectangle(new SolidBrush(colorBg), clientRectangle);
                pen1 = new Pen(colorGrid, 1f);
                pen2 = new Pen(colorAxis, 1f);
                solidBrush = new SolidBrush(colorAxis);
                int left = ClientRectangle.Left + borderLeft;
                int top = ClientRectangle.Top + borderTop;
                int width = ClientRectangle.Width - borderLeft - borderRight;
                int height = ClientRectangle.Height - borderTop - borderBottom;
                int right = ClientRectangle.Right - borderRight;
                int bottom = ClientRectangle.Bottom - borderBottom;
                int xCount = 1;
                int xGridPixel = 0;
                format = new StringFormat();

                if (xLogBase < 2) {
                    xCount = Convert.ToInt32((xRangeEnd - xRangeStart) / xGrid);
                    if (xCount == 0) {
                        xCount = 1;
                    }
                    xGridPixel = width / xCount;
                    // x轴坐标
                    for (int i = 0; i <= xCount; ++i) {
                        int xCoor = i * xGridPixel + left;
                        g.DrawLine(pen1, xCoor, top, xCoor, bottom);
                        string str = Convert.ToString((xRangeEnd - xRangeStart) * i / (double)xCount + xRangeStart);
                        SizeF sizeF = g.MeasureString(str, fontAxis);
                        g.DrawString(str, fontAxis, solidBrush, xCoor - sizeF.Width * 0.5f, sizeF.Height * 0.5f + bottom);
                    }
                } else {
                    xCount = Convert.ToInt32(Math.Log(xRangeEnd, xLogBase) - Math.Log(xRangeStart, xLogBase));
                    if (xCount == 0) {
                        xCount = 1;
                    }
                    xGridPixel = width / xCount;
                    // x轴坐标
                    for (int i = 0; i <= xCount; ++i) {
                        int xCoor = i * xGridPixel + left;
                        if (i < xCount) {
                            for (int j = 1; j < xLogBase; ++j) {
                                int xCoorLog = xCoor + Convert.ToInt32(Math.Log(j, xLogBase) * xGridPixel);
                                g.DrawLine(pen1, xCoorLog, top, xCoorLog, bottom);
                            }
                        }
                        string str = Convert.ToString(Math.Pow(xLogBase, Math.Log(xRangeStart, xLogBase) + i));
                        SizeF sizeF = g.MeasureString(str, fontAxis);
                        g.DrawString(str, fontAxis, solidBrush, xCoor - sizeF.Width * 0.5f, sizeF.Height * 0.5f + bottom);
                    }
                }
                SizeF sizeF1 = g.MeasureString("RPM (x 1000)", fontAxis);
                g.DrawString("RPM (x 1000)", fontAxis, solidBrush, (float)((right - left) / 2 + left) - sizeF1.Width * 0.5f, sizeF1.Height * 2f + bottom);

                int yCount = 1;
                int yGridPixel = 0;
                if (yLogBase < 2) {
                    yCount = Convert.ToInt32((yRangeEnd - yRangeStart) / yGrid);
                    if (yCount == 0) {
                        yCount = 1;
                    }
                    yGridPixel = height / yCount;
                    // y轴坐标
                    for (int i = 0; i <= yCount; ++i) {
                        int yCoor = bottom - i * yGridPixel;
                        g.DrawLine(pen1, left, yCoor, right, yCoor);
                        string str = Convert.ToString((yRangeEnd - yRangeStart) * i / (double)yCount + yRangeStart);
                        SizeF sizeF = g.MeasureString(str, fontAxis);
                        g.DrawString(str, fontAxis, solidBrush, left - sizeF.Width - sizeF.Height * 0.25f, yCoor - sizeF.Height * 0.5f);
                    }
                } else {
                    yCount = Convert.ToInt32(Math.Log(yRangeEnd, yLogBase) - Math.Log(yRangeStart, yLogBase));
                    if (yCount == 0) {
                        yCount = 1;
                    }
                    yGridPixel = height / yCount;
                    // y轴坐标
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
                        g.DrawString(str, fontAxis, solidBrush, left - sizeF.Width - sizeF.Height * 0.25f, yCoor - sizeF.Height * 0.5f);
                    }
                }

                g.DrawRectangle(pen2, left, top, width, height);

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
                        m_dMaxHpValue = 0.0;
                        m_dMaxHpRpm = 0.0;
                        break;
                    case 1:
                        numArray1 = xData2;
                        numArray2 = yData2;
                        flag = showData2;
                        color2 = colorSet2;
                        m_dMaxTqValue = 0.0;
                        m_dMaxTqRpm = 0.0;
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
                    if (flag && numArray1 != null && numArray2 != null && numArray1.Length == numArray2.Length) {
                        Point[] pointArray = new Point[numArray1.Length];
                        Point point2 = new Point(left, bottom);
                        for (int j = 0; j < pointArray.Length; ++j) {
                            try {
                                if (i == 0) {
                                    if (numArray2[j] > m_dMaxHpValue) {
                                        m_dMaxHpValue = numArray2[j];
                                        m_dMaxHpRpm = numArray1[j];
                                    }
                                } else if (numArray2[j] > m_dMaxTqValue) {
                                    m_dMaxTqValue = numArray2[j];
                                    m_dMaxTqRpm = numArray1[j];
                                }
                                pointArray[j].X = xLogBase >= 2 ? Convert.ToInt32((Math.Log(numArray1[j], xLogBase) - Math.Log(xRangeStart, xLogBase)) / (Math.Log(xRangeEnd, xLogBase) - Math.Log(xRangeStart, xLogBase)) * xPixel + left) : Convert.ToInt32((numArray1[j] - xRangeStart) / (xRangeEnd - xRangeStart) * xPixel + left);
                                pointArray[j].Y = yLogBase >= 2 ? Convert.ToInt32(bottom - (Math.Log(numArray2[j], yLogBase) - Math.Log(yRangeStart, yLogBase)) / (Math.Log(yRangeEnd, yLogBase) - Math.Log(yRangeStart, yLogBase)) * yPixel) : Convert.ToInt32(bottom - (numArray2[j] - yRangeStart) / (yRangeEnd - yRangeStart) * yPixel);
                                point2 = pointArray[j];
                            } catch (Exception) {
                                pointArray[j] = point2;
                            }
                        }
                        pen3 = new Pen(color2, penWidth);
                        for (int j = 0; j < pointArray.Length; ++j) {
                            switch (drawMode) {
                            case DynoControl.DrawModeType.Dot:
                                g.DrawEllipse(pen3, pointArray[j].X - penWidth / 2, pointArray[j].Y - penWidth / 2, penWidth, penWidth);
                                break;
                            case DynoControl.DrawModeType.Bar:
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

                // 画Logo
                if (Logo != null) {
                    g.DrawImage(Logo, 35, 10, 200, 75);
                }

                // 画图例
                string str1 = "马力 (HP)";
                string str2 = "扭矩 (LB*FT)";
                SizeF sizeF2 = g.MeasureString(str1, fontAxis);
                SizeF sizeF3 = g.MeasureString(str2, fontAxis);
                Rectangle rect1 = new Rectangle(
                    borderLeft + 5,
                    top,
                    (int)(sizeF3.Width + (double)sizeF2.Width) + 80,
                    (int)sizeF2.Height + 20
                );
                //g.FillRectangle(Brushes.White, rect1);
                //g.DrawRectangle(new Pen(Brushes.Black), rect1);
                pen4 = new Pen(colorSet1);
                pen5 = new Pen(colorSet2);
                g.DrawString(str1, fontAxis, pen4.Brush, rect1.Left + 35, rect1.Top + 10);
                g.DrawString(str2, fontAxis, pen5.Brush, rect1.Left + 70 + sizeF2.Width, rect1.Top + 10);
                g.FillRectangle(pen4.Brush, new Rectangle(rect1.Left + 10, rect1.Top + 10, 20, (int)sizeF2.Height));
                g.FillRectangle(pen5.Brush, new Rectangle(rect1.Left + (int)sizeF2.Width + 45, rect1.Top + 10, 20, (int)sizeF3.Height));

                // 画标签
                format.Alignment = StringAlignment.Far;
                format.LineAlignment = StringAlignment.Far;
                g.DrawString(strName, fontAxis, solidBrush, new RectangleF(230f, height - 5f, Width - 255f, 75f), format);

                // 画RPM极值
                string str3 = m_dMaxHpValue.ToString("####.##") + " RWHP @ " + (m_dMaxHpRpm * 1000.0).ToString("#####") + " RPM" + "\r\n\r\n" + m_dMaxTqValue.ToString("####.##")
                    + " LB*FT @ " + (m_dMaxTqRpm * 1000.0).ToString("#####") + " RPM";
                SizeF sizeF4 = g.MeasureString(str3, fontAxis);
                Rectangle rect4 = new Rectangle(
                    Width - 25 - (int)sizeF4.Width - borderRight,
                    borderTop + 5,
                    (int)sizeF4.Width + 20,
                    (int)sizeF4.Height + 20
                );
                //g.FillRectangle(Brushes.White, rect4);
                g.DrawRectangle(new Pen(Brushes.Black), rect4);
                RectangleF layoutRectangle2 = new RectangleF(
                    Width - borderRight - sizeF4.Width - 25.0f,
                    borderTop + 5,
                    sizeF4.Width + 20f,
                    sizeF4.Height + 20f
                );
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                g.DrawString(str3, fontAxis, solidBrush, layoutRectangle2, format);
            } catch {
                MessageBox.Show("PaintControl() in DynoControl occurred ERROR");
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
                if (pen4 != null) {
                    pen4.Dispose();
                }
                if (pen5 != null) {
                    pen5.Dispose();
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
            using (Graphics gfx = Graphics.FromImage(bitmap)) {
                gfx.FillRectangle(new SolidBrush(colorBg), 0, 0, Width, Height);
                PaintControl(gfx);
            }
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
