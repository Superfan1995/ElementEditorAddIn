using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace ElementEditorAddIn
{
    #region External Application

    /// <summary>
    /// Create Revit UI and control Element Editor Windows Form, the 'main' function of the program
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class ElementEditorAppplication : IExternalApplication
    {
        public static ElementEditorAppplication thisApp;
        private ElementEditorForm elementEditorForm;        // Element Editor Windows Form

        /// <summary>
        /// Close the ElementEditorForm when revit shut down
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public Result OnShutdown(UIControlledApplication application)
        {
            if (elementEditorForm != null && elementEditorForm.Visible)
            {
                elementEditorForm.Close();
            }

            return Autodesk.Revit.UI.Result.Succeeded;
        }

        /// <summary>
        /// Create Revit UI elements for Element Editor on start up
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public Result OnStartup(UIControlledApplication application)
        {
            thisApp = this;
            elementEditorForm = null;

            // set up panel
            RibbonPanel elePreviewPanel = application.CreateRibbonPanel("Element Editor");

            // get assembly path
            string assemblyPath = Assembly.GetExecutingAssembly().Location;

            // create pushbutton
            PushButtonData elePreviewButtonData = new PushButtonData("cmdEleEditor", "Element Editor", assemblyPath, "ElementEditorAddIn.ElementEditorOpenForm");
            PushButton elePreviewButton = elePreviewPanel.AddItem(elePreviewButtonData) as PushButton;

            // Get picture for the pushbutton
            Uri uriImage1 = new Uri(@"C:\Users\labuser\Pictures\test32x32.png");
            BitmapImage buttonImage = new BitmapImage(uriImage1);
            elePreviewButton.LargeImage = buttonImage;

            return Autodesk.Revit.UI.Result.Succeeded;
        }

        /// <summary>
        /// Create and Show new ElementEditorForm
        /// </summary>
        /// <param name="commandData"></param>
        public void CreateForm(ExternalCommandData commandData)
        {
            // create ExternalEventHandler for Edit parameters
            ParaEditEventHandler handler = new ParaEditEventHandler();
            ExternalEvent paraEditEvent = ExternalEvent.Create(handler);
            handler.ParaEditEvent = paraEditEvent;

            // create element editor form
            elementEditorForm = new ElementEditorForm(commandData, handler);

            handler.ElementEditorForm = elementEditorForm;

            elementEditorForm.Show();
        }
    }

    #endregion

    #region External Command

    /// <summary>
    /// OnClick Event that open the ElementEditorForm
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class ElementEditorOpenForm : IExternalCommand
    {
        /// <summary>
        /// Use method in ElementEditorAppplication to create new Element Editor form when the element 
        /// editor push putton is clicked
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="message"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                // use the ElementEditorApplication to create and show ElementEditorForm
                ElementEditorAppplication.thisApp.CreateForm(commandData);
                return Autodesk.Revit.UI.Result.Succeeded;
            }

            catch (Exception ex)
            {
                TaskDialog.Show("Revit", ex.ToString());
                return Autodesk.Revit.UI.Result.Failed;
            }
        }
    }

    #endregion
}
