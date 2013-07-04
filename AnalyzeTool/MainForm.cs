﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Xml.XPath;
using WurmUtils;

namespace AnalyzeTool
{
    public partial class MainForm : Form
    {

        private String mapFileName;
        private String autoSaveFileName;
        private AnalyzeMap map;
        private Font font;
        private List<Player> players = new List<Player>();
        private MapTool activeTool = null;
        private Boolean renderAnalyzerOverlay = true;
        private int textureWidth = 32;
        private int textureHeight = 32;

        private abstract class MapTool
        {
            public abstract bool UseTool(AnalyzeMap map, Tile tile);
        }

        private class TileTypeTool : MapTool
        {
            private TileType tileType;
            public TileTypeTool(TileType tileType)
            {
                this.tileType = tileType;
            }


            public override bool UseTool(AnalyzeMap map, Tile tile)
            {
                if (map[tile].Type != tileType)
                {
                    map[tile].Type = tileType;
                    return true;
                }
                return false;
            }
        }

        public void NewMap()
        {
            gridControl1.GridSizeX = 16;
            gridControl1.GridSizeY = 16;

            map = new AnalyzeMap(gridControl1.GridSizeX, gridControl1.GridSizeY);
            map.OnResize += new AnalyzeMap.MapResizeHandler(map_OnResize);
            mapFileName = null;
            autoSaveFileName = null;
        }

        public MainForm()
        {
            InitializeComponent();
            font = new Font("Verdana", 18);

            NewMap();

            LoadConfig();

            if (players.Count == 0)
                players.Add(new Player());

            foreach (Player player in players)
            {
                LogParser parser = new LogParser(player);
                parser.OnAnalyze += new LogParser.AnalyzeEventHandler(parser_OnAnalyze);
                parser.Start();
            }
        }

        void map_OnResize(object sender, int newX, int newY, int dx, int dy)
        {
            gridControl1.GridSizeX = map.SizeX;
            gridControl1.GridSizeY = map.SizeY;
            gridControl1.Redraw();
        }

        private void AnalyzeFile(String fileName)
        {
            try
            {
                LogParser parser = new LogParser();
                parser.OnAnalyze += new LogParser.AnalyzeEventHandler(parser_OnAnalyze);

                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader(stream);
                while (!reader.EndOfStream)
                {
                    String line = reader.ReadLine();
                    parser.ParseLine(line);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not analyze the file: " + ex.Message);
            }
        }

        private bool LoadConfig()
        {
            String path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            String filename = path + "\\" + "AnalyzeTool.xml";

            if (!new FileInfo(filename).Exists)
                return false;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);

                XPathNavigator nav = doc.CreateNavigator();

                XPathNodeIterator iter;
                iter = nav.Select("//AnalyzeTool/Player");
                while (iter.MoveNext())
                {
                    XmlElement element = iter.Current.UnderlyingObject as XmlElement;

                    Player player = new Player();
                    player.PlayerName = iter.Current.Evaluate("string(@name)") as String;
                    player.WurmDir = iter.Current.Evaluate("string(@wurmdir)") as String;

                    System.Diagnostics.Debug.WriteLine(String.Format("Configured player {0} in {1}", player.PlayerName, player.WurmDir));
                    this.players.Add(player);
                }
            }
            catch (Exception e)
            {
                this.players.Clear();
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return false;
            }

            return true;
        }


        void parser_OnAnalyze(object sender, List<AnalyzeMatch> matches)
        {
            Debug.Print("### Analyze results");
            foreach (AnalyzeMatch match in matches)
            {
                Debug.Print("{0}  {1} {2}", match.Distance, match.Type, match.Quality);
            }

            Debug.Print("{0}", new AnalyzeResult(matches));

            AddResult(new AnalyzeResult(matches));
        }

        delegate void AddResultCallback(AnalyzeResult result);
        private void AddResult(AnalyzeResult result)
        {
            if (resultsBox.InvokeRequired)
            {
                AddResultCallback d = new AddResultCallback(AddResult);
                this.Invoke(d, new object[] { result });
            }
            else
            {
                resultsBox.Items.Add(result);
                resultsBox.SelectedItem = result;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DrawCellBackground(GridControl.Cell cell, Rectangle area, Graphics graphics)
        {
            Image texture = GetTileTexture(cell.X, cell.Y);
            if (texture == null)
            {
                switch (map[cell.X, cell.Y].Type)
                {
                    case TileType.Tunnel:
                        graphics.FillRectangle(Brushes.White, area);
                        break;
                    case TileType.Rock:
                        graphics.FillRectangle(Brushes.Gray, area);
                        break;
                    default:
                        graphics.FillRectangle(Brushes.Green, area);
                        break;
                }
            }
            else
            {
                graphics.DrawImage(texture, area);
            }
        }

        private void DrawCellAnalyzeLayer(GridControl.Cell cell, Rectangle area, Graphics graphics)
        {
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;
            if (map[cell.X, cell.Y].IsEmpty)
            {
            }
            else if (map[cell.X, cell.Y].IsSet)
            {
            }
            else if (map[cell.X, cell.Y].IsUndecided)
            {
                graphics.DrawString("?", font, Brushes.Red, area, drawFormat);
            }

            if (map[cell.X, cell.Y].Result != null)
            {
                graphics.DrawRectangle(Pens.Red, new Rectangle(area.Left, area.Top, area.Width - 1, area.Height - 1));
            }
        }

        private void gridControl1_OnCellPaint(object sender, GridControl.Cell cell, PaintEventArgs eventArgs)
        {
            DrawCellBackground(cell, eventArgs.ClipRectangle, eventArgs.Graphics);
            if (renderAnalyzerOverlay)
            {
                DrawCellAnalyzeLayer(cell, eventArgs.ClipRectangle, eventArgs.Graphics);
            }
        }

        private Tile CellToTile(GridControl.Cell cell)
        {
            return new Tile(cell.X, cell.Y);
        }

        private void gridControl1_CellClick(object sender, GridControl.Cell cell, MouseEventArgs eventArgs)
        {
            if (eventArgs.Button == MouseButtons.Left && (Control.ModifierKeys & Keys.Control) != 0)
            {
                map[cell.X, cell.Y].SetEmpty();
                Recalculate();
            }
            else if (eventArgs.Button == MouseButtons.Left)
            {
                if (activeTool != null)
                {
                    if (activeTool.UseTool(map, CellToTile(cell)))
                    {
                        gridControl1.PaintCell(cell.X, cell.Y);
                    }
                }
            }
        }

        private void resultsBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (resultsBox.SelectedItem != null)
                resultsBox.DoDragDrop(resultsBox.SelectedItem, DragDropEffects.Move);
        }

        private void gridControl1_DragEnter(object sender, DragEventArgs e)
        {
            AnalyzeResult result = e.Data.GetData(typeof(AnalyzeResult)) as AnalyzeResult;
            if (result != null)
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void gridControl1_DragOver(object sender, DragEventArgs e)
        {
        }

        private void gridControl1_DragDrop(object sender, DragEventArgs e)
        {
            Point pt = gridControl1.PointToClient(new Point(e.X,e.Y));
            System.Diagnostics.Debug.Print("DragDrop {0},{1}", pt.X, pt.Y);
            AnalyzeResult result = e.Data.GetData(typeof(AnalyzeResult)) as AnalyzeResult;
            if (result != null)
            {
                GridControl.Cell cell = gridControl1.CellFromPoint(pt.X, pt.Y);
                if (cell != null)
                {
                    map.SetResult(cell.X, cell.Y, result);
                    gridControl1.Redraw();
                }
            }
        }

        private void Recalculate()
        {
            map.Refresh();
            gridControl1.Redraw();
        }

        private void gridControl1_CellMouseMove(object sender, GridControl.Cell cell, MouseEventArgs eventArgs)
        {
            if (eventArgs.Button == MouseButtons.Left && activeTool != null && Control.ModifierKeys == Keys.None)
            {
                if (activeTool.UseTool(map, CellToTile(cell)))
                {
                    gridControl1.PaintCell(cell.X, cell.Y);
                }
            }
        }

        private void gridControl1_CellMouseEnter(object sender, GridControl.Cell cell, MouseEventArgs eventArgs)
        {
            String tileType;
            if (map[cell.X, cell.Y].Quality != Quality.Unknown)
            {
                tileType = String.Format("{2},{3} {0} ({1})", map[cell.X, cell.Y].Type, map[cell.X, cell.Y].Quality, cell.X, cell.Y);
            } else {
                tileType = String.Format("{2},{3} {0}", map[cell.X, cell.Y].Type, map[cell.X, cell.Y].Quality, cell.X, cell.Y);
            }

            if (map[cell.X, cell.Y].IsSet || map[cell.X, cell.Y].IsUndecided)
            {
                toolStripStatusLabel1.Text = String.Format("{0}; {1}", tileType, map[cell.X, cell.Y].ToString());
            }
            else
            {
                toolStripStatusLabel1.Text = tileType;
            }
        }

        private void gridControl1_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void analyzeFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = players[0].LogDir;
            dialog.Filter = "Event log (_Event*.txt)|_Event*.txt|Logfiles (*.txt)|*.txt|All files|*.*";
            dialog.FilterIndex = 1;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                AnalyzeFile(dialog.FileName);
            }
        }

        Dictionary<TileType, Bitmap> textureCache = new Dictionary<TileType, Bitmap>();
        private Bitmap GetTileTexture(int x, int y)
        {
            TileType type = map[x, y].Type;
            if (!textureCache.ContainsKey(type))
            {
                Bitmap bitmap = GetTileTexture(type);
                if (bitmap == null)
                {
                    textureCache.Add(type, null);
                } 
                else if (bitmap.Width != textureWidth || bitmap.Height != textureHeight)
                {
                    Bitmap scaled = new Bitmap(bitmap, new Size(textureWidth, textureHeight));
                    textureCache.Add(type, scaled);
                }
                else
                {
                    textureCache.Add(type, bitmap);
                }
            }
            return textureCache[type];
        }

        private Bitmap GetTileTexture(TileType type)
        {
            switch (type)
            {
                case TileType.Tunnel:
                    return AnalyzeTool.Properties.Resources.slab;
                case TileType.Unknown:
                case TileType.Rock:
                    return AnalyzeTool.Properties.Resources.rock;
                case TileType.Copper:
                    return AnalyzeTool.Properties.Resources.copperore;
                case TileType.Gold:
                    return AnalyzeTool.Properties.Resources.goldore;
                case TileType.Iron:
                    return AnalyzeTool.Properties.Resources.ironore;
                case TileType.Lead:
                    return AnalyzeTool.Properties.Resources.leadore;
                case TileType.Marble:
                    return AnalyzeTool.Properties.Resources.marbleshards;
                case TileType.Silver:
                    return AnalyzeTool.Properties.Resources.silverore;
                case TileType.Slate:
                    return AnalyzeTool.Properties.Resources.slateshards;
                case TileType.Tin:
                    return AnalyzeTool.Properties.Resources.tinore;
                case TileType.Zinc:
                    return AnalyzeTool.Properties.Resources.zincore;
                default:
                    return null;
            }
        }

        private void setCheckedTool(object sender) 
        {
            if (sender == null)
            {
                setCheckedToolButton(null);
                setCheckedToolMenuItem(null);
            }
            else if (sender is ToolStripButton)
            {
                setCheckedToolButton(sender as ToolStripButton);
            }
            else if (sender is ToolStripMenuItem)
            {
                setCheckedToolMenuItem(sender as ToolStripMenuItem);
            }
        }

        private void setCheckedToolMenuItem(ToolStripMenuItem item)
        {
            setCheckedToolButton(null);
            if (item != null)
                toolStripOre.Image = item.Image;
            else
                toolStripOre.Image = toolStripIron.Image;
            foreach (ToolStripItem dropDownItem in toolStripOre.DropDownItems)
            {
                if (dropDownItem is ToolStripMenuItem)
                {
                    ToolStripMenuItem menuItem = dropDownItem as ToolStripMenuItem;
                    menuItem.Checked = menuItem == item;
                }
            }
        }

        private void setCheckedToolButton(ToolStripButton sender)
        {
            foreach (ToolStripItem item in toolStrip1.Items)
            {
                ToolStripButton button = item as ToolStripButton;
                if (button != null)
                {
                    button.Checked = button == sender;
                }
            }
        }

        private void setActiveTool(TileType tileType)
        {
            this.activeTool = new TileTypeTool(tileType);
        }

        private void toolStripTunnel_Click(object sender, EventArgs e)
        {
            setActiveTool(TileType.Tunnel);
            setCheckedTool(sender);
        }

        private void toolStripRock_Click(object sender, EventArgs e)
        {
            setActiveTool(TileType.Rock);
            setCheckedTool(sender);
        }

        private void copperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setActiveTool(TileType.Copper);
            setCheckedTool(sender);
        }

        private void toolStripLead_Click(object sender, EventArgs e)
        {
            setActiveTool(TileType.Lead);
            setCheckedTool(sender);
        }

        private void toolStripGold_Click(object sender, EventArgs e)
        {
            setActiveTool(TileType.Gold);
            setCheckedTool(sender);
        }

        private void toolStripIron_Click(object sender, EventArgs e)
        {
            setActiveTool(TileType.Iron);
            setCheckedTool(sender);
        }

        private void toolStripMarble_Click(object sender, EventArgs e)
        {
            setActiveTool(TileType.Marble);
            setCheckedTool(sender);
        }

        private void toolStripSilver_Click(object sender, EventArgs e)
        {
            setActiveTool(TileType.Silver);
            setCheckedTool(sender);
        }

        private void toolStripSlate_Click(object sender, EventArgs e)
        {
            setActiveTool(TileType.Slate);
            setCheckedTool(sender);
        }

        private void toolStripTip_Click(object sender, EventArgs e)
        {
            setActiveTool(TileType.Tin);
            setCheckedTool(sender);
        }

        private void toolStripZinc_Click(object sender, EventArgs e)
        {
            setActiveTool(TileType.Zinc);
            setCheckedTool(sender);
        }

        private void resetAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewMap();
            gridControl1.Redraw();
        }

        private void resetAnalyzedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            map.ClearResults();
            gridControl1.Redraw();
        }

        private void resizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResizeMapForm dialog = new ResizeMapForm();
            dialog.WidthBox.Text = map.SizeX.ToString();
            dialog.HeightBox.Text = map.SizeY.ToString();
            dialog.LeftBox.Text = "0";
            dialog.TopBox.Text = "0";

            if (dialog.ShowDialog() == DialogResult.OK) 
            {
                try
                {
                    int newX = Int32.Parse(dialog.WidthBox.Text);
                    int newY = Int32.Parse(dialog.HeightBox.Text);
                    int dX = Int32.Parse(dialog.LeftBox.Text);
                    int dY = Int32.Parse(dialog.TopBox.Text);
                    map.ResizeMap(newX, newY, dX, dY);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error resizing the map: " + ex.Message);
                }
            }

        }

        private void SaveMap(String filename)
        {
            try
            {
                using (Stream stream = new FileStream(filename, FileMode.Create))
                {
                    map.Save(stream);
                    mapFileName = filename;
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving the map: " + ex.Message);
            }
        }

        private void saveMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (autoSaveFileName != null)
            {
                SaveMap(autoSaveFileName);
            }
            else
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Analyze tool map (*.atm)|*.atm";
                dialog.FilterIndex = 1;
                dialog.FileName = mapFileName;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    SaveMap(dialog.FileName);
                    autoSaveFileName = dialog.FileName;
                }
            }
        }

        private void exportBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "JPEG files (*.jpg,*.jpeg)|*.jpg *.jpeg|PNG files (*.png)|*.png";
            dialog.FilterIndex = 2;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap bitmap = new Bitmap(gridControl1.ClientSize.Width, gridControl1.ClientSize.Height);
                Boolean oldState = renderAnalyzerOverlay;
                renderAnalyzerOverlay = false;
                gridControl1.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                renderAnalyzerOverlay = oldState;

                bitmap.Save(dialog.FileName);
            }
        }

        private void LoadMap(String filename)
        {
            using (Stream stream = new FileStream(filename, FileMode.Open))
            {
                this.map = AnalyzeMap.Load(stream);
                gridControl1.GridSizeX = map.SizeX;
                gridControl1.GridSizeY = map.SizeY;
                map.OnResize += new AnalyzeMap.MapResizeHandler(map_OnResize);
                mapFileName = filename;
                autoSaveFileName = null;
                activeTool = null;
                setCheckedTool(null);
            }
        }

        private void openMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Analyze tool map (*.atm)|*.atm";
            dialog.FilterIndex = 1;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    LoadMap(dialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading the map: " + ex.Message);
                }
            }
        }
    }
}