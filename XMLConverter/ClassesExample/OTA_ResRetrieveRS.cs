using System;using System.Collections.Generic;using System.Xml;using System.Xml.Serialization;using System.Linq;using System.Xml.Linq;namespace BELHXmlTool{	[XmlRoot(ElementName="OTA_ResRetrieveRS", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class OTA_ResRetrieveRS	{		[XmlElement(ElementName = "Success", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string Success { get; set; }		[XmlElement(ElementName="ReservationsList", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public ReservationsList ElementoReservationsList { get; set; } = new ReservationsList();		[XmlAttribute(AttributeName="xmlns")]		public string Xmlns { get; set; }		[XmlAttribute(AttributeName="PrimaryLangID")]		public string PrimaryLangID { get; set; }		[XmlAttribute(AttributeName="Target")]		public string Target { get; set; }		[XmlIgnore]		public DateTime TimeStamp { get; set; }		[XmlAttribute(AttributeName="TimeStamp")]		public string TimeStampString		{				get { return this.TimeStamp.ToString("s"); }				set { this.TimeStamp = DateTime.Parse(value); }		}		[XmlAttribute(AttributeName="Version")]		public decimal Version { get; set; }	}	[XmlRoot(ElementName="ReservationsList", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class ReservationsList	{		[XmlElement(ElementName="HotelReservation", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public List<HotelReservation> ListaElementoHotelReservation { get; set; } = new List<HotelReservation>();	}	[XmlRoot(ElementName="HotelReservation", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class HotelReservation	{		[XmlElement(ElementName="RoomStays", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public RoomStays ElementoRoomStays { get; set; } = new RoomStays();		[XmlElement(ElementName="Services", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Services ElementoServices { get; set; } = new Services();		[XmlElement(ElementName="ResGuests", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public ResGuests ElementoResGuests { get; set; } = new ResGuests();		[XmlElement(ElementName="ResGlobalInfo", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public ResGlobalInfo ElementoResGlobalInfo { get; set; } = new ResGlobalInfo();		[XmlElement(ElementName="TPA_Extensions", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public TPA_Extensions ElementoTPA_Extensions { get; set; } = new TPA_Extensions();		[XmlAttribute(AttributeName="ResStatus")]		public string ResStatus { get; set; }	}	[XmlRoot(ElementName="RoomStays", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class RoomStays	{		[XmlElement(ElementName="RoomStay", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public List<RoomStay> ListaElementoRoomStay { get; set; } = new List<RoomStay>();	}	[XmlRoot(ElementName="RoomStay", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class RoomStay	{		[XmlElement(ElementName = "ResGuestRPHs", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public decimal ResGuestRPHs { get; set; }		[XmlElement(ElementName="RoomTypes", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public RoomTypes ElementoRoomTypes { get; set; } = new RoomTypes();		[XmlElement(ElementName="RoomRates", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public RoomRates ElementoRoomRates { get; set; } = new RoomRates();		[XmlElement(ElementName="GuestCounts", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public GuestCounts ElementoGuestCounts { get; set; } = new GuestCounts();		[XmlElement(ElementName="Total", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Total ElementoTotal { get; set; } = new Total();		[XmlElement(ElementName="ServiceRPHs", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public ServiceRPHs ElementoServiceRPHs { get; set; } = new ServiceRPHs();	}	[XmlRoot(ElementName="RoomTypes", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class RoomTypes	{		[XmlElement(ElementName="RoomType", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public RoomType ElementoRoomType { get; set; } = new RoomType();	}	[XmlRoot(ElementName="RoomType", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class RoomType	{		[XmlAttribute(AttributeName="RoomTypeCode")]		public string RoomTypeCode { get; set; }	}	[XmlRoot(ElementName="RoomRates", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class RoomRates	{		[XmlElement(ElementName="RoomRate", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public List<RoomRate> ListaElementoRoomRate { get; set; } = new List<RoomRate>();	}	[XmlRoot(ElementName="RoomRate", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class RoomRate	{		[XmlElement(ElementName="Rates", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Rates ElementoRates { get; set; } = new Rates();		[XmlAttribute(AttributeName="RatePlanCode")]		public string RatePlanCode { get; set; }		[XmlIgnore]		public DateTime EffectiveDate { get; set; }		[XmlAttribute(AttributeName="EffectiveDate")]		public string EffectiveDateString		{				get { return this.EffectiveDate.ToString("yyyy-MM-dd"); }				set { this.EffectiveDate = DateTime.Parse(value); }		}		[XmlAttribute(AttributeName="NumberOfUnits")]		public decimal NumberOfUnits { get; set; }	}	[XmlRoot(ElementName="Rates", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class Rates	{		[XmlElement(ElementName="Rate", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Rate ElementoRate { get; set; } = new Rate();	}	[XmlRoot(ElementName="Rate", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class Rate	{		[XmlElement(ElementName="Base", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Base ElementoBase { get; set; } = new Base();		[XmlElement(ElementName="CancelPolicies", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public CancelPolicies ElementoCancelPolicies { get; set; } = new CancelPolicies();		[XmlElement(ElementName="Discount", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Discount ElementoDiscount { get; set; } = new Discount();		[XmlElement(ElementName="Total", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Total ElementoTotal { get; set; } = new Total();	}	[XmlRoot(ElementName="Base", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class Base	{		[XmlAttribute(AttributeName="AmountAfterTax")]		public decimal AmountAfterTax { get; set; }		[XmlAttribute(AttributeName="DecimalPlaces")]		public decimal DecimalPlaces { get; set; }		[XmlAttribute(AttributeName="CurrencyCode")]		public string CurrencyCode { get; set; }	}	[XmlRoot(ElementName="CancelPolicies", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class CancelPolicies	{		[XmlElement(ElementName="CancelPenalty", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public CancelPenalty ElementoCancelPenalty { get; set; } = new CancelPenalty();	}	[XmlRoot(ElementName="CancelPenalty", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class CancelPenalty	{		[XmlAttribute(AttributeName="PolicyCode")]		public string PolicyCode { get; set; }	}	[XmlRoot(ElementName="Discount", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class Discount	{		[XmlElement(ElementName="DiscountReason", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public DiscountReason ElementoDiscountReason { get; set; } = new DiscountReason();		[XmlAttribute(AttributeName="AmountAfterTax")]		public decimal AmountAfterTax { get; set; }		[XmlAttribute(AttributeName="DecimalPlaces")]		public decimal DecimalPlaces { get; set; }		[XmlAttribute(AttributeName="CurrencyCode")]		public string CurrencyCode { get; set; }	}	[XmlRoot(ElementName="DiscountReason", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class DiscountReason	{		[XmlElement(ElementName = "Text", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string Text { get; set; }	}	[XmlRoot(ElementName="Total", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class Total	{		[XmlAttribute(AttributeName="AmountAfterTax")]		public decimal AmountAfterTax { get; set; }		[XmlAttribute(AttributeName="DecimalPlaces")]		public decimal DecimalPlaces { get; set; }		[XmlAttribute(AttributeName="CurrencyCode")]		public string CurrencyCode { get; set; }	}	[XmlRoot(ElementName="GuestCounts", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class GuestCounts	{		[XmlElement(ElementName="GuestCount", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public List<GuestCount> ListaElementoGuestCount { get; set; } = new List<GuestCount>();	}	[XmlRoot(ElementName="GuestCount", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class GuestCount	{		[XmlAttribute(AttributeName="AgeQualifyingCode")]		public decimal AgeQualifyingCode { get; set; }		[XmlAttribute(AttributeName="Age")]		public decimal Age { get; set; }	}	[XmlRoot(ElementName="ServiceRPHs", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class ServiceRPHs	{		[XmlElement(ElementName="ServiceRPH", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public ServiceRPH ElementoServiceRPH { get; set; } = new ServiceRPH();	}	[XmlRoot(ElementName="ServiceRPH", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class ServiceRPH	{		[XmlAttribute(AttributeName="RPH")]		public decimal RPH { get; set; }		[XmlAttribute(AttributeName="IsPerRoom")]		public bool IsPerRoom { get; set; }	}	[XmlRoot(ElementName="Services", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class Services	{		[XmlElement(ElementName="Service", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public List<Service> ListaElementoService { get; set; } = new List<Service>();	}	[XmlRoot(ElementName="Service", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class Service	{		[XmlElement(ElementName="Price", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public List<Price> ListaElementoPrice { get; set; } = new List<Price>();		[XmlElement(ElementName="TPA_Extensions", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public TPA_Extensions ElementoTPA_Extensions { get; set; } = new TPA_Extensions();		[XmlAttribute(AttributeName="Type")]		public decimal Type { get; set; }		[XmlAttribute(AttributeName="ID")]		public decimal ID { get; set; }		[XmlAttribute(AttributeName="ServiceRPH")]		public decimal ServiceRPH { get; set; }	}	[XmlRoot(ElementName="Price", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class Price	{		[XmlElement(ElementName="Base", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Base ElementoBase { get; set; } = new Base();		[XmlElement(ElementName="CancelPolicies", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public CancelPolicies ElementoCancelPolicies { get; set; } = new CancelPolicies();		[XmlElement(ElementName="Discount", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Discount ElementoDiscount { get; set; } = new Discount();		[XmlElement(ElementName="Total", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Total ElementoTotal { get; set; } = new Total();		[XmlIgnore]		public DateTime EffectiveDate { get; set; }		[XmlAttribute(AttributeName="EffectiveDate")]		public string EffectiveDateString		{				get { return this.EffectiveDate.ToString("yyyy-MM-dd"); }				set { this.EffectiveDate = DateTime.Parse(value); }		}		[XmlAttribute(AttributeName="NumberOfUnits")]		public decimal NumberOfUnits { get; set; }	}	[XmlRoot(ElementName="TPA_Extensions", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class TPA_Extensions	{		[XmlElement(ElementName = "ServiceDescription", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string ServiceDescription { get; set; }		[XmlElement(ElementName = "IATACode", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public decimal IATACode { get; set; }		[XmlElement(ElementName = "TaxCode", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string TaxCode { get; set; }		[XmlElement(ElementName = "VATCode", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public decimal VATCode { get; set; }		[XmlElement(ElementName = "Newsletter", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public bool Newsletter { get; set; }		[XmlElement(ElementName = "ReservationNotes", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string ReservationNotes { get; set; }		[XmlIgnore]		public DateTime OptionExpiringDate { get; set; }		[XmlElement(ElementName = "OptionExpiringDate", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string OptionExpiringDateString		{				get { return this.OptionExpiringDate.ToString("yyyy-MM-dd HH:mm:ss"); }				set { this.OptionExpiringDate = DateTime.Parse(value); }		}		[XmlElement(ElementName="Form", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public List<Form> ListaElementoForm { get; set; } = new List<Form>();		[XmlElement(ElementName="SearchParams", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public SearchParams ElementoSearchParams { get; set; } = new SearchParams();		[XmlElement(ElementName="Layout", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Layout ElementoLayout { get; set; } = new Layout();		[XmlElement(ElementName="ACR", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public ACR ElementoACR { get; set; } = new ACR();		[XmlElement(ElementName="Category", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Category ElementoCategory { get; set; } = new Category();		[XmlElement(ElementName="Ecommerce", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Ecommerce ElementoEcommerce { get; set; } = new Ecommerce();	}	[XmlRoot(ElementName="ResGuests", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class ResGuests	{		[XmlElement(ElementName="ResGuest", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public ResGuest ElementoResGuest { get; set; } = new ResGuest();	}	[XmlRoot(ElementName="ResGuest", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class ResGuest	{		[XmlElement(ElementName="Profiles", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Profiles ElementoProfiles { get; set; } = new Profiles();		[XmlAttribute(AttributeName="ResGuestRPH")]		public decimal ResGuestRPH { get; set; }	}	[XmlRoot(ElementName="Profiles", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class Profiles	{		[XmlElement(ElementName="ProfileInfo", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public List<ProfileInfo> ListaElementoProfileInfo { get; set; } = new List<ProfileInfo>();	}	[XmlRoot(ElementName="ProfileInfo", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class ProfileInfo	{		[XmlElement(ElementName="Profile", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Profile ElementoProfile { get; set; } = new Profile();	}	[XmlRoot(ElementName="Profile", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class Profile	{		[XmlElement(ElementName="Customer", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Customer ElementoCustomer { get; set; } = new Customer();		[XmlElement(ElementName="UserID", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public UserID ElementoUserID { get; set; } = new UserID();		[XmlElement(ElementName="CompanyInfo", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public CompanyInfo ElementoCompanyInfo { get; set; } = new CompanyInfo();		[XmlElement(ElementName="TPA_Extensions", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public TPA_Extensions ElementoTPA_Extensions { get; set; } = new TPA_Extensions();		[XmlAttribute(AttributeName="ProfileType")]		public decimal ProfileType { get; set; }	}	[XmlRoot(ElementName="Customer", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class Customer	{		[XmlElement(ElementName = "Email", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string Email { get; set; }		[XmlElement(ElementName="PersonName", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public PersonName ElementoPersonName { get; set; } = new PersonName();		[XmlElement(ElementName="Address", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Address ElementoAddress { get; set; } = new Address();		[XmlElement(ElementName="Telephone", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Telephone ElementoTelephone { get; set; } = new Telephone();		[XmlElement(ElementName="TPA_Extensions", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public TPA_Extensions ElementoTPA_Extensions { get; set; } = new TPA_Extensions();	}	[XmlRoot(ElementName="PersonName", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class PersonName	{		[XmlElement(ElementName = "GivenName", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string GivenName { get; set; }		[XmlElement(ElementName = "Surname", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string Surname { get; set; }	}	[XmlRoot(ElementName="ResGlobalInfo", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class ResGlobalInfo	{		[XmlElement(ElementName="TimeSpan", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public TimeSpan ElementoTimeSpan { get; set; } = new TimeSpan();		[XmlElement(ElementName="Guarantee", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Guarantee ElementoGuarantee { get; set; } = new Guarantee();		[XmlElement(ElementName="Total", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Total ElementoTotal { get; set; } = new Total();		[XmlElement(ElementName="HotelReservationIDs", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public HotelReservationIDs ElementoHotelReservationIDs { get; set; } = new HotelReservationIDs();		[XmlElement(ElementName="Profiles", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Profiles ElementoProfiles { get; set; } = new Profiles();	}	[XmlRoot(ElementName="TimeSpan", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class TimeSpan	{		[XmlIgnore]		public DateTime Start { get; set; }		[XmlAttribute(AttributeName="Start")]		public string StartString		{				get { return this.Start.ToString("yyyy-MM-dd"); }				set { this.Start = DateTime.Parse(value); }		}		[XmlIgnore]		public DateTime End { get; set; }		[XmlAttribute(AttributeName="End")]		public string EndString		{				get { return this.End.ToString("yyyy-MM-dd"); }				set { this.End = DateTime.Parse(value); }		}	}	[XmlRoot(ElementName="Guarantee", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class Guarantee	{		[XmlElement(ElementName="GuaranteeDescription", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public GuaranteeDescription ElementoGuaranteeDescription { get; set; } = new GuaranteeDescription();		[XmlElement(ElementName="GuaranteesAccepted", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public GuaranteesAccepted ElementoGuaranteesAccepted { get; set; } = new GuaranteesAccepted();	}	[XmlRoot(ElementName="GuaranteeDescription", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class GuaranteeDescription	{		[XmlElement(ElementName = "Text", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string Text { get; set; }	}	[XmlRoot(ElementName="HotelReservationIDs", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class HotelReservationIDs	{		[XmlElement(ElementName="HotelReservationID", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public HotelReservationID ElementoHotelReservationID { get; set; } = new HotelReservationID();	}	[XmlRoot(ElementName="HotelReservationID", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class HotelReservationID	{		[XmlIgnore]		public DateTime ResID_Date { get; set; }		[XmlAttribute(AttributeName="ResID_Date")]		public string ResID_DateString		{				get { return this.ResID_Date.ToString("s"); }				set { this.ResID_Date = DateTime.Parse(value); }		}		[XmlAttribute(AttributeName="ResID_Value")]		public decimal ResID_Value { get; set; }	}	[XmlRoot(ElementName="UserID", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class UserID	{		[XmlAttribute(AttributeName="Type")]		public decimal Type { get; set; }		[XmlAttribute(AttributeName="ID")]		public string ID { get; set; }	}	[XmlRoot(ElementName="CompanyInfo", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class CompanyInfo	{		[XmlElement(ElementName = "Email", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string Email { get; set; }		[XmlElement(ElementName = "CompanyName", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string CompanyName { get; set; }		[XmlElement(ElementName = "URL", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string URL { get; set; }		[XmlElement(ElementName="ContactPerson", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public ContactPerson ElementoContactPerson { get; set; } = new ContactPerson();		[XmlElement(ElementName="Telephone", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public List<Telephone> ListaElementoTelephone { get; set; } = new List<Telephone>();		[XmlElement(ElementName="AddressInfo", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public AddressInfo ElementoAddressInfo { get; set; } = new AddressInfo();	}	[XmlRoot(ElementName="ContactPerson", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class ContactPerson	{		[XmlElement(ElementName="PersonName", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public PersonName ElementoPersonName { get; set; } = new PersonName();	}	[XmlRoot(ElementName="Telephone", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class Telephone	{		[XmlAttribute(AttributeName="PhoneTechType")]		public decimal PhoneTechType { get; set; }		[XmlAttribute(AttributeName="PhoneNumber")]		public decimal PhoneNumber { get; set; }	}	[XmlRoot(ElementName="AddressInfo", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class AddressInfo	{		[XmlElement(ElementName = "AddressLine", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string AddressLine { get; set; }		[XmlElement(ElementName = "CityName", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string CityName { get; set; }		[XmlElement(ElementName = "PostalCode", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public decimal PostalCode { get; set; }		[XmlElement(ElementName = "StateProv", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string StateProv { get; set; }		[XmlElement(ElementName = "CountryName", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string CountryName { get; set; }	}	[XmlRoot(ElementName="Form", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class Form	{		[XmlAttribute(AttributeName="name")]		public string Name { get; set; }		[XmlText]		public string ValoreForm { get; set; }	}	[XmlRoot(ElementName="Address", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class Address	{		[XmlElement(ElementName = "CountryName", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string CountryName { get; set; }	}	[XmlRoot(ElementName="SearchParams", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class SearchParams	{		[XmlElement(ElementName = "param1", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string Param1 { get; set; }	}	[XmlRoot(ElementName="Layout", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class Layout	{		[XmlAttribute(AttributeName="Code")]		public string Code { get; set; }		[XmlAttribute(AttributeName="Type")]		public string Type { get; set; }	}	[XmlRoot(ElementName="ACR", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class ACR	{		[XmlElement(ElementName="Tags", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Tags ElementoTags { get; set; } = new Tags();		[XmlElement(ElementName="Operator", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public Operator ElementoOperator { get; set; } = new Operator();	}	[XmlRoot(ElementName="Tags", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class Tags	{		[XmlElement(ElementName="Tag", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public List<Tag> ListaElementoTag { get; set; } = new List<Tag>();	}	[XmlRoot(ElementName="Tag", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class Tag	{		[XmlAttribute(AttributeName="Type")]		public string Type { get; set; }		[XmlText]		public string ValoreTag { get; set; }	}	[XmlRoot(ElementName="Operator", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class Operator	{		[XmlElement(ElementName = "Surname", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string Surname { get; set; }		[XmlElement(ElementName = "FirstName", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string FirstName { get; set; }	}	[XmlRoot(ElementName="Category", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class Category	{		[XmlAttribute(AttributeName="Code")]		public string Code { get; set; }		[XmlAttribute(AttributeName="ID")]		public decimal ID { get; set; }	}	[XmlRoot(ElementName="GuaranteesAccepted", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class GuaranteesAccepted	{		[XmlElement(ElementName="GuaranteeAccepted", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public GuaranteeAccepted ElementoGuaranteeAccepted { get; set; } = new GuaranteeAccepted();	}	[XmlRoot(ElementName="GuaranteeAccepted", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class GuaranteeAccepted	{		[XmlElement(ElementName="PaymentCard", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public PaymentCard ElementoPaymentCard { get; set; } = new PaymentCard();	}	[XmlRoot(ElementName="PaymentCard", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class PaymentCard	{		[XmlElement(ElementName = "CardHolderName", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public string CardHolderName { get; set; }		[XmlElement(ElementName="CardNumber", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public CardNumber ElementoCardNumber { get; set; } = new CardNumber();		[XmlAttribute(AttributeName="CardCode")]		public string CardCode { get; set; }		[XmlAttribute(AttributeName="ExpireDate")]		public decimal ExpireDate { get; set; }	}	[XmlRoot(ElementName="CardNumber", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class CardNumber	{		[XmlElement(ElementName = "PlainText", Namespace ="http://www.opentravel.org/OTA/2003/05")]		public decimal PlainText { get; set; }	}	[XmlRoot(ElementName="Ecommerce", Namespace ="http://www.opentravel.org/OTA/2003/05")]	public class Ecommerce	{		[XmlAttribute(AttributeName="Code")]		public string Code { get; set; }	}}