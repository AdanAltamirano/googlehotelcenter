Imports Portal.General.Common.Data
Imports Portal.General.Facade
Imports Portal.General.DataAccess
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports System.Runtime.Serialization

Imports System.IO
Imports System.Text


Partial Class ctrlFaresExc
    Inherits System.Web.UI.UserControl
    Public Property RatePlanRow() As RowRatePlan
        Get
            Return ViewState("_RatePlanRow")
        End Get
        Set(ByVal Value As RowRatePlan)
            VIEWSTATE("_RatePlanRow") = Value
        End Set
    End Property
    Public Property m_nameRoom() As String
        Get
            Return viewState("nameRoom")
        End Get
        Set(ByVal Value As String)
            viewState("nameRoom") = Value
        End Set
    End Property
    Public Property m_iFareId() As Integer
        Get
            Return ViewState("KEY_FAREID")
        End Get
        Set(ByVal Value As Integer)
            ViewState("KEY_FAREID") = Value
        End Set
    End Property
    Public Property m_iHotelId() As Integer
        Get
            Return ViewState("HotelID")
        End Get
        Set(ByVal Value As Integer)
            VIEWSTATE("HotelID") = Value
        End Set
    End Property
    Public Property m_startDate() As Date
        Get
            Return ViewState("StartDate")
        End Get
        Set(ByVal Value As Date)
            viewstate("StartDate") = Value
        End Set
    End Property
    Public Property m_endDate() As Date
        Get
            Return viewstate("EndDate")
        End Get
        Set(ByVal Value As Date)
            viewstate("EndDate") = Value
        End Set
    End Property
    Public Property m_RoomId() As Integer
        Get
            Return viewstate("RoomId")
        End Get
        Set(ByVal Value As Integer)
            viewstate("RoomId") = Value
        End Set
    End Property
    Public Property m_RatePlan()
        Get
            Return ViewState("RatePlan")
        End Get
        Set(ByVal Value)
            VIEWSTATE("RatePlan") = Value
        End Set
    End Property
    Public Property Adult() As String
        Get
            Return Me.txtAdult.Text
        End Get
        Set(ByVal Value As String)
            Me.txtAdult.Text = Value
        End Set
    End Property
    Public Property AdultExtra() As String
        Get
            Return Me.txtAdultExtra.Text
        End Get
        Set(ByVal Value As String)
            Me.txtAdultExtra.Text = Value
        End Set
    End Property
    Public Property Child() As String
        Get
            Return Me.txtChild.Text
        End Get
        Set(ByVal Value As String)
            Me.txtChild.Text = Value
        End Set
    End Property
    Public Property ChildExtra() As String
        Get
            Return Me.txtChildExtra.Text
        End Get
        Set(ByVal Value As String)
            Me.txtChildExtra.Text = Value
        End Set
    End Property

    Public Property Junior() As String
        Get
            Return Me.txtJunior.Text
        End Get
        Set(ByVal Value As String)
            Me.txtJunior.Text = Value
        End Set
    End Property

    Public Property JuniorExtra() As String
        Get
            Return Me.txtJuniorExtra.Text
        End Get
        Set(ByVal Value As String)
            Me.txtJuniorExtra.Text = Value
        End Set
    End Property

    Public Property change() As String
        Get
            Return Me.txtCambia.Text
        End Get
        Set(ByVal Value As String)
            Me.txtCambia.Text = Value
        End Set
    End Property

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Me.txtAdult.Attributes.Add("onChange", "javascript:FillPrices('Ctrlplanfares1_dgAdult" & "','txtAdult'" & ",'" & Me.txtAdult.ClientID & "','" & Me.ClientID & "_PriceAdultChildExc" & "')" & ";javascript:CambiaTxt('" & txtCambia.ClientID & "');CambiaTxt('" & txtCambia.ClientID & "')")
        Me.txtChild.Attributes.Add("onChange", "javascript:FillPrices('Ctrlplanfares1_dgChild" & "','txtChild'" & ",'" & Me.txtChild.ClientID & "','" & Me.ClientID & "_PriceAdultChildExc" & "')" & ";javascript:CambiaTxt('" & txtCambia.ClientID & "');CambiaTxt('" & txtCambia.ClientID & "')")
        Me.txtJunior.Attributes.Add("onChange", "javascript:FillPrices('Ctrlplanfares1_dgTeen" & "','txtTeenFare'" & ",'" & Me.txtJunior.ClientID & "','" & Me.ClientID & "_PriceAdultChildExc" & "')" & ";javascript:CambiaTxt('" & txtCambia.ClientID & "');CambiaTxt('" & txtCambia.ClientID & "')")
        Me.txtAdultExtra.Attributes.Add("onchange", "javascript:CambiaTxt('" & txtCambia.ClientID & "')")
        Me.txtChildExtra.Attributes.Add("onchange", "javascript:CambiaTxt('" & txtCambia.ClientID & "')")
        Me.txtJuniorExtra.Attributes.Add("onchange", "javascript:CambiaTxt('" & txtCambia.ClientID & "')")


        Me.txtAdult.Attributes.Add("onKeyUp", "javascript:ValidPrice('" & Me.txtAdult.ClientID & "', true)")
        Me.txtChild.Attributes.Add("onKeyUp", "javascript:ValidPrice('" & Me.txtChild.ClientID & "')")
        Me.txtJunior.Attributes.Add("onKeyUp", "javascript:ValidPrice('" & Me.txtJunior.ClientID & "')")
        Me.txtAdultExtra.Attributes.Add("onKeyUp", "javascript:ValidPrice('" & Me.txtAdultExtra.ClientID & "')")
        Me.txtChildExtra.Attributes.Add("onKeyUp", "javascript:ValidPrice('" & Me.txtChildExtra.ClientID & "')")
        Me.txtJuniorExtra.Attributes.Add("onKeyUp", "javascript:ValidPrice('" & Me.txtJuniorExtra.ClientID & "')")

        Me.txtAdult.Attributes.Add("onblur", "javascript:ValidPrice('" & Me.txtAdult.ClientID & "',true)")
        Me.txtChild.Attributes.Add("onblur", "javascript:ValidPrice('" & Me.txtChild.ClientID & "')")
        Me.txtJunior.Attributes.Add("onblur", "javascript:ValidPrice('" & Me.txtJunior.ClientID & "')")
        Me.txtAdultExtra.Attributes.Add("onblur", "javascript:ValidPrice('" & Me.txtAdultExtra.ClientID & "')")
        Me.txtChildExtra.Attributes.Add("onblur", "javascript:ValidPrice('" & Me.txtChildExtra.ClientID & "')")
        Me.txtJuniorExtra.Attributes.Add("onblur", "javascript:ValidPrice('" & Me.txtJuniorExtra.ClientID & "')")

        Dim isJunior As Boolean = CType(Me.Page, PaginaBase).isConfigAdolescente
        Me.lnkViewRate.NavigateUrl = "javascript:ShowTxt2('" & Me.lnkViewRate.ClientID & "','" & txtAdult.ClientID & "','" & txtAdultExtra.ClientID & "','" & txtChild.ClientID & "','" & txtChildExtra.ClientID & "','" & txtJunior.ClientID & "','" & txtJuniorExtra.ClientID & "','" & Me.lblAdult.ClientID & "','" & Me.lblAdultExtra.ClientID & "','" & Me.lblChild.ClientID & "','" & Me.lblChildExtra.ClientID & "','" & Me.lbljunior.ClientID & "','" & Me.lblJuniorExtra.ClientID & "','" & Me.hplShowRates.ClientID & "','" & Me.hPersonasExtras.ClientID & "'," & isJunior.ToString.ToLower & ")"
        Me.lnkViewRate.ToolTip = PortalCulture.GetString("01368")
        'Me.lnkViewRate.NavigateUrl = String.Format("FireHideControls('{0}')", ParametrosHide)
        Me.hplShowRates.Attributes.Add("onclick", "javascript:Ocultar('1','modalPage','" & Me.ClientID & "_PriceAdultChildExc" & "','" & Me.txtAdult.ClientID & "','" & Me.txtChild.ClientID & "','" & Me.txtJunior.ClientID & "')" & ";javascript:CambiaTxt('" & txtCambia.ClientID & "')")
        LoadCulture()
    End Sub
    Public Sub ClearData()
        Me.txtAdult.Text = ""
        Me.txtAdultExtra.Text = ""
        Me.txtChild.Text = ""
        Me.txtChildExtra.Text = ""
    End Sub

    Public Function Show(ByVal valor As Boolean)
        lnkDay.Visible = valor
        lnkViewRate.Visible = valor
        hplShowRates.Style.Add("display", "none")
        lblAdult.Style.Add("display", "none")
        lblAdultExtra.Style.Add("display", "none")
        lblChild.Style.Add("display", "none")
        lblChildExtra.Style.Add("display", "none")
        txtAdult.Style.Add("display", "none")
        txtAdultExtra.Style.Add("display", "none")
        txtChild.Style.Add("display", "none")
        txtChildExtra.Style.Add("display", "none")
        'If Not CType(Me.Page, PaginaBase).isConfigAdolescente Then
        lbljunior.Style.Add("display", "none")
        lblJuniorExtra.Style.Add("display", "none")
        txtJunior.Style.Add("display", "none")
        txtJuniorExtra.Style.Add("display", "none")
        'End If
    End Function
    Public Property Day() As Integer
        Get
            Return CInt(viewstate("_midia"))
        End Get
        Set(ByVal Value As Integer)
            viewstate("_midia") = Value
            Me.lnkDay.Text = Value
        End Set
    End Property
    Public WriteOnly Property Editable() As Boolean
        Set(ByVal Value As Boolean)
            lnkDay.Enabled = Value
            lnkViewRate.Enabled = Value
            txtAdult.Enabled = Value
            txtAdultExtra.Enabled = Value
            txtChild.Enabled = Value
            txtChildExtra.Enabled = Value
        End Set
    End Property
    Public WriteOnly Property ShowLink() As Boolean
        Set(ByVal Value As Boolean)
            lnkViewRate.Visible = Value
        End Set
    End Property

    Public Property PersonasExtras() As Integer
        Get
            Return hPersonasExtras.Value
        End Get
        Set(ByVal value As Integer)
            hPersonasExtras.Value = value
        End Set
    End Property

    Public Sub LoadFare()
        Dim dPrecio As Double
        ClearData()
        If m_iFareId <> 0 Then
            Dim datFares As DataSet
            Dim rowFare As DataRow
            'get fare information
            With New FaresSystem
                datFares = .GetFareByFareId(m_iFareId)
            End With
            If Not datFares Is Nothing AndAlso datFares.Tables(FaresData.FARES_TABLE).Rows.Count > 0 Then
                rowFare = datFares.Tables(FaresData.FARES_TABLE).Rows(0)
                Me.txtAdult.Text = CDbl(rowFare(FaresData.PRICE_FIELD))
                Me.txtChild.Text = CDbl(Val(rowFare(FaresData.NINIORATE).ToString))
                Me.txtAdultExtra.Text = CDbl(Val(rowFare(FaresData.EXTRAADULTPRICE_FIELD)))
                Me.txtChildExtra.Text = CDbl(Val(rowFare(FaresData.EXTRACHILDPRICE_FIELD)))

                dPrecio = 0
                If Not rowFare.IsNull(FaresData.RATEENPRICE_FIELD) Then
                    Double.TryParse(rowFare(FaresData.RATEENPRICE_FIELD), dPrecio)
                End If
                txtJunior.Text = dPrecio

                dPrecio = 0
                If Not rowFare.IsNull(FaresData.EXTRATEENPRICE_FIELD) Then
                    Double.TryParse(rowFare(FaresData.EXTRATEENPRICE_FIELD), dPrecio)
                End If
                txtJuniorExtra.Text = dPrecio
                Me.lnkViewRate.Text = "" & CDbl((Me.txtAdult.Text)).ToString
            End If
        End If
    End Sub


    Function GetMinRate(ByVal pa As Array) As Double
        Dim dMinPrecio As Double = 999999999
        Dim dConvPr As Double
        For k As Integer = 0 To pa.Length - 1
            If pa(k) <> "" Then
                Double.TryParse(pa(k), dConvPr)
                If dConvPr < dMinPrecio Then
                    dMinPrecio = dConvPr
                End If
            End If
        Next
        Return If(dMinPrecio = 999999999, pa(0), dMinPrecio)
    End Function

    Public Sub ReFill()
        Dim datRestrictions As FaresRestrictionsData
        Dim dtAdults As DataTable, dtChildren As DataTable
        Dim ad As Byte, ch As Byte
        Dim drnew As DataRow
        Dim strch As String = ""
        Dim PriceAdult As String = ""
        Dim priceChild As String = ""
        Dim priceJunior As String = ""
        Dim pa As Array
        Dim pc As Array
        Dim pj As Array
        Dim sDatos As String
        priceChild = ""

        dtAdults = New DataTable("Adults")
        dtChildren = New DataTable("Children")
        dtAdults.Columns.Add("Adults")
        dtAdults.Columns.Add("Price")
        dtAdults.Columns.Add(datRestrictions.PKIDRESTRICTION_FIELD)

        dtChildren.Columns.Add("Children")
        dtChildren.Columns.Add("Price")
        dtChildren.Columns.Add(datRestrictions.PKIDRESTRICTION_FIELD)

        datRestrictions = GetFareRestrictions(sDatos)
        If Not datRestrictions Is Nothing Then
            For Each dr As DataRow In datRestrictions.Tables(datRestrictions.FARESRESTRICTION_TABLE).Rows
                If dr(datRestrictions.ADULTNUMBER_FIELD) <> ad Then
                    ad = dr(datRestrictions.ADULTNUMBER_FIELD)
                    PriceAdult &= dr(datRestrictions.ADULTFARE_FIELD) & "$"
                End If
                If dr(datRestrictions.CHILDNUMBER_FIELD) > 0 AndAlso strch.IndexOf("," & dr(datRestrictions.CHILDNUMBER_FIELD) & ",") = -1 Then
                    ch = dr(datRestrictions.CHILDNUMBER_FIELD)
                    strch &= "," & ch & ","
                    priceChild &= dr(datRestrictions.CHILDFARE_FIELD) & "$"

                    If Not dr.IsNull(datRestrictions.TEENFARE_FIELD) Then
                        priceJunior &= dr(datRestrictions.TEENFARE_FIELD) & "$"
                    Else
                        priceJunior &= "0$"
                    End If

                End If
            Next
            If PriceAdult.Length > 0 Then
                Me.PriceAdultChildExc.Value = Mid(PriceAdult, 1, Len(PriceAdult) - 1)
            End If
            If priceChild.Length > 0 Then
                Me.PriceAdultChildExc.Value = Me.PriceAdultChildExc.Value & "|" & Mid(priceChild, 1, Len(priceChild) - 1)
            Else
                Me.PriceAdultChildExc.Value = Me.PriceAdultChildExc.Value & "|"
            End If
            If priceJunior.Length > 0 Then
                Me.PriceAdultChildExc.Value = Me.PriceAdultChildExc.Value & "|" & Mid(priceJunior, 1, Len(priceJunior) - 1)
            Else
                Me.PriceAdultChildExc.Value = Me.PriceAdultChildExc.Value & "|"
            End If


            pa = PriceAdult.Split("$")
            pc = priceChild.Split("$")
            pj = priceJunior.Split("$")

            If Len(pa(0)) > 0 Then
                'Dim dMinPrecio As Double = 999999999
                'Dim dConvPr As Double
                'For k As Integer = 0 To pa.Length - 1
                '    If pa(k) <> "" Then
                '        Double.TryParse(pa(k), dConvPr)
                '        If dConvPr < dMinPrecio Then
                '            dMinPrecio = dConvPr
                '        End If
                '    End If
                'Next
                'Me.txtAdult.Text = If(dMinPrecio = 999999999, pa(0), dMinPrecio)

                Me.txtAdult.Text = GetMinRate(pa)
            Else
                Me.txtAdult.Text = "0"
            End If
            If Len(pc(0)) > 0 Then
                Me.txtChild.Text = pc(0)
                Me.txtChild.Text = GetMinRate(pc)
            Else
                '  Me.txtChild.Text = "0"
            End If
            If Len(pj(0)) > 0 Then
                Me.txtJunior.Text = pj(0)
                Me.txtJunior.Text = GetMinRate(pj)
            Else
                '  Me.txtJunior.Text = "0"
            End If


            If CDbl((Me.txtAdult.Text)) <> 0 Then
                Me.lnkViewRate.Text = "" & CDbl((Me.txtAdult.Text)).ToString
            End If
        End If
    End Sub

    Private Function GetFareRestrictions(ByRef sDatos As String) As FaresRestrictionsData
        Dim datRestrictions As New FaresRestrictionsData
        Dim datDatagridRestrictions As New FaresRestrictionsData
        Dim datRooms As RoomsHotelData
        Dim datFare As FaresData
        Dim iRoomId As Integer
        Dim iPriceRoom As Decimal
        Dim iPriceExtraChild As Decimal
        Dim iPeople As Integer
        Dim iChild As Integer
        Dim iAdult As Integer
        Dim iMaxPeople As Integer


        If Me.m_iFareId <> 0 Then
            ' get fare data
            With New FaresSystem
                datFare = .GetFareById(Me.m_iFareId)
            End With

            ' get fares' restrictions
            With New FaresRestrictionSystem
                datRestrictions = .GetFareRestrictionByFareId(Me.m_iFareId)
            End With
        End If

        Try
            datRestrictions.Tables(0).Columns.Add("Descr_rateplan")
            datRestrictions.Tables(0).Rows(0)("Descr_rateplan") = String.Format("{0}", ViewState("nameRoom"))
        Catch ex As Exception
        End Try

        sDatos = Util.Utility.GetXml(datRestrictions.FARESRESTRICTION_TABLE, "UpdateRateRestrictionByDay", datRestrictions)

        ' get room id
        If m_RoomId <> 0 Then
            iRoomId = m_RoomId
            If Not datFare Is Nothing AndAlso datFare.Tables(FaresData.FARES_TABLE).Rows.Count > 0 Then
                iPriceRoom = CInt(Val(datFare.Tables(FaresData.FARES_TABLE).Rows(0)(FaresData.PRICE_FIELD).ToString))
                iPriceExtraChild = CInt(Val(datFare.Tables(FaresData.FARES_TABLE).Rows(0)(FaresData.NINIORATE).ToString))
            Else
                iPriceRoom = 0
                iPriceExtraChild = 0
            End If
        Else
            Return Nothing
        End If

        ' get room infomration
        With New RoomFacade
            datRooms = .getRoomByID(iRoomId)
        End With
        Dim rowRoom As DataRow
        If Not datRooms Is Nothing AndAlso datRooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows.Count > 0 Then
            rowRoom = datRooms.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows(0)
            iPeople = rowRoom(RoomsHotelData.FLD_NUMBER_PEOPLESINROOM)
            iChild = rowRoom(RoomsHotelData.FLD_NUMBER_MAXCHILDREN)
            iAdult = rowRoom(RoomsHotelData.FLD_NUMBER_MAXADULTS)
            iMaxPeople = rowRoom(RoomsHotelData.FLD_NUMBER_PEOPLESINROOM) + rowRoom(RoomsHotelData.FLD_NUMBER_PEOPLESEXTRAS)
        End If

        Dim idxAdults As Integer
        Dim idxChild As Integer

        For idxAdults = 1 To iAdult
            For idxChild = 0 To iChild
                'Combinaciond de adultos - niños
                ' check if row is actually in fare's restrictions
                Dim newRow As DataRow = RowInPlan(datRestrictions, idxAdults, idxChild)
                If Not newRow Is Nothing Then
                    ' if row is in fare's restrictions get restriction data
                    datDatagridRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).ImportRow(newRow)
                Else
                    ' if not, create new fares data
                    newRow = datDatagridRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).NewRow()
                    With newRow
                        .Item(FaresRestrictionsData.ADULTFARE_FIELD) = iPriceRoom
                        .Item(FaresRestrictionsData.ADULTNUMBER_FIELD) = idxAdults
                        .Item(FaresRestrictionsData.CHILDFARE_FIELD) = iPriceExtraChild '* idxChild
                        .Item(FaresRestrictionsData.CHILDNUMBER_FIELD) = idxChild
                        .Item(FaresRestrictionsData.IDFARE_FIELD) = Me.m_iFareId
                        .Item(FaresRestrictionsData.PKIDRESTRICTION_FIELD) = 0
                        .Item(FaresRestrictionsData.EXCADULTFARE_FIELD) = 0 'iPriceRoomExc
                        .Item(FaresRestrictionsData.EXCNINIOFARE_FIELD) = 0 'iPriceExtraChildExc * idxChild

                    End With
                    datDatagridRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows.Add(newRow)
                End If

            Next
        Next

        datDatagridRestrictions.AcceptChanges()
        Return datDatagridRestrictions

    End Function
    Private Function RowInPlan(ByVal datRestrictions As FaresRestrictionsData, ByVal iAdults As Integer, ByVal iChildren As Integer) As DataRow
        Dim row As DataRow
        For Each row In datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows
            If row(FaresRestrictionsData.ADULTNUMBER_FIELD) = iAdults And _
            row(FaresRestrictionsData.CHILDNUMBER_FIELD) = iChildren Then
                Return row
            End If
        Next
        Return Nothing
    End Function
    Public Sub LoadCulture()
        hplShowRates.Text = PortalCulture.GetString("00280")
        Me.lblAdult.Text = Mid(PortalCulture.GetString("00687"), 1, 2).Trim
        Me.lblAdultExtra.Text = Mid(PortalCulture.GetString("00688"), 1, 2).Trim
        Me.lblChild.Text = Mid(PortalCulture.GetString("00689"), 1, 2).Trim
        Me.lblChildExtra.Text = Mid(PortalCulture.GetString("00690"), 1, 2).Trim
    End Sub

    Public Function res_Save(Optional ByVal dt As DataTable = Nothing) As Boolean
        Dim f1 As Date
        Dim f2 As Date
        f1 = Me.m_startDate.ToString("MM/dd/yyyy")
        f2 = Me.m_endDate.ToString("MM/dd/yyyy")
        If AddFare(Me.m_RoomId, m_iFareId, f1, f2, Me.m_nameRoom, Me.m_RatePlan, Nothing, Nothing) = True Then
            Call SaveRes(Me.m_nameRoom, Me.m_RatePlan)
        End If
    End Function

    Function Nota(ByVal rooom As String, ByVal f1last As String, ByVal f2last As String, ByVal rp As String, ByVal f1 As String, ByVal f2 As String, ByRef sreference As String) As String
        Dim msg As String = "Se modificó la tarifa de la habitación " & rooom & " de la fecha " & f1last & " a la fecha " & f2last & " con el rateplan " & rp & " su nueva fecha es (o sigue siendo) del " & f1 & " al " & f2 & " el rateplan es (o sigue siendo) " & Me.m_RatePlan
        Dim drhotel As DataRow = CType(Me.Page, PaginaBase).HotelInfo
        Dim idioma As String

        sreference = "Cambio de tarifa por día"
        idioma = IIf(drhotel.IsNull("idiomaemail"), "en-US", drhotel.Item("idiomaemail"))
        If idioma = "en-US" Then
            sreference = "Update rate by day"
            msg = "It changed the room rate " & rooom & " from " & f1last & " to " & f2last & " with rateplan " & rp & " the new date is (or remains) of " & f1 & " to " & f2 & " , with rateplan " & Me.m_RatePlan
        End If
        Return msg
    End Function

    Function getDataXML(ByVal ds) As String
        Return Util.Utility.GetXml(ds.FARESRESTRICTION_TABLE, "UpdatePlanFaresNR", ds)
    End Function

    Public Function AddFare(ByVal idRoom As Integer, ByRef idFare As Integer, ByVal f1 As Date, ByVal f2 As Date, ByVal ch As String, ByVal rp As String, ByVal f1last As String, ByVal f2last As String) As Boolean
        Dim ds As RatePlanData
        Dim sdatos As String
        Dim sdatosDespues As String

        With New RatePlanFacade
            ds = .GetDataRatePlan(Me.m_RatePlan, Me.m_iHotelId)
        End With

        If ds.Tables(ds.RATEPLAN_TABLE).Rows.Count > 0 Then
            With ds.Tables(ds.RATEPLAN_TABLE).Rows(0)
                Try
                    Me.RatePlanRow = New RowRatePlan
                    Me.RatePlanRow.IDRATEPLAN = .Item(ds.FIELD_IDRATEPLAN)
                    Me.RatePlanRow.SEGMENT = .Item(ds.FIELD_SEGMENT)
                    Me.RatePlanRow.RATECODE = .Item(ds.FIELD_CODIGOTARIFA)
                Catch ex As Exception
                    Return False
                End Try
            End With
        End If
        If Me.m_iFareId = 0 Then
            If SaveNewFare(idRoom, idFare, f1, f2, sdatos) Then
                CType(Me.Page, PaginaBase).guardalog("/Pages/FaresCatalogueException.aspx", PaginaBase.acciones.Crear, "Se creó la tarifa de la habitación " & ch.Substring(0, ch.IndexOf("--")) & " de la fecha " & f1 & " a la fecha " & f2 & " con el rateplan " & Me.m_RatePlan, "", "", sdatos)
                Return True
            End If
            Return False
        Else
            Dim dsBefore As New DataSet
            Dim dsTrans As New FaresData
            Dim sDatoCorreo As String
            Dim sference As String = ""
            If UpdateFare(idRoom, f1, f2, dsBefore, dsTrans) Then
                sDatoCorreo = CreateRPDHtml(dsBefore, dsTrans)
                'sdatos = ds.GetXml.ToString
                sdatos = Util.Utility.GetXml(dsTrans.FARES_TABLE, "UpdateRateByDay", dsBefore)
                'sdatosDespues = ds.GetXml.ToString
                sdatosDespues = Util.Utility.GetXml(dsTrans.FARES_TABLE, "UpdateRateByDay", dsTrans)
                Dim snota As String = Nota(ch, f1last, f2last, rp, f1, f2, sference)
                CType(Me.Page, PaginaBase).guardalog("/Pages/FaresCatalogueException.aspx", PaginaBase.acciones.Modificar, snota, sference, sdatos, sdatosDespues, sDatoCorreo)
                'CType(Me.Page, PaginaBase).guardalog("/Pages/FaresCatalogueException.aspx", PaginaBase.acciones.Modificar, "Se modificó la tarifa de la habitación " & ch & " de la fecha " & f1last & " a la fecha " & f2last & " con el rateplan " & rp & " su nueva fecha es (o sigue siendo) del " & f1 & " al " & f2 & " el rateplan es (o sigue siendo) " & Me.m_RatePlan, "Update rate by day", sdatos, sdatosDespues, sDatoCorreo)
                Return True
            End If
            Return False
            m_iFareId = 0
        End If
        Return False
    End Function


    Public Function CreateRPDHtml(ByVal dsBefore As DataSet, ByVal dsFares As FaresData) As String
        Dim menu As New Table
        Dim tr As TableRow
        Dim td As TableCell
        Dim dv As New DataView
        Dim sw As StringWriter = New StringWriter
        Dim writer As HtmlTextWriter = New HtmlTextWriter(sw)
        Dim shtml As String = ""
        Dim idioma As String
        Dim drhotel As DataRow = CType(Me.Page, PaginaBase).HotelInfo

        idioma = IIf(drhotel.IsNull("idiomaemail"), "en-US", drhotel.Item("idiomaemail"))

        menu.CellSpacing = 1
        menu.CellPadding = 1
        'menu.BorderWidth = 1
        menu.Width = New System.Web.UI.WebControls.Unit(660, UnitType.Pixel)

        tr = New TableRow
        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("00122", idioma)  ' "Rate"
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("00056", idioma)  ' "Room " 
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("00278", idioma)  ' "Adult Price"
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("00279", idioma)  ' "Price Child"   
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("00115", idioma)  ' "Extra Price Child"  
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("00114", idioma)  ' "Extra Price Adult"  
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("00016", idioma)  ' "Rate Plan"
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("M000192", idioma)  ' "Star date"
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("M000193", idioma)  ' "End Date" 
        tr.Cells.Add(td)
        menu.Rows.Add(tr)
        Dim codigo As String
        Dim habitacion As String

        For Each dr As DataRow In dsBefore.Tables(0).Rows
            tr = New TableRow
            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            codigo = dr("CodigoTarifa")
            habitacion = dr("NombreTipoHabitacion")
            td.Text = codigo
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = dr("NombreTipoHabitacion")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = Format(dr("Precio"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = Format(dr("niniosrate"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = Format(dr("PrecioExtraNinio"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = Format(dr("PrecioExtraAdulto"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = dr("idrateplan")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = dr("FechaInicia")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = dr("FechaFinaliza")
            tr.Cells.Add(td)
            menu.Rows.Add(tr)
        Next
        For Each dr As DataRow In dsFares.Tables(0).Rows
            tr = New TableRow
            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = codigo
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = habitacion
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = Format(dr("Precio"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = Format(dr("niniosrate"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = Format(dr("PrecioExtraNinio"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = Format(dr("PrecioExtraAdulto"), "########0.00")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = dr("idrateplan")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = dr("FechaInicia")
            tr.Cells.Add(td)

            td = New TableHeaderCell
            td.Attributes.Add("class", "dow")
            td.Text = dr("FechaFinaliza")
            tr.Cells.Add(td)
            menu.Rows.Add(tr)

        Next
        menu.RenderControl(writer)
        shtml = sw.ToString()

        Return shtml
    End Function

    Public Function SaveNewFare(ByVal idRoom As Integer, ByRef idtar As Integer, ByVal f1 As Date, ByVal f2 As Date, ByRef sdato As String) As Boolean
        Dim datFare As New FaresData
        Dim ExistCode As New FaresData
        Dim rowFare As DataRow
        Dim dv As DataView
        'Dim sdatosDespues As String

        'buscar el codigo que le pertenece
        Dim room As RoomsHotelData
        With New RoomFacade
            room = .getRoomByID(idRoom)
        End With
        If room.Tables(room.TBL_ROOM_HOTEL).Rows.Count = 0 Then Return False
        Try
            With datFare.Tables(FaresData.FARES_TABLE)
                rowFare = .NewRow()
                rowFare(FaresData.ENDDATE_FIELD) = f2
                rowFare(FaresData.EXTRAADULTPRICE_FIELD) = CDbl(Val(txtAdultExtra.Text))
                rowFare(FaresData.EXTRACHILDPRICE_FIELD) = CDbl(Val(txtChildExtra.Text))
                rowFare(FaresData.EXTRATEENPRICE_FIELD) = CDbl(Val(txtJuniorExtra.Text))
                rowFare(FaresData.PRICE_FIELD) = CDbl(Me.txtAdult.Text)
                rowFare(FaresData.NINIORATE) = CDbl(Me.txtChild.Text)
                rowFare(FaresData.RATEENPRICE_FIELD) = CDbl(Me.txtJunior.Text)

                rowFare(FaresData.STARTDATE_FIELD) = f1
                rowFare(FaresData.EXCEPTION_FIELD) = "NNNNNNN"

                rowFare(FaresData.RULESDEFAULT) = True
                rowFare(FaresData.NOARRIVOS_FIELD) = "NNNNNNN"
                rowFare(FaresData.HOTELROOMTYPEID_FIELD) = idRoom
                rowFare(FaresData.RATETYPE_FIELD) = RatePlanRow.SEGMENT
                rowFare(FaresData.IDRATEPLAN_FIELD) = RatePlanRow.IDRATEPLAN
                rowFare(FaresData.RATECODE_FIELD) = room.Tables(room.TBL_ROOM_HOTEL).Rows(0).Item(RoomsHotelData.FLD_ROOM_CODE) & Me.m_RatePlan
                .Rows.Add(rowFare)
            End With

            With New FaresSystem
                Try
                    If .InsertFares(datFare) Then
                        idtar = datFare.Tables(FaresData.FARES_TABLE).Rows(0)(FaresData.PKIDFARES_FIELD)
                        sdato = Util.Utility.GetXml(FaresData.FARES_TABLE, "UpdateRateByDay", datFare)
                        Me.m_iFareId = idtar
                    End If
                Catch ex As OverflowException
                    Return False
                End Try
            End With
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function
    Public Function UpdateFare(ByVal idroom As Integer, ByVal f1 As Date, ByVal f2 As Date, ByRef dsBefore As DataSet, ByRef dsTrans As FaresData) As Boolean
        Dim datFares As New FaresData
        Dim ExistCode As New FaresData
        Dim fareRow As DataRow
        Dim bResult As Boolean

        dsBefore = (New FaresSystem).GetFareByFareId(Me.m_iFareId)

        With datFares
            fareRow = .Tables(.FARES_TABLE).NewRow()
            Try
                ' try to fill fare row data
                fareRow(.PKIDFARES_FIELD) = Me.m_iFareId
                fareRow(.ENDDATE_FIELD) = Format(f2, "yyyy/MM/dd")
                fareRow(.EXTRAADULTPRICE_FIELD) = Double.Parse(Me.txtAdultExtra.Text)
                fareRow(.EXTRACHILDPRICE_FIELD) = Double.Parse(Me.txtChildExtra.Text)
                fareRow(.EXTRATEENPRICE_FIELD) = Double.Parse(Me.txtJuniorExtra.Text)

                fareRow(FaresData.EXCEPTION_FIELD) = "NNNNNNN"
                fareRow(.HOTELROOMTYPEID_FIELD) = idroom
                fareRow(.PRICE_FIELD) = Double.Parse(Me.txtAdult.Text)
                fareRow(.NINIORATE) = CDbl(Me.txtChild.Text)
                fareRow(.RATEENPRICE_FIELD) = CDbl(Me.txtJunior.Text)

                fareRow(.STARTDATE_FIELD) = Format(CDate(f1), "yyyy/MM/dd")
                'fareRow(.NINIORATE) = 0
                fareRow(FaresData.NOARRIVOS_FIELD) = "NNNNNNN"
                fareRow(FaresData.RULESDEFAULT) = True

                fareRow(FaresData.IDRATEPLAN_FIELD) = RatePlanRow.IDRATEPLAN
                .Tables(.FARES_TABLE).Rows.Add(fareRow)
                ' set row state to modified
                fareRow.AcceptChanges()
                fareRow(.PKIDFARES_FIELD) = fareRow(.PKIDFARES_FIELD)
                With New FaresSystem
                    Try
                        bResult = .ActualizaFares(datFares)
                        dsTrans = datFares
                        dsTrans.AcceptChanges()
                    Catch ex As OverflowException
                        Return False
                    End Try
                End With
                If bResult = True Then
                    Me.m_iFareId = .Tables(.FARES_TABLE).Rows(0)(.PKIDFARES_FIELD)
                End If
            Catch ex As Exception
                Return False
            End Try
        End With

        'Update Restrictions


        Return True
    End Function
    Public Sub SetDefaultValues()
        ' Set default fare values of input boxes if no values entered
        txtAdult.Text = "0"
        txtAdultExtra.Text = "0"
        txtChild.Text = "0"
        txtChildExtra.Text = "0"
        txtJunior.Text = "0"
        txtJuniorExtra.Text = "0"
    End Sub
    Function isValidData(ByRef Err As String) As Boolean
        If IsNumeric(Adult()) And IsNumeric(AdultExtra()) And IsNumeric(Child()) And IsNumeric(ChildExtra()) Then
            If CInt(Adult()) < 0 Then
                Err = "Adult<0"
            End If
        End If
    End Function

    Private Function SaveRes(ByVal room As String, ByVal rp As String)
        Dim validate As Boolean = True
        Dim sDatos As String
        Dim datRestrictions As FaresRestrictionsData = GetFareRestrictions(sDatos)
        Dim row As DataRow
        Dim i As Integer
        Dim Ad As Integer
        Dim Ch As Integer
        Dim dr As DataRow
        ' Check if the input controls are valid 
        If Not Page.IsValid Then
            Return False
        End If
        Dim adultPrices As String
        Dim childPrices As String
        Dim juniorPrices As String
        Dim Prices As Array
        Dim adult As Array
        Dim child As Array
        Dim junior As Array
        Dim valid As Boolean = False

        Prices = Split(PriceAdultChildExc.Value, "|")
        adultPrices = Prices(0)
        childPrices = Prices(1)
        juniorPrices = Prices(2)

        adult = Split(adultPrices, "$")
        child = Split(childPrices, "$")
        junior = Split(juniorPrices, "$")

        For i = 0 To datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows.Count - 1
            Dim chkActivo As CheckBox
            Dim chkActivo2 As CheckBox
            Dim txtAdultFare As String
            Dim txtChildFare As String
            Dim txtJuniorFares As String

            dr = datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows(i)

            Ch = dr(datRestrictions.CHILDNUMBER_FIELD)
            Ad = dr(datRestrictions.ADULTNUMBER_FIELD)

            txtAdultFare = adult(Ad - 1)
            txtChildFare = Nothing
            txtJuniorFares = Nothing

            If Ch > 0 And child.Length >= Ch Then ''''''''''''para los ninios''''''''''''''''''''''''
                txtChildFare = child(Ch - 1)

                txtJuniorFares = junior(Ch - 1)
            End If

            With datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE)
                If txtAdultFare.Trim.Length = 0 Then
                    txtChildFare = "0"
                End If

                ' si se ha modificado el registro entonces lo marcamos como modified
                If .Rows(i)(FaresRestrictionsData.ADULTFARE_FIELD) <> Double.Parse(txtAdultFare) Then
                    .Rows(i)(FaresRestrictionsData.ADULTFARE_FIELD) = Double.Parse(txtAdultFare)
                End If

                ' si se ha modificado el registro entonces lo marcamos como modified
                If Not txtChildFare Is Nothing Then
                    If txtChildFare.Trim.Length = 0 Then
                        txtChildFare = "0"
                    End If
                End If

                If Not txtChildFare Is Nothing Then
                    If .Rows(i)(FaresRestrictionsData.CHILDFARE_FIELD) <> Double.Parse(txtChildFare) Then
                        .Rows(i)(FaresRestrictionsData.CHILDFARE_FIELD) = Double.Parse(txtChildFare)
                    End If
                End If
                If Not txtChildFare Is Nothing Then
                    .Rows(i)(FaresRestrictionsData.CHILDFARE_FIELD) = Double.Parse(txtChildFare)
                Else
                    .Rows(i)(FaresRestrictionsData.CHILDFARE_FIELD) = 0
                End If

                If Not txtJuniorFares Is Nothing Then
                    .Rows(i)(FaresRestrictionsData.TEENFARE_FIELD) = Double.Parse(txtJuniorFares)
                Else
                    .Rows(i)(FaresRestrictionsData.TEENFARE_FIELD) = 0
                End If
                .Rows(i)(FaresRestrictionsData.ADULTFARE_FIELD) = Double.Parse(txtAdultFare)
                .Rows(i)(FaresRestrictionsData.EXCADULTFARE_FIELD) = 0
                .Rows(i)(FaresRestrictionsData.EXCNINIOFARE_FIELD) = 0
            End With
        Next

        If validate Then
            Try
                datRestrictions.Tables(0).Columns.Add("Descr_rateplan")
                datRestrictions.Tables(0).Rows(0)("Descr_rateplan") = String.Format("{0}", ViewState("nameRoom"))
            Catch ex As Exception
            End Try
            Dim sdatoDespues As String = Util.Utility.GetXml(datRestrictions.FARESRESTRICTION_TABLE, "UpdateRateRestrictionByDay", datRestrictions)
            With New FaresRestrictionSystem
                valid = .UpdateFaresRestrictions(datRestrictions)
                If valid Then
                    CType(Me.Page, PaginaBase).guardalog("/Pages/FaresCatalogueException.aspx", PaginaBase.acciones.Modificar, String.Format("Se modificó la tarifa por dia de la habitación {0} con el rateplan  {1} ", room, rp), "Update rates by ocupacion by day", sDatos, sdatoDespues)
                    Return valid
                End If
            End With
        Else
            Return False
        End If

    End Function
    
End Class
