//------------------------------------------------------------------------
// Generated by Neo on 8/25/2007 3:02:01 PM for INDEPENDENCE\mrose
//
// This file was autogenerated but you can (and are meant to) edit it as 
// it will not be overwritten unless explicitly requested.
//------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Expergent;
using Neo.Core;

namespace CRMSample.Models
{

    #region Status

    /// <summary>Basic entity object representing a database table.</summary>
    /// <remarks>
    ///   <para>Database Path: CRMSample.Status.</para>
    /// </remarks>
    public partial class Status
    {
        /// <summary>Use this method to establish the default value of a property upon creation.</summary>
        [LifecycleCreate()]
        protected void SetupAfterCreate()
        {
            //pass
        }

        /// <summary>
        /// Invokes the method.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameters">The parameters.</param>
        public override void InvokeMethod(string methodName, params object[] parameters)
        {
            //Use this to implement rule actions.
            base.InvokeMethod(methodName, parameters);
            return;
        }

        /// <summary>
        /// Extends the GetProperty method to allow the developer to add custom properties.
        /// </summary>
        /// <param name="propName">Name of the prop.</param>
        /// <returns>The property value.</returns>
        protected override object ExtendGetProperty(string propName)
        {
            return base.GetProperty(propName);
        }

        /// <summary>
        /// Extends the SetProperty method to allow the developer to add custom properties.
        /// </summary>
        /// <param name="propName">Name of the prop.</param>
        /// <param name="propValue">The prop value.</param>
        protected override void ExtendSetProperty(string propName, object propValue)
        {
            base.SetProperty(propName, propValue);
        }

        /// <summary>
        /// Extends the MakeFacts method to create custom facts.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="parent">The parent.</param>
        protected override void ExtendMakeFacts(List<WME> list, string parent)
        {
        }

        /// <summary>Use this method to return a meaningful string representation of your object.</summary>
        public override string ToString()
        {
            return base.ToString();
        }
    }

    #endregion

    #region StatusSurrogate

    /// <summary>Represents an entity object disconnected from the database.</summary>
    /// <remarks>
    ///   <para>Database Path: CRMSample.Status.</para>
    /// </remarks>
    [Serializable]
    public class StatusSurrogate : StatusSurrogateBase
    {
        private StringBuilder _sb;

        /// <summary>Default constructor.</summary>
        public StatusSurrogate()
        {
            _sb = new StringBuilder();
        }

        /// <summary>Constructor for assembling a surrogate from an entity object.</summary>
        /// <param name="entity">The Entity Object.</param>
        public StatusSurrogate(Status entity) : base(entity)
        {
            _sb = new StringBuilder();
        }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsValid()
        {
            return _sb.Length == 0;
        }


        /// <summary>
        /// Gets the validation message.
        /// </summary>
        /// <value>The validation message.</value>
        public override String ValidationMessage
        {
            get { return _sb.ToString(); }
        }


        /// <summary>Use this method to return a meaningful string representation of your object.</summary>
        public override string ToString()
        {
            return base.ToString();
        }
    }

    #endregion
}
