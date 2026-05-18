using System.Linq;
using NUnit.Framework;
using RIMA.MapDesigner.Editor.Blueprint;
using UnityEngine;

namespace RIMA.MapDesigner.Tests
{
    public sealed class BlueprintCanvasTests
    {
        [Test]
        public void Paint_SingleCell_StoresZoneId()
        {
            var canvas = new BlueprintCanvas(new Vector2Int(36, 22));

            canvas.Paint(new Vector2Int(4, 5), "grass", 1);

            Assert.AreEqual("grass", canvas.GetZoneAt(new Vector2Int(4, 5)));
        }

        [Test]
        public void Paint_WithBrushSize3_Stores9Cells()
        {
            var canvas = new BlueprintCanvas(new Vector2Int(36, 22));

            canvas.Paint(new Vector2Int(10, 10), "stone", 3);

            Assert.AreEqual(9, canvas.CellsForZone("stone").Count());
        }

        [Test]
        public void Erase_RemovesCell()
        {
            var canvas = new BlueprintCanvas(new Vector2Int(36, 22));
            Vector2Int cell = new Vector2Int(2, 3);
            canvas.Paint(cell, "path", 1);

            canvas.Erase(cell, 1);

            Assert.IsNull(canvas.GetZoneAt(cell));
        }

        [Test]
        public void FloodFill_FillsContiguousRegion_Iterative()
        {
            var canvas = new BlueprintCanvas(new Vector2Int(36, 22));

            canvas.FloodFill(new Vector2Int(0, 0), "wall");

            Assert.AreEqual(36 * 22, canvas.CellsForZone("wall").Count());
        }

        [Test]
        public void Clear_RemovesAllCells()
        {
            var canvas = new BlueprintCanvas(new Vector2Int(36, 22));
            canvas.Paint(new Vector2Int(1, 1), "path", 3);

            canvas.Clear();

            Assert.AreEqual(0, canvas.Count);
        }

        [Test]
        public void BoundaryEdges_DetectsZoneBoundaries()
        {
            var canvas = new BlueprintCanvas(new Vector2Int(36, 22));
            canvas.Paint(new Vector2Int(0, 0), "grass", 1);
            canvas.Paint(new Vector2Int(1, 0), "stone", 1);
            canvas.Paint(new Vector2Int(0, 1), "grass", 1);

            var edges = canvas.BoundaryEdges().ToList();

            Assert.AreEqual(1, edges.Count);
            Assert.AreEqual(new Vector2Int(0, 0), edges[0].a);
            Assert.AreEqual(new Vector2Int(1, 0), edges[0].b);
        }
    }
}
