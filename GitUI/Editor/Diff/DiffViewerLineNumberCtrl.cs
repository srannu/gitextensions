﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GitCommands;
using ICSharpCode.TextEditor;

namespace GitUI.Editor.Diff
{
    class DiffViewerLineNumberCtrl : AbstractMargin
    {
        const int TextHorizontalMargin = 4;

        private static readonly Pen NullPen;

        private int _maxValueOfLineNum = 0;

        static DiffViewerLineNumberCtrl()
        {
            NullPen = new Pen(Brushes.Transparent);
        }

        public DiffViewerLineNumberCtrl(TextArea textArea) : base(textArea)
        {
            DiffLines = new Dictionary<int, DiffLineNum>();
        }

        public override Size Size
        {
            get
            {
                if (!DiffLines.Any())
                {
                    return new Size(0, 0);
                }

                var size = Graphics.FromHwnd(textArea.Handle).MeasureString(_maxValueOfLineNum.ToString(), textArea.Font);

                return new Size((int)size.Width * 2 + TextHorizontalMargin, 0);
            }
        }

        public override void Paint(Graphics g, Rectangle rect)
        {
            if (!DiffLines.Any())
            {
                return;
            }

            var totalWidth = Size.Width;
            var leftWidth = (int)(totalWidth/2.0);
            var rightWidth = rect.Width - leftWidth;

            var fontHeight = textArea.TextView.FontHeight;
            var lineNumberPainterColor = textArea.Document.HighlightingStrategy.GetColorFor("LineNumbers");
            Brush fillBrush = textArea.Enabled ? BrushRegistry.GetBrush(lineNumberPainterColor.BackgroundColor) : SystemBrushes.InactiveBorder;
            Brush drawBrush = BrushRegistry.GetBrush(lineNumberPainterColor.Color);

            for (int y = 0; y < (DrawingPosition.Height + textArea.TextView.VisibleLineDrawingRemainder) / fontHeight + 1; ++y)
            {
                int ypos = drawingPosition.Y + fontHeight * y - textArea.TextView.VisibleLineDrawingRemainder;
                Rectangle backgroundRectangle = new Rectangle(drawingPosition.X, ypos, drawingPosition.Width, fontHeight);
                if (rect.IntersectsWith(backgroundRectangle))
                {
                    g.FillRectangle(fillBrush, backgroundRectangle);
                    int curLine = textArea.Document.GetFirstLogicalLine(textArea.Document.GetVisibleLine(textArea.TextView.FirstVisibleLine) + y);

                    if (curLine < textArea.Document.TotalNumberOfLines)
                    {
                        if (DiffLines.ContainsKey(curLine + 1))
                        {
                            var diffLine = DiffLines[curLine + 1];
                            if (diffLine.Style != DiffLineNum.DiffLineStyle.Context)
                            {
                                var brush = default(Brush);
                                switch (diffLine.Style)
                                {
                                    case DiffLineNum.DiffLineStyle.Plus:
                                        brush = new SolidBrush(AppSettings.DiffAddedColor);
                                        break;
                                    case DiffLineNum.DiffLineStyle.Minus:
                                        brush = new SolidBrush(AppSettings.DiffRemovedColor);
                                        break;
                                    case DiffLineNum.DiffLineStyle.Header:
                                        brush = new SolidBrush(AppSettings.DiffSectionColor);
                                        break;
                                }

                                g.FillRectangle(brush, new Rectangle(0, backgroundRectangle.Top, leftWidth, backgroundRectangle.Height));

                                g.FillRectangle(brush, new Rectangle(leftWidth, backgroundRectangle.Top, rightWidth, backgroundRectangle.Height));
                            }
                            if (diffLine.LeftLineNum != DiffLineNum.NotApplicableLineNum)
                            {
                                g.DrawString(diffLine.LeftLineNum.ToString(),
                                     lineNumberPainterColor.GetFont(TextEditorProperties.FontContainer),
                                     drawBrush,
                                     new Point(TextHorizontalMargin, backgroundRectangle.Top));
                            }

                            if (diffLine.RightLineNum != DiffLineNum.NotApplicableLineNum)
                            {
                                g.DrawString(diffLine.RightLineNum.ToString(),
                                     lineNumberPainterColor.GetFont(TextEditorProperties.FontContainer),
                                     drawBrush,
                                     new Point(TextHorizontalMargin + totalWidth / 2, backgroundRectangle.Top));
                            }
                        }
                    }
                }
            }
        }

        private Dictionary<int, DiffLineNum> DiffLines { get; set; }

        public void AddDiffLineNum(DiffLineNum diffLineNum)
        {
            DiffLines[diffLineNum.LineNumInDiff] = diffLineNum;
            _maxValueOfLineNum = Math.Max(diffLineNum.LeftLineNum, diffLineNum.RightLineNum);
        }

        public void Clear()
        {
            DiffLines.Clear();
        }

        public string GetLineDesc(int lineNumInDiffFile)
        {
            DiffLineNum line;
            if (!DiffLines.TryGetValue(lineNumInDiffFile, out line)) return null;

            if (line.LeftLineNum != DiffLineNum.NotApplicableLineNum)
            {
                return "L" + line.LeftLineNum;
            }
            if (line.RightLineNum != DiffLineNum.NotApplicableLineNum)
            {
                return "R" + line.RightLineNum;
            }
            return null;
        }
    }
}