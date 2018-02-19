using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using BELHXmlTool;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplications.Utilities;

namespace TesterProject
{
    [TestClass]
    public class ClasseTest
    {
        [TestMethod]
        public void ProvaCreaOggetto()
        {
            OTA_ResRetrieveRS newObject = this.CreaNuovoOggetto();
            var documento = this.ToXml(newObject);
            var documentoPartenza = XDocument.Load("C:\\Users\\user\\Source\\Repos\\XMLConverter\\TesterProject\\ClassiSerializzabili\\xmlGenerato.xml");
            var documentoSerializzato = XDocument.Parse(documento);
            Tuple<XObject, XObject> result = documentoPartenza.DeepEquals(documentoSerializzato, XObjectComparisonOptions.Semantic);
            Assert.IsNull(result, $"{result}");
        }

        /// <summary>
        /// Permette di creare un nuovo oggetto con il file txt appena creato
        /// </summary>
        private OTA_ResRetrieveRS CreaNuovoOggetto()
        {
            return new OTA_ResRetrieveRS
            {
                ElementoReservationsList = new ReservationsList
                {
                    ListaElementoHotelReservation = new List<HotelReservation>
                    {
                        new HotelReservation
                        {
                            ElementoRoomStays = new RoomStays
                            {
                                ListaElementoRoomStay = new List<RoomStay>
                                {
                                    new RoomStay
                                    {
                                        ResGuestRPHs = 6768,
                                        ElementoRoomTypes = new RoomTypes
                                        {
                                            ElementoRoomType = new RoomType
                                            {
                                                RoomTypeCode = 1422,
                                            },
                                        },
                                        ElementoRoomRates = new RoomRates
                                        {
                                            ListaElementoRoomRate = new List<RoomRate>
                                            {
                                                new RoomRate
                                                {
                                                    ElementoRates = new Rates
                                                    {
                                                        ElementoRate = new Rate
                                                        {
                                                            ElementoBase = new Base
                                                            {
                                                                AmountAfterTax = 100.00m,
                                                                DecimalPlaces = 0,
                                                            },
                                                            ElementoDiscount = new Discount
                                                            {
                                                                AmountAfterTax = -10.00m,
                                                                DecimalPlaces = 0,
                                                            },
                                                            ElementoTotal = new Total
                                                            {
                                                                AmountAfterTax = 90.00m,
                                                                DecimalPlaces = 0,
                                                            },
                                                        },
                                                    },
                                                    RatePlanCode = 1422,
                                                    EffectiveDate = DateTime.Parse("18/02/2014 00:00:00"),
                                                    NumberOfUnits = 1,
                                                },
                                                new RoomRate
                                                {
                                                    ElementoRates = new Rates
                                                    {
                                                        ElementoRate = new Rate
                                                        {
                                                            ElementoBase = new Base
                                                            {
                                                                AmountAfterTax = 100.00m,
                                                                DecimalPlaces = 0,
                                                            },
                                                            ElementoDiscount = new Discount
                                                            {
                                                                AmountAfterTax = -10.00m,
                                                                DecimalPlaces = 0,
                                                            },
                                                            ElementoTotal = new Total
                                                            {
                                                                AmountAfterTax = 90.00m,
                                                                DecimalPlaces = 0,
                                                            },
                                                        },
                                                    },
                                                    RatePlanCode = 1422,
                                                    EffectiveDate = DateTime.Parse("19/02/2014 00:00:00"),
                                                    NumberOfUnits = 1,
                                                },
                                                new RoomRate
                                                {
                                                    ElementoRates = new Rates
                                                    {
                                                        ElementoRate = new Rate
                                                        {
                                                            ElementoBase = new Base
                                                            {
                                                                AmountAfterTax = 100.00m,
                                                                DecimalPlaces = 0,
                                                            },
                                                            ElementoDiscount = new Discount
                                                            {
                                                                AmountAfterTax = -10.00m,
                                                                DecimalPlaces = 0,
                                                            },
                                                            ElementoTotal = new Total
                                                            {
                                                                AmountAfterTax = 90.00m,
                                                                DecimalPlaces = 0,
                                                            },
                                                        },
                                                    },
                                                    RatePlanCode = 1422,
                                                    EffectiveDate = DateTime.Parse("20/02/2014 00:00:00"),
                                                    NumberOfUnits = 1,
                                                },
                                            },
                                        },
                                        ElementoGuestCounts = new GuestCounts
                                        {
                                            ListaElementoGuestCount = new List<GuestCount>
                                            {
                                                new GuestCount
                                                {
                                                    AgeQualifyingCode = 10,
                                                    Age = 19,
                                                },
                                            },
                                        },
                                        ElementoTotal = new Total
                                        {
                                            AmountAfterTax = 270.00m,
                                            DecimalPlaces = 0,
                                        },
                                        ElementoServiceRPHs = new ServiceRPHs
                                        {
                                            ElementoServiceRPH = new ServiceRPH
                                            {
                                                RPH = 6772,
                                                IsPerRoom = true,
                                            },
                                        },
                                    },
                                    new RoomStay
                                    {
                                        ElementoRoomTypes = new RoomTypes
                                        {
                                            ElementoRoomType = new RoomType
                                            {
                                                RoomTypeCode = 34,
                                            },
                                        },
                                        ElementoRoomRates = new RoomRates
                                        {
                                            ListaElementoRoomRate = new List<RoomRate>
                                            {
                                                new RoomRate
                                                {
                                                    ElementoRates = new Rates
                                                    {
                                                        ElementoRate = new Rate
                                                        {
                                                            ElementoBase = new Base
                                                            {
                                                                AmountAfterTax = 100.00m,
                                                                DecimalPlaces = 0,
                                                            },
                                                            ElementoDiscount = new Discount
                                                            {
                                                                AmountAfterTax = -10.00m,
                                                                DecimalPlaces = 0,
                                                            },
                                                            ElementoTotal = new Total
                                                            {
                                                                AmountAfterTax = 90.00m,
                                                                DecimalPlaces = 0,
                                                            },
                                                        },
                                                    },
                                                    RatePlanCode = 34,
                                                    EffectiveDate = DateTime.Parse("18/02/2014 00:00:00"),
                                                    NumberOfUnits = 1,
                                                },
                                                new RoomRate
                                                {
                                                    ElementoRates = new Rates
                                                    {
                                                        ElementoRate = new Rate
                                                        {
                                                            ElementoBase = new Base
                                                            {
                                                                AmountAfterTax = 100.00m,
                                                                DecimalPlaces = 0,
                                                            },
                                                            ElementoDiscount = new Discount
                                                            {
                                                                AmountAfterTax = -10.00m,
                                                                DecimalPlaces = 0,
                                                            },
                                                            ElementoTotal = new Total
                                                            {
                                                                AmountAfterTax = 90.00m,
                                                                DecimalPlaces = 0,
                                                            },
                                                        },
                                                    },
                                                    RatePlanCode = 34,
                                                    EffectiveDate = DateTime.Parse("19/02/2014 00:00:00"),
                                                    NumberOfUnits = 1,
                                                },
                                                new RoomRate
                                                {
                                                    ElementoRates = new Rates
                                                    {
                                                        ElementoRate = new Rate
                                                        {
                                                            ElementoBase = new Base
                                                            {
                                                                AmountAfterTax = 100.00m,
                                                                DecimalPlaces = 0,
                                                            },
                                                            ElementoDiscount = new Discount
                                                            {
                                                                AmountAfterTax = -10.00m,
                                                                DecimalPlaces = 0,
                                                            },
                                                            ElementoTotal = new Total
                                                            {
                                                                AmountAfterTax = 90.00m,
                                                                DecimalPlaces = 0,
                                                            },
                                                        },
                                                    },
                                                    RatePlanCode = 34,
                                                    EffectiveDate = DateTime.Parse("20/02/2014 00:00:00"),
                                                    NumberOfUnits = 1,
                                                },
                                            },
                                        },
                                        ElementoGuestCounts = new GuestCounts
                                        {
                                            ListaElementoGuestCount = new List<GuestCount>
                                            {
                                                new GuestCount
                                                {
                                                    AgeQualifyingCode = 10,
                                                    Age = 18,
                                                },
                                                new GuestCount
                                                {
                                                    AgeQualifyingCode = 8,
                                                    Age = 10,
                                                },
                                            },
                                        },
                                        ElementoTotal = new Total
                                        {
                                            AmountAfterTax = 270.00m,
                                            DecimalPlaces = 0,
                                        },
                                    },
                                },
                            },
                            ElementoServices = new Services
                            {
                                ListaElementoService = new List<Service>
                                {
                                    new Service
                                    {
                                        ListaElementoPrice = new List<Price>
                                        {
                                            new Price
                                            {
                                                ElementoBase = new Base
                                                {
                                                    AmountAfterTax = 20.00m,
                                                    DecimalPlaces = 0,
                                                },
                                                ElementoDiscount = new Discount
                                                {
                                                    AmountAfterTax = -02.00m,
                                                    DecimalPlaces = 0,
                                                },
                                                ElementoTotal = new Total
                                                {
                                                    AmountAfterTax = 18.00m,
                                                    DecimalPlaces = 0,
                                                },
                                                EffectiveDate = DateTime.Parse("19/02/2014 00:00:00"),
                                            },
                                        },
                                        Type = 18,
                                        ID = 4,
                                        ServiceRPH = 1177,
                                    },
                                    new Service
                                    {
                                        ListaElementoPrice = new List<Price>
                                        {
                                            new Price
                                            {
                                                ElementoTotal = new Total
                                                {
                                                    AmountAfterTax = 09.00m,
                                                    DecimalPlaces = 0,
                                                },
                                            },
                                            new Price
                                            {
                                                ElementoBase = new Base
                                                {
                                                    AmountAfterTax = 10.00m,
                                                    DecimalPlaces = 0,
                                                },
                                                ElementoDiscount = new Discount
                                                {
                                                    AmountAfterTax = -01.00m,
                                                    DecimalPlaces = 0,
                                                },
                                                ElementoTotal = new Total
                                                {
                                                    AmountAfterTax = 09.00m,
                                                    DecimalPlaces = 0,
                                                },
                                                EffectiveDate = DateTime.Parse("19/02/2014 00:00:00"),
                                            },
                                        },
                                        Type = 18,
                                        ID = 190,
                                    },
                                },
                            },
                            ElementoResGuests = new ResGuests
                            {
                                ElementoResGuest = new ResGuest
                                {
                                    ResGuestRPH = 1173,
                                },
                            },
                            ElementoResGlobalInfo = new ResGlobalInfo
                            {
                                ElementoTimeSpan = new BELHXmlTool.TimeSpan
                                {
                                    Start = DateTime.Parse("06/12/2013 00:00:00"),
                                    End = DateTime.Parse("09/12/2013 00:00:00"),
                                },
                                ElementoTotal = new Total
                                {
                                    AmountAfterTax = 369.25m,
                                    DecimalPlaces = 0,
                                },
                                ElementoHotelReservationIDs = new HotelReservationIDs
                                {
                                    ElementoHotelReservationID = new HotelReservationID
                                    {
                                        ResID_Date = DateTime.Parse("04/12/2013 15:18:08"),
                                        ResID_Value = 954603802,
                                    },
                                },
                                ElementoProfiles = new Profiles
                                {
                                    ListaElementoProfileInfo = new List<ProfileInfo>
                                    {
                                        new ProfileInfo
                                        {
                                            ElementoProfile = new Profile
                                            {
                                                ElementoUserID = new UserID
                                                {
                                                    Type = 7,
                                                },
                                                ElementoCompanyInfo = new CompanyInfo
                                                {
                                                    ListaElementoTelephone = new List<Telephone>
                                                    {
                                                        new Telephone
                                                        {
                                                            PhoneTechType = 1,
                                                            PhoneNumber = 123456,
                                                        },
                                                        new Telephone
                                                        {
                                                            PhoneTechType = 3,
                                                            PhoneNumber = 54543,
                                                        },
                                                        new Telephone
                                                        {
                                                            PhoneTechType = 5,
                                                            PhoneNumber = 3656565,
                                                        },
                                                    },
                                                    ElementoAddressInfo = new AddressInfo
                                                    {
                                                        PostalCode = 74887,
                                                    },
                                                },
                                                ElementoTPA_Extensions = new TPA_Extensions
                                                {
                                                    IATACode = 342363476,
                                                    VATCode = 221215125,
                                                },
                                                ProfileType = 23,
                                            },
                                        },
                                        new ProfileInfo
                                        {
                                            ElementoProfile = new Profile
                                            {
                                                ElementoCustomer = new Customer
                                                {
                                                    ElementoTelephone = new Telephone
                                                    {
                                                        PhoneTechType = 1,
                                                        PhoneNumber = 123456,
                                                    },
                                                    ElementoTPA_Extensions = new TPA_Extensions
                                                    {
                                                        Newsletter = false,
                                                    },
                                                },
                                                ProfileType = 1,
                                            },
                                        },
                                    },
                                },
                            },
                            ElementoTPA_Extensions = new TPA_Extensions
                            {
                                OptionExpiringDate = DateTime.Parse("10/03/2017 08:00:00"),
                            },
                        },
                        new HotelReservation
                        {
                            ElementoServices = new Services
                            {
                                ListaElementoService = new List<Service>
                                {
                                    new Service
                                    {
                                        ListaElementoPrice = new List<Price>
                                        {
                                            new Price
                                            {
                                                ElementoBase = new Base
                                                {
                                                    AmountAfterTax = 128.00m,
                                                    DecimalPlaces = 0,
                                                },
                                                ElementoDiscount = new Discount
                                                {
                                                    AmountAfterTax = -12.80m,
                                                    DecimalPlaces = 0,
                                                },
                                                ElementoTotal = new Total
                                                {
                                                    AmountAfterTax = 115.20m,
                                                    DecimalPlaces = 0,
                                                },
                                                EffectiveDate = DateTime.Parse("18/01/2018 00:00:00"),
                                                NumberOfUnits = 1,
                                            },
                                        },
                                        ElementoTPA_Extensions = new TPA_Extensions
                                        {
                                            ElementoCategory = new Category
                                            {
                                                ID = 20014,
                                            },
                                        },
                                        Type = 18,
                                        ID = 3745,
                                    },
                                },
                            },
                            ElementoResGlobalInfo = new ResGlobalInfo
                            {
                                ElementoTimeSpan = new BELHXmlTool.TimeSpan
                                {
                                    Start = DateTime.Parse("18/01/2018 00:00:00"),
                                    End = DateTime.Parse("19/01/2018 00:00:00"),
                                },
                                ElementoGuarantee = new Guarantee
                                {
                                    ElementoGuaranteesAccepted = new GuaranteesAccepted
                                    {
                                        ElementoGuaranteeAccepted = new GuaranteeAccepted
                                        {
                                            ElementoPaymentCard = new PaymentCard
                                            {
                                                ElementoCardNumber = new CardNumber
                                                {
                                                    PlainText = 55555555555555555.00m,
                                                },
                                                ExpireDate = 121,
                                            },
                                        },
                                    },
                                },
                                ElementoTotal = new Total
                                {
                                    AmountAfterTax = 115.20m,
                                    DecimalPlaces = 0,
                                },
                                ElementoHotelReservationIDs = new HotelReservationIDs
                                {
                                    ElementoHotelReservationID = new HotelReservationID
                                    {
                                        ResID_Date = DateTime.Parse("18/01/2018 14:58:35"),
                                        ResID_Value = 896794353,
                                    },
                                },
                                ElementoProfiles = new Profiles
                                {
                                    ListaElementoProfileInfo = new List<ProfileInfo>
                                    {
                                        new ProfileInfo
                                        {
                                            ElementoProfile = new Profile
                                            {
                                                ElementoCustomer = new Customer
                                                {
                                                    ElementoTPA_Extensions = new TPA_Extensions
                                                    {
                                                        Newsletter = false,
                                                    },
                                                },
                                            },
                                        },
                                        new ProfileInfo
                                        {
                                            ElementoProfile = new Profile
                                            {
                                                ElementoCustomer = new Customer
                                                {
                                                    ElementoTelephone = new Telephone
                                                    {
                                                        PhoneTechType = 1,
                                                        PhoneNumber = 5353454,
                                                    },
                                                    ElementoTPA_Extensions = new TPA_Extensions
                                                    {
                                                        Newsletter = false,
                                                    },
                                                },
                                            },
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
                TimeStamp = DateTime.Parse("04/12/2013 15:18:36"),
                Version = 07.00m,
            };
        }

        /// <summary>
        /// Trasforma l'oggetto in xml
        /// </summary>
        public string ToXml(object objToXml)
        {
            StreamWriter stWriter = null;
            XmlSerializer xmlSerializer;
            string buffer;
            try
            {
                xmlSerializer = new XmlSerializer(objToXml.GetType());
                MemoryStream memStream = new MemoryStream();
                stWriter = new StreamWriter(memStream);
                xmlSerializer.Serialize(stWriter, objToXml);
                buffer = Encoding.UTF8.GetString(memStream.GetBuffer()).Replace("\x00", "");
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (stWriter != null) stWriter.Close();
            }
            return buffer;
        }

    }
}
