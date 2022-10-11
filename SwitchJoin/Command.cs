#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#endregion

namespace AM_Gerator
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            Selection selection = uidoc.Selection;
            ICollection<ElementId> selectedIds = selection.GetElementIds();
            

            
            FilteredElementCollector wall = new FilteredElementCollector(doc, selectedIds).OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType();
            FilteredElementCollector floor = new FilteredElementCollector(doc, selectedIds).OfCategory(BuiltInCategory.OST_Floors).WhereElementIsNotElementType();
            FilteredElementCollector beam = new FilteredElementCollector(doc, selectedIds).OfCategory(BuiltInCategory.OST_StructuralFraming).WhereElementIsNotElementType();
            FilteredElementCollector column = new FilteredElementCollector(doc, selectedIds).OfCategory(BuiltInCategory.OST_StructuralColumns).WhereElementIsNotElementType();


            var elementClasses = new List<FilteredElementCollector> { wall, beam, column, floor };

            

            // Switch joint order
            Transaction t = new Transaction(doc, "Switch join order");
            t.Start();

        }
     }
 }


