Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports System.IO


Partial Class ctrlPlanFaresNR
    Inherits System.Web.UI.UserControl

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Const KEY_FAREID As String = "FareId"
    Const KEY_ROOMID As String = "RoomId"

    Enum RestrictionsDtgCols As Integer
        AdultNumber = 0
        AdultFare
        ChildNumber
        ChildFare
        TotalFare
        RestrictionId
        Tools
    End Enum

    Public ReadOnly Property iddg() As String
        Get
            'Return dtgRestrictions.ClientID
        End Get
    End Property

    Public ReadOnly Property iddgAdult() As String
        Get
            Return dgAdult.ClientID
        End Get
    End Property

    Public ReadOnly Property iddgChild() As String
        Get
            Return dgChild.ClientID
        End Get
    End Property

    Public ReadOnly Property iddgTeen() As String
        Get
            Return dgTeen.ClientID
        End Get
    End Property

    Public Property m_iFareId() As Integer
        Get
            Return ViewState(KEY_FAREID)
        End Get
        Set(ByVal Value As Integer)
            ViewState(KEY_FAREID) = Value
        End Set
    End Property

    Public Property m_iRoomId() As Integer
        Get
            Return ViewState(KEY_ROOMID)
        End Get
        Set(ByVal Value As Integer)
            ViewState(KEY_ROOMID) = Value
        End Set
    End Property

    Public Property m_TextBoxPorcMin() As String
        Get
            Return viewstate("TextBoxPorcMinClientId")
        End Get
        Set(ByVal Value As String)
            viewstate("TextBoxPorcMinClientId") = Value
        End Set
    End Property

    Public Property m_TextBoxPorcMax() As String
        Get
            Return viewstate("TextBoxPorcMaxClientId")
        End Get
        Set(ByVal Value As String)
            viewstate("TextBoxPorcMaxClientId") = Value
        End Set
    End Property


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            BindDataGridColumns()
            FillDataGrid()
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

    End Sub


#Region "FILL DE LOS NUEVOS DATAGRIDS"
    Private Sub FillDatas()
        Dim datRestrictions As FaresRestrictionsData
        Dim dtAdults As DataTable, dtChildren As DataTable
        Dim dtTeen As DataTable
        Dim ad As Byte, ch As Byte
        Dim drnew As DataRow
        Dim strch As String = ""

        dtAdults = New DataTable("Adults")
        dtChildren = New DataTable("Children")
        dtTeen = New DataTable("Teen")
        dtAdults.Columns.Add("Adults")
        dtAdults.Columns.Add("Price")
        dtAdults.Columns.Add("PriceNR")
        dtAdults.Columns.Add(FaresRestrictionsData.PKIDRESTRICTION_FIELD)

        dtChildren.Columns.Add("Children")
        dtChildren.Columns.Add("Price")
        dtChildren.Columns.Add("PriceNR")
        dtChildren.Columns.Add(FaresRestrictionsData.PKIDRESTRICTION_FIELD)

        dtTeen.Columns.Add("Teen")
        dtTeen.Columns.Add("Price")
        dtTeen.Columns.Add("PriceNR")
        dtTeen.Columns.Add(FaresRestrictionsData.PKIDRESTRICTION_FIELD)

        datRestrictions = GetFareRestrictions()
        If Not datRestrictions Is Nothing Then
            For Each dr As DataRow In datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows
                If dr(FaresRestrictionsData.ADULTNUMBER_FIELD) <> ad Then
                    ad = dr(FaresRestrictionsData.ADULTNUMBER_FIELD)
                    drnew = dtAdults.NewRow
                    drnew("Adults") = ad
                    drnew("Price") = dr(FaresRestrictionsData.ADULTFARE_FIELD)
                    drnew("PriceNR") = IIf(dr(FaresRestrictionsData.ADULTFARENR_FIELD) Is DBNull.Value, 0, dr(FaresRestrictionsData.ADULTFARENR_FIELD))
                    drnew(FaresRestrictionsData.PKIDRESTRICTION_FIELD) = dr(FaresRestrictionsData.PKIDRESTRICTION_FIELD)
                    dtAdults.Rows.Add(drnew)
                End If
                If dr(FaresRestrictionsData.CHILDNUMBER_FIELD) > 0 AndAlso strch.IndexOf("," & dr(FaresRestrictionsData.CHILDNUMBER_FIELD) & ",") = -1 Then
                    ch = dr(FaresRestrictionsData.CHILDNUMBER_FIELD)
                    strch &= "," & ch & ","
                    drnew = dtChildren.NewRow
                    drnew("Children") = ch
                    drnew("Price") = dr(FaresRestrictionsData.CHILDFARE_FIELD)
                    drnew("PriceNR") = IIf(dr(FaresRestrictionsData.CHILDFARENR_FIELD) Is DBNull.Value, 0, dr(FaresRestrictionsData.CHILDFARENR_FIELD))
                    drnew(FaresRestrictionsData.PKIDRESTRICTION_FIELD) = dr(FaresRestrictionsData.PKIDRESTRICTION_FIELD)
                    dtChildren.Rows.Add(drnew)

                    drnew = dtTeen.NewRow
                    drnew("Teen") = ch
                    drnew("Price") = dr(FaresRestrictionsData.TEENFARE_FIELD)
                    drnew("PriceNR") = IIf(dr(FaresRestrictionsData.TEENFARENR_FIELD) Is DBNull.Value, 0, dr(FaresRestrictionsData.TEENFARENR_FIELD))
                    drnew(FaresRestrictionsData.PKIDRESTRICTION_FIELD) = dr(FaresRestrictionsData.PKIDRESTRICTION_FIELD)
                    dtTeen.Rows.Add(drnew)
                End If
            Next
            dgChild.DataSource = dtChildren
            dgAdult.DataSource = dtAdults
            dgChild.DataBind()
            dgAdult.DataBind()

            If CType(Me.Page, PaginaBase).isConfigAdolescente Then
                dgTeen.DataSource = dtTeen
                dgTeen.DataBind()
            End If
        End If

       




    End Sub
#End Region

#Region "METODOS  y  PROCEDIMIENTOS"

    Private Sub FillDataGrid()
        Dim datRestrictions As FaresRestrictionsData
        datRestrictions = GetFareRestrictions()
        'If Not datRestrictions Is Nothing Then
        '    dtgRestrictions.DataSource = datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE)
        'End If
        'dtgRestrictions.DataBind()
        FillDatas() '//TEST
    End Sub


    Public Function TarifaMinima(ByRef lst As Queue(Of Double)) As Double
        Dim txtAdultFare As TextBox
        Dim dFare As Double
        Dim dAdultMin As Double
        Dim dChildtMin As Double
        Dim dJuniorMin As Double
        Dim dAdultNRMin As Double
        Dim dChildNRMin As Double
        Dim dJuniorNRMin As Double

        lst = New Queue(Of Double)
        dAdultMin = 999999999
        dChildtMin = 999999999
        dJuniorMin = 999999999
        dAdultNRMin = 999999999
        dChildNRMin = 999999999
        dJuniorNRMin = 999999999

        For i As Integer = 0 To dgAdult.Items.Count - 1
            txtAdultFare = dgAdult.Items(i).Cells(1).FindControl("txtAdultFare")
            Double.TryParse(txtAdultFare.Text, dFare)
            If dFare < dAdultMin Then
                dAdultMin = dFare
            End If

            txtAdultFare = dgAdult.Items(i).Cells(1).FindControl("txtAdultFareNR")
            Double.TryParse(txtAdultFare.Text, dFare)
            If dFare < dAdultNRMin Then
                dAdultNRMin = dFare
            End If
        Next

        For i As Integer = 0 To dgChild.Items.Count - 1
            txtAdultFare = dgChild.Items(i).Cells(1).FindControl("txtChildrenFare")
            Double.TryParse(txtAdultFare.Text, dFare)
            If dFare < dChildtMin Then
                dChildtMin = dFare
            End If
            txtAdultFare = dgChild.Items(i).Cells(1).FindControl("txtChildrenFareNR")
            Double.TryParse(txtAdultFare.Text, dFare)
            If dFare < dChildNRMin Then
                dChildNRMin = dFare
            End If
        Next

        For i As Integer = 0 To dgTeen.Items.Count - 1
            txtAdultFare = dgTeen.Items(i).Cells(1).FindControl("txtTeenFare")
            If Not txtAdultFare Is Nothing Then
                Double.TryParse(txtAdultFare.Text, dFare)
                If dFare < dJuniorMin Then
                    dJuniorMin = dFare
                End If
            End If

            txtAdultFare = dgTeen.Items(i).Cells(1).FindControl("txtTeenFareNR")
            If Not txtAdultFare Is Nothing Then
                Double.TryParse(txtAdultFare.Text, dFare)
                If dFare < dJuniorNRMin Then
                    dJuniorNRMin = dFare
                End If
            End If
        Next

        'dAdultMin = dAdultNRMin
        'dChildtMin = dChildNRMin
        'dJuniorMin = dJuniorNRMin

        lst.Enqueue(If(dAdultMin = 999999999, 0, dAdultMin))
        lst.Enqueue(If(dChildtMin = 999999999, 0, dChildtMin))
        lst.Enqueue(If(dJuniorMin = 999999999, 0, dJuniorMin))
        lst.Enqueue(If(dAdultNRMin = 999999999, 0, dAdultNRMin))
        lst.Enqueue(If(dChildNRMin = 999999999, 0, dChildNRMin))
        lst.Enqueue(If(dJuniorNRMin = 999999999, 0, dJuniorNRMin))

    End Function

    'Funcion actualizada al viernes 01 de abril del 2005
    Public Function GetFareRestrictions() As FaresRestrictionsData
        Dim datRestrictions As New FaresRestrictionsData
        Dim datDatagridRestrictions As New FaresRestrictionsData
        Dim datRooms As RoomsHotelData
        Dim datFare As FaresData
        Dim iRoomId As Integer
        Dim iPriceRoom As Decimal
        Dim iPriceExtraChild As Decimal

        Dim iPriceRoomNR As Decimal
        Dim iPriceExtraChildNR As Decimal

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
        ' get room id
        If Me.m_iRoomId <> 0 Then
            iRoomId = m_iRoomId
            If Not datFare Is Nothing AndAlso datFare.Tables(FaresData.FARES_TABLE).Rows.Count > 0 Then
                iPriceRoom = CInt(Val(datFare.Tables(FaresData.FARES_TABLE).Rows(0)(FaresData.PRICE_FIELD).ToString))
                iPriceExtraChild = CInt(Val(datFare.Tables(FaresData.FARES_TABLE).Rows(0)(FaresData.NINIORATE).ToString))
                iPriceRoomNR = CInt(Val(datFare.Tables(FaresData.FARES_TABLE).Rows(0)(FaresData.PRICENR_FIELD).ToString))
                iPriceExtraChildNR = CInt(Val(datFare.Tables(FaresData.FARES_TABLE).Rows(0)(FaresData.NINIORATENR).ToString))
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
                        .Item(FaresRestrictionsData.ADULTFARENR_FIELD) = iPriceRoomNR
                        .Item(FaresRestrictionsData.ADULTNUMBER_FIELD) = idxAdults
                        .Item(FaresRestrictionsData.CHILDFARE_FIELD) = iPriceExtraChild '* idxChild
                        .Item(FaresRestrictionsData.CHILDFARENR_FIELD) = iPriceExtraChildNR '* idxChild
                        .Item(FaresRestrictionsData.CHILDNUMBER_FIELD) = idxChild
                        .Item(FaresRestrictionsData.IDFARE_FIELD) = Me.m_iFareId
                        .Item(FaresRestrictionsData.PKIDRESTRICTION_FIELD) = 0
                        .Item(FaresRestrictionsData.EXCADULTFARE_FIELD) = 0 'iPriceRoomExc
                        .Item(FaresRestrictionsData.EXCNINIOFARE_FIELD) = 0 'iPriceExtraChildExc * idxChild
                        .Item(FaresRestrictionsData.EXCADULTFARENR_FIELD) = 0 'iPriceRoomExc
                        .Item(FaresRestrictionsData.EXCNINIOFARENR_FIELD) = 0 'iPriceExtraChildExc * idxChild
                        .Item(FaresRestrictionsData.TEENFARE_FIELD) = 0
                        .Item(FaresRestrictionsData.TEENFARENR_FIELD) = 0
                        .Item(FaresRestrictionsData.EXTEENFARE_FIELD) = 0
                        .Item(FaresRestrictionsData.EXTEENFARENR_FIELD) = 0
                    End With
                    datDatagridRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows.Add(newRow)
                End If
            Next
        Next
        datDatagridRestrictions.AcceptChanges()
        Return datDatagridRestrictions
    End Function

    Protected ReadOnly Property IsSupervisor() As Boolean
        Get
            Return CType(Me.Page, PaginaBase).IsSupervisor
        End Get
    End Property

    Public Function IsValidData() As Boolean

        IsValidData = True

        Dim txtNetRate As TextBox
        Dim txtRate As TextBox

        Dim netRate As Double = 0
        Dim rate As Double = 0
        Dim minPercent As Integer = 0
        Dim maxPercent As Integer = 0
        Dim isSupervisor As Boolean = CType(Me.Page, PaginaBase).IsSupervisor

        Integer.TryParse(Me.varMinPercent.Value, minPercent)
        Integer.TryParse(Me.varMaxPercent.Value, maxPercent)

        For Each item As DataGridItem In dgAdult.Items
            txtNetRate = item.Cells(2).FindControl("txtAdultFareNR")
            txtRate = item.Cells(1).FindControl("txtAdultFare")

            ' Double.TryParse(txtRate.Text, rate)
            'Double.TryParse(txtNetRate.Text, netRate)
            Double.TryParse(CType(item.Cells(1).FindControl("varRate"), HtmlInputHidden).Value, rate)
            Double.TryParse(txtNetRate.Text, netRate)

            '  If netRate > 0 AndAlso (Not isSupervisor OrElse rate > 0) Then
            If Not isSupervisor AndAlso (netRate > 0) Then

                'If Not isSupervisor AndAlso rate = 0 Then
                If Not isSupervisor AndAlso (rate = 0 OrElse netRate * (1 + (minPercent / 100)) > rate OrElse netRate * (1 + (maxPercent / 100)) < rate OrElse netRate = 0) Then
                    rate = (netRate * (1 + (maxPercent / 100)))
                    txtRate.Text = rate.ToString()
                ElseIf Not isSupervisor Then
                    txtRate.Text = rate
                End If
            Else
                IsValidData = False
                If isSupervisor Then
                    Double.TryParse(txtRate.Text, rate)
                    If rate > 0 AndAlso netRate > 0 Then
                        IsValidData = True
                    End If
                End If

            End If

            'IsValidData = (IsValidData AndAlso (isSupervisor OrElse ((netRate * (1 + (minPercent / 100))) <= rate)))
            '   IsValidData = IsValidData AndAlso (isSupervisor OrElse (netRate * (1 + (minPercent / 100)) < rate OrElse netRate * (1 + (maxPercent / 100)) > rate))

            If Not IsValidData Then Exit Function

        Next


        If Not isSupervisor Then
            For Each item As DataGridItem In Me.dgChild.Items
                txtNetRate = item.Cells(2).FindControl("txtChildrenFareNR")
                txtRate = item.Cells(1).FindControl("txtChildrenFare")

                'Double.TryParse(txtRate.Text, rate)
                Double.TryParse(CType(item.Cells(1).FindControl("varRate"), HtmlInputHidden).Value, rate)
                Double.TryParse(txtNetRate.Text, netRate)

                'If netRate > 0 AndAlso rate = 0 Then
                If rate = 0 OrElse netRate * (1 + (minPercent / 100)) > rate OrElse netRate * (1 + (maxPercent / 100)) < rate OrElse netRate = 0 Then
                    rate = (netRate * (1 + (maxPercent / 100)))
                    txtRate.Text = rate.ToString()
                Else
                    txtRate.Text = rate
                End If

                ' IsValidData = ((netRate * (1 + (minPercent / 100))) <= rate)
                ' IsValidData = netRate * (1 + (minPercent / 100)) < rate OrElse netRate * (1 + (maxPercent / 100)) > rate

                ' If Not IsValidData Then Exit Function

            Next

            For Each item As DataGridItem In Me.dgTeen.Items
                txtNetRate = item.Cells(2).FindControl("txtTeenFareNR")
                txtRate = item.Cells(1).FindControl("txtTeenFare")

                'Double.TryParse(txtRate.Text, rate)
                Double.TryParse(CType(item.Cells(1).FindControl("varRate"), HtmlInputHidden).Value, rate)
                Double.TryParse(txtNetRate.Text, netRate)

                'If netRate > 0 AndAlso rate = 0 Then
                If rate = 0 OrElse netRate * (1 + (minPercent / 100)) > rate OrElse netRate * (1 + (maxPercent / 100)) < rate OrElse netRate = 0 Then
                    rate = (netRate * (1 + (maxPercent / 100)))
                    txtRate.Text = rate.ToString()
                Else
                    txtRate.Text = rate
                End If

                'IsValidData = ((netRate * (1 + (minPercent / 100))) <= rate)
                ' IsValidData = netRate * (1 + (minPercent / 100)) < rate OrElse netRate * (1 + (maxPercent / 100)) > rate

                ' If Not IsValidData Then Exit Function

            Next

        End If

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

    Private Sub BindDataGridColumns()
        '     With Me.dtgRestrictions
        '         CType(.Columns(Me.RestrictionsDtgCols.RestrictionId), BoundColumn).DataField = _
        'FaresRestrictionsData.PKIDRESTRICTION_FIELD
        '     End With
    End Sub

    'Public Function Save(Optional ByVal DT As DataTable = Nothing) As Boolean
    '    Dim validate As Boolean = True
    '    Dim datRestrictions As FaresRestrictionsData = GetFareRestrictions()
    '    Dim row As DataRow
    '    Dim i As Integer

    '    ' Check if the input controls are valid 
    '    If Not Page.IsValid Then
    '        Return False
    '    End If

    '    For i = 0 To datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows.Count - 1
    '        Dim chkActivo As CheckBox
    '        Dim txtAdultFare As TextBox
    '        Dim txtChildFare As TextBox
    '        Dim cvAd As System.Web.UI.WebControls.CustomValidator
    '        Dim cvCh As System.Web.UI.WebControls.CustomValidator

    '        chkActivo = dtgRestrictions.Items(i).Cells(RestrictionsDtgCols.Tools).FindControl("chkActive")
    '        txtAdultFare = dtgRestrictions.Items(i).Cells(RestrictionsDtgCols.AdultFare).FindControl("txtAdultFare")
    '        cvAd = dtgRestrictions.Items(i).Cells(RestrictionsDtgCols.AdultFare).FindControl("cvErrAdults")
    '        txtChildFare = dtgRestrictions.Items(i).Cells(RestrictionsDtgCols.AdultFare).FindControl("txtChildrenFare")
    '        cvCh = dtgRestrictions.Items(i).Cells(RestrictionsDtgCols.AdultFare).FindControl("cvErrChilds")


    '        With datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE)
    '            If txtAdultFare.Text.Trim.Length = 0 Then
    '                txtChildFare.Text = 0
    '            End If
    '            'If chkActivo.Checked = True Then				' si está habilitada la restricción
    '            If True = True Then    'siempre guardamos
    '                If CType(txtAdultFare.Text, Integer) = 0 And CType(txtChildFare.Text, Integer) = 0 Then
    '                    cvAd.IsValid = False
    '                    cvCh.IsValid = False
    '                    'validate = False
    '                    ' Si los dos valores son cero no guardamos el registro
    '                End If

    '                ' si se ha modificado el registro entonces lo marcamos como modified
    '                If .Rows(i)(FaresRestrictionsData.ADULTFARE_FIELD) <> Double.Parse(txtAdultFare.Text) Then
    '                    .Rows(i)(FaresRestrictionsData.ADULTFARE_FIELD) = Double.Parse(txtAdultFare.Text)
    '                End If
    '                ' si se ha modificado el registro entonces lo marcamos como modified
    '                If txtChildFare.Text.Trim.Length = 0 Then
    '                    txtChildFare.Text = 0
    '                End If
    '                If .Rows(i)(FaresRestrictionsData.CHILDFARE_FIELD) <> Double.Parse(txtChildFare.Text) Then
    '                    .Rows(i)(FaresRestrictionsData.CHILDFARE_FIELD) = Double.Parse(txtChildFare.Text)
    '                End If
    '                If dtgRestrictions.Items(i).Cells(RestrictionsDtgCols.RestrictionId).Text = 0 Then
    '                    .Rows(i)(FaresRestrictionsData.CHILDFARE_FIELD) = Double.Parse(txtChildFare.Text)
    '                    .Rows(i)(FaresRestrictionsData.ADULTFARE_FIELD) = Double.Parse(txtAdultFare.Text)
    '                End If
    '                If Not DT Is Nothing Then
    '                    .Rows(i)(FaresRestrictionsData.EXCADULTFARE_FIELD) = DT.Rows(i)(FaresRestrictionsData.EXCADULTFARE_FIELD)
    '                    .Rows(i)(FaresRestrictionsData.EXCNINIOFARE_FIELD) = DT.Rows(i)(FaresRestrictionsData.EXCNINIOFARE_FIELD)
    '                Else
    '                    .Rows(i)(FaresRestrictionsData.EXCADULTFARE_FIELD) = 0
    '                    .Rows(i)(FaresRestrictionsData.EXCNINIOFARE_FIELD) = 0

    '                End If


    '            Else    ' si no está habilitado el registro pero si existía antes
    '                If Me.dtgRestrictions.Items(i).Cells(RestrictionsDtgCols.RestrictionId).Text <> 0 Then       'Es delete
    '                    .Rows(i).Delete()
    '                End If
    '            End If

    '        End With
    '    Next

    '    If validate Then
    '        With New FaresRestrictionSystem
    '            Save = .UpdateFaresRestrictions(datRestrictions)
    '        End With
    '    Else
    '        Me.cvErrMsg.IsValid = False
    '        Save = validate
    '    End If
    'End Function

    Sub Addrateplan(ByVal ds As DataSet, ByVal field As String, ByVal valor As String, ByVal iscreate As Boolean)
        If Not ds Is Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
            If iscreate Then ds.Tables(0).Columns.Add(field)
            ds.Tables(0).Rows(0)(field) = valor
        End If
    End Sub

    Function getDataXML(ByVal ds As DataSet, ByVal descrPlan As String, ByVal iscreate As Boolean) As String
        Addrateplan(ds, "Descr_rateplan", descrPlan, iscreate)
        Return Util.Utility.GetXml(FaresRestrictionsData.FARESRESTRICTION_TABLE, "UpdatePlanFaresNR", ds)
    End Function


    Public Function CreateRPDHtml(ByVal dsBefore As FaresRestrictionsData, ByVal dsFares As FaresRestrictionsData, ByRef hr As Boolean) As String
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
        menu.Width = New System.Web.UI.WebControls.Unit(600, UnitType.Pixel)

        tr = New TableRow
        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("00060", idioma)   ' "Adults"
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("00278", idioma)   '"Rate Adult "
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("00279", idioma)   ' "Rate Child" 
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("01182", idioma)   '"Rate Adult Update "   
        tr.Cells.Add(td)

        td = New TableHeaderCell
        td.Attributes.Add("class", "divHeaderL")
        td.Text = PortalCulture.GetString("01183", idioma)   '"Rate Child Update"  
        tr.Cells.Add(td)

        menu.Rows.Add(tr)
        Dim drr As DataRow
        Dim drd As DataRow
        For i As Integer = 0 To dsBefore.Tables(0).Rows.Count - 1
            If (i < dsBefore.Tables(0).Rows.Count) And (i < dsFares.Tables(0).Rows.Count) Then
                drr = dsBefore.Tables(0).Rows(i)
                drd = dsFares.Tables(0).Rows(i)
                'If drr("TarifaAdulto") <> drd("TarifaAdulto") Or drr("TarifaNinio") <> drd("TarifaNinio") Then
                tr = New TableRow
                td = New TableHeaderCell
                td.Attributes.Add("class", "dow")
                td.Text = drr("Adultos")
                tr.Cells.Add(td)

                td = New TableHeaderCell
                td.Attributes.Add("class", "dow")
                td.Text = Format(drr("TarifaAdulto"), "########0.00")
                tr.Cells.Add(td)

                td = New TableHeaderCell
                td.Attributes.Add("class", "dow")
                td.Text = Format(drr("TarifaNinio"), "########0.00")
                tr.Cells.Add(td)

                td = New TableHeaderCell
                td.Attributes.Add("class", "dow")
                td.Text = Format(drd("TarifaAdulto"), "########0.00")
                tr.Cells.Add(td)

                td = New TableHeaderCell
                td.Attributes.Add("class", "dow")
                td.Text = Format(drd("TarifaNinio"), "########0.00")
                tr.Cells.Add(td)

                menu.Rows.Add(tr)
                hr = True
                'end If
            End If
        Next

        menu.RenderControl(writer)
        shtml = sw.ToString()

        Return shtml
    End Function

    ''''''''''''''' para guardar los datos '''''con los nuevosgrid
    Public Function Save(ByVal rp As String, ByVal sroom As String, ByVal scorreo As String, Optional ByVal dt As DataTable = Nothing) As Boolean
        Dim validate As Boolean = True
        Dim datRestrictions As FaresRestrictionsData = GetFareRestrictions()
        Dim datPrev As FaresRestrictionsData = GetFareRestrictions()
        'Dim row As DataRow
        Dim i As Integer
        Dim Ad As Integer
        Dim Ch As Integer
        Dim te As Integer
        Dim dr As DataRow
        Dim sData As String = ""
        Dim sDataPrev As String = ""
        Dim sdatoCorreo As String = ""

        ' Check if the input controls are valid 
        If Not Page.IsValid Then
            Return False
        End If


        Addrateplan(datRestrictions, "Descr_rateplan", String.Format("{0}/ {1}", rp, sroom), True)

        For i = 0 To datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows.Count - 1
            Dim chkActivo As CheckBox
            Dim chkActivo2 As CheckBox
            Dim chkActivo3 As CheckBox
            Dim txtAdultFare As TextBox
            Dim txtChildFare As TextBox
            Dim txtTeenFare As TextBox
            Dim txtAdultFareNR As TextBox
            Dim txtChildFareNR As TextBox
            Dim txtTeenFareNR As TextBox

            Dim cvAdNR As System.Web.UI.WebControls.CustomValidator
            Dim cvChNR As System.Web.UI.WebControls.CustomValidator
            Dim cvTeNR As System.Web.UI.WebControls.CustomValidator
            Dim cvAd As System.Web.UI.WebControls.CustomValidator
            Dim cvCh As System.Web.UI.WebControls.CustomValidator
            Dim cvTe As System.Web.UI.WebControls.CustomValidator

            dr = datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE).Rows(i)

            Ch = dr(FaresRestrictionsData.CHILDNUMBER_FIELD)
            Ad = dr(FaresRestrictionsData.ADULTNUMBER_FIELD)


            chkActivo = dgAdult.Items(Ad - 1).Cells(2).FindControl("chkActive")

            txtAdultFareNR = dgAdult.Items(Ad - 1).Cells(1).FindControl("txtAdultFareNR")
            cvAdNR = dgAdult.Items(Ad - 1).Cells(1).FindControl("cvErrAdultsNR")

            txtAdultFare = dgAdult.Items(Ad - 1).Cells(2).FindControl("txtAdultFare")
            cvAd = dgAdult.Items(Ad - 1).Cells(3).FindControl("cvErrAdults")


            txtChildFare = Nothing
            cvCh = Nothing
            txtChildFareNR = Nothing
            cvChNR = Nothing

            txtTeenFare = Nothing
            txtTeenFareNR = Nothing
            cvTe = Nothing
            cvTeNR = Nothing

            chkActivo2 = Nothing

            If Ch > 0 Then ''''''''''''para los ninios''''''''''''''''''''''''

                txtChildFareNR = dgChild.Items(Ch - 1).Cells(1).FindControl("txtChildrenFareNR")
                cvChNR = dgChild.Items(Ch - 1).Cells(1).FindControl("cvErrChildsNR")

                txtChildFare = dgChild.Items(Ch - 1).Cells(2).FindControl("txtChildrenFare")
                cvCh = dgChild.Items(Ch - 1).Cells(2).FindControl("cvErrChilds")

                chkActivo2 = dgChild.Items(Ch - 1).Cells(3).FindControl("chkActive")

            End If

            If Ch > 0 Then
                If dgTeen.Items.Count > 0 Then
                    txtTeenFare = dgTeen.Items(Ch - 1).Cells(2).FindControl("txtTeenFare")
                    cvTe = dgTeen.Items(Ch - 1).Cells(3).FindControl("cvErrTeens")

                    txtTeenFareNR = dgTeen.Items(Ch - 1).Cells(2).FindControl("txtTeenFareNR")
                    cvTeNR = dgTeen.Items(Ch - 1).Cells(3).FindControl("cvErrTeensNr")
                End If
                'chkActivo3 = dgChild.Items(Ch - 1).Cells(3).FindControl("chkActive")
            End If

            With datRestrictions.Tables(FaresRestrictionsData.FARESRESTRICTION_TABLE)

                If txtAdultFareNR.Text.Trim.Length = 0 Then
                    txtChildFareNR.Text = 0
                End If
                If txtAdultFare.Text.Trim.Length = 0 Then
                    txtChildFare.Text = 0
                End If

                'If chkActivo.Checked = True Then				' si está habilitada la restricción
                ' If True = True Then    'siempre guardamos

                If CType(txtAdultFareNR.Text, Integer) = 0 AndAlso Not txtChildFareNR Is Nothing AndAlso CType(txtChildFareNR.Text, Integer) = 0 Then
                    cvAdNR.IsValid = False
                    cvChNR.IsValid = False
                    'validate = False
                    ' Si los dos valores son cero no guardamos el registro
                End If

                If CType(txtAdultFare.Text, Integer) = 0 AndAlso Not txtChildFare Is Nothing AndAlso CType(txtChildFare.Text, Integer) = 0 Then
                    cvAd.IsValid = False
                    cvCh.IsValid = False
                    'validate = False
                    ' Si los dos valores son cero no guardamos el registro
                End If

                ' si se ha modificado el registro entonces lo marcamos como modified
                If IIf(.Rows(i)(FaresRestrictionsData.ADULTFARENR_FIELD) Is DBNull.Value, 0, .Rows(i)(FaresRestrictionsData.ADULTFARENR_FIELD)) <> Double.Parse(txtAdultFareNR.Text) Then
                    .Rows(i)(FaresRestrictionsData.ADULTFARENR_FIELD) = Double.Parse(txtAdultFareNR.Text)
                End If

                If .Rows(i)(FaresRestrictionsData.ADULTFARE_FIELD) <> Double.Parse(txtAdultFare.Text) Then
                    .Rows(i)(FaresRestrictionsData.ADULTFARE_FIELD) = Double.Parse(txtAdultFare.Text)
                End If

                ' si se ha modificado el registro entonces lo marcamos como modified
                If Not txtChildFareNR Is Nothing Then
                    If txtChildFareNR.Text.Trim.Length = 0 Then
                        txtChildFareNR.Text = 0
                    End If
                End If

                If Not txtChildFare Is Nothing Then
                    If txtChildFare.Text.Trim.Length = 0 Then
                        txtChildFare.Text = 0
                    End If
                End If

                If Not txtChildFareNR Is Nothing Then
                    If IIf(.Rows(i)(FaresRestrictionsData.CHILDFARENR_FIELD) Is DBNull.Value, 0, .Rows(i)(FaresRestrictionsData.CHILDFARENR_FIELD)) <> Double.Parse(txtChildFareNR.Text) Then
                        .Rows(i)(FaresRestrictionsData.CHILDFARENR_FIELD) = Double.Parse(txtChildFareNR.Text)
                    End If
                End If

                If Not txtChildFare Is Nothing Then
                    If .Rows(i)(FaresRestrictionsData.CHILDFARE_FIELD) <> Double.Parse(txtChildFare.Text) Then
                        .Rows(i)(FaresRestrictionsData.CHILDFARE_FIELD) = Double.Parse(txtChildFare.Text)
                    End If
                End If

                ''''''''''''''''''''' ADOLESCENTES '''''''''''''''''''''

                ' si se ha modificado el registro entonces lo marcamos como modified
                If Not txtTeenFareNR Is Nothing Then
                    If String.IsNullOrEmpty(txtTeenFareNR.Text) Then
                        txtTeenFareNR.Text = 0
                    End If
                End If

                If Not txtTeenFare Is Nothing Then
                    If String.IsNullOrEmpty(txtTeenFare.Text) Then
                        txtTeenFare.Text = 0
                    End If
                End If

                If Not txtTeenFareNR Is Nothing Then
                    If IIf(.Rows(i)(FaresRestrictionsData.TEENFARENR_FIELD) Is DBNull.Value, 0, .Rows(i)(FaresRestrictionsData.TEENFARENR_FIELD)) <> Double.Parse(txtTeenFareNR.Text) Then
                        .Rows(i)(FaresRestrictionsData.TEENFARENR_FIELD) = Double.Parse(txtTeenFareNR.Text)
                    End If
                End If

                If Not txtTeenFare Is Nothing Then
                    If .Rows(i)(FaresRestrictionsData.TEENFARE_FIELD) <> Double.Parse(txtTeenFare.Text) Then
                        .Rows(i)(FaresRestrictionsData.TEENFARE_FIELD) = Double.Parse(txtTeenFare.Text)
                    End If
                End If

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''

                'If dgAdult.Items(Ad - 1).Cells(RestrictionsDtgCols.RestrictionId).Text = 0 Then

                If Not txtChildFareNR Is Nothing Then
                    .Rows(i)(FaresRestrictionsData.CHILDFARENR_FIELD) = Double.Parse(txtChildFareNR.Text)
                Else
                    .Rows(i)(FaresRestrictionsData.CHILDFARENR_FIELD) = 0
                End If
                .Rows(i)(FaresRestrictionsData.ADULTFARENR_FIELD) = Double.Parse(txtAdultFareNR.Text)

                If Not txtChildFare Is Nothing Then
                    .Rows(i)(FaresRestrictionsData.CHILDFARE_FIELD) = Double.Parse(txtChildFare.Text)
                Else
                    .Rows(i)(FaresRestrictionsData.CHILDFARE_FIELD) = 0
                End If
                .Rows(i)(FaresRestrictionsData.ADULTFARE_FIELD) = Double.Parse(txtAdultFare.Text)

                If Not txtTeenFareNR Is Nothing Then
                    .Rows(i)(FaresRestrictionsData.TEENFARENR_FIELD) = Double.Parse(txtTeenFareNR.Text)
                Else
                    .Rows(i)(FaresRestrictionsData.TEENFARENR_FIELD) = 0
                End If
                If Not txtTeenFare Is Nothing Then
                    .Rows(i)(FaresRestrictionsData.TEENFARE_FIELD) = Double.Parse(txtTeenFare.Text)
                Else
                    .Rows(i)(FaresRestrictionsData.TEENFARE_FIELD) = 0
                End If


                'End If
                If Not dt Is Nothing Then
                    .Rows(i)(FaresRestrictionsData.EXCADULTFARE_FIELD) = dt.Rows(i)(FaresRestrictionsData.EXCADULTFARE_FIELD)
                    .Rows(i)(FaresRestrictionsData.EXCNINIOFARE_FIELD) = dt.Rows(i)(FaresRestrictionsData.EXCNINIOFARE_FIELD)
                    .Rows(i)(FaresRestrictionsData.EXTEENFARE_FIELD) = dt.Rows(i)(FaresRestrictionsData.EXTEENFARE_FIELD)

                    .Rows(i)(FaresRestrictionsData.EXCADULTFARENR_FIELD) = dt.Rows(i)(FaresRestrictionsData.EXCADULTFARENR_FIELD)
                    .Rows(i)(FaresRestrictionsData.EXCNINIOFARENR_FIELD) = dt.Rows(i)(FaresRestrictionsData.EXCNINIOFARENR_FIELD)
                    .Rows(i)(FaresRestrictionsData.EXTEENFARENR_FIELD) = dt.Rows(i)(FaresRestrictionsData.EXTEENFARENR_FIELD)
                Else
                    .Rows(i)(FaresRestrictionsData.EXCADULTFARE_FIELD) = 0
                    .Rows(i)(FaresRestrictionsData.EXCNINIOFARE_FIELD) = 0
                    .Rows(i)(FaresRestrictionsData.EXTEENFARE_FIELD) = 0

                    .Rows(i)(FaresRestrictionsData.EXCADULTFARENR_FIELD) = 0
                    .Rows(i)(FaresRestrictionsData.EXCNINIOFARENR_FIELD) = 0
                    .Rows(i)(FaresRestrictionsData.EXTEENFARENR_FIELD) = 0
                End If
                'Else    ' si no está habilitado el registro pero si existía antes
                '    If Me.dtgRestrictions.Items(i).Cells(RestrictionsDtgCols.RestrictionId).Text <> 0 Then       'Es delete
                '        .Rows(i).Delete()
                '    End If
                'End If
            End With
        Next

        If validate Then
            Dim hr As Boolean
            With New FaresRestrictionSystem
                Save = .UpdateFaresRestrictions(datRestrictions)
                If Save Then
                    sData = getDataXML(datRestrictions, String.Format("{0}/ {1}", rp, sroom), False)
                    sDataPrev = getDataXML(datPrev, String.Format("{0}/ {1}", rp, sroom), True)
                    'sdatoCorreo = CreateRPDHtml(datPrev, datRestrictions, hr)
                    'If hr Then
                    '    sdatoCorreo += "<br/><br/>"
                    'Else
                    '    sdatoCorreo = ""
                    'End If
                    sdatoCorreo = (New Util.Utility).GeneraCorreoXslt(sDataPrev, sData)
                    scorreo += sdatoCorreo
                    CType(Me.Page, PaginaBase).guardalog("/Pages/FaresCatalogueNR.aspx", PaginaBase.acciones.Modificar, _
                        "Se modificó la tarifa de la habitación NR con el rateplan " & FaresRestrictionsData.IDFARE_FIELD, "Cambio de Tarifa Neta " & CType(Me.Page, PaginaBase).cInfoActual.HotelName, sDataPrev, sData, scorreo)
                End If
            End With
        Else
            Me.cvErrMsg.IsValid = False
            Save = validate
        End If
    End Function

    Public Sub ReFill()
        FillDataGrid()
    End Sub

    'Private Sub dtgRestrictions_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgRestrictions.ItemCommand
    '    Dim iRestrictionId As Integer
    '    Dim webControl As Control


    '    Try
    '        iRestrictionId = Integer.Parse(e.Item.Cells(Me.RestrictionsDtgCols.RestrictionId).Text)
    '    Catch ex As Exception
    '        Return
    '    End Try

    'End Sub

    Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
        For Each item As DataGridItem In Me.dgAdult.Items
            Dim txtAdultFare As TextBox
            Dim txtChildFare As TextBox
            If item.ItemType = ListItemType.AlternatingItem Or item.ItemType = ListItemType.EditItem Or item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.SelectedItem Then
                txtAdultFare = item.Cells(RestrictionsDtgCols.AdultFare).FindControl("txtAdultFare")
                txtChildFare = item.Cells(RestrictionsDtgCols.AdultFare).FindControl("txtChildrenFare")
                'Dim lblTotalFare As Label
                'lblTotalFare = item.Cells(RestrictionsDtgCols.TotalFare).FindControl("lblTotal")
                'If Not lblTotalFare Is Nothing Then
                'lblTotalFare.Text = CDec(CDec(txtAdultFare.Text) + CDec(txtChildFare.Text)).ToString("C")
                'End If
            End If
            If item.ItemType = ListItemType.Header Then
                item.Cells(0).Text = "AA"
            End If
        Next
    End Sub

    'Private Sub dtgRestrictions_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgRestrictions.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Header Then
    '        e.Item.Cells(RestrictionsDtgCols.AdultNumber).Text = PortalCulture.GetString("00060")
    '        e.Item.Cells(RestrictionsDtgCols.ChildNumber).Text = PortalCulture.GetString("00061")
    '        e.Item.Cells(RestrictionsDtgCols.AdultFare).Text = PortalCulture.GetString("00134")
    '        e.Item.Cells(RestrictionsDtgCols.ChildFare).Text = PortalCulture.GetString("00135")
    '        e.Item.Cells(RestrictionsDtgCols.TotalFare).Text = PortalCulture.GetString("00136")
    '    End If

    '    Dim txtAdultFare As TextBox
    '    Dim txtChildFare As TextBox
    '    Dim lblAdult As Label
    '    Dim lblChild As Label
    '    Dim valAdt As RegularExpressionValidator
    '    Dim valChd As RegularExpressionValidator

    '    txtAdultFare = e.Item.Cells(RestrictionsDtgCols.AdultFare).FindControl("txtAdultFare")
    '    txtChildFare = e.Item.Cells(RestrictionsDtgCols.AdultFare).FindControl("txtChildrenFare")

    '    lblAdult = e.Item.Cells(RestrictionsDtgCols.AdultNumber).FindControl("lblAdults")
    '    lblChild = e.Item.Cells(RestrictionsDtgCols.ChildNumber).FindControl("lblChildren")



    '    valAdt = e.Item.Cells(RestrictionsDtgCols.AdultFare).FindControl("valAdultExtraPrice")
    '    valChd = e.Item.Cells(RestrictionsDtgCols.ChildFare).FindControl("valChildrenExtraPrice")

    '    If Not valAdt Is Nothing Then valAdt.Text = PortalCulture.GetString("00240")
    '    If Not valChd Is Nothing Then valChd.Text = PortalCulture.GetString("00240")

    '    If Not txtAdultFare Is Nothing AndAlso lblAdult.Text <= 0 Then
    '        txtAdultFare.Text = 0
    '        txtAdultFare.Enabled = False
    '        txtAdultFare.Visible = False

    '    End If
    '    If Not txtChildFare Is Nothing AndAlso lblChild.Text <= 0 Then
    '        txtChildFare.Text = 0
    '        txtChildFare.Enabled = False
    '        txtChildFare.Visible = False
    '        If lblChild.Text <= 0 Then
    '            lblChild.Text = ""
    '        End If
    '    End If

    '    'Mostramos la tarifa total 
    '    If Not txtAdultFare Is Nothing And Not txtChildFare Is Nothing Then
    '        Dim lblTotalFare As Label
    '        lblTotalFare = e.Item.Cells(RestrictionsDtgCols.TotalFare).FindControl("lblTotal")
    '        If Not lblTotalFare Is Nothing Then
    '            txtAdultFare.Attributes.Add("onChange", "sP('" & txtAdultFare.ClientID & "','" & txtChildFare.ClientID & "','" & lblTotalFare.ClientID & "');")
    '            txtChildFare.Attributes.Add("onChange", "sP('" & txtAdultFare.ClientID & "','" & txtChildFare.ClientID & "','" & lblTotalFare.ClientID & "');")
    '            lblTotalFare.Text = CDec(CDec(txtAdultFare.Text) + CDec(txtChildFare.Text)).ToString("C")
    '        End If
    '    End If

    '    Dim chkActive As CheckBox
    '    chkActive = e.Item.Cells(RestrictionsDtgCols.Tools).FindControl("chkActive")
    '    Dim dgi As DataGridItem
    '    If Not chkActive Is Nothing Then
    '        dgi = CType(chkActive.Parent.Parent, DataGridItem)
    '        If e.Item.Cells(RestrictionsDtgCols.RestrictionId).Text = "0" Then
    '            chkActive.Checked = False
    '            If dgi.ItemType = ListItemType.AlternatingItem Then
    '                dgi.CssClass = "dgAlternate"
    '                dgi.Attributes.Add("itemType", "AlternatingItem")
    '            Else
    '                dgi.CssClass = "Dgitem"
    '                dgi.Attributes.Add("itemType", "Item")
    '            End If
    '        Else
    '            chkActive.Checked = True
    '            dgi = CType(chkActive.Parent.Parent, DataGridItem)
    '            dgi.CssClass = "dgSelected"
    '        End If
    '    End If
    'End Sub

    Public Sub CargaRecursos()
        Me.cvErrMsg.Text = PortalCulture.GetString("00234")
        'Me.dtgRestrictions.Columns(6).HeaderText = PortalCulture.GetString("00239")
    End Sub

    Public Function getMaxFare() As Decimal
        getMaxFare = 0
        Dim txtAdultFare As TextBox
        If dgAdult.Items.Count > 0 Then
            txtAdultFare = dgAdult.Items(dgAdult.Items.Count - 1).Cells(1).FindControl("txtAdultFare")
            If Not txtAdultFare Is Nothing Then
                Try
                    getMaxFare = CDec(txtAdultFare.Text)
                Catch ex As Exception
                End Try
            End If
        End If
    End Function

#End Region

    Private Sub dgAdult_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgAdult.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(0).Text = PortalCulture.GetString("00060")
            e.Item.Cells(1).Text = PortalCulture.GetString("01125") 'Tarifa NR
            e.Item.Cells(2).Text = PortalCulture.GetString("01124") 'Tarifa UV
        End If
        Dim lblAdult As Label
        lblAdult = e.Item.Cells(0).FindControl("lblAdults")

        Dim lblAdultValMax As Label
        Dim lblAdultValMin As Label
        lblAdultValMax = e.Item.Cells(5).FindControl("lblAdultValMax")
        lblAdultValMin = e.Item.Cells(5).FindControl("lblAdultValMin")

        If Not lblAdultValMax Is Nothing Then
            lblAdultValMax.Style.Add("display", "none")
            lblAdultValMax.Text = PortalCulture.GetString("01135")
        End If
        If Not lblAdultValMin Is Nothing Then
            lblAdultValMin.Style.Add("display", "none")
            lblAdultValMin.Text = PortalCulture.GetString("01136")
        End If


        Dim txtAdultFareNR As TextBox
        'Dim lblAdultNR As Label
        Dim valAdtNR As RegularExpressionValidator
        Dim varRate As HtmlInputHidden
        txtAdultFareNR = e.Item.Cells(1).FindControl("txtAdultFareNR")
        valAdtNR = e.Item.Cells(1).FindControl("valAdultExtraPriceNR")
        If Not valAdtNR Is Nothing Then valAdtNR.Text = PortalCulture.GetString("00240")
        If Not txtAdultFareNR Is Nothing AndAlso lblAdult.Text <= 0 Then
            txtAdultFareNR.Text = 0
            txtAdultFareNR.Enabled = False
            txtAdultFareNR.Visible = False
        End If

        Dim txtAdultFare As TextBox
        Dim valAdt As RegularExpressionValidator
        txtAdultFare = e.Item.Cells(2).FindControl("txtAdultFare")
        valAdt = e.Item.Cells(2).FindControl("valAdultExtraPrice")
        If Not valAdt Is Nothing Then valAdt.Text = PortalCulture.GetString("00240")
        If Not txtAdultFare Is Nothing AndAlso lblAdult.Text <= 0 Then
            txtAdultFare.Text = 0
            txtAdultFare.Enabled = False
            txtAdultFare.Visible = False
        End If

        If Not txtAdultFareNR Is Nothing AndAlso Not txtAdultFare Is Nothing Then
            txtAdultFareNR.Attributes.Add("onChange", "javascript:CheckValContract('" & m_TextBoxPorcMin & "','" & m_TextBoxPorcMax & "','" & txtAdultFareNR.ClientID & "','" & txtAdultFare.ClientID & "','" & lblAdultValMax.ClientID & "','" & lblAdultValMin.ClientID & "')")
        End If

        Dim chkActive As CheckBox
        chkActive = e.Item.Cells(4).FindControl("chkActive")
        Dim dgi As DataGridItem
        If Not chkActive Is Nothing Then
            dgi = CType(chkActive.Parent.Parent, DataGridItem)
            If e.Item.Cells(3).Text = "0" Then
                chkActive.Checked = False
                If dgi.ItemType = ListItemType.AlternatingItem Then
                    dgi.CssClass = "dgAlternate"
                    dgi.Attributes.Add("itemType", "AlternatingItem")
                Else
                    dgi.CssClass = "Dgitem"
                    dgi.Attributes.Add("itemType", "Item")
                End If
            Else
                chkActive.Checked = True
                dgi = CType(chkActive.Parent.Parent, DataGridItem)
                dgi.CssClass = "dgSelected"
            End If
        End If
    End Sub

    Private Sub dgChild_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgChild.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(0).Text = PortalCulture.GetString("00061")
            e.Item.Cells(1).Text = PortalCulture.GetString("01125") 'Tarifa Neta
            e.Item.Cells(2).Text = PortalCulture.GetString("01124") 'Tarifa Neta
        End If

        Dim lblChild As Label
        lblChild = e.Item.Cells(0).FindControl("lblChildren")

        Dim lblChildValMax As Label
        Dim lblChildValMin As Label
        lblChildValMax = e.Item.Cells(5).FindControl("lblChildValMax")
        lblChildValMin = e.Item.Cells(5).FindControl("lblChildValMin")
        If Not lblChildValMax Is Nothing Then
            lblChildValMax.Style.Add("display", "none")
            lblChildValMax.Text = PortalCulture.GetString("01135")
        End If
        If Not lblChildValMin Is Nothing Then
            lblChildValMin.Style.Add("display", "none")
            lblChildValMin.Text = PortalCulture.GetString("01136")
        End If

        Dim valChdNR As RegularExpressionValidator
        Dim txtChildFareNR As TextBox
        txtChildFareNR = e.Item.Cells(1).FindControl("txtChildrenFareNR")
        valChdNR = e.Item.Cells(1).FindControl("valChildrenExtraPriceNR")
        If Not valChdNR Is Nothing Then valChdNR.Text = PortalCulture.GetString("00240")
        If Not txtChildFareNR Is Nothing AndAlso lblChild.Text <= 0 Then
            txtChildFareNR.Text = 0
            txtChildFareNR.Enabled = False
            txtChildFareNR.Visible = False
            If lblChild.Text <= 0 Then
                lblChild.Text = ""
            End If
        End If

        Dim valChd As RegularExpressionValidator
        Dim txtChildFare As TextBox
        txtChildFare = e.Item.Cells(2).FindControl("txtChildrenFare")
        valChd = e.Item.Cells(2).FindControl("valChildrenExtraPrice")
        If Not valChd Is Nothing Then valChd.Text = PortalCulture.GetString("00240")
        If Not txtChildFare Is Nothing AndAlso lblChild.Text <= 0 Then
            txtChildFare.Text = 0
            txtChildFare.Enabled = False
            txtChildFare.Visible = False
            If lblChild.Text <= 0 Then
                lblChild.Text = ""
            End If
        End If

        'Validacion de maximo y minimo porcentaje de gananciaUV
        If Not txtChildFareNR Is Nothing AndAlso Not txtChildFare Is Nothing Then
            txtChildFareNR.Attributes.Add("onChange", "javascript:CheckValContract('" & m_TextBoxPorcMin & "','" & m_TextBoxPorcMax & "','" & txtChildFareNR.ClientID & "','" & txtChildFare.ClientID & "','" & lblChildValMax.ClientID & "','" & lblChildValMin.ClientID & "')")
        End If

        Dim chkActive As CheckBox
        chkActive = e.Item.Cells(4).FindControl("chkActive")
        Dim dgi As DataGridItem
        If Not chkActive Is Nothing Then
            dgi = CType(chkActive.Parent.Parent, DataGridItem)
            If e.Item.Cells(3).Text = "0" Then
                chkActive.Checked = False
                If dgi.ItemType = ListItemType.AlternatingItem Then
                    dgi.CssClass = "dgAlternate"
                    dgi.Attributes.Add("itemType", "AlternatingItem")
                Else
                    dgi.CssClass = "Dgitem"
                    dgi.Attributes.Add("itemType", "Item")
                End If
            Else
                chkActive.Checked = True
                dgi = CType(chkActive.Parent.Parent, DataGridItem)
                dgi.CssClass = "dgSelected"
            End If
        End If
    End Sub

    Public Function IsFareValuesEqualTo(ByVal target As String, ByVal value As Double, ByVal isNetRate As Boolean) As Boolean

        Dim flag As Boolean = True
        Dim input As TextBox
        Dim grid As DataGrid = Me.FindControl("dg" + If(target.ToLower() = "children", "Child", target))
        Dim wild As Integer = 0

        If grid IsNot Nothing Then
            For Each row As DataGridItem In grid.Items
                If row.ItemType = ListItemType.AlternatingItem OrElse row.ItemType = ListItemType.Item Then
                    input = row.FindControl("txt" + target + "Fare" + If(isNetRate, "NR", ""))
                    If input IsNot Nothing Then
                        Double.TryParse(input.Text, wild)
                        flag = (wild = value)
                    End If
                End If
                If Not flag Then Exit For
            Next
        End If
        Return flag

    End Function

    Private Sub dgTeen_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgTeen.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(0).Text = PortalCulture.GetString("01282")
            e.Item.Cells(1).Text = PortalCulture.GetString("01125") 'Tarifa Neta
            e.Item.Cells(2).Text = PortalCulture.GetString("01124") 'Tarifa Neta
        End If

        Dim lblChild As Label
        lblChild = e.Item.Cells(0).FindControl("lblChildren")

        Dim lblChildValMax As Label
        Dim lblChildValMin As Label
        lblChildValMax = e.Item.Cells(5).FindControl("lblChildValMax")
        lblChildValMin = e.Item.Cells(5).FindControl("lblChildValMin")
        If Not lblChildValMax Is Nothing Then
            lblChildValMax.Style.Add("display", "none")
            lblChildValMax.Text = PortalCulture.GetString("01135")
        End If
        If Not lblChildValMin Is Nothing Then
            lblChildValMin.Style.Add("display", "none")
            lblChildValMin.Text = PortalCulture.GetString("01136")
        End If

        Dim valChdNR As RegularExpressionValidator
        Dim txtChildFareNR As TextBox
        txtChildFareNR = e.Item.Cells(1).FindControl("txtTeenFareNR")
        valChdNR = e.Item.Cells(1).FindControl("valTeenExtraPriceNR")
        If Not valChdNR Is Nothing Then valChdNR.Text = PortalCulture.GetString("00240")
        If Not txtChildFareNR Is Nothing AndAlso lblChild.Text <= 0 Then
            txtChildFareNR.Text = 0
            txtChildFareNR.Enabled = False
            txtChildFareNR.Visible = False
            If lblChild.Text <= 0 Then
                lblChild.Text = ""
            End If
        End If

        Dim valChd As RegularExpressionValidator
        Dim txtChildFare As TextBox
        txtChildFare = e.Item.Cells(2).FindControl("txtTeenFare")
        valChd = e.Item.Cells(2).FindControl("valTeenExtraPrice")
        If Not valChd Is Nothing Then valChd.Text = PortalCulture.GetString("00240")
        If Not txtChildFare Is Nothing AndAlso lblChild.Text <= 0 Then
            txtChildFare.Text = 0
            txtChildFare.Enabled = False
            txtChildFare.Visible = False
            If lblChild.Text <= 0 Then
                lblChild.Text = ""
            End If
        End If

        'Validacion de maximo y minimo porcentaje de gananciaUV
        If Not txtChildFareNR Is Nothing AndAlso Not txtChildFare Is Nothing Then
            txtChildFareNR.Attributes.Add("onChange", "javascript:CheckValContract('" & m_TextBoxPorcMin & "','" & m_TextBoxPorcMax & "','" & txtChildFareNR.ClientID & "','" & txtChildFare.ClientID & "','" & lblChildValMax.ClientID & "','" & lblChildValMin.ClientID & "')")
        End If

        Dim chkActive As CheckBox
        chkActive = e.Item.Cells(4).FindControl("chkActive")
        Dim dgi As DataGridItem
        If Not chkActive Is Nothing Then
            dgi = CType(chkActive.Parent.Parent, DataGridItem)
            If e.Item.Cells(3).Text = "0" Then
                chkActive.Checked = False
                If dgi.ItemType = ListItemType.AlternatingItem Then
                    dgi.CssClass = "dgAlternate"
                    dgi.Attributes.Add("itemType", "AlternatingItem")
                Else
                    dgi.CssClass = "Dgitem"
                    dgi.Attributes.Add("itemType", "Item")
                End If
            Else
                chkActive.Checked = True
                dgi = CType(chkActive.Parent.Parent, DataGridItem)
                dgi.CssClass = "dgSelected"
            End If
        End If
    End Sub

    Private Sub dgAdult_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgAdult.Load

    End Sub

    Public ReadOnly Property MinStorageControlId() As String
        Get
            Return Me.varMinPercent.ClientID
        End Get
    End Property

    Public ReadOnly Property MaxStorageControlId() As String
        Get
            Return Me.varMaxPercent.ClientID
        End Get
    End Property

    Public Sub CalculateUvRevenue()

    End Sub

    Private Sub grid_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgAdult.PreRender, dgChild.PreRender, dgTeen.PreRender

        CType(sender, DataGrid).Columns(2).Visible = Me.IsSupervisor

    End Sub

End Class
