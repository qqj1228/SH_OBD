﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace O2Waveform {
    public class O2WaveformControl : UserControl {
        private Color colorRichBG;
        private Color colorMidBG;
        private Color colorLeanBG;
        private Color colorLine;
        private Color colorWave;
        private Color colorTitle;
        private Color colorLabel;
        private Font fontTitle;
        private Font fontLabel;

        public O2WaveformControl() {
            InitializeComponent();

            colorRichBG = Color.Black;
            colorMidBG = Color.DarkGray;
            colorLeanBG = Color.Black;
            colorLine = Color.White;
            colorWave = Color.White;
            colorTitle = Color.White;
            colorLabel = Color.White;
            fontLabel = new Font("Arial", 10f);
            fontTitle = new Font("Arial", 10f);
        }

        private void InitializeComponent() {
            this.SuspendLayout();
            // 
            // O2WaveformControl
            // 
            this.Name = "O2WaveformControl";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.O2WaveformControl_Paint);
            this.Resize += new System.EventHandler(this.O2WaveformControl_Resize);
            this.ResumeLayout(false);

        }

        [DefaultValue("Arial, 10pt")]
        [Category("O2Waveform")]
        [Description("The font for the title.")]
        public Font TitleFont {
            get { return fontTitle; }
            set {
                fontTitle = value;
                Invalidate();
            }
        }

        [Description("The color for the title text.")]
        [DefaultValue("White")]
        [Category("O2Waveform")]
        public Color TitleColor {
            get { return colorTitle; }
            set {
                colorTitle = value;
                Invalidate();
            }
        }

        [Category("O2Waveform")]
        [DefaultValue("White")]
        [Description("The color for the label text.")]
        public Color LabelColor {
            get { return colorLabel; }
            set {
                colorLabel = value;
                Invalidate();
            }
        }

        [DefaultValue("Arial, 10pt")]
        [Category("O2Waveform")]
        [Description("The font for the label.")]
        public Font LabelFont {
            get { return fontLabel; }
            set {
                fontLabel = value;
                Invalidate();
            }
        }

        [DefaultValue("White")]
        [Category("O2Waveform")]
        [Description("The color for the waveform.")]
        public Color WaveColor {
            get { return colorWave; }
            set {
                colorWave = value;
                Invalidate();
            }
        }

        [Category("O2Waveform")]
        [Description("The color for the marker lines.")]
        [DefaultValue("White")]
        public Color LineColor {
            get { return colorLine; }
            set {
                colorLine = value;
                Invalidate();
            }
        }

        [DefaultValue("LightSkyBlue")]
        [Description("The background color for the lean region.")]
        [Category("O2Waveform")]
        public Color LeanBGColor {
            get { return colorLeanBG; }
            set {
                colorLeanBG = value;
                Invalidate();
            }
        }

        [Category("O2Waveform")]
        [DefaultValue("LightSkyBlue")]
        [Description("The background color for the middle region.")]
        public Color MidBGColor {
            get { return colorMidBG; }
            set {
                colorMidBG = value;
                Invalidate();
            }
        }

        [Category("O2Waveform")]
        [Description("The background color for the rich region.")]
        [DefaultValue("LightPink")]
        public Color RichBGColor {
            get { return colorRichBG; }
            set {
                colorRichBG = value;
                Invalidate();
            }
        }

        protected override void Dispose([MarshalAs(UnmanagedType.U1)] bool disposing) {
            base.Dispose(disposing);
        }

        private void O2WaveformControl_Paint(object sender, PaintEventArgs e) {
            Graphics graphics = e.Graphics;

            int num1 = Height / 3;

            graphics.FillRectangle(new SolidBrush(colorRichBG), new Rectangle(0, 0, Width, num1));
            graphics.FillRectangle(new SolidBrush(colorMidBG), new Rectangle(0, num1, Width, num1));
            graphics.FillRectangle(new SolidBrush(colorLeanBG), new Rectangle(0, num1 * 2, Width, num1));

            Pen pen1 = new Pen(colorLine, 1f);
            graphics.DrawLine(pen1, 0, num1, Width, num1);
            graphics.DrawLine(pen1, 0, num1 * 2, Width, num1 * 2);
            StringFormat format = new StringFormat {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            SolidBrush solidBrush1 = new SolidBrush(colorTitle);
            RectangleF layoutRectangle1 = new RectangleF(0.0f, 0.0f, Width, fontTitle.Height + 5f);
            graphics.DrawString("氧气传感器输出", fontTitle, solidBrush1, layoutRectangle1, format);
            graphics.DrawString("富油", fontTitle, solidBrush1, 2f, 2f);
            graphics.DrawString("贫油", fontTitle, solidBrush1, 2f, Height - fontTitle.Height - 2);
            Pen pen2 = new Pen(colorWave, 2f);
            for (int i = 25; i < Width - 25; i++) {
                double a1 = ((i - 1) / (double)Width) * (13.0 * Math.PI / 2.0) + Math.PI;
                double a2 = (i / (double)Width) * (13.0 * Math.PI / 2.0) + Math.PI;
                float y1 = (Height * 0.5f - (Height * 0.5f - (fontTitle.Height + 5f)) * (float)Math.Sin(a1));
                float y2 = (Height * 0.5f - (Height * 0.5f - (fontTitle.Height + 5f)) * (float)Math.Sin(a2));
                float x1 = i;
                graphics.DrawLine(pen2, x1, y1, x1 + 1f, y2);
            }
            SolidBrush solidBrush2 = new SolidBrush(colorLabel);
            float y1_1 = num1;
            float y = (float)(y1_1 - (double)fontLabel.Height - 2.0f);
            graphics.DrawString("$04", fontLabel, solidBrush2, 2f, y);
            graphics.DrawString("$03", fontLabel, solidBrush2, 2f, (y1_1 + 1) * 2);
            float num3 = Width * 0.135f;
            float num4 = (float)(Height - (double)(fontLabel.Height << 1) - 10.0f);
            Pen pen3 = new Pen(colorLabel, 1f);
            float y1_2 = y1_1 * 2f;
            graphics.DrawLine(pen3, num3, y1_2, num3, num4);
            float num5 = Width * 0.175f;
            float num6 = (float)(Height - (double)(fontLabel.Height << 1) - 10.0f);
            graphics.DrawLine(pen3, num5, y1_1, num5, num6);
            RectangleF layoutRectangle2 = new RectangleF(num3 - 10f, num4 + 5f, num5 - num3 + 20.0f, fontLabel.Height);
            graphics.DrawString("$06", fontLabel, solidBrush2, layoutRectangle2, format);
            PaintArrowLine(graphics, pen3, num3, num4, num5, num6);
            float num7 = Width * 0.288f;
            float num8 = (float)(Height - (double)(fontLabel.Height << 1) - 10.0f);
            graphics.DrawLine(pen3, num7, y1_1, num7, num8);
            float num9 = Width * 0.328f;
            float num10 = (float)(Height - (double)(fontLabel.Height << 1) - 10.0f);
            graphics.DrawLine(pen3, num9, y1_2, num9, num10);
            layoutRectangle2.X = num7 - 10f;
            graphics.DrawString("$05", fontLabel, solidBrush2, layoutRectangle2, format);
            PaintArrowLine(graphics, pen3, num7, num8, num9, num10);
            float num11 = Width * 0.443f;
            float num12 = (float)(Height - (double)(fontLabel.Height << 1) - 10.0f);
            graphics.DrawLine(pen3, num11, y1_2, num11, num12);
            float num13 = Width * 0.75f;
            float num14 = (float)(Height - (double)(fontLabel.Height << 1) - 10.0f);
            graphics.DrawLine(pen3, num13, y1_2, num13, num14);
            layoutRectangle2.X = num11 - 10f;
            layoutRectangle2.Width = (float)(num13 - (double)num11 + 20.0f);
            graphics.DrawString("$0A", fontLabel, solidBrush2, layoutRectangle2, format);
            PaintArrowLine(graphics, pen3, num11, num12, num13, num14);
            float num15 = Width * 0.768f;
            float y1_3 = Height * 0.5f + 10.0f;
            float num16 = (float)(Height - (double)(fontLabel.Height << 1) - 10.0f);
            graphics.DrawLine(pen3, num15, y1_3, num15, num16);
            float num17 = Width * 0.92f;
            float y1_4 = Height * 0.5f - 10.0f;
            float num18 = (float)(Height - (double)(fontLabel.Height << 1) - 10.0f);
            graphics.DrawLine(pen3, num17, y1_4, num17, num18);
            layoutRectangle2.X = num15 - 10f;
            layoutRectangle2.Width = (float)(num17 - (double)num15 + 20.0f);
            graphics.DrawString("$09", fontLabel, solidBrush2, layoutRectangle2, format);
            PaintArrowLine(graphics, pen3, num15, num16, num17, num18);
            float x1_1 = Width * 0.36f;
            float num19 = (float)(Height - (double)fontTitle.Height - 5.0f);
            float x2_1 = Width * 0.05f + x1_1;
            graphics.DrawLine(pen3, x1_1, num19, x2_1, num19);
            layoutRectangle2.X = x1_1 - 10f;
            layoutRectangle2.Y = num19 + 2f;
            layoutRectangle2.Width = (float)(x2_1 - (double)x1_1 + 20.0f);
            graphics.DrawString("$07", fontLabel, solidBrush2, layoutRectangle2, format);
            float x1_2 = Width * 0.743f;
            float num20 = Height * 0.5f + 10.0f;
            float x2_2 = Width * 0.05f + x1_2;
            graphics.DrawLine(pen3, x1_2, num20, x2_2, num20);
            SizeF sizeF = graphics.MeasureString("$02", fontLabel);
            layoutRectangle2.Width = sizeF.Width;
            layoutRectangle2.X = x1_2 - sizeF.Width;
            layoutRectangle2.Y = num20 - fontLabel.Height * 0.5f;
            graphics.DrawString("$02", fontLabel, solidBrush2, layoutRectangle2, format);
            float x1_3 = Width * 0.895f;
            float num21 = Height * 0.5f - 10.0f;
            float x2_3 = Width * 0.05f + x1_3;
            graphics.DrawLine(pen3, x1_3, num21, x2_3, num21);
            sizeF = graphics.MeasureString("$01", fontLabel);
            layoutRectangle2.Width = sizeF.Width;
            layoutRectangle2.X = x1_3 - sizeF.Width;
            layoutRectangle2.Y = num21 - fontLabel.Height * 0.5f;
            graphics.DrawString("$01", fontLabel, solidBrush2, layoutRectangle2, format);
            float x1_4 = Width * 0.82f;
            float num22 = fontTitle.Height + 5f;
            float x2_4 = Width * 0.05f + x1_4;
            graphics.DrawLine(pen3, x1_4, num22, x2_4, num22);
            sizeF = graphics.MeasureString("$08", fontLabel);
            layoutRectangle2.Width = sizeF.Width;
            layoutRectangle2.X = x1_4 - sizeF.Width;
            layoutRectangle2.Y = num22 - fontLabel.Height * 0.5f;
            graphics.DrawString("$08", fontLabel, solidBrush2, layoutRectangle2, format);
            pen1.Dispose();
            pen2.Dispose();
            pen3.Dispose();
        }

        private void PaintArrowLine(Graphics g, Pen pen, float fX0, float fY0, float fX1, float fY1) {
            g.DrawLine(pen, fX0, fY0, fX1, fY1);
            float x2_1 = fX0 + 5f;
            g.DrawLine(pen, fX0, fY0, x2_1, fY0 - 5f);
            g.DrawLine(pen, fX0, fY0, x2_1, fY0 + 5f);
            float x2_2 = fX1 - 5f;
            g.DrawLine(pen, fX1, fY1, x2_2, fY1 - 5f);
            g.DrawLine(pen, fX1, fY1, x2_2, fY1 + 5f);
        }

        private void O2WaveformControl_Resize(object sender, EventArgs e) {
            Refresh();
        }
    }
}
