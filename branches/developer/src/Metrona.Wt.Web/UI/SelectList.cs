// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace Metrona.Wt.Web.UI
{
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "This is a shipped API")]
    public class SelectList : MultiSelectList
    {
        public SelectList(IEnumerable items)
            : this(items, selectedValue: null)
        {
        }

        public SelectList(IEnumerable items, object selectedValue)
            : this(items, dataValueField: null, dataTextField: null, selectedValue: selectedValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SelectList class by using the specified items for the list,
        /// the selected value, and the disabled values.
        /// </summary>
        /// <param name="items">The items used to build each <see cref="Metrona.Wt.Web.UI.SelectListItem"/> of the list.</param>
        /// <param name="selectedValue">The selected value. Used to match the Selected property of the corresponding
        /// <see cref="Metrona.Wt.Web.UI.SelectListItem"/>.</param>
        /// <param name="disabledValues">The disabled values. Used to match the Disabled property of the corresponding
        /// <see cref="Metrona.Wt.Web.UI.SelectListItem"/>.</param>
        public SelectList(IEnumerable items, object selectedValue, IEnumerable disabledValues)
            : this(items,
                   dataValueField: null,
                   dataTextField: null,
                   selectedValue: selectedValue,
                   disabledValues: disabledValues)
        {
        }

        public SelectList(IEnumerable items, string dataValueField, string dataTextField)
            : this(items, dataValueField, dataTextField, selectedValue: null)
        {
        }

        public SelectList(IEnumerable items, string dataValueField, string dataTextField, object selectedValue)
            : base(items, dataValueField, dataTextField, ToEnumerable(selectedValue))
        {
            this.SelectedValue = selectedValue;
        }

        /// <summary>
        /// Initializes a new instance of the SelectList class by using the specified items for the list,
        /// the data value field, the data text field, the data group field, and the selected value.
        /// </summary>
        /// <param name="items">The items used to build each <see cref="Metrona.Wt.Web.UI.SelectListItem"/> of the list.</param>
        /// <param name="dataValueField">The data value field. Used to match the Value property of the corresponding
        /// <see cref="Metrona.Wt.Web.UI.SelectListItem"/>.</param>
        /// <param name="dataTextField">The data text field. Used to match the Text property of the corresponding
        /// <see cref="Metrona.Wt.Web.UI.SelectListItem"/>.</param>
        /// <param name="dataGroupField">The data group field. Used to match the Group property of the corresponding
        /// <see cref="Metrona.Wt.Web.UI.SelectListItem"/>.</param>
        /// <param name="selectedValue">The selected value. Used to match the Selected property of the corresponding
        /// <see cref="Metrona.Wt.Web.UI.SelectListItem"/>.</param>
        public SelectList(IEnumerable items,
                          string dataValueField,
                          string dataTextField,
                          string dataGroupField,
                          object selectedValue)
            : base(items, dataValueField, dataTextField, dataGroupField, ToEnumerable(selectedValue))
        {
            this.SelectedValue = selectedValue;
        }

        /// <summary>
        /// Initializes a new instance of the SelectList class by using the specified items for the list,
        /// the data value field, the data text field, the selected value, and the disabled values.
        /// </summary>
        /// <param name="items">The items used to build each <see cref="Metrona.Wt.Web.UI.SelectListItem"/> of the list.</param>
        /// <param name="dataValueField">The data value field. Used to match the Value property of the corresponding
        /// <see cref="Metrona.Wt.Web.UI.SelectListItem"/>.</param>
        /// <param name="dataTextField">The data text field. Used to match the Text property of the corresponding
        /// <see cref="Metrona.Wt.Web.UI.SelectListItem"/>.</param>
        /// <param name="selectedValue">The selected value. Used to match the Selected property of the corresponding
        /// <see cref="Metrona.Wt.Web.UI.SelectListItem"/>.</param>
        /// <param name="disabledValues">The disabled values. Used to match the Disabled property of the corresponding
        /// <see cref="Metrona.Wt.Web.UI.SelectListItem"/>.</param>
        public SelectList(IEnumerable items,
                          string dataValueField,
                          string dataTextField,
                          object selectedValue,
                          IEnumerable disabledValues)
            : base(items, dataValueField, dataTextField, ToEnumerable(selectedValue), disabledValues)
        {
            this.SelectedValue = selectedValue;
        }

        /// <summary>
        /// Initializes a new instance of the SelectList class by using the specified items for the list,
        /// the data value field, the data text field, the data group field, the selected value, and the disabled
        /// values.
        /// </summary>
        /// <param name="items">The items used to build each <see cref="Metrona.Wt.Web.UI.SelectListItem"/> of the list.</param>
        /// <param name="dataValueField">The data value field. Used to match the Value property of the corresponding
        /// <see cref="Metrona.Wt.Web.UI.SelectListItem"/>.</param>
        /// <param name="dataTextField">The data text field. Used to match the Text property of the corresponding
        /// <see cref="Metrona.Wt.Web.UI.SelectListItem"/>.</param>
        /// <param name="dataGroupField">The data group field. Used to match the Group property of the corresponding
        /// <see cref="Metrona.Wt.Web.UI.SelectListItem"/>.</param>
        /// <param name="selectedValue">The selected value. Used to match the Selected property of the corresponding
        /// <see cref="Metrona.Wt.Web.UI.SelectListItem"/>.</param>
        /// <param name="disabledValues">The disabled values. Used to match the Disabled property of the corresponding
        /// <see cref="Metrona.Wt.Web.UI.SelectListItem"/>.</param>
        public SelectList(IEnumerable items,
                          string dataValueField,
                          string dataTextField,
                          string dataGroupField,
                          object selectedValue,
                          IEnumerable disabledValues)
            : base(items, dataValueField, dataTextField, dataGroupField, ToEnumerable(selectedValue), disabledValues)
        {
            this.SelectedValue = selectedValue;
        }

        /// <summary>
        /// Initializes a new instance of the SelectList class by using the specified items for the list,
        /// the data value field, the data text field, the data group field. the selected value, the disabled values,
        /// and the disabled groups.
        /// </summary>
        /// <param name="items">The items used to build each <see cref="Metrona.Wt.Web.UI.SelectListItem"/> of the list.</param>
        /// <param name="dataValueField">The data value field. Used to match the Value property of the corresponding
        /// <see cref="Metrona.Wt.Web.UI.SelectListItem"/>.</param>
        /// <param name="dataTextField">The data text field. Used to match the Text property of the corresponding
        /// <see cref="Metrona.Wt.Web.UI.SelectListItem"/>.</param>
        /// <param name="dataGroupField">The data group field. Used to match the Group property of the corresponding
        /// <see cref="Metrona.Wt.Web.UI.SelectListItem"/>.</param>
        /// <param name="selectedValue">The selected value. Used to match the Selected property of the corresponding
        /// <see cref="Metrona.Wt.Web.UI.SelectListItem"/>.</param>
        /// <param name="disabledValues">The disabled values. Used to match the Disabled property of the corresponding
        /// <see cref="Metrona.Wt.Web.UI.SelectListItem"/>.</param>
        /// <param name="disabledGroups">The disabled groups. Used to match the Disabled property of the corresponding
        /// <see cref="SelectListGroup"/>.</param>
        public SelectList(IEnumerable items,
                          string dataValueField,
                          string dataTextField,
                          string dataGroupField,
                          object selectedValue,
                          IEnumerable disabledValues,
                          IEnumerable disabledGroups)
            : base(items,
                   dataValueField,
                   dataTextField,
                   dataGroupField,
                   ToEnumerable(selectedValue),
                   disabledValues,
                   disabledGroups)
        {
            this.SelectedValue = selectedValue;
        }

        public object SelectedValue { get; private set; }

        private static IEnumerable ToEnumerable(object selectedValue)
        {
            return (selectedValue != null) ? new object[] { selectedValue } : null;
        }
    }
}