Imports Portal.General.Facade
Imports Portal.General.DataAccess
Imports Portal.Catalogos.Facade
Imports Portal.Catalogos.Common.Data
Imports System.Collections.Generic
Imports System.Configuration.ConfigurationManager
Imports CrystalDecisions.CrystalReports.Engine
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Globalization
Imports System.Xml
Imports ReferencesSystem

Imports Portal.Hotel.Facade
Imports Portal.Hotel.Common.Data
Imports Portal.General.Common.Data


Partial Public Class ConveniosCasas
    Inherits PaginaBase
    Enum dgColAgreement
        IdConvenio
        Referencia
        Nombre
        ConvenioDescription
        Seleccionar
    End Enum

    Enum dgColHomoClaves
        idConvenioHomoClave
        HomoClave
        Company
        Contact
        Eliminar
    End Enum
    Public ReadOnly Property idCorporate() As Integer
        Get
            Dim RES As Integer
            If IsSupervisor AndAlso Integer.TryParse(idSelectedCorpororate.Value, RES) Then
                Return RES
            ElseIf IdCorporativoUserChain > -1 AndAlso Integer.TryParse(IdCorporativoUserChain, RES) Then
                Return RES
            Else
                Return -1
            End If
        End Get
    End Property
    Public ReadOnly Property idConvenio() As Integer
        Get
            If dgAgreementWorking.SelectedIndex = -1 Then
                Return -1
            Else
                Return dgAgreementWorking.DataKeys(dgAgreementWorking.SelectedIndex)
            End If
        End Get
    End Property

    Private Const ATR_HOTEL = "Hotel"
    Private Const ATR_RATEPLAN = "RateCode"
    Private Const ATR_INISTATUS = "IniState"

    Public Property dsConvenioHomoClave() As ConvenioHomoClaveData
        Get
            If ViewState("dsConvenioHomoClave") Is Nothing Then
                Return New ConvenioHomoClaveData()
            Else
                Return ViewState("dsConvenioHomoClave")
            End If

        End Get
        Set(ByVal value As ConvenioHomoClaveData)
            ViewState("dsConvenioHomoClave") = value
        End Set
    End Property
    'Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
    '    Dim stringWriter As New System.IO.StringWriter()
    '    Dim htmlWriter As New HtmlTextWriter(stringWriter)
    '    MyBase.Render(htmlWriter)
    '    Dim html As String = stringWriter.ToString()
    '    Dim endPoint As Integer
    '    Dim startPoint As Integer = html.IndexOf("<input type=""hidden"" name=""__VIEWSTATE""")
    '    If (startPoint >= 0) Then
    '        endPoint = html.IndexOf("/>", startPoint) + 2
    '        html = html.Remove(startPoint, endPoint - startPoint)
    '    End If
    '    writer.Write(html)
    'End Sub

    'Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
    '    Return Session("ViewState")
    'End Function

    'Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
    '    Session("ViewState") = viewState
    '    ClientScript.RegisterHiddenField("__VIEWSTATE", "")
    'End Sub

    Private Function CalulaDigitoVerificador(ByVal ref As String) As String
        Return (New BBVA_A36).CalculateDigitVerifier(ref)
    End Function


    Private Sub Convenios_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not Session("AgreementData") Is Nothing Then
            ReportProcessDS()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not IsPostBack Then
            If Request.QueryString("a") IsNot Nothing Then
                Dim result As New StringBuilder
                Response.Clear()

                Select Case Request.QueryString("a").ToLower()
                    Case "searchrates"
                        If Request.QueryString("h") IsNot Nothing Then
                            Dim dsRates As DataSet
                            dsRates = (New RatePlanAccess).GetListRatesByKeyword(CInt(Request.QueryString("h")), Request.QueryString("q"), PortalCulture.GetIDCulture)
                            If dsRates IsNot Nothing Then
                                For Each dr As DataRow In dsRates.Tables(0).Rows
                                    result.AppendFormat("{0} - {1}|{2}{3}", dr("codigoTarifa"), dr("name"), dr("idRatePlan"), vbNewLine)
                                Next
                            End If
                        End If
                    Case "searchrate"
                        Dim ratePlanToSend As RatePlanAccess.RatePlan = GetRatePlan(CInt(Request.QueryString("h")), Request.QueryString("r"), Request.QueryString("ag"), True)
                        Dim theConverter As New Converters.IsoDateTimeConverter()
                        theConverter.DateTimeFormat = "MM/dd/yyyy"
                        If ratePlanToSend IsNot Nothing Then result.AppendLine(JsonConvert.SerializeObject(ratePlanToSend, theConverter))
                    Case "getrates"
                        If Request.QueryString("h") IsNot Nothing Then
                            Dim dsRates As DataSet
                            dsRates = (New RatePlanAccess).GetListRatesByIdHotel(CInt(Request.QueryString("h")), PortalCulture.GetIDCulture)
                            If dsRates IsNot Nothing Then
                                Dim sb As New System.Text.StringBuilder()
                                Dim tw As New System.IO.StringWriter(sb)
                                Dim hw As New HtmlTextWriter(tw)

                                dgRatesPlan.Columns(0).HeaderText = PortalCulture.GetString("00016")
                                dgRatesPlan.DataSource = dsRates
                                dgRatesPlan.DataBind()
                                dgRatesPlan.Visible = True
                                dgRatesPlan.RenderControl(hw)
                                dgRatesPlan.Visible = False

                                result.Append(sb.ToString())
                            End If
                        End If
                    Case "getoffices"
                        If Request.QueryString("c") IsNot Nothing Then

                            Dim corporate As Integer = 0
                            Integer.TryParse(Request.QueryString("c"), corporate)
                            If corporate > 0 Then
                                Dim adapter As New SqlClient.SqlDataAdapter("spGetCorporateOffices", ConfigurationSettings.AppSettings("HotelConnectionString"))
                                adapter.SelectCommand.CommandType = CommandType.StoredProcedure
                                adapter.SelectCommand.Parameters.Add("@idCorporate", SqlDbType.Int).Value = corporate

                                Dim data As New Data.DataTable()
                                Dim list As New JArray()
                                adapter.Fill(data)

                                If data IsNot Nothing Then
                                    For Each record As DataRow In data.Rows
                                        list.Add(New JObject( _
                                             New JProperty("id", JToken.FromObject(record("id"))), _
                                             New JProperty("name", JToken.FromObject(record("name"))) _
                                        ))
                                    Next
                                End If
                                result.Append(JsonConvert.SerializeObject(list))

                            End If

                        End If
                    Case "getcontacts"
                        If Request.QueryString("o") IsNot Nothing Then
                            Dim office As Integer = 0
                            Integer.TryParse(Request.QueryString("o"), office)
                            If office > 0 Then
                                Dim adapter As New SqlClient.SqlDataAdapter("spGetCorporateContactOffices", ConfigurationSettings.AppSettings("HotelConnectionString"))
                                adapter.SelectCommand.CommandType = CommandType.StoredProcedure
                                adapter.SelectCommand.Parameters.Add("@idOffice", SqlDbType.Int).Value = office

                                Dim data As New Data.DataTable()
                                Dim list As New JArray()
                                adapter.Fill(data)

                                If data IsNot Nothing Then
                                    For Each record As DataRow In data.Rows
                                        list.Add(New JObject( _
                                             New JProperty("id", JToken.FromObject(record("id"))), _
                                             New JProperty("position", JToken.FromObject(record("position"))), _
                                             New JProperty("name", JToken.FromObject(record("name"))) _
                                        ))
                                    Next
                                End If
                                result.Append(JsonConvert.SerializeObject(list))

                            End If
                        End If
                    Case "getagreements"
                        If Request.QueryString("c") IsNot Nothing Then
                            Dim idcorp As Integer = 0
                            Integer.TryParse(Request.QueryString("c"), idcorp)
                            If idcorp > 0 Then
                                Dim ds As DataSet = (New RatePlanAccess).GetAgreementsByIdCorporate(idcorp)
                                Dim dsRates As DataSet = (New RatePlanAccess).GetRatesPlanByIdCorporate(idcorp)

                                Dim rCell As String = ""
                                Dim row As DataRow
                                Dim x As Integer
                                Dim y As Integer = 0

                                ds.Tables(0).Columns.Add("RatePlans")

                                For x = 0 To ds.Tables(0).Rows.Count - 1


                                    Dim drs() As DataRow = dsRates.Tables(0).Select("idConvenio=" + Convert.ToString(ds.Tables(0).Rows(x)("idConvenio")))
                                    For Each DrsRow As DataRow In drs
                                        rCell = rCell + DrsRow("idRatePlan") + ","
                                        y = y + 1
                                    Next

                                    ds.Tables(0).Rows(x)("RatePlans") = rCell
                                    rCell = ""
                                Next

                                Dim data As New Data.DataTable()
                                Dim list As New JArray()


                                If ds IsNot Nothing Then
                                    For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                                        list.Add(New JObject( _
                                                 New JProperty("Numero", JToken.FromObject((i + 1).ToString())), _
                                             New JProperty("idConvenio", JToken.FromObject(ds.Tables(0).Rows(i)("idConvenio"))), _
                                             New JProperty("Referencia", JToken.FromObject(ds.Tables(0).Rows(i)("Referencia"))), _
                                             New JProperty("Nombre", JToken.FromObject(ds.Tables(0).Rows(i)("Nombre"))), _
                                             New JProperty("idRatePlan", JToken.FromObject(ds.Tables(0).Rows(i)("RatePlans"))) _
                                        ))
                                    Next
                                End If
                                result.Append(JsonConvert.SerializeObject(list))

                            End If
                        End If
                End Select
                Response.Write(result.ToString)
                Response.End()
            Else
                ClientScript.RegisterStartupScript(Me.GetType(), "Inicializa Ciudad", "SetInitialLocation('MX', '', '', '');", True)
                LoadCorporates(False)
                Dim sdatosdespues As String

                If Request.QueryString("cnv") IsNot Nothing Then
                    LoadAgreement(Request.QueryString("cnv"), sdatosdespues)
                    hdnIdAgreement.Value = Request.QueryString("cnv")
                    txtNoAgreement.Enabled = False
                End If
            End If
            Call LoadSegments()

        End If

    End Sub

    Public Sub LoadSegments()
        cmbSegmento.Items.Add(PortalCulture.GetString("00838", False)) 'Corporativo
        cmbSegmento.Items.Add(PortalCulture.GetString("01590", False)) 'Agencias
        cmbSegmento.Items.Add(PortalCulture.GetString("01589", False)) 'Intercambio
        cmbSegmento.Items(0).Value = 0
        cmbSegmento.Items(1).Value = 1
        cmbSegmento.Items(2).Value = 2
    End Sub

    Private Function GetRatePlan(ByVal idHotel As Integer, ByVal idRateplan As String, Optional ByVal idAggregment As Integer = 0, Optional ByVal validateDate As Boolean = False) As RatePlanAccess.RatePlan
        Dim ratePlanToSend As RatePlanAccess.RatePlan
        Dim dsRate As DataSet

        If idAggregment = 0 AndAlso idConvenio > 0 Then
            'idAggregment = Me.ddlAgreementWorking.Items(Me.ddlAgreementWorking.SelectedIndex).Value
            idAggregment = idConvenio
        End If

        dsRate = (New RatePlanAccess).GetRateInformation(idHotel, idRateplan, PortalCulture.GetIDCulture, If(idAggregment < 0, 0, idAggregment))
        If dsRate IsNot Nothing Then
            If dsRate.Tables.Count = 3 AndAlso dsRate.Tables(0).Rows.Count = 1 AndAlso dsRate.Tables(1).Rows.Count > 0 AndAlso dsRate.Tables(2).Rows.Count > 0 Then
                ratePlanToSend = New RatePlanAccess.RatePlan
                ratePlanToSend.IdHotel = idHotel
                ratePlanToSend.IdRatePlan = dsRate.Tables(0).Rows(0)("IdRatePlan")
                ratePlanToSend.RateCode = dsRate.Tables(0).Rows(0)("CodigoTarifa")
                ratePlanToSend.Name = dsRate.Tables(0).Rows(0)("Name")
                ratePlanToSend.Description = dsRate.Tables(0).Rows(0)("Description")

                ratePlanToSend.Currency = dsRate.Tables(0).Rows(0)("Currency")

                ratePlanToSend.Vigencia = If(dsRate.Tables(0).Rows(0)("Vigencia").Equals(DBNull.Value), New Date(), dsRate.Tables(0).Rows(0)("Vigencia"))

                Dim idxRates As Integer = 0
                For Each dr As DataRow In dsRate.Tables(1).Rows
                    If (Not validateDate OrElse Convert.ToDateTime(dr("FechaFinaliza")) >= Today) Then
                        Dim drRestrictions() As DataRow = dsRate.Tables(2).Select("IdTarifa=" & dr("IdTarifa"))

                        ReDim Preserve ratePlanToSend.Rates(idxRates)
                        ratePlanToSend.Rates(idxRates) = New RatePlanAccess.RatePlan.Rate
                        ratePlanToSend.Rates(idxRates).IdRate = CInt(dr("IdTarifa"))

                        Dim current As System.Globalization.CultureInfo
                        current = System.Threading.Thread.CurrentThread.CurrentCulture
                        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
                        ratePlanToSend.Rates(idxRates).DateStart = String.Format("{0:dd MMMM, yyyy}", dr("FechaInicia"))
                        ratePlanToSend.Rates(idxRates).DateEnd = String.Format("{0:dd MMMM, yyyy}", dr("FechaFinaliza"))
                        System.Threading.Thread.CurrentThread.CurrentCulture = current

                        Dim excepciones() As String = GetDays(dr("Excepciones"))

                        Dim idxRestrictions As Integer = 0
                        Dim minAdults As Integer = -1
                        Dim maxAdults As Integer = -1
                        Dim minChildren As Integer = -1
                        Dim maxChildren As Integer = -1
                        Dim idsRestrictions As String = String.Empty

                        For i As Integer = 0 To drRestrictions.Length - 1
                            Dim adultsNext As Integer
                            Dim adultRateNext As Double
                            Dim childRateNext As Double
                            Dim noAdults() As Integer
                            Dim idTypeRoomNext As Integer
                            Dim ChildrenRates() As RatePlanAccess.RatePlan.Rate.Restriction.ChildRate
                            Dim adultRate As Double = CDbl(drRestrictions(i)("TarifaAdulto"))
                            Dim childRate As Double = CDbl(drRestrictions(i)("TarifaNinio"))
                            Dim teenRate As Double = Convert.ToDouble(drRestrictions(i)("TarifaAdolecente"))
                            Dim adults As Integer = CInt(drRestrictions(i)("Adultos"))
                            Dim children As Integer = CInt(drRestrictions(i)("Ninios"))
                            Dim adultRateExc As Double = CDbl(drRestrictions(i)("TarifaAdultoExc"))
                            Dim childRateExc As Double = CDbl(drRestrictions(i)("TarifaNinioExc"))
                            Dim teenRateExc As Double = Convert.ToDouble(drRestrictions(i)("TarifaAdolecenteExc"))
                            Dim adultPriceExt As Double = CDbl(drRestrictions(i)("PrecioExtraAdulto"))
                            Dim childPriceExt As Double = CDbl(drRestrictions(i)("PrecioExtraNinio"))
                            Dim teenPriceExt As Double = CDbl(drRestrictions(i)("PrecioExtraAdolecente"))

                            If adults <= CInt(drRestrictions(i)("MaxAdultos")) Then
                                If i < drRestrictions.Length - 1 Then
                                    adultsNext = CInt(drRestrictions(i + 1)("Adultos"))
                                    adultRateNext = CDbl(drRestrictions(i + 1)("TarifaAdulto"))
                                    childRateNext = CDbl(drRestrictions(i + 1)("TarifaNinio"))
                                    idTypeRoomNext = CDbl(drRestrictions(i + 1)("IdTipoHabitacion_Hotel"))
                                Else
                                    adultsNext = -1
                                    adultRateNext = -1
                                    childRateNext = -1
                                    idTypeRoomNext = -1
                                End If

                                Dim foundAdult As Boolean = False
                                If noAdults IsNot Nothing Then
                                    For Each noAdult As Integer In noAdults
                                        If noAdult = adults Then foundAdult = True
                                    Next
                                End If

                                If Not foundAdult Then
                                    Dim idx As Integer = 0
                                    If noAdults IsNot Nothing Then idx = noAdults.Length
                                    ReDim Preserve noAdults(idx)
                                    noAdults(noAdults.Length - 1) = adults
                                End If

                                If adultRate = adultRateNext And CInt(drRestrictions(i)("IdTipoHabitacion_Hotel")) = idTypeRoomNext And adultsNext <= CInt(drRestrictions(i)("MaxAdultos")) Then
                                    If idsRestrictions Is String.Empty Then
                                        minAdults = CInt(drRestrictions(i)("MaxAdultos"))
                                        minChildren = CInt(drRestrictions(i)("MaxNinios"))
                                        maxAdults = 1
                                        maxChildren = 1
                                    End If

                                    idsRestrictions &= drRestrictions(i)("IdRestriccion") & "-"

                                    GetlinkPrice(drRestrictions(i), adults, children, adultRate, childRate, adultRateExc, childRateExc, adultPriceExt, childPriceExt)

                                    If children > 0 And children <= CInt(drRestrictions(i)("MaxNinios")) Then
                                        Dim foundChild As Boolean = False

                                        If ChildrenRates IsNot Nothing Then
                                            For Each child As RatePlanAccess.RatePlan.Rate.Restriction.ChildRate In ChildrenRates
                                                If child.Count = children Then
                                                    foundChild = True
                                                    Exit For
                                                End If
                                            Next
                                        End If

                                        If Not foundChild Then
                                            Dim idx As Integer = 0
                                            If ChildrenRates IsNot Nothing Then idx = ChildrenRates.Length
                                            ReDim Preserve ChildrenRates(idx)
                                            ChildrenRates(ChildrenRates.Length - 1) = New RatePlanAccess.RatePlan.Rate.Restriction.ChildRate
                                            ChildrenRates(ChildrenRates.Length - 1).Count = children
                                            ChildrenRates(ChildrenRates.Length - 1).Rate = childRate
                                            ChildrenRates(ChildrenRates.Length - 1).RateExc = childRateExc
                                            ChildrenRates(ChildrenRates.Length - 1).TeenRate = teenRate
                                            ChildrenRates(ChildrenRates.Length - 1).TeenRateExc = teenRateExc
                                        End If
                                    End If

                                    If minAdults > CInt(drRestrictions(i)("Adultos")) Then minAdults = adults
                                    If maxAdults < CInt(drRestrictions(i)("Adultos")) Then maxAdults = adults
                                    If minChildren > CInt(drRestrictions(i)("Ninios")) Then minChildren = children
                                    If maxChildren < CInt(drRestrictions(i)("Ninios")) Then maxChildren = children
                                Else
                                    GetlinkPrice(drRestrictions(i), adults, children, adultRate, childRate, adultRateExc, childRateExc, adultPriceExt, childPriceExt)

                                    If children > 0 And children <= CInt(drRestrictions(i)("MaxNinios")) Then
                                        Dim foundChild As Boolean = False

                                        If ChildrenRates IsNot Nothing Then
                                            For Each child As RatePlanAccess.RatePlan.Rate.Restriction.ChildRate In ChildrenRates
                                                If child.Count = children Then
                                                    foundChild = True
                                                    Exit For
                                                End If
                                            Next
                                        End If

                                        Dim idx As Integer = 0
                                        If Not foundChild Then
                                            If ChildrenRates IsNot Nothing Then idx = ChildrenRates.Length
                                            ReDim Preserve ChildrenRates(idx)
                                            ChildrenRates(ChildrenRates.Length - 1) = New RatePlanAccess.RatePlan.Rate.Restriction.ChildRate
                                            ChildrenRates(ChildrenRates.Length - 1).Count = children
                                            ChildrenRates(ChildrenRates.Length - 1).Rate = childRate
                                            ChildrenRates(ChildrenRates.Length - 1).RateExc = childRateExc
                                            ChildrenRates(ChildrenRates.Length - 1).TeenRate = teenRate
                                            ChildrenRates(ChildrenRates.Length - 1).TeenRateExc = teenRateExc
                                        End If

                                        If children < CInt(drRestrictions(i)("MaxNinios")) Then
                                            For auxChildren As Integer = children + 1 To CInt(drRestrictions(i)("MaxNinios"))
                                                idx = ChildrenRates.Length
                                                ReDim Preserve ChildrenRates(idx)
                                                ChildrenRates(ChildrenRates.Length - 1) = New RatePlanAccess.RatePlan.Rate.Restriction.ChildRate
                                                ChildrenRates(ChildrenRates.Length - 1).Count = auxChildren
                                                ChildrenRates(ChildrenRates.Length - 1).Rate = childRate
                                                ChildrenRates(ChildrenRates.Length - 1).RateExc = childRateExc
                                                ChildrenRates(ChildrenRates.Length - 1).TeenRate = teenRate
                                                ChildrenRates(ChildrenRates.Length - 1).TeenRateExc = teenRateExc
                                            Next
                                        End If
                                    End If

                                    If minChildren > children Then minChildren = children
                                    If maxChildren < children Then maxChildren = children

                                    Dim idxExcepciones As Integer = 0
                                    For Each excepcion As String In excepciones
                                        If excepcion IsNot Nothing AndAlso excepcion IsNot String.Empty Then
                                            ReDim Preserve ratePlanToSend.Rates(idxRates).Restrictions(idxRestrictions)
                                            ratePlanToSend.Rates(idxRates).Restrictions(idxRestrictions) = New RatePlanAccess.RatePlan.Rate.Restriction
                                            ratePlanToSend.Rates(idxRates).Restrictions(idxRestrictions).IdRoomType = CInt(drRestrictions(i)("IdTipoHabitacion_Hotel"))
                                            ratePlanToSend.Rates(idxRates).Restrictions(idxRestrictions).IdsRestrictions = idsRestrictions & drRestrictions(i)("IdRestriccion")
                                            ratePlanToSend.Rates(idxRates).Restrictions(idxRestrictions).NameRoom = drRestrictions(i)("NameRoom")
                                            ratePlanToSend.Rates(idxRates).Restrictions(idxRestrictions).Exceptions = excepcion
                                            If idxExcepciones = 0 Then
                                                ratePlanToSend.Rates(idxRates).Restrictions(idxRestrictions).AdultRate = adultRate
                                                'ratePlanToSend.Rates(idxRates).Restrictions(idxRestrictions).TeenRate = teenRate
                                                ratePlanToSend.Rates(idxRates).Restrictions(idxRestrictions).isException = False
                                            Else
                                                ratePlanToSend.Rates(idxRates).Restrictions(idxRestrictions).AdultRate = adultRateExc
                                                'ratePlanToSend.Rates(idxRates).Restrictions(idxRestrictions).TeenRate = teenRateExc
                                                ratePlanToSend.Rates(idxRates).Restrictions(idxRestrictions).isException = True
                                            End If
                                            ratePlanToSend.Rates(idxRates).Restrictions(idxRestrictions).AdultRateExt = adultPriceExt
                                            ratePlanToSend.Rates(idxRates).Restrictions(idxRestrictions).TeenRateExt = teenPriceExt
                                            ratePlanToSend.Rates(idxRates).Restrictions(idxRestrictions).ChildRateExt = childPriceExt

                                            ratePlanToSend.Rates(idxRates).Restrictions(idxRestrictions).Adults = adults
                                            ratePlanToSend.Rates(idxRates).Restrictions(idxRestrictions).MaxAdults = IIf(maxAdults = -1 Or (maxAdults < CInt(drRestrictions(i)("MaxAdultos")) And Not CInt(drRestrictions(i)("IdTipoHabitacion_Hotel")) = idTypeRoomNext), CInt(drRestrictions(i)("MaxAdultos")), maxAdults)
                                            ratePlanToSend.Rates(idxRates).Restrictions(idxRestrictions).MaxChildren = IIf(maxChildren = -1 Or maxChildren < CInt(drRestrictions(i)("MaxNinios")), CInt(drRestrictions(i)("MaxNinios")), maxChildren)
                                            ratePlanToSend.Rates(idxRates).Restrictions(idxRestrictions).MinAdults = IIf(minAdults = -1, 1, minAdults)
                                            ratePlanToSend.Rates(idxRates).Restrictions(idxRestrictions).MinChildren = IIf(minChildren < 1, 1, minChildren)
                                            ratePlanToSend.Rates(idxRates).Restrictions(idxRestrictions).ChildrenRates = ChildrenRates
                                            ratePlanToSend.TotalRatesRestrictions += 1

                                            idxRestrictions += 1
                                        End If
                                        idxExcepciones += 1
                                    Next

                                    minAdults = -1
                                    maxAdults = -1
                                    minChildren = -1
                                    maxChildren = -1

                                    noAdults = Nothing
                                    ChildrenRates = Nothing
                                    idsRestrictions = String.Empty
                                End If
                            End If
                        Next

                        idxRates += 1
                    End If
                Next
            End If
        End If
        Return ratePlanToSend
    End Function

    Private Sub LoadAgreement(ByVal idAgreement As Integer, ByRef sDatoDespues As String, Optional ByVal onlyRatesPlan As Boolean = False)
        Dim dsAgreement As DataSet = (New RatePlanAccess).GetAgreement(idAgreement, PortalCulture.GetIDCulture)
        'carga los planes agregados al convenio
        dlRatesPlan.DataSource = New DataView(dsAgreement.Tables(1)).ToTable(True, "IdRatePlan")
        dlRatesPlan.DataBind()

        LoadHotelesConveio(String.Empty)

        sDatoDespues = Util.Utility.GetXml("table", "UpdateAgreement", dsAgreement, "Convenios")
        If dsAgreement IsNot Nothing AndAlso dsAgreement.Tables(0).Rows.Count > 0 AndAlso dsAgreement.Tables(1).Rows.Count > 0 Then
            LoadHomoClaves(idAgreement)
            If Not onlyRatesPlan Then

                'If (dsAgreement.Tables(0).Rows(0)("EsGeneral") IsNot DBNull.Value) Then Me.chkIsGeneral.Checked = Convert.ToBoolean(dsAgreement.Tables(0).Rows(0)("EsGeneral"))
                Me.chkIsGeneral.Checked = False
                Me.chkCC.Checked = Convert.ToBoolean(dsAgreement.Tables(0).Rows(0)("hascredit"))

                txtNoAgreement.Text = IIf(dsAgreement.Tables(0).Rows(0)("Referencia") Is DBNull.Value, "", dsAgreement.Tables(0).Rows(0)("Referencia"))
                'If Not Me.chkIsGeneral.Checked Then
                With dsAgreement.Tables(0).Rows(0)

                    txtAgency.Text = If(.IsNull("Empresa"), "", .Item("Empresa"))
                    txtAddress.Text = If(.IsNull("Domicilio"), "", .Item("Domicilio"))
                    txtTown.Text = If(.IsNull("Colonia"), "", .Item("Colonia"))
                    txtZIP.Text = If(.IsNull("CodigoPostal"), "", .Item("CodigoPostal"))
                    txtPhone.Text = If(.IsNull("Telefono"), "", .Item("Telefono"))
                    txtContact.Text = If(.IsNull("Contacto"), "", .Item("Contacto"))
                    txtJob.Text = If(.IsNull("Puesto"), "", .Item("Puesto"))
                    txtEmail.Text = If(.IsNull("email"), "", .Item("email"))
                    'chkIsAgency.Checked = (dsAgreement.Tables(0).Rows(0)("EsAgencia") IsNot DBNull.Value AndAlso Convert.ToBoolean(dsAgreement.Tables(0).Rows(0)("EsAgencia")))
                    cmbSegmento.SelectedValue = dsAgreement.Tables(0).Rows(0)("EsAgencia")
                    ddlhour.SelectedValue = "00"
                    ddlmin.SelectedValue = "00"
                    If Not .IsNull("checkinTime") Then
                        Try
                            Dim sD As Date
                            sD = .Item("checkinTime")
                            ddlhour.SelectedValue = sD.ToString("HH")
                            ddlmin.SelectedValue = sD.ToString("mm")
                        Catch ex As Exception

                        End Try
                    End If

                End With
                Dim estado As Integer = -1
                Dim municipio As Integer = -1
                Dim ciudad As Integer = IIf(dsAgreement.Tables(0).Rows(0)("idCiudad") Is DBNull.Value, 0, dsAgreement.Tables(0).Rows(0)("idCiudad"))
                Dim err As String
                Dim citydata As clsCommonCiudades

                citydata = (New clsFacadeCiudades).GetById(ciudad, err)
                If Not IsNothing(citydata) Then
                    With citydata.Tables(clsCommonCiudades.TABLA_CIUDADES).Rows(0)
                        Dim fx As String = "SetInitialLocation("
                        fx += "'" + .Item("idPais").ToString() + "', "
                        With (New clsFacadeMunicipios).GetById(.Item(clsCommonCiudades.FLD_IDMUNICIPIO), err)
                            fx += "'" + .Tables(clsCommonMunicipios.TABLA_MUNICIPIOS).Rows(0).Item(clsCommonMunicipios.FLD_IDESTADO).ToString() + "', "
                        End With
                        fx += "'" + .Item(clsCommonCiudades.FLD_IDMUNICIPIO).ToString() + "', "
                        fx += "'" + ciudad.ToString() + "');"

                        ClientScript.RegisterStartupScript(Me.GetType(), "Inicializa Ciudad", fx, True)
                    End With
                End If

                'Else
                '    txtAgency.Text = String.Empty
                '    txtAddress.Text = String.Empty
                '    txtTown.Text = String.Empty
                '    txtZIP.Text = String.Empty
                '    txtPhone.Text = String.Empty
                '    txtContact.Text = String.Empty
                '    txtJob.Text = String.Empty
                '    txtEmail.Text = String.Empty
                '    chkIsAgency.Checked = False
                '    Me.imgPrint.Visible = False
                'End If
                Me.txtOwnerOffice.Value = If(dsAgreement.Tables(0).Rows(0).IsNull("CorporativoIdOficina"), "", dsAgreement.Tables(0).Rows(0)("CorporativoIdOficina"))
                Me.txtOwnerOfficeContact.Value = If(dsAgreement.Tables(0).Rows(0).IsNull("CorporativoIdContacto"), "", dsAgreement.Tables(0).Rows(0)("CorporativoIdContacto"))
                txtCorporateContact.Text = If(dsAgreement.Tables(0).Rows(0).IsNull("CorporativoContacto"), "", dsAgreement.Tables(0).Rows(0)("CorporativoContacto"))
                txtCorporateJob.Text = If(dsAgreement.Tables(0).Rows(0).IsNull("CorporativoPuesto"), "", dsAgreement.Tables(0).Rows(0)("CorporativoPuesto"))

            End If

            Dim generalRestrictions()() As Object
            Dim especificRestrictions()() As Object
            Dim usedEspecificRestrictions()() As Object

            Dim drGeneralRestrictions() As DataRow = dsAgreement.Tables(2).Select("tipo=0")
            Dim drEspecificRestrictions() As DataRow = dsAgreement.Tables(2).Select("tipo=1")

            ReDim generalRestrictions(drGeneralRestrictions.Length - 1)
            Dim idx As Integer = 0
            For Each dr As DataRow In drGeneralRestrictions
                generalRestrictions(idx) = New Object() {dr("idConvenioRestriccion"), dr("Descripcion")}
                idx += 1
            Next

            ReDim especificRestrictions(drEspecificRestrictions.Length - 1)
            idx = 0
            For Each dr As DataRow In drEspecificRestrictions
                especificRestrictions(idx) = New Object() {dr("idConvenioRestriccion"), dr("Descripcion")}
                idx += 1
            Next

            ReDim usedEspecificRestrictions(dsAgreement.Tables(3).Rows.Count - 1)
            idx = 0
            For Each dr As DataRow In dsAgreement.Tables(3).Rows
                usedEspecificRestrictions(idx) = New Object() {dr("idHotel"), dr("idRatePlan"), dr("idTarifa"), dr("idTipoHabitacion_Hotel"), dr("idsRestriccions"), dr("idConvenioRestriccion"), IIf(dr("isException") = 0, False, True)}
                idx += 1
            Next

            Dim theConverter As New Converters.IsoDateTimeConverter()
            theConverter.DateTimeFormat = "MM/dd/yyyy"

            'hdnGeneralRestrictions.Value = JsonConvert.SerializeObject(generalRestrictions, theConverter)
            hdnEspecificRestrictions.Value = JsonConvert.SerializeObject(especificRestrictions, theConverter)
            hdnUsedEspecificRestrictions.Value = JsonConvert.SerializeObject(usedEspecificRestrictions, theConverter)

            'For Each item As DataGridItem In dgHotels.Items
            '    Dim drRatesPlan() As DataRow = dsAgreement.Tables(1).Select("idHotel=" & CType(item.FindControl("idHotel"), HiddenField).Value)
            '    If drRatesPlan.Length > 0 Then
            '        Dim RatesPlan(drRatesPlan.Length - 1) As RatePlanAccess.RatePlan
            '        idx = 0
            '        For Each dr As DataRow In drRatesPlan
            '            Dim ratePlan As RatePlanAccess.RatePlan = GetRatePlan(dr("idHotel"), dr("idRatePlan"), idAgreement)
            '            RatesPlan(idx) = ratePlan
            '            idx += 1
            '        Next

            '        If RatesPlan IsNot Nothing AndAlso RatesPlan(0) IsNot Nothing Then
            '            CType(item.FindControl("hotelRates"), HiddenField).Value = JsonConvert.SerializeObject(RatesPlan, theConverter)
            '            CType(item.FindControl("chkHotel"), CheckBox).Checked = True
            '            CType(item.FindControl("Vigencia"), HiddenField).Value = RatesPlan(0).Vigencia.ToString("MM/dd/yyyy")
            '        Else
            '            CType(item.FindControl("chkHotel"), CheckBox).Checked = False
            '            CType(item.FindControl("hotelRates"), HiddenField).Value = ""
            '            CType(item.FindControl("Vigencia"), HiddenField).Value = ""
            '        End If
            '    Else
            '        CType(item.FindControl("chkHotel"), CheckBox).Checked = False
            '        CType(item.FindControl("hotelRates"), HiddenField).Value = ""
            '        CType(item.FindControl("Vigencia"), HiddenField).Value = ""
            '    End If
            'Next


        End If
    End Sub

    Private Sub LoadHotels(ByVal idCorporate As Integer)

        If MyBase.IsUsuarioHotel Then

            Dim ds As PermisosData

            With New PermisosFacade
                ds = .PermisosGetByUser(Usuario)
            End With
            Dim sw As Boolean = False
            For Each dr As DataRow In ds.Tables(0).Rows
                If dr("PermisoName").ToString.ToLower.IndexOf("pages/convenios.aspx") > 0 Then
                    sw = True
                    Exit For
                End If
            Next
            If sw Then
                '    Dim dsHotels As DataSet = (New RatePlanAccess).GetHotelsByCorporate(idCorporate, Usuario)                
                '    dgHotels.DataSource = dsHotels
                '    dgHotels.DataBind()
                'TODO: aplicar este caso para la nueva manera de traer hoteles.
                txtTarifaConvenio.Enabled = True
                btnTarifaCovnenio.Enabled = True
            Else
                txtTarifaConvenio.Enabled = False
                btnTarifaCovnenio.Enabled = False
            End If

        Else
            txtTarifaConvenio.Enabled = True
            btnTarifaCovnenio.Enabled = True
            '    Dim dsHotels As DataSet = (New RatePlanAccess).GetHotelsByCorporate(idCorporate)
            '    dgHotels.DataSource = dsHotels
            '    dgHotels.DataBind()
        End If


    End Sub

    Protected Sub ddlAgreements_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAgreements.SelectedIndexChanged
        If ddlAgreements.SelectedValue > 0 Then
            LoadAgreement(ddlAgreements.SelectedValue, True)
        End If
    End Sub

    'Protected Sub ddlAgreementWorking_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAgreementWorking.SelectedIndexChanged

    '    If ddlAgreementWorking.SelectedValue > 0 Then
    '        Dim sdatosdespues As String
    '        LoadAgreement(ddlAgreementWorking.SelectedValue, sdatosdespues)
    '        hdnIdAgreement.Value = ddlAgreementWorking.SelectedValue
    '        txtNoAgreement.Enabled = False
    '    Else
    '        ResetForm()
    '    End If
    'End Sub

    Private Sub ResetForm()
        LoadAgreements(idCorporate, ctrlAutoComplete1.GetFilter)
        LoadHotels(idCorporate)
        ClientScript.RegisterStartupScript(Me.GetType(), "Inicializa Ciudad", "SetInitialLocation('MX', '', '', '');", True)
        'ddlCountries_SelectedIndexChanged(New Object, New EventArgs)

        Me.txtOwnerOffice.Value = "0"
        Me.txtOwnerOfficeContact.Value = "0"
        txtNoAgreement.Enabled = True
        txtAgency.Text = String.Empty
        txtAddress.Text = String.Empty
        txtTown.Text = String.Empty
        txtZIP.Text = String.Empty
        txtPhone.Text = String.Empty
        txtContact.Text = String.Empty
        txtJob.Text = String.Empty
        txtEmail.Text = String.Empty
        txtNoAgreement.Text = String.Empty
        txtCorporateContact.Text = String.Empty
        txtCorporateJob.Text = String.Empty
        chkIsAgency.Checked = False
        'hdnGeneralRestrictions.Value = String.Empty
        hdnEspecificRestrictions.Value = String.Empty
        hdnUsedEspecificRestrictions.Value = String.Empty
        hdnIdAgreement.Value = 0

        dgAgreementWorking.SelectedIndex = -1
        tabEditConvenio.Visible = False
        '        lblAgreementWorking.Visible = True
    End Sub


    Private _LabelDictionary As Dictionary(Of String, String)
    Protected ReadOnly Property Labels() As Dictionary(Of String, String)
        Get
            If Me._LabelDictionary Is Nothing Then
                Me._LabelDictionary = New Dictionary(Of String, String)
                Try
                    Dim doc As New XmlDocument()
                    doc.Load(Server.MapPath(Request.ApplicationPath & "/Data/ConveniosLabels.xml"))
                    For Each node As XmlElement In doc.GetElementsByTagName("Label")

                        If Not Me._LabelDictionary.ContainsKey(node.GetAttribute("key")) Then
                            If node.GetElementsByTagName(PortalCulture.GetCulture().ToString()).Count > 0 Then
                                Me._LabelDictionary.Add(node.GetAttribute("key"), node.GetElementsByTagName(PortalCulture.GetCulture().ToString())(0).InnerText.Replace("\n", Environment.NewLine))
                            End If
                        End If

                    Next

                Catch ex As Exception
                End Try
            End If
            Return Me._LabelDictionary
        End Get
    End Property

    Protected Function GetLabel(ByVal key As String) As String
        Dim result As String = String.Empty
        If Me.Labels.ContainsKey(key) Then result = Me.Labels(key)
        Return result
    End Function


    'Protected Sub imgPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgPrint.Click
    '    If ValidaCampos() AndAlso Not Me.chkIsGeneral.Checked Then
    '        ImgSaveNoPrint_Click(sender, e)
    '        Dim usedEspecificRestrictions()() As Object = JsonConvert.DeserializeObject(Of Object()())(hdnUsedEspecificRestrictions.Value)
    '        Dim RatesPlan()() As RatePlanAccess.RatePlan

    '        Dim agreementData As New Agreement()
    '        agreementData.FromData.AddFromDataRow(idSelectedCorpororateName.Value, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, Me.txtCorporateContact.Text, Me.txtCorporateJob.Text, String.Empty)

    '        Dim agreementRow As Agreement.AgreementRow = agreementData._Agreement.AddAgreementRow(txtNoAgreement.Text.Trim(), Today, Today, String.Empty)

    '        'Dim dsAgreement As New dsAgreements
    '        'Dim drCorporate As dsAgreements.CorporateRow = dsAgreement.Corporate.NewCorporateRow
    '        ''drCorporate.Logo = ConversionImagen(HttpContext.Current.ApplicationInstance.Server.MapPath("../Images/logoCertificado.JPG"))
    '        'drCorporate.Logo = GetImage(Request.Url.OriginalString.Replace(Request.Url.PathAndQuery, "") & ResolveUrl("../Images/CorporateLogo/HotelesMision.jpg"))
    '        'drCorporate.ImageDemo = GetImage(Request.Url.OriginalString.Replace(Request.Url.PathAndQuery, "") & ResolveUrl("../Images/CorporateLogo/HotelMision_Demo_" + PortalCulture.GetCulture().ToString().ToLower().Substring(0, 2) + ".jpg"))
    '        'drCorporate.Name = "HOTELES MISION"
    '        'drCorporate.Contact = txtCorporateContact.Text
    '        'drCorporate.WorkPosition = txtCorporateJob.Text
    '        'dsAgreement.Corporate.AddCorporateRow(drCorporate)

    '        Dim minVigency As New Date(2050, 12, 31)
    '        Dim maxVigency As New Date(2000, 12, 31)

    '        Dim idxRatesPlan As Integer = 0

    '        For Each item As DataGridItem In dgHotels.Items
    '            'Creamos un row para el hotel
    '            Dim DataHotel() As String = JsonConvert.DeserializeObject(Of String())(CType(item.FindControl("DataHotel"), HiddenField).Value)
    '            Dim tax As Double = 0
    '            Double.TryParse(Regex.Replace(DataHotel(0), "[^\d.-]", ""), tax)

    '            'HOTEL NAME => CType(item.FindControl("chkHotel"), CheckBox).Text
    '            'HOTEL TAX => DataHotel(0)
    '            'HOTEL CURRENCY => DataHotel(1)
    '            Dim hotelRow As Agreement.HotelRow = agreementData.Hotel.AddHotelRow(agreementRow, CType(item.FindControl("chkHotel"), CheckBox).Text, tax, Today)


    '            'Dim hotelChecked As Boolean = CType(item.FindControl("chkHotel"), CheckBox).Checked
    '            Dim hotelRatesPlan As String = CType(item.FindControl("hotelRates"), HiddenField).Value
    '            If hotelRatesPlan IsNot String.Empty Then
    '                ReDim Preserve RatesPlan(idxRatesPlan)
    '                RatesPlan(idxRatesPlan) = JsonConvert.DeserializeObject(Of RatePlanAccess.RatePlan())(hotelRatesPlan)
    '                For Each ratePlan As RatePlanAccess.RatePlan In RatesPlan(idxRatesPlan)

    '                    'AQUI SE CREA EL RATEPLAN
    '                    'RATEPLAN CODE => ratePlan.RateCode
    '                    'RATEPLAN NAME => ratePlan.Name
    '                    'RATEPLAN CURRENCY => If(ratePlan.Currency.Trim().Length > 0, ratePlan.Currency, DataHotel(1))
    '                    Dim ratePlanRow As Agreement.RatePlanRow = agreementData.RatePlan.AddRatePlanRow(hotelRow, ratePlan.RateCode, ratePlan.Name, If(ratePlan.Currency.Trim().Length > 0, ratePlan.Currency, DataHotel(1)))
    '                    If hotelRow.Vigency = Today Then hotelRow.Vigency = ratePlan.Vigencia

    '                    '/*
    '                    'Dim hotelName As String = CType(item.FindControl("chkHotel"), CheckBox).Text
    '                    'Dim DataHotel() As String = JsonConvert.DeserializeObject(Of String())(CType(item.FindControl("DataHotel"), HiddenField).Value)
    '                    '*/

    '                    For Each rate As RatePlanAccess.RatePlan.Rate In ratePlan.Rates
    '                        For Each restriction As RatePlanAccess.RatePlan.Rate.Restriction In rate.Restrictions
    '                            'CREAMOS EL REGISTRO DE RATE
    '                            'DAYS TO APPLY => restriction.Exceptions
    '                            'START DATE => rate.DateStart
    '                            'END DATE => rate.DateEnd
    '                            'QUOTES => GetEspecificRestriction(usedEspecificRestrictions, ratePlan.IdHotel, ratePlan.IdRatePlan, rate.IdRate, restriction.IdRoomType, restriction.IdsRestrictions, restriction.isException)
    '                            'ROOM => restriction.NameRoom
    '                            Dim days As Integer = 0
    '                            For Each day As String In restriction.Exceptions.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
    '                                Select Case day.ToLower()
    '                                    Case "lu", "mo"
    '                                        days = (days Or 1)
    '                                    Case "ma", "tu"
    '                                        days = (days Or 2)
    '                                    Case "mi", "we"
    '                                        days = (days Or 4)
    '                                    Case "ju", "th"
    '                                        days = (days Or 8)
    '                                    Case "vi", "fr"
    '                                        days = (days Or 16)
    '                                    Case "sa"
    '                                        days = (days Or 32)
    '                                    Case "do", "su"
    '                                        days = (days Or 64)
    '                                End Select
    '                            Next
    '                            Dim rateRow As Agreement.RateRow = agreementData.Rate.AddRateRow(ratePlanRow, days, Convert.ToDateTime(rate.DateStart, New CultureInfo(PortalCulture.GetCulture.ToString)), Convert.ToDateTime(hotelRow.Vigency, New CultureInfo(PortalCulture.GetCulture.ToString)), GetEspecificRestriction(usedEspecificRestrictions, ratePlan.IdHotel, ratePlan.IdRatePlan, rate.IdRate, restriction.IdRoomType, restriction.IdsRestrictions, restriction.isException), restriction.NameRoom)

    '                            'CREAMOS EL REGISTRO DE TARIFA DE ADULTO
    '                            'TARGET => 'AD'
    '                            'RANGE => If(restriction.VariousAdults, if(restriction.MinAdults = restriction.MaxAdults, restriction.MaxAdults, restriction.MinAdults + "-" + restriction.MaxAdults), restriction.Adults)
    '                            'AMOUNT => restriction.AdultRate
    '                            agreementData.Amount.AddAmountRow(rateRow, "AD", If(restriction.VariousAdults, If(restriction.MinAdults = restriction.MaxAdults, restriction.MaxAdults, restriction.MinAdults + "-" + restriction.MaxAdults), restriction.Adults), restriction.AdultRate)

    '                            'PortalCulture.GetString("M000165") & "(" & IIf(restriction.VariousAdults, IIf(restriction.MinAdults = restriction.MaxAdults, restriction.MaxAdults, restriction.MinAdults & "-" & restriction.MaxAdults), restriction.Adults) & "): " & restriction.AdultRate & ChildrenRate & IIf(restriction.AdultRateExt > 0, vbNewLine & PortalCulture.GetString("M000226", True) & " " & restriction.AdultRateExt, "") & IIf(restriction.ChildRateExt > 0, vbNewLine & PortalCulture.GetString("M000227", True) & " " & restriction.ChildRateExt, "") & IIf(restriction.TeenRateExt > 0, vbNewLine & PortalCulture.GetString("01311", True) & " " & restriction.TeenRateExt, "")

    '                            '/*
    '                            'Dim ChildrenRate As String = String.Empty
    '                            'Dim TeenRate As String = String.Empty
    '                            '*/
    '                            Dim rateRange As String = String.Empty

    '                            If restriction.ChildrenRates IsNot Nothing Then
    '                                For i As Integer = 0 To restriction.ChildrenRates.Length - 1
    '                                    If i + 1 < restriction.ChildrenRates.Length AndAlso restriction.ChildrenRates(i).Rate = restriction.ChildrenRates(i + 1).Rate Then
    '                                        rateRange = IIf(rateRange = String.Empty, restriction.ChildrenRates(i).Count, rateRange)
    '                                    Else
    '                                        rateRange += IIf(rateRange = String.Empty, String.Empty, "-") & restriction.ChildrenRates(i).Count
    '                                        'CREAMOS EL REGISTRO DE TARIFA DE NIÑO
    '                                        'TARGET => 'CH'
    '                                        'RANGE => rateRange
    '                                        'AMOUNT => If(restriction.isException, restriction.ChildrenRates(i).RateExc, restriction.ChildrenRates(i).Rate)
    '                                        '/*
    '                                        'ChildrenRate += IIf((restriction.isException AndAlso restriction.ChildrenRates(i).RateExc > 0) OrElse (Not restriction.isException AndAlso restriction.ChildrenRates(i).Rate > 0), vbNewLine & PortalCulture.GetString("M000166") & "(" & rateRange & "): " & IIf(restriction.isException, restriction.ChildrenRates(i).RateExc, restriction.ChildrenRates(i).Rate), String.Empty)
    '                                        '*/
    '                                        agreementData.Amount.AddAmountRow(rateRow, "CH", rateRange, If(restriction.isException, restriction.ChildrenRates(i).RateExc, restriction.ChildrenRates(i).Rate))
    '                                        rateRange = String.Empty
    '                                    End If
    '                                Next
    '                            End If

    '                            If restriction.ChildrenRates IsNot Nothing Then
    '                                For i As Integer = 0 To restriction.ChildrenRates.Length - 1
    '                                    If i + 1 < restriction.ChildrenRates.Length AndAlso restriction.ChildrenRates(i).TeenRate = restriction.ChildrenRates(i + 1).TeenRate Then
    '                                        rateRange = IIf(rateRange = String.Empty, restriction.ChildrenRates(i).Count, rateRange)
    '                                    Else
    '                                        rateRange &= IIf(rateRange = String.Empty, String.Empty, "-") & restriction.ChildrenRates(i).Count 'CREAMOS EL REGISTRO DE TARIFA DE NIÑO
    '                                        'CREAMOS EL REGISTRO DE TARIFA DE ADOLECENTE
    '                                        'TARGET => 'TE'
    '                                        'RANGE => rateRange
    '                                        'AMOUNT => If(restriction.isException, restriction.ChildrenRates(i).TeenRateExc, restriction.ChildrenRates(i).TeenRate)
    '                                        '/*
    '                                        'TeenRate &= IIf((restriction.isException AndAlso restriction.ChildrenRates(i).TeenRateExc > 0) OrElse (Not restriction.isException AndAlso restriction.ChildrenRates(i).TeenRate > 0), vbNewLine & PortalCulture.GetString("01282") & "(" & rateRange & "): " & IIf(restriction.isException, restriction.ChildrenRates(i).TeenRateExc, restriction.ChildrenRates(i).TeenRate), String.Empty)
    '                                        '*/
    '                                        agreementData.Amount.AddAmountRow(rateRow, "TE", rateRange, If(restriction.isException, restriction.ChildrenRates(i).TeenRateExc, restriction.ChildrenRates(i).TeenRate))
    '                                        rateRange = String.Empty
    '                                    End If
    '                                Next
    '                            End If

    '                            'CREAMOS EL REGISTRO DE TARIFA DE ADULTO EXTRA
    '                            'TARGET => 'XAD'
    '                            'RANGE => ''
    '                            'AMOUNT => restriction.AdultRateExt
    '                            agreementData.Amount.AddAmountRow(rateRow, "XAD", "", restriction.AdultRateExt)

    '                            'CREAMOS EL REGISTRO DE TARIFA DE NIÑO EXTRA
    '                            'TARGET => 'XCH'
    '                            'RANGE => ''
    '                            'AMOUNT => restriction.ChildRateExt
    '                            agreementData.Amount.AddAmountRow(rateRow, "XCH", "", restriction.ChildRateExt)

    '                            'CREAMOS EL REGISTRO DE TARIFA DE ADOLECENTE EXTRA
    '                            'TARGET => 'XTE'
    '                            'RANGE => ''
    '                            'AMOUNT => restriction.TeenRateExt
    '                            agreementData.Amount.AddAmountRow(rateRow, "XTE", "", restriction.TeenRateExt)

    '                            '/*
    '                            'Dim tax As Double = 0
    '                            'Double.TryParse(Regex.Replace(DataHotel(0), "[^\d.-]", ""), tax)

    '                            'Dim current As System.Globalization.CultureInfo
    '                            'current = System.Threading.Thread.CurrentThread.CurrentCulture
    '                            'System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)

    '                            'agreementData.AgreementDetail.AddAgreementDetailRow( _
    '                            '    HotelName:=hotelName, _
    '                            '    ratePlan:=ratePlan.RateCode & ", " & vbNewLine & ratePlan.Name, _
    '                            '    DaysToApply:=restriction.Exceptions & GetEspecificRestriction(usedEspecificRestrictions, ratePlan.IdHotel, ratePlan.IdRatePlan, rate.IdRate, restriction.IdRoomType, restriction.IdsRestrictions, restriction.isException) & vbNewLine & restriction.NameRoom, _
    '                            '    rate:=PortalCulture.GetString("M000165") & "(" & IIf(restriction.VariousAdults, IIf(restriction.MinAdults = restriction.MaxAdults, restriction.MaxAdults, restriction.MinAdults & "-" & restriction.MaxAdults), restriction.Adults) & "): " & restriction.AdultRate & ChildrenRate & IIf(restriction.AdultRateExt > 0, vbNewLine & PortalCulture.GetString("M000226", True) & " " & restriction.AdultRateExt, "") & IIf(restriction.ChildRateExt > 0, vbNewLine & PortalCulture.GetString("M000227", True) & " " & restriction.ChildRateExt, "") & IIf(restriction.TeenRateExt > 0, vbNewLine & PortalCulture.GetString("01311", True) & " " & restriction.TeenRateExt, ""), _
    '                            '    Validaty:=rate.DateStart & Environment.NewLine & "a " & rate.DateEnd & vbNewLine & PortalCulture.GetString("M000263", True) & " " & If(ratePlan.Currency.Trim().Length > 0, ratePlan.Currency, DataHotel(1)), _
    '                            '    tax:=tax, _
    '                            '    Vigency:=ratePlan.Vigencia _
    '                            ')
    '                            '*/
    '                            If ratePlan.Vigencia < minVigency Then minVigency = ratePlan.Vigencia
    '                            If ratePlan.Vigencia > maxVigency Then maxVigency = ratePlan.Vigencia
    '                            '/*
    '                            'System.Threading.Thread.CurrentThread.CurrentCulture = current
    '                            '*/
    '                        Next
    '                    Next
    '                Next
    '                idxRatesPlan += 1
    '            End If
    '        Next

    '        If RatesPlan IsNot Nothing AndAlso RatesPlan.Length > 0 AndAlso RatesPlan(0).Length > 0 Then
    '            FormAgreement.Visible = False
    '            ReportAgreement.Visible = True

    '            Dim generalRestrictions()() As Object = JsonConvert.DeserializeObject(Of Object()())(hdnGeneralRestrictions.Value)
    '            Dim especificRestrictions()() As Object = JsonConvert.DeserializeObject(Of Object()())(hdnEspecificRestrictions.Value)
    '            Dim restrictions As String = String.Empty

    '            If generalRestrictions IsNot Nothing Then
    '                For Each generalRestriction() As Object In generalRestrictions
    '                    restrictions &= generalRestriction(1) & vbNewLine
    '                Next
    '            End If

    '            If especificRestrictions IsNot Nothing Then
    '                If restrictions IsNot String.Empty Then restrictions &= vbNewLine
    '                For Each especificRestriction() As Object In especificRestrictions
    '                    restrictions &= especificRestriction(1) & vbNewLine
    '                Next
    '            End If

    '            'agreementData._Agreement.AddAgreementRow(txtNoAgreement.Text.Trim.Trim, minVigency, maxVigency, restrictions)

    '            agreementRow.BeginEdit()
    '            agreementRow.EndDate = maxVigency
    '            agreementRow.BeginDate = minVigency
    '            agreementRow.Quotes = restrictions
    '            agreementRow.EndEdit()

    '            agreementData.ToData.AddToDataRow( _
    '                Company:=Me.txtAgency.Text.Trim(), _
    '                Address:=Me.txtAddress.Text.Trim(), _
    '                Suburb:=Me.txtTown.Text.Trim(), _
    '                City:=String.Empty, _
    '                State:=String.Empty, _
    '                Country:=String.Empty, _
    '                ZipCode:=Me.txtZIP.Text.Trim(), _
    '                Phone:=Me.txtPhone.Text.Trim(), _
    '                Contact:=Me.txtContact.Text.Trim(), _
    '                WorkPosition:=Me.txtJob.Text.Trim(), _
    '                Email:=Me.txtEmail.Text.Trim())

    '            Dim CSession As New CrystalSession
    '            Dim ds As New Crystal_1_0
    '            Dim CInfo As New CrystalInfoISerializable

    '            CSession.CrystalReportSetReport("agreement", "", ds)
    '            CSession.CrystalReportSetHeader("title", "subtitle", "subtitle2", "description", ds)

    '            'CSession.SetQueryParameters("Contact", txtContact.Text.Trim.Trim, ds)
    '            'CSession.SetQueryParameters("WorkPosition", txtJob.Text.Trim.Trim, ds)
    '            'CSession.SetQueryParameters("Agency", txtAgency.Text.Trim.Trim, ds)
    '            'CSession.SetQueryParameters("Address", txtAddress.Text.Trim.Trim, ds)
    '            'CSession.SetQueryParameters("ZIP", "CP: " + txtZIP.Text.Trim.Trim, ds)
    '            'CSession.SetQueryParameters("Phone", "Teléfono: " + txtPhone.Text.Trim.Trim, ds)
    '            'CSession.SetQueryParameters("Email", "Correo: " + txtEmail.Text.Trim.Trim, ds)
    '            'CSession.SetQueryParameters("NoAgreement", "Convenio No. " + txtNoAgreement.Text.Trim.Trim, ds)

    '            'CSession.SetQueryParameters("rsH_Reference", PortalCulture.GetString("00695", True) + " " + PortalCulture.GetString("01207"), ds)
    '            'CSession.SetQueryParameters("rsH_CorporateAgency", "HOTELES MISION/" + txtAgency.Text.Trim.Trim.ToUpper, ds)
    '            'CSession.SetQueryParameters("rsH_Presentation", String.Format(Me.GetLabel("Presentation"), txtContact.Text.Trim), ds)
    '            'CSession.SetQueryParameters("rsH_AgreementIntroduction", String.Format(Me.GetLabel("AgreementIntroduction"), "HOTELES MISION"), ds)

    '            'Dim year As String = minVigency.Year.ToString()
    '            'If maxVigency.Year <> minVigency.Year Then year += "-" + maxVigency.Year.ToString()

    '            'CSession.SetQueryParameters("rsH_TitleAgreements", String.Format(Me.GetLabel("TitleAgreements"), year), ds)

    '            'CSession.SetQueryParameters("rsH_RatePlanColumn", PortalCulture.GetString("00016"), ds)
    '            'CSession.SetQueryParameters("rsH_DaysToApplyColumn", PortalCulture.GetString("01201"), ds)
    '            'CSession.SetQueryParameters("rsH_RateColumn", PortalCulture.GetString("M000232"), ds)
    '            'CSession.SetQueryParameters("rsH_ValidatyColumn", PortalCulture.GetString("01224"), ds)
    '            'CSession.SetQueryParameters("rsH_TaxColumn", PortalCulture.GetString("M000080"), ds)

    '            'Dim current As System.Globalization.CultureInfo
    '            'current = System.Threading.Thread.CurrentThread.CurrentCulture
    '            'System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
    '            'CSession.SetQueryParameters("rsH_PrintDate", Now.ToString("dd MMMM, yyyy"), ds)
    '            'System.Threading.Thread.CurrentThread.CurrentCulture = current

    '            'CSession.SetQueryParameters("rsH_VigencyField", PortalCulture.GetString("01202"), ds)

    '            'CSession.SetQueryParameters("rsF_TitleRestrictions", IIf(restrictions IsNot String.Empty, Me.GetLabel("TitleRestrictions"), ""), ds)
    '            'CSession.SetQueryParameters("rsF_Restrictions", restrictions, ds)
    '            'CSession.SetQueryParameters("rsF_TitleReservations", Me.GetLabel("TitleReservations"), ds)
    '            'CSession.SetQueryParameters("rsF_Reservations", Me.GetLabel("Reservations"), ds)
    '            'CSession.SetQueryParameters("rsF_Indications", String.Format(Me.GetLabel("Indications"), txtNoAgreement.Text.Trim.Trim), ds)
    '            'CSession.SetQueryParameters("rsF_TitleGaranty", Me.GetLabel("TitleGaranty"), ds)
    '            'CSession.SetQueryParameters("rsF_Garanty", Me.GetLabel("Garanty"), ds)
    '            'CSession.SetQueryParameters("rsF_TitleSchedule", Me.GetLabel("TitleSchedule"), ds)
    '            'CSession.SetQueryParameters("rsF_Schedule", Me.GetLabel("Schedule"), ds)
    '            'CSession.SetQueryParameters("rsF_ATTE", Me.GetLabel("ATTE"), ds)
    '            'CSession.SetQueryParameters("rsF_Accept", Me.GetLabel("Accept"), ds)
    '            'CSession.SetQueryParameters("rsF_ContentFooter", "Praga No. 60, Col. Juárez, C.P. 06600 México D.F.    Tel.: 5209 17 00 con 20 Líneas, Fax: 5511 4714" + vbNewLine + "Lada: 01 (800) 900- 3800   http://www.hotelesmision.com", ds)

    '            CInfo.dsCrystal = ds
    '            CSession.CCrystalInfo = CInfo
    '            'ds = CType(CSession.CCrystalInfo.dsCrystal, Crystal_1_0)

    '            Session("AgreementData") = agreementData
    '            ReportProcessDS(True)
    '        Else
    '            Page.ClientScript.RegisterStartupScript(Me.GetType, "Msg", "alert('" + PortalCulture.GetString("01193") + "');", True)
    '        End If
    '    End If

    'End Sub

    Private Function GetEspecificRestriction(ByVal usedEspecificRestrictions()() As Object, ByVal IdHotel As Integer, ByVal IdRatePlan As String, ByVal IdRate As Integer, ByVal IdRoomType As Integer, ByVal IdsRestrictions As String, ByVal isException As Boolean) As String
        Dim result As String = String.Empty
        If usedEspecificRestrictions IsNot Nothing Then
            For Each restriction() As Object In usedEspecificRestrictions
                If restriction(0) = IdHotel AndAlso restriction(1) = IdRatePlan AndAlso restriction(2) = IdRate AndAlso restriction(3) = IdRoomType AndAlso restriction(4) = IdsRestrictions AndAlso restriction(6) = isException Then
                    result = String.Format("{0} {1}", result, New String("*", restriction(5) + 1))
                End If
            Next
        End If
        Return result
    End Function

    Private Sub ReportProcessDS(Optional ByVal init As Boolean = False)

        If init Then Me.Session.Remove("Agreement_Report")

        Dim report As ReportDocument = Me.Session("Agreement_Report")

        If report Is Nothing Then
            'Dim CrRptDocument As New ReportDocument
            Dim CSession As New CrystalSession
            Dim CReport As New CReportset()

            If AppSettings("RutaReport") IsNot Nothing Then
                Dim path As String = AppSettings("RutaReport")
                If path.EndsWith("\") Then path = path.Substring(0, path.Length - 1)
                path += "\Agreements"

                If IO.Directory.Exists(path) Then
                    If IO.Directory.Exists(path + "\" + Me.idCorporate.ToString()) Then
                        path += "\" + Me.idSelectedCorpororate.Value.ToString()
                    Else
                        path += "\_default"
                    End If
                    path += "\agreement_" + If(PortalCulture.GetIDCulture() = 2, "EN", "ES") + ".rpt"

                    If IO.File.Exists(path) Then
                        Dim data As Agreement = Me.Session("AgreementData")
                        If data IsNot Nothing Then

                            Dim current As System.Globalization.CultureInfo
                            current = System.Threading.Thread.CurrentThread.CurrentCulture
                            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)

                            Me.Session("Agreement_Report") = CReport.Report(CType(CSession.CCrystalInfo.dsCrystal, Crystal_1_0), data, path)
                            report = Me.Session("Agreement_Report")

                            System.Threading.Thread.CurrentThread.CurrentCulture = current

                        End If
                    End If
                End If

            End If

        End If

        If report IsNot Nothing Then Me.CrystalReportViewer1.ReportSource = report

    End Sub

    Private Function GetImage(ByVal urlImage As String) As Byte()
        Dim rawData() As Byte = Nothing
        Dim response As Net.WebResponse = Nothing
        Dim remoteStream As IO.Stream = Nothing
        Dim readStream As IO.StreamReader = Nothing

        Try
            Dim Request As Net.WebRequest = Net.WebRequest.Create(urlImage)
            If Request IsNot Nothing Then
                response = Request.GetResponse()
                If response IsNot Nothing Then
                    remoteStream = response.GetResponseStream()

                    Dim content_type As String = response.Headers("Content-type")
                    Dim imageType As System.Drawing.Imaging.ImageFormat
                    Select Case content_type
                        Case "image/jpeg"
                            imageType = System.Drawing.Imaging.ImageFormat.Jpeg
                        Case "image/jpg"
                            imageType = System.Drawing.Imaging.ImageFormat.Jpeg
                        Case "image/png"
                            imageType = System.Drawing.Imaging.ImageFormat.Png
                        Case "image/gif"
                            imageType = System.Drawing.Imaging.ImageFormat.Gif
                        Case Else
                            Return Nothing
                    End Select

                    readStream = New IO.StreamReader(remoteStream)
                    Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(remoteStream)

                    If img Is Nothing Then
                        Return Nothing
                    End If

                    Using ms As New IO.MemoryStream()
                        img.Save(ms, imageType)
                        rawData = ms.GetBuffer()
                    End Using

                End If
            End If

        Catch ex As Exception
        Finally
            If (response IsNot Nothing) Then response.Close()
            If (remoteStream IsNot Nothing) Then remoteStream.Close()
            If (readStream IsNot Nothing) Then readStream.Close()
        End Try

        Return rawData
    End Function

    Private Function ConversionImagen(ByVal nombrearchivo As String) As Byte()
        Dim fs As New IO.FileStream(nombrearchivo, IO.FileMode.Open)
        Dim br As New IO.BinaryReader(fs)
        Dim imagen(fs.Length) As Byte
        br.Read(imagen, 0, fs.Length)
        br.Close()
        fs.Close()
        Return imagen
    End Function

    Protected Sub imgCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCancel.Click
        ResetForm()
    End Sub

    Protected Sub imgDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgDelete.Click
        With (New RatePlanAccess)
            .DeleteAgreementbyId(idConvenio)
        End With
        ResetForm()
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        FormAgreement.Visible = True
        ReportAgreement.Visible = False

        'If ddlAgreementWorking.SelectedValue > 0 Then
        '    LoadAgreement(ddlAgreementWorking.SelectedValue)
        '    hdnIdAgreement.Value = ddlAgreementWorking.SelectedValue
        '    txtNoAgreement.Enabled = False
        'Else
        '    ResetForm()
        'End If
    End Sub

    Function GetAgreement(ByVal idAgreement As Integer) As String
        Dim sdato As String = ""
        Dim dsAgreement As DataSet = (New RatePlanAccess).GetAgreement(idAgreement, PortalCulture.GetIDCulture)
        sdato = Util.Utility.GetXml("table", "UpdateAgreement", dsAgreement, "Convenios")
        Return sdato
    End Function

    Private Sub ImgSaveNoPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgSaveNoPrint.Click
        If ValidaCampos() Then
            'Dim generalRestrictions()() As Object = JsonConvert.DeserializeObject(Of Object()())(hdnGeneralRestrictions.Value)
            Dim especificRestrictions()() As Object = JsonConvert.DeserializeObject(Of Object()())(hdnEspecificRestrictions.Value)
            Dim usedEspecificRestrictions()() As Object = JsonConvert.DeserializeObject(Of Object()())(hdnUsedEspecificRestrictions.Value)
            Dim RatesPlan()() As RatePlanAccess.RatePlan
            Dim sDato As String = ""
            Dim sDatoDespues As String = ""

            'Dim idxRatesPlan As Integer = 0
            'For Each item As DataGridItem In dgHotels.Items
            '    'Dim hotelChecked As Boolean = CType(item.FindControl("chkHotel"), CheckBox).Checked
            '    Dim hotelRatesPlan As String = CType(item.FindControl("hotelRates"), HiddenField).Value
            '    Dim vigencia As String = CType(item.FindControl("Vigencia"), HiddenField).Value

            '    If hotelRatesPlan IsNot String.Empty Then
            '        ReDim Preserve RatesPlan(idxRatesPlan)
            '        RatesPlan(idxRatesPlan) = JsonConvert.DeserializeObject(Of RatePlanAccess.RatePlan())(hotelRatesPlan)
            '        For i As Integer = 0 To RatesPlan(idxRatesPlan).Length - 1
            '            RatesPlan(idxRatesPlan)(i).Vigencia = CType(vigencia, DateTime).ToString("MM/dd/yyyy")
            '        Next
            '        idxRatesPlan += 1
            '    End If
            'Next
            'Dim rate As 
            RatesPlan = New RatePlanAccess.RatePlan()() {}

            Dim ds As DataSet = New RatePlanAccess().GetHotelByAllTarifaConvenio(idConvenio, idCorporate)
            Dim idhotel As Integer = -1
            For Each row As DataRow In ds.Tables(0).Rows
                Dim new_rateplan As New RatePlanAccess.RatePlan
                new_rateplan.IdRatePlan = row("idRatePlan").ToString()
                new_rateplan.IdHotel = row("idHotel").ToString()
                new_rateplan.Vigencia = CType("2079-01-01", DateTime).ToString("MM/dd/yyyy")

                'para para seguir la logica anterior...
                If (idhotel = row("idHotel").ToString()) Then
                    Dim last_RP As RatePlanAccess.RatePlan() = RatesPlan(RatesPlan.Length - 1)
                    ReDim Preserve last_RP(last_RP.Length)
                    last_RP(last_RP.Length - 1) = new_rateplan
                    RatesPlan(RatesPlan.Length - 1) = last_RP
                Else
                    idhotel = row("idHotel").ToString()
                    Dim last_RP(0) As RatePlanAccess.RatePlan
                    last_RP(0) = new_rateplan
                    ReDim Preserve RatesPlan(RatesPlan.Length)
                    RatesPlan(RatesPlan.Length - 1) = last_RP
                End If

            Next



            For i As Integer = 0 To dlHoteles.Items.Count - 1

                'Falta validar si está checkeado antes de crear el nuevo rateplan
                Dim chk As CheckBox = CType(dlHoteles.Items(i).FindControl("chk"), CheckBox)
                Dim hidState As HiddenField = CType(dlHoteles.Items(i).FindControl("IniState"), HiddenField)
                Dim hidHotel As HiddenField = CType(dlHoteles.Items(i).FindControl("Hotel"), HiddenField)
                'Si no está checkeado no tiene que hacer el RP
                Dim hidRate As HiddenField
                If chk.Checked Then
                    hidRate = CType(NewRatePlan(hidHotel.Value), HiddenField)
                Else
                    hidRate = New HiddenField
                End If
                'Dim hidRate As HiddenField = CType(dlHoteles.Items(i).FindControl("RateCode"), HiddenField)



                If hidState.Value = "1" AndAlso Not chk.Checked Then
                    If Not RatesPlan Is Nothing Then
                        For j As Integer = 0 To RatesPlan.Length - 1
                            For k As Integer = 0 To RatesPlan(j).Length - 1
                                If Not RatesPlan(j)(k) Is Nothing AndAlso (RatesPlan(j)(k).IdHotel = hidHotel.Value AndAlso RatesPlan(j)(k).IdRatePlan = hidRate.Value) Then
                                    RatesPlan(j)(k) = Nothing
                                End If
                            Next
                        Next
                    End If

                ElseIf hidState.Value = "0" AndAlso chk.Checked Then

                    Dim rp(0) As RatePlanAccess.RatePlan
                    rp(0) = New RatePlanAccess.RatePlan
                    rp(0).IdRatePlan = hidRate.Value
                    rp(0).IdHotel = hidHotel.Value
                    rp(0).Vigencia = CType("2079-01-01", DateTime).ToString("MM/dd/yyyy")
                    'If RatesPlan Is Nothing Then RatesPlan = New RatePlanAccess.RatePlan()() {}
                    ReDim Preserve RatesPlan(RatesPlan.Length)
                    RatesPlan(RatesPlan.Length - 1) = rp
                End If


            Next

            'dtTarifaConvenio = dt

            'Dim dtTarifaC As DataTable = dtTarifaConvenio
            'For Each row As DataRow In dtTarifaC.Rows
            '        Select row(FLD_STATUS)
            '        Case "New"
            '            Dim rp(0) As RatePlanAccess.RatePlan
            '            rp(0) = New RatePlanAccess.RatePlan
            '            rp(0).IdRatePlan = row(FLD_RATEPLAN)
            '            rp(0).IdHotel = row(FLD_IDHOTEL)
            '            rp(0).Vigencia = CType("2079-01-01", DateTime).ToString("MM/dd/yyyy")
            '            ReDim Preserve RatesPlan(RatesPlan.Length)
            '            RatesPlan(RatesPlan.Length - 1) = rp
            '        Case "Del"
            '            For j As Integer = 0 To RatesPlan.Length - 1
            '                For k As Integer = 0 To RatesPlan(j).Length - 1
            '                    If Not RatesPlan(j)(k) Is Nothing AndAlso (RatesPlan(j)(k).IdHotel = row(FLD_IDHOTEL) AndAlso RatesPlan(j)(k).IdRatePlan = row(FLD_RATEPLAN)) Then
            '                        RatesPlan(j)(k) = Nothing
            '                    End If
            '                Next
            '            Next
            '    End Select
            'Next


            If RatesPlan IsNot Nothing AndAlso RatesPlan.Length > 0 AndAlso RatesPlan(0).Length > 0 Then
                Dim isEditMode As Boolean = IIf(CInt(hdnIdAgreement.Value) > 0, True, False)
                Dim idAgreement As Integer
                Dim saveResponse As Integer = 0
                If isEditMode Then
                    idAgreement = hdnIdAgreement.Value
                End If

                Dim idOffice As Integer = 0
                Dim idContact As Integer = 0

                Integer.TryParse(Me.txtOwnerOffice.Value, idOffice)
                Integer.TryParse(Me.txtOwnerOfficeContact.Value, idContact)

                Dim flag As Boolean = False
                Dim adapter As New RatePlanAccess()

                Dim UsuarioHotelId As Integer = 0
                If MyBase.IsUsuarioHotel Then
                    UsuarioHotelId = Usuario
                End If
                sDato = GetAgreement(idConvenio)

                Dim sD As Date
                sD = Today
                sD = sD.AddHours(ddlhour.SelectedValue)
                sD = sD.AddMinutes(ddlmin.SelectedValue)

                Dim digito As String = CalulaDigitoVerificador(txtNoAgreement.Text.Trim)
                If Me.chkIsGeneral.Checked Then
                    flag = adapter.SaveAgreement(isEditMode, idAgreement, "", "", "", "", "", 0, "", "", "", txtNoAgreement.Text.Trim, txtCorporateContact.Text.Trim, txtCorporateJob.Text.Trim, False, True, RatesPlan, Nothing, especificRestrictions, usedEspecificRestrictions, saveResponse, idOffice, idContact, chkCC.Checked, sD, UsuarioHotelId, digitoVer:=digito)
                Else
                    flag = adapter.SaveAgreement(isEditMode, idAgreement, txtAgency.Text.Trim, txtAddress.Text.Trim, txtTown.Text.Trim, txtZIP.Text.Trim, txtPhone.Text.Trim, Me.selectedCity.Value, txtContact.Text.Trim, txtJob.Text.Trim, txtEmail.Text.Trim, txtNoAgreement.Text.Trim, txtCorporateContact.Text.Trim, txtCorporateJob.Text.Trim, cmbSegmento.SelectedValue, False, RatesPlan, Nothing, especificRestrictions, usedEspecificRestrictions, saveResponse, idOffice, idContact, chkCC.Checked, sD, UsuarioHotelId, digitoVer:=digito)
                End If

                Dim user As UserData
                Dim password As String = crypto.generatePassword()

                'Primero traemos el usuario si existe lo asociamos si no lo creamos y despues lo asociamos
                With New cUserSystem
                    user = .GetUserByEmail(txtEmail.Text)
                    ' Si no existe lo creamos
                    If user.Tables(user.USER_TABLE).Rows.Count = 0 Then
                        Dim inserted = .createUser(PortalCulture.GetCulture().ToString(), txtEmail.Text, password, UserData.EmailType.HTML, -1, user)
                    End If
                End With

                If flag Then
                    For Each row As DataRow In dsConvenioHomoClave.Tables(0).Rows
                        If row.RowState = DataRowState.Added Then
                            With New RatePlanFacade()
                                .InsertHomoClave(idAgreement, row(dsConvenioHomoClave.FIELD_COMPANY), row(dsConvenioHomoClave.FIELD_ADDRESS), row(dsConvenioHomoClave.FIELD_TOWN), row(dsConvenioHomoClave.FIELD_CITY), row(dsConvenioHomoClave.FIELD_COUNTRY), row(dsConvenioHomoClave.FIELD_ZIPCODE), row(dsConvenioHomoClave.FIELD_PHONE), row(dsConvenioHomoClave.FIELD_CONTACT), row(dsConvenioHomoClave.FIELD_WORKPOSITION), row(dsConvenioHomoClave.FIELD_EMAIL), row(dsConvenioHomoClave.FIELD_HOMOCLAVE))
                            End With
                        ElseIf row.RowState = DataRowState.Deleted Then
                            row.RejectChanges()
                            If Not row.IsNull(dsConvenioHomoClave.FIELD_IDCONVENIOHOMOCLAVE) Then
                                With New RatePlanFacade()
                                    .DeleteConvenioHomoClaveById(row(dsConvenioHomoClave.FIELD_IDCONVENIOHOMOCLAVE))
                                End With
                            End If

                        End If
                    Next
                    If CType(sender, ImageButton).ID = "imgSave" Then

                        LoadAgreement(idConvenio, sDatoDespues)
                        hdnIdAgreement.Value = idConvenio
                        txtNoAgreement.Enabled = False

                        ResetForm()
                        Page.ClientScript.RegisterStartupScript(Me.GetType, "Msg", "ShowMsgSave();", True)
                    Else
                        btnNewConvenio_Click(sender, e) ':P
                    End If



                    CType(Me.Page, PaginaBase).guardalog("/Pages/Convenios.aspx", PaginaBase.acciones.Modificar, "Se guardo convenio", "", sDato, sDatoDespues)
                    cmbSegmento.SelectedValue = 0
                Else
                    If saveResponse = 1 Then
                        '                        If CType(sender, ImageButton).ID = "ImgSaveNoPrint" Then
                        Page.ClientScript.RegisterStartupScript(Me.GetType, "Msg", "alert('" + PortalCulture.GetString("01204") + "');", True)
                    Else
                        'If CType(sender, ImageButton).ID = "ImgSaveNoPrint" Then 
                        Page.ClientScript.RegisterStartupScript(Me.GetType, "Msg", "alert('" + PortalCulture.GetString("01192") + "');", True)
                    End If
                End If
                'Me.LoadAgreement(idAgreement)
            Else
                If CType(sender, ImageButton).ID = "imgSave" Then Page.ClientScript.RegisterStartupScript(Me.GetType, "Msg", "alert('" + PortalCulture.GetString("01193") + "');", True)
            End If
        End If
    End Sub

    Protected Sub imgSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click

    End Sub

    Private Sub dgCorporates_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgCorporates.PageIndexChanged
        dgCorporates.CurrentPageIndex = e.NewPageIndex
        LoadCorporates()
    End Sub

    Protected Sub dgCorporates_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgCorporates.ItemCommand
        If e.CommandName = "Select" Then
            Dim values() As String = e.CommandArgument.ToString().Split("|".ToCharArray)
            If values.Length >= 2 Then
                idSelectedCorpororate.Value = values(0)
                idSelectedCorpororateName.Value = values(1)
                trCorporates.Visible = False
                FormAgreement.Visible = True
                ResetForm()
            End If

        End If
    End Sub

    Protected Sub btnSelectCorporate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectCorporate.Click
        'ResetForm()
        LoadCorporates()
    End Sub

    Private Function ValidaCampos() As Boolean
        Dim validate As Boolean = True

        If Not Me.chkIsGeneral.Checked Then
            If txtAgency.Text = String.Empty Then
                rfvAgency.Visible = True
                validate = False
            End If

            If txtContact.Text = String.Empty Then
                rfvContact.Visible = True
                validate = False
            End If

            If txtJob.Text = String.Empty Then
                rfvJob.Visible = True
                validate = False
            End If

            If txtNoAgreement.Text = String.Empty Then
                rfvNoAgreement.Visible = True
                validate = False
            End If

            If Not txtEmail.Text = String.Empty And Not (New Regex("^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")).IsMatch(txtEmail.Text) Then
                revEmail.Visible = True
                validate = False
            End If

            If Not (New Regex("\d+")).IsMatch(Me.selectedCity.Value) Then
                rfvCity.Visible = True
                validate = False
            End If
        End If
        Me.rfvCorporateContact.Validate()
        validate = validate AndAlso Me.rfvCorporateContact.IsValid

        Return validate
    End Function

    Private Function GetDays(ByVal days As String) As String()
        Dim result(1) As String
        Dim baseDays() As String

        If PortalCulture.GetIDCulture = 1 Then
            baseDays = New String() {"Lu", "Ma", "Mi", "Ju", "Vi", "Sa", "Do"}
        Else
            baseDays = New String() {"Mo", "Tu", "We", "Th", "Fr", "Sa", "Su"}
        End If

        For i As Integer = 0 To days.Length - 1
            If days.Substring(i, 1) = "N" Then
                result(0) += baseDays(i) + ","
            ElseIf days.Substring(i, 1) = "Y" Then
                result(1) += baseDays(i) + ","
            End If
        Next

        If result(0) IsNot Nothing AndAlso result(0).Length > 0 Then
            result(0) = result(0).Substring(0, result(0).Length - 1)
        End If

        If result(1) IsNot Nothing AndAlso result(1).Length > 0 Then
            result(1) = result(1).Substring(0, result(1).Length - 1)
        End If

        Return result
    End Function

    Private Sub LoadAgreements(ByVal idCorporate As Integer, ByVal filter As String)
        Dim ds As DataSet = (New RatePlanAccess).GetAgreementsByIdCorporate(idCorporate)
        Dim dv As DataView

        If Not Me.dsEmpty(ds) Then
            dv = ds.Tables(0).DefaultView
            dv.RowFilter = filter
        End If

        dgAgreementWorking.DataSource = dv
        dgAgreementWorking.DataKeyField = "IdConvenio"
        dgAgreementWorking.DataBind()


        ddlAgreements.DataSource = ds
        ddlAgreements.DataTextField = "ConvenioDescription"
        ddlAgreements.DataValueField = "IdConvenio"
        ddlAgreements.DataBind()
        ddlAgreements.Items.Insert(0, New ListItem(PortalCulture.GetString("00753", False), -1))
    End Sub

    Public Shared Sub GetlinkPrice(ByVal dr As DataRow, ByVal adultos As Byte, ByVal ninios As Byte, ByRef tarAd As Double, ByRef tarCh As Double, ByRef tarAdExc As Double, ByRef tarChExc As Double, ByRef TarExtAd As Double, ByRef tarExtCh As Double)
        '''''''''''''''''' links rate plans '''''''''''''''''''''''''''''''
        Dim ratio As Double
        Dim offset As Double
        If Not dr.IsNull("SourceRatePlan") Then
            Select Case adultos
                Case 1
                    If dr.IsNull("OnePersonRatio") OrElse dr("OnePersonRatio") = 0 Then
                        ratio = 1
                    Else
                        ratio = dr("OnePersonRatio")
                    End If
                    offset = 0
                    If Not dr.IsNull("OnePersonOffset") Then
                        offset = dr("OnePersonOffset")
                    End If
                    tarAd = tarAd * ratio + offset
                    tarAdExc = tarAdExc * ratio + offset
                Case 2
                    If dr.IsNull("TwoPersonRatio") OrElse dr("TwoPersonRatio") = 0 Then
                        ratio = 1
                    Else
                        ratio = dr("TwoPersonRatio")
                    End If
                    offset = 0
                    If Not dr.IsNull("TwoPersonOffset") Then
                        offset = dr("TwoPersonOffset")
                    End If
                    tarAd = tarAd * ratio + offset
                    tarAdExc = tarAdExc * ratio + offset
                Case Else
                    If dr.IsNull("OthersOccupationRatio") OrElse dr("OthersOccupationRatio") = 0 Then
                        ratio = 1
                    Else
                        ratio = dr("OthersOccupationRatio")
                    End If
                    offset = 0
                    If Not dr.IsNull("OthersOccupationOffset") Then
                        offset = dr("OthersOccupationOffset")
                    End If
                    tarAd = tarAd * ratio + offset
                    tarAdExc = tarAdExc * ratio + offset
            End Select

            If ninios > 0 AndAlso tarCh > 0 Then
                Select Case ninios
                    Case 1
                        If dr.IsNull("OnePersonRatio") OrElse dr("OnePersonRatio") = 0 Then
                            ratio = 1
                        Else
                            ratio = dr("OnePersonRatio")
                        End If
                        offset = 0
                        If Not dr.IsNull("OnePersonOffset") Then
                            offset = dr("OnePersonOffset")
                        End If
                        tarCh = tarCh * ratio + offset
                        tarChExc = tarChExc * ratio + offset
                    Case 2
                        If dr.IsNull("TwoPersonRatio") OrElse dr("TwoPersonRatio") = 0 Then
                            ratio = 1
                        Else
                            ratio = dr("TwoPersonRatio")
                        End If
                        offset = 0
                        If Not dr.IsNull("TwoPersonOffset") Then
                            offset = dr("TwoPersonOffset")
                        End If
                        tarCh = tarCh * ratio + offset
                        tarChExc = tarChExc * ratio + offset
                    Case Else
                        If dr.IsNull("OthersOccupationRatio") OrElse dr("OthersOccupationRatio") = 0 Then
                            ratio = 1
                        Else
                            ratio = dr("OthersOccupationRatio")
                        End If
                        offset = 0
                        If Not dr.IsNull("OthersOccupationOffset") Then
                            offset = dr("OthersOccupationOffset")
                        End If
                        tarCh = tarCh * ratio + offset
                        tarChExc = tarChExc * ratio + offset
                End Select
            End If
            If TarExtAd > 0 Then
                If dr.IsNull("ExtraAdultRatio") OrElse dr("ExtraAdultRatio") = 0 Then
                    ratio = 1
                Else
                    ratio = dr("ExtraAdultRatio")
                End If
                offset = 0
                If Not dr.IsNull("ExtraAdultOffset") Then
                    offset = dr("ExtraAdultOffset")
                End If
                TarExtAd = TarExtAd * ratio + offset
            End If
            If tarExtCh > 0 Then
                If dr.IsNull("ExtraChildRatio") OrElse dr("ExtraChildRatio") = 0 Then
                    ratio = 1
                Else
                    ratio = dr("ExtraChildRatio")
                End If
                offset = 0
                If Not dr.IsNull("ExtraChildOffset") Then
                    offset = dr("ExtraChildOffset")
                End If
                tarExtCh = tarExtCh * ratio + offset
            End If
        End If

        '--------------------------- link roomtypes ----------------
        If Not dr.IsNull("IdTipohabitacion_Source") Then
            Select Case adultos
                Case 1
                    If dr.IsNull("tOnePersonRatio") OrElse dr("tOnePersonRatio") = 0 Then
                        ratio = 1
                    Else
                        ratio = dr("tOnePersonRatio")
                    End If
                    offset = 0
                    If Not dr.IsNull("tOnePersonOffset") Then
                        offset = dr("tOnePersonOffset")
                    End If
                    tarAd = tarAd * ratio + offset
                    tarAdExc = tarAdExc * ratio + offset
                Case 2
                    If dr.IsNull("tTwoPersonRatio") OrElse dr("tTwoPersonRatio") = 0 Then
                        ratio = 1
                    Else
                        ratio = dr("tTwoPersonRatio")
                    End If
                    offset = 0
                    If Not dr.IsNull("tTwoPersonOffset") Then
                        offset = dr("tTwoPersonOffset")
                    End If
                    tarAd = tarAd * ratio + offset
                    tarAdExc = tarAdExc * ratio + offset
                Case Else
                    If dr.IsNull("OtherOccupationRatio") OrElse dr("OtherOccupationRatio") = 0 Then
                        ratio = 1
                    Else
                        ratio = dr("OtherOccupationRatio")
                    End If
                    offset = 0
                    If Not dr.IsNull("OtherOccupationOffset") Then
                        offset = dr("OtherOccupationOffset")
                    End If
                    tarAd = tarAd * ratio + offset
                    tarAdExc = tarAdExc * ratio + offset
            End Select

            If ninios > 0 AndAlso tarCh > 0 Then
                Select Case ninios
                    Case 1
                        If dr.IsNull("tOnePersonRatio") OrElse dr("tOnePersonRatio") = 0 Then
                            ratio = 1
                        Else
                            ratio = dr("tOnePersonRatio")
                        End If
                        offset = 0
                        If Not dr.IsNull("tOnePersonOffset") Then
                            offset = dr("tOnePersonOffset")
                        End If
                        tarCh = tarCh * ratio + offset
                        tarChExc = tarChExc * ratio + offset
                    Case 2
                        If dr.IsNull("tTwoPersonRatio") OrElse dr("tTwoPersonRatio") = 0 Then
                            ratio = 1
                        Else
                            ratio = dr("tTwoPersonRatio")
                        End If
                        offset = 0
                        If Not dr.IsNull("tTwoPersonOffset") Then
                            offset = dr("tTwoPersonOffset")
                        End If
                        tarCh = tarCh * ratio + offset
                        tarChExc = tarChExc * ratio + offset
                    Case Else
                        If dr.IsNull("OtherOccupationRatio") OrElse dr("OtherOccupationRatio") = 0 Then
                            ratio = 1
                        Else
                            ratio = dr("OtherOccupationRatio")
                        End If
                        offset = 0
                        If Not dr.IsNull("OtherOccupationOffset") Then
                            offset = dr("OtherOccupationOffset")
                        End If
                        tarCh = tarCh * ratio + offset
                        tarChExc = tarChExc * ratio + offset
                End Select
            End If
            If TarExtAd > 0 Then
                If dr.IsNull("tExtraAdultRatio") OrElse dr("tExtraAdultRatio") = 0 Then
                    ratio = 1
                Else
                    ratio = dr("tExtraAdultRatio")
                End If
                offset = 0
                If Not dr.IsNull("tExtraAdultOffset") Then
                    offset = dr("tExtraAdultOffset")
                End If
                TarExtAd = TarExtAd * ratio + offset
            End If
            If tarExtCh > 0 Then
                If dr.IsNull("tExtraChildRatio") OrElse dr("tExtraChildRatio") = 0 Then
                    ratio = 1
                Else
                    ratio = dr("tExtraChildRatio")
                End If
                offset = 0
                If Not dr.IsNull("tExtraChildOffset") Then
                    offset = dr("tExtraChildOffset")
                End If
                tarExtCh = tarExtCh * ratio + offset
            End If
            If tarAd < 0 Then
                tarAd = 0
            End If
            If tarCh < 0 Then
                tarCh = 0
            End If
            If tarAdExc < 0 Then
                tarAdExc = 0
            End If
            If tarChExc < 0 Then
                tarChExc = 0
            End If
            If TarExtAd < 0 Then
                TarExtAd = 0
            End If
            If TarExtAd < 0 Then
                TarExtAd = 0
            End If
            If tarExtCh < 0 Then
                tarExtCh = 0
            End If
        End If

    End Sub

    'Private Sub LoadCountries(Optional ByVal selected As String = "")
    '    If Me.ddlCountries.Items.Count = 0 Then
    '        ddlCountries.DataSource = (New clsFacadePaises).GetPaises(PortalCulture.GetIDCulture())
    '        ddlCountries.DataTextField = clsCommonPaises.FLD_NOMBRE
    '        ddlCountries.DataValueField = clsCommonPaises.FLD_IDPAIS
    '        ddlCountries.DataBind()
    '    End If
    '    If ddlCountries.Items.FindByValue(selected) IsNot Nothing Then ddlCountries.SelectedIndex = ddlCountries.Items.IndexOf(ddlCountries.Items.FindByValue(selected))
    '    If selected.Trim.Length = 0 Then Me.LoadStates("")
    'End Sub

    'Private Function GetDictionaryString(ByVal dictionary As Dictionary(Of String, String)) As String
    '    Dim result As String = String.Empty
    '    If dictionary.Count > 0 Then
    '        result += "["
    '        Dim sep As String = ""
    '        For Each item As KeyValuePair(Of String, String) In dictionary
    '            result += sep + "{"
    '            result += """id"":""" + item.Key + """,""name"":""" + item.Value + """"
    '            result += "}"
    '            sep = ","
    '        Next
    '        result += "]"
    '    End If
    '    Return result
    'End Function

    'Private Function GetStates(ByVal parent As String) As Dictionary(Of String, String)
    '    GetStates = New Dictionary(Of String, String)
    '    If parent.Trim().Length > 0 Then
    '        Dim strErr As String = String.Empty
    '        Dim data As clsCommonEstados = (New clsFacadeEstados).GetByPais(parent, strErr)

    '        If data.Tables.Contains(clsCommonEstados.TABLA_ESTADOS) Then
    '            For Each row As DataRow In data.Tables(clsCommonEstados.TABLA_ESTADOS).Rows
    '                GetStates.Add(row(clsCommonEstados.FLD_IDESTADO), row(clsCommonEstados.FLD_NOMBRE))
    '            Next
    '        End If
    '    End If
    'End Function

    'Private Sub LoadStates(Optional ByVal selected As String = "")
    '    Dim country As ListItem = Me.ddlCountries.Items(Me.ddlCountries.SelectedIndex)
    '    Me.ddlStates.Items.Clear()
    '    If country IsNot Nothing Then
    '        For Each item As KeyValuePair(Of String, String) In Me.GetStates(country.Value)
    '            Me.ddlStates.Items.Add(New ListItem(item.Value, item.Key))
    '            Me.ddlStates.Items.FindByValue(item.Key).Selected = (item.Key = selected)
    '        Next
    '    End If
    '    If selected.Trim.Length = 0 Then Me.LoadDistricts("")
    'End Sub

    'Private Function GetDistricts(ByVal parent As String) As Dictionary(Of String, String)
    '    GetDistricts = New Dictionary(Of String, String)
    '    If parent.Trim().Length > 0 Then
    '        Dim strErr As String = String.Empty
    '        Dim data As clsCommonMunicipios = (New clsFacadeMunicipios).GetByIdEstado(parent, strErr)

    '        If data.Tables.Contains(clsCommonMunicipios.TABLA_MUNICIPIOS) Then
    '            For Each row As DataRow In data.Tables(clsCommonMunicipios.TABLA_MUNICIPIOS).Rows
    '                GetDistricts.Add(row(clsCommonMunicipios.FLD_IDMUNICIPIO), row(clsCommonMunicipios.FLD_NOMBRE))
    '            Next
    '        End If
    '    End If
    'End Function

    'Private Sub LoadDistricts(Optional ByVal selected As String = "")
    '    Dim state As ListItem = Me.ddlStates.Items(Me.ddlStates.SelectedIndex)
    '    Me.ddlDistricts.Items.Clear()
    '    If state IsNot Nothing Then
    '        For Each item As KeyValuePair(Of String, String) In Me.GetDistricts(state.Value)
    '            Me.ddlDistricts.Items.Add(New ListItem(item.Value, item.Key))
    '            Me.ddlDistricts.Items.FindByValue(item.Key).Selected = (item.Key = selected)
    '        Next
    '    End If
    '    If selected.Trim.Length = 0 Then Me.LoadCities("")
    'End Sub

    'Private Function GetCities(ByVal parent As String) As Dictionary(Of String, String)
    '    GetCities = New Dictionary(Of String, String)
    '    If parent.Trim().Length > 0 Then
    '        Dim strErr As String = String.Empty
    '        Dim data As clsCommonCiudades = (New clsFacadeCiudades).GetByIdMunicipio(parent, strErr)

    '        If data.Tables.Contains(clsCommonCiudades.TABLA_CIUDADES) Then
    '            For Each row As DataRow In data.Tables(clsCommonCiudades.TABLA_CIUDADES).Rows
    '                GetCities.Add(row(clsCommonCiudades.FLD_IDCIUDAD), row(clsCommonCiudades.FLD_NOMBRE))
    '            Next
    '        End If

    '        GetCities.Add("0", PortalCulture.GetString("00821", False))

    '    End If
    'End Function

    'Private Sub LoadCities(Optional ByVal selected As String = "")
    '    Dim district As ListItem = Me.ddlDistricts.Items(Me.ddlDistricts.SelectedIndex)
    '    Me.ddlCities.Items.Clear()
    '    If district IsNot Nothing Then
    '        For Each item As KeyValuePair(Of String, String) In Me.GetCities(district.Value)
    '            Me.ddlCities.Items.Add(New ListItem(item.Value, item.Key))
    '            Me.ddlCities.Items.FindByValue(item.Key).Selected = (item.Key = selected)
    '        Next
    '    End If
    'End Sub

    Private Sub LoadCorporates(Optional ByVal loadForm As Boolean = True)
        Dim ds As DataSet
        With New HotelSistema
            ds = .GetCorporativos(0)
        End With
        If IsSupervisor Then
            dgCorporates.Columns(1).FooterText = ds.Tables(0).Select("Type = 2").Count & " " & PortalCulture.GetString("01219")
            dgCorporates.DataSource = ds.Tables(0).Select("Type = 2")
            dgCorporates.DataBind()
            dgCorporates.SelectedIndex = -1
            trCorporates.Visible = True
            FormAgreement.Visible = False
            btnSelectCorporate.Visible = True
        Else
            Dim corporates As DataRow() = ds.Tables(0).Select("idCorporativo='" + Me.idCorporate.ToString() + "'")
            If corporates.Length > 0 Then
                Me.idSelectedCorpororate.Value = corporates(0)("idCorporativo")
                Me.idSelectedCorpororateName.Value = corporates(0)("NombreCorp")
            Else

                MyBase.redirectTo(PaginaBase.pages.Home)

            End If
            trCorporates.Visible = False
            btnSelectCorporate.Visible = False
            If loadForm Then ResetForm()
        End If

    End Sub

    'Private Sub ddlCountries_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCountries.SelectedIndexChanged
    '    Call LoadStates()
    '    ddlStates_SelectedIndexChanged(sender, e)
    '    ddlStates.AutoUpdateAfterCallBack = True
    'End Sub

    'Private Sub ddlStates_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlStates.SelectedIndexChanged
    '    Call LoadDistricts()
    '    ddlDistricts_SelectedIndexChanged(sender, e)
    '    ddlDistricts.AutoUpdateAfterCallBack = True
    'End Sub

    'Private Sub ddlDistricts_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlDistricts.SelectedIndexChanged
    '    Call LoadCities()
    '    ddlCities.AutoUpdateAfterCallBack = True
    'End Sub

    Private Sub dgCorporates_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgCorporates.Init
        dgCorporates.Columns(0).HeaderText = PortalCulture.GetString("00838")
    End Sub

    Private Sub Convenios_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        lblCheckinHour.InnerHtml = PortalCulture.GetString("01470", True)
        chkCC.Text = PortalCulture.GetString("01469")
        Title = PortalCulture.GetString("01211")
        lblTitle.Text = PortalCulture.GetString("01211")
        'lblAgreementWorking.Text = PortalCulture.GetString("01206", True)
        lblAgency.Text = PortalCulture.GetString("01220", True)
        lblContact.Text = PortalCulture.GetString("00162", True)
        lblAddress.Text = PortalCulture.GetString("M0BT0000163", True)
        lblJob.Text = PortalCulture.GetString("00987", True)
        lblTown.Text = PortalCulture.GetString("00988", True)
        lblEmail.Text = PortalCulture.GetString("00181", True)
        lblCountry.Text = PortalCulture.GetString("00251", True)
        lblState.Text = PortalCulture.GetString("00252", True)
        lblDistrict.Text = PortalCulture.GetString("00253", True)
        lblCity.Text = PortalCulture.GetString("00254", True)
        lblZIP.Text = PortalCulture.GetString("00731", True)
        lblPhone.Text = PortalCulture.GetString("00164", True)
        lblNoAgreement.Text = PortalCulture.GetString("M0BT0000195", True)
        'lblGeneralRestrictions.Text = PortalCulture.GetString("01189", True)
        'lblEspecificRestrictions.Text = PortalCulture.GetString("01190", True)
        lblRateSearch.Text = PortalCulture.GetString("00016", True)
        lblCopyAgreement.Text = PortalCulture.GetString("01208", True)
        lblCorporateContact.Text = PortalCulture.GetString("01209", True)
        lblCorporateJob.Text = PortalCulture.GetString("01210", True)
        Me.rfvCorporateContact.ErrorMessage = PortalCulture.GetString("01439")
        lblCorporates.Text = PortalCulture.GetString("01214", True)
        cmdBack.Text = PortalCulture.GetString("00010")
        lblTitleHotels.Text = PortalCulture.GetString("01215").ToUpper
        lblTitleGeneralInformation.Text = PortalCulture.GetString("01216").ToUpper
        lblTip.Text = PortalCulture.GetString("01217")
        btnSelectCorporate.Text = PortalCulture.GetString("01218")
        lblMsg.Text = PortalCulture.GetString("01191")
        Me.chkIsAgency.Text = PortalCulture.GetString("01225")
        bnAgregarHomoClave.Text = PortalCulture.GetString("01580")
        lblHomoClave.Text = PortalCulture.GetString("01579")
        lblHAgency.Text = PortalCulture.GetString("01220", True)
        lblHContact.Text = PortalCulture.GetString("00162", True)
        lblHAddress.Text = PortalCulture.GetString("M0BT0000163", True)
        lblHJob.Text = PortalCulture.GetString("00987", True)
        lblHTown.Text = PortalCulture.GetString("00988", True)
        lblHEmail.Text = PortalCulture.GetString("00181", True)
        lblHCountry.Text = PortalCulture.GetString("00251", True)
        lblHState.Text = PortalCulture.GetString("00252", True)
        lblHDistrict.Text = PortalCulture.GetString("00253", True)
        lblHCity.Text = PortalCulture.GetString("00254", True)
        lblHZIP.Text = PortalCulture.GetString("00731", True)
        lblHPhone.Text = PortalCulture.GetString("00164", True)
        btnTarifaCovnenio.Text = PortalCulture.GetString("M0BT0000115")
        lblTarifaConvenio.Text = PortalCulture.GetString("01581")
        lblSegmento.Text = PortalCulture.GetString("M000353")

        lblTipoPago.Text = PortalCulture.GetString("M0UT02719", True)
        ddlTipoPago.Items.Clear()
        ddlTipoPago.Items.Add(PortalCulture.GetString("M0UT02720"))
        ddlTipoPago.Items.Add(PortalCulture.GetString("01445"))
        ddlTipoPago.DataBind()

        lblRPDesc.Text = PortalCulture.GetString("00002", True)

        btnNewConvenio.Text = PortalCulture.GetString("00752")
        Dim isEditMode As Boolean = IIf(CInt(hdnIdAgreement.Value) > 0, True, False)

        If isEditMode Then
            imgDelete.Visible = True
        End If
    End Sub

    Public Class clsCurrencySymbol

        Private Symbols(1, 52) As String

        Public Function GetSymbol(ByVal currencyCode As String) As String
            CreateArray()
            Dim result As String = "$"
            Dim length As Integer = CInt(Symbols.Length / 2)
            For i As Integer = 0 To length - 1
                If Symbols(0, i) = currencyCode Then
                    result = Symbols(1, i)
                    Exit For
                End If
            Next
            Return result
        End Function

        Public Function GetSymbol(ByVal idIndex As Integer) As String
            CreateArray()
            Dim result As String = "$"
            Try
                result = Symbols(1, idIndex)
            Catch ex As Exception
            End Try
            Return result
        End Function

        Private Sub CreateArray()
            Symbols(0, 0) = "MXN"
            Symbols(1, 0) = "$"
            Symbols(0, 1) = "USD"
            Symbols(1, 1) = "$"
            Symbols(0, 2) = "EUR"
            Symbols(1, 2) = "€"
            Symbols(0, 3) = "AED"
            Symbols(1, 3) = "د.إ"
            Symbols(0, 4) = "ARS"
            Symbols(1, 4) = "$"
            Symbols(0, 5) = "AUD"
            Symbols(1, 5) = "$"
            Symbols(0, 6) = "BOB"
            Symbols(1, 6) = "Bs"
            Symbols(0, 7) = "BRL"
            Symbols(1, 7) = "R$"
            Symbols(0, 8) = "BGN"
            Symbols(1, 8) = "лв"
            Symbols(0, 9) = "CAD"
            Symbols(1, 9) = "C$"
            Symbols(0, 10) = "CLP"
            Symbols(1, 10) = "$"
            Symbols(0, 11) = "CNY"
            Symbols(1, 11) = "¥"
            Symbols(0, 12) = "COP"
            Symbols(1, 12) = "$"
            Symbols(0, 13) = "HRK"
            Symbols(1, 13) = "kn"
            Symbols(0, 14) = "CYP"
            Symbols(1, 14) = "£"
            Symbols(0, 15) = "CZK"
            Symbols(1, 15) = "Kč"
            Symbols(0, 16) = "DKK"
            Symbols(1, 16) = "kr"
            Symbols(0, 17) = "DOP"
            Symbols(1, 17) = "RD$"
            Symbols(0, 18) = "EGP"
            Symbols(1, 18) = "ج.م"
            Symbols(0, 19) = "EEK"
            Symbols(1, 19) = "EEK"
            Symbols(0, 20) = "HKD"
            Symbols(1, 20) = "HK$"
            Symbols(0, 21) = "HUF"
            Symbols(1, 21) = "Ft"
            Symbols(0, 22) = "ISK"
            Symbols(1, 22) = "kr"
            Symbols(0, 23) = "INR"
            Symbols(1, 23) = "Rs."
            Symbols(0, 24) = "IDR"
            Symbols(1, 24) = "Rp"
            Symbols(0, 25) = "ILS"
            Symbols(1, 25) = "₪"
            Symbols(0, 26) = "JPY"
            Symbols(1, 26) = "¥"
            Symbols(0, 27) = "LVL"
            Symbols(1, 27) = "Ls"
            Symbols(0, 28) = "LYD"
            Symbols(1, 28) = "ل.د"
            Symbols(0, 29) = "LTL"
            Symbols(1, 29) = "Lt"
            Symbols(0, 30) = "MYR"
            Symbols(1, 30) = "RM"
            Symbols(0, 31) = "MTL"
            Symbols(1, 31) = "Lm"
            Symbols(0, 32) = "MUR"
            Symbols(1, 32) = "₨"
            Symbols(0, 33) = "MAD"
            Symbols(1, 33) = "د.م."
            Symbols(0, 34) = "NZD"
            Symbols(1, 34) = "$"
            Symbols(0, 35) = "NOK"
            Symbols(1, 35) = "kr"
            Symbols(0, 36) = "OMR"
            Symbols(1, 36) = "ر.ع."
            Symbols(0, 37) = "PEN"
            Symbols(1, 37) = "S/."
            Symbols(0, 38) = "PHP"
            Symbols(1, 38) = "P"
            Symbols(0, 39) = "PLN"
            Symbols(1, 39) = "zł"
            Symbols(0, 40) = "RON"
            Symbols(1, 40) = "L"
            Symbols(0, 41) = "RUB"
            Symbols(1, 41) = "руб"
            Symbols(0, 42) = "SKK"
            Symbols(1, 42) = "Sk"
            Symbols(0, 43) = "ZAR"
            Symbols(1, 43) = "R"
            Symbols(0, 44) = "KRW"
            Symbols(1, 44) = "₩"
            Symbols(0, 45) = "SDG"
            Symbols(1, 45) = "SD£"
            Symbols(0, 46) = "SEK"
            Symbols(1, 46) = "kr"
            Symbols(0, 47) = "CHF"
            Symbols(1, 47) = "Fr"
            Symbols(0, 48) = "TWD"
            Symbols(1, 48) = "NT$"
            Symbols(0, 49) = "THB"
            Symbols(1, 49) = "฿"
            Symbols(0, 50) = "TRY"
            Symbols(1, 50) = "YTL"
            Symbols(0, 51) = "GBP"
            Symbols(1, 51) = "£"
            Symbols(0, 52) = "VEB"
            Symbols(1, 52) = "Bs."
        End Sub
    End Class


    Private Sub dgAgreementWorking_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgAgreementWorking.ItemCommand
        If e.CommandName = "Select" Then
            Dim sdatosdespues As String
            LoadAgreement(dgAgreementWorking.DataKeys(e.Item.ItemIndex), sdatosdespues)
            hdnIdAgreement.Value = dgAgreementWorking.DataKeys(e.Item.ItemIndex)
            txtNoAgreement.Enabled = False
            tabEditConvenio.Visible = True
        End If
    End Sub

    Private Sub btnNewConvenio_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewConvenio.Click
        LoadHomoClaves(-1)
        LoadHotelesConveio(String.Empty)
        ResetForm()
        dlRatesPlan.DataBind()
        tabEditConvenio.Visible = True
    End Sub

    Private Sub dgAgreementWorking_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgAgreementWorking.PageIndexChanged
        dgAgreementWorking.CurrentPageIndex = e.NewPageIndex
        dgAgreementWorking.SelectedIndex = -1
        LoadAgreements(idCorporate, ctrlAutoComplete1.GetFilter)
    End Sub

    Private Sub ctrlAutoComplete1_onSendFilter(ByVal id As String, ByVal descripcion As String) Handles ctrlAutoComplete1.OnSendFilter
        dgAgreementWorking.CurrentPageIndex = 0
        'LoadAgreements(idCorporate, ctrlAutoComplete1.GetFilter)
        ResetForm()
        dgAgreementWorking.SelectedIndex = -1
    End Sub

    Protected Sub btnTarifaCovnenio_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTarifaCovnenio.Click
        LoadHotelesConveio(txtTarifaConvenio.Text)
    End Sub
    Private Sub LoadHotelesConveio(ByVal rateplan As String)

        Dim ds As DataSet = (New RatePlanAccess).GetHotelByTarifaConvenio(rateplan, idConvenio, -2)
        ' divHotelTarifasConvenio.Attributes("style") = "display:inline"
        If ds.Tables(0).Rows.Count > 0 Then
            dlHoteles.DataSource = ds
            dlHoteles.DataKeyField = "idHotel"
            dlHoteles.DataBind()
        End If
        lblMsgNumHoteles.Text = String.Format(PortalCulture.GetString("01584"), ds.Tables(0).Rows.Count.ToString(), rateplan)
        dlHoteles.Visible = ds.Tables(0).Rows.Count > 0
        dlHoteles.Enabled = True ' Not String.IsNullOrEmpty(rateplan)
        lblMsgNumHoteles.Visible = dlHoteles.Enabled
        txtTarifaConvenio.Text = rateplan
    End Sub

    Private Sub bnAgregarHomoClave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bnAgregarHomoClave.Click

        If Page.IsValid Then
            Dim company As String = txtHAgency.Text
            Dim addres As String = txtHAddress.Text
            Dim town As String = txtHTown.Text
            Dim zipcode As String = txtHZIP.Text
            Dim phone As String = txtHPhone.Text
            Dim contact As String = txtHContact.Text
            Dim workposition As String = txtHJob.Text
            Dim email As String = txtHEmail.Text
            Dim homoclave As String = txtHomoClave.Text

            'If (New RatePlanAccess().InsertHomoClave(idConvenio, company, addres, town, 0, "", zipcode, phone, contact, workposition, email, homoclave)) Then
            '    LoadHomoClaves(idConvenio)
            Dim ds As ConvenioHomoClaveData = dsConvenioHomoClave
            With ds
                Dim row As DataRow = .Tables(.CONVENIOHOMOCLAVE_TABLE).NewRow()
                row(.FIELD_COMPANY) = company
                row(.FIELD_ADDRESS) = addres
                row(.FIELD_TOWN) = town
                row(.FIELD_ZIPCODE) = zipcode
                row(.FIELD_PHONE) = phone
                row(.FIELD_CONTACT) = contact
                row(.FIELD_WORKPOSITION) = workposition
                row(.FIELD_EMAIL) = email
                row(.FIELD_HOMOCLAVE) = homoclave
                row(.FIELD_CITY) = Integer.Parse(selectedHCity.Value)
                row(.FIELD_COUNTRY) = selectedHCountry.Value
                row(.FIELD_IDCONVENIO) = idConvenio
                .Tables(.CONVENIOHOMOCLAVE_TABLE).Rows.Add(row)
            End With

            dsConvenioHomoClave = ds
            divHomoclave.Attributes("style") = "display:none;"

            'dgHomoClaves.DataSource = ds
            'dgHomoClaves.DataKeyField = "idConvenioHomoClave"
            'dgHomoClaves.DataBind()
            'Else
            '    divHomoclave.Attributes("style") = "display:inlne;"
            '    lblHError.Text = "Error al agregar homoclave."
            'End If

        End If
    End Sub

    Private Sub LoadHomoClaves(ByVal idconvenio As Integer)

        Dim ds As ConvenioHomoClaveData
        If idconvenio >= 0 Then
            ds = (New RatePlanFacade).GetConvenioHomoClaveByIDConvenio(idconvenio)
        Else
            ds = New ConvenioHomoClaveData()
        End If
        dsConvenioHomoClave = ds
        'dgHomoClaves.DataSource = ds
        'dgHomoClaves.DataKeyField = "idConvenioHomoClave"
        'dgHomoClaves.DataBind()
    End Sub

    ''Private Sub dgHomoClaves_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgHomoClaves.ItemCommand
    '    If e.CommandName = "Delete" Then
    'Dim ds As ConvenioHomoClaveData = dsConvenioHomoClave
    '        ds.Tables(ConvenioHomoClaveData.CONVENIOHOMOCLAVE_TABLE)(e.Item.DataSetIndex).Delete()
    '        dsConvenioHomoClave = ds
    ''dgHomoClaves.DataSource = ds
    ''dgHomoClaves.DataKeyField = "idConvenioHomoClave"
    ''dgHomoClaves.DataBind()
    '    End If
    'End Sub

    'Private Sub dgHomoClaves_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgHomoClaves.ItemDataBound
    '    If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then
    '        Dim LK As HyperLink
    '        Dim LK2 As LinkButton

    '        LK2 = e.Item.Cells(dgColHomoClaves.Eliminar).FindControl("lnkEliminar2")
    '        LK = e.Item.Cells(dgColHomoClaves.Eliminar).FindControl("lnkEliminar")
    '        LK.Text = PortalCulture.GetString("00103")
    '        'LK.NavigateUrl = CtlMensajes1.getShow(LK2.ClientID, PortalCulture.GetString("00047"), _
    '        'PortalCulture.GetString("00613") & ", " & PortalCulture.GetString("00464"))

    '        LK.NavigateUrl = CtlMensajes1.getShow(LK2.ClientID, PortalCulture.GetString("01579"), PortalCulture.GetString("01578"))
    '    ElseIf e.Item.ItemType = ListItemType.Header Then
    '        e.Item.Cells(dgColHomoClaves.Company).Text = PortalCulture.GetString("00073")
    '        e.Item.Cells(dgColHomoClaves.Contact).Text = PortalCulture.GetString("00629")
    '    End If
    'End Sub


    Private Sub dlHoteles_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlHoteles.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then
            Dim chk As CheckBox

            chk = e.Item.FindControl("chk")

            chk.Checked = (Not CType(e.Item.DataItem, DataRowView)("Check") = 0)
        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim chk As CheckBox
            If dlHoteles.DataSource.Tables(0).Select("Check=1").Length = dlHoteles.DataSource.Tables(0).Rows.Count Then
                chk = CType(e.Item.FindControl("chkAllHotels"), CheckBox)
                If Not chk Is Nothing Then
                    chk.Checked = True
                End If
            End If
        End If
    End Sub

    Public Sub LinkRatePlan_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        LoadHotelesConveio((CType(sender, LinkButton).Text))
    End Sub
    Public Function NewRatePlan(ByVal idHotel As String) As HiddenField
        Dim dsRate As New RatePlanData
        Dim Rp As RatePlanData
        Dim hf As New HiddenField
        Dim rRate As DataRow
        Dim dsRateCheck As New RatePlanData
        With New RatePlanFacade
            dsRateCheck = .GetDataRatePlan(txtNoAgreement.Text.Substring(0, 4), idHotel)
        End With
        Try
            rRate = dsRate.Tables(dsRate.RATEPLAN_TABLE).NewRow()
            With rRate
                .Item(dsRate.FIELD_IDRATEPLAN) = txtNoAgreement.Text.Substring(0, 4)
                .Item(dsRate.FIELD_DESCRIPTION) = descripcionRatePlan.textodefault
                .Item(dsRate.FIELD_SEGMENT) = "R"
                .Item(dsRate.FIELD_IDHOTEL) = idHotel
                .Item(dsRate.FIELD_IDDICDESC) = descripcionRatePlan.Insert()
                .Item(dsRate.FIELD_HOTELPAYMENT) = False
                .Item(dsRate.FIELD_CODIGOTARIFA) = txtNoAgreement.Text.Substring(0, 4)
                .Item(dsRate.FIELD_NAME) = txtAgency.Text
                '.Item(dsRate.FIELD_IDDICSHORTDESC) = "Tarifa Estandar"
                .Item(dsRate.FIELD_GDS) = False
                .Item(dsRate.FIELD_GDSAPPLY) = False
                .Item(dsRate.FIELD_PORTAL) = True
                .Item(dsRate.FIELD_UNIPANTALLA) = False
                .Item(dsRate.FIELD_ADS) = False
                .Item(dsRate.FIELD_COMGDS) = DBNull.Value
                .Item(dsRate.FIELD_COMPORTAL) = DBNull.Value
                .Item(dsRate.FIELD_COMONEPAGE) = DBNull.Value
                .Item(dsRate.FIELD_COMADS) = DBNull.Value
                .Item(dsRate.WAITLISTAVAILABLE_FIELD) = True
                '.Item(dsRate.FIELD_IDDICCPROMODESC) = Me.txtPromoDescription.IdIndice
                .Item(dsRate.FIELD_IDCONTRATO) = System.DBNull.Value
                .Item(dsRate.FIELD_IDRULE) = System.DBNull.Value
                '.Item(dsRate.FIELD_TIPOPAGO) = ddlTipoPago.SelectedIndex
            End With
            dsRate.Tables(dsRate.RATEPLAN_TABLE).Rows.Add(rRate)

        Catch
        End Try

        If dsRateCheck Is Nothing OrElse dsRateCheck.Tables(0).Rows.Count = 0 Then
            With New RatePlanFacade
                Dim insertedRP As Boolean = .InsertRatePlan(dsRate, 0, 0, 0)
            End With
        Else
            'Update RP Details
            With New RatePlanFacade
                .UpdateRatePlan(dsRate)
            End With
        End If

        hf.Value = txtNoAgreement.Text.Substring(0, 4)
        Return hf
    End Function
End Class