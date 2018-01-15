using System;using System.Collections.Generic;using System.Xml;using System.Xml.Serialization;namespace BLHXML{
    [XmlRoot(ElementName = "spec")]
    public class Spec
    {
        [XmlElement(ElementName = "header")]
        public Header ElementoHeader { get; set; }
        [XmlElement(ElementName = "body")]
        public Body ElementoBody { get; set; }
        [XmlElement(ElementName = "back")]
        public Back ElementoBack { get; set; }
        [XmlAttribute(AttributeName = "w3c-doctype")]
        public string W3cdoctype { get; set; }
        [XmlAttribute(AttributeName = "lang")]
        public string Lang { get; set; }
    }
    [XmlRoot(ElementName = "header")]
    public class Header
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
        [XmlElement(ElementName = "version")]
        public string Version { get; set; }
        [XmlElement(ElementName = "w3c-designation")]
        public string W3cdesignation { get; set; }
        [XmlElement(ElementName = "w3c-doctype")]
        public string W3cdoctype { get; set; }
        [XmlElement(ElementName = "publoc")]
        public string Publoc { get; set; }
        [XmlElement(ElementName = "latestloc")]
        public string Latestloc { get; set; }
        [XmlElement(ElementName = "abstract")]
        public string Abstract { get; set; }
        [XmlElement(ElementName = "pubstmt")]
        public string Pubstmt { get; set; }
        [XmlElement(ElementName = "sourcedesc")]
        public string Sourcedesc { get; set; }
        [XmlElement(ElementName = "revisiondesc")]
        public string Revisiondesc { get; set; }
        [XmlElement(ElementName = "pubdate")]
        public Pubdate ElementoPubdate { get; set; }
        [XmlElement(ElementName = "altlocs")]
        public Altlocs ElementoAltlocs { get; set; }
        [XmlElement(ElementName = "prevlocs")]
        public Prevlocs ElementoPrevlocs { get; set; }
        [XmlElement(ElementName = "authlist")]
        public Authlist ElementoAuthlist { get; set; }
        [XmlElement(ElementName = "errataloc")]
        public Errataloc ElementoErrataloc { get; set; }
        [XmlElement(ElementName = "preverrataloc")]
        public Preverrataloc ElementoPreverrataloc { get; set; }
        [XmlElement(ElementName = "translationloc")]
        public Translationloc ElementoTranslationloc { get; set; }
        [XmlElement(ElementName = "status")]
        public Status ElementoStatus { get; set; }
        [XmlElement(ElementName = "langusage")]
        public Langusage ElementoLangusage { get; set; }
    }
    [XmlRoot(ElementName = "pubdate")]
    public class Pubdate
    {
        [XmlElement(ElementName = "day")]
        public decimal Day { get; set; }
        [XmlElement(ElementName = "month")]
        public string Month { get; set; }
        [XmlElement(ElementName = "year")]
        public decimal Year { get; set; }
    }
    [XmlRoot(ElementName = "loc")]
    public class Loc
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "role")]
        public string Role { get; set; }
    }
    [XmlRoot(ElementName = "altlocs")]
    public class Altlocs
    {
        [XmlElement(ElementName = "loc")]
        public List<Loc> ListaElementoLoc { get; set; }
    }
    [XmlRoot(ElementName = "prevlocs")]
    public class Prevlocs
    {
        [XmlElement(ElementName = "loc")]
        public List<Loc> ListaElementoLoc { get; set; }
    }
    [XmlRoot(ElementName = "authlist")]
    public class Authlist
    {
        [XmlElement(ElementName = "author")]
        public List<Author> ListaElementoAuthor { get; set; }
    }
    [XmlRoot(ElementName = "author")]
    public class Author
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "affiliation")]
        public string Affiliation { get; set; }
        [XmlElement(ElementName = "email")]
        public Email ElementoEmail { get; set; }
        [XmlAttribute(AttributeName = "role")]
        public string Role { get; set; }
    }
    [XmlRoot(ElementName = "email")]
    public class Email
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
    }
    [XmlRoot(ElementName = "errataloc")]
    public class Errataloc
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
    }
    [XmlRoot(ElementName = "preverrataloc")]
    public class Preverrataloc
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
    }
    [XmlRoot(ElementName = "translationloc")]
    public class Translationloc
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
    }
    [XmlRoot(ElementName = "status")]
    public class Status
    {
        [XmlElement(ElementName = "p")]
        public List<P> ListaP { get; set; }
    }
    [XmlRoot(ElementName = "p")]
    public class P
    {
        [XmlElement(ElementName = "emph")]
        public List<string> ListaEmph { get; set; }
        [XmlElement(ElementName = "term")]
        public List<string> ListaTerm { get; set; }
        [XmlElement(ElementName = "rfc2119")]
        public List<string> ListaRfc2119 { get; set; }
        [XmlElement(ElementName = "quote")]
        public string Quote { get; set; }
        [XmlElement(ElementName = "el")]
        public List<string> ListaEl { get; set; }
        [XmlElement(ElementName = "code")]
        public List<string> ListaCode { get; set; }
        [XmlElement(ElementName = "attval")]
        public List<string> ListaAttval { get; set; }
        [XmlElement(ElementName = "att")]
        public List<string> ListaAtt { get; set; }
        [XmlElement(ElementName = "kw")]
        public List<string> ListaKw { get; set; }
        [XmlElement(ElementName = "var")]
        public string Var { get; set; }
        [XmlElement(ElementName = "loc")]
        public List<Loc> ListaElementoLoc { get; set; }
        [XmlElement(ElementName = "termref")]
        public List<Termref> ListaElementoTermref { get; set; }
        [XmlElement(ElementName = "bibref")]
        public List<Bibref> ListaElementoBibref { get; set; }
        [XmlElement(ElementName = "termdef")]
        public List<Termdef> ListaElementoTermdef { get; set; }
        [XmlElement(ElementName = "phrase")]
        public List<Phrase> ListaElementoPhrase { get; set; }
        [XmlElement(ElementName = "glist")]
        public List<Glist> ListaElementoGlist { get; set; }
        [XmlElement(ElementName = "specref")]
        public Specref ElementoSpecref { get; set; }
        [XmlElement(ElementName = "nt")]
        public List<Nt> ListaElementoNt { get; set; }
        [XmlElement(ElementName = "titleref")]
        public List<Titleref> ListaElementoTitleref { get; set; }
        [XmlAttribute(AttributeName = "role")]
        public string Role { get; set; }
        [XmlAttribute(AttributeName = "diff")]
        public string Diff { get; set; }
    }
    [XmlRoot(ElementName = "langusage")]
    public class Langusage
    {
        [XmlElement(ElementName = "language")]
        public List<Language> ListaElementoLanguage { get; set; }
    }
    [XmlRoot(ElementName = "language")]
    public class Language
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }
    [XmlRoot(ElementName = "body")]
    public class Body
    {
        [XmlElement(ElementName = "div1")]
        public List<Div1> ListaElementoDiv1 { get; set; }
    }
    [XmlRoot(ElementName = "div1")]
    public class Div1
    {
        [XmlElement(ElementName = "p")]
        public List<P> ListaP { get; set; }
        [XmlElement(ElementName = "head")]
        public string Head { get; set; }
        [XmlElement(ElementName = "eg")]
        public List<Eg> ListaEg { get; set; }
        [XmlElement(ElementName = "div2")]
        public List<Div2> ListaElementoDiv2 { get; set; }
        [XmlElement(ElementName = "scrap")]
        public Scrap ElementoScrap { get; set; }
        [XmlElement(ElementName = "wfcnote")]
        public Wfcnote ElementoWfcnote { get; set; }
        [XmlElement(ElementName = "vcnote")]
        public Vcnote ElementoVcnote { get; set; }
        [XmlElement(ElementName = "ulist")]
        public Ulist ElementoUlist { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }
    [XmlRoot(ElementName = "termref")]
    public class Termref
    {
        [XmlAttribute(AttributeName = "def")]
        public string Def { get; set; }
    }
    [XmlRoot(ElementName = "bibref")]
    public class Bibref
    {
        [XmlAttribute(AttributeName = "ref")]
        public string Ref { get; set; }
    }
    [XmlRoot(ElementName = "termdef")]
    public class Termdef
    {
        [XmlElement(ElementName = "emph")]
        public List<string> ListaEmph { get; set; }
        [XmlElement(ElementName = "term")]
        public List<string> ListaTerm { get; set; }
        [XmlElement(ElementName = "rfc2119")]
        public List<string> ListaRfc2119 { get; set; }
        [XmlElement(ElementName = "quote")]
        public string Quote { get; set; }
        [XmlElement(ElementName = "el")]
        public List<string> ListaEl { get; set; }
        [XmlElement(ElementName = "code")]
        public List<string> ListaCode { get; set; }
        [XmlElement(ElementName = "kw")]
        public List<string> ListaKw { get; set; }
        [XmlElement(ElementName = "termref")]
        public List<Termref> ListaElementoTermref { get; set; }
        [XmlElement(ElementName = "specref")]
        public Specref ElementoSpecref { get; set; }
        [XmlElement(ElementName = "bibref")]
        public Bibref ElementoBibref { get; set; }
        [XmlElement(ElementName = "nt")]
        public List<Nt> ListaElementoNt { get; set; }
        [XmlElement(ElementName = "titleref")]
        public List<Titleref> ListaElementoTitleref { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "term")]
        public string Term { get; set; }
    }
    [XmlRoot(ElementName = "div2")]
    public class Div2
    {
        [XmlElement(ElementName = "p")]
        public List<P> ListaP { get; set; }
        [XmlElement(ElementName = "head")]
        public string Head { get; set; }
        [XmlElement(ElementName = "eg")]
        public List<Eg> ListaEg { get; set; }
        [XmlElement(ElementName = "olist")]
        public List<Olist> ListaElementoOlist { get; set; }
        [XmlElement(ElementName = "scrap")]
        public List<Scrap> ListaElementoScrap { get; set; }
        [XmlElement(ElementName = "note")]
        public List<Note> ListaElementoNote { get; set; }
        [XmlElement(ElementName = "vcnote")]
        public List<Vcnote> ListaElementoVcnote { get; set; }
        [XmlElement(ElementName = "wfcnote")]
        public List<Wfcnote> ListaElementoWfcnote { get; set; }
        [XmlElement(ElementName = "div3")]
        public List<Div3> ListaElementoDiv3 { get; set; }
        [XmlElement(ElementName = "ulist")]
        public Ulist ElementoUlist { get; set; }
        [XmlElement(ElementName = "table")]
        public List<Table> ListaElementoTable { get; set; }
        [XmlElement(ElementName = "blist")]
        public Blist ElementoBlist { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }
    [XmlRoot(ElementName = "olist")]
    public class Olist
    {
        [XmlElement(ElementName = "item")]
        public List<string> ListaItem { get; set; }
    }
    [XmlRoot(ElementName = "phrase")]
    public class Phrase
    {
        [XmlElement(ElementName = "quote")]
        public string Quote { get; set; }
        [XmlElement(ElementName = "loc")]
        public Loc ElementoLoc { get; set; }
        [XmlElement(ElementName = "bibref")]
        public List<Bibref> ListaElementoBibref { get; set; }
        [XmlAttribute(AttributeName = "diff")]
        public string Diff { get; set; }
    }
    [XmlRoot(ElementName = "glist")]
    public class Glist
    {
        [XmlElement(ElementName = "gitem")]
        public List<Gitem> ListaElementoGitem { get; set; }
    }
    [XmlRoot(ElementName = "gitem")]
    public class Gitem
    {
        [XmlElement(ElementName = "label")]
        public string Label { get; set; }
        [XmlElement(ElementName = "def")]
        public string Def { get; set; }
    }
    [XmlRoot(ElementName = "specref")]
    public class Specref
    {
        [XmlAttribute(AttributeName = "ref")]
        public string Ref { get; set; }
    }
    [XmlRoot(ElementName = "nt")]
    public class Nt
    {
        [XmlAttribute(AttributeName = "def")]
        public string Def { get; set; }
    }
    [XmlRoot(ElementName = "scrap")]
    public class Scrap
    {
        [XmlElement(ElementName = "head")]
        public string Head { get; set; }
        [XmlElement(ElementName = "prod")]
        public List<Prod> ListaElementoProd { get; set; }
        [XmlElement(ElementName = "prodgroup")]
        public Prodgroup ElementoProdgroup { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "lang")]
        public string Lang { get; set; }
    }
    [XmlRoot(ElementName = "prod")]
    public class Prod
    {
        [XmlElement(ElementName = "lhs")]
        public Lhs Lhs { get; set; }
        [XmlElement(ElementName = "com")]
        public Com Com { get; set; }
        [XmlElement(ElementName = "rhs")]
        public List<Rhs> ListaElementoRhs { get; set; }
        [XmlElement(ElementName = "vc")]
        public List<Vc> ListaElementoVc { get; set; }
        [XmlElement(ElementName = "wfc")]
        public List<Wfc> ListaElementoWfc { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "num")]
        public string Num { get; set; }
        [XmlAttribute(AttributeName = "diff")]
        public string Diff { get; set; }
    }
    [XmlRoot(ElementName = "rhs")]
    public class Rhs
    {
        [XmlElement(ElementName = "nt")]
        public List<Nt> ListaElementoNt { get; set; }
        [XmlAttribute(AttributeName = "diff")]
        public string Diff { get; set; }
    }
    [XmlRoot(ElementName = "prodgroup")]
    public class Prodgroup
    {
        [XmlElement(ElementName = "prod")]
        public List<Prod> ListaElementoProd { get; set; }
        [XmlAttribute(AttributeName = "pcw2")]
        public decimal Pcw2 { get; set; }
        [XmlAttribute(AttributeName = "pcw4")]
        public decimal Pcw4 { get; set; }
        [XmlAttribute(AttributeName = "pcw5")]
        public decimal Pcw5 { get; set; }
        [XmlAttribute(AttributeName = "pcw3")]
        public decimal Pcw3 { get; set; }
    }
    [XmlRoot(ElementName = "note")]
    public class Note
    {
        [XmlElement(ElementName = "p")]
        public List<P> ListaP { get; set; }
        [XmlElement(ElementName = "eg")]
        public List<Eg> ListaEg { get; set; }
        [XmlAttribute(AttributeName = "diff")]
        public string Diff { get; set; }
    }
    [XmlRoot(ElementName = "lhs")]
    public class Lhs
    {
        [XmlAttribute(AttributeName = "diff")]
        public string Diff { get; set; }
    }
    [XmlRoot(ElementName = "com")]
    public class Com
    {
        [XmlElement(ElementName = "loc")]
        public Loc ElementoLoc { get; set; }
        [XmlAttribute(AttributeName = "diff")]
        public string Diff { get; set; }
    }
    [XmlRoot(ElementName = "vc")]
    public class Vc
    {
        [XmlAttribute(AttributeName = "def")]
        public string Def { get; set; }
    }
    [XmlRoot(ElementName = "wfc")]
    public class Wfc
    {
        [XmlAttribute(AttributeName = "def")]
        public string Def { get; set; }
    }
    [XmlRoot(ElementName = "vcnote")]
    public class Vcnote
    {
        [XmlElement(ElementName = "p")]
        public List<P> ListaP { get; set; }
        [XmlElement(ElementName = "head")]
        public string Head { get; set; }
        [XmlElement(ElementName = "ulist")]
        public Ulist ElementoUlist { get; set; }
        [XmlElement(ElementName = "olist")]
        public Olist ElementoOlist { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }
    [XmlRoot(ElementName = "wfcnote")]
    public class Wfcnote
    {
        [XmlElement(ElementName = "p")]
        public List<P> ListaP { get; set; }
        [XmlElement(ElementName = "head")]
        public string Head { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }
    [XmlRoot(ElementName = "ulist")]
    public class Ulist
    {
        [XmlElement(ElementName = "item")]
        public List<string> ListaItem { get; set; }
        [XmlAttribute(AttributeName = "spacing")]
        public string Spacing { get; set; }
    }
    [XmlRoot(ElementName = "titleref")]
    public class Titleref
    {
        [XmlElement(ElementName = "loc")]
        public List<Loc> ListaElementoLoc { get; set; }
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
    }
    [XmlRoot(ElementName = "div3")]
    public class Div3
    {
        [XmlElement(ElementName = "p")]
        public List<P> ListaP { get; set; }
        [XmlElement(ElementName = "head")]
        public string Head { get; set; }
        [XmlElement(ElementName = "eg")]
        public List<Eg> ListaEg { get; set; }
        [XmlElement(ElementName = "scrap")]
        public List<Scrap> ListaElementoScrap { get; set; }
        [XmlElement(ElementName = "vcnote")]
        public List<Vcnote> ListaElementoVcnote { get; set; }
        [XmlElement(ElementName = "olist")]
        public Olist ElementoOlist { get; set; }
        [XmlElement(ElementName = "table")]
        public Table ElementoTable { get; set; }
        [XmlElement(ElementName = "note")]
        public Note ElementoNote { get; set; }
        [XmlElement(ElementName = "ulist")]
        public Ulist ElementoUlist { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }
    [XmlRoot(ElementName = "item")]
    public class Item
    {
        [XmlElement(ElementName = "p")]
        public List<P> ListaP { get; set; }
        [XmlElement(ElementName = "ulist")]
        public Ulist ElementoUlist { get; set; }
    }
    [XmlRoot(ElementName = "table")]
    public class Table
    {
        [XmlElement(ElementName = "thead")]
        public string Thead { get; set; }
        [XmlElement(ElementName = "tbody")]
        public Tbody ElementoTbody { get; set; }
        [XmlAttribute(AttributeName = "border")]
        public decimal Border { get; set; }
        [XmlAttribute(AttributeName = "frame")]
        public string Frame { get; set; }
        [XmlAttribute(AttributeName = "cellpadding")]
        public decimal Cellpadding { get; set; }
    }
    [XmlRoot(ElementName = "tr")]
    public class Tr
    {
        [XmlElement(ElementName = "th")]
        public List<string> ListaTh { get; set; }
        [XmlElement(ElementName = "td")]
        public List<Td> ListaTd { get; set; }
        [XmlAttribute(AttributeName = "align")]
        public string Align { get; set; }
        [XmlAttribute(AttributeName = "valign")]
        public string Valign { get; set; }
    }
    [XmlRoot(ElementName = "tbody")]
    public class Tbody
    {
        [XmlElement(ElementName = "tr")]
        public List<Tr> ListaElementoTr { get; set; }
        [XmlAttribute(AttributeName = "align")]
        public string Align { get; set; }
    }
    [XmlRoot(ElementName = "quote")]
    public class Quote
    {
        [XmlElement(ElementName = "code")]
        public List<string> ListaCode { get; set; }
        [XmlElement(ElementName = "var")]
        public string Var { get; set; }
    }
    [XmlRoot(ElementName = "def")]
    public class Def
    {
        [XmlElement(ElementName = "p")]
        public List<P> ListaP { get; set; }
    }
    [XmlRoot(ElementName = "td")]
    public class Td
    {
        [XmlElement(ElementName = "code")]
        public List<string> ListaCode { get; set; }
        [XmlElement(ElementName = "titleref")]
        public Titleref ElementoTitleref { get; set; }
        [XmlAttribute(AttributeName = "rowspan")]
        public decimal Rowspan { get; set; }
        [XmlAttribute(AttributeName = "colspan")]
        public decimal Colspan { get; set; }
        [XmlAttribute(AttributeName = "align")]
        public string Align { get; set; }
        [XmlAttribute(AttributeName = "valign")]
        public string Valign { get; set; }
    }
    [XmlRoot(ElementName = "eg")]
    public class Eg
    {
        [XmlElement(ElementName = "loc")]
        public Loc ElementoLoc { get; set; }
        [XmlAttribute(AttributeName = "lang")]
        public string Lang { get; set; }
        [XmlAttribute(AttributeName = "diff")]
        public string Diff { get; set; }
    }
    [XmlRoot(ElementName = "label")]
    public class Label
    {
        [XmlElement(ElementName = "code")]
        public List<string> ListaCode { get; set; }
    }
    [XmlRoot(ElementName = "back")]
    public class Back
    {
        [XmlElement(ElementName = "div1")]
        public List<Div1> ListaElementoDiv1 { get; set; }
        [XmlElement(ElementName = "inform-div1")]
        public List<Informdiv1> ListaElementoInformdiv1 { get; set; }
    }
    [XmlRoot(ElementName = "blist")]
    public class Blist
    {
        [XmlElement(ElementName = "bibl")]
        public List<Bibl> ListaElementoBibl { get; set; }
    }
    [XmlRoot(ElementName = "bibl")]
    public class Bibl
    {
        [XmlElement(ElementName = "emph")]
        public List<string> ListaEmph { get; set; }
        [XmlElement(ElementName = "titleref")]
        public List<Titleref> ListaElementoTitleref { get; set; }
        [XmlElement(ElementName = "loc")]
        public List<Loc> ListaElementoLoc { get; set; }
        [XmlElement(ElementName = "phrase")]
        public List<Phrase> ListaElementoPhrase { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "key")]
        public string Key { get; set; }
        [XmlAttribute(AttributeName = "diff")]
        public string Diff { get; set; }
    }
    [XmlRoot(ElementName = "inform-div1")]
    public class Informdiv1
    {
        [XmlElement(ElementName = "p")]
        public List<P> ListaP { get; set; }
        [XmlElement(ElementName = "head")]
        public string Head { get; set; }
        [XmlElement(ElementName = "eg")]
        public List<Eg> ListaEg { get; set; }
        [XmlElement(ElementName = "ulist")]
        public Ulist ElementoUlist { get; set; }
        [XmlElement(ElementName = "div2")]
        public List<Div2> ListaElementoDiv2 { get; set; }
        [XmlElement(ElementName = "orglist")]
        public Orglist ElementoOrglist { get; set; }
        [XmlElement(ElementName = "olist")]
        public Olist ElementoOlist { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "diff")]
        public string Diff { get; set; }
    }
    [XmlRoot(ElementName = "orglist")]
    public class Orglist
    {
        [XmlElement(ElementName = "member")]
        public List<Member> ListaElementoMember { get; set; }
    }
    [XmlRoot(ElementName = "member")]
    public class Member
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "affiliation")]
        public string Affiliation { get; set; }
        [XmlElement(ElementName = "role")]
        public string Role { get; set; }
    }}