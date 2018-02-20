﻿using System;
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
            var documentoPartenza = XDocument.Load("C:\\Users\\user\\Source\\Repos\\XMLConverter\\TesterProject\\ClassiSerializzabili\\BookingExpertXML.xml");
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
                Success = "",
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
                                                RoomTypeCode = "CodiceTipologia1",
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
                                                                CurrencyCode = "EUR",
                                                            },
                                                            ElementoCancelPolicies = new CancelPolicies
                                                            {
                                                                ElementoCancelPenalty = new CancelPenalty
                                                                {
                                                                    PolicyCode = "Default",
                                                                },
                                                            },
                                                            ElementoDiscount = new Discount
                                                            {
                                                                ElementoDiscountReason = new DiscountReason
                                                                {
                                                                    Text = "Sconto5",
                                                                },
                                                                AmountAfterTax = -10.00m,
                                                                DecimalPlaces = 0,
                                                                CurrencyCode = "EUR",
                                                            },
                                                            ElementoTotal = new Total
                                                            {
                                                                AmountAfterTax = 90.00m,
                                                                DecimalPlaces = 0,
                                                                CurrencyCode = "EUR",
                                                            },
                                                        },
                                                    },
                                                    RatePlanCode = "CodiceTipologia1",
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
                                                                CurrencyCode = "EUR",
                                                            },
                                                            ElementoCancelPolicies = new CancelPolicies
                                                            {
                                                                ElementoCancelPenalty = new CancelPenalty
                                                                {
                                                                    PolicyCode = "Default",
                                                                },
                                                            },
                                                            ElementoDiscount = new Discount
                                                            {
                                                                ElementoDiscountReason = new DiscountReason
                                                                {
                                                                    Text = "Sconto5",
                                                                },
                                                                AmountAfterTax = -10.00m,
                                                                DecimalPlaces = 0,
                                                                CurrencyCode = "EUR",
                                                            },
                                                            ElementoTotal = new Total
                                                            {
                                                                AmountAfterTax = 90.00m,
                                                                DecimalPlaces = 0,
                                                                CurrencyCode = "EUR",
                                                            },
                                                        },
                                                    },
                                                    RatePlanCode = "CodiceTipologia1",
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
                                                                CurrencyCode = "EUR",
                                                            },
                                                            ElementoCancelPolicies = new CancelPolicies
                                                            {
                                                                ElementoCancelPenalty = new CancelPenalty
                                                                {
                                                                    PolicyCode = "Default",
                                                                },
                                                            },
                                                            ElementoDiscount = new Discount
                                                            {
                                                                ElementoDiscountReason = new DiscountReason
                                                                {
                                                                    Text = "Sconto5",
                                                                },
                                                                AmountAfterTax = -10.00m,
                                                                DecimalPlaces = 0,
                                                                CurrencyCode = "EUR",
                                                            },
                                                            ElementoTotal = new Total
                                                            {
                                                                AmountAfterTax = 90.00m,
                                                                DecimalPlaces = 0,
                                                                CurrencyCode = "EUR",
                                                            },
                                                        },
                                                    },
                                                    RatePlanCode = "CodiceTipologia1",
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
                                            CurrencyCode = "EUR",
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
                                                RoomTypeCode = "CodiceRetta1",
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
                                                                CurrencyCode = "EUR",
                                                            },
                                                            ElementoCancelPolicies = new CancelPolicies
                                                            {
                                                                ElementoCancelPenalty = new CancelPenalty
                                                                {
                                                                    PolicyCode = "Default",
                                                                },
                                                            },
                                                            ElementoDiscount = new Discount
                                                            {
                                                                ElementoDiscountReason = new DiscountReason
                                                                {
                                                                    Text = "Sconto5",
                                                                },
                                                                AmountAfterTax = -10.00m,
                                                                DecimalPlaces = 0,
                                                                CurrencyCode = "EUR",
                                                            },
                                                            ElementoTotal = new Total
                                                            {
                                                                AmountAfterTax = 90.00m,
                                                                DecimalPlaces = 0,
                                                                CurrencyCode = "EUR",
                                                            },
                                                        },
                                                    },
                                                    RatePlanCode = "CodiceRetta1",
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
                                                                CurrencyCode = "EUR",
                                                            },
                                                            ElementoCancelPolicies = new CancelPolicies
                                                            {
                                                                ElementoCancelPenalty = new CancelPenalty
                                                                {
                                                                    PolicyCode = "Default",
                                                                },
                                                            },
                                                            ElementoDiscount = new Discount
                                                            {
                                                                ElementoDiscountReason = new DiscountReason
                                                                {
                                                                    Text = "Sconto5",
                                                                },
                                                                AmountAfterTax = -10.00m,
                                                                DecimalPlaces = 0,
                                                                CurrencyCode = "EUR",
                                                            },
                                                            ElementoTotal = new Total
                                                            {
                                                                AmountAfterTax = 90.00m,
                                                                DecimalPlaces = 0,
                                                                CurrencyCode = "EUR",
                                                            },
                                                        },
                                                    },
                                                    RatePlanCode = "CodiceRetta1",
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
                                                                CurrencyCode = "EUR",
                                                            },
                                                            ElementoCancelPolicies = new CancelPolicies
                                                            {
                                                                ElementoCancelPenalty = new CancelPenalty
                                                                {
                                                                    PolicyCode = "Default",
                                                                },
                                                            },
                                                            ElementoDiscount = new Discount
                                                            {
                                                                ElementoDiscountReason = new DiscountReason
                                                                {
                                                                    Text = "Sconto5",
                                                                },
                                                                AmountAfterTax = -10.00m,
                                                                DecimalPlaces = 0,
                                                                CurrencyCode = "EUR",
                                                            },
                                                            ElementoTotal = new Total
                                                            {
                                                                AmountAfterTax = 90.00m,
                                                                DecimalPlaces = 0,
                                                                CurrencyCode = "EUR",
                                                            },
                                                        },
                                                    },
                                                    RatePlanCode = "CodiceRetta1",
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
                                            CurrencyCode = "EUR",
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
                                                    CurrencyCode = "EUR",
                                                },
                                                ElementoCancelPolicies = new CancelPolicies
                                                {
                                                    ElementoCancelPenalty = new CancelPenalty
                                                    {
                                                        PolicyCode = "Default",
                                                    },
                                                },
                                                ElementoDiscount = new Discount
                                                {
                                                    ElementoDiscountReason = new DiscountReason
                                                    {
                                                        Text = "Sconto5",
                                                    },
                                                    AmountAfterTax = -02.00m,
                                                    DecimalPlaces = 0,
                                                    CurrencyCode = "EUR",
                                                },
                                                ElementoTotal = new Total
                                                {
                                                    AmountAfterTax = 18.00m,
                                                    DecimalPlaces = 0,
                                                    CurrencyCode = "EUR",
                                                },
                                                EffectiveDate = DateTime.Parse("19/02/2014 00:00:00"),
                                            },
                                        },
                                        ElementoTPA_Extensions = new TPA_Extensions
                                        {
                                            ServiceDescription = "Fiori",
                                        },
                                        Type = 18,
                                        ID = 4,
                                        ServiceRPH = 1379,
                                    },
                                    new Service
                                    {
                                        ListaElementoPrice = new List<Price>
                                        {
                                            new Price
                                            {
                                                ElementoCancelPolicies = new CancelPolicies
                                                {
                                                    ElementoCancelPenalty = new CancelPenalty
                                                    {
                                                        PolicyCode = "Default",
                                                    },
                                                },
                                                ElementoTotal = new Total
                                                {
                                                    AmountAfterTax = 09.00m,
                                                    DecimalPlaces = 0,
                                                    CurrencyCode = "EUR",
                                                },
                                            },
                                            new Price
                                            {
                                                ElementoBase = new Base
                                                {
                                                    AmountAfterTax = 10.00m,
                                                    DecimalPlaces = 0,
                                                    CurrencyCode = "EUR",
                                                },
                                                ElementoCancelPolicies = new CancelPolicies
                                                {
                                                    ElementoCancelPenalty = new CancelPenalty
                                                    {
                                                        PolicyCode = "Default",
                                                    },
                                                },
                                                ElementoDiscount = new Discount
                                                {
                                                    ElementoDiscountReason = new DiscountReason
                                                    {
                                                        Text = "Sconto5",
                                                    },
                                                    AmountAfterTax = -01.00m,
                                                    DecimalPlaces = 0,
                                                    CurrencyCode = "EUR",
                                                },
                                                ElementoTotal = new Total
                                                {
                                                    AmountAfterTax = 09.00m,
                                                    DecimalPlaces = 0,
                                                    CurrencyCode = "EUR",
                                                },
                                                EffectiveDate = DateTime.Parse("19/02/2014 00:00:00"),
                                            },
                                        },
                                        ElementoTPA_Extensions = new TPA_Extensions
                                        {
                                            ServiceDescription = "Vino",
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
                                                        Email = "pippo@email.em",
                                                        ElementoPersonName = new PersonName
                                                        {
                                                            GivenName = "Pippo",
                                                            Surname = "Pluto",
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
                                                        ElementoPersonName = new PersonName
                                                        {
                                                            GivenName = "Topolino",
                                                            Surname = "Paparino",
                                                        },
                                                    },
                                                },
                                            },
                                        },
                                    },
                                    ResGuestRPH = 1379,
                                },
                            },
                            ElementoResGlobalInfo = new ResGlobalInfo
                            {
                                ElementoTimeSpan = new BELHXmlTool.TimeSpan
                                {
                                    Start = DateTime.Parse("06/12/2013 00:00:00"),
                                    End = DateTime.Parse("09/12/2013 00:00:00"),
                                },
                                ElementoGuarantee = new Guarantee
                                {
                                    ElementoGuaranteeDescription = new GuaranteeDescription
                                    {
                                        Text = "Default",
                                    },
                                },
                                ElementoTotal = new Total
                                {
                                    AmountAfterTax = 369.25m,
                                    DecimalPlaces = 0,
                                    CurrencyCode = "EUR",
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
                                                    ID = "AV001",
                                                },
                                                ElementoCompanyInfo = new CompanyInfo
                                                {
                                                    Email = "agenzia@email.em",
                                                    CompanyName = "Agenzia viaggi srl",
                                                    URL = "www.agenziaviaggi.ag",
                                                    ElementoContactPerson = new ContactPerson
                                                    {
                                                        ElementoPersonName = new PersonName
                                                        {
                                                            GivenName = "Nome operatore",
                                                            Surname = "Cognome operatore",
                                                        },
                                                    },
                                                    ListaElementoTelephone = new List<Telephone>
                                                    {
                                                        new Telephone
                                                        {
                                                            PhoneTechType = 1,
                                                            PhoneNumber = "12CodiceRetta156",
                                                        },
                                                        new Telephone
                                                        {
                                                            PhoneTechType = 3,
                                                            PhoneNumber = "54543",
                                                        },
                                                        new Telephone
                                                        {
                                                            PhoneTechType = 5,
                                                            PhoneNumber = "3656565",
                                                        },
                                                    },
                                                    ElementoAddressInfo = new AddressInfo
                                                    {
                                                        AddressLine = "via roma,23",
                                                        CityName = "roma",
                                                        PostalCode = 74887,
                                                        StateProv = "Roma",
                                                        CountryName = "Italy",
                                                    },
                                                },
                                                ElementoTPA_Extensions = new TPA_Extensions
                                                {
                                                    IATACode = "CodiceRetta1236CodiceRetta176",
                                                    TaxCode = "AGVGG",
                                                    VATCode = 221215125,
                                                    ListaElementoForm = new List<Form>
                                                    {
                                                        new Form
                                                        {
                                                            Name = "n. operatore",
                                                            ValoreForm = "12CodiceRetta15",
                                                        },
                                                    },
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
                                                    Email = "mario@rossi.it",
                                                    ElementoPersonName = new PersonName
                                                    {
                                                        GivenName = "Mario",
                                                        Surname = "Rossi",
                                                    },
                                                    ElementoAddress = new Address
                                                    {
                                                        CountryName = "AL",
                                                    },
                                                    ElementoTelephone = new Telephone
                                                    {
                                                        PhoneTechType = 1,
                                                        PhoneNumber = "12CodiceRetta156",
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
                                ReservationNotes = "NOTA OPERATORE - 2017-10-06 15:17 : Prenotazione inserita nel gestionale",
                                OptionExpiringDate = DateTime.Parse("10/03/2017 08:00:00"),
                                ElementoSearchParams = new SearchParams
                                {
                                    Param1 = "value1",
                                },
                                ElementoLayout = new Layout
                                {
                                    Code = "Default",
                                    Type = "ACR",
                                },
                                ElementoACR = new ACR
                                {
                                    ElementoTags = new Tags
                                    {
                                        ListaElementoTag = new List<Tag>
                                        {
                                            new Tag
                                            {
                                                Type = "CHANNEL",
                                                ValoreTag = "Telephone",
                                            },
                                            new Tag
                                            {
                                                Type = "CUSTOMER",
                                                ValoreTag = "Business",
                                            },
                                            new Tag
                                            {
                                                Type = "CUSTOMER",
                                                ValoreTag = "Friend",
                                            },
                                            new Tag
                                            {
                                                Type = "STAY",
                                                ValoreTag = "Bike",
                                            },
                                            new Tag
                                            {
                                                Type = "STAY",
                                                ValoreTag = "Family",
                                            },
                                            new Tag
                                            {
                                                Type = "STAY",
                                                ValoreTag = "Cultural",
                                            },
                                        },
                                    },
                                    ElementoOperator = new Operator
                                    {
                                        Surname = "Rossi",
                                        FirstName = "Mario",
                                    },
                                },
                            },
                            ResStatus = "Waitlisted",
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
                                                    CurrencyCode = "EUR",
                                                },
                                                ElementoCancelPolicies = new CancelPolicies
                                                {
                                                    ElementoCancelPenalty = new CancelPenalty
                                                    {
                                                        PolicyCode = "Non RImborsabil",
                                                    },
                                                },
                                                ElementoDiscount = new Discount
                                                {
                                                    ElementoDiscountReason = new DiscountReason
                                                    {
                                                        Text = "Prova",
                                                    },
                                                    AmountAfterTax = -12.80m,
                                                    DecimalPlaces = 0,
                                                    CurrencyCode = "EUR",
                                                },
                                                ElementoTotal = new Total
                                                {
                                                    AmountAfterTax = 115.20m,
                                                    DecimalPlaces = 0,
                                                    CurrencyCode = "EUR",
                                                },
                                                EffectiveDate = DateTime.Parse("18/01/2018 00:00:00"),
                                                NumberOfUnits = 1,
                                            },
                                        },
                                        ElementoTPA_Extensions = new TPA_Extensions
                                        {
                                            ServiceDescription = "PrivateDaySpa i GIF",
                                            ElementoCategory = new Category
                                            {
                                                Code = "SPA",
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
                                    ElementoGuaranteeDescription = new GuaranteeDescription
                                    {
                                        Text = "Cartaprepagamento",
                                    },
                                    ElementoGuaranteesAccepted = new GuaranteesAccepted
                                    {
                                        ElementoGuaranteeAccepted = new GuaranteeAccepted
                                        {
                                            ElementoPaymentCard = new PaymentCard
                                            {
                                                CardHolderName = "Card holder",
                                                ElementoCardNumber = new CardNumber
                                                {
                                                    PlainText = 55555555555555555m,
                                                },
                                                CardCode = "VI",
                                                ExpireDate = DateTime.Parse("01/01/2021 00:00:00"),
                                            },
                                        },
                                    },
                                },
                                ElementoTotal = new Total
                                {
                                    AmountAfterTax = 115.20m,
                                    DecimalPlaces = 0,
                                    CurrencyCode = "EUR",
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
                                                        ListaElementoForm = new List<Form>
                                                        {
                                                            new Form
                                                            {
                                                                Name = "Destinatario",
                                                                ValoreForm = "To",
                                                            },
                                                            new Form
                                                            {
                                                                Name = "Regalato da",
                                                                ValoreForm = "Gift from..",
                                                            },
                                                        },
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
                                                    Email = "developer@bookingexpert.com",
                                                    ElementoPersonName = new PersonName
                                                    {
                                                        GivenName = "John",
                                                        Surname = "Doe",
                                                    },
                                                    ElementoTelephone = new Telephone
                                                    {
                                                        PhoneTechType = 1,
                                                        PhoneNumber = "535CodiceRetta154",
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
                            ElementoTPA_Extensions = new TPA_Extensions
                            {
                                ElementoLayout = new Layout
                                {
                                    Code = "Voucher+E-commerce",
                                    Type = "BER",
                                },
                                ElementoEcommerce = new Ecommerce
                                {
                                    Code = "Voucher/Gif",
                                },
                            },
                            ResStatus = "Reserved",
                        },
                    },
                },
                PrimaryLangID = "it",
                Target = "Production",
                TimeStamp = DateTime.Parse("04/12/2013 15:18:36"),
                Version = 07.000m,
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
