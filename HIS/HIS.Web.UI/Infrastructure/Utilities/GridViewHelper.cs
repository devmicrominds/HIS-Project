using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

public class GridViewHelper
{

    public static int GetGridViewRowIndex(GridView grid, int dataItemIndex)
    {
        return (grid.PageIndex * grid.PageSize) + dataItemIndex + 1;
    }

    public static void GetGridViewHeader(GridView grid, TableCellCollection cells)
    {

        GridViewRow row = new GridViewRow(0, 0,
                            DataControlRowType.Header, DataControlRowState.Insert);

        List<TableCell> collection = new List<TableCell>();

        foreach (TableCell x in cells)
        {
            string[] headerText = x.Text.Split('_');


            var oTableCell = new TableCell();

            var count = headerText.Count();
            if (count > 1)
            {
                if (!collection.Exists(cell => cell.Text == headerText[0]))
                {
                    oTableCell.Text = headerText[0];
                }
                x.Text = headerText[1];
            }
            else
            {
                oTableCell.Text = string.Empty;

            }

            collection.Add(oTableCell);

        }


        row.Cells.AddRange(collection.ToArray());
        row.BackColor = System.Drawing.Color.White;
        row.Font.Bold = true;
        grid.Controls[0].Controls.AddAt(0, row);

    }

    public static string GetTableRow(object name, object value)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<tr style='height:30px;'>");
        sb.AppendFormat(@"<td style='width:auto;' align='left'><p class='text-info'>{0}<p></td>", name);
        sb.AppendFormat(@"<td style='width:auto;' align='right'>{0}</td>", value);
        sb.Append("</tr>");
        return sb.ToString();
    }

    public static string GetTableRow(object value)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<tr style='height:30px;'>");
        sb.AppendFormat(@"<td style='width:auto;' align='left'>{0}</td>", value);
        sb.Append("</tr>");
        return sb.ToString();
    }

    public static string GetEmptyRow()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<tr style='height:30px;'>");
        sb.AppendFormat(@"<td style='width:auto;' align='left'></td>");
        sb.AppendFormat(@"<td style='width:auto;' align='right'></td>");
        sb.Append("</tr>");
        return sb.ToString();
    }


}