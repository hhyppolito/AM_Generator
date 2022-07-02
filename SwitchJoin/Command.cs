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

namespace SwitchJoin
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

            FilteredElementCollector wall = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType();
            FilteredElementCollector floor = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Floors).WhereElementIsNotElementType();
            FilteredElementCollector beam = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralFraming).WhereElementIsNotElementType();
            FilteredElementCollector column = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralColumns).WhereElementIsNotElementType();



            //start

            Form1 forme = new Form1();
            forme.ShowDialog();


            //get string and convert to respective type;


            //string wallValueString = forme..ToString();
            //int intWallValue = int.Parse(wallValueString);

            //string columnValueString = forme.columnOrderstr.ToString();
            //int intColumnValue = int.Parse(columnValueString);

            //string beamValueString = forme.beamOrderstr.ToString();
            //int intBeamValue = int.Parse(beamValueString);

            //string floorlValueString = forme.floorOrderstr.ToString();
            //int intFloorValue = int.Parse(floorlValueString);



            var classOrder = new List<int> { Form1.Orders[2], Form1.Orders[0], Form1.Orders[1], Form1.Orders[3] };
          
            //select element

            var elementClasses = new List<FilteredElementCollector> { wall, beam, column, floor };

            // Switch joint order
            Transaction t = new Transaction(doc, "Switch join order");
            t.Start();
            for (int i = 0; i < classOrder.Count - 1; i++)
            {
                foreach (Element principalClass in elementClasses[classOrder.IndexOf(i + 1)])
                {
                    for (int j = 0; j < classOrder.Count - i - 1; j++)
                    {
                        foreach (Element secondaryClass in elementClasses[classOrder.IndexOf(j + i + 2)])
                        {
                            bool areJoined = JoinGeometryUtils.AreElementsJoined(doc, secondaryClass, principalClass);

                            //VERIFICA SE O PILAR ESTA ATTACHED A VIGA, SE SIM NÃO FAZ NADA
                            if (classOrder.IndexOf(i + 1) == 2 & classOrder.IndexOf(j + i + 2) == 1)
                            {
                                FamilyInstance famInst = principalClass.Document.GetElement(principalClass.Id) as FamilyInstance;
                                var targetElement = ColumnAttachment.GetColumnAttachment(famInst, secondaryClass.Id);
                                bool areAttached = true;
                                if (targetElement != null)
                                {
                                    areAttached = Equals(targetElement.TargetId, secondaryClass.Id);

                                }
                                else
                                {
                                    areAttached = false;
                                }
                                if (areAttached)
                                {
                                    areJoined = false;
                                }
                            }
                            if (classOrder.IndexOf(j + i + 2) == 2 & classOrder.IndexOf(i + 1) == 1)
                            {
                                FamilyInstance famInst = secondaryClass.Document.GetElement(secondaryClass.Id) as FamilyInstance;
                                var targetElement = ColumnAttachment.GetColumnAttachment(famInst, principalClass.Id);
                                bool areAttached = true;
                                if (targetElement != null)
                                {
                                    areAttached = Equals(targetElement.TargetId, principalClass.Id);
                                }
                                else
                                {
                                    areAttached = false;
                                }
                                if (areAttached)
                                {
                                    areJoined = false;
                                }
                            }

                            if (areJoined)
                            {
                                bool iscut = JoinGeometryUtils.IsCuttingElementInJoin(doc, secondaryClass, principalClass);
                                if (iscut)
                                {
                                    try
                                    {
                                        JoinGeometryUtils.SwitchJoinOrder(doc, principalClass, secondaryClass);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Error found.");
                                    }
                                }

                            }

                        }
                    }
                }
            }

            Form1.Orders.Clear();

            t.Commit();
            return Result.Succeeded;
        }
    }
}

