Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade

Imports System.IO
Imports System.Text

Partial Public Class ctrlExceptionFares
    Inherits System.Web.UI.UserControl

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
            Return Nothing
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

    Public Property dsRest() As FaresRestrictionsDataExc
        Get
            Return ViewState("dsRest")
        End Get
        Set(ByVal Value As FaresRestrictionsDataExc)
            ViewState("dsRest") = Value
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
        Dim datRestrictions As FaresRestrictionsDataExc
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
        dtAdults.Columns.Add(FaresRestrictionsDataExc.PKIDRESTRICTION_FIELD)

        dtChildren.Columns.Add("Children")
        dtChildren.Columns.Add("Price")
        dtChildren.Columns.Add(FaresRestrictionsDataExc.PKIDRESTRICTION_FIELD)

        dtTeen.Columns.Add("Teen")
        dtTeen.Columns.Add("Price")
        dtTeen.Columns.Add(FaresRestrictionsDataExc.PKIDRESTRICTION_FIELD)

        datRestrictions = GetFareRestrictions()
        If Not datRestrictions Is Nothing Then
            For Each dr As DataRow In datRestrictions.Tables(FaresRestrictionsDataExc.FARESRESTRICTIONEXC_TABLE).Rows
                If dr(FaresRestrictionsDataExc.ADULTNUMBER_FIELD) <> ad Then
                    ad = dr(FaresRestrictionsDataExc.ADULTNUMBER_FIELD)
                    drnew = dtAdults.NewRow
                    drnew("Adults") = ad
                    drnew("Price") = dr(FaresRestrictionsDataExc.ADULTFARE_FIELD)
                    drnew(FaresRestrictionsDataExc.PKIDRESTRICTION_FIELD) = dr(FaresRestrictionsDataExc.PKIDRESTRICTION_FIELD)
                    dtAdults.Rows.Add(drnew)
                End If
                If dr(FaresRestrictionsDataExc.CHILDNUMBER_FIELD) > 0 AndAlso strch.IndexOf("," & dr(FaresRestrictionsDataExc.CHILDNUMBER_FIELD) & ",") = -1 Then
                    ch = dr(FaresRestrictionsDataExc.CHILDNUMBER_FIELD)
                    strch &= "," & ch & ","
                    drnew = dtChildren.NewRow
                    drnew("Children") = ch
                    drnew("Price") = dr(FaresRestrictionsDataExc.CHILDFARE_FIELD)
                    drnew(FaresRestrictionsDataExc.PKIDRESTRICTION_FIELD) = dr(FaresRestrictionsDataExc.PKIDRESTRICTION_FIELD)
                    dtChildren.Rows.Add(drnew)

                    drnew = dtTeen.NewRow
                    drnew("Teen") = ch
                    drnew("Price") = dr(FaresRestrictionsDataExc.TEENFARE_FIELD)
                    drnew(FaresRestrictionsDataExc.PKIDRESTRICTION_FIELD) = dr(FaresRestrictionsDataExc.PKIDRESTRICTION_FIELD)
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
        Dim datRestrictions As FaresRestrictionsDataExc
        datRestrictions = GetFareRestrictions()
        'If Not datRestrictions Is Nothing Then
        '    dtgRestrictions.DataSource = datRestrictions.Tables(FaresRestrictionsDataExc.FARESRESTRICTIONEXC_TABLE)
        'End If
        'dtgRestrictions.DataBind()
        FillDatas() '// TEST
    End Sub

    Public Function TarifaMinima(ByRef dAdultMin As Double, ByRef dChildtMin As Double, ByRef dJuniorMin As Double) As Boolean
        Dim txtAdultFare As TextBox
        Dim dAdultFare As Double
        Dim dChildFare As Double
        Dim dJuniorFare As Double

        dAdultMin = 999999999
        dChildtMin = 999999999
        dJuniorMin = 999999999

        For i As Integer = 0 To dgAdult.Items.Count - 1
            txtAdultFare = dgAdult.Items(i).Cells(1).FindControl("txtAdultFare")
            Double.TryParse(txtAdultFare.Text, dAdultFare)
            If dAdultFare < dAdultMin Then
                dAdultMin = dAdultFare
            End If
        Next

        For i As Integer = 0 To dgChild.Items.Count - 1
            txtAdultFare = dgChild.Items(i).Cells(1).FindControl("txtChildrenFare")
            Double.TryParse(txtAdultFare.Text, dChildFare)
            If dChildFare < dChildtMin Then
                dChildtMin = dChildFare
            End If
        Next

        For i As Integer = 0 To dgTeen.Items.Count - 1
            txtAdultFare = dgTeen.Items(i).Cells(1).FindControl("txtTeenFare")
            If Not txtAdultFare Is Nothing Then
                Double.TryParse(txtAdultFare.Text, dJuniorFare)
                If dJuniorFare < dJuniorMin Then
                    dJuniorMin = dJuniorFare
                End If
            End If
        Next

        dAdultMin = If(dAdultMin = 999999999, 0, dAdultMin)
        dChildtMin = If(dChildtMin = 999999999, 0, dChildtMin)
        dJuniorMin = If(dJuniorMin = 999999999, 0, dJuniorMin)

    End Function

    'Funcion actualizada al viernes 01 de abril del 2005
    Public Function GetFareRestrictions() As FaresRestrictionsDataExc
        Dim datRestrictions As New FaresRestrictionsDataExc
        Dim datDatagridRestrictions As New FaresRestrictionsDataExc
        Dim datRooms As RoomsHotelData
        Dim datFare As FaresDataExc
        Dim iRoomId As Integer
        Dim iPriceRoom As Decimal
        ' Dim iPriceRoomExc As Decimal
        Dim iPriceExtraChild As Decimal
        Dim iPeople As Integer
        Dim iChild As Integer
        Dim iAdult As Integer
        Dim iMaxPeople As Integer


        If Me.m_iFareId <> 0 Then
            ' get fare data
            With New FaresExcFacade
                datFare = .GetFareById(Me.m_iFareId)
            End With

            ' get fares' restrictions
            With New FaresRestrictionException
                datRestrictions = .GetFareRestrictionByFareId(Me.m_iFareId)
                dsRest = datRestrictions
            End With
        End If
        ' get room id
        If Me.m_iRoomId <> 0 Then
            iRoomId = m_iRoomId
            If Not datFare Is Nothing AndAlso datFare.Tables(FaresDataExc.FARESEXC_TABLE).Rows.Count > 0 Then
                iPriceRoom = CInt(Val(datFare.Tables(FaresDataExc.FARESEXC_TABLE).Rows(0)(FaresData.PRICE_FIELD).ToString))
                'iPriceExtraChild = datFare.Tables(FaresData.FARES_TABLE).Rows(0)(FaresData.EXTRACHILDPRICE_FIELD)
                iPriceExtraChild = CInt(Val(datFare.Tables(FaresDataExc.FARESEXC_TABLE).Rows(0)(FaresDataExc.NINIORATE_FIELD).ToString))
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
                    datDatagridRestrictions.Tables(FaresRestrictionsDataExc.FARESRESTRICTIONEXC_TABLE).ImportRow(newRow)
                Else
                    ' if not, create new fares data
                    newRow = datDatagridRestrictions.Tables(FaresRestrictionsDataExc.FARESRESTRICTIONEXC_TABLE).NewRow()
                    With newRow
                        .Item(FaresRestrictionsDataExc.ADULTFARE_FIELD) = iPriceRoom
                        .Item(FaresRestrictionsDataExc.ADULTNUMBER_FIELD) = idxAdults
                        .Item(FaresRestrictionsDataExc.CHILDFARE_FIELD) = iPriceExtraChild '* idxChild
                        .Item(FaresRestrictionsDataExc.CHILDNUMBER_FIELD) = idxChild
                        .Item(FaresRestrictionsDataExc.IDFARE_FIELD) = Me.m_iFareId
                        .Item(FaresRestrictionsDataExc.PKIDRESTRICTION_FIELD) = 0
                        .Item(FaresRestrictionsDataExc.EXCADULTFARE_FIELD) = 0 'iPriceRoomExc
                        .Item(FaresRestrictionsDataExc.EXCNINIOFARE_FIELD) = 0 'iPriceExtraChildExc * idxChild
                        .Item(FaresRestrictionsDataExc.TEENFARE_FIELD) = 0
                        .Item(FaresRestrictionsDataExc.TEENFARENR_FIELD) = 0
                        .Item(FaresRestrictionsDataExc.EXTEENFARE_FIELD) = 0
                        .Item(FaresRestrictionsDataExc.EXTEENFARENR_FIELD) = 0
                    End With
                    datDatagridRestrictions.Tables(FaresRestrictionsDataExc.FARESRESTRICTIONEXC_TABLE).Rows.Add(newRow)
                End If

            Next
        Next

        datDatagridRestrictions.AcceptChanges()
        Return datDatagridRestrictions

    End Function
    Public Function IsValidData() As Boolean
        Dim txtAdultFare As TextBox
        Dim dPrecio As Double

        For Each item As DataGridItem In dgAdult.Items
            txtAdultFare = item.Cells(1).FindControl("txtAdultFare")
            Double.TryParse(txtAdultFare.Text, dPrecio)
            If dPrecio < 1 Then
                Return False
            End If
        Next
        Return True
    End Function
    Private Function RowInPlan(ByVal datRestrictions As FaresRestrictionsDataExc, ByVal iAdults As Integer, ByVal iChildren As Integer) As DataRow
        Dim row As DataRow
        For Each dr As DataRow In datRestrictions.Tables(FaresRestrictionsDataExc.FARESRESTRICTIONEXC_TABLE).Rows
            If dr(FaresRestrictionsDataExc.ADULTNUMBER_FIELD) = iAdults And _
            dr(FaresRestrictionsDataExc.CHILDNUMBER_FIELD) = iChildren Then
                Return dr
            End If
        Next
        Return Nothing
    End Function

    Private Sub BindDataGridColumns()
        '     With Me.dtgRestrictions
        '         CType(.Columns(Me.RestrictionsDtgCols.RestrictionId), BoundColumn).DataField = _
        'FaresRestrictionsDataExc.PKIDRESTRICTION_FIELD
        '     End With
    End Sub

    'Public Function Save(Optional ByVal DT As DataTable = Nothing) As Boolean
    '    Dim validate As Boolean = True
    '    Dim datRestrictions As FaresRestrictionsDataExc = GetFareRestrictions()
    '    Dim row As DataRow
    '    Dim i As Integer

    '    ' Check if the input controls are valid 
    '    If Not Page.IsValid Then
    '        Return False
    '    End If

    '    For i = 0 To datRestrictions.Tables(FaresRestrictionsDataExc.FARESRESTRICTIONEXC_TABLE).Rows.Count - 1
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


    '        With datRestrictions.Tables(FaresRestrictionsDataExc.FARESRESTRICTIONEXC_TABLE)
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
    '                If .Rows(i)(FaresRestrictionsDataExc.ADULTFARE_FIELD) <> Double.Parse(txtAdultFare.Text) Then
    '                    .Rows(i)(FaresRestrictionsDataExc.ADULTFARE_FIELD) = Double.Parse(txtAdultFare.Text)
    '                End If
    '                ' si se ha modificado el registro entonces lo marcamos como modified
    '                If txtChildFare.Text.Trim.Length = 0 Then
    '                    txtChildFare.Text = 0
    '                End If
    '                If .Rows(i)(FaresRestrictionsDataExc.CHILDFARE_FIELD) <> Double.Parse(txtChildFare.Text) Then
    '                    .Rows(i)(FaresRestrictionsDataExc.CHILDFARE_FIELD) = Double.Parse(txtChildFare.Text)
    '                End If
    '                If dtgRestrictions.Items(i).Cells(RestrictionsDtgCols.RestrictionId).Text = 0 Then
    '                    .Rows(i)(FaresRestrictionsDataExc.CHILDFARE_FIELD) = Double.Parse(txtChildFare.Text)
    '                    .Rows(i)(FaresRestrictionsDataExc.ADULTFARE_FIELD) = Double.Parse(txtAdultFare.Text)
    '                End If
    '                If Not DT Is Nothing Then
    '                    .Rows(i)(FaresRestrictionsDataExc.EXCADULTFARE_FIELD) = DT.Rows(i)(FaresRestrictionsDataExc.EXCADULTFARE_FIELD)
    '                    .Rows(i)(FaresRestrictionsDataExc.EXCNINIOFARE_FIELD) = DT.Rows(i)(FaresRestrictionsDataExc.EXCNINIOFARE_FIELD)
    '                Else
    '                    .Rows(i)(FaresRestrictionsDataExc.EXCADULTFARE_FIELD) = 0
    '                    .Rows(i)(FaresRestrictionsDataExc.EXCNINIOFARE_FIELD) = 0

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

    Function Nota(ByVal rp As String, ByRef sference As String) As String
        Dim msg As String = "Se modificó la tarifa de la habitación por ocupación con el rateplan " & rp
        Dim drhotel As DataRow = CType(Me.Page, PaginaBase).HotelInfo
        Dim idioma As String

        sference = "Cambio de tarifa por ocupación"
        idioma = IIf(drhotel.IsNull("idiomaemail"), "en-US", drhotel.Item("idiomaemail"))
        If idioma = "en-US" Then
            sference = "Update Rate by ocupacion"
            msg = "It changed the room rate by occupation in rateplan " & rp
        End If
        Return msg
    End Function

    Sub Addrateplan(ByVal ds As FaresRestrictionsDataExc, ByVal field As String, ByVal valor As String)
        If Not ds Is Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
            ds.Tables(0).Columns.Add(field)
            ds.Tables(0).Rows(0)(field) = valor
        End If
    End Sub

    ''''''''''''''' para guardar los datos '''''con los nuevosgrid
    Public Function Save(ByVal rp As String, ByVal sroom As String, ByVal scorreoPeticion As String, Optional ByVal dt As DataTable = Nothing) As Boolean
        Dim validate As Boolean = True
        Dim datRestrictions As FaresRestrictionsDataExc = GetFareRestrictions()
        Dim row As DataRow
        Dim i As Integer
        Dim Ad As Integer
        Dim Ch As Integer
        Dim dr As DataRow
        Dim sdato As String
        Dim sdatoDespues As String

        ' Check if the input controls are valid 
        If Not Page.IsValid Then
            Return False
        End If

        'sdato = datRestrictions.GetXml.ToString
        'sdato = Util.Utility.GetXml(datRestrictions.FARESRESTRICTIONEXC_TABLE, "UpdateRateRestriction", datRestrictions)
        For i = 0 To datRestrictions.Tables(FaresRestrictionsDataExc.FARESRESTRICTIONEXC_TABLE).Rows.Count - 1
            Dim chkActivo As CheckBox
            Dim chkActivo2 As CheckBox
            Dim txtAdultFare As TextBox
            Dim txtChildFare As TextBox
            Dim txtTeenFare As TextBox
            Dim cvAd As System.Web.UI.WebControls.CustomValidator
            Dim cvCh As System.Web.UI.WebControls.CustomValidator
            Dim cvTe As System.Web.UI.WebControls.CustomValidator
            Dim dPrecio As Double

            dr = datRestrictions.Tables(FaresRestrictionsDataExc.FARESRESTRICTIONEXC_TABLE).Rows(i)

            Ch = dr(datRestrictions.CHILDNUMBER_FIELD)
            Ad = dr(datRestrictions.ADULTNUMBER_FIELD)


            chkActivo = dgAdult.Items(Ad - 1).Cells(2).FindControl("chkActive")
            txtAdultFare = dgAdult.Items(Ad - 1).Cells(1).FindControl("txtAdultFare")
            cvAd = dgAdult.Items(Ad - 1).Cells(1).FindControl("cvErrAdults")

            txtChildFare = Nothing
            txtTeenFare = Nothing
            cvCh = Nothing
            cvTe = Nothing
            chkActivo2 = Nothing

            If Ch > 0 Then ''''''''''''para los ninios''''''''''''''''''''''''
                txtChildFare = dgChild.Items(Ch - 1).Cells(1).FindControl("txtChildrenFare")
                cvCh = dgChild.Items(Ch - 1).Cells(1).FindControl("cvErrChilds")
                chkActivo2 = dgChild.Items(Ch - 1).Cells(2).FindControl("chkActive")
            End If

            If Ch > 0 Then ''''''''''''para los Adolescentes''''''''''''''''''''''''
                If dgTeen.Items.Count > 0 Then
                    txtTeenFare = dgTeen.Items(Ch - 1).Cells(1).FindControl("txtTeenFare")
                    cvTe = dgTeen.Items(Ch - 1).Cells(1).FindControl("cvErrTeen")
                End If
                'chkActivo2 = dgChild.Items(Ch - 1).Cells(2).FindControl("chkActive")
            End If

            With datRestrictions.Tables(FaresRestrictionsDataExc.FARESRESTRICTIONEXC_TABLE)
                If txtAdultFare.Text.Trim.Length = 0 Then
                    txtChildFare.Text = 0
                End If
                'If chkActivo.Checked = True Then				' si está habilitada la restricción
                ' If True = True Then    'siempre guardamos
                If CType(txtAdultFare.Text, Integer) = 0 AndAlso Not txtChildFare Is Nothing AndAlso CType(txtChildFare.Text, Integer) = 0 Then
                    cvAd.IsValid = False
                    cvCh.IsValid = False
                    'validate = False
                    ' Si los dos valores son cero no guardamos el registro
                End If

                ' si se ha modificado el registro entonces lo marcamos como modified
                If .Rows(i)(FaresRestrictionsDataExc.ADULTFARE_FIELD) <> Double.Parse(txtAdultFare.Text) Then
                    .Rows(i)(FaresRestrictionsDataExc.ADULTFARE_FIELD) = Double.Parse(txtAdultFare.Text)
                End If

                ' si se ha modificado el registro entonces lo marcamos como modified
                If Not txtChildFare Is Nothing Then
                    If txtChildFare.Text.Trim.Length = 0 Then
                        txtChildFare.Text = 0
                    End If
                End If

                If Not txtChildFare Is Nothing Then
                    If .Rows(i)(FaresRestrictionsDataExc.CHILDFARE_FIELD) <> Double.Parse(txtChildFare.Text) Then
                        .Rows(i)(FaresRestrictionsDataExc.CHILDFARE_FIELD) = Double.Parse(txtChildFare.Text)
                    End If
                End If

                If Not txtTeenFare Is Nothing Then
                    If String.IsNullOrEmpty(txtTeenFare.Text) Then
                        txtTeenFare.Text = 0
                    End If
                End If
                If Not txtTeenFare Is Nothing Then
                    If .Rows(i)(FaresRestrictionsDataExc.TEENFARE_FIELD) <> Double.Parse(txtTeenFare.Text) Then
                        .Rows(i)(FaresRestrictionsDataExc.TEENFARE_FIELD) = Double.Parse(txtTeenFare.Text)
                    End If
                End If

                'If dgAdult.Items(Ad - 1).Cells(RestrictionsDtgCols.RestrictionId).Text = 0 Then

                '.Rows(i)(FaresRestrictionsDataExc.ADULTFARE_FIELD) = Double.Parse(txtAdultFare.Text)
                Double.TryParse(txtAdultFare.Text, dPrecio)
                .Rows(i)(FaresRestrictionsDataExc.ADULTFARE_FIELD) = dPrecio
                If Not txtChildFare Is Nothing Then
                    '.Rows(i)(FaresRestrictionsDataExc.CHILDFARE_FIELD) = Double.Parse(txtChildFare.Text)
                    Double.TryParse(txtChildFare.Text, dPrecio)
                    .Rows(i)(FaresRestrictionsDataExc.CHILDFARE_FIELD) = dPrecio
                Else
                    .Rows(i)(FaresRestrictionsDataExc.CHILDFARE_FIELD) = 0
                End If
                If Not txtTeenFare Is Nothing Then
                    '.Rows(i)(FaresRestrictionsDataExc.TEENFARE_FIELD) = Double.Parse(txtTeenFare.Text)
                    Double.TryParse(txtTeenFare.Text, dPrecio)
                    .Rows(i)(FaresRestrictionsDataExc.TEENFARE_FIELD) = dPrecio

                Else
                    .Rows(i)(FaresRestrictionsDataExc.TEENFARE_FIELD) = 0
                End If

                'End If
                If Not dt Is Nothing Then
                    .Rows(i)(FaresRestrictionsDataExc.EXCADULTFARE_FIELD) = dt.Rows(i)(FaresRestrictionsDataExc.EXCADULTFARE_FIELD)
                    .Rows(i)(FaresRestrictionsDataExc.EXCNINIOFARE_FIELD) = dt.Rows(i)(FaresRestrictionsDataExc.EXCNINIOFARE_FIELD)
                    .Rows(i)(FaresRestrictionsDataExc.EXTEENFARE_FIELD) = dt.Rows(i)(FaresRestrictionsDataExc.EXTEENFARE_FIELD)
                Else
                    .Rows(i)(FaresRestrictionsDataExc.EXCADULTFARE_FIELD) = 0
                    .Rows(i)(FaresRestrictionsDataExc.EXCNINIOFARE_FIELD) = 0
                    .Rows(i)(FaresRestrictionsDataExc.EXTEENFARE_FIELD) = 0
                End If
                'Else    ' si no está habilitado el registro pero si existía antes
                '    If Me.dtgRestrictions.Items(i).Cells(RestrictionsDtgCols.RestrictionId).Text <> 0 Then       'Es delete
                '        .Rows(i).Delete()
                '    End If
                'End If
            End With
        Next

        Dim sdatoCorreo As String = ""
        Dim hr As Boolean
        If validate Then
            With New FaresRestrictionException

                Save = .UpdateFaresRestrictions(datRestrictions)
                If Save Then
                    Dim sreference As String = ""
                    Addrateplan(dsRest, "Descr_rateplan", String.Format("{0}/ {1}", rp, sroom))
                    sdato = Util.Utility.GetXml(datRestrictions.FARESRESTRICTIONEXC_TABLE, "UpdateRateRestriction", dsRest)

                    Addrateplan(datRestrictions, "Descr_rateplan", String.Format("{0}/ {1}", rp, sroom))
                    sdatoDespues = Util.Utility.GetXml(datRestrictions.FARESRESTRICTIONEXC_TABLE, "UpdateRateRestriction", datRestrictions)
                    'sdatoCorreo = CreateRPDHtml(dsRest, datRestrictions, hr)
                    'If hr Then
                    '    sdatoCorreo += "<br/><br/>"
                    'Else
                    '    'CType(Me.Page, PaginaBase).guardalog("/Pages/FaresCataloguePromo.aspx", PaginaBase.acciones.Modificar, "Se modificó la tarifa promocion de la habitación por ocupación con el rateplan " & rp, "Update Rate by ocupacion", sdato, sdatoDespues)
                    '    sdatoCorreo = ""
                    'End If
                    Dim snota As String = Nota(rp, sreference)

                    'CType(Me.Page, PaginaBase).guardalog("/Pages/FaresCatalogue.aspx", PaginaBase.acciones.Modificar, "Se modificó la tarifa de la habitación por ocupación con el rateplan " & rp, "Update Rate by ocupacion", sdato, sdatoDespues, sdatoCorreo)
                    sdatoCorreo = (New Util.Utility).GeneraCorreoXslt(sdato, sdatoDespues)
                    scorreoPeticion += sdatoCorreo
                    CType(Me.Page, PaginaBase).guardalog("/Pages/FaresCataloguePromo.aspx", PaginaBase.acciones.Modificar, snota, sreference, sdato, sdatoDespues, scorreoPeticion)


                End If
            End With
        Else
            Me.cvErrMsg.IsValid = False
            Save = validate
        End If
    End Function

    Public Function CreateRPDHtml(ByVal dsBefore As FaresRestrictionsDataExc, ByVal dsFares As FaresRestrictionsDataExc, ByRef hr As Boolean) As String
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
                If drr("TarifaAdulto") <> drd("TarifaAdulto") Or drr("TarifaNinio") <> drd("TarifaNinio") Then
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
                End If
            End If
        Next

        menu.RenderControl(writer)
        shtml = sw.ToString()

        Return shtml
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

    Public Function IsFareValuesEqualTo(ByVal target As String, ByVal value As Double, ByVal isNetRate As Boolean) As Boolean

        Dim flag As Boolean = True
        Dim input As TextBox
        Dim grid As DataGrid = Me.FindControl("dg" + If(target.ToLower() = "children", "Child", target))
        Dim dPrecio As Double

        If grid IsNot Nothing Then
            For Each row As DataGridItem In grid.Items
                If row.ItemType = ListItemType.AlternatingItem OrElse row.ItemType = ListItemType.Item Then
                    input = row.FindControl("txt" + target + "Fare" + If(isNetRate, "NR", ""))
                    If input IsNot Nothing Then
                        Double.TryParse(input.Text, dPrecio)
                        'flag = Convert.ToDouble(input.Text) = value
                        flag = dPrecio = value
                    End If
                End If
                If Not flag Then Exit For
            Next
        End If
        Return flag

    End Function

    Private Sub dgAdult_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgAdult.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(0).Text = PortalCulture.GetString("00060")
            'e.Item.Cells(RestrictionsDtgCols.ChildNumber).Text = PortalCulture.GetString("00061")
            e.Item.Cells(1).Text = PortalCulture.GetString("00134")
            'e.Item.Cells(RestrictionsDtgCols.ChildFare).Text = PortalCulture.GetString("00135")
            'e.Item.Cells(RestrictionsDtgCols.TotalFare).Text = PortalCulture.GetString("00136")
        End If

        Dim txtAdultFare As TextBox
        'Dim txtChildFare As TextBox
        Dim lblAdult As Label
        'Dim lblChild As Label
        Dim valAdt As RegularExpressionValidator
        'Dim valChd As RegularExpressionValidator

        txtAdultFare = e.Item.Cells(1).FindControl("txtAdultFare")
        'txtChildFare = e.Item.Cells(RestrictionsDtgCols.AdultFare).FindControl("txtChildrenFare")

        lblAdult = e.Item.Cells(0).FindControl("lblAdults")
        'lblChild = e.Item.Cells(RestrictionsDtgCols.ChildNumber).FindControl("lblChildren")



        valAdt = e.Item.Cells(1).FindControl("valAdultExtraPrice")
        'valChd = e.Item.Cells(RestrictionsDtgCols.ChildFare).FindControl("valChildrenExtraPrice")

        If Not valAdt Is Nothing Then valAdt.Text = PortalCulture.GetString("00116")
        'If Not valChd Is Nothing Then valChd.Text = PortalCulture.GetString("00240")

        If Not txtAdultFare Is Nothing AndAlso lblAdult.Text <= 0 Then
            txtAdultFare.Text = 0
            txtAdultFare.Enabled = False
            txtAdultFare.Visible = False

        End If
        'If Not txtChildFare Is Nothing AndAlso lblChild.Text <= 0 Then
        '    txtChildFare.Text = 0
        '    txtChildFare.Enabled = False
        '    txtChildFare.Visible = False
        '    If lblChild.Text <= 0 Then
        '        lblChild.Text = ""
        '    End If
        'End If

        'Mostramos la tarifa total 
        'If Not txtAdultFare Is Nothing Then 'And Not txtChildFare Is Nothing Then
        'Dim lblTotalFare As Label
        'lblTotalFare = e.Item.Cells(RestrictionsDtgCols.TotalFare).FindControl("lblTotal")
        'If Not lblTotalFare Is Nothing Then
        'txtAdultFare.Attributes.Add("onChange", "sP('" & txtAdultFare.ClientID & "','" & txtChildFare.ClientID & "','" & lblTotalFare.ClientID & "');")
        'txtChildFare.Attributes.Add("onChange", "sP('" & txtAdultFare.ClientID & "','" & txtChildFare.ClientID & "','" & lblTotalFare.ClientID & "');")
        'lblTotalFare.Text = CDec(CDec(txtAdultFare.Text) + CDec(txtChildFare.Text)).ToString("C")
        'End If
        'End If'
        Dim chkActive As CheckBox
        chkActive = e.Item.Cells(3).FindControl("chkActive")
        Dim dgi As DataGridItem
        If Not chkActive Is Nothing Then
            dgi = CType(chkActive.Parent.Parent, DataGridItem)
            If e.Item.Cells(2).Text = "0" Then
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
            'e.Item.Cells(RestrictionsDtgCols.AdultNumber).Text = PortalCulture.GetString("00060")
            e.Item.Cells(0).Text = PortalCulture.GetString("00061")
            'e.Item.Cells(RestrictionsDtgCols.AdultFare).Text = PortalCulture.GetString("00134")
            e.Item.Cells(1).Text = PortalCulture.GetString("00135")
            'e.Item.Cells(RestrictionsDtgCols.TotalFare).Text = PortalCulture.GetString("00136")
        End If
        'Dim txtAdultFare As TextBox
        Dim txtChildFare As TextBox
        'Dim lblAdult As Label
        Dim lblChild As Label
        'Dim valAdt As RegularExpressionValidator
        Dim valChd As RegularExpressionValidator
        'txtAdultFare = e.Item.Cells(RestrictionsDtgCols.AdultFare).FindControl("txtAdultFare")
        txtChildFare = e.Item.Cells(1).FindControl("txtChildrenFare")
        'lblAdult = e.Item.Cells(RestrictionsDtgCols.AdultNumber).FindControl("lblAdults")
        lblChild = e.Item.Cells(0).FindControl("lblChildren")
        'valAdt = e.Item.Cells(RestrictionsDtgCols.AdultFare).FindControl("valAdultExtraPrice")
        valChd = e.Item.Cells(1).FindControl("valChildrenExtraPrice")
        'If Not valAdt Is Nothing Then valAdt.Text = PortalCulture.GetString("00240")
        If Not valChd Is Nothing Then valChd.Text = PortalCulture.GetString("00116")
        'If Not txtAdultFare Is Nothing AndAlso lblAdult.Text <= 0 Then
        '    txtAdultFare.Text = 0
        '    txtAdultFare.Enabled = False
        '    txtAdultFare.Visible = False
        'End If
        If Not txtChildFare Is Nothing AndAlso lblChild.Text <= 0 Then
            txtChildFare.Text = 0
            txtChildFare.Enabled = False
            txtChildFare.Visible = False
            If lblChild.Text <= 0 Then
                lblChild.Text = ""
            End If
        End If
        'Mostramos la tarifa total 
        'If Not txtAdultFare Is Nothing And Not txtChildFare Is Nothing Then
        '    Dim lblTotalFare As Label
        '    lblTotalFare = e.Item.Cells(RestrictionsDtgCols.TotalFare).FindControl("lblTotal")
        '    If Not lblTotalFare Is Nothing Then
        '        txtAdultFare.Attributes.Add("onChange", "sP('" & txtAdultFare.ClientID & "','" & txtChildFare.ClientID & "','" & lblTotalFare.ClientID & "');")
        '        txtChildFare.Attributes.Add("onChange", "sP('" & txtAdultFare.ClientID & "','" & txtChildFare.ClientID & "','" & lblTotalFare.ClientID & "');")
        '        lblTotalFare.Text = CDec(CDec(txtAdultFare.Text) + CDec(txtChildFare.Text)).ToString("C")
        '    End If
        'End If
        Dim chkActive As CheckBox
        chkActive = e.Item.Cells(2).FindControl("chkActive")
        Dim dgi As DataGridItem
        If Not chkActive Is Nothing Then
            dgi = CType(chkActive.Parent.Parent, DataGridItem)
            If e.Item.Cells(2).Text = "0" Then
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

    Private Sub dgTeen_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgTeen.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(0).Text = PortalCulture.GetString("01282")
            e.Item.Cells(1).Text = PortalCulture.GetString("01283")
        End If

        Dim txtTeenFare As TextBox
        Dim lblTeen As Label
        Dim valTeen As RegularExpressionValidator

        txtTeenFare = e.Item.Cells(1).FindControl("txtTeenFare")
        lblTeen = e.Item.Cells(0).FindControl("lblTeen")
        valTeen = e.Item.Cells(1).FindControl("valTeenExtraPrice")
        If Not valTeen Is Nothing Then valTeen.Text = PortalCulture.GetString("00116")

        If Not txtTeenFare Is Nothing AndAlso lblTeen.Text <= 0 Then
            txtTeenFare.Text = 0
            txtTeenFare.Enabled = False
            txtTeenFare.Visible = False
            If lblTeen.Text <= 0 Then
                lblTeen.Text = ""
            End If
        End If

        Dim chkActive As CheckBox
        chkActive = e.Item.Cells(2).FindControl("chkActive")
        Dim dgi As DataGridItem
        If Not chkActive Is Nothing Then
            dgi = CType(chkActive.Parent.Parent, DataGridItem)
            If e.Item.Cells(2).Text = "0" Then
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

End Class