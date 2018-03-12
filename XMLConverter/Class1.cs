using System;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Xml.Linq;

namespace BELHXmlTool.asdasd
{
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope
    {
        [XmlElement(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Header ElementoHeader { get; set; }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ShouldSerializeElementoHeader() { return ElementoHeader != null; }
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Body ElementoBody { get; set; }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ShouldSerializeElementoBody() { return ElementoBody != null; }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Namespaces
        {
            get
            {
                return new XmlSerializerNamespaces(new XmlQualifiedName[]
                {
                    new XmlQualifiedName("env", "http://schemas.xmlsoap.org/soap/envelope/"),
                    new XmlQualifiedName("wsa", "http://www.w3.org/2005/08/addressing"),
                });
            }
            set
            {
            }
        }
    }
    [XmlRoot(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Header
    {
        [XmlElement(ElementName = "MessageID", Namespace = "http://www.w3.org/2005/08/addressing")]
        public string MessageID { get; set; }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ShouldSerializeMessageID() { return this.MessageID != null; }
        [XmlElement(ElementName = "ReplyTo", Namespace = "http://www.w3.org/2005/08/addressing")]
        public ReplyTo ElementoReplyTo { get; set; }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ShouldSerializeElementoReplyTo() { return ElementoReplyTo != null; }
    }
    [XmlRoot(ElementName = "ReplyTo", Namespace = "http://www.w3.org/2005/08/addressing")]
    public class ReplyTo
    {
        [XmlElement(ElementName = "Address", Namespace = "http://www.w3.org/2005/08/addressing")]
        public string Address { get; set; }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ShouldSerializeAddress() { return this.Address != null; }
        [XmlElement(ElementName = "ReferenceParameters", Namespace = "http://www.w3.org/2005/08/addressing")]
        public ReferenceParameters ElementoReferenceParameters { get; set; }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ShouldSerializeElementoReferenceParameters() { return ElementoReferenceParameters != null; }
    }
    [XmlRoot(ElementName = "ReferenceParameters", Namespace = "http://www.w3.org/2005/08/addressing")]
    public class ReferenceParameters
    {
        [XmlElement(ElementName = "tracking", Namespace = "http://xmlns.oracle.com/sca/tracking/1.0")]
        public Tracking ElementoTracking { get; set; }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ShouldSerializeElementoTracking() { return ElementoTracking != null; }
    }
    [XmlRoot(ElementName = "tracking", Namespace = "http://xmlns.oracle.com/sca/tracking/1.0")]
    public class Tracking
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Namespaces
        {
            get
            {
                return new XmlSerializerNamespaces(new XmlQualifiedName[]
                {
                    new XmlQualifiedName("instra", "http://xmlns.oracle.com/sca/tracking/1.0"),
                });
            }
            set
            {
            }
        }
        [XmlText]
        public string ValoreTracking { get; set; }
    }
    [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Body
    {
        [XmlElement(ElementName = "GetUserIDByNameResponse", Namespace = "http://tempuri.org/")]
        public GetUserIDByNameResponse ElementoGetUserIDByNameResponse { get; set; }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ShouldSerializeElementoGetUserIDByNameResponse() { return ElementoGetUserIDByNameResponse != null; }
    }
    [XmlRoot(ElementName = "GetUserIDByNameResponse", Namespace = "http://tempuri.org/")]
    public class GetUserIDByNameResponse
    {
        [XmlElement(ElementName = "ReturnValue", Namespace = "http://tempuri.org/")]
        public short? ReturnValue { get; set; }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ShouldSerializeReturnValue() { return this.ReturnValue != null; }
        [XmlElement(ElementName = "GetUserIDByNameResult", Namespace = "http://tempuri.org/")]
        public short? GetUserIDByNameResult { get; set; }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ShouldSerializeGetUserIDByNameResult() { return this.GetUserIDByNameResult != null; }
        [XmlElement(ElementName = "Error", Namespace = "http://tempuri.org/")]
        public Error ElementoError { get; set; }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ShouldSerializeElementoError() { return ElementoError != null; }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Namespaces
        {
            get
            {
                return new XmlSerializerNamespaces(new XmlQualifiedName[]
                {
                    new XmlQualifiedName(String.Empty, "http://tempuri.org/"),
                });
            }
            set
            {
            }
        }
    }
    [XmlRoot(ElementName = "Error", Namespace = "http://tempuri.org/")]
    public class Error
    {
        [XmlElement(ElementName = "ErrorCode", Namespace = "http://tempuri.org/")]
        public short? ErrorCode { get; set; }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ShouldSerializeErrorCode() { return this.ErrorCode != null; }
        [XmlElement(ElementName = "ErrorMessage", Namespace = "http://tempuri.org/")]
        public string ErrorMessage { get; set; }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ShouldSerializeErrorMessage() { return this.ErrorMessage != null; }
    }
}
