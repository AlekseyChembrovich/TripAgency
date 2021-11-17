using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TripAgency.PrecentationLayer.Tools
{
    public static class FormExtensions
    {
        public static void GenerateColumns(this Form form, ListView listView, int width, params string[] names)
        {
            listView.Items.Clear();
            listView.Columns.Clear();
            foreach (var item in names)
            {
                listView.Columns.Add(item, width);
            }

            listView.Columns[0].Width = 0;
        }

        public static void OpenAddPanel(this Form form, Panel target, List<Panel> addPanels)
        {
            addPanels.Except(new List<Panel> { target }).ToList().ForEach(x =>
            {
                x.ClosePanel();
            });

            target.OpenPanel();
        }

        public static void FillCombobox(this Form form, ComboBox boxAttraction, string[] items)
        {
            boxAttraction.Items.Clear();
            boxAttraction.Items.AddRange(items);
        }
    }
}
