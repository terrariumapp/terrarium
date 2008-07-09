//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------


using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Data;
using System.Text;
using System.IO;
using System.Web.Util;
using System.Web;
using System.Web.UI;
using System.Web.Caching;

namespace Terrarium.Server {
    /*
        Class:      ChartNet
        Purpose:    This class encapsulates all of the code required
        to render charts for Terrarium population data.  The class is
        capable of making Point series or line graphs, as well as Bar
        series, or bar graphs.
        
        This class is also capable of cleaning graphs from the resource
        cache once they are no longer in use.
    */
    public class ChartNet {
        Color[] colorTable = new Color[] {
            Color.Red,
            Color.Green,
            Color.Yellow,
            Color.Gold,
            Color.Cyan,
            Color.Orange,
            Color.Brown,
            Color.Purple,
            Color.Blue,
            Color.DarkGreen
        };
        ArrayList series = new ArrayList();
        string chartPath;
        string chartUrl;
        string title;

        /*
            Method:     AddPointSeries
            Purpose:    The AddPointSeries series of functions makes
            use of a series name and a set of {x,y} coordinate pairs
            in order to generate line graphs.
        */
        public void AddPointSeries(string seriesName, ArrayList data) {
            AddPointSeries(new PointSeries(seriesName, data));
        }
        public void AddPointSeries(string seriesName, Point[] data) {
            AddPointSeries(new PointSeries(seriesName, data));
        }
        public void AddPointSeries(PointSeries p) {
            series.Add(p);
        }

        /*
            Method:     AddBarSeries
            Purpose:    The AddBarSeries series of functions makes use
            of a series name and a set of {x,y} coordinate pairs in order
            to generate a bar graph.
        */
        public void AddBarSeries(string seriesName, ArrayList data) {
            AddBarSeries(new BarSeries(seriesName, data));
        }
        public void AddBarSeries(string seriesName, Point[] data) {
            AddBarSeries(new BarSeries(seriesName, data));
        }
        public void AddBarSeries(BarSeries p) {
            series.Add(p);
        }

        /*
            Class:      PointSeries
            Purpose:    Internal helper class for formatting point series
            data into different resource formats {array format, tab delimted,
            etc...}.
        */
        public class PointSeries {
            float[] x;
            float[] y;
            string seriesName;

            public PointSeries(string seriesName, ArrayList data) {
                this.seriesName = seriesName;
                this.x = new float[data.Count];
                this.y = new float[data.Count];

                if ( data.Count > 0 ) {
                    if ( data[0] is Point ) {
                        for(int i = 0; i < data.Count; i++) {
                            this.x[i] = ((Point) data[i]).X;
                            this.y[i] = ((Point) data[i]).Y;
                        }
                    }
                    else {
                        for(int i = 0; i < data.Count; i++) {
                            this.x[i] = ((PointF) data[i]).X;
                            this.y[i] = ((PointF) data[i]).Y;
                        }
                    }
                }
            }

            public PointSeries(string seriesName, Point[] data) {
                this.seriesName = seriesName;
                this.x = new float[data.Length];
                this.y = new float[data.Length];

                for(int i = 0; i < data.Length; i++) {
                    this.x[i] = data[i].X;
                    this.y[i] = data[i].Y;
                }
            }

            public PointSeries(string seriesName, PointF[] data) {
                this.seriesName = seriesName;
                this.x = new float[data.Length];
                this.y = new float[data.Length];

                for(int i = 0; i < data.Length; i++) {
                    this.x[i] = data[i].X;
                    this.y[i] = data[i].Y;
                }
            }
            
            public PointF[] Normalize(float xMax, float xMin, float xZone, float xOffset, float yMax, float yMin, float yZone, float yOffset) {
                PointF[] normalized = new PointF[x.Length];
                
                for(int i = 0; i < x.Length; i++) {
                    normalized[i] = new PointF(((x[i]-xMin)/(xMax-xMin)) * xZone + xOffset, (yZone - (((y[i]-yMin)/(yMax-yMin)) * yZone)) + yOffset);
                }
                
                return normalized;
            }

            public float[] XValues {
                get {
                    return x;
                }
            }

            public float[] YValues {
                get {
                    return y;
                }
            }

            public string XTabValues {
                get {
                    string[] xstr = new string[x.Length];

                    for(int i = 0; i < x.Length; i++) {
                        xstr[i] = x[i].ToString();
                    }

                    return String.Join("\t", xstr);
                }
            }

            public string YTabValues {
                get {
                    string[] ystr = new string[y.Length];

                    for(int i = 0; i < y.Length; i++) {
                        ystr[i] = y[i].ToString();
                    }

                    return String.Join("\t", ystr);
                }
            }

            public string SeriesName {
                get {
                    return seriesName;
                }
            }
        }

        /*
            Class:      BarSeries
            Purpose:    Internal helper class for formatting bar series
            data into different resource formats {array format, tab delimted,
            etc...}.
        */
        public class BarSeries {
            float[] low;
            float[] high;
            string seriesName;

            public BarSeries(string seriesName, Point[] data) {
                this.seriesName = seriesName;
                this.low = new float[data.Length];
                this.high = new float[data.Length];

                for(int i = 0; i < data.Length; i++) {
                    this.low[i] = data[i].X;
                    this.high[i] = data[i].Y;
                }
            }

            public BarSeries(string seriesName, ArrayList data) {
                this.seriesName = seriesName;
                this.low = new float[data.Count];
                this.high = new float[data.Count];

                if ( data.Count > 0 ) {
                    if ( data[0] is Point ) {
                        for(int i = 0; i < data.Count; i++) {
                            this.low[i] = ((Point) data[i]).X;
                            this.high[i] = ((Point) data[i]).Y;
                        }
                    } else {
                        for(int i = 0; i < data.Count; i++) {
                            this.low[i] = ((PointF) data[i]).X;
                            this.high[i] = ((PointF) data[i]).Y;
                        }
                    }
                }
            }

            public float[] XValues {
                get {
                    return low;
                }
            }

            public float[] YValues {
                get {
                    return high;
                }
            }

            public string XTabValues {
                get {
                    string[] xstr = new string[low.Length];

                    for(int i = 0; i < low.Length; i++) {
                        xstr[i] = low[i].ToString();
                    }

                    return String.Join("\t", xstr);
                }
            }

            public string YTabValues {
                get {
                    string[] ystr = new string[high.Length];

                    for(int i = 0; i < high.Length; i++) {
                        ystr[i] = high[i].ToString();
                    }

                    return String.Join("\t", ystr);
                }
            }

            public string SeriesName {
                get {
                    return seriesName;
                }
            }
        }

        /*
            Property:   ChartUrl
            Purpose:    Returns the fully qualified url required to access
            the gif chart from the web.  The actual location of this url
            is defined in the application's web configuration file.
        */
        public string ChartUrl {
            get {
                if ( chartUrl == null ) {
                    chartUrl = Path.Combine(LocalSettings.ChartUrl, Path.GetFileName(ChartPath));
                }

                return chartUrl;
            }
        }

        /*
            Property:   ChartPath
            Purpose:    Returns the local disk path for the generated gif
            chart.
            The actual location of this file path is defined in the
            application's web configuration file.
        */
        public string ChartPath {
            get {
                if ( chartPath == null ) {
                    chartPath = Path.Combine(LocalSettings.ChartPath, Guid.NewGuid().ToString() + ".gif");
                }

                return chartPath;
            }
        }

        /*
            Property:   Title
            Purpose:    Allows the user to get or set the title for the
            graph before it is generated.
        */
        public string Title {
            get {
                return title;
            }
            set {
                title = value;
            }
        }

        /*
            Method:     BuildChart
            Purpose:    The BuildChart function is responsible for combining
            together the user input point data and title for submission to the
            chart generator.
            
            Once the chart has been generated it is also marked for removal
            in the web application's cache so that it can be deleted once it
            is no longer in use.
        */
        private const int bitmapWidth = 600;
        private const int bitmapHeight = 400;
        
        private const int clearWidth = bitmapWidth - (marginLeft + marginRight);
        private const int clearHeight = bitmapHeight - (marginTop + marginBottom);
        
        private const int marginLeft = 60;
        private const int marginRight = 10;
        private const int marginTop = 10;
        private const int marginBottom = 30;

        private const int captionWidth = clearWidth;                        // 500
        private const int captionHeight = (int) (clearHeight * .1);         // 60

        private const int graphWidth = (int) (clearWidth * .8);             // 350
        private const int graphHeight = (int) (clearHeight * .875);           // 210
        
        private const int legendWidth = (int) (clearWidth * .2);            // 100
        private const int legendHeight = (int) (clearHeight * .875);          // 210
        
        private const int horzLines = 8;
        private const int vertLines = 5;

        private Rectangle captionBox = new Rectangle(marginLeft, marginTop, captionWidth, captionHeight);
        private Rectangle graphBox = new Rectangle(marginLeft, marginTop + ((int) (clearHeight * .125)), graphWidth, graphHeight);

        private Rectangle legendBox = new Rectangle(marginLeft + ((int) (clearWidth * .8)), marginTop + ((int) (clearHeight * .125)), legendWidth, legendHeight);
        private Font captionFont = new Font(new FontFamily(GenericFontFamilies.SansSerif), 14, FontStyle.Bold);
        private Font graphFont = new Font(new FontFamily(GenericFontFamilies.SansSerif), 8);
        private Font legendFont = new Font(new FontFamily(GenericFontFamilies.SansSerif), 8);
        
        public void BuildChart() {
            Bitmap netChart = new Bitmap(600, 400);
            Graphics gfxChart = Graphics.FromImage((Image) netChart);
            gfxChart.SmoothingMode = SmoothingMode.HighQuality;
            gfxChart.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gfxChart.CompositingQuality = CompositingQuality.HighQuality;
            gfxChart.CompositingMode = CompositingMode.SourceCopy;
            
            gfxChart.Clear(Color.White);

            // Render Legend
            StringFormat captionFormat = new StringFormat();
            captionFormat.LineAlignment = StringAlignment.Center;

            gfxChart.DrawRectangle(Pens.Black, captionBox);
            gfxChart.DrawString(Title, captionFont, Brushes.Black, captionBox, captionFormat);
            
            // Render Graph
            gfxChart.FillRectangle(new SolidBrush(Color.FromArgb(0x00, 0x99, 0xCC)), graphBox);
            gfxChart.DrawRectangle(Pens.Black, graphBox);

            // Obtain Extrema
            if ( series.Count > 1 ) {
                int xMin = Int32.MaxValue;
                int xMax = Int32.MinValue;
                int yMin = Int32.MaxValue;
                int yMax = Int32.MinValue;

                foreach(PointSeries ps in series) {
                    for(int i = 0; i < ps.XValues.Length; i++) {
                        if ( xMin > ps.XValues[i] ) {
                            xMin = (int) Math.Round(ps.XValues[i]);
                        }
                        if ( xMax < ps.XValues[i] ) {
                            xMax = (int) Math.Round(ps.XValues[i]);
                        }
                    }

                    for(int i = 0; i < ps.YValues.Length; i++) {
                        if ( yMin > ps.YValues[i] ) {
                            yMin = (int) Math.Round(ps.YValues[i]);
                        }
                        if ( yMax < ps.YValues[i] ) {
                            yMax = (int) Math.Round(ps.YValues[i]);
                        }
                    }
                }
                
                if ( xMin != Int32.MaxValue && xMax != Int32.MinValue && yMin != Int32.MaxValue && yMax != Int32.MinValue ) {
                    // Graph Lines
                    for(int i = 0; i < horzLines; i++) {
                        gfxChart.DrawLine(
                            Pens.Black, graphBox.Left + (i * (graphBox.Width/horzLines)), graphBox.Top,
                            graphBox.Left + (i * (graphBox.Width/horzLines)), graphBox.Bottom);
                        gfxChart.DrawString(
                            (xMin + (((xMax - xMin) / horzLines) * i)).ToString(), graphFont, Brushes.Black,
                            new Point(graphBox.Left + (i * (graphBox.Width/horzLines)), graphBox.Bottom + 10), captionFormat);
                    }
                    for(int i = 0; i < vertLines; i++) {
                        StringFormat graphFormat = new StringFormat();
                        graphFormat.LineAlignment = StringAlignment.Far;
                        
                        gfxChart.DrawLine(
                            Pens.Black, graphBox.Left, graphBox.Bottom - (i * (graphBox.Height/vertLines)),
                            graphBox.Right, graphBox.Bottom - (i * (graphBox.Height/vertLines)));
                        gfxChart.DrawString(
                            (yMin + (((yMax - yMin) / vertLines) * i)).ToString(),
                            graphFont, Brushes.Black,
                            new Rectangle(0, (graphBox.Bottom - 6) - (i * (graphBox.Height/vertLines)), marginLeft - 5, 12), graphFormat);
                    }
                }
                
                for(int i = 0; i < series.Count; i++) {
                    gfxChart.DrawLines(new Pen(colorTable[i]), ((PointSeries) series[i]).Normalize(xMax, xMin, graphBox.Width, graphBox.Left, yMax, yMin, graphBox.Height, graphBox.Top));
                }
            }
            
            // Render Legend
            gfxChart.DrawRectangle(Pens.Black, legendBox);
            for(int i = 0; i < series.Count; i++) {
                Rectangle colorBox = new Rectangle(legendBox.Left + 5, legendBox.Top + 15 + (i * 15), 10, 10);
                gfxChart.FillRectangle(new SolidBrush(colorTable[i]), colorBox);
                
                Rectangle textBox = new Rectangle(colorBox.Right + 5, colorBox.Top, legendBox.Right - colorBox.Right - 10, 10);
                gfxChart.DrawString(((PointSeries) series[i]).SeriesName, legendFont, new SolidBrush(colorTable[i]), textBox, captionFormat);
            }
            gfxChart.Dispose();
            netChart.Save(ChartPath, ImageFormat.Gif);

            HttpContext.Current.Cache.Insert(ChartPath, ChartPath, null, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration,
                CacheItemPriority.Normal, new CacheItemRemovedCallback(ItemRemovedCallback));
        }

        /*
            Method:     ItemRemovedCallback
            Purpose:    Callback function responsible for removing old graph
            files from the web server.
        */
        private static void ItemRemovedCallback(string key, object value, CacheItemRemovedReason reason) {
            File.Delete((string) value);
        }
    }
}
