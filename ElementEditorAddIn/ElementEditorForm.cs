using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ElementEditorAddIn
{
    /// <summary>
    /// Windows Form for viewing and editing common parameters of element in given categories
    /// </summary>
    public partial class ElementEditorForm : System.Windows.Forms.Form
    {
        #region parameters

        private ExternalCommandData commandData;           // commandDate
        private List<ElementId> categoriesElementIds;      // Id of All Element in selected categories
        private ParaEditEventHandler handler;

        #endregion

        #region Public Methods

        // create ElementEditor form
        public ElementEditorForm(
            ExternalCommandData commandData,
            ParaEditEventHandler handler)
        {
            InitializeComponent();

            // inialize the data
            this.commandData = commandData;
            this.handler = handler;
            categoriesElementIds = new List<ElementId>();

            // find and show all categories in current project
            List<Category> categories = GetAllCategories();

            foreach (Category category in categories)
            {
                eleListBox.Items.Add(category.Name);
            }

            InitializeUI();
        }

        #endregion

        #region Windows Forms Event Methods

        /// <summary>
        /// Select Category Button onclick event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectCategoryButton_Click(object sender, EventArgs e)
        {
            Selection selection = GetSelection();

            // 2019-2-1
            // get selected categories from the ListBox
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.listbox.selecteditems?view=netframework-4.7.2
            ListBox.SelectedObjectCollection categoriesCollctor = eleListBox.SelectedItems;

            List<string> selectedCategories = new List<string>();
            List<ElementId> selectedElementIds = new List<ElementId>();

            foreach (String selectedCategory in categoriesCollctor)
            {
                selectedCategories.Add(selectedCategory);
            }

            // get all element in the selected categories
            List<Element> elements = GetAllElementInCategories(selectedCategories);

            foreach (Element element in elements)
            {
                selectedElementIds.Add(element.Id);
            }

            categoriesElementIds = selectedElementIds;

            // clear the old data in combo box
            paraComboBox.Items.Clear();
            paraComboBox.ResetText();

            // get common parameter
            List<string> parameters = GetCommmonParameterStringInCategories(selectedCategories);

            foreach (string parameter in parameters)
            {
                paraComboBox.Items.Add(parameter);
            }

            // Set up corresponding UI
            ShowSelectCategory();
        }

        /// <summary>
        /// search button onclick event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchButton_Click(object sender, EventArgs e)
        {
            // reset the current selected element when filter so it still filtered
            // all element in the categories
            Selection selection = GetSelection();
            selection.SetElementIds(categoriesElementIds);

            object parameterNameObject = paraComboBox.SelectedItem;
            object filterOptionObject = filterOptionComboBox.SelectedItem;

            if ((parameterNameObject != null) && (filterOptionObject != null))
            {
                // get the name, type of the selected parameter, as well as filter option and filter value
                string parameterName = paraComboBox.SelectedItem.ToString();
                string parameterType = paraTypeTextBox.Text;
                string filterOption = filterOptionComboBox.SelectedItem.ToString();
                string filterValue = filterValueTextBox.Text;

                // check whether the selected search option is valid
                // parameter with double and integer as storage type cannot set the search value to string.Empty
                if ( (parameterType != "String") && (parameterType != "ElementId") && (filterValue == ""))
                {
                    TaskDialog.Show("Revit", "Need to Input a Value to filter the parameters");
                }

                // parameter with string must use string (substring filter) as search option
                else if ((parameterType == "String") && (filterOption != "String"))
                {
                    TaskDialog.Show("Revit", "Need to Select a valid filter operator");
                }

                // parameter with ElementId must use string (substring filter) as search option
                else if ((parameterType == "ElementId") && (filterOption != "String"))
                {
                    TaskDialog.Show("Revit", "Need to Select a valid filter operator");
                }

                // parameter with string and parameter must use string (substring filter) as search option
                else if ( (parameterType != "String") && (parameterType != "ElementId") && (filterOption == "String"))
                {
                    TaskDialog.Show("Revit", "Need to Select a valid filter operator");
                }

                else
                {
                    try
                    {
                        // search element that satisfy the condition
                        List<ElementId> elementIds = FilterElements(categoriesElementIds, parameterName, filterOption, filterValue);
                        
                        // hight light the element that satisfied the condition
                        selection.SetElementIds(elementIds);
                    }

                    catch (Exception ex)
                    {
                        TaskDialog.Show("Revit", ex.ToString());
                    }
                }
            }

            else
            {
                TaskDialog.Show("Revit", "Need to select an filter Operatior");
            }
        }

        /// <summary>
        /// edit button onclick event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void paraEditButton_Click(object sender, EventArgs e)
        {
            try
            {
                // get the current selected element
                Selection selection = GetSelection();
                ICollection<ElementId> collection = selection.GetElementIds();
                List<ElementId> selectedElementIds = new List<ElementId>();

                // get the current selected parameter name and type, as well as the new parameter value
                string parameterName = paraComboBox.SelectedItem.ToString();
                string parameterType = paraTypeTextBox.Text;
                string newParameterValue = paraEditTextBox.Text;

                foreach (ElementId selectedElementId in collection)
                {
                    selectedElementIds.Add(selectedElementId);
                }

                // edit the parameter
                EditParameters(
                    selectedElementIds, 
                    parameterName, 
                    parameterType, 
                    newParameterValue);
            }

            catch (Exception ex)
            {
                TaskDialog.Show("Revit", ex.ToString());
            }
        }

        /// <summary>
        /// Reset selected element when filter option changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filterOptionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // When filter option change, selected all element with selected categories
            Selection selection = GetSelection();
            selection.SetElementIds(categoriesElementIds);
        }

        /// <summary>
        /// Event when user change the selected parameter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void paraComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // when change selected parameter, initialize the value in the filter and edit UI element

            // reset parameter type TextBox
            paraTypeTextBox.Text = "";

            // reset filter option ComboBox
            filterOptionComboBox.Items.Clear();
            filterOptionComboBox.ResetText();

            // reset filter value TextBox
            filterValueTextBox.Clear();

            // reset parameter edit TextBox
            paraEditTextBox.Clear();

            if (categoriesElementIds.Count > 0)
            {
                // Get parameterType
                ElementId eleId = categoriesElementIds.First();
                string ParameterName = paraComboBox.SelectedItem.ToString();
                string parameterType = GetParameterType(eleId, ParameterName);
                paraTypeTextBox.Text = parameterType;

                // set the filter option according to the parameter type
                if (parameterType == "String")
                {
                    filterOptionComboBox.Items.Add("String");
                }

                else if (parameterType == "Double")
                {
                    filterOptionComboBox.Items.Add("=");
                    filterOptionComboBox.Items.Add("!=");
                    filterOptionComboBox.Items.Add(">");
                    filterOptionComboBox.Items.Add("<");
                }

                else if (parameterType == "Integer")
                {
                    filterOptionComboBox.Items.Add("=");
                    filterOptionComboBox.Items.Add("!=");
                    filterOptionComboBox.Items.Add(">");
                    filterOptionComboBox.Items.Add("<");
                }

                else if (parameterType == "ElementId")
                {
                    filterOptionComboBox.Items.Add("String");
                }
            }

            ShowParaFilter();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Return the database level Document
        /// </summary>
        /// <returns>database level Document</returns>
        private Document GetDocument()
        {
            return commandData.Application.ActiveUIDocument.Document;
        }

        /// <summary>
        /// Return the Currently selected element
        /// </summary>
        /// <returns> Currently selected element</returns>
        private Selection GetSelection()
        {
            return commandData.Application.ActiveUIDocument.Selection;
        }

        /// <summary>
        /// Get all elements represent a real construction object in the revit document
        /// </summary>
        /// <returns>All elements represent a real construction object</returns>
        public List<Element> GetAllElements()
        {
            Document document = GetDocument();

            List<Element> elements = new List<Element>();

            // filter out all element Type element
            FilteredElementCollector collector =
                new FilteredElementCollector(document).WhereElementIsNotElementType();

            // get all element that has material Quantities
            foreach (Element element in collector)
            {
                if (element.Category != null &&
                    element.Category.HasMaterialQuantities)
                {
                    elements.Add(element);
                }
            }

            return elements;
        }

        /// <summary>
        /// Get all categories in the project
        /// </summary>
        /// <returns>List of all categories in the project</returns>
        public List<Category> GetAllCategories()
        {
            List<Element> elements = GetAllElements();

            List<Category> categories = new List<Category>();
            List<string> categoryNames = new List<string>();

            // get all categories from the elements in the project
            foreach (Element element in elements)
            {
                if (!categoryNames.Contains(element.Category.Name))
                {
                    categoryNames.Add(element.Category.Name);
                    categories.Add(element.Category);
                }
            }

            return categories;
        }

        /// <summary>
        /// Get the category names of a list of categories
        /// </summary>
        /// <param name="categories">List of categoires</param>
        /// <returns>List of categoires name</returns>
        private List<string> CategoriesToString(List<Category> categories)
        {
            List<string> stringCatetories = new List<string>();

            foreach (Category category in categories)
            {
                if (category != null && category.HasMaterialQuantities)
                {
                    stringCatetories.Add(category.Name);
                }
            }

            stringCatetories.Sort();

            return stringCatetories;
        }

        /// <summary>
        /// find all elements with the selected categories
        /// </summary>
        /// <param name="categories">Names of selected categories</param>
        /// <returns>All element with the selected categories</returns>
        public List<Element> GetAllElementInCategories(List<string> categories)
        {
            List<Element> elements = new List<Element>();

            foreach (string category in categories)
            {
                List<Element> categoryElements = GetAllElementInCategory(category);

                foreach (Element categoryElement in categoryElements)
                {
                    elements.Add(categoryElement);
                }
            }

            return elements;
        }

        /// <summary>
        /// Get all elements with a selected category
        /// </summary>
        /// <param name="category">Name of a selected category</param>
        /// <returns>All element with the selected category</returns>
        public List<Element> GetAllElementInCategory(string category)
        {
            List<Element> allElements = GetAllElements();
            List<Element> elements = new List<Element>();

            foreach (Element element in allElements)
            {
                if (element.Category.Name == category)
                {
                    elements.Add(element);
                }
            }

            return elements;
        }

        /// <summary>
        /// Get common parameter in the elements of selected categories
        /// </summary>
        /// <param name="categories">selected categories</param>
        /// <returns>Name of the common parameters of elements in the selected categories</returns>
        public List<string> GetCommmonParameterStringInCategories(List<string> categories)
        {
            List<Element> elements = GetAllElementInCategories(categories);

            List<string> commonParameters = new List<string>();

            if (!(elements.Count == 0))
            {

                // get all name of parameters
                ParameterSet firstParameters = elements.First().Parameters;

                foreach (Parameter parameter in firstParameters)
                {
                    string parameterName = parameter.Definition.Name;

                    commonParameters.Add(parameterName);
                }

                // find the parameters in every elemnts in the selected categories
                foreach (Element element in elements)
                {
                    ParameterSet parameters = element.Parameters;

                    List<string> tmpCommonParameters = new List<string>(commonParameters);

                    commonParameters.Clear();

                    // find all parameters that contained in the elements that we have checked, 
                    // filter out the parameter that doesn't appear in some elements
                    foreach (Parameter parameter in parameters)
                    {
                        if (tmpCommonParameters.Contains(parameter.Definition.Name))
                        {
                            commonParameters.Add(parameter.Definition.Name);
                        }
                    }
                }
            }

            // remove repeated elements
            List<string> distinctcommonParameters = commonParameters.Distinct().ToList();

            // sort the list
            distinctcommonParameters.Sort();

            return distinctcommonParameters;
        }

        /// <summary>
        /// Get the storage type of a given parameter
        /// </summary>
        /// <param name="element">an element with the given parameters</param>
        /// <param name="ParameterName">name of the parameter</param>
        /// <returns></returns>
        public string GetParameterType(Element element, string ParameterName)
        {
            string paraType = "NoType";

            // get the parameter of the element that has the given parameter name 
            ICollection<Parameter> parameters = element.GetParameters(ParameterName);

            if (parameters.Count > 0)
            {
                Parameter parameter = parameters.First();

                // find the storage type
                switch (parameter.StorageType)
                {
                    case StorageType.String:
                        paraType = "String";
                        break;

                    case StorageType.Double:
                        paraType = "Double";
                        break;

                    case StorageType.Integer:
                        paraType = "Integer";
                        break;

                    case StorageType.ElementId:
                        paraType = "ElementId";
                        break;

                    default:
                        break;
                }
            }

            return paraType;
        }

        /// <summary>
        /// Get the type of parameter
        /// </summary>
        /// <param name="elementId">Id of the element with the parameter</param>
        /// <param name="ParameterName">Name of the parameter</param>
        /// <returns>storage type of the parameter in string</returns>
        public string GetParameterType(ElementId elementId, string ParameterName)
        {
            string paraType = "NoType";

            // get element from elementId
            Document document = GetDocument();
            Element element = document.GetElement(elementId);

            ICollection<Parameter> parameters = element.GetParameters(ParameterName);

            // get the storage type of the parameter
            if (parameters.Count > 0)
            {
                Parameter parameter = parameters.First();

                switch (parameter.StorageType)
                {
                    case StorageType.String:
                        paraType = "String";
                        break;

                    case StorageType.Double:
                        paraType = "Double";
                        break;

                    case StorageType.Integer:
                        paraType = "Integer";
                        break;

                    case StorageType.ElementId:
                        paraType = "ElementId";
                        break;

                    default:
                        break;
                }
            }

            return paraType;
        }

        /// <summary>
        /// Find the elements that the given parameters satisfied the given condition (filter option + filter value)
        /// </summary>
        /// <param name="selectedElementIds">ElementId of the element that need filted</param>
        /// <param name="parameterName">name of given parameter</param>
        /// <param name="filterOption">filter option</param>
        /// <param name="filterValue">filter value</param>
        /// <returns></returns>
    public List<ElementId> FilterElements(
        List<ElementId> selectedElementIds,
        string parameterName,
        string filterOption,
        string filterValue)
        {
            Document document = GetDocument();

            List<ElementId> filteredElementIds = new List<ElementId>();

            if (!(selectedElementIds.Count == 0))
            {
                // find the parameter storage type of the parameters
                ElementId firstElementId = selectedElementIds.First();
                Element firstElement = document.GetElement(firstElementId);

                ICollection<Parameter> firstParameters = firstElement.GetParameters(parameterName);

                if (firstParameters.Count > 0)
                {
                    Parameter firstParameter = firstParameters.First();

                    // find all element that has parameter value satisfied the input filtered condition,
                    // the elements will be filted with different methods according to the storagetype
                    // of the parameter
                    switch (firstParameter.StorageType)
                    {
                        // parameter storage type is string
                        case StorageType.String:

                            if (filterOption == "String")
                            {
                                foreach (ElementId elementId in selectedElementIds)
                                {
                                    if (IsFilteredString(elementId, parameterName, filterValue))
                                    {
                                        filteredElementIds.Add(elementId);
                                    }
                                }
                            }

                            break;

                        // parameter storage type is double
                        case StorageType.Double:

                            if (filterOption != "" + "String")
                            {
                                foreach (ElementId elementId in selectedElementIds)
                                {
                                    if (IsFilteredDouble(elementId, parameterName, filterOption, filterValue))
                                    {
                                        filteredElementIds.Add(elementId);
                                    }
                                }
                            }

                            break;

                        // parameter storage type is integer
                        case StorageType.Integer:

                            if (filterOption != "" + "String")
                            {
                                foreach (ElementId elementId in selectedElementIds)
                                {
                                    if (IsFilteredInteger(elementId, parameterName, filterOption, filterValue))
                                    {
                                        filteredElementIds.Add(elementId);
                                    }
                                }
                            }

                            break;
                        
                        // parameter storage type is elementId
                        case StorageType.ElementId:

                            if (filterOption == "String")
                            {
                                foreach (ElementId elementId in selectedElementIds)
                                {
                                    if (IsFilteredElementId(elementId, parameterName, filterValue))
                                    {
                                        filteredElementIds.Add(elementId);
                                    }
                                }
                            }

                            break;

                        default:
                            break;
                    }
                }
            }

            return filteredElementIds;
        }

        /// <summary>
        /// Check whether a string parameter of a element contain the input substring, thus 
        /// satify the filter condition
        /// </summary>
        /// <param name="eleId">Id of the element</param>
        /// <param name="parameterName">Name of the parameter</param>
        /// <param name="filterValue">substring used for filter</param>
        /// <returns>Whether the element satisfy the filter condition</returns>
        private bool IsFilteredString(ElementId eleId, string parameterName, string filterValue)
        {
            bool isFiltered = false;

            Document document = GetDocument();
            Element element = document.GetElement(eleId);

            // get the parameter of this element
            ICollection<Parameter> eleParameters = element.GetParameters(parameterName);

            if (eleParameters.Count != 0)
            {
                // get the value of parameter
                Parameter parameter = eleParameters.First();

                if (parameter.StorageType == StorageType.String)
                {
                    // check whether the parameter value of the element satisfied the condition
                    string parameterValue = parameter.AsString();

                    // if the input substring is an empty string, then no element should be filtered out
                    if (filterValue == "")
                    {
                        isFiltered = true;
                    }

                    else if (parameterValue != null)
                    {
                        // find whether the string contain the substring
                        if (parameterValue.Contains(filterValue))
                        {
                            isFiltered = true;
                        }
                    }

                }
            }

            return isFiltered;
        }

        /// <summary>
        /// Check whether the value of a double parameter of a element satisfy the given condition
        /// </summary>
        /// <param name="eleId">Id of the element</param>
        /// <param name="parameterName">Name of the parameter</param>
        /// <param name="filterOption">Operator of the filter condition</param>
        /// <param name="filterValue">Value of the filter condition</param>
        /// <returns>Whether the element satisfy the filter condition</returns>
        private bool IsFilteredDouble(
            ElementId eleId,
            string parameterName,
            string filterOption,
            string filterValue)
        {
            bool isFiltered = false;

            Document document = GetDocument();
            Element element = document.GetElement(eleId);

            // get the parameter of this element
            ICollection<Parameter> eleParameters = element.GetParameters(parameterName);

            if (eleParameters.Count != 0)
            {
                // get the value of parameter
                Parameter parameter = eleParameters.First();

                if (parameter.StorageType == StorageType.Double)
                {
                    // check whether the parameter value of the element satisfied the condition
                    double paraValue = parameter.AsDouble();
                    double doubleFilterValue = Convert.ToDouble(filterValue);

                    // filter operator is "="
                    if (filterOption == "=")
                    {
                        if (paraValue == doubleFilterValue)
                        {
                            return true;
                        }
                    }

                    // filter operator is "!="
                    else if (filterOption == "!=")
                    {
                        if (paraValue != doubleFilterValue)
                        {
                            return true;
                        }
                    }

                    // filter operator is ">"
                    else if (filterOption == ">")
                    {
                        if (paraValue > doubleFilterValue)
                        {
                            return true;
                        }
                    }

                    // filter operator is ">"
                    else if (filterOption == "<")
                    {
                        if (paraValue < doubleFilterValue)
                        {
                            return true;
                        }
                    }
                }
            }

            return isFiltered;
        }

        /// <summary>
        /// Check whether the value of a integer parameter of a element satisfy the given condition
        /// </summary>
        /// <param name="eleId">Id of the element</param>
        /// <param name="parameterName">Name of the parameter</param>
        /// <param name="filterOption">Operator of the filter condition</param>
        /// <param name="filterValue">Value of the filter condition</param>
        /// <returns>Whether the element satisfy the filter condition</returns>
        private bool IsFilteredInteger(
            ElementId eleId,
            string parameterName,
            string filterOption,
            string filterValue)
        {
            bool isFiltered = false;

            Document document = GetDocument();
            Element element = document.GetElement(eleId);

            // get the parameter of this element
            ICollection<Parameter> eleParameters = element.GetParameters(parameterName);

            if (eleParameters.Count != 0)
            {
                // get the value of parameter
                Parameter parameter = eleParameters.First();

                if (parameter.StorageType == StorageType.Double)
                {
                    // check whether the parameter value of the element satisfied the condition
                    int paraValue = parameter.AsInteger();
                    int doubleFilterValue = Convert.ToInt32(filterValue);

                    // filter operator is "="
                    if (filterOption == "=")
                    {
                        if (paraValue == doubleFilterValue)
                        {
                            return true;
                        }
                    }

                    // filter operator is "!="
                    else if (filterOption == "!=")
                    {
                        if (paraValue != doubleFilterValue)
                        {
                            return true;
                        }
                    }

                    // filter operator is ">"
                    else if (filterOption == ">")
                    {
                        if (paraValue > doubleFilterValue)
                        {
                            return true;
                        }
                    }

                    // filter operator is ">"
                    else if (filterOption == "<")
                    {
                        if (paraValue < doubleFilterValue)
                        {
                            return true;
                        }
                    }
                }
            }

            return isFiltered;
        }

        /// <summary>
        /// Check whether a elementId parameter of a element contain the input substring, thus 
        /// satify the filter condition
        /// </summary>
        /// <param name="eleId">Id of the element</param>
        /// <param name="parameterName">Name of the parameter</param>
        /// <param name="filterValue">substring used for filter</param>
        /// <returns>Whether the element satisfy the filter condition</returns>
        private bool IsFilteredElementId(ElementId eleId, string parameterName, string filterValue)
        {
            bool isFiltered = false;

            Document document = GetDocument();
            Element element = document.GetElement(eleId);

            // get the parameter of this element
            ICollection<Parameter> eleParameters = element.GetParameters(parameterName);

            if (eleParameters.Count != 0)
            {
                // get the value of parameter
                Parameter parameter = eleParameters.First();

                if (parameter.StorageType == StorageType.ElementId)
                {
                    ElementId id = parameter.AsElementId();

                    // if the input substring is an empty string, then no element should be filtered out
                    if (filterValue == "")
                    {
                        isFiltered = true;
                    }

                    else if (id != null)
                    {
                        // find whether the elementId as a string contain the substring
                        string ParameterValueString = id.IntegerValue.ToString();

                        if (ParameterValueString.Contains(filterValue))
                        {
                            isFiltered = true;
                        }
                    }

                }
            }

            return isFiltered;
        }

        /// <summary>
        /// change the value of a parameter of all given elements to a provided value
        /// </summary>
        /// <param name="selectedEleIds">Id of element that need parameter changed</param>
        /// <param name="paraName">Name of the parameter</param>
        /// <param name="paraType">Type of the parameter</param>
        /// <param name="newParaValue">the new value of the parameter</param>
        private void EditParameters(
            List<ElementId> selectedEleIds,
            string paraName,
            string paraType,
            string newParaValue)
        {
            // if the storage type of the parameter is string
            if (paraType == "String")
            {
                foreach (ElementId selectedEleId in selectedEleIds)
                {
                    bool result = EditParameterString(selectedEleId, paraName, newParaValue);
                    
                    if (result == false)
                    {
                        break;
                    }
                }
            }

            // if the storage type of the parameter is double
            else if (paraType == "Double")
            {
                foreach (ElementId selectedEleId in selectedEleIds)
                {
                    bool result = EditParameterDouble(selectedEleId, paraName, newParaValue);

                    if (result == false)
                    {
                        break;
                    }
                }
            }

            // if the storage type of parameter is interger
            else if (paraType == "Integer")
            {
                foreach (ElementId selectedEleId in selectedEleIds)
                {
                    bool result = EditParameterInteger(selectedEleId, paraName, newParaValue);

                    if (result == false)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Change the selected string parameter of an element to a new value
        /// </summary>
        /// <param name="selectedEleId">Id of the element</param>
        /// <param name="paraName">Name of the parameter</param>
        /// <param name="newParaValue">New string value of the parameter</param>
        private bool EditParameterString(
            ElementId selectedEleId,
            string paraName,
            string newParaValue)
        {
            bool result = true;

            Document document = GetDocument();
            Element element = document.GetElement(selectedEleId);

            // get the selected string parameter in the element
            ICollection<Parameter> eleParameters = element.GetParameters(paraName);

            if (eleParameters.Count != 0)
            {
                Parameter parameter = eleParameters.First();

                // check whether the parameter can be modified
                if (!(parameter.UserModifiable))
                { 
                    // check the type of the parameter
                    if (parameter.StorageType == StorageType.String)
                    {
                        // Edit the parameter use the ExternalEventHandler
                        handler.SetParameterAndRaise(parameter, newParaValue);
                    }

                    else
                    {
                        TaskDialog.Show("Revit", "Error: Parameter Type Is Not String");

                        result = false;
                    }
                }

                else
                {
                    TaskDialog.Show("Revit", "Error: Parameter Is Not UserModifiable");

                    result = false;
                }
            }

            else
            {
                TaskDialog.Show("Revit", "Error: Parameter Not Found");

                result = false;
            }

            return result;
        }

        /// <summary>
        /// Change the selected double parameter of an element to a new value
        /// </summary>
        /// <param name="selectedElementId">Id of the element</param>
        /// <param name="parameterName">Name of the parameter</param>
        /// <param name="newParameterValue">New double value of the parameter</param>
        private bool EditParameterDouble(
            ElementId selectedElementId,
            string parameterName,
            string newParameterValue)
        {
            bool result = true;

            Document document = GetDocument();
            Element element = document.GetElement(selectedElementId);

            // get the selected double parameter in the element
            ICollection<Parameter> eleParameters = element.GetParameters(parameterName);

            if (eleParameters.Count != 0)
            {
                Parameter parameter = eleParameters.First();

                // check whether the parameter can be modified
                if (!(parameter.UserModifiable))
                {
                    // check the type of the parameter
                    if (parameter.StorageType == StorageType.Double)
                    {
                        // Edit the parameter use the ExternalEventHandler
                        double parameterNameDouble = Convert.ToDouble(newParameterValue);
                        handler.SetParameterAndRaise(parameter, parameterNameDouble);
                    }

                    else
                    {
                        TaskDialog.Show("Revit", "Error: Parameter Type Is Not Double");
                        result = false;
                    }
                }

                else
                {
                    TaskDialog.Show("Revit", "Error: Parameter Is Not UserModifiable");
                    result = false;
                }
            }

            else
            {
                TaskDialog.Show("Revit", "Error: Parameter Not Found");
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Change the selected integer parameter of an element to a new value
        /// </summary>
        /// <param name="selectedElementId">Id of the element</param>
        /// <param name="parameterName">Name of the parameter</param>
        /// <param name="newParameterValue">New integer value of the parameter</param>
        private bool EditParameterInteger(
            ElementId selectedElementId,
            string parameterName,
            string newParameterValue)
        {
            bool result = true;

            Document document = GetDocument();
            Element element = document.GetElement(selectedElementId);

            // get the selected integer parameter in the element
            ICollection<Parameter> eleParameters = element.GetParameters(parameterName);

            if (eleParameters.Count != 0)
            {
                Parameter parameter = eleParameters.First();

                // check whether the parameter can be modified
                if (!(parameter.UserModifiable))
                {
                    // check the type of the parameter
                    if (parameter.StorageType == StorageType.Integer)
                    {
                        // Edit the parameter use the ExternalEventHandler
                        int parameterNameInt = Convert.ToInt32(newParameterValue);
                        handler.SetParameterAndRaise(parameter, parameterNameInt);
                    }

                    else
                    {
                        TaskDialog.Show("Revit", "Error: Parameter Type is not Integer");
                        result = false;
                    }
                }

                else
                {
                    TaskDialog.Show("Revit", "Error: Parameter Is Not UserModifiable");
                    result = false;
                }
            }

            else
            {
                TaskDialog.Show("Revit", "Error: Parameter Not Found");
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Set the ElementHost to show the active View of the revit
        /// </summary>
        private void SetElementHost()
        {
            Document document = GetDocument();
            elementHost.Child = new PreviewControl(document, document.ActiveView.Id);
        }

        /// <summary>
        /// initialize the UI of the Element Editor Form, used when the form created
        /// </summary>
        private void InitializeUI()
        {
            SetElementHost();
            splitContainer2.Panel2Collapsed = true;
        }

        /// <summary>
        /// Show UI of viewing and edit elements in the selected categories, used when the 
        /// select category button is clicked
        /// </summary>
        private void ShowSelectCategory()
        {
            splitContainer2.Panel2Collapsed = false;

            filterOptionComboBox.Enabled = false;
            filterValueTextBox.Enabled = false;
            searchButton.Enabled = false;
            paraEditTextBox.Enabled = false;
            searchButton.Enabled = false;

            // Show all elements in the selected categories
            Selection selection = GetSelection();
            selection.SetElementIds(categoriesElementIds);
        }

        /// <summary>
        /// Enable UI of the filter and edit elements, used when the user selected a parameter
        /// </summary>
        private void ShowParaFilter()
        {
            // Enable the filter of the seleted parameter
            filterOptionComboBox.Enabled = true;
            filterValueTextBox.Enabled = true;
            searchButton.Enabled = true;

            // ElementId cannot be edited, so disable the element Edit button and TextBox
            if (paraTypeTextBox.Text == "ElementId")
            {
                paraEditTextBox.Enabled = false;
                paraEditButton.Enabled = false;
            }

            // otherwise, enable the Parameter edit button and TextBox
            else
            {
                paraEditTextBox.Enabled = true;
                paraEditButton.Enabled = true;
            }
        }

        /*
        // A test method used to see if it possible to find to true value of some parameter
        // with ElementId as value
        private void GetEleIdParaOption(string paraName)
        {
            TaskDialog.Show("Revit", "Test started");

            Selection selection = GetSelection();
            Document document = GetDocument();

            ICollection<ElementId> elementIds = selection.GetElementIds();

            if (elementIds.Count > 0)
            {
                ElementId elementId = elementIds.First();
                Element element = document.GetElement(elementId);

                ICollection<Parameter> parameters = element.GetParameters(paraName);

                if (parameters.Count > 0)
                {
                    Parameter parameter = parameters.First();
                    ElementId paraEleId = parameter.AsElementId();

                    TaskDialog.Show("Revit", paraEleId.ToString());

                    //Element paraEle = document.GetElement(paraEleId);

                    //TaskDialog.Show("Revit", paraEle.Name);

                    if (paraEle.Category != null)
                    {
                        string paraCalName = paraEle.Category.Name;

                        TaskDialog.Show("Revit", paraCalName);
                    }
                }
            }

            //Document document = GetDocument();
            //Element paraValueElement = document.GetElement(paraId);     // element id of input parameter

            //TaskDialog.Show("Revit", paraValueElement.Name);            
        }

                // testing
                try
                {
                    object parameterNameObject = paraComboBox.SelectedItem;

                    if (parameterNameObject != null)
                    {
                        string ParameterName = paraComboBox.SelectedItem.ToString();
                        GetEleIdParaOption(ParameterName);
                    }
                }

                catch (Exception ex)
                {
                    TaskDialog.Show("Revit", ex.ToString());
                }
                // testing
        */

        /// <summary>
        /// find all elements with the selected categories
        /// </summary>
        /// <param name="selectedCategories">The selected categories</param>
        /// <returns>All element with the selected categories </returns>
        /// 
        /*
        public List<Element> GetCategoriesElements(List<Category> selectedCategories)
        {
            List<Element> allElements = new List<Element>();
            List<string> stringCategories = CategoriesToString(selectedCategories);

            foreach (Element element in allElements)
            {
                if (stringCategories.Contains(element.Category.Name))
                {
                    allElements.Add(element);
                }
            }

            return allElements;
        
        }
        */

        #endregion
    }
}
