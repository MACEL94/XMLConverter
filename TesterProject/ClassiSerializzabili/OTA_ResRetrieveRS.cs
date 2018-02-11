using System;
using System.Reflection;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Xml.Linq;

namespace BELHXmlTool
{
    [XmlRoot(ElementName = "OTA_ResRetrieveRS", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class OTA_ResRetrieveRS
    {
        [XmlElement(ElementName = "Success", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string Success { get; set; }
        public bool ShouldSerializeSuccess() { return this.Success != null; }
        [XmlElement(ElementName = "ReservationsList", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public ReservationsList ElementoReservationsList { get; set; }
        public bool ShouldSerializeElementoReservationsList() { return ElementoReservationsList != null; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        public bool ShouldSerializeXmlns() { return this.Xmlns != null; }
        [XmlAttribute(AttributeName = "PrimaryLangID")]
        public string PrimaryLangID { get; set; }
        public bool ShouldSerializePrimaryLangID() { return this.PrimaryLangID != null; }
        [XmlAttribute(AttributeName = "Target")]
        public string Target { get; set; }
        public bool ShouldSerializeTarget() { return this.Target != null; }
        [XmlIgnore]
        public DateTime? TimeStamp { get; set; }
        [XmlAttribute(AttributeName = "TimeStamp")]
        public string TimeStampString
        {
            get { return this.TimeStamp.Value.ToString("s"); }
            set { this.TimeStamp = DateTime.Parse(value); }
        }
        [XmlAttribute(AttributeName = "Version")]
        public decimal? Version { get; set; }
        public bool ShouldSerializeVersion() { return this.Version.HasValue; }
    }
    [XmlRoot(ElementName = "ReservationsList", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ReservationsList
    {
        [XmlElement(ElementName = "HotelReservation", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<HotelReservation> ListaElementoHotelReservation { get; set; } = new List<HotelReservation>();
        public bool ShouldSerializeListaElementoHotelReservation() { return ListaElementoHotelReservation != null && ListaElementoHotelReservation.Count > 0; }
    }
    [XmlRoot(ElementName = "HotelReservation", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class HotelReservation
    {
        [XmlElement(ElementName = "RoomStays", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomStays ElementoRoomStays { get; set; }
        public bool ShouldSerializeElementoRoomStays() { return ElementoRoomStays != null; }
        [XmlElement(ElementName = "Services", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Services ElementoServices { get; set; }
        public bool ShouldSerializeElementoServices() { return ElementoServices != null; }
        [XmlElement(ElementName = "ResGuests", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public ResGuests ElementoResGuests { get; set; }
        public bool ShouldSerializeElementoResGuests() { return ElementoResGuests != null; }
        [XmlElement(ElementName = "ResGlobalInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public ResGlobalInfo ElementoResGlobalInfo { get; set; }
        public bool ShouldSerializeElementoResGlobalInfo() { return ElementoResGlobalInfo != null; }
        [XmlElement(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TPA_Extensions ElementoTPA_Extensions { get; set; }
        public bool ShouldSerializeElementoTPA_Extensions() { return ElementoTPA_Extensions != null; }
        [XmlAttribute(AttributeName = "ResStatus")]
        public string ResStatus { get; set; }
        public bool ShouldSerializeResStatus() { return this.ResStatus != null; }
    }
    [XmlRoot(ElementName = "RoomStays", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomStays
    {
        [XmlElement(ElementName = "RoomStay", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<RoomStay> ListaElementoRoomStay { get; set; } = new List<RoomStay>();
        public bool ShouldSerializeListaElementoRoomStay() { return ListaElementoRoomStay != null && ListaElementoRoomStay.Count > 0; }
    }
    [XmlRoot(ElementName = "RoomStay", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomStay
    {
        [XmlElement(ElementName = "ResGuestRPHs", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public short? ResGuestRPHs { get; set; }
        public bool ShouldSerializeResGuestRPHs() { return this.ResGuestRPHs != null; }
        [XmlElement(ElementName = "RoomTypes", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomTypes ElementoRoomTypes { get; set; }
        public bool ShouldSerializeElementoRoomTypes() { return ElementoRoomTypes != null; }
        [XmlElement(ElementName = "RoomRates", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomRates ElementoRoomRates { get; set; }
        public bool ShouldSerializeElementoRoomRates() { return ElementoRoomRates != null; }
        [XmlElement(ElementName = "GuestCounts", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public GuestCounts ElementoGuestCounts { get; set; }
        public bool ShouldSerializeElementoGuestCounts() { return ElementoGuestCounts != null; }
        [XmlElement(ElementName = "Total", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Total ElementoTotal { get; set; }
        public bool ShouldSerializeElementoTotal() { return ElementoTotal != null; }
        [XmlElement(ElementName = "ServiceRPHs", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public ServiceRPHs ElementoServiceRPHs { get; set; }
        public bool ShouldSerializeElementoServiceRPHs() { return ElementoServiceRPHs != null; }
    }
    [XmlRoot(ElementName = "RoomTypes", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomTypes
    {
        [XmlElement(ElementName = "RoomType", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomType ElementoRoomType { get; set; }
        public bool ShouldSerializeElementoRoomType() { return ElementoRoomType != null; }
    }
    [XmlRoot(ElementName = "RoomType", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomType
    {
        [XmlAttribute(AttributeName = "RoomTypeCode")]
        public short? RoomTypeCode { get; set; }
        public bool ShouldSerializeRoomTypeCode() { return this.RoomTypeCode.HasValue; }
    }
    [XmlRoot(ElementName = "RoomRates", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomRates
    {
        [XmlElement(ElementName = "RoomRate", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<RoomRate> ListaElementoRoomRate { get; set; } = new List<RoomRate>();
        public bool ShouldSerializeListaElementoRoomRate() { return ListaElementoRoomRate != null && ListaElementoRoomRate.Count > 0; }
    }
    [XmlRoot(ElementName = "RoomRate", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomRate
    {
        [XmlElement(ElementName = "Rates", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Rates ElementoRates { get; set; }
        public bool ShouldSerializeElementoRates() { return ElementoRates != null; }
        [XmlAttribute(AttributeName = "RatePlanCode")]
        public short? RatePlanCode { get; set; }
        public bool ShouldSerializeRatePlanCode() { return this.RatePlanCode.HasValue; }
        [XmlIgnore]
        public DateTime? EffectiveDate { get; set; }
        [XmlAttribute(AttributeName = "EffectiveDate")]
        public string EffectiveDateString
        {
            get { return this.EffectiveDate.Value.ToString("yyyy-MM-dd"); }
            set { this.EffectiveDate = DateTime.Parse(value); }
        }
        [XmlAttribute(AttributeName = "NumberOfUnits")]
        public short? NumberOfUnits { get; set; }
        public bool ShouldSerializeNumberOfUnits() { return this.NumberOfUnits.HasValue; }
    }
    [XmlRoot(ElementName = "Rates", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Rates
    {
        [XmlElement(ElementName = "Rate", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Rate ElementoRate { get; set; }
        public bool ShouldSerializeElementoRate() { return ElementoRate != null; }
    }
    [XmlRoot(ElementName = "Rate", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Rate
    {
        [XmlElement(ElementName = "Base", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Base ElementoBase { get; set; }
        public bool ShouldSerializeElementoBase() { return ElementoBase != null; }
        [XmlElement(ElementName = "CancelPolicies", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public CancelPolicies ElementoCancelPolicies { get; set; }
        public bool ShouldSerializeElementoCancelPolicies() { return ElementoCancelPolicies != null; }
        [XmlElement(ElementName = "Discount", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Discount ElementoDiscount { get; set; }
        public bool ShouldSerializeElementoDiscount() { return ElementoDiscount != null; }
        [XmlElement(ElementName = "Total", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Total ElementoTotal { get; set; }
        public bool ShouldSerializeElementoTotal() { return ElementoTotal != null; }
    }
    [XmlRoot(ElementName = "Base", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Base
    {
        [XmlAttribute(AttributeName = "AmountAfterTax")]
        public decimal? AmountAfterTax { get; set; }
        public bool ShouldSerializeAmountAfterTax() { return this.AmountAfterTax.HasValue; }
        [XmlAttribute(AttributeName = "DecimalPlaces")]
        public short? DecimalPlaces { get; set; }
        public bool ShouldSerializeDecimalPlaces() { return this.DecimalPlaces.HasValue; }
        [XmlAttribute(AttributeName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
        public bool ShouldSerializeCurrencyCode() { return this.CurrencyCode != null; }
    }
    [XmlRoot(ElementName = "CancelPolicies", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class CancelPolicies
    {
        [XmlElement(ElementName = "CancelPenalty", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public CancelPenalty ElementoCancelPenalty { get; set; }
        public bool ShouldSerializeElementoCancelPenalty() { return ElementoCancelPenalty != null; }
    }
    [XmlRoot(ElementName = "CancelPenalty", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class CancelPenalty
    {
        [XmlAttribute(AttributeName = "PolicyCode")]
        public string PolicyCode { get; set; }
        public bool ShouldSerializePolicyCode() { return this.PolicyCode != null; }
    }
    [XmlRoot(ElementName = "Discount", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Discount
    {
        [XmlElement(ElementName = "DiscountReason", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public DiscountReason ElementoDiscountReason { get; set; }
        public bool ShouldSerializeElementoDiscountReason() { return ElementoDiscountReason != null; }
        [XmlAttribute(AttributeName = "AmountAfterTax")]
        public decimal? AmountAfterTax { get; set; }
        public bool ShouldSerializeAmountAfterTax() { return this.AmountAfterTax.HasValue; }
        [XmlAttribute(AttributeName = "DecimalPlaces")]
        public short? DecimalPlaces { get; set; }
        public bool ShouldSerializeDecimalPlaces() { return this.DecimalPlaces.HasValue; }
        [XmlAttribute(AttributeName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
        public bool ShouldSerializeCurrencyCode() { return this.CurrencyCode != null; }
    }
    [XmlRoot(ElementName = "DiscountReason", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class DiscountReason
    {
        [XmlElement(ElementName = "Text", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string Text { get; set; }
        public bool ShouldSerializeText() { return this.Text != null; }
    }
    [XmlRoot(ElementName = "Total", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Total
    {
        [XmlAttribute(AttributeName = "AmountAfterTax")]
        public decimal? AmountAfterTax { get; set; }
        public bool ShouldSerializeAmountAfterTax() { return this.AmountAfterTax.HasValue; }
        [XmlAttribute(AttributeName = "DecimalPlaces")]
        public short? DecimalPlaces { get; set; }
        public bool ShouldSerializeDecimalPlaces() { return this.DecimalPlaces.HasValue; }
        [XmlAttribute(AttributeName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
        public bool ShouldSerializeCurrencyCode() { return this.CurrencyCode != null; }
    }
    [XmlRoot(ElementName = "GuestCounts", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class GuestCounts
    {
        [XmlElement(ElementName = "GuestCount", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<GuestCount> ListaElementoGuestCount { get; set; } = new List<GuestCount>();
        public bool ShouldSerializeListaElementoGuestCount() { return ListaElementoGuestCount != null && ListaElementoGuestCount.Count > 0; }
    }
    [XmlRoot(ElementName = "GuestCount", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class GuestCount
    {
        [XmlAttribute(AttributeName = "AgeQualifyingCode")]
        public short? AgeQualifyingCode { get; set; }
        public bool ShouldSerializeAgeQualifyingCode() { return this.AgeQualifyingCode.HasValue; }
        [XmlAttribute(AttributeName = "Age")]
        public short? Age { get; set; }
        public bool ShouldSerializeAge() { return this.Age.HasValue; }
    }
    [XmlRoot(ElementName = "ServiceRPHs", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ServiceRPHs
    {
        [XmlElement(ElementName = "ServiceRPH", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public ServiceRPH ElementoServiceRPH { get; set; }
        public bool ShouldSerializeElementoServiceRPH() { return ElementoServiceRPH != null; }
    }
    [XmlRoot(ElementName = "ServiceRPH", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ServiceRPH
    {
        [XmlAttribute(AttributeName = "RPH")]
        public short? RPH { get; set; }
        public bool ShouldSerializeRPH() { return this.RPH.HasValue; }
        [XmlAttribute(AttributeName = "IsPerRoom")]
        public bool? IsPerRoom { get; set; }
        public bool ShouldSerializeIsPerRoom() { return this.IsPerRoom.HasValue; }
    }
    [XmlRoot(ElementName = "Services", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Services
    {
        [XmlElement(ElementName = "Service", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<Service> ListaElementoService { get; set; } = new List<Service>();
        public bool ShouldSerializeListaElementoService() { return ListaElementoService != null && ListaElementoService.Count > 0; }
    }
    [XmlRoot(ElementName = "Service", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Service
    {
        [XmlElement(ElementName = "Price", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<Price> ListaElementoPrice { get; set; } = new List<Price>();
        public bool ShouldSerializeListaElementoPrice() { return ListaElementoPrice != null && ListaElementoPrice.Count > 0; }
        [XmlElement(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TPA_Extensions ElementoTPA_Extensions { get; set; }
        public bool ShouldSerializeElementoTPA_Extensions() { return ElementoTPA_Extensions != null; }
        [XmlAttribute(AttributeName = "Type")]
        public short? Type { get; set; }
        public bool ShouldSerializeType() { return this.Type.HasValue; }
        [XmlAttribute(AttributeName = "ID")]
        public short? ID { get; set; }
        public bool ShouldSerializeID() { return this.ID.HasValue; }
        [XmlAttribute(AttributeName = "ServiceRPH")]
        public short? ServiceRPH { get; set; }
        public bool ShouldSerializeServiceRPH() { return this.ServiceRPH.HasValue; }
    }
    [XmlRoot(ElementName = "Price", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Price
    {
        [XmlElement(ElementName = "Base", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Base ElementoBase { get; set; }
        public bool ShouldSerializeElementoBase() { return ElementoBase != null; }
        [XmlElement(ElementName = "CancelPolicies", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public CancelPolicies ElementoCancelPolicies { get; set; }
        public bool ShouldSerializeElementoCancelPolicies() { return ElementoCancelPolicies != null; }
        [XmlElement(ElementName = "Discount", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Discount ElementoDiscount { get; set; }
        public bool ShouldSerializeElementoDiscount() { return ElementoDiscount != null; }
        [XmlElement(ElementName = "Total", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Total ElementoTotal { get; set; }
        public bool ShouldSerializeElementoTotal() { return ElementoTotal != null; }
        [XmlIgnore]
        public DateTime? EffectiveDate { get; set; }
        [XmlAttribute(AttributeName = "EffectiveDate")]
        public string EffectiveDateString
        {
            get { return this.EffectiveDate.Value.ToString("yyyy-MM-dd"); }
            set { this.EffectiveDate = DateTime.Parse(value); }
        }
        [XmlAttribute(AttributeName = "NumberOfUnits")]
        public short? NumberOfUnits { get; set; }
        public bool ShouldSerializeNumberOfUnits() { return this.NumberOfUnits.HasValue; }
    }
    [XmlRoot(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class TPA_Extensions
    {
        [XmlElement(ElementName = "ServiceDescription", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string ServiceDescription { get; set; }
        public bool ShouldSerializeServiceDescription() { return this.ServiceDescription != null; }
        [XmlElement(ElementName = "IATACode", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public int? IATACode { get; set; }
        public bool ShouldSerializeIATACode() { return this.IATACode != null; }
        [XmlElement(ElementName = "TaxCode", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string TaxCode { get; set; }
        public bool ShouldSerializeTaxCode() { return this.TaxCode != null; }
        [XmlElement(ElementName = "VATCode", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public int? VATCode { get; set; }
        public bool ShouldSerializeVATCode() { return this.VATCode != null; }
        [XmlElement(ElementName = "Newsletter", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public bool? Newsletter { get; set; }
        public bool ShouldSerializeNewsletter() { return this.Newsletter != null; }
        [XmlElement(ElementName = "ReservationNotes", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string ReservationNotes { get; set; }
        public bool ShouldSerializeReservationNotes() { return this.ReservationNotes != null; }
        [XmlIgnore]
        public DateTime? OptionExpiringDate { get; set; }
        [XmlElement(ElementName = "OptionExpiringDate", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string OptionExpiringDateString
        {
            get { return this.OptionExpiringDate.Value.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { this.OptionExpiringDate = DateTime.Parse(value); }
        }
        public bool ShouldSerializeOptionExpiringDate() { return this.OptionExpiringDate != null; }
        [XmlElement(ElementName = "Form", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<Form> ListaElementoForm { get; set; } = new List<Form>();
        public bool ShouldSerializeListaElementoForm() { return ListaElementoForm != null && ListaElementoForm.Count > 0; }
        [XmlElement(ElementName = "SearchParams", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public SearchParams ElementoSearchParams { get; set; }
        public bool ShouldSerializeElementoSearchParams() { return ElementoSearchParams != null; }
        [XmlElement(ElementName = "Layout", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Layout ElementoLayout { get; set; }
        public bool ShouldSerializeElementoLayout() { return ElementoLayout != null; }
        [XmlElement(ElementName = "ACR", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public ACR ElementoACR { get; set; }
        public bool ShouldSerializeElementoACR() { return ElementoACR != null; }
        [XmlElement(ElementName = "Category", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Category ElementoCategory { get; set; }
        public bool ShouldSerializeElementoCategory() { return ElementoCategory != null; }
        [XmlElement(ElementName = "Ecommerce", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Ecommerce ElementoEcommerce { get; set; }
        public bool ShouldSerializeElementoEcommerce() { return ElementoEcommerce != null; }
    }
    [XmlRoot(ElementName = "ResGuests", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ResGuests
    {
        [XmlElement(ElementName = "ResGuest", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public ResGuest ElementoResGuest { get; set; }
        public bool ShouldSerializeElementoResGuest() { return ElementoResGuest != null; }
    }
    [XmlRoot(ElementName = "ResGuest", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ResGuest
    {
        [XmlElement(ElementName = "Profiles", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Profiles ElementoProfiles { get; set; }
        public bool ShouldSerializeElementoProfiles() { return ElementoProfiles != null; }
        [XmlAttribute(AttributeName = "ResGuestRPH")]
        public short? ResGuestRPH { get; set; }
        public bool ShouldSerializeResGuestRPH() { return this.ResGuestRPH.HasValue; }
    }
    [XmlRoot(ElementName = "Profiles", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Profiles
    {
        [XmlElement(ElementName = "ProfileInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<ProfileInfo> ListaElementoProfileInfo { get; set; } = new List<ProfileInfo>();
        public bool ShouldSerializeListaElementoProfileInfo() { return ListaElementoProfileInfo != null && ListaElementoProfileInfo.Count > 0; }
    }
    [XmlRoot(ElementName = "ProfileInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ProfileInfo
    {
        [XmlElement(ElementName = "Profile", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Profile ElementoProfile { get; set; }
        public bool ShouldSerializeElementoProfile() { return ElementoProfile != null; }
    }
    [XmlRoot(ElementName = "Profile", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Profile
    {
        [XmlElement(ElementName = "Customer", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Customer ElementoCustomer { get; set; }
        public bool ShouldSerializeElementoCustomer() { return ElementoCustomer != null; }
        [XmlElement(ElementName = "UserID", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public UserID ElementoUserID { get; set; }
        public bool ShouldSerializeElementoUserID() { return ElementoUserID != null; }
        [XmlElement(ElementName = "CompanyInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public CompanyInfo ElementoCompanyInfo { get; set; }
        public bool ShouldSerializeElementoCompanyInfo() { return ElementoCompanyInfo != null; }
        [XmlElement(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TPA_Extensions ElementoTPA_Extensions { get; set; }
        public bool ShouldSerializeElementoTPA_Extensions() { return ElementoTPA_Extensions != null; }
        [XmlAttribute(AttributeName = "ProfileType")]
        public short? ProfileType { get; set; }
        public bool ShouldSerializeProfileType() { return this.ProfileType.HasValue; }
    }
    [XmlRoot(ElementName = "Customer", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Customer
    {
        [XmlElement(ElementName = "Email", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string Email { get; set; }
        public bool ShouldSerializeEmail() { return this.Email != null; }
        [XmlElement(ElementName = "PersonName", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public PersonName ElementoPersonName { get; set; }
        public bool ShouldSerializeElementoPersonName() { return ElementoPersonName != null; }
        [XmlElement(ElementName = "Address", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Address ElementoAddress { get; set; }
        public bool ShouldSerializeElementoAddress() { return ElementoAddress != null; }
        [XmlElement(ElementName = "Telephone", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Telephone ElementoTelephone { get; set; }
        public bool ShouldSerializeElementoTelephone() { return ElementoTelephone != null; }
        [XmlElement(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TPA_Extensions ElementoTPA_Extensions { get; set; }
        public bool ShouldSerializeElementoTPA_Extensions() { return ElementoTPA_Extensions != null; }
    }
    [XmlRoot(ElementName = "PersonName", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class PersonName
    {
        [XmlElement(ElementName = "GivenName", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string GivenName { get; set; }
        public bool ShouldSerializeGivenName() { return this.GivenName != null; }
        [XmlElement(ElementName = "Surname", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string Surname { get; set; }
        public bool ShouldSerializeSurname() { return this.Surname != null; }
    }
    [XmlRoot(ElementName = "ResGlobalInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ResGlobalInfo
    {
        [XmlElement(ElementName = "TimeSpan", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TimeSpan ElementoTimeSpan { get; set; }
        public bool ShouldSerializeElementoTimeSpan() { return ElementoTimeSpan != null; }
        [XmlElement(ElementName = "Guarantee", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Guarantee ElementoGuarantee { get; set; }
        public bool ShouldSerializeElementoGuarantee() { return ElementoGuarantee != null; }
        [XmlElement(ElementName = "Total", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Total ElementoTotal { get; set; }
        public bool ShouldSerializeElementoTotal() { return ElementoTotal != null; }
        [XmlElement(ElementName = "HotelReservationIDs", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public HotelReservationIDs ElementoHotelReservationIDs { get; set; }
        public bool ShouldSerializeElementoHotelReservationIDs() { return ElementoHotelReservationIDs != null; }
        [XmlElement(ElementName = "Profiles", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Profiles ElementoProfiles { get; set; }
        public bool ShouldSerializeElementoProfiles() { return ElementoProfiles != null; }
    }
    [XmlRoot(ElementName = "TimeSpan", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class TimeSpan
    {
        [XmlIgnore]
        public DateTime? Start { get; set; }
        [XmlAttribute(AttributeName = "Start")]
        public string StartString
        {
            get { return this.Start.Value.ToString("yyyy-MM-dd"); }
            set { this.Start = DateTime.Parse(value); }
        }
        [XmlIgnore]
        public DateTime? End { get; set; }
        [XmlAttribute(AttributeName = "End")]
        public string EndString
        {
            get { return this.End.Value.ToString("yyyy-MM-dd"); }
            set { this.End = DateTime.Parse(value); }
        }
    }
    [XmlRoot(ElementName = "Guarantee", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Guarantee
    {
        [XmlElement(ElementName = "GuaranteeDescription", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public GuaranteeDescription ElementoGuaranteeDescription { get; set; }
        public bool ShouldSerializeElementoGuaranteeDescription() { return ElementoGuaranteeDescription != null; }
        [XmlElement(ElementName = "GuaranteesAccepted", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public GuaranteesAccepted ElementoGuaranteesAccepted { get; set; }
        public bool ShouldSerializeElementoGuaranteesAccepted() { return ElementoGuaranteesAccepted != null; }
    }
    [XmlRoot(ElementName = "GuaranteeDescription", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class GuaranteeDescription
    {
        [XmlElement(ElementName = "Text", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string Text { get; set; }
        public bool ShouldSerializeText() { return this.Text != null; }
    }
    [XmlRoot(ElementName = "HotelReservationIDs", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class HotelReservationIDs
    {
        [XmlElement(ElementName = "HotelReservationID", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public HotelReservationID ElementoHotelReservationID { get; set; }
        public bool ShouldSerializeElementoHotelReservationID() { return ElementoHotelReservationID != null; }
    }
    [XmlRoot(ElementName = "HotelReservationID", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class HotelReservationID
    {
        [XmlIgnore]
        public DateTime? ResID_Date { get; set; }
        [XmlAttribute(AttributeName = "ResID_Date")]
        public string ResID_DateString
        {
            get { return this.ResID_Date.Value.ToString("s"); }
            set { this.ResID_Date = DateTime.Parse(value); }
        }
        [XmlAttribute(AttributeName = "ResID_Value")]
        public int? ResID_Value { get; set; }
        public bool ShouldSerializeResID_Value() { return this.ResID_Value.HasValue; }
    }
    [XmlRoot(ElementName = "UserID", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class UserID
    {
        [XmlAttribute(AttributeName = "Type")]
        public short? Type { get; set; }
        public bool ShouldSerializeType() { return this.Type.HasValue; }
        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }
        public bool ShouldSerializeID() { return this.ID != null; }
    }
    [XmlRoot(ElementName = "CompanyInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class CompanyInfo
    {
        [XmlElement(ElementName = "Email", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string Email { get; set; }
        public bool ShouldSerializeEmail() { return this.Email != null; }
        [XmlElement(ElementName = "CompanyName", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string CompanyName { get; set; }
        public bool ShouldSerializeCompanyName() { return this.CompanyName != null; }
        [XmlElement(ElementName = "URL", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string URL { get; set; }
        public bool ShouldSerializeURL() { return this.URL != null; }
        [XmlElement(ElementName = "ContactPerson", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public ContactPerson ElementoContactPerson { get; set; }
        public bool ShouldSerializeElementoContactPerson() { return ElementoContactPerson != null; }
        [XmlElement(ElementName = "Telephone", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<Telephone> ListaElementoTelephone { get; set; } = new List<Telephone>();
        public bool ShouldSerializeListaElementoTelephone() { return ListaElementoTelephone != null && ListaElementoTelephone.Count > 0; }
        [XmlElement(ElementName = "AddressInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public AddressInfo ElementoAddressInfo { get; set; }
        public bool ShouldSerializeElementoAddressInfo() { return ElementoAddressInfo != null; }
    }
    [XmlRoot(ElementName = "ContactPerson", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ContactPerson
    {
        [XmlElement(ElementName = "PersonName", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public PersonName ElementoPersonName { get; set; }
        public bool ShouldSerializeElementoPersonName() { return ElementoPersonName != null; }
    }
    [XmlRoot(ElementName = "Telephone", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Telephone
    {
        [XmlAttribute(AttributeName = "PhoneTechType")]
        public short? PhoneTechType { get; set; }
        public bool ShouldSerializePhoneTechType() { return this.PhoneTechType.HasValue; }
        [XmlAttribute(AttributeName = "PhoneNumber")]
        public int? PhoneNumber { get; set; }
        public bool ShouldSerializePhoneNumber() { return this.PhoneNumber.HasValue; }
    }
    [XmlRoot(ElementName = "AddressInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class AddressInfo
    {
        [XmlElement(ElementName = "AddressLine", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string AddressLine { get; set; }
        public bool ShouldSerializeAddressLine() { return this.AddressLine != null; }
        [XmlElement(ElementName = "CityName", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string CityName { get; set; }
        public bool ShouldSerializeCityName() { return this.CityName != null; }
        [XmlElement(ElementName = "PostalCode", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public int? PostalCode { get; set; }
        public bool ShouldSerializePostalCode() { return this.PostalCode != null; }
        [XmlElement(ElementName = "StateProv", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string StateProv { get; set; }
        public bool ShouldSerializeStateProv() { return this.StateProv != null; }
        [XmlElement(ElementName = "CountryName", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string CountryName { get; set; }
        public bool ShouldSerializeCountryName() { return this.CountryName != null; }
    }
    [XmlRoot(ElementName = "Form", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Form
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        public bool ShouldSerializeName() { return this.Name != null; }
        [XmlText]
        public string ValoreForm { get; set; }
    }
    [XmlRoot(ElementName = "Address", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Address
    {
        [XmlElement(ElementName = "CountryName", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string CountryName { get; set; }
        public bool ShouldSerializeCountryName() { return this.CountryName != null; }
    }
    [XmlRoot(ElementName = "SearchParams", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class SearchParams
    {
        [XmlElement(ElementName = "param1", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string Param1 { get; set; }
        public bool ShouldSerializeParam1() { return this.Param1 != null; }
    }
    [XmlRoot(ElementName = "Layout", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Layout
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
        public bool ShouldSerializeCode() { return this.Code != null; }
        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }
        public bool ShouldSerializeType() { return this.Type != null; }
    }
    [XmlRoot(ElementName = "ACR", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ACR
    {
        [XmlElement(ElementName = "Tags", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Tags ElementoTags { get; set; }
        public bool ShouldSerializeElementoTags() { return ElementoTags != null; }
        [XmlElement(ElementName = "Operator", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Operator ElementoOperator { get; set; }
        public bool ShouldSerializeElementoOperator() { return ElementoOperator != null; }
    }
    [XmlRoot(ElementName = "Tags", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Tags
    {
        [XmlElement(ElementName = "Tag", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<Tag> ListaElementoTag { get; set; } = new List<Tag>();
        public bool ShouldSerializeListaElementoTag() { return ListaElementoTag != null && ListaElementoTag.Count > 0; }
    }
    [XmlRoot(ElementName = "Tag", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Tag
    {
        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }
        public bool ShouldSerializeType() { return this.Type != null; }
        [XmlText]
        public string ValoreTag { get; set; }
    }
    [XmlRoot(ElementName = "Operator", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Operator
    {
        [XmlElement(ElementName = "Surname", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string Surname { get; set; }
        public bool ShouldSerializeSurname() { return this.Surname != null; }
        [XmlElement(ElementName = "FirstName", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string FirstName { get; set; }
        public bool ShouldSerializeFirstName() { return this.FirstName != null; }
    }
    [XmlRoot(ElementName = "Category", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Category
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
        public bool ShouldSerializeCode() { return this.Code != null; }
        [XmlAttribute(AttributeName = "ID")]
        public short? ID { get; set; }
        public bool ShouldSerializeID() { return this.ID.HasValue; }
    }
    [XmlRoot(ElementName = "GuaranteesAccepted", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class GuaranteesAccepted
    {
        [XmlElement(ElementName = "GuaranteeAccepted", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public GuaranteeAccepted ElementoGuaranteeAccepted { get; set; }
        public bool ShouldSerializeElementoGuaranteeAccepted() { return ElementoGuaranteeAccepted != null; }
    }
    [XmlRoot(ElementName = "GuaranteeAccepted", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class GuaranteeAccepted
    {
        [XmlElement(ElementName = "PaymentCard", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public PaymentCard ElementoPaymentCard { get; set; }
        public bool ShouldSerializeElementoPaymentCard() { return ElementoPaymentCard != null; }
    }
    [XmlRoot(ElementName = "PaymentCard", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class PaymentCard
    {
        [XmlElement(ElementName = "CardHolderName", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string CardHolderName { get; set; }
        public bool ShouldSerializeCardHolderName() { return this.CardHolderName != null; }
        [XmlElement(ElementName = "CardNumber", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public CardNumber ElementoCardNumber { get; set; }
        public bool ShouldSerializeElementoCardNumber() { return ElementoCardNumber != null; }
        [XmlAttribute(AttributeName = "CardCode")]
        public string CardCode { get; set; }
        public bool ShouldSerializeCardCode() { return this.CardCode != null; }
        [XmlAttribute(AttributeName = "ExpireDate")]
        public short? ExpireDate { get; set; }
        public bool ShouldSerializeExpireDate() { return this.ExpireDate.HasValue; }
    }
    [XmlRoot(ElementName = "CardNumber", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class CardNumber
    {
        [XmlElement(ElementName = "PlainText", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public decimal? PlainText { get; set; }
        public bool ShouldSerializePlainText() { return this.PlainText != null; }
    }
    [XmlRoot(ElementName = "Ecommerce", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Ecommerce
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
        public bool ShouldSerializeCode() { return this.Code != null; }
    }
}
