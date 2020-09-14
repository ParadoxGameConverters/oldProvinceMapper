using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProvinceMapper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Bitmap bmpSrc;
        private Bitmap bmpDest;

        private SortedList<int, Province> srcChroma;
        private SortedList<int, Province> destChroma;

        private float scaleFactorSource = 1.0f;
        private float scaleFactorDest = 1.0f;

        private Dictionary<string, System.Windows.Forms.ListBox> lbMappingsDict;

        private void Form1_Load(object sender, EventArgs e)
        {
            srcChroma = new SortedList<int, Province>();
            foreach (Province p in Program.sourceDef.provinces)
            {
                srcChroma.Add(p.rgb.ToArgb(), p);
            }

            destChroma = new SortedList<int, Province>();
            foreach (Province p in Program.targetDef.provinces)
            {
                destChroma.Add(p.rgb.ToArgb(), p);
            }

            // force rescale/redraw
            cbZoomSource.SelectedIndex = 0;
            cbZoomDest.SelectedIndex = 0;

            // get the different mapping listings
            lbMappingsDict = new Dictionary<string, System.Windows.Forms.ListBox>();
            foreach (KeyValuePair<string, List<IMapping>> oneMapping in Program.mappings.mappings)
            {
                mappingsTabs.TabPages.Add(oneMapping.Key);
                ListBox newListBox = new ListBox
                {
                    Dock = DockStyle.Fill
                };
                newListBox.SelectedIndexChanged += LbMappings_SelectedIndexChanged;

                newListBox.Items.AddRange(oneMapping.Value.ToArray());
                newListBox.Items.Add(newMappingItem);
                newListBox.Items.Add(newCommentItem);

                mappingsTabs.TabPages[mappingsTabs.TabPages.Count - 1].Controls.Add(newListBox);
                lbMappingsDict.Add(oneMapping.Key, newListBox);
            }
            if (mappingsTabs.TabCount == 0)
            {
                mappingsTabs.TabPages.Add("mappings");
                ListBox newListBox = new ListBox
                {
                    Dock = DockStyle.Fill
                };
                newListBox.SelectedIndexChanged += LbMappings_SelectedIndexChanged;

                newListBox.Items.Add(newMappingItem);
                newListBox.Items.Add(newCommentItem);

                List<IMapping> oneMapping = new List<IMapping>();
                Program.mappings.mappings.Add("mappings", oneMapping);
                mappingsTabs.TabPages[mappingsTabs.TabPages.Count - 1].Controls.Add(newListBox);
                lbMappingsDict.Add("mappings", newListBox);
            }
        }

        private Point srcPt;
        private void PbSource_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X != srcPt.X || e.Y != srcPt.Y)
            {
                srcPt.X = e.X;
                srcPt.Y = e.Y;
                Color c = bmpSrc.GetPixel(srcPt.X, srcPt.Y);
                Province p = null;
                if (srcChroma.TryGetValue(c.ToArgb(), out p))
                {
                    toolTip1.Show(p.ToString(), pbSource, new Point(srcPt.X, srcPt.Y - 20));
                    StatusLabel.Text = p.ToString();
                }
            }
        }

        private Point destPt;
        private void PbTarget_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X != destPt.X || e.Y != destPt.Y)
            {
                destPt.X = e.X;
                destPt.Y = e.Y;
                Color c = bmpDest.GetPixel(destPt.X, destPt.Y);
                Province p = null;
                if (destChroma.TryGetValue(c.ToArgb(), out p))
                {
                    toolTip1.Show(p.ToString(), pbTarget, new Point(destPt.X, destPt.Y - 20));
                    StatusLabel.Text = p.ToString();
                }
            }
        }

        private List<Province> oldSrcSelection = new List<Province>();
        private List<Province> srcSelection = new List<Province>();
        private void PbSource_MouseUp(object sender, MouseEventArgs e)
        {
            Color c = bmpSrc.GetPixel(srcPt.X, srcPt.Y);
            Province p = null;
            if (srcChroma.TryGetValue(c.ToArgb(), out p))
            {
                ProvinceMapping m = lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedItem as ProvinceMapping;
                if ((p.mappings.ContainsKey(mappingsTabs.SelectedTab.Text)) && (p.mappings[mappingsTabs.SelectedTab.Text] != m))
                {
                    // the province is mapped, but not to the current mapping;
                    // switch to this province's mapping
                    skipSelPBRedraw = true;
                    lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedItem = p.mappings[mappingsTabs.SelectedTab.Text];
                    skipSelPBRedraw = false;
                    CreateSelPBs(false);
                }
                else
                {
                    // the province is mapped to the current mapping, or is unmapped
                    // alter this mapping for the province
                    if (m != null)
                    {
                        if (m.srcProvs.Contains(p))
                        {
                            m.srcProvs.Remove(p);
                            p.mappings.Remove(mappingsTabs.SelectedTab.Text);
                        }
                        else
                        {
                            m.srcProvs.Add(p);
                            p.mappings.Add(mappingsTabs.SelectedTab.Text, m);
                        }
                        skipSelPBRedraw = true;
                        lbMappingsDict[mappingsTabs.SelectedTab.Text].Items[lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedIndex] = m;
                        skipSelPBRedraw = false;
                        CreateSelPBs(false);
                    }
                }
            }
        }

        private List<Province> oldDestSelection = new List<Province>();
        private List<Province> destSelection = new List<Province>();
        private void PbTarget_MouseUp(object sender, MouseEventArgs e)
        {
            Color c = bmpDest.GetPixel(destPt.X, destPt.Y);
            Province p = null;
            if (destChroma.TryGetValue(c.ToArgb(), out p))
            {
                ProvinceMapping m = lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedItem as ProvinceMapping;
                if ((p.mappings.ContainsKey(mappingsTabs.SelectedTab.Text)) && (p.mappings[mappingsTabs.SelectedTab.Text] != m))
                {
                    // the province is mapped, but not to the current mapping;
                    // switch to this province's mapping
                    skipSelPBRedraw = true;
                    lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedItem = p.mappings[mappingsTabs.SelectedTab.Text];
                    skipSelPBRedraw = false;
                    CreateSelPBs(false);
                }
                else
                {
                    // the province is mapped to the current mapping, or is unmapped
                    // alter this mapping for the province
                    if (m != null)
                    {
                        if (m.destProvs.Contains(p))
                        {
                            m.destProvs.Remove(p);
                            p.mappings.Remove(mappingsTabs.SelectedTab.Text);
                        }
                        else
                        {
                            m.destProvs.Add(p);
                            p.mappings.Add(mappingsTabs.SelectedTab.Text, m);
                        }
                        skipSelPBRedraw = true;
                        lbMappingsDict[mappingsTabs.SelectedTab.Text].Items[lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedIndex] = m;
                        skipSelPBRedraw = false;
                        CreateSelPBs(false);
                    }
                }
            }
        }

        private void PbSource_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.RemoveAll();
            StatusLabel.Text = String.Empty;
        }

        private void PbTarget_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.RemoveAll();
            StatusLabel.Text = String.Empty;
        }

        private void CreateSelPBs(bool force)
        {
            CreateSelPBs(force, srcSelection, oldSrcSelection, srcChroma.Values, pbSource, scaleFactorSource);
            CreateSelPBs(force, destSelection, oldDestSelection, destChroma.Values, pbTarget, scaleFactorDest);
        }

        private void CreateSelPBs(bool force, List<Province> newSelection, List<Province> oldSelection, IList<Province> provinces, PictureBox pb, float scaleFactor)
        {
            if (force || !newSelection.SequenceEqual(oldSelection))
            {
                Rectangle invalidRect = Rectangle.Empty;
                if (force)
                {
                    invalidRect = new Rectangle(0, 0, pb.Image.Width, pb.Image.Height);
                }
                else
                {
                    if (newSelection.Count > 0)
                    {
                        invalidRect = Program.ScaleRect(newSelection[0].Rect, scaleFactor);
                    }
                    else if (oldSelection.Count > 0)
                    {
                        invalidRect = Program.ScaleRect(oldSelection[0].Rect, scaleFactor);
                    }
                    foreach (Province p in newSelection)
                    {
                        invalidRect = Rectangle.Union(invalidRect, Program.ScaleRect(p.Rect, scaleFactor));
                    }
                    foreach (Province p in oldSelection)
                    {
                        invalidRect = Rectangle.Union(invalidRect, Program.ScaleRect(p.Rect, scaleFactor));
                    }
                }

                Graphics g = Graphics.FromImage(pb.Image);
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                g.FillRectangle(Brushes.Transparent, invalidRect);
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

                // disable interpolation and smoothing to preserve chroma
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                if (MaskMapped)
                {
                    foreach (Province p in provinces)
                    {
                        Rectangle scaledRect = Program.ScaleRect(p.Rect, scaleFactor);
                        if ((p.mappings.ContainsKey(mappingsTabs.SelectedTab.Text)) && (Rectangle.Intersect(scaledRect, invalidRect) != Rectangle.Empty))
                        {
                            g.DrawImage(p.BlackMask, scaledRect);
                        }
                    }
                }

                foreach (Province p in newSelection)
                {
                    Rectangle scaledRect = Program.ScaleRect(p.Rect, scaleFactor);
                    if (Rectangle.Intersect(scaledRect, invalidRect) != Rectangle.Empty)
                    {
                        g.DrawImage(p.SelectionMask, scaledRect);
                    }
                }
                pb.Invalidate(invalidRect);

                oldSelection.Clear();
                oldSelection.AddRange(newSelection);
            }
        }

        private readonly string newMappingItem = "-- <Create New Mapping> --";
        private readonly string newCommentItem = "-- <Create New Comment> --";
        private bool skipSelPBRedraw = false;
        private void LbMappings_SelectedIndexChanged(object sender, EventArgs e)
        {
            srcSelection.Clear();
            destSelection.Clear();

            if (lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedItem == (Object)newMappingItem) // reference test!
            {
                CreateNewMapping(false, lbMappingsDict[mappingsTabs.SelectedTab.Text].Items.Count - 2);
            }
            else if (lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedItem == (Object)newCommentItem) // reference test!
            {
                CreateNewMapping(true, lbMappingsDict[mappingsTabs.SelectedTab.Text].Items.Count - 2);
            }
            else
            {
                ProvinceMapping m = lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedItem as ProvinceMapping;
                if (m != null)
                {
                    srcSelection.AddRange(m.srcProvs);
                    destSelection.AddRange(m.destProvs);
                }
            }

            if (!skipSelPBRedraw)
            {
                CreateSelPBs(false);
            }
        }

        private void CreateNewMapping(bool comment, int location)
        {
            IMapping m;
            if (comment)
            {
                CommentMapping cm = new CommentMapping();
                CommentForm cf = new CommentForm();
                cf.SetComment(cm);
                cf.ShowDialog();
                m = cm;
            }
            else // province map
            {
                m = new ProvinceMapping();
            }
            Program.mappings.mappings[mappingsTabs.SelectedTab.Text].Insert(location, m);
            lbMappingsDict[mappingsTabs.SelectedTab.Text].Items.Insert(location, m);
            lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedItem = m;
        }

        private void MoveToSelected()
        {
            if (srcSelection.Count > 0)
            {
                Rectangle srcFit = Program.ScaleRect(srcSelection[0].Rect, scaleFactorSource);
                foreach (Province p in srcSelection)
                {
                    srcFit = Rectangle.Union(srcFit, Program.ScaleRect(p.Rect, scaleFactorSource));
                }
                Point fitCenter = new Point(srcFit.X + srcFit.Width / 2, srcFit.Y + srcFit.Height / 2);
                Point panelCenter = new Point(HorizontalSplit.Panel1.Width / 2, HorizontalSplit.Panel1.Height / 2);
                Point offset = new Point(fitCenter.X - panelCenter.X, fitCenter.Y - panelCenter.Y);
                HorizontalSplit.Panel1.AutoScrollPosition = offset;
            }

            if (destSelection.Count > 0)
            {
                Rectangle destFit = Program.ScaleRect(destSelection[0].Rect, scaleFactorDest);
                foreach (Province p in destSelection)
                {
                    destFit = Rectangle.Union(destFit, Program.ScaleRect(p.Rect, scaleFactorDest));
                }
                Point fitCenter = new Point(destFit.X + destFit.Width / 2, destFit.Y + destFit.Height / 2);
                Point panelCenter = new Point(HorizontalSplit.Panel1.Width / 2, HorizontalSplit.Panel1.Height / 2);
                Point offset = new Point(fitCenter.X - panelCenter.X, fitCenter.Y - panelCenter.Y);
                HorizontalSplit.Panel2.AutoScrollPosition = offset;
            }
        }

        private void TbFitSelection_Click(object sender, EventArgs e)
        {
            MoveToSelected();
        }

        private void TbSave_Click(object sender, EventArgs e)
        {
            Program.mappings.Write();
        }

        private void TbMoveUp_Click(object sender, EventArgs e)
        {
            IMapping m = lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedItem as IMapping;
            int idx = Program.mappings.mappings[mappingsTabs.SelectedTab.Text].IndexOf(m);
            if (idx > 0)
            {
                Program.mappings.mappings[mappingsTabs.SelectedTab.Text][idx] = Program.mappings.mappings[mappingsTabs.SelectedTab.Text][idx - 1];
                Program.mappings.mappings[mappingsTabs.SelectedTab.Text][idx - 1] = m;
                lbMappingsDict[mappingsTabs.SelectedTab.Text].Items[idx] = Program.mappings.mappings[mappingsTabs.SelectedTab.Text][idx];
                lbMappingsDict[mappingsTabs.SelectedTab.Text].Items[idx - 1] = Program.mappings.mappings[mappingsTabs.SelectedTab.Text][idx - 1];
                lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedItem = m;
            }
        }

        private void TbMoveDown_Click(object sender, EventArgs e)
        {
            IMapping m = lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedItem as IMapping;
            int idx = Program.mappings.mappings[mappingsTabs.SelectedTab.Text].IndexOf(m);
            if (idx < Program.mappings.mappings[mappingsTabs.SelectedTab.Text].Count - 1)
            {
                Program.mappings.mappings[mappingsTabs.SelectedTab.Text][idx] = Program.mappings.mappings[mappingsTabs.SelectedTab.Text][idx + 1];
                Program.mappings.mappings[mappingsTabs.SelectedTab.Text][idx + 1] = m;
                lbMappingsDict[mappingsTabs.SelectedTab.Text].Items[idx] = Program.mappings.mappings[mappingsTabs.SelectedTab.Text][idx];
                lbMappingsDict[mappingsTabs.SelectedTab.Text].Items[idx + 1] = Program.mappings.mappings[mappingsTabs.SelectedTab.Text][idx + 1];
                lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedItem = m;
            }
        }

        private bool MaskMapped = false;
        private void ShowSelectedMapping()
        {
            tbSelection.Checked = true;
            tbUnmapped.Checked = false;
            MaskMapped = false;
            CreateSelPBs(true);
        }

        private void ShowUnmapped()
        {
            tbUnmapped.Checked = true;
            tbSelection.Checked = false;
            MaskMapped = true;
            CreateSelPBs(true);
        }

        private void TbUnmapped_Click(object sender, EventArgs e)
        {
            ShowUnmapped();
        }

        private void TbSelection_Click(object sender, EventArgs e)
        {
            ShowSelectedMapping();
        }

        private void CbZoomSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            float oldScaleFactor = scaleFactorSource;
            if (cbZoomSource.SelectedItem != null)
            {
                scaleFactorSource = float.Parse(cbZoomSource.SelectedItem.ToString().TrimEnd('x'));
            }
            if (pbSource.BackgroundImage != null)
            {
                pbSource.BackgroundImage.Dispose();
            }
            if (pbSource.Image != null)
            {
                pbSource.Image.Dispose();
            }

            Point sourceScroll = HorizontalSplit.Panel1.AutoScrollPosition;
            pbSource.BackgroundImage = bmpSrc = Program.CleanResizeBitmap(Program.sourceMap.map,
                 (int)(Program.sourceMap.map.Width * scaleFactorSource), (int)(Program.sourceMap.map.Height * scaleFactorSource));
            pbSource.Size = bmpSrc.Size;
            pbSource.Image = new Bitmap(bmpSrc.Width, bmpSrc.Height);
            Graphics g = Graphics.FromImage(pbSource.Image);
            g.FillRectangle(Brushes.Transparent, new Rectangle(new Point(0, 0), bmpSrc.Size));
            g.Flush();
            sourceScroll.X = (int)((-sourceScroll.X * scaleFactorSource / oldScaleFactor) + (HorizontalSplit.Panel1.Width * scaleFactorSource / (2 * oldScaleFactor)) - (HorizontalSplit.Panel1.Width / 2));
            sourceScroll.Y = (int)((-sourceScroll.Y * scaleFactorSource / oldScaleFactor) + (HorizontalSplit.Panel1.Height * scaleFactorSource / (2 * oldScaleFactor)) - (HorizontalSplit.Panel1.Height / 2));
            HorizontalSplit.Panel1.AutoScrollPosition = sourceScroll;

            CreateSelPBs(true, srcSelection, oldSrcSelection, srcChroma.Values, pbSource, scaleFactorSource);
        }

        private void CbZoomDest_SelectedIndexChanged(object sender, EventArgs e)
        {
            float oldScaleFactor = scaleFactorDest;
            if (cbZoomDest.SelectedItem != null)
            {
                scaleFactorDest = float.Parse(cbZoomDest.SelectedItem.ToString().TrimEnd('x'));
            }

            if (pbTarget.BackgroundImage != null)
            {
                pbTarget.BackgroundImage.Dispose();
            }
            if (pbTarget.Image != null)
            {
                pbTarget.Image.Dispose();
            }

            Point destScroll = HorizontalSplit.Panel2.AutoScrollPosition;
            pbTarget.BackgroundImage = bmpDest = Program.CleanResizeBitmap(Program.targetMap.map,
                 (int)(Program.targetMap.map.Width * scaleFactorDest), (int)(Program.targetMap.map.Height * scaleFactorDest));
            pbTarget.Size = bmpDest.Size;
            pbTarget.Image = new Bitmap(bmpDest.Width, bmpDest.Height);
            Graphics g = Graphics.FromImage(pbTarget.Image);
            g.FillRectangle(Brushes.Transparent, new Rectangle(new Point(0, 0), bmpDest.Size));
            g.Flush();
            destScroll.X = (int)((-destScroll.X * scaleFactorDest / oldScaleFactor) + (HorizontalSplit.Panel2.Width * scaleFactorDest / (2 * oldScaleFactor)) - (HorizontalSplit.Panel2.Width / 2));
            destScroll.Y = (int)((-destScroll.Y * scaleFactorDest / oldScaleFactor) + (HorizontalSplit.Panel2.Height * scaleFactorDest / (2 * oldScaleFactor)) - (HorizontalSplit.Panel2.Height / 2));
            HorizontalSplit.Panel2.AutoScrollPosition = destScroll;

            CreateSelPBs(true, destSelection, oldDestSelection, destChroma.Values, pbTarget, scaleFactorDest);
        }


        private void EditCommentText()
        {
            CommentMapping cm = lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedItem as CommentMapping;
            if (cm != null)
            {
                CommentForm cf = new CommentForm();
                cf.SetComment(cm);
                cf.ShowDialog();
            }
            lbMappingsDict[mappingsTabs.SelectedTab.Text].Items[lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedIndex] = cm;
        }

        private void CreateNewComment()
        {
            if (lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedIndex >= 0)
            {
                CreateNewMapping(true, lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedIndex);
            }
            else
            {
                CreateNewMapping(true, lbMappingsDict[mappingsTabs.SelectedTab.Text].Items.Count - 2);
            }
        }

        private void CreateNewMapping()
        {
            if (lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedIndex >= 0)
            {
                CreateNewMapping(false, lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedIndex);
            }
            else
            {
                CreateNewMapping(false, lbMappingsDict[mappingsTabs.SelectedTab.Text].Items.Count - 2);
            }
        }

        private void Delete()
        {
            if (lbMappingsDict[mappingsTabs.SelectedTab.Text].Items.Count >= 0)
            {
                Program.mappings.mappings[mappingsTabs.SelectedTab.Text].Remove(lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedItem as IMapping);
                lbMappingsDict[mappingsTabs.SelectedTab.Text].Items.Remove(lbMappingsDict[mappingsTabs.SelectedTab.Text].SelectedItem);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                EditCommentText();
            }
            else if (e.KeyCode == Keys.F3)
            {
                CreateNewComment();
            }
            else if (e.KeyCode == Keys.F4)
            {
                CreateNewMapping();
            }
            else if ((e.KeyCode == Keys.F5) || (e.KeyCode == Keys.Delete))
            {
                Delete();
            }
            else if ((e.KeyCode == Keys.Oemplus) || (e.KeyCode == Keys.Add))
            {
                TbMoveUp_Click(sender, e);
            }
            else if ((e.KeyCode == Keys.OemMinus) || (e.KeyCode == Keys.Subtract))
            {
                TbMoveDown_Click(sender, e);
            }
        }
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.mappings.Write();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CreateCommentsF2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditCommentText();
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            CreateNewComment();
        }

        private void CreateMappingF4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewMapping();
        }

        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void MoveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TbMoveUp_Click(sender, e);
        }

        private void MoveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TbMoveDown_Click(sender, e);
        }

        private void SelectedMappingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSelectedMapping();
        }

        private void UnmappedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowUnmapped();
        }

        private void MoveToSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveToSelected();
        }
    }
}