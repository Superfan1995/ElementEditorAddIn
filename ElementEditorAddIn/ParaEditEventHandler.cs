using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ElementEditorAddIn
{
    /// <summary>
    /// ExternalEventHandler that edit the value of parameter
    /// </summary>
    public class ParaEditEventHandler : IExternalEventHandler
    {
        #region Parameters

        private ExternalEvent paraEditEvent;
        private ElementEditorForm elementEditorForm;

        private List<Parameter> parameters;
        private List<string> newParaString;
        private List<double> newParaDouble;
        private List<int> newParaInteger;

        private List<string> handlerMode;

        private static string MODE_STRING = "STRING";
        private static string MODE_DOUBLE = "DOUBLE";
        private static string MODE_INTEGER = "INTEGER";

        #endregion

        #region Properties

        /// <summary>
        /// Get the ExternalEvent of the ExternalEventHandler
        /// </summary>
        public ExternalEvent ParaEditEvent
        {
            get
            {
                return this.paraEditEvent;
            }

            set
            {
                this.paraEditEvent = value;
            }
        }

        public ElementEditorForm ElementEditorForm
        {
            get
            {
                return this.elementEditorForm;
            }

            set
            {
                this.elementEditorForm = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Change the value of the parameters to new value
        /// </summary>
        /// <param name="app"></param>
        public void Execute(UIApplication app)
        {
            Document document = app.ActiveUIDocument.Document;

            try
            {
                // index used to get value from corresponding index
                int stringCount = 0;
                int doubleCount = 0;
                int integerCount = 0;

                for (int i = 0; i < parameters.Count; i = i + 1)
                { 
                    Parameter parameter = parameters[i];

                    // change the value of a string paremeter
                    if (handlerMode[i] == MODE_STRING)
                    {
                        string newParaValue = newParaString[stringCount];

                        Transaction transaction = new Transaction(document, "Set Parameter");

                        transaction.Start();
                        parameter.Set(newParaValue);
                        transaction.Commit();

                        stringCount += 1;
                    }

                    // change the value of a double parameter
                    else if (handlerMode[i] == MODE_DOUBLE)
                    {
                        double newParaValue = newParaDouble[doubleCount];

                        Transaction transaction = new Transaction(document, "Set Parameter");

                        transaction.Start();
                        parameter.Set(newParaValue);
                        transaction.Commit();

                        doubleCount += 1;
                    }

                    // change the value of a integer parameter
                    else if (handlerMode[i] == MODE_INTEGER)
                    {
                        int newParaValue = newParaInteger[integerCount];

                        Transaction transaction = new Transaction(document, "Set Parameter");

                        transaction.Start();
                        parameter.Set(newParaValue);
                        transaction.Commit();

                        integerCount += 1;
                    }

                }

                // initialize all list that stored the parameter
                this.parameters = new List<Parameter>();
                this.newParaString = new List<string>();
                this.newParaDouble = new List<double>();
                this.newParaInteger = new List<int>();
            }

            catch (Exception ex)
            {
                TaskDialog.Show("Revit", ex.ToString());
            }

            finally
            {
                elementEditorForm.BringToFront();
                elementEditorForm.Activate();
            }
        }

        /// <summary>
        /// Get the name of External event handler
        /// </summary>
        /// <returns>Name of ExternalEventHandler</returns>
        public string GetName()
        { 
            return "ParaEditorEventHandler";
        }

        /// <summary>
        /// Change the value of a string parameter to a new string value
        /// </summary>
        /// <param name="parameter">Parameter that need to be edited</param>
        /// <param name="newParaValue">New value of the parameter</param>
        public void SetParameterAndRaise(Parameter parameter, string newParaValue)
        {
            if (parameters == null)
            {
                this.parameters = new List<Parameter>();
                this.newParaString = new List<string>();
                this.handlerMode = new List<string>();
            }

            // store the element parameter value that need to be changed
            parameters.Add(parameter);
            newParaString.Add(newParaValue);
            handlerMode.Add(MODE_STRING);

            SetForegroundWindow(Autodesk.Windows.ComponentManager.ApplicationWindow);

            // raise the event so the change can be executed
            this.paraEditEvent.Raise();
            
            // Try to force immediate execution of event by "jiggling" the mouse.
            // Adapted from Jo Ye, ACE DevBlog.
            // http://adndevblog.typepad.com/aec/2013/07/tricks-to-force-trigger-idling-event.html
            Cursor.Position = new System.Drawing.Point(Cursor.Position.X + 1, Cursor.Position.Y + 1);
            Cursor.Position = new System.Drawing.Point(Cursor.Position.X - 1, Cursor.Position.Y - 1);
        }

        /// <summary>
        /// Change the value of a double parameter to a new double value
        /// </summary>
        /// <param name="parameter">Parameter that need to be edited</param>
        /// <param name="newParaValue">New value of the parameter</param>
        public void SetParameterAndRaise(Parameter parameter, double newParaValue)
        {
            if (parameters == null)
            {
                this.parameters = new List<Parameter>();
                this.newParaDouble = new List<double>();
                this.handlerMode = new List<string>();
            }

            // store the element parameter value that need to be changed
            parameters.Add(parameter);
            newParaDouble.Add(newParaValue);
            handlerMode.Add(MODE_DOUBLE);

            SetForegroundWindow(Autodesk.Windows.ComponentManager.ApplicationWindow);

            // raise the event so the change can be executed
            this.paraEditEvent.Raise();

            // Try to force immediate execution of event by "jiggling" the mouse.
            // Adapted from Jo Ye, ACE DevBlog.
            // http://adndevblog.typepad.com/aec/2013/07/tricks-to-force-trigger-idling-event.html
            Cursor.Position = new System.Drawing.Point(Cursor.Position.X + 1, Cursor.Position.Y + 1);
            Cursor.Position = new System.Drawing.Point(Cursor.Position.X - 1, Cursor.Position.Y - 1);
        }

        /// <summary>
        /// Change the value of a integer parameter to a new integer value
        /// </summary>
        /// <param name="parameter">Parameter that need to be edited</param>
        /// <param name="newParaValue">New value of the parameter</param>
        public void SetParameterAndRaise(Parameter parameter, int newParaValue)
        {
            if (parameters == null)
            {
                this.parameters = new List<Parameter>();
                this.newParaInteger = new List<int>();
                this.handlerMode = new List<string>();
            }

            // store the element parameter value that need to be changed
            parameters.Add(parameter);
            newParaInteger.Add(newParaValue);
            handlerMode.Add(MODE_INTEGER);

            SetForegroundWindow(Autodesk.Windows.ComponentManager.ApplicationWindow);

            // raise the event so the change can be executed
            this.paraEditEvent.Raise();

            // Try to force immediate execution of event by "jiggling" the mouse.
            // Adapted from Jo Ye, ACE DevBlog.
            // http://adndevblog.typepad.com/aec/2013/07/tricks-to-force-trigger-idling-event.html
            Cursor.Position = new System.Drawing.Point(Cursor.Position.X + 1, Cursor.Position.Y + 1);
            Cursor.Position = new System.Drawing.Point(Cursor.Position.X - 1, Cursor.Position.Y - 1);
        }

        // Revit application window must be focused window for event to execute.
        // Use external Windows user32 method to set foreground window.
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);

        #endregion
    }
}
