using Nit.Phonebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using Nit.Phonebook.Models.Data;
using System.Threading;
using System.IO;
using Xceed.Words.NET;
using System.Diagnostics;

namespace Nit.Phonebook.Logics
{
    class ReportGenerator
    {
        internal const string SampleDirectory = @"..\..\Reports\";

        const string TableSampleResourcesDirectory = SampleDirectory + @"Table\Resources\";
        const string TableSampleOutputDirectory = SampleDirectory + @"Output\";
        const string SavedName = @"Report.docx";

        float[] ColWidthsSize = new float[]{
            300f,//عنوان
            100f,//داخلی
            200f,//مستقیم
            300,//اسامی
        };
        const double FirstPargFontSize = 15d;
        const double FontSize = 11d;


        TreeItem root = null;

        int cntRows = 0;
        int CurrentRow = 1;


        public ReportGenerator(int catIdParent)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("fa-IR");
                RefillTree(catIdParent);
                CreateTable();
                Process.Start(TableSampleOutputDirectory + SavedName);
            }
            catch
            {
                throw new Exception();
            }
        }

        public void CreateTable()
        {
            try
            {
                if (root == null)
                {
                    RefillTree();
                }
                if (root == null) return;

                CurrentRow = 1;

                if (!Directory.Exists(TableSampleOutputDirectory))
                {
                    Directory.CreateDirectory(TableSampleOutputDirectory);
                }
                using (DocX document = DocX.Create(TableSampleOutputDirectory + SavedName))
                {
                    document.AddHeaders();

                    // Get the default Header.
                    Header header = document.Headers.Odd;

                    // Insert a Paragraph into the Header.
                    Paragraph p0 = header.InsertParagraph();
                    p0.Direction = Direction.RightToLeft;
                    p0.Alignment = Alignment.right;
                    // Appemd place holders for PageNumber and PageCount into the Header.
                    // Word will replace these with the correct value for each Page.
                    p0.Append("صفحه (");
                    p0.AppendPageNumber(PageNumberFormat.normal);
                    p0.Append(")");


                    int cntChildren = root.Children.Count;

                    foreach (var child in root.Children)
                    {
                        #region new page
                        var par = document.InsertParagraph(child.Category.Title).FontSize(FirstPargFontSize).SpacingAfter(50d);
                        par.Alignment = Alignment.left;
                        par.Direction = Direction.RightToLeft;

                        var columnWidths = new float[child.Height + 3 >= 4 ? child.Height + 3 : 4];//root.Height;
                        columnWidths[columnWidths.Length - 4] = ColWidthsSize[0];//"عنوان"
                        columnWidths[columnWidths.Length - 3] = ColWidthsSize[1];//"داخلی"
                        columnWidths[columnWidths.Length - 2] = ColWidthsSize[2];//"مستقیم"
                        columnWidths[columnWidths.Length - 1] = ColWidthsSize[3];//"اسامی"

                        var t = document.InsertTable(1, columnWidths.Length);

                        t.SetDirection(Direction.RightToLeft);

                        t.SetWidths(columnWidths);
                        t.Design = TableDesign.TableNormal;
                        t.AutoFit = AutoFit.Contents;

                        var row = t.Rows.First();

                        foreach (var cell in row.Cells)
                        {
                            cell.FillColor = Color.LightGray;
                        }
                        // Fill in the columns of the first row in the table.
                        row.Cells[0].Paragraphs.First().Append("عنوان").Alignment = Alignment.center;
                        row.Cells[row.Cells.Count - 3].Paragraphs.First().Append("داخلی").Alignment = Alignment.center;
                        row.Cells[row.Cells.Count - 2].Paragraphs.First().Append("مستقیم").Alignment = Alignment.center;
                        row.Cells[row.Cells.Count - 1].Paragraphs.First().Append("اسامی").Alignment = Alignment.center;
                        if (row.Cells.Count - 4 > 0)
                            row.MergeCells(0, row.Cells.Count - 4);


                        // Add rows in the table.
                        for (int i = 0; i < child.CntTotalRows; i++)
                        {
                            var newRow = t.InsertRow();

                            // Fill in the columns of the new rows.
                            for (int j = 0; j < newRow.Cells.Count; ++j)
                            {
                                var newCell = newRow.Cells[j];
                                //newCell.Paragraphs.First().Append("$EMPTY");
                                newCell.SetDirection(Direction.RightToLeft);
                                newCell.VerticalAlignment = VerticalAlignment.Center;
                            }
                        }


                        // Set a blank border for the table's top/bottom borders.
                        var blankBorder = new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.one, 0, Color.Black);
                        t.SetBorder(TableBorderType.Bottom, blankBorder);
                        t.SetBorder(TableBorderType.Top, blankBorder);
                        t.SetBorder(TableBorderType.Left, blankBorder);
                        t.SetBorder(TableBorderType.Right, blankBorder);
                        t.SetBorder(TableBorderType.InsideH, blankBorder);
                        t.SetBorder(TableBorderType.InsideV, blankBorder);
                        #endregion

                        CurrentRow = 1;
                        FillTableRecursivelyHelper(document, t, child, CurrentRow, 0);
                        if (cntChildren > 1)
                            document.InsertSectionPageBreak();

                        cntChildren--;
                    }

                    document.Save();
                }
            }
            catch
            {

            }
        }

        public void FillTableRecursivelyHelper(DocX document, Table t, TreeItem node, int currentRow, int currentCol)
        {
            try
            {
                bool isLevel1 = root != null && node.Parent == root;

                if (isLevel1 == false)
                {
                    if (currentRow != currentRow + node.CntTotalRows - 1 && currentRow + node.CntTotalRows - 1 < t.RowCount)
                        t.MergeCellsInColumn(currentCol, currentRow, currentRow + node.CntTotalRows - 1);

                    var currRow = t.Rows[currentRow].Cells[currentCol].InsertParagraph(node.Category.Title).FontSize(11d);
                    currRow.Alignment = Alignment.center;
                    currRow.Direction = Direction.RightToLeft;
                }


                CurrentRow += node.Category.Rows.Count;

                int localCurrentRow = currentRow;


                foreach (var row in node.Category.Rows)
                {
                    foreach (var emp in row.Employees.ToList())
                    {
                        var p = t.Rows[localCurrentRow].Cells[t.ColumnCount - 1].InsertParagraph(emp.Name).FontSize(FontSize);
                        p.Alignment = Alignment.center;
                        p.Direction = Direction.RightToLeft;

                    }

                    foreach (var ph in row.PhoneNumbers.ToList())
                    {

                        var p = t.Rows[localCurrentRow].Cells[ph.IsInternal ? t.ColumnCount - 3 : t.ColumnCount - 2].InsertParagraph(ph.Number).FontSize(FontSize);
                        p.Alignment = Alignment.center;
                        p.Direction = Direction.RightToLeft;
                    }
                    if (node.Children.Count == 0 && currentCol < t.ColumnCount - 4)
                    {
                        t.Rows[localCurrentRow].MergeCells(currentCol, t.ColumnCount - 4);
                    }
                    else if (root.Height - node.Height - 1 < t.ColumnCount - 4 && root.Height - node.Height - 1 >= 0)
                    {
                        //if (isLevel1 == true)
                        //{
                        //    t.Rows[localCurrentRow].MergeCells(currentCol, t.ColumnCount - 4);
                        //}
                        // t.Rows[localCurrentRow].MergeCells(root.Height - node.Height - 1, t.ColumnCount - 4);
                    }
                    localCurrentRow++;
                }


                if (isLevel1 == true)
                {
                    foreach (var child in node.Children)
                    {
                        FillTableRecursivelyHelper(document, t, child, CurrentRow < t.RowCount ? CurrentRow : t.RowCount - 1, currentCol);
                    }
                    return;
                }

                foreach (var child in node.Children)
                {
                    FillTableRecursivelyHelper(document, t, child, CurrentRow < t.RowCount ? CurrentRow : t.RowCount - 1, currentCol + 1);
                }
            }
            catch
            {

            }

        }

        private void RefillTree(int parentId = 1)
        {
            cntRows = 0;
            using (var db = new PhonebookContext(ConnectionStringBuilder.ConnectionString))
            {
                db.Employees.ToList();//load employees
                db.PhoneNumbers.ToList();//load phoneNumbers

                TreeItem root = new TreeItem();
                root.Category = db.Categories.Single(c => c.Id == parentId);
                FillTree(db.Categories.ToList(), root, db);
                this.root = root;
            }

        }
        private void FillTree(List<Category> Categories, TreeItem Item, PhonebookContext db)
        {
            cntRows += Item.Category.Rows.Count;
            Item.CntTotalRows = Item.Category.Rows.Count();

            Item.Category.Rows.ToList().ForEach(r =>
            {

                r.Employees.ToList();
                r.PhoneNumbers.ToList();
            });

            var children = Categories.Where(c => c.ParentId == Item.Category.Id);
            foreach (var child in children)
            {
                TreeItem menuItem = new TreeItem();
                menuItem.Category = child;
                menuItem.Parent = Item;
                Item.Children.Add(menuItem);
                FillTree(Categories, menuItem, db);

                Item.Height = Math.Max(Item.Height, menuItem.Height + 1);
                Item.CntTotalRows += menuItem.CntTotalRows;
            }
        }
        public class TreeItem
        {
            public TreeItem()
            {
                Children = new List<TreeItem>();
            }

            public Category Category { get; set; }//Current Category

            public TreeItem Parent { get; set; }
            public List<TreeItem> Children { get; set; }
            public int Height { get; set; }
            public int CntTotalRows { get; set; }
            public bool IsLeaf()
            {
                if (Children == null) return true;
                if (Children.Count == 0) return true;
                return false;
            }

        }
    }
}
