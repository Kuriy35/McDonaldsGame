using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Net.NetworkInformation;
using System.Windows;

namespace McDonalds.Models.Restaurant
{
    public class DiningArea
    {
        public List<Table> Tables { get; private set; }

        public DiningArea(List<Point> tablePositions = null)
        {
            Tables = new List<Table>();

            if (tablePositions == null)
            {
                tablePositions = new List<Point>();
                tablePositions.AddRange(new[] {
                    new Point(100, 100),
                    new Point(300, 100),
                    new Point(500, 100),
                    new Point(200, 300),
                    new Point(400, 300),   
                });
            }

            for (int i = 0; i < tablePositions.Count; i++)
            {
                Tables.Add(new Table(i + 1, tablePositions[i]));
            }
        }

        public Table GetFreeTable()
        {
            return Tables.FirstOrDefault(t => !t.IsOccupied);
        }
    }
}